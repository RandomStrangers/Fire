using Flames.Events.PlayerEvents;

namespace Flames.Commands
{
    public class CmdMapHack : Command2
    {

        public override string name { get { return "MapHack"; } }
        public override string type { get { return CommandTypes.Added; } }
        public override CommandPerm[] ExtraPerms
        {
            get { return new[] { new CommandPerm(LevelPermission.Operator, "can bypass hack restrictions on all maps") }; }
        }

        public static bool hooked;
        public const string ext_allowed_key = "__MAPHACK_ALLOWED";

        public void EnableHacksBypass(Player p)
        {
            p.Extras[ext_allowed_key] = true;
            p.SendMapMotd();
            p.Message("&aYou are now bypassing hacks restrictions on this map");
        }

        public void DisableHacksBypass(Player p)
        {
            p.Extras[ext_allowed_key] = false;
            p.SendMapMotd();
            p.Message("&HHacks bypassing reset, use &T/MapHack &Hto turn on again");
        }

        public override void Use(Player p, string message, CommandData data)
        {
            if (message.Length == 0)
            {
                if (!hooked)
                { // not thread-safe but meh
                    OnSentMapEvent.Register(HandleOnSentMap, Priority.High);
                    OnGettingMotdEvent.Register(HandleGettingMotd, Priority.High);
                    hooked = true;
                }

                if (LevelInfo.IsRealmOwner(p.name, p.level.MapName) || CheckExtraPerm(p, data, 1))
                {
                    EnableHacksBypass(p);
                }
                else
                {
                    p.Message("&cYou can only bypass hacks on your own realms.");
                }
            }
            else if (message.CaselessEq("stop"))
            {
                DisableHacksBypass(p);
            }
            else
            {
                Help(p);
            }
        }

        public void HandleOnSentMap(Player p, Level prevLevel, Level level)
        {
            if (!p.Extras.GetBoolean(ext_allowed_key)) return;
            // disable /maphack when you reload or change maps
            DisableHacksBypass(p);
        }

        public void HandleGettingMotd(Player p, ref string motd)
        {
            if (!p.Extras.GetBoolean(ext_allowed_key)) return;
            motd = "+hax";
        }

        public override void Help(Player p)
        {
            p.Message("&T/MapHack &H- Lets you bypass hacks restrictions on your own map (e.g. for when making a parkour map with -hax on)");
            p.Message("&T/MapHack stop &H- Enables hacks restrictions again");
        }
    }
}
