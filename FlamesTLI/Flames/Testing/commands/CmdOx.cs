// This time I properly credited GoldenSparks
namespace Flames.Commands
{
    public class CmdOX : Command
    {
        public override string name { get { return "Oxidation"; } }
        public override string shortcut { get { return "ox"; } }
        public override string type { get { return CommandTypes.Added; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public override void Use(Player p, string message)
        {
            bool messageEmpty = string.IsNullOrEmpty(message);
            Find("say").Use(Player.Flame, "&9THIS CALLS FOR OXIDATION AND DREAM MONSTERS!!!");
            Find("say").Use(Player.Flame, "&fhttps://www.youtube.com/watch?v=5vnHaVjP33M");
            Find("say").Use(Player.Flame, "(from " + p.truename + ")");
            if (!messageEmpty)
            {
                Find("say").Use(Player.Flame, "&7-------------------");
                Find("say").Use(Player.Flame, "&7" + p.truename + " says: " + message);
            }
        }
        public override void Help(Player p)
        {
            p.Message("&T/Oxidation (message, optional) &H- &9THIS CALLS FOR A Oxidation and Dream Monsters | GHOST Reupload!!!");
            p.Message("&7-- cmd made by koy (Lexi) with the assistance of GoldenSparks --");
        }
    }
}