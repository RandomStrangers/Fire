using System;
using MCGalaxy.Tasks;
namespace MCGalaxy
{
    public class NoAFK : Plugin
    {
        public override string name { get { return "NoAFK"; } }
        public override string creator { get { return "HarmonyNetwork"; } }
        public static SchedulerTask NoAFKTask;
        public override void Load(bool startup)
        {
            NoAFKTask = Server.Background.QueueRepeat(KickAFK, null, TimeSpan.FromSeconds(10));
        }
        public void KickAFK(SchedulerTask task)
        {
            Player[] players = PlayerInfo.Online.Items;
           	foreach (Player p2 in players)
            {
				if (p2.IsAfk)
				{
                	p2.Kick("kicked by console: AFK.");
				}
            }
        }
        public override void Unload(bool shutdown)
        {
            Server.Background.Cancel(NoAFKTask);
        }
    }
}