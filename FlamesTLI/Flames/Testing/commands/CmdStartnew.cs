//reference System.dll
using System.Diagnostics;
namespace Flames.Commands
{
    public class CmdStartNew : Command
    {
        public override string name { get { return "ServerStart"; } }
        public override bool MessageBlockRestricted { get { return true; } }
        public override string shortcut { get { return "Start"; } }
        public override string type { get { return CommandTypes.Added; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Flames; } }

        public override void Use(Player p, string message)
        {
            StartNew(p);
        }
        public static void StartNew(Player p)
        {
            if (!CheckPerms(p))
            {
                p.Message("Only the Flames or the Server Owner can use this command!"); 
                return;
            }
            Process.Start("FlamesCLI.exe");
        }
        public static bool CheckPerms(Player p)
        {
            if (p.IsFire)
            {
                return true;
            }
            if (p.IsSuper && !p.IsFire)
            { 
                return false;
            }
            if (Server.Config.OwnerName.CaselessEq("Notch"))
            {
                return false;
            }
            return p.name.CaselessEq(Server.Config.OwnerName);
        }
        public override void Help(Player p)
        {
            p.Message("");
        }
    }
}