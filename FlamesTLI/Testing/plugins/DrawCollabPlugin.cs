using Flames.Events.PlayerEvents;
namespace Flames
{
    public class DrawCollabPlugin : Plugin
    {
        public override string name { get { return "DrawCollabPlugin"; } }

        public override void Load(bool startup)
        {
            Command.Register(new CmdDrawCollab());
            OnPlayerCommandEvent.Register(HandleCommand, Priority.Critical);
            OnGettingMotdEvent.Register(HandleMotd, Priority.Critical);
        }

        public override void Unload(bool shutdown)
        {
            Command.Unregister(Command.Find("DrawCollab"));
            OnPlayerCommandEvent.Unregister(HandleCommand);
            OnGettingMotdEvent.Unregister(HandleMotd);

        }
        public static void HandleCommand(Player p, string cmd, string args, CommandData data)
        {
            Command command = Command.Find(cmd);
            if (command != null && p != null)
            {
                Level lvl = p.level;
                bool IsPaintCmd = command.name.CaselessContains("paint");

                if (IsPaintCmd && lvl.Config.MOTD.CaselessContains("+collab"))
                {
                    p.Message("Cannot disable painting mode on this map.");
                    p.cancelcommand = true;
                }
            }
        }
        public void HandleMotd(Player p, ref string motd)
        {
            Level lvl = p.level;
            if (lvl.Config.MOTD.CaselessContains("+collab"))
            {
                p.painting = true;
                p.Message("Painting mode: &aON&S, use /paint to disable.");
            }
        }
    }
    public class CmdDrawCollab : Command
    {
        public override string name { get { return "DrawCollab"; } }
        public override string shortcut { get { return "DC"; } }
        public override string type { get { return CommandTypes.Added; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Owner; } }
        public override bool MessageBlockRestricted { get { return true; } }
        public override bool UseableWhenFrozen { get { return true; } }
        public override void Use(Player p, string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                Help(p);
                return;
            }
            Level lvl;
            string[] args = message.SplitSpaces();
            if (args.Length == 1)
            {
                lvl = p.Level;
            }
            else
            {
                lvl = Matcher.FindLevels(p, args[1]);
            }
            if (p.IsSuper) 
            { 
                SuperRequiresArgs(p, "level name"); 
                return; 
            }
            string config = args[0];
            if (config.CaselessEq("true"))
            {
                lvl.Config.MOTD += "+collab";
                p.Message("Added +collab to MOTD.");
            }
            else if (config.CaselessEq("false"))
            {
                if (lvl.Config.MOTD.CaselessContains("+collab"))
                {
                    lvl.Config.MOTD.Replace("+collab", "");
                }
            }
            else
            {
                Help(p);
            }
        }
        public override void Help(Player p)
        {
            p.Message("&T/DrawCollab true [level] &H- Appends +collab to the provided level's MOTD.");
            p.Message("&T/DrawCollab false [level] &H- Removes +collab from the provided level's MOTD.");
            p.Message("If no level is provided, uses current level.");
        }
    }
}