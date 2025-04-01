namespace Flames
{
    public class ErrorPlugin : Plugin
    {
        public override string name { get { return "Error"; } }
        public override string creator { get { return "HarmonyNetwork"; } }
        public override void Load(bool startup)
        {
            Command.Register(new CmdError());
        }
        public override void Unload(bool shutdown)
        {
            Command.Unregister(Command.Find("Error"));
        }
        public override void Help(Player p)
        {
            p.Message("No help is available for this plugin.");
        }
        public class CmdError : Command
        {
            public override string name { get { return "Error"; } }
            public override string shortcut { get { return ""; } }
            public override string type { get { return CommandTypes.Added; } }
            public override bool museumUsable { get { return true; } }
            public override LevelPermission defaultRank { get { return LevelPermission.Owner; } }
            public override void Use(Player p, string message)
            {
                bool messageEmpty = string.IsNullOrEmpty(message);
                string playername = p.truename;
                p.Message("&cTest error sent.");
                if (messageEmpty == true)
                {
                    Logger.Log(LogType.Error, playername + " used /Error");
                }
                else
                {
                    Logger.Log(LogType.Error, message);
                }
            }

            public override void Help(Player p)
            {
                p.Message("&T/Error &H- Sends a false error message to Console");
            }
        }
    }
}