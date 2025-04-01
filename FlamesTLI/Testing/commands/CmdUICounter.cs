namespace Flames.Commands
{
    public class CmdUnsignedIntCounter : Command
    {
        public override string name { get { return "UnsignedIntCounter"; } }
        public override string shortcut { get { return "UICount"; } }
        public override string type { get { return CommandTypes.Added; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Owner; } }
        public static uint x;
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
            bool Success = uint.TryParse(message, out uint z);
            bool TooHigh = z > uint.MaxValue;
            if (TooHigh)
            {
                p.Message("The requested end value is too high! Please choose a value less than {0}.", uint.MaxValue);
                return;
            }
            bool TooLow = z < uint.MinValue;
            if (TooLow)
            {
                p.Message("The requested end value is too low! Please choose a value greater than {0}.", uint.MinValue);
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
            p.Message("&T/UnsignedIntCounter [number] &H- Counts to the unsigned number provided using uint instead of byte.");
            p.Message("&HNumber must be less than {0} and greater than {1}.", uint.MaxValue, uint.MinValue);
        }
    }
}