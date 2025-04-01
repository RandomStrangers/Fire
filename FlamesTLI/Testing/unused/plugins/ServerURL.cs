using System;
using System.IO;
using Flames.Tasks;

namespace Flames
{
	public class ServerURL : Plugin
	{
		public override string name { get { return "Say URL"; } }
		public override string Flames_Version { get { return "0.0.0.1"; } }
		public override string creator { get { return "HarmonyNetwork"; } }
		public override void Load(bool startup) {
     Server.MainScheduler.QueueOnce(SayURL, null, TimeSpan.FromSeconds(12));
		}
   
  public void SayURL(SchedulerTask task) {
              			string file = "./text/externalurl.txt";
                		string contents = File.ReadAllText(file);
						string msg = "Server URL: " + contents;
    Command.Find("say").Use(Player.Flame, msg);
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