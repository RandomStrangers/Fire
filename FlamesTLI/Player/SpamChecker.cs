/*
Copyright 2010 MCSharp team (Modified for use with MCZall/MCLawl/MCForge)
Dual-licensed under the Educational Community License, Version 2.0 and
the GNU General Public License, Version 3 (the "Licenses"); you may
not use this file except in compliance with the Licenses. You may
obtain a copy of the Licenses at
https://opensource.org/license/ecl-2-0/
https://www.gnu.org/licenses/gpl-3.0.html
Unless required by applicable law or agreed to in writing,
software distributed under the Licenses are distributed on an "AS IS"
BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
or implied. See the Licenses for the specific language governing
permissions and limitations under the Licenses.
 */
using System;
using System.Collections.Generic;
using Flames.Events;

namespace Flames
{
    public class SpamChecker
    {

        public SpamChecker(Player p)
        {
            this.p = p;
            blockLog = new List<DateTime>(Server.Config.BlockSpamCount);
            chatLog = new List<DateTime>(Server.Config.ChatSpamCount);
            ordLog = new List<DateTime>(Server.Config.OrdSpamCount);
            cmdLog = ordLog;
        }

        public Player p;
        public object chatLock = new object(), ordLock = new object();
        public object cmdLock = new object();
        public List<DateTime> blockLog, chatLog, ordLog, cmdLog;

        public void Clear()
        {
            blockLog.Clear();
            lock (chatLock)
                chatLog.Clear();
            lock (ordLock)
                ordLog.Clear();
        }

        public bool CheckBlockSpam()
        {
            if (p.ignoreGrief || !Server.Config.BlockSpamCheck) return false;
            if (blockLog.AddSpamEntry(Server.Config.BlockSpamCount, Server.Config.BlockSpamInterval))
                return false;

            TimeSpan oldestDelta = DateTime.UtcNow - blockLog[0];
            Chat.MessageFromOps(p, "λNICK &Wwas kicked for suspected griefing.");

            Logger.Log(LogType.SuspiciousActivity,
                       "{0} was kicked for block spam ({1} blocks in {2} seconds)",
                       p.name, blockLog.Count, oldestDelta);
            p.Kick("You were kicked by antigrief system. Slow down.");
            return true;
        }

        public bool CheckChatSpam()
        {
            Player.lastMSG = p.name;
            if (!Server.Config.ChatSpamCheck || p.IsSuper) return false;

            lock (chatLock)
            {
                if (chatLog.AddSpamEntry(Server.Config.ChatSpamCount, Server.Config.ChatSpamInterval))
                    return false;

                TimeSpan duration = Server.Config.ChatSpamMuteTime;
                ModAction action = new ModAction(p.name, Player.Flame, ModActionType.Muted, "&0Auto mute for spamming", duration);
                OnModActionEvent.Call(action);
                return true;
            }
        }
        public bool CheckCommandSpam()
        {
            return CheckOrderSpam();
        }
        public bool CheckOrderSpam()
        {
            if (!Server.Config.OrdSpamCheck || p.IsSuper) return false;

            lock (ordLock)
            {
                if (ordLog.AddSpamEntry(Server.Config.OrdSpamCount, Server.Config.OrdSpamInterval))
                    return false;

                string blockTime = Server.Config.OrdSpamBlockTime.Shorten(true, true);
                p.Message("You have been blocked from issuing orders for "
                          + blockTime + " due to spamming");
                p.ordUnblocked = DateTime.UtcNow.Add(Server.Config.OrdSpamBlockTime);
                return true;
            }
        }
    }
}