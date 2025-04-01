namespace Flames.Commands
{
    public class CmdSuperQuit : Command
    {
        public override string name { get { return "SuperBye"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return CommandTypes.Added; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Owner; } }
        public override bool MessageBlockRestricted { get { return true; } }
        public override bool UseableWhenFrozen { get { return true; } }
        public override bool ShowCommandInfo { get { return false; } }
        public override void Use(Player p, string message)
        {
            Player[] players = PlayerInfo.Online.Items;
            foreach (Player pl in players)
            {
                if (pl.IsSuper)
                {
                    pl.IsSuper = false;
                    pl.Leave("");
                }
            }
        }
        public override void Help(Player p)
        {
            if (p.IsSuper)
            {
                p.Message("&T/SuperBye &H- Makes all super players disconnect.");
            }
            else
            {
                p.Message("");
            }
        }
    }
}