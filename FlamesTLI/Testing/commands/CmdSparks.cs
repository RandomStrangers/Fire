namespace Flames.Commands
{
    public class CmdGoldenSparks : Command
    {
        public override string name { get { return "GoldenSparks"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return CommandTypes.Added; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public override CommandAlias[] Aliases { get { return new[] { new CommandAlias("Sparkie"), new CommandAlias("Sparks") }; } }
        public override void Use(Player p, string message)
        {
            bool messageEmpty = string.IsNullOrEmpty(message);
            Find("say").Use(Player.Flame, "&6S&eO&6M&eE &6B&eO&6T&eS &6A&eR&6E &eJ&6U&eS&6T &eG&6O&eL&6D&e!");
            Find("say").Use(Player.Flame, "&6https://www.youtube.com/watch?v=dfeshGvJhtI");
            if (p == null && !p.IsFire)
            {
                Find("say").Use(Player.Flame, "(from Discord!)");
            }
            else if (p.IsFire)
            {
                Find("say").Use(Player.Flame, "(from the fire!)");
            }
            else
            {
                Find("say").Use(Player.Flame, "(from " + $"{p.color}{p.DisplayName}" + ")");
            }
            if (!messageEmpty)
            {
                Find("say").Use(Player.Flame, "&6-------------------");
                Find("say").Use(Player.Flame, "&6" + $"{p.color}{p.DisplayName}" + " says: " + message);
            }
        }
        public override void Help(Player p)
        {
            p.Message("&T/GoldenSparks [message] &H- &6S&eO&6M&eE &6B&eO&6T&eS &6A&eR&6E &eJ&6U&eS&6T &eG&6O&eL&6D&e!");
            p.Message("&7-- Original cmd made by Vivian3 --");
        }
    }
}