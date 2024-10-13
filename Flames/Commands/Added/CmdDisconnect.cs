
namespace Flames.Commands.Misc
{
    public class CmdDisconnect : Command
    {
        public override string name { get { return "Disconnect"; } }
        public override string shortcut { get { return "leave"; } }
        public override string type { get { return CommandTypes.Added; } }
        public override bool MessageBlockRestricted { get { return true; } }
        public override bool SuperUseable { get { return false; } }

        public override bool UseableWhenFrozen { get { return true; } }
        public override void Use(Player p, string message)
        {
            p.Leave(message);
        }
        public override void Help(Player p)
        {
            p.Message("&T/Disconnect [message] &H- Leaves the server with an optional message.");
        }
    }
}