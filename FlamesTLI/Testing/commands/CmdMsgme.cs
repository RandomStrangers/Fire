namespace Flames.Commands
{
    public class CmdMessageMe : Command
    {
        public override string name { get { return "MessageMe"; } }
        public override string shortcut { get { return "msgme"; } }
        public override string type { get { return CommandTypes.Added; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Guest; } }
        public override void Use(Player p, string message)
        {
            bool MessageEmpty = string.IsNullOrEmpty(message);
            if (MessageEmpty)
            {
                Help(p);
                return;
            }
            else
            {
                p.Message(message);
            }
        }
        public override void Help(Player p)
        {
            p.Message("&T/MessageMe [message] &H- Sends a message to yourself.");
        }
    }
}