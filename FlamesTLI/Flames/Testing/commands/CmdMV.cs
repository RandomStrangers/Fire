namespace Flames.Commands
{
    public class CmdMV : Command
    {
        public override string name { get { return "MellifluousVengeance"; } }
        public override string shortcut { get { return "mvenge"; } }
        public override string type { get { return CommandTypes.Added; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public override CommandAlias[] Aliases { get { return new[] { new CommandAlias("mellyvelly"), new CommandAlias("mv") }; } }
        public override void Use(Player p, string message)
        {
            bool messageEmpty = string.IsNullOrEmpty(message);
            Find("say").Use(Player.Flame, "&9THIS CALLS FOR A MELLIFLUOUS VENGEANCE!!!");
            Find("say").Use(Player.Flame, "&fhttps://www.youtube.com/watch?v=HUulEOotqvw");
            Find("say").Use(Player.Flame, "(from " + $"{p.color}{p.DisplayName}" + ")");
            if (!messageEmpty)
            {
                Find("say").Use(Player.Flame, "&7-------------------");
                Find("say").Use(Player.Flame, "&7" + $"{p.color}{p.DisplayName}" + " says: " + message);
            }
        }
        public override void Help(Player p)
        {
            p.Message("&T/MellifluousVengeance (message) &H- &9THIS CALLS FOR A MELLIFLUOUS VENGEANCE!!!");
            p.Message("&7-- cmd made by Vivian3 --");
        }
    }
}