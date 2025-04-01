//reference System.Net.dll
//reference System.dll
//reference Newtonsoft.Json.dll

// You will need to put your channel's webhook URL in line 32

using System;
using System.IO;
using System.Net;
using Flames;
using Newtonsoft.Json;

namespace Core {
	public class LogWebhook : Plugin {
        
        public override string creator { get { return ""; } }
        public override string name { get { return "Webhook"; } }

        public override void Load(bool startup) {
        	HookLogger();
        }
        
        void HookLogger() {
            
		    Logger.LogHandler += LogMessage;
		}
		
        static void LogMessage(LogType type, string loggedmessage) {
            if (type == LogType.RelayActivity) return;
            string webhookUrl = "https://discord.com/api/webhooks/1170738785307340800/YF4-wJr8oaUGwC8-ztxATzsFNfPHc8jOkKVo8ECBGpZCrbf8Orpn17gJxfFtgDCTEYj8";
            string message = "\n```\n" + loggedmessage + "```";
            try { sendRequest(webhookUrl, message); } catch {}
        }

        public static void sendRequest(string URL, string msg) {
     		using (DiscordWeb dcWeb = new DiscordWeb()) {
         		dcWeb.ProfilePicture = "https://files.catbox.moe/6uiix1.png";
         		dcWeb.UserName = Server.Config.Name;
         		dcWeb.WebHook = URL;
         		dcWeb.SendMessage(msg);
      		}
   		}
            public override void Unload(bool shutdown) {
            Logger.LogHandler -= LogMessage;
        dcWeb.SendMessage("Plugin Unloaded!");
        }
    }
	
	public class DiscordWeb : IDisposable {
        readonly WebClient wc;
        public string WebHook, UserName, ProfilePicture;
        
        sealed class DiscordMessage {
        	public string username;
        	public string avatar_url;
        	public string content;
        }

        public DiscordWeb() {
            wc = new WebClient();
        }

        string ToJson(string message) {
        	DiscordMessage msg = new DiscordMessage();
        	msg.username = UserName;
        	msg.avatar_url = "https://files.catbox.moe/6uiix1.png";
        	msg.content = message;
			return JsonConvert.SerializeObject(msg);
        }

		void LogFailure(WebException ex) {
			try {
				string msg = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
				Logger.Log(LogType.Warning, "Error sending Discord webhook: " + msg);
			} catch {
			}
		}
        
        public void SendMessage(string msgSend) {
			wc.Headers[HttpRequestHeader.ContentType] = "application/json";
			try {
	        		wc.UploadString(WebHook, ToJson(msgSend));
			} catch (WebException ex) {
				LogFailure(ex);
				throw;
			}
        }

        public void Dispose() {
            wc.Dispose();
        }
    }    
}
