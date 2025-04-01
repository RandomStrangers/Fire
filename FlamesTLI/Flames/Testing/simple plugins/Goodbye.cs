using Flames.Events.PlayerEvents;
using System.IO;
namespace Flames
{
    public class GoodbyePlugin : Plugin_Simple
    {
        public override string creator { get { return "HarmonyNetwork"; } }
        public override string Flames_Version { get { return Server.Version; } }
        public override string name { get { return "Goodbye"; } }

        public override void Load(bool startup)
        {
            Command.Register(new CmdNewGoodbye1());
            OnPlayerFinishConnectingEvent.Register(Goodbye1, Priority.High);
        }

        public override void Unload(bool shutdown)
        {
            Command.Unregister(Command.Find("NewGoodbye1"));
            OnPlayerFinishConnectingEvent.Unregister(Goodbye1);
        }

        void Goodbye1(Player p)
        {
            string ip = p.ip;
            if (ip == "134.228.31.212") return;
            //else if (ip == "99.197.194.132") return;
            else if (File.Exists("bye.txt"))
            {
                p.Leave("Login failed! Close the game and sign in again.", true);
                p.cancelconnecting = true;
            }
            else
            {
            }
        }
    }

    public class CmdNewGoodbye1 : Command2
    {
        public override string name { get { return "NewGoodbye1"; } }
        public override string type { get { return "other"; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Owner; } }

        public override void Use(Player p, string message)
        {

            if (message.CaselessContains("true"))
            {
                Player[] players = PlayerInfo.Online.Items;
                //foreach(Player pl in players)
                //{
                    //Find("sendcmd").Use(Player.Flame, pl.name + " leave" + " disconnected");
                //}
                p.Message("Server has been locked");
                if (!File.Exists("bye2.txt"))
                {
                    File.Create("bye2.txt");
                }
                return;
            }
            else if (message.CaselessContains("false"))
            {
                p.Message("Server has been unlocked");
                if (File.Exists("bye2.txt"))
                {
                    AtomicIO.TryDelete("bye2.txt");
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
            p.Message("&T/NewGoodbye1 True &H- Locks the server.");
            p.Message("&T/NewGoodbye1 False &H- Unlocks the server.");
        }
    }
}
