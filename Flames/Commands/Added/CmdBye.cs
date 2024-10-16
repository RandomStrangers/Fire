namespace Flames.Commands.Misc
{
    public class CmdBye : Command
    {
        public override string name { get { return "Bye"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return CommandTypes.Added; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Owner; } }
        public override bool MessageBlockRestricted { get { return true; } }
        public override bool UseableWhenFrozen { get { return true; } }
        public override void Use(Player p, string message)
        {
            Player[] players = PlayerInfo.Online.Items;
            foreach (Player p2 in players)
            {
                p2.Leave(message);
            }
        }
        public override void Help(Player p)
        {
            p.Message("&T/Bye [message] &H- Makes ALL players leave the server with an optional message");
        }
    }
}