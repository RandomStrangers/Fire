using System.IO;
using Flames.Events.PlayerEvents;
namespace Flames
{
    public class Kick : Plugin
    {
        public override string creator { get { return "HarmonyNetwork"; } }
        public override string Flames_Version { get { return Server.Version; } }
        public override string name { get { return "KickPlayers"; } }
        public Command NoFabricCmd = new CmdNoFabric();
        public Command DevOnlyCmd = new CmdDevOnly();
        public override void Load(bool startup)
        {
            Command.Register(NoFabricCmd, DevOnlyCmd);
            OnPlayerFinishConnectingEvent.Register(KickPlayer, Priority.High);
        }

        public override void Unload(bool shutdown)
        {
            Command.Unregister(Command.Find("DevOnly"), Command.Find("NoFabric"));
            OnPlayerFinishConnectingEvent.Unregister(KickPlayer);
        }
        void KickPlayer(Player p)
        {
            string clientName = p.Session.ClientName();
            if (!PlayerInfo.IsDev(p) && File.Exists("Dev-Only.lock"))
            {
                p.Leave("Error contacting web server: 403", true);
                p.cancelconnecting = true;
            }
            if (clientName.CaselessContains("Fabric") && File.Exists("properties/no-fabric.txt"))
            {
                string Message = Colors.Strip(clientName) + " is not allowed here!";
                p.Leave(Message, true);
                p.cancelconnecting = true;
            }
        }
    }
    public class CmdDevOnly : Command
    {
        public override string name { get { return "DevOnly"; } }
        public override string type { get { return CommandTypes.Added; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Owner; } }

        public override void Use(Player p, string message)
        {

            if (message.CaselessContains("true"))
            {
                p.Message("Server is now available to developers only!");
                if (!File.Exists("Dev-Only.lock"))
                {
                    File.Create("Dev-Only.lock");
                }
                return;
            }
            else if (message.CaselessContains("false"))
            {
                p.Message("Server has been opened to public!");
                if (File.Exists("Dev-Only.lock"))
                {
                    AtomicIO.TryDelete("Dev-Only.lock");
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
            p.Message("&T/DevOnly True &H- Makes the server available only to developers of {0}.", Server.SoftwareName);
            p.Message("&T/DevOnly False &H- Makes the server open to all players.");
        }
    }
    public class CmdNoFabric : Command
    {
        public override string name { get { return "NoFabric"; } }
        public override string type { get { return CommandTypes.Added; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Owner; } }

        public override void Use(Player p, string message)
        {

            if (message.CaselessContains("true"))
            {
                p.Message("Clients containing \"Fabric\" can no longer join!");
                if (!File.Exists("properties/no-fabric.txt"))
                {
                    File.Create("properties/no-fabric.txt");
                }
                return;
            }
            else if (message.CaselessContains("false"))
            {
                p.Message("Clients containing \"Fabric\" can now join!");
                if (File.Exists("properties/no-fabric.txt"))
                {
                    AtomicIO.TryDelete("properties/no-fabric.txt");
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
            p.Message("&T/NoFabric True &H- Disallows clients containing \"Fabric\" from joining.");
            p.Message("&H/NoFabric False &H- Allows clients containing \"Fabric\" to join.");
        }
    }
}