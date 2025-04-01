using System;

namespace Flames.Commands.Chatting
{
    public class CmdEnd : Command
    {
        public override string name { get { return "End"; } }
        public override string type { get { return CommandTypes.Added; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Flames; } }

        public override void Use(Player p, string message)
        {
            End(p);
        }
        public static void End(Player p)
        {
            if (!CheckPerms(p))
            {
                p.Message("Only the Flames or the Server Owner can end the server."); 
                return;
            }
            p.Message("Command disabled in this branch.");
            //Environment.Exit(0);
        }
        public static bool CheckPerms(Player p)
        {
#if CORE 
            if (p.IsNull) return true;
#else
            if (p.IsFire) return true;
            else if (p.IsConsole) return true;
#endif

            if (Server.Config.OwnerName.CaselessEq("Notch")) return false;
            return p.name.CaselessEq(Server.Config.OwnerName);
        }
        public override void Help(Player p)
        {
            p.Message("&T/End &H- Kills the server");
        }
    }
}
