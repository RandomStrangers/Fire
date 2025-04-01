using System;
using Flames.Tasks;

namespace Flames
{
	public class NoAFK : Plugin_Simple
	{
		public override string name { get { return "Plugin_Simple"; } }
		public override string creator { get { return "RandomStrangers"; } }
		public override void Load(bool startup) {
	 Server.MainScheduler.QueueOnce(UnAFK, null, TimeSpan.FromSeconds(0));
     Server.MainScheduler.QueueRepeat(UnAFK, null, TimeSpan.FromSeconds(660));
		}
   
  void UnAFK(SchedulerTask task) {
    Command.Find("sendcmd").Use(Player.Flame, "Stella" + " afk");
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