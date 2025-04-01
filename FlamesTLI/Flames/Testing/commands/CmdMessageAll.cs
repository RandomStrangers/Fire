namespace Flames.Commands
{
    public class CmdMessageAll : Command
    {
        public override string name { get { return "MessageAll"; } }
        public override string shortcut { get { return "msgall"; } }
        public override string type { get { return CommandTypes.Added; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Owner; } }
        public override void Use(Player p, string message)
        {
            bool Empty = string.IsNullOrEmpty(message);
            if (Empty)
            {
                Help(p);
                return;
            }
            else
            {
                Player[] players = PlayerInfo.Online.Items;
                foreach (Player pl in players)
                {
                    pl.Message(message);
                }
            }
        }
        public override void Help(Player p)
        {
            p.Message("&T/MessageAll [message] &H- Sends a message to everyone.");
        }
    }
}