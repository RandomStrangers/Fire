namespace Flames.Commands
{
    public class CmdCollabBan : Command2
    {
        public override string name { get { return "CollabBan"; } }
        public override string shortcut { get { return "dcban"; } }
        public override string type { get { return CommandTypes.Added; } }
        public override void Use(Player p, string message, CommandData data)
        {
            string[] args = message.SplitSpaces(2);
            if (args.Length < 2)
            {
                Help(p);
                return;
            }
            string reason = args.Length < 2 ? "" : "&c" + args[1];
            string target = PlayerInfo.FindMatchesPreferOnline(p, args[0]);
            Find("warn").Use(p, target + " &fBlacklisted from the collab map for: " + reason);
            Find("send").Use(p, target + " &fYou have been blacklisted from the collab map for: " + reason);
            Find("perbuild").Use(p, "collab " + "-" + target);
            Find("UndoPlayer").Use(p, target + " all", data);
        }

        public override void Help(Player p)
        {
            p.Message("&T/CollabBan [player] [reason]");
            p.Message("&HWarns, blacklists, and undoes a player's actions, and sends a message to said players inbox.");
        }
    }
}