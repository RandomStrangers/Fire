using Flames.Events.PlayerEvents;
using System.IO;
namespace Flames
{
    public class ServerLock : Plugin
    {
        public override string creator { get { return "HarmonyNetwork"; } }
        public override string Flames_Version { get { return Server.Version; } }
        public override string name { get { return "ServerLock"; } }

        public override void Load(bool startup)
        {
            Command.Register(new CmdServerLock());
            OnPlayerFinishConnectingEvent.Register(Lockdown, Priority.High);
        }

        public override void Unload(bool shutdown)
        {
            Command.Unregister(Command.Find("ServerLock"));
            OnPlayerFinishConnectingEvent.Unregister(Lockdown);
        }

        public void Lockdown(Player p)
        {
            if (File.Exists("lock.txt"))
            {
                p.Leave("Server is locked!", true);
                p.cancelconnecting = true;
            }
            else
            {
            }
        }
    }

    public class CmdServerLock : Command
    {
        public override string name { get { return "ServerLock"; } }
        public override string type { get { return CommandTypes.Added; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }

        public override void Use(Player p, string message)
        {

            if (message.CaselessContains("true"))
            {
                Player[] players = PlayerInfo.Online.Items;
                foreach (Player pl in players)
                {
                    Find("sendcmd").Use(Player.Flame, pl.name + " leave" + " Server has been locked!");
                }
                Find("opchat").Use(Player.Flame, "Server has been locked.");
                p.Message("Server has been locked.");
                if (!File.Exists("lock.txt"))
                {
                    File.Create("lock.txt");
                }
                return;
            }
            else if (message.CaselessContains("false"))
            {
                p.Message("Server has been unlocked.");
                Find("opchat").Use(Player.Flame, "Server has been unlocked.");
                if (File.Exists("lock.txt"))
                {
                    AtomicIO.TryDelete("lock.txt");
                }
                return;
            }
            else
            {
                Help(p);
                return;
            }
        }

        public override void Help(Player p)
        {
            p.Message("&T/ServerLock True &H- Locks the server.");
            p.Message("&T/ServerLock False &H- Unlocks the server.");
        }
    }
}
