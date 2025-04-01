//reference System.dll
//reference System.Core.dll
//reference System.Xml.dll
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using Flames.Authentication;
using Flames.Config;
using Flames.Events.ServerEvents;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Text;
using System.Xml;
namespace Flames.Network.HeartBeat
{
    /// <summary> Abstracts listening on network socket </summary>
    public abstract class INetListen
    {
        /// <summary> The IP address this network socket is listening on </summary>
        public IPAddress IP;
        /// <summary> The port this network socket is listening on </summary>
        public int Port;
        /// <summary> Whether connections are currently being accepted </summary>
        public bool Listening;

        /// <summary> Begins listening for connections on the given IP and port </summary>
        /// <remarks> Client connections are asynchronously accepted </remarks>
        public abstract void Listen(IPAddress ip, int port);
        public abstract void Listen2(IPAddress ip, int port);
        /// <summary> Closes this network listener </summary>
        public abstract void Close();
    }
    public class UPnP
    {
        public static TimeSpan Timeout = TimeSpan.FromSeconds(3);
        public const string TCP_PROTOCOL = "TCP";

        public Action<string> Log;

        public const string SEARCH_REQUEST =
            "M-SEARCH * HTTP/1.1\r\n" +
            "HOST: 239.255.255.250:1900\r\n" +
            "ST:upnp:rootdevice\r\n" +
            "MAN:\"ssdp:discover\"\r\n" +
            "MX:3\r\n" +
            "\r\n";

        public string _serviceUrl;
        public List<string> visitedLocations = new List<string>();


        public bool Discover()
        {
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            s.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);

            byte[] data = Encoding.ASCII.GetBytes(SEARCH_REQUEST);
            IPEndPoint ep = new IPEndPoint(IPAddress.Broadcast, 1900);
            byte[] buffer = new byte[0x1000];

            s.ReceiveTimeout = 3000;
            visitedLocations.Clear();
            Log("Searching for UPnP supporting routers..");
            DateTime end = DateTime.UtcNow.Add(Timeout);

            try
            {
                while (DateTime.UtcNow < end)
                {
                    s.SendTo(data, ep);
                    s.SendTo(data, ep);
                    s.SendTo(data, ep);

                    int length = -1;
                    do
                    {
                        length = s.Receive(buffer);
                        string resp = Encoding.ASCII.GetString(buffer, 0, length);

                        if (resp.Contains("upnp:rootdevice"))
                        {
                            string location = resp.Substring(resp.ToLower().IndexOf("location:") + 9);
                            location = location.Substring(0, location.IndexOf("\r")).Trim();

                            if (!visitedLocations.Contains(location))
                            {
                                visitedLocations.Add(location);
                                //Log("Found UPnP device: " + location);
                                Log("  Found: " + location);

                                _serviceUrl = GetServiceUrl(location);
                                if (string.IsNullOrEmpty(_serviceUrl)) continue;

                                Log("  Service URL: " + _serviceUrl);
                                return true;
                            }
                        }
                    }
                    while (length > 0);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Error discovering UPnP devices", ex);
            }

            Log("No UPnP supporting routers found");
            return false;
        }

        public void ForwardPort(int port, string protocol, string description)
        {
            if (string.IsNullOrEmpty(_serviceUrl))
                throw new InvalidOperationException("No UPnP service available or Discover() has not been called");

            string xdoc = SOAPRequest(_serviceUrl, "AddPortMapping",
                "<u:AddPortMapping xmlns:u=\"urn:schemas-upnp-org:service:WANIPConnection:1\">" +
                "<NewRemoteHost></NewRemoteHost>" +
                "<NewExternalPort>" + port + "</NewExternalPort>" +
                "<NewProtocol>" + protocol + "</NewProtocol>" +
                "<NewInternalPort>" + port + "</NewInternalPort>" +
                "<NewInternalClient>" + GetLocalIP() + "</NewInternalClient>" +
                "<NewEnabled>1</NewEnabled>" +
                "<NewPortMappingDescription>" + description + "</NewPortMappingDescription>" +
                "<NewLeaseDuration>0</NewLeaseDuration>" +
                "</u:AddPortMapping>");
            Log("Forwarded port " + port);
        }

        public void DeleteForwardingRule(int port, string protocol)
        {
            if (string.IsNullOrEmpty(_serviceUrl))
                throw new InvalidOperationException("No UPnP service available or Discover() has not been called");

            string xdoc = SOAPRequest(_serviceUrl, "DeletePortMapping",
                "<u:DeletePortMapping xmlns:u=\"urn:schemas-upnp-org:service:WANIPConnection:1\">" +
                "<NewRemoteHost></NewRemoteHost>" +
                "<NewExternalPort>" + port + "</NewExternalPort>" +
                "<NewProtocol>" + protocol + "</NewProtocol>" +
                "</u:DeletePortMapping>");
            Log("Un-forwarded port " + port);
        }


        public static string GetServiceUrl(string location)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                WebRequest request = WebRequest.Create(location);
                doc.Load(request.GetResponse().GetResponseStream());

                XmlNamespaceManager nsMgr = new XmlNamespaceManager(doc.NameTable);
                nsMgr.AddNamespace("tns", "urn:schemas-upnp-org:device-1-0");
                XmlNode node;

                node = doc.SelectSingleNode("//tns:device/tns:deviceType/text()", nsMgr);
                if (node == null || !node.Value.Contains("InternetGatewayDevice"))
                    return null;

                node = doc.SelectSingleNode("//tns:service[tns:serviceType=\"urn:schemas-upnp-org:service:WANIPConnection:1\"]/tns:controlURL/text()", nsMgr);
                if (node != null) return CombineUrls(location, node.Value);

                // Try again with version 2
                node = doc.SelectSingleNode("//tns:service[tns:serviceType=\"urn:schemas-upnp-org:service:WANIPConnection:2\"]/tns:controlURL/text()", nsMgr);
                if (node != null) return CombineUrls(location, node.Value);

            }
            catch (Exception ex)
            {
                HttpUtil.DisposeErrorResponse(ex);
                Logger.LogError("Error getting UPnP device service URL", ex);
            }
            return null;
        }

        public static string CombineUrls(string location, string p)
        {
            int n = location.IndexOf("://");
            n = location.IndexOf('/', n + 3);
            return location.Substring(0, n) + p;
        }

        public static string GetLocalIP()
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return "?";
        }

        /// <summary> Performs a XML SOAP request </summary>
        /// <returns> XML response from the service </returns>
        public static string SOAPRequest(string url, string function, string soap)
        {
            string req =
                "<?xml version=\"1.0\"?>" +
                "<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\" s:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">" +
                "<s:Body>" + soap + "</s:Body>" +
                "</s:Envelope>";

            WebRequest r = WebRequest.Create(url);
            r.Method = "POST";
            r.Headers.Add("SOAPACTION", "\"urn:schemas-upnp-org:service:WANIPConnection:1#" + function + "\"");
            r.ContentType = "text/xml; charset=\"utf-8\"";

            byte[] data = Encoding.UTF8.GetBytes(req);
            HttpUtil.SetRequestData(r, data);

            WebResponse res = r.GetResponse();
            return HttpUtil.GetResponseText(res);
        }
    }
    /// <summary> Abstracts listening on a TCP socket </summary>
    public class TcpListen : INetListen
    {
        public Socket socket;

        public void DisableIPV6OnlyListener()
        {
            if (socket.AddressFamily != AddressFamily.InterNetworkV6) return;
            // TODO: Make windows only?

            // NOTE: SocketOptionName.IPv6Only is not defined in Mono, but apparently
            //  macOS and Linux default to dual stack by default already
            const SocketOptionName ipv6Only = (SocketOptionName)27;
            try
            {
                socket.SetSocketOption(SocketOptionLevel.IPv6, ipv6Only, false);
            }
            catch(Exception ex)
            {
                Logger.LogError("Failed to disable IPv6 only listener setting", ex);
            }
        }

        public void EnableAddressReuse()
        {
            // This fixes when on certain environments, if the server is restarted while there are still some
            // sockets in the TIME_WAIT state, the listener in the new server process will fail with EADDRINUSE
            //   https://stackoverflow.com/questions/3229860/what-is-the-meaning-of-so-reuseaddr-setsockopt-option-linux
            //   https://superuser.com/questions/173535/what-are-close-wait-and-time-wait-states
            //   https://stackoverflow.com/questions/14388706/how-do-so-reuseaddr-and-so-reuseport-differ
            // SO_REUSEADDR behaves differently on Windows though, so don't enable it there
            //  (note that this code is required for WINE, therefore just check if running in mono)
            //  (see WS_SO_REUSEADDR case handling in WS_setsockopt in WINE/dlls/ws2_32/socket.c)
            if (!Server.RunningOnMono()) return;

            try
            {
                socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);
            }
            catch
            {
                // not really a critical issue if this fails to work
            }
        }
        public static bool Success;

        public override void Listen2(IPAddress ip, int port)
        {
            port += 1;
            Listen(ip, port);
        }
        public UPnP upnp;

        public override void Listen(IPAddress ip, int port)
        {
            if (IP == ip && Port == port) return;
            Close();
            upnp.ForwardPort(port, UPnP.TCP_PROTOCOL, Server.SoftwareName + "Server");
            IP = ip;
            Port = port;
            ClassiCubePlugin.CCConfig.Port = Port;
            try
            {
                socket = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                DisableIPV6OnlyListener();
                EnableAddressReuse();

                socket.Bind(new IPEndPoint(ip, port));
                socket.Listen((int)SocketOptionName.MaxConnections);
                AcceptNextAsync();
            }
            catch (Exception ex)
            {
                Success = false;
                Listen2(ip, port);
                return;
                Logger.LogError(ex);
                Logger.Log(LogType.Warning, "Failed to start listening on port {0} ({1})", port, ex.Message);

                string msg = string.Format("Failed to start listening. Is another server or instance of {0} already running on port {1}?",
                                           Server.SoftwareName, port);
                Server.UpdateUrl(msg);
                Success = false;
                socket = null;
                return;
            }
            Success = true;
            Listening = true;
            Logger.Log(LogType.SystemActivity, "Started listening on port {0}... ", port);
        }
        public void AcceptNextAsync2()
        {
            AcceptNextAsync();
        }
        public void AcceptNextAsync()
        {
            // retry, because if we don't call BeginAccept, no one can connect anymore
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    socket.BeginAccept(acceptCallback, this);
                    return;
                }
                catch (Exception ex)
                {
                    AcceptNextAsync2();
                    /*if (!(ex is SocketException)
                        && !(ex is ObjectDisposedException)
                        && !(ex is ArgumentException))
                    {
                        Logger.LogError(ex);
                    }*/
                }
            }
        }

        public static AsyncCallback acceptCallback = new AsyncCallback(AcceptCallback);
        public static void AcceptCallback2(IAsyncResult result)
        {
            AcceptCallback(result);
        }
        public static void AcceptCallback(IAsyncResult result)
        {
            if (Server.shuttingDown) return;
            TcpListen listen = (TcpListen)result.AsyncState;
            INetSocket s = null;

            try
            {
                Socket raw = listen.socket.EndAccept(result);
                bool cancel = false, announce = true;

                OnConnectionReceivedEvent.Call(raw, ref cancel, ref announce);
                if (cancel)
                {
                    // intentionally non-clean connection close
                    try
                    {
                        raw.Close();
                    }
                    catch(Exception ex)
                    {
                        AcceptCallback2(result);
                        /*if (!(ex is SocketException)
                        && !(ex is ObjectDisposedException)
                        && !(ex is ArgumentException))
                        {
                            Logger.LogError(ex);
                        }*/
                    }
                }
                else
                {
                    s = new TcpSocket(raw);

                    if (announce) Logger.Log(LogType.UserActivity, s.IP + " connected to the server.");
                    s.Init();
                }
            }
            catch (Exception ex)
            {
                /*if (!(ex is SocketException) 
                    && !(ex is ObjectDisposedException) 
                    && !(ex is ArgumentException))
                {
                    Logger.LogError(ex);
                }
                if (s != null) s.Close();*/
                AcceptCallback2(result);
            }
            listen.AcceptNextAsync();
        }

        public override void Close()
        {
            try
            {
                Listening = false;
                if (socket != null) socket.Close();
            }
            catch (Exception ex)
            {
                if (!(ex is SocketException)
                    && !(ex is ObjectDisposedException)
                    && !(ex is ArgumentException))
                {
                    Logger.LogError(ex);
                }
            }
        }
    }
    public class ClassiCubePlugin : Plugin
    {
        public static Mppass2Authenticator mppass2auth = new Mppass2Authenticator();
        public static MppassAuthenticator mppassAuth = new MppassAuthenticator();
        public const string CONFIG_FOLDER = "properties";
        public static AuthService CCService = AuthService.GetOrCreate("https://www.classicube.net/heartbeat.jsp", false);
        public override string Flames_Version { get { return Server.Version; } }
        public override string name { get { return "ClassiCube"; } }
        public override string creator { get { return "icanttellyou"; } }
        public static INetListen Listener = new TcpListen();
        public override void Load(bool auto)
        {
            ConfigElement.ParseFile(HeartbeatConfig.beatConfig, CONFIG_FOLDER + "/ccheartbeat.properties", CCConfig);
        	if (!LoginAuthenticator.Authenticators.Contains(mppass2auth))
            {
            	LoginAuthenticator.Authenticators.Add(mppass2auth);
			}
        	if (LoginAuthenticator.Authenticators.Contains(mppassAuth))
            {
            	LoginAuthenticator.Authenticators.Remove(mppassAuth);
			}
            AuthService.Services.Add(CCService);
			string Text = "\n\nURL = https://www.classicube.net/heartbeat.jsp \nname-suffix = " 
            	+ CCConfig.NameSuffix + " \nskin-prefix = " 
            	+ CCConfig.SkinPrefix + " \nmojang-auth = False";
         	File.AppendAllText("properties/authservices.properties", Text);
            AuthService.UpdateList();
            OnConfigUpdatedEvent.Register(OnConfigUpdated, Priority.Critical);
            OnConfigUpdated();
        }
        public override void Unload(bool auto)
        {
        	if (LoginAuthenticator.Authenticators.Contains(mppass2auth))
            {
            	LoginAuthenticator.Authenticators.Remove(mppass2auth);
			}
        	if (!LoginAuthenticator.Authenticators.Contains(mppassAuth))
            {
            	LoginAuthenticator.Authenticators.Add(mppassAuth);
			}
            OnConfigUpdatedEvent.Unregister(OnConfigUpdated);
            AuthService.Services.Remove(CCService);
            string contents = File.ReadAllText("properties/authservices.properties");
			string Text = "\n\nURL = https://www.classicube.net/heartbeat.jsp \nname-suffix = " 
                + CCConfig.NameSuffix + " \nskin-prefix = " 
                + CCConfig.SkinPrefix + " \nmojang-auth = False";
            string newContents = contents.Replace(Text, "");
            File.WriteAllText("properties/authservices.properties", newContents);
            AuthService.UpdateList();
            Heartbeat CCBeat = new ClassiCubeHeartbeat(ClassiCubeConfig);
            Heartbeat.Heartbeats.Remove(CCBeat);
            HeartbeatConfig.CloseSocket();
        }
        public HeartbeatConfig ClassiCubeConfig = new HeartbeatConfig();
        public static HeartbeatConfig CCConfig = new HeartbeatConfig();

        public void OnConfigUpdated()
        {
            if (!Directory.Exists(CONFIG_FOLDER)) Directory.CreateDirectory(CONFIG_FOLDER);
            if (!File.Exists(CONFIG_FOLDER + "/ccheartbeat.properties")) ClassiCubeConfig.Save(CONFIG_FOLDER);
            ClassiCubeConfig.Load(CONFIG_FOLDER);
            CCService.Salt = Server.GenerateSalt();
            CCService.MojangAuth = false;
            CCService.NameSuffix = ClassiCubeConfig.NameSuffix;
            CCService.SkinPrefix = ClassiCubeConfig.SkinPrefix;
            if (!AuthService.Services.Contains(CCService))
            {
                AuthService.Services.Add(CCService);
            }
            if (Heartbeat.Heartbeats.Any(h => h is ClassiCubeHeartbeat)) return;
            Heartbeat CCBeat = new ClassiCubeHeartbeat(ClassiCubeConfig);
            Heartbeat.Register(CCBeat);
        }
    }
    public class Mppass2Authenticator : LoginAuthenticator
    {
        public override bool Verify(Player p, string mppass)
        {
            foreach (AuthService auth in AuthService.Services)
            {
                if (Authenticate(auth, p, mppass)) return true;
            }
            return false;
        }

        public static bool Authenticate(AuthService auth, Player p, string mppass)
        {
            string calc = Server.CalcMppass(p.truename, auth.Salt);
			bool verified = mppass.CaselessEq(calc);
            if (!verified)
            {
            	if (auth == ClassiCubePlugin.CCService)
                {
                	Logger.Log(LogType.UserActivity, "&cLogin of {0} failed!", p.truename); 
                	Logger.Log(LogType.UserActivity, "(Connected using CC)"); 
                    p.name += ClassiCubePlugin.CCService.NameSuffix;
                    p.DisplayName += ClassiCubePlugin.CCService.NameSuffix;
                    p.truename += ClassiCubePlugin.CCService.NameSuffix;
                    return true;
                }
                else
                {
                	return false;
                }
            }
			auth.AcceptPlayer(p);
			return true;
    	}
    }
    public class ClassiCubeHeartbeat : Heartbeat
    {
        public HeartbeatConfig config;
        public new AuthService Auth = ClassiCubePlugin.CCService;
        public string LastResponse;
        public ClassiCubeHeartbeat(HeartbeatConfig conf)
        {
            URL = "https://www.classicube.net/heartbeat.jsp";
            config = conf;
        }
        public string proxyUrl;
        public bool checkedAddr;
        public void CheckAddress()
        {
            string hostUrl;
            checkedAddr = true;

            try
            {
                hostUrl = GetHost();
                proxyUrl = EnsureIPv4Url(hostUrl);
            }
            catch (Exception ex)
            {
                Logger.LogError("Error retrieving DNS information for CC", ex);
            }
        }

        // only want ASCII alphanumerical characters for salt
        public static bool AcceptableSaltChar(char c)
        {
            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z')
                || (c >= '0' && c <= '9');
        }

        /// <summary> Generates a random salt that is used for calculating mppasses. </summary>
        public static string GenerateSalt()
        {
            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            char[] str = new char[32];
            byte[] one = new byte[1];

            for (int i = 0; i < str.Length;)
            {
                rng.GetBytes(one);
                if (!AcceptableSaltChar((char)one[0])) continue;

                str[i] = (char)one[0]; i++;
            }
            string salt;
            if (!File.Exists("properties/cc_salt.txt"))
            {
            	salt = new string(str);
            }
            else
            {
            	salt = File.ReadAllText("properties/cc_salt.txt");
            }
            File.WriteAllText("properties/cc_salt.txt", salt);
            if (File.Exists("properties/cc_salt2.txt") && salt == File.ReadAllText("properties/cc_salt2.txt"))
            {
                GenerateNewSalt();
            }
            return salt;
        }
        public static void GenerateNewSalt()
        {
            GenerateSalt();
        }
        public static int NonHiddenCount()
        {
            Player[] players = PlayerInfo.Online.Items;
            int PlayerNumber = 0;
            foreach (Player p in players)
            {
				PlayerNumber++;
                if (p.hidden) 
                {
                	PlayerNumber--;
                }
            }
            return PlayerNumber;
        }
        public override string GetHeartbeatData()
        {
            string name = config.Name;
            OnSendingHeartbeatEvent.Call(this, ref name);
            name = Colors.StripUsed(name);
            Player p = new Player("Guest");
            return
                "&port=" + config.Port +
                "&max=" + config.MaxPlayers +
                "&name=" + name +
                "&public=true" +
                "&version=7" +
                "&salt=" + GenerateSalt() +
                "&users=" + NonHiddenCount().ToString() +
                "&software=" + Uri.EscapeDataString(config.Software) +
                "&web=" + config.WebClient;
        }
        public override void OnRequest(HttpWebRequest request)
        {
            request.ContentType = "application/x-www-form-urlencoded";

            if (!checkedAddr) CheckAddress();

            if (proxyUrl == null) return;
            request.Proxy = new WebProxy(proxyUrl);
        }

        public override void OnResponse(WebResponse response)
        {
            string text = HttpUtil.GetResponseText(response);
            if (!NeedsProcessing(text)) return;

            if (!text.Contains("\"errors\":"))
            {
                OnSuccess(text);
            }
            else
            {
                string error = GetError(text);
                if (error == null) error = "Error while finding CC URL. Is the port open?";
                OnError(error);
            }
        }
        public static void OnError(string error)
        {
            error = Truncate(error);
            Logger.Log(LogType.Warning, error);
        }


        public static string GetError(string json)
        {
            JsonReader reader = new JsonReader(json);
            // silly design, but form of json is:
            // {
            //   "errors": [ ["Error 1"], ["Error 2"] ],
            //   "response": "",
            //   "status": "fail"
            // }
            JsonObject obj = reader.Parse() as JsonObject;
            if (obj == null || !obj.ContainsKey("errors")) return null;

            JsonArray errors = obj["errors"] as JsonArray;
            if (errors == null) return null;

            foreach (object raw in errors)
            {
                JsonArray err = raw as JsonArray;
                if (err != null && err.Count > 0) return (string)err[0];
            }
            return null;
        }

        public static string Truncate(string text)
        {
            if (text.Length < 256) return text;

            return text.Substring(0, 256) + "..";
        }
        public bool NeedsProcessing(string text)
        {
            if (string.IsNullOrEmpty(text)) return false;
            if (text == LastResponse) return false;

            // only need to process responses that have changed
            LastResponse = text;
            return true;
        }

        public static void OnSuccess(string text)
        {
            text = Truncate(text);
            File.WriteAllText("text/ccexternalurl.txt", text);
            Logger.Log(LogType.SystemActivity, "CC URL found: " + text);
        }

        public override void OnFailure(string response)
        {
            if (!string.IsNullOrEmpty(response))
                Logger.Log(LogType.Warning, "[CC] Error: Server responded with " + response);
        }
    }

    public class ConfigLongAttribute : ConfigSignedIntegerAttribute
    {
        public long defValue, minValue, maxValue;

        public ConfigLongAttribute() : this(null, null, 0, long.MinValue, long.MaxValue)
        {
        }
        public ConfigLongAttribute(string name, string section, long def,
                                  long min = long.MinValue, long max = long.MaxValue) : base(name, section)
        {
            defValue = def;
            minValue = min;
            maxValue = max;
        }

        public override object Parse(string value)
        {
            return ParseLong(value, defValue, minValue, maxValue);
        }
    }
}
namespace Flames.Network.HeartBeat
{
    public class HeartbeatConfig
    {
        [ConfigString("name", "Auth service", "[MCGalaxy] Default", false)]
        public string Name = "[MCGalaxy] Default";
        [ConfigString("software", "Auth service", "MCGalaxy 1.9.5.1", true)]
        public string Software = "MCGalaxy 1.9.5.1";
        [ConfigLong("max-players", "Auth service", 16, 0, long.MaxValue)]
        public long MaxPlayers = 16;
        [ConfigInt("port", "Auth service", 25566, 0, int.MaxValue)]
        public int Port = 25566;
        [ConfigBool("support-web-client", "Auth service", true)]
        public bool WebClient = true;
        [ConfigBool("public", "Auth service", true)]
        public bool Public = true;
        [ConfigString("name-suffix", "Auth service", " (CC)", true)]
        public string NameSuffix = " (CC)";
        [ConfigString("skin-prefix", "Auth service", "", true)]
        public string SkinPrefix = "";

        public static INetListen Listener = new TcpListen();
        public static ConfigElement[] beatConfig = ConfigElement.GetAll(typeof(HeartbeatConfig));
        public void Load(string path)
        {
            ConfigElement.ParseFile(beatConfig, path + "/ccheartbeat.properties", this);
            if (beatConfig == null) beatConfig = ConfigElement.GetAll(typeof(HeartbeatConfig));
            SetupSocket(Port);
        }
        public static void SetupSocket(int port)
        {
            IPAddress ip;
            if (!IPAddress.TryParse(Server.Config.ListenIP, out ip))
            {
                Logger.Log(LogType.Warning, "Unable to parse listen IP config key, listening on any IP");
                ip = IPAddress.Any;
            }
            Listener.Listen2(ip, port);
            Logger.Log(LogType.SystemActivity, "Port {0} is for CC", port);
        }
        public static void CloseSocket()
        {
            int port = Listener.Port;
            try
            {
                Listener.Close();
            }
            catch (Exception ex)
            {
                if (!(ex is SocketException)
                    && !(ex is ObjectDisposedException)
                    && !(ex is ArgumentException))
                {
                    Logger.Log(LogType.Error, "Error closing CC port {0}:{1}", port, ex);
                }
                return;
            }
            Logger.Log(LogType.SystemActivity, "CC port {0} closed.", port);
        }
        public void Save(string path)
        {
            if (beatConfig == null) beatConfig = ConfigElement.GetAll(typeof(HeartbeatConfig));
            using (StreamWriter w = new StreamWriter(path + "/ccheartbeat.properties"))
            {
                w.WriteLine("# This file contains settings for configuring the CC heartbeat.");
                w.WriteLine("# name-suffix - See description in authservices.properties");
                w.WriteLine("# skin-prefix - See description in authservices.properties");
                w.WriteLine();
                ConfigElement.Serialise(beatConfig, w, this);
            }
            OnConfigUpdatedEvent.Call();
        }

        public static string externalIP;
        public static string GetExternalIP()
        {
            if (externalIP != null) return externalIP;

            try
            {
                externalIP = HttpUtil.LookupExternalIP();
            }
            catch (Exception ex)
            {
                Logger.LogError("Retrieving external IP", ex);
            }
            return externalIP;
        }
    }
}