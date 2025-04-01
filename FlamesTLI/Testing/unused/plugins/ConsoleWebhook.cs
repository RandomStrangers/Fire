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
	public class ConsoleLogger : Plugin {
        
        public override string creator { get { return "Venk"; } }
        public override string name { get { return "ConsoleLogger"; } }

        public override void Load(bool startup) {
        	HookLogger();
            Logger.Log(LogType.SystemActivity, "Plugin loaded!");
        }
        public void HookLogger() {
		    Logger.LogHandler += ConsoleMessage;
		}
		
        public static void ConsoleMessage(LogType type, string ConsoleLog) {
            string webhookUrl = "https://discord.com/api/webhooks/1178363934022054010/U3HtnLvluicrAeVj2ev2XfiNl6Qzx1BygyWZNPKh9_NEy-MwM2vnRZRMQl29kUMwtjyy";
            string message = "`" + ConsoleLog + "`";
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
            Logger.LogHandler -= ConsoleMessage;
         Logger.Log(LogType.SystemActivity, "Console webhook unloaded!");   
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
				Logger.Log(LogType.Warning, "Error sending Console webhook: " + msg);
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
