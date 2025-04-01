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
	public class BugWebhook : Plugin {
        
        public override string creator { get { return "Venk"; } }
        public override string name { get { return "BugWebhook"; } }

        public override void Load(bool startup) {
        	HookLogger();
        }

        public override void Unload(bool shutdown) {
           Logger.LogHandler -= LogMessage;
        }
        
        public void HookLogger() {
		    Logger.LogHandler += LogMessage;
		}
		
       public static void LogMessage(LogType type, string error) {
            if (type != LogType.Error) return;
            string webhookUrl = "https://discord.com/api/webhooks/1170745327708553326/fsMuw5VAp8cL3JTGtT9o6YSp8hDqZ-_jLBB-WQossQ-ONTvOaJZQ6-Ja25PZk0fraPTl";
            string message = "`" + error + "`";
            try { sendRequest(webhookUrl, message); } catch {}
        }

        public static void sendRequest(string URL, string msg) {
     		using (DiscordWeb dcWeb = new DiscordWeb()) {
         		dcWeb.ProfilePicture = "https://files.catbox.moe/gtugrk.png";
         		dcWeb.UserName = "Debug Webhook";
         		dcWeb.WebHook = URL;
         		dcWeb.SendMessage(msg);
      		}
   		}
    }
	
	public class DiscordWeb : IDisposable {
        public WebClient wc;
        public string WebHook, UserName, ProfilePicture;
        
        public class DiscordMessage {
        	public string username;
        	public string avatar_url;
        	public string content;
        }

        public DiscordWeb() {
            wc = new WebClient();
        }

       public string ToJson(string message) {
        	DiscordMessage msg = new DiscordMessage();
        	msg.username = UserName;
        	msg.avatar_url = ProfilePicture;
        	msg.content = message;
			return JsonConvert.SerializeObject(msg);
        }

		public void LogFailure(WebException ex) {
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
