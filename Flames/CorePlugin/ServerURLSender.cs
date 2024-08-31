using System;
using System.IO;
using Flames.Tasks;

namespace Flames.Core
{
    public class ServerURLSender : Plugin
    {
        public override string name { get { return "Say URL"; } }
        public override string creator { get { return Server.SoftwareName + " team"; } }
        public override void Load(bool startup)
        {
            bool CanSend = Server.Config.SendURL;
            bool IsPublic = Server.Config.Public;
            bool SayHi = Server.Config.SayHello;

            if (IsPublic)
            {

                if (CanSend)
                {
                    {
                        Server.MainScheduler.QueueOnce(SayURL, null, TimeSpan.FromSeconds(12));
                    }
                }
                else
                {
                    Logger.Log(LogType.SystemActivity, "Server setting \"send-url\" is false!");
                    Logger.Log(LogType.SystemActivity, "Cannot send URL to chat!");
                    return;
                }
            }
            else
            {
                Logger.Log(LogType.SystemActivity, "Server setting \"public\" is false!");
                Logger.Log(LogType.SystemActivity, "Cannot send URL to chat!");
                return;
            }
            if (SayHi)
            {
                Server.Background.QueueOnce(SayHello, null, TimeSpan.FromSeconds(10));
            }
        }
       public static void SayHello(SchedulerTask task)
        {
            Command.Find("say").Use(Player.Flame, Server.SoftwareNameVersioned + " &Sonline!");
            Logger.Log(LogType.SystemActivity, "Hello World!");
        }
        public static void SayURL(SchedulerTask task)
        {
            string file = "./text/externalurl.txt";
            string contents = File.ReadAllText(file);
            string msg = "Server URL: " + contents;
            Command.Find("say").Use(Player.Flame, msg);
            Logger.Log(LogType.SystemActivity, "Server URL sent to chat!");
        }
        public override void Unload(bool shutdown)
        {
        }
        public override void Help(Player p)
        {
            p.Message("");
        }
    }
}