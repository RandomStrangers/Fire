namespace Flames.Commands
{
    public class CmdRealBanall : Command
    {
        public override string name { get { return "RealBanall"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return CommandTypes.Added; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Owner; } }
        public override void Use(Player p, string message)
        {
            Player[] onlineUsers = PlayerInfo.Online.Items;
            foreach (Player user in onlineUsers)
            {
                Find("tempban").Use(Player.Flame, user.name + " 1d You know why...");
                Find("kick").Use(Player.Flame, user.name + " Pretend you know why.");
            }
        }
        public override void Help(Player p)
        {
            p.Message("&T/RealBanAll &H- Actually bans all online players for one day.");
        }
    }
}