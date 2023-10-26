using System;

namespace Flames.Commands.Chatting
{
    public sealed class CmdEnd : Command2
    {
        public override string name { get { return "End"; } }
        public override string type { get { return CommandTypes.Other; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Flames; } }
 
        public override void Use(Player p, string message, CommandData data)
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
            p.Message("&T/End &H- Kills the server");
        }
    }
}