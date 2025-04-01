using System;
using Flames.Tasks;
using Flames.Events.PlayerEvents;
namespace Flames
{
    public class NoAFK : Plugin
    {
        public override string name { get { return "NoAFK"; } }
        public override string creator { get { return "HarmonyNetwork"; } }
        public static SchedulerTask UnAFKTask;
        public override void Load(bool startup)
        {
            //Command.Register(new CmdNotAFK());
            UnAFKTask = Server.Background.QueueRepeat(UnAFK, null, TimeSpan.FromSeconds(660));
        }
        public void UnAFK(SchedulerTask task)
        {
            Player[] players = PlayerInfo.Online.Items;
            foreach (Player p2 in players)
            {
                string PlayerName = p2.truename;
                if (PlayerName.CaselessEq("HarmonyNetwork"))
                {
                    if (p2.IsAfk)
                    {
                        CmdNotAFK.ToggleNotAFK(p2);
                        p2.AutoAfk = false;
                        p2.IsAfk = false;
                        //Logger.Log(LogType.SystemActivity, "{0} is no longer AFK.", p2.truename);
                    }
                }
            }
        }
        public override void Unload(bool shutdown)
        {
            //Command.Unregister(Command.Find("NotAFK"));
            Server.Background.Cancel(UnAFKTask);
        }
        public override void Help(Player p)
        {
            p.Message("");
        }
    }

    public class CmdNotAFK : Command
    {
        public override string name { get { return "NotAFK"; } }
        public override string type { get { return CommandTypes.Information; } }
        public override bool SuperUseable { get { return false; } }
        public override bool MessageBlockRestricted { get { return true; } }
        public override LevelPermission defaultRank { get { return DefaultRank; } }
        public virtual LevelPermission DefaultRank { get { return LevelPermission.Flames; } }
        public override bool UseableWhenFrozen { get { return true; } }

        public override void Use(Player p, string message)
        {
            ToggleNotAFK(p);
        }
        public static void ToggleNotAFK(Player p)
        {
            p.AutoAfk = false;
            p.IsAfk = false;
            p.afkMessage = null;
            TabList.Update(p, true);
            p.LastAction = DateTime.UtcNow;

            bool cantSend = !p.CanSpeak();
            if (cantSend)
            {
                p.Message("You are no longer marked as being AFK.");
            }
            else
            {
                p.CheckForMessageSpam();
            }
            OnPlayerActionEvent.Call(p, PlayerAction.UnAFK, null, cantSend);
        }

        public override void Help(Player p)
        {
            p.Message("&T/NotAFK");
            p.Message("&HMarks you as not AFK.");
        }
    }
}