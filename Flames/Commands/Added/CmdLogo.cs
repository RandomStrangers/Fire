namespace Flames.Commands.Chatting
{
    public sealed class CmdServerLogo : Command2
    {
        public override string name { get { return "ServerLogo"; } }
        public override string shortcut { get { return "Logo"; } }
        public override string type { get { return CommandTypes.Chat; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Banned; } }
        public override void Use(Player p, string message, CommandData data)
        {
            p.Message(Server.Config.ServerLogo);
        }

        public override void Help(Player p)
        {
            p.Message("&T/ServerLogo &H- Displays the server logo.");
        }
    }
}