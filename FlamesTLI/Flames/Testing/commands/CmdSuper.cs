using Flames.DB;
namespace Flames.Commands
{
    public class CmdSuper : Command
    {
        public override string name { get { return "Super"; } }
        public override bool MessageBlockRestricted { get { return true; } }
        public override string type { get { return CommandTypes.Added; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Admin; } }
        public override string shortcut { get { return ""; } }
        public override bool UpdatesLastCmd { get { return false; } }
        public override bool ShowCommandInfo { get { return false; } }

        public override void Use(Player p, string message)
        {
            Execute(p);
        }
        public void Execute(Player pl)
        {
            if (!pl.IsSuper)
            {
                pl.IsSuper = true;
                pl.Message("&cWarning: &5You can no longer be kicked and will remain connected until you use this command again!");
                pl.SuperName = pl.truename;
            }
            else
            {
                pl.IsSuper = false;
                pl.Message("&cWarning: &5You will be disconnected upon closing the client now.");
            }
        }
        public override void Help(Player p)
        {
            if (p.IsFire)
            {
                p.Message("Does nothing.");
            }
            else if (p.IsSuper)
            {
                p.Message("Does nothing.");
            }
            else
            {
                string msg = PlayerDB.GetLogoutMessage(p.name);
                p.Leave(msg);
            }
        }
    }
}