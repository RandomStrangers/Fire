namespace Flames 
{
	public class CmdFreebuildBan : Command2 
    {
		public override string name { get { return "FreebuildBan"; } }
        public override string shortcut { get { return "fbban"; } }
		public override string type { get { return CommandTypes.Added; } }
		public override void Use(Player p, string message, CommandData data) 
        {
            string[] args = message.SplitSpaces(2);
            if (args.Length < 2) 
            { 
                Help(p); 
                return; 
            }
        	string reason = args.Length < 2 ? "" :  "&c" + args[1];
        	string target = PlayerInfo.FindMatchesPreferOnline(p, args[0]);
  		Command.Find("warn").Use(p, target + " &fBlacklisted from the freebuilds for: " + reason);
		Command.Find("send").Use(p, target + " &fYou have been blacklisted from the freebuilds for: " + reason);
        Command.Find("perbuild").Use(p, "fb " + "-" + target);
        Command.Find("perbuild").Use(p, "fb_flat " + "-" + target);
        Command.Find("UndoPlayer").Use(p, target + " all", data);
        }
		
       public override void Help(Player p) 
       {
            p.Message("&T/FreebuildBan [player] [reason]");
           	p.Message("&HWarns, blacklists, and undoes a player's actions, and sends a message to said players inbox.");
       }
	}
}