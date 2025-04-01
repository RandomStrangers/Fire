using Flames.Events.PlayerEvents;
using System.IO;
namespace Flames
{
    public class Protocol : Plugin_Simple
    {
        public override string creator { get { return Colors.Strip(Server.Config.Name); } }
        public override string Flames_Version { get { return Server.Version; } }
        public override string name { get { return "Protocol 1"; } }
        public override string Name { get { return "Protocol 1"; } }

        public override void Load(bool startup)
        {
            Command.Register(new CmdProtocol1());
            OnPlayerFinishConnectingEvent.Register(Protocol1, Priority.High);
        }

        public override void Unload(bool shutdown)
        {
            Command.Unregister(Command.Find("Protocol1"));
            OnPlayerFinishConnectingEvent.Unregister(Protocol1);
        }

        void Protocol1(Player p)
        {
            if (File.Exists("override"))
            {
                p.Leave("Protocol 1 engaged.", true);
                p.cancelconnecting = true;
            }
            else
            {
            }
        }
    }

    public class CmdProtocol1 : Command
    {
        public override string name { get { return "Protocol1"; } }
        public override string type { get { return "other"; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Owner; } }

        public override void Use(Player p, string message)
        {

            if (message.CaselessContains("true"))
            {
                Player[] players = PlayerInfo.Online.Items;
                foreach (Player pl in players)
                {
                    pl.Leave("Protocol 1 engaged.", true);
                }
                Logger.Log(LogType.FlameMessage, "Protocol 1: Protect the network.");
                p.Message("Protocol 1 engaged.");
                if (!File.Exists("override"))
                {
                    File.Create("override");
                }
                return;
            }
            else if (message.CaselessContains("false"))
            {
                p.Message("Protocol 1 disengaged.");
                if (File.Exists("override"))
                {
                    AtomicIO.TryDelete("override");
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
            p.Message("&T/Protocol1 True &H- Engages Protocol 1.");
            p.Message("&T/Protocol1 False &H- Disengages Protocol 1.");
        }
    }
}
