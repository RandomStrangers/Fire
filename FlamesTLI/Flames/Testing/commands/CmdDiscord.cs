namespace Flames.Commands
{
    public class CmdDiscord : Command2
    {
        public override string name { get { return "Discord"; } }
        public override string type { get { return CommandTypes.Added; } }
        public override CommandPerm[] ExtraPerms
        {
            get { return new[] { new CommandPerm(LevelPermission.Builder, "can send discord link to others") }; }
        }
        public override void Use(Player p, string message, CommandData data)
        {
            Player target = p;
            if (message.Length > 0)
            {
                if (!CheckExtraPerm(p, data, 1)) return;
                target = PlayerInfo.FindMatches(p, message);
                if (target == null) return;
            }
            target?.Message("&dDiscord: https://discord.gg/BJ6WZHEzX5");
            if (target != null && p != target)
            {
                p.Message("Sent the discord link to {0}&S.", p.FormatNick(target));
                target.Message("{0} &Ssent you the discord link.", target.FormatNick(p));
            }
        }

        public override void Help(Player p)
        {
            if (HasExtraPerm(p, p.Rank, 1))
            {
                p.Message("&T/Discord [player] &H- Displays server discord link to [player]");
            }
            p.Message("&T/Discord &H- Displays the server discord link to you");
        }
    }
}