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
	public class ConsoleWebhook : Plugin {
        
        public override string creator { get { return "Venk"; } }
        public override string name { get { return "Console"; } }

        public override void Load(bool startup) {
        	HookLogger();
            Logger.Log(LogType.UserActivity, "Plugin loaded!");
        }
        public void HookLogger() {
		    Logger.LogHandler += LogMessage;
		}
		
        public static void LogMessage(LogType type, string connection) {
            if (type != LogType.UserActivity) return;
            string webhookUrl = "https://discord.com/api/webhooks/1178363934022054010/U3HtnLvluicrAeVj2ev2XfiNl6Qzx1BygyWZNPKh9_NEy-MwM2vnRZRMQl29kUMwtjyy";
            string message = "\n`\n" + connection + "`";
            try { sendRequest(webhookUrl, message); } catch {}
        }

        public static void sendRequest(string URL, string msg) {
     		using (DiscordWeb dcWeb = new DiscordWeb()) {
         		dcWeb.ProfilePicture = "https://files.catbox.moe/6uiix1.png";
         		dcWeb.UserName = "Harmony Network (Console)";
         		dcWeb.WebHook = URL;
         		dcWeb.SendMessage(msg);
      		}
   		}
           public override void Unload(bool shutdown) {
            Logger.LogHandler -= LogMessage;
         Logger.Log(LogType.UserActivity, "Plugin Unloaded!");   
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
