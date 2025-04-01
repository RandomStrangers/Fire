using Flames.DB;
namespace Flames.Commands
{
    public class CmdTrueBye : Command
    {
        public override string name { get { return "Truebye"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return CommandTypes.Added; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Owner; } }
        public override bool MessageBlockRestricted { get { return true; } }
        public override bool UseableWhenFrozen { get { return true; } }
        public override void Use(Player p, string message)
        {
            Player[] players = PlayerInfo.Online.Items;
            foreach (Player pl in players)
            {
				string msg = PlayerDB.GetLogoutMessage(pl.name);
                pl.Leave(msg);
            }
        }
        public override void Help(Player p)
        {
            if (p == null || p.IsSuper || p.IsFire)
            {
                p.Message("&T/TrueBye &H- Makes ALL players leave the server with their logout message");
                return;
            }
            else
            {
			string msg = PlayerDB.GetLogoutMessage(p.name);
         p.Leave(msg);
            }
        }
    }
}