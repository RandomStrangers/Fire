namespace Flames.Commands
{
    public class CmdLongCounter : Command
    {
        public override string name { get { return "LongCounter"; } }
        public override string shortcut { get { return "LCount"; } }
        public override string type { get { return CommandTypes.Added; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Owner; } }
        public static long x;
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
            bool Success = long.TryParse(message, out long z);
            bool TooHigh = z > long.MaxValue;
            if (TooHigh)
            {
                p.Message("The requested end value is too high! Please choose a value less than {0}.", int.MaxValue);
                return;
            }
            bool TooLow = z < long.MinValue;
            if (TooLow)
            {
                p.Message("The requested end value is too low! Please choose a value greater than {0}.", long.MinValue);
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
            p.Message("&T/LongCounter [number] &H- Counts to the number provided using long instead of sbyte.");
            p.Message("&HNumber must be less than {0} and greater than {1}.", long.MaxValue, long.MinValue);
        }
    }
}