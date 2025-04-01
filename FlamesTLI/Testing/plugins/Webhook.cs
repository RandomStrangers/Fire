//reference System.Net.dll
//reference System.dll
//reference Newtonsoft.Json.dll

// You will need to put your channel's webhook URL in line 32

using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace Flames
{
    public class ShutdownWebhook : Plugin
    {

        public override string creator { get { return "Venk"; } }
        public override string name { get { return "ShutdownWebhook"; } }

        public override void Load(bool startup)
        {
            HookLogger();
        }

        public override void Unload(bool shutdown)
        {
            Logger.LogHandler -= LogShutdownMessage;
        }

        public void HookLogger()
        {
            Logger.LogHandler += LogShutdownMessage;
        }

        public static void LogShutdownMessage(LogType type, string reason)
        {
            if (!Server.shuttingDown) return;
            else
            {
                string webhookUrl = "https://discord.com/api/webhooks/1273284410195578880/-bICkQvuobnGvJ7dLZ-r8sY2r-MruiaA78Qv1BqyFGqA26WAE5FSFWXpQbypHtbFHnvO";
                string message = "Server shutting down: " + reason + ".";
                try
                {
                    sendRequest(webhookUrl, message);
                }
                catch
                {
                }
            }
        }

        public static void sendRequest(string URL, string msg)
        {
            using (DiscordWeb dcWeb = new DiscordWeb())
            {
                dcWeb.ProfilePicture = Server.Config.ServerLogo;
                dcWeb.UserName = "Shutdown Webhook";
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
                Logger.Log(LogType.Warning, "Error sending shutdown webhook: " + msg);
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