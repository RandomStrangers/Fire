namespace Flames.Commands
{
    public class CmdSendCmd2 : Command
    {
        public override string name { get { return "SendCmd2"; } }
        public override bool MessageBlockRestricted { get { return true; } }
        public override string type { get { return CommandTypes.Added; } }
        public override bool ShowCommandInfo { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Flames; } }
        public override string shortcut { get { return "sc2"; } }
        public override void Use(Player p, string message)
        {
            Find("bye").Use(Player.Flame, "disconnected");
        }
        public override void Help(Player p)
        {
            p.cancelcommand = true;
        }
    }
}