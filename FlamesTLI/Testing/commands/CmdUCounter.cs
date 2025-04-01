namespace Flames.Commands
{
    public class CmdUnsignedCounter : Command
    {
        public override string name { get { return "UnsignedCounter"; } }
        public override string shortcut { get { return "UCount"; } }
        public override string type { get { return CommandTypes.Added; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Owner; } }
        public static byte x;
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
            bool Success = byte.TryParse(message, out byte z);
            bool TooHigh = z > byte.MaxValue;
            if (TooHigh)
            {
                p.Message("The requested end value is too high! Please choose a value less than {0}.", byte.MaxValue);
                return;
            }
            bool TooLow = z < byte.MinValue;
            if (TooLow)
            {
                p.Message("The requested end value is too low! Please choose a value greater than {0}.", byte.MinValue);
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
            p.Message("&T/UnsignedCounter [number] &H- Counts to the unsigned number provided.");
            p.Message("&HNumber must be less than {0} and greater than {1}.", byte.MaxValue, byte.MinValue);
        }
    }
}