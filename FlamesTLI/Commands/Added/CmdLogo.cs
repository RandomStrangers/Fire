namespace Flames.Commands.Chatting
{
    public class CmdServerLogo : Command
    {
        public override string name { get { return "ServerLogo"; } }
        public override string shortcut { get { return "Logo"; } }
        public override string type { get { return CommandTypes.Added; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Guest; } }
        public override void Use(Player p, string message)
        {
            p.Message(Server.Config.ServerLogo);
        }

        public override void Help(Player p)
        {
            p.Message("&T/ServerLogo &H- Displays the server logo.");
        }
    }
}