namespace Flames.Commands
{
    public class CmdHelloWorld : Command
    {
        public override string name { get { return "HelloWorld"; } }
        public override string shortcut { get { return "hw"; } }
        public override string type { get { return CommandTypes.Added; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Guest; } }
        public override void Use(Player p, string message)
        {
            p.Message("Hello World!");
        }

        public override void Help(Player p)
        {
            p.Message("&T/HelloWorld &H- Does stuff. Example command.");
        }
    }
}