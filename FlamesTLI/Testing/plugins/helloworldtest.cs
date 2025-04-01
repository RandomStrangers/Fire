namespace Flames
{
    public class helloworld : Plugin
    {
        public override string name { get { return "HelloWorld"; } }
        public override string creator { get { return "HarmonyNetwork"; } }
        public override void Load(bool startup)
        {
            Command.Register(new CmdHelloWorld2());
        }
        public override void Unload(bool shutdown)
        {
            Command.Unregister(Command.Find("HelloWorld2"));
        }
        public override void Help(Player p)
        {
            p.Message("No help is available for this plugin.");
        }
        public class CmdHelloWorld2 : Command
        {
            public override string name { get { return "HelloWorld2"; } }
            public override string shortcut { get { return "hw2"; } }
            public override string type { get { return CommandTypes.Added; } }
            public override bool museumUsable { get { return true; } }
            public override LevelPermission defaultRank { get { return LevelPermission.Guest; } }
            public override void Use(Player p, string message)
            {
                p.Message("Hello World!");
            }

            public override void Help(Player p)
            {
                p.Message("&T/HelloWorld2 &H- Does stuff. Example command.");
            }
        }
    }
}