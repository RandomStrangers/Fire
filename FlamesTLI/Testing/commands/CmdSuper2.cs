//reference System.dll
using Flames.DB;
using System.Net;
namespace Flames
{
    public class NewPlayer : Player
    {
        public static Group NullRank = new Group(LevelPermission.Null, int.MaxValue,
                int.MaxValue, "Wouldn't you like to know?", "&S", int.MaxValue, int.MaxValue);
        public NewPlayer(Player p) : base(p.truename)
        {
            SuperName = p.truename;
            DisplayName = p.DisplayName;
            truename = p.truename;
            group = NullRank;
            SetIP(IPAddress.Loopback);
            p.color = "&S";
            p.IsSuper = true;
        }
    }
    public class CmdSuper2 : Command
    {
        public override string name { get { return "Super2"; } }
        public override bool MessageBlockRestricted { get { return true; } }
        public override string type { get { return CommandTypes.Added; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Admin; } }
        public override string shortcut { get { return ""; } }
        public override bool UpdatesLastCmd { get { return false; } }
        public override bool ShowCommandInfo { get { return false; } }
        public static bool ChangeMe = false;
        public static Group PlayerGroup;
        public static IPAddress PlayerIP;

        public override void Use(Player p, string message)
        {
            Execute(p);
        }
        public void ChangePlayer(Player p)
        {
            if (ChangeMe)
            {
                /*p.group = NewPlayer.NullRank;
                p.color = "&S";
                p.SuperName = p.truename;
                p.SetIP(IPAddress.Loopback);
                p.IsSuper = true; */
                p = new NewPlayer(p);
            }
            else
            {
                p = new Player(p.truename);
            }
        }
        public void Execute(Player p)
        {
            if (!p.IsFire && !p.IsNull)
            {
                if (!p.IsSuper)
                {
                    ChangeMe = true;
                    PlayerGroup = p.group;
                    PlayerIP = p.IP;
                    ChangePlayer(p);
                    p.Message("&cWarning: &5You can no longer be kicked and will remain connected until you use this command again!");
                }
                else
                {
                    p.IP = PlayerIP;
                    p.group = PlayerGroup;
                    p.IsSuper = false;
                    p.Message("&cWarning: &5You will be disconnected upon closing the client now.");
                    ChangeMe = false;
                }
            }
        }
        public override void Help(Player p)
        {
            if (p.IsFire)
            {
                p.Message("Does nothing.");
            }
            else if (p.IsSuper)
            {
                p.Message("Does nothing.");
            }
            else
            {
                string msg = PlayerDB.GetLogoutMessage(p.name);
                p.Leave(msg);
            }
        }
    }
}