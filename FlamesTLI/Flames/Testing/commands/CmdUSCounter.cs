namespace Flames.Commands
{
    public class CmdUnsignedShortCounter : Command
    {
        public override string name { get { return "UnsignedShortCounter"; } }
        public override string shortcut { get { return "USCount"; } }
        public override string type { get { return CommandTypes.Added; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Owner; } }
        public static ushort x;
        public override void Use(Player p, string message)
        {
            bool MessageEmpty = string.IsNullOrEmpty(message);
            bool IsZero = message.CaselessEq("0");
            if (IsZero)
            {
                p.Message("0");
                p.Message("Limit reached!");
                p.Message("Do the command again to restart!");
                return;
            }
            if (MessageEmpty)
            {
                Help(p);
                return;
            }
            bool Success = ushort.TryParse(message, out ushort z);
            bool TooHigh = z > ushort.MaxValue;
            if (TooHigh)
            {
                p.Message("The requested end value is too high! Please choose a value less than {0}.", ushort.MaxValue);
                return;
            }
            bool TooLow = z < ushort.MinValue;
            if (TooLow)
            {
                p.Message("The requested end value is too low! Please choose a value greater than {0}.", ushort.MinValue);
                return;
            }
            if (!Success)
            {
                Help(p);
                return;
            }
            if (Success)
            {
                while (x != z)
                {
                    x++;
                    string strX = x.ToString();
                    p.Message(strX);
                    if (x == z)
                    {
                        p.Message("Max limit reached!");
                        x = 0;
                        p.Message("Do the command again to restart!");
                        return;
                    }
                }
            }
        }
        public override void Help(Player p)
        {
            p.Message("&T/UnsignedShortCounter [number] &H- Counts to the unsigned number provided using ushort instead of byte.");
            p.Message("&HNumber must be less than {0} and greater than {1}.", ushort.MaxValue, ushort.MinValue);
        }
    }
}