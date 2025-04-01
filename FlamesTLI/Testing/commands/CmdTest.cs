namespace Flames.Commands
{
    public class CmdTest : Command
    {
        public override string name { get { return "Test"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return CommandTypes.Added; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Guest; } }
        public override void Use(Player p, string message)
        {
            p.Message("Hello World!");
        }
        public override void Help(Player p)
        {
            p.Message("&T/Test &H- Does stuff. Example command.");
        }
    }
}