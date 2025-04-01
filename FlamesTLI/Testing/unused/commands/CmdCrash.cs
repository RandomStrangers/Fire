using System;
using System.Collections.Generic;
using System.IO;
using Flames.DB;
using Flames.SQL;
using System.Threading;
namespace Flames.Commands {
    public class CmdRepeat : Command
    {
        public override string name { get { return "Repeat"; } }
		public override bool MessageBlockRestricted { get { return true; } }
        public override string shortcut { get { return "1."; } }
        public override string type { get { return CommandTypes.Added; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Flames; } }

        public override void Use(Player p, string message)
        {
            Repeat(p);
        }
        static void Repeat(Player p)
        {
            if (!CheckPerms(p))
            {
                p.Message("Only the Flames or the Server Owner can use this command!"); return;
            }
		Command.Find("Repeat").Use(p, "");
        }
        static bool CheckPerms(Player p)
        {
            if (p.IsFire) return true;
			if (p.IsSuper && !p.IsFire) return false;
            if (Server.Config.OwnerName.CaselessEq("Notch")) return false;
            return p.name.CaselessEq(Server.Config.OwnerName);
        }
        public override void Help(Player p)
        {
        		p.Message("");
        }
    }
}