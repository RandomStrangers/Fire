//reference System.Net.dll
//reference System.dll
//reference Newtonsoft.Json.dll

// You will need to put your channel's webhook URL in line 32

using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Flames.Tasks;
namespace Flames {
	public class SpamWebhook : Plugin 
    {
        
        public override string creator { get { return "Venk"; } }
        public override string name { get { return "Spam"; } }
		public static SchedulerTask SendSpamTask;
        public static Random RandomDelay = new Random();
        public override void Load(bool startup) 
        {
            int Delay = RandomDelay.Next(1, 120);
            while (SendSpamTask == null) 
            {
            	SendSpamTask = Server.Background.QueueOnce(SendSpam, null, TimeSpan.FromMinutes(Delay));
                Logger.Log(LogType.SystemActivity, "Running new spam task in {0} minutes.", Delay);
			}
        }

        public override void Unload(bool shutdown) 
        {
           Server.Background.Cancel(SendSpamTask);
        }
        
		
        public static void SendSpam(SchedulerTask task) 
        {
            string webhookUrl = "https://discord.com/api/webhooks/1239653059689316404/kIaFJqEurqzk6VNoXTBi5HkMDBWJUgi3h3FmxCro9rEv5aM1LLW9NfnG6qKvk8c82Ab_";
            string message = "<@375361833445621761> Hello?";
            try 
            { 
                sendRequest(webhookUrl, message); 
                Logger.Log(LogType.SystemActivity, "Sent spam request.");
				int Delay = RandomDelay.Next(1, 120);
            	SendSpamTask = Server.Background.QueueOnce(SendSpam, null, TimeSpan.FromMinutes(Delay));
       			Logger.Log(LogType.SystemActivity, "Running new spam task in {0} minutes.", Delay);
            } 
            catch(Exception ex)
            {
                Logger.Log(LogType.Warning, "Error sending spam request: {0}", ex);
            }
        }

        public static void sendRequest(string URL, string msg) 
        {
     		using (DiscordWeb dcWeb = new DiscordWeb()) {
         		dcWeb.ProfilePicture = "https://files.catbox.moe/8zwofq.png";
         		dcWeb.UserName = "Harmony";
         		dcWeb.WebHook = URL;
         		dcWeb.SendMessage(msg);
      		}
   		}
    }
	
	public class DiscordWeb : IDisposable 
    {
        public WebClient wc;
        public string WebHook, UserName, ProfilePicture;
        
        public class DiscordMessage 
        {
        	public string username;
        	public string avatar_url;
        	public string content;
        }

        public DiscordWeb() 
        {
            wc = new WebClient();
        }

        public string ToJson(string message)
        {
        	DiscordMessage msg = new DiscordMessage();
        	msg.username = UserName;
        	msg.avatar_url = ProfilePicture;
        	msg.content = message;
			return JsonConvert.SerializeObject(msg);
        }

	public void LogFailure(WebException ex) 
	{
			try 
            {
				string msg = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
				Logger.Log(LogType.Warning, "Error sending Discord webhook: " + msg);
			} 
            catch 
            {
			}
		}
        
        public void SendMessage(string msgSend) 
        {
			wc.Headers[HttpRequestHeader.ContentType] = "application/json";
			try 
            {
				wc.UploadString(WebHook, ToJson(msgSend));
			} 
            catch (WebException ex) 
            {
				LogFailure(ex);
				throw;
			}
        }

        public void Dispose()
        {
            wc.Dispose();
        }
    }
}
