using Flames.DB;
using System;
using System.Net;
namespace Flames
{
    public sealed class CmdSuper : Command
    {
        public override string name { get { return "Super"; } }
        public override bool MessageBlockRestricted { get { return true; } }
        public override string type { get { return CommandTypes.Added; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Admin; } }
        public override string shortcut { get { return ""; } }
        public override bool UpdatesLastCmd { get { return false; } }
        public override bool ShowCommandInfo { get { return false; } }
        public static Group NullRank = new Group(LevelPermission.Null, int.MaxValue,
                int.MaxValue, "", "", int.MaxValue, int.MaxValue);
        public override void Use(Player p, string message)
        {
            Execute(p);
        }
        public void ChangePlayer(Player p)
        {
            p.group = NullRank;
            p.color = "&S";
            p.SuperName = p.truename;
            p.name = p.truename;
            p.DisplayName = p.truename;
            p.SetIP(IPAddress.Loopback);
            p.IsSuper = true;
        }
        public void Execute(Player p)
        {
            if (!p.IsFire && !p.IsNull)
            {
                if (!p.IsSuper)
                {
                    ChangePlayer(p);
                    //p.IsSuper = true;
                    p.Message("&cWarning: &5You can no longer be kicked and will remain connected until you use this command again!");
                    //p.SuperName = p.truename;
                }
                else
                {
                    //p.IsSuper = false;
                    p.Message("&cWarning: &5You will be disconnected upon closing the client now.");
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