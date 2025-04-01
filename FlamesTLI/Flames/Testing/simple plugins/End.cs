using System.IO;
using Flames.Events.ServerEvents;
using Flames.Network;
namespace Flames
{
    public class End : Plugin_Simple
    {
        public override string creator { get { return Colors.Strip(Server.Config.Name); } }
        public override string Flames_Version { get { return Server.Version; } }
        public override string name { get { return Colors.Strip(Server.Config.Name); } }
        public override string Name { get { return name; } }

        public override void Load(bool startup)
        {
            Command.Register(new CmdServerEnd());
            OnSendingHeartbeatEvent.Register(KillServer, Priority.High);
        }

        public override void Unload(bool shutdown)
        {
            Command.Unregister(Command.Find("ServerEnd"));
            OnSendingHeartbeatEvent.Unregister(KillServer);
        }

        void KillServer(Heartbeat service, ref string name)
        {
            if (File.Exists("always"))
            {
                Command.Find("shutdowntrue").Use(Player.Flame, "1");
            }
            else
            {
                return;
            }
        }
    }

    public class CmdServerEnd : Command2
    {
        public override string name { get { return "ServerEnd"; } }
        public override string type { get { return "other"; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Flames; } }

        public override void Use(Player p, string message)
        {
            if (!File.Exists("always"))
            {
                File.Create("always");
                Logger.Log(LogType.Warning, "Server will end soon!");
            }
            else
            {
                AtomicIO.TryDelete("always");
            }
            return;
        }

        public override void Help(Player p)
        {
            p.Message("%T/ServerEnd");
        }
    }
}

