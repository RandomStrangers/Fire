namespace Flames.Commands.Maintenance
{
    public class CmdShutdown : Command
    {
        public override string name { get { return "Shutdown"; } }
        public override string type { get { return CommandTypes.Moderation; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Admin; } }

        public override void Use(Player p, string message)
        {

#if CORE
            if (p.IsNull)
            {
                p.Message("Sorry, not shutting down today!");
            }
#else
            if (p.IsFire || p.IsConsole)
            {
                p.Message("Sorry, not shutting down today!");
            }
#endif
            p.Message("Command has been moved to /ShutdownTrue.");
            p.Message("This is to prevent automatic shut downs.");
        }
        public override void Help(Player p)
        {
            p.Message("Command has been moved to /ShutdownTrue.");
            p.Message("This is to prevent automatic shut downs.");
        }
    }
}
