namespace Flames.Commands
{
    public class CmdOsGo : Command
    {
        public override string name { get { return "OsGo"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return CommandTypes.Added; } }
        public override bool SuperUseable { get { return false; } }
        public override bool ShowCommandInfo { get { return false; } }

        public override void Use(Player p, string message)
        {
            if (!LevelInfo.MapExists(p.truename))
            {
                p.Message("You do not have a realm, try creating one using /os map add!");
                return;
            }
            else
            {
                Find("Overseer").Use(p, "go");
                return;
            }
        }

        public override void Help(Player p)
        {
            return;
        }
    }
}