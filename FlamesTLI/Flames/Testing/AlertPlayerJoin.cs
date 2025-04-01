using System.Media;
using Flames.Events.PlayerEvents;

namespace Flames
{
    public class AlertPlayerJoin : Plugin
    {
        public override string creator { get { return "HarmonyNetwork"; } }
        public override string name { get { return "AlertPlayerJoin"; } }
        public override void Load(bool startup)
        {
            Command.Register(new CmdCommand());
            OnPlayerFinishConnectingEvent.Register(AlertJoin, Priority.Critical);
        }

        public override void Unload(bool shutdown)
        {
            Command.Unregister(Command.Find("Command"));
            OnPlayerFinishConnectingEvent.Unregister(AlertJoin);
        }
        public void AlertJoin(Player p)
        {
            Command.Find("Command").Use(p, "");
            SystemSounds.Beep.Play();
        }
    }
    public class CmdCommand : Command
    {
        public override string name { get { return "Command"; } }
        public override string shortcut { get { return "cmd"; } }
        public override bool UpdatesLastCmd {  get { return false; } }
        public override string type { get { return CommandTypes.Added; } }
        public override bool ShowCommandInfo { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Guest; } }
        public override void Use(Player p, string message)
        {
            return;
        }

        public override void Help(Player p)
        {
            return;
        }
    }
}