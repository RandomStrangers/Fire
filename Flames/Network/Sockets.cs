﻿/*
Copyright 2010 MCSharp team (Modified for use with MCZall/MCLawl/MCForge)
Dual-licensed under the Educational Community License, Version 2.0 and
the GNU General Public License, Version 3 (the "Licenses"); you may
not use this file except in compliance with the Licenses. You may
obtain a copy of the Licenses at
https://opensource.org/license/ecl-2-0/
https://www.gnu.org/licenses/gpl-3.0.html
Unless required by applicable law or agreed to in writing,
software distributed under the Licenses are distributed on an "AS IS"
BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
or implied. See the Licenses for the specific language governing
permissions and limitations under the Licenses.
 */
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Flames.Network 
{    
    public enum SendFlags 
    {
        None        = 0x00,
        Synchronous = 0x01,
        LowPriority = 0x02,
    }
    public delegate INetProtocol ProtocolConstructor(INetSocket socket);
    
    /// <summary> Abstracts sending to/receiving from a network socket </summary>
    public abstract class INetSocket 
    {
        public INetProtocol protocol;        
        /// <summary> Whether the socket has been closed/disconnected </summary>
        public bool Disconnected;
        public byte[] leftData;
        public int leftLen;
        
        /// <summary> Gets the IP address of the remote end (i.e. client) of the socket </summary>
        public abstract IPAddress IP { get; }      
        /// <summary> Sets whether this socket operates in low-latency mode (e.g. for TCP, disabes nagle's algorithm) </summary>
        public abstract bool LowLatency { set; }
        
        /// <summary> Initialises state to begin asynchronously sending and receiving data </summary>
        public abstract void Init();        
        /// <summary> Sends a block of data </summary>
        public abstract void Send(byte[] buffer, SendFlags flags);      
        /// <summary> Closes this network socket </summary>
        public abstract void Close();

        public void HandleReceived(byte[] data, int len) {
            // Identify the protocol user is connecting with
            // It could be ClassiCube, Minecraft Modern, WebSocket, etc
            if (protocol == null) {
                IdentifyProtocol(data[0]);
                if (protocol == null) return;
            }
            byte[] src;
            
            // Is there any data leftover from last Receive call?
            if (leftLen == 0) {
                src     = data;
                leftLen = len;
            } else {
                // Yes, combine it with new data
                int totalLen = leftLen + len;
                if (totalLen > leftData.Length) Array.Resize(ref leftData, totalLen);
                
                Buffer.BlockCopy(data, 0, leftData, leftLen, len);
                src     = leftData;
                leftLen = totalLen;
            }
            
            // Packets may not always be fully received
            // Hence may need to retain partial packet data after processing
            int processedLen = protocol.ProcessReceived(src, leftLen);
            leftLen -= processedLen;
            if (leftLen == 0) return;
            
            if (leftData == null || leftLen > leftData.Length) leftData = new byte[leftLen];
            // move remaining partial packet data back to start of leftover/remaining buffer
            for (int i = 0; i < leftLen; i++) {
                leftData[i] = src[processedLen + i];
            }
        }


        public static VolatileArray<INetSocket> pending = new VolatileArray<INetSocket>();
        public static ProtocolConstructor[] Protocols     = new ProtocolConstructor[256];

        public void IdentifyProtocol(byte opcode) {
            ProtocolConstructor cons   = Protocols[opcode];
            if (cons != null) protocol = cons(this);
            if (protocol != null) return;
            
            Logger.Log(LogType.UserActivity, "Disconnected {0} (unknown opcode {1})", IP, opcode);
            Close();
        }
        
        static INetSocket() {
            Protocols[Opcode.Handshake] = ConstructClassic;
            Protocols['G']              = ConstructWebsocket;
        }

        public static INetProtocol ConstructClassic(INetSocket socket) {
            return new ClassicProtocol(socket);
        }

        public static INetProtocol ConstructWebsocket(INetSocket socket) {
            if (!Server.Config.WebClient) return null;
            return new WebSocket(socket);
        }
    }
    
    /// <summary> Interface for a class that inteprets the data received from an INetSocket </summary>
    public interface INetProtocol 
    {
        int ProcessReceived(byte[] buffer, int length);
        void Disconnect();
    }
    
    /// <summary> Abstracts sending to/receiving from a TCP socket </summary>
    public sealed class TcpSocket : INetSocket 
    {
        public readonly Socket socket;
        public byte[] recvBuffer = new byte[256];
        public readonly SocketAsyncEventArgs recvArgs = new SocketAsyncEventArgs();

        public byte[] sendBuffer = new byte[4096];
        public readonly object sendLock = new object();
        public readonly Queue<byte[]> sendQueue = new Queue<byte[]>(64);
        public volatile bool sendInProgress;
        public readonly SocketAsyncEventArgs sendArgs = new SocketAsyncEventArgs();
        
        public TcpSocket(Socket s) {
            socket = s;
            recvArgs.UserToken = this;
            recvArgs.SetBuffer(recvBuffer, 0, recvBuffer.Length);
            sendArgs.UserToken = this;
            sendArgs.SetBuffer(sendBuffer, 0, sendBuffer.Length);
        }
        
        public override void Init() {
            recvArgs.Completed += recvCallback;
            sendArgs.Completed += sendCallback;
            ReceiveNextAsync();
        }
        
        public override IPAddress IP {
            get { return SocketUtil.GetIP(socket); }
        }
        public override bool LowLatency { set { socket.NoDelay = value; } }


        public static EventHandler<SocketAsyncEventArgs> recvCallback = RecvCallback;
        public void ReceiveNextAsync() {
            // ReceiveAsync returns false if completed sync
            if (!socket.ReceiveAsync(recvArgs)) RecvCallback(null, recvArgs);
        }

        public static void RecvCallback(object sender, SocketAsyncEventArgs e) {
            TcpSocket s = (TcpSocket)e.UserToken;
            if (s.Disconnected) return;
            
            try {
                // If received 0, means socket was closed
                int recvLen = e.BytesTransferred;
                if (recvLen == 0) { s.Disconnect(); return; }
                
                s.HandleReceived(s.recvBuffer, recvLen);
                if (!s.Disconnected) s.ReceiveNextAsync();
            } catch (SocketException) {
                s.Disconnect();
            } catch (ObjectDisposedException) {
                // Socket was closed by another thread, mark as disconnected
            } catch (Exception ex) {
                Logger.LogError(ex);
                s.Disconnect();
            }
        }


        public static EventHandler<SocketAsyncEventArgs> sendCallback = SendCallback;
        public override void Send(byte[] buffer, SendFlags flags) {
            if (Disconnected || !socket.Connected) return;

            // TODO: Low priority sending support
            try {
                if ((flags & SendFlags.Synchronous) != 0) {
                    socket.Send(buffer, 0, buffer.Length, SocketFlags.None);
                    return;
                }
                
                lock (sendLock) {
                    if (sendInProgress) {
                        sendQueue.Enqueue(buffer);
                    } else {
                        sendInProgress = TrySendAsync(buffer);
                    }
                }
            } catch (SocketException) {
                Disconnect();
            } catch (ObjectDisposedException) {
                // Socket was already closed by another thread
            }
        }

        public bool TrySendAsync(byte[] buffer) {
            // BlockCopy has some overhead, not worth it for very small data
            if (buffer.Length <= 16) {
                for (int i = 0; i < buffer.Length; i++) {
                    sendBuffer[i] = buffer[i];
                }
            } else {
                Buffer.BlockCopy(buffer, 0, sendBuffer, 0, buffer.Length);
            }

            sendArgs.SetBuffer(0, buffer.Length);
            // SendAsync returns false if completed sync
            return socket.SendAsync(sendArgs);
        }

        public static void SendCallback(object sender, SocketAsyncEventArgs e) {
            TcpSocket s = (TcpSocket)e.UserToken;
            try {
                lock (s.sendLock) {
                    // check if last packet was only partially sent
                    for (;;) {
                        int sent  = e.BytesTransferred;
                        int count = e.Count;
                        if (sent >= count || sent <= 0) break;
                        
                        // last packet was only partially sent - resend rest of packet
                        s.sendArgs.SetBuffer(e.Offset + sent, e.Count - sent);
                        s.sendInProgress = s.socket.SendAsync(s.sendArgs);
                        if (s.sendInProgress) return;
                    }
                    
                    s.sendInProgress = false;
                    while (s.sendQueue.Count > 0) {
                        // DoSendAsync returns false if SendAsync completed sync
                        // If that happens, SendCallback isn't called so we need to send data here instead
                        s.sendInProgress = s.TrySendAsync(s.sendQueue.Dequeue());
                        
                        if (s.sendInProgress) return;
                        if (s.Disconnected) s.sendQueue.Clear();
                    }
                }
            } catch (SocketException) {
                s.Disconnect();
            } catch (ObjectDisposedException) {
                // Socket was already closed by another thread
            } catch (Exception ex) {
                Logger.LogError(ex);
            }
        }

        // Close while also notifying higher level (i.e. show 'X disconnected' in chat)
        public void Disconnect() {
            if (protocol != null) protocol.Disconnect();
            Close();
        }
        
        public override void Close() {
            Disconnected = true;
            pending.Remove(this);
            
            // swallow errors as connection is being closed anyways
            try { socket.Shutdown(SocketShutdown.Both); } catch { }
            try { socket.Close(); } catch { }
            
            lock (sendLock) { sendQueue.Clear(); }
            try { recvArgs.Dispose(); } catch { }
            try { sendArgs.Dispose(); } catch { }
        }
    }
    
    /// <summary> Abstracts a WebSocket on top of a socket </summary>
    public sealed class WebSocket : ServerWebSocket 
    {
        public readonly INetSocket s;
        // websocket connection may be a proxied connection
        public IPAddress clientIP;      
        
        public WebSocket(INetSocket socket) { s  = socket; }
        
        // Init taken care by underlying socket
        public override void Init() { }
        public override IPAddress IP { get { return clientIP ?? s.IP; } }
        public override bool LowLatency { set { s.LowLatency = value; } }

        public override void SendRaw(byte[] data, SendFlags flags) {
            s.Send(data, flags);
        } 
        public override void Send(byte[] buffer, SendFlags flags) {
            s.Send(WrapData(buffer), flags);
        }

        public override void HandleData(byte[] data, int len) {
            HandleReceived(data, len);
        }

        public override void OnDisconnected(int reason) {
            if (protocol != null) protocol.Disconnect();
            s.Close();
        }
        
        public override void Close() { s.Close(); }


        // Websocket proxying support
        public override void OnGotHeader(string name, string value) {
            base.OnGotHeader(name, value);

            if (name == "X-Real-IP" && Server.Config.AllowIPForwarding && IsTrustedForwarderIP()) {
                Logger.Log(LogType.SystemActivity, "{0} is forwarding a connection from {1}", IP, value);
                IPAddress.TryParse(value, out clientIP);
            }
        }

        // by default the following IPs are trusted for proxying/forwarding connections
        //  1) loopback (assumed to be a reverse proxy running on the same machine as the server)
        //  2) classicube.net's websocket proxy IP (used as a fallback for https only connections)
        public static IPAddress ccnetIP = new IPAddress(0xFA05DF22); // 34.223.5.250
        bool IsTrustedForwarderIP() {
            IPAddress ip = IP;
            return IPAddress.IsLoopback(ip) || ip.Equals(ccnetIP);
        }
    }
}