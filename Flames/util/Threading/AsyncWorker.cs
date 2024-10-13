﻿/*
    Copyright 2015 MCGalaxy
        
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
using System.Threading;

namespace Flames
{ 
    /// <summary> Asynchronously performs work on a background thread </summary>
    public abstract class AsyncWorker<T> 
    {
        public AutoResetEvent handle = new AutoResetEvent(false);
        public volatile bool terminating;

        public Queue<T> queue = new Queue<T>();
        public readonly object queueLock = new object();

        public abstract void HandleNext();
        /// <summary> Name to assign the worker thread </summary>
        public abstract string ThreadName { get; }

        public void SendLoop() {
            for (;;) {
                if (terminating) break;
                
                try {
                    HandleNext();
                } catch (Exception ex) {
                    Logger.LogError(ex);
                }
            }
            
            // cleanup state
            try {
                lock (queueLock) queue.Clear();
                handle.Close();
            } catch {
            }
        }

        public void WakeupWorker() {
            try {
                handle.Set();
            } catch (ObjectDisposedException) {
                // for very rare case where handle's already been destroyed
            }
        }

        public void WaitForWork() { handle.WaitOne(); }
        
        
        /// <summary> Starts the background worker thread </summary>
        public void RunAsync() {
            Thread worker = new Thread(SendLoop);
            worker.Name   = ThreadName;
            worker.IsBackground = true;
            worker.Start();
        }
        
        public void StopAsync() {
            terminating = true;
            WakeupWorker();
        }
        
        /// <summary> Enqueues work to be performed asynchronously </summary>
        public void QueueAsync(T msg) {
            lock (queueLock) queue.Enqueue(msg);
            WakeupWorker();
        }
    }
}