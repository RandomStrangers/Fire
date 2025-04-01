namespace Flames
{
    public class LogPlugin : Plugin
    {
        public override string name { get { return "LogChoice"; } }
        public override string creator { get { return "HarmonyNetwork"; } }
        public override void Load(bool startup)
        {
            Command.Register(new CmdLogChoice());
        }
        public override void Unload(bool shutdown)
        {
            Command.Unregister(Command.Find("LogChoice"));
        }
        public override void Help(Player p)
        {
            p.Message("No help is available for this plugin.");
        }
        public class CmdLogChoice : Command
        {
            public override string name { get { return "LogChoice"; } }
            public override string shortcut { get { return "Log"; } }
            public override string type { get { return CommandTypes.Added; } }
            public override bool museumUsable { get { return true; } }
            public override LevelPermission defaultRank { get { return LevelPermission.Owner; } }
            public override void Use(Player p, string message)
            {
                bool messageEmpty = string.IsNullOrEmpty(message);
                string[] args = message.SplitSpaces(2);
                bool Success = int.TryParse(args[0], out int x);
                LogType type = LogType.Debug;
                if (Success)
                {
                    type = (LogType)x;
                }
                if (args.Length < 1) 
                { 
                    Help(p); 
                    return; 
                }

                if (args.Length == 1)
                {
                    p.Message(args[0] + " is LogType." + type + ".");
                    return;
                }
                else
                {
                    if (args.Length >= 2)
                    {
                        string playername = p.truename;
                        string reason = args[1] + " sent by " + playername + ".";
                        p.Message("&cLog of type: " + type + " sent.");
                        Logger.Log(type, reason);
                    }
                }
                if (messageEmpty == true)
                {
                    Help(p);
                    return;
                }
            }

            public override void Help(Player p)
            {
                p.Message("&T/LogChoice [LogType] [Message] &H- Sends a message to Console.");
            }
        }
    }
}