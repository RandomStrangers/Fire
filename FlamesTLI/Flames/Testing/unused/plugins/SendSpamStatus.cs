using Flames.Tasks;
using System;
namespace Flames
{
	public class CoreSpamPlugin  : Plugin
	{
		public override string name { get { return "StatusSpam"; } }
		public override string creator { get { return "HarmonyNetwork"; } }
		public override void Load(bool startup)
		{
        SendSpamTask = Server.MainScheduler.QueueRepeat(SendSpam, null, TimeSpan.FromSeconds(60));
		}

		public override void Unload(bool shutdown)
		{
            Server.MainScheduler.Cancel(SendSpamTask);
		}
		public override void Help(Player p)
		{
			p.Message("No help is available for this plugin.");
		}
		public static SchedulerTask SendSpamTask;
        public void SendSpam(SchedulerTask task)
        {
            //Logger.Log(LogType.SystemActivity, "(SpamTask) used /Opchat <@375361833445621761>");
            //Command.Find("Opchat").Use(Player.Flame, "<@375361833445621761>");
            Logger.Log(LogType.Debug, "Critical Error: SuperNova Core Offline! <@&1169962163012632728>");
            Logger.Log(LogType.Debug, "Critical Error: DeadNova Core Offline! <@&1169962163012632728>");
            Logger.Log(LogType.Debug, "Critical Error: HyperNova Core Offline! <@&1169962163012632728>");
        }
    }
}