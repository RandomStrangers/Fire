using Flames.Tasks;
using System;
namespace Flames
{
	public class SpamPlugin  : Plugin
	{
		public override string name { get { return "Spam"; } }
		public override string creator { get { return "HarmonyNetwork"; } }
		public override void Load(bool startup)
		{
        SendSpamTask = Server.Background.QueueRepeat(SendSpam, null, TimeSpan.FromSeconds(2));
		}

		public override void Unload(bool shutdown)
		{
            Server.Background.Cancel(SendSpamTask);
		}
		public override void Help(Player p)
		{
			p.Message("No help is available for this plugin.");
		}
		public static SchedulerTask SendSpamTask;
        public void SendSpam(SchedulerTask task)
        {            //Logger.Log(LogType.SystemActivity, "(SpamTask) used /Opchat <@375361833445621761>");
            //Command.Find("Opchat").Use(Player.Flame, "<@375361833445621761>");
          Logger.Log(LogType.Debug, "<@375361833445621761>");
        }
    }
}