namespace Flames.Commands
{
    public class CmdCounter : Command
    {
        public override string name { get { return "Counter"; } }
        public override string shortcut { get { return "Count"; } }
        public override string type { get { return CommandTypes.Added; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Guest; } }
        public static sbyte x;
        public override void Use(Player p, string message)
        {
            bool MessageEmpty = string.IsNullOrEmpty(message);
            bool NegativeNumber = message.CaselessContains("-");
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
            bool Success = sbyte.TryParse(message, out sbyte z);
            bool TooHigh = z > sbyte.MaxValue;
            if (TooHigh)
            {
                p.Message("The requested end value is too high! Please choose a value less than {0}.", sbyte.MaxValue);
                return;
            }
            bool TooLow = z < sbyte.MinValue;
            if (TooLow)
            {
                p.Message("The requested end value is too low! Please choose a value greater than {0}.", sbyte.MinValue);
                return;
            }
            if (!Success)
            {
                Help(p);
                return;
            }
            if (Success)
            {
                if (NegativeNumber)
                {
                    while (x != z)
                    {
                        x--;
                        string strX = x.ToString();
                        p.Message(strX);
                        if (x == z)
                        {
                            p.Message("Min limit reached!");
                            x = 0;
                            p.Message("Do the command again to restart!");
                            return;
                        }
                    }
                }
                else
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
        }
        public override void Help(Player p)
        {
            p.Message("&T/Counter [number] &H- Counts to the number provided.");
            p.Message("&HNumber must be less than {0} and greater than {1}.", sbyte.MaxValue, sbyte.MinValue);
        }
    }
}