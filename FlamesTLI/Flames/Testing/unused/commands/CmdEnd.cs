using System;

namespace Flames.Commands
{
    public class CmdEnd2 : Command
    {
        public override string name { get { return "End2"; } }
        public override string type { get { return CommandTypes.Added; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Flames; } }
 
        public override void Use(Player p, string message)
        {
            End(p);
        }
        static void End(Player p)
        {
            if (!CheckPerms(p))
            {
                p.Message("Only the Flames or the Server Owner can end the server."); return;
            }
            Environment.Exit(0);        
        }
        static bool CheckPerms(Player p)
        {
            if (p.IsFire) return true;

            if (Server.Config.OwnerName.CaselessEq("Notch")) return false;
            return p.name.CaselessEq(Server.Config.OwnerName);
        }
        public override void Help(Player p)
        {
            p.Message("&T/End2 &H- Kills the server.");
        }
    }
}