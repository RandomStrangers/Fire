using System;
using Flames.Network;
namespace Flames.Commands
{
    public class CmdHeartbeat : Command
    {
        public override string name { get { return "heartbeat"; } }
        public override string shortcut { get { return "beat"; } }
        public override string type { get { return CommandTypes.Added; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Owner; } }

        public override void Use(Player p, string message)
        {
            string Name = "";
            try
            {
                foreach (Heartbeat beat in Heartbeat.Heartbeats)
                {
                    Name = beat.URL;
                    beat.Pump();
                    p.Message("Heartbeat pump {0} sent.", Name);
                }

            }
            catch (Exception e)
            {
                Logger.Log(LogType.Error, "Error with " + Name + " pump.", e);
                p.Message("Error with " + Name + " pump: " + e + ".");
            }
        }
        public override void Help(Player p)
        {
            p.Message("&T/heartbeat &H- Forces a pump for all heartbeats.  DEBUG PURPOSES ONLY.");
        }
    }
}