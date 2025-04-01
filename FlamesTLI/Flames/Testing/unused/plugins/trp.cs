using System;
using Flames.Tasks;
using System.IO;
namespace Flames
{
	public class TheRestorationProject : Plugin
	{
		public override string name { get { return "TRP"; } }
		public override string Flames_Version { get { return Server.Version; } }
		public override string creator { get { return "HarmonyNetwork"; } }
		public override void Load(bool startup) { 
        Server.Background.QueueOnce(Part1, null, TimeSpan.FromSeconds(0));
        Server.Background.QueueOnce(Part2, null, TimeSpan.FromSeconds(2));
        Server.Background.QueueOnce(Part3, null, TimeSpan.FromSeconds(4));
        }
   
  		public void Part1(SchedulerTask task) 
  		{
  			Command.Find("say").Use(Player.Flame,  Server.SoftwareNameVersioned + " detected as offline! Shutting down!..."); 
  		}
		public void Part2(SchedulerTask task) 
        {  
       	    File.Delete("plugins/trp.dll"); 
        }
       public void Part3(SchedulerTask task) 
       {  
       		Command.Find("end").Use(Player.Flame, ""); 
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