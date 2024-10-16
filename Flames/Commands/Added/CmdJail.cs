/*
    Copyright 2011 MCForge
    
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
using Flames.Events;
using System;

namespace Flames.Commands.Moderation
{
    public sealed class CmdJail : Command2
    {
        public override string name { get { return "Jail"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return CommandTypes.Added; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public override void Use(Player p, string message, CommandData data)
        {
            if (message.Length == 0) 
            { 
                Help(p); 
                return; 
            }
            string[] args = message.SplitSpaces(2);
            string target = PlayerInfo.FindMatchesPreferOnline(p, args[0]);
            if (target == null) return;
            Group group = ModActionCmd.CheckTarget(p, data, "jail", target);
            if (group == null) return;
            if (Server.jailed.Contains(target))
            {
                DoUnjail(p, target, args);
            }
            else
            {
                // unjail has second argument as reason, jail has third argument instead
                DoJail(p, target, message.SplitSpaces(3));
            }
        }
        public void DoJail(Player p, string target, string[] args)
        {
            if (args.Length < 2) 
            { 
                Help(p); 
                return; 
            }
            TimeSpan duration = TimeSpan.Zero;
            if (!CommandParser.GetTimespan(p, args[1], ref duration, "jail for", "m")) return;
            string reason = args.Length > 2 ? args[2] : "";
            reason = ModActionCmd.ExpandReason(p, reason);
            if (reason == null) return;
            ModAction action = new ModAction(target, p, ModActionType.Jailed, reason, duration);
            OnModActionEvent.Call(action);
        }
        public void DoUnjail(Player p, string target, string[] args)
        {
            string reason = args.Length > 1 ? args[1] : "";
            reason = ModActionCmd.ExpandReason(p, reason);
            if (reason == null) return;
            ModAction action = new ModAction(target, p, ModActionType.Unjailed, reason);
            OnModActionEvent.Call(action);
        }
        public override void Help(Player p)
        {
            p.Message("&T/Jail [name] [timespan] <reason>");
            p.Message("&HPrevents [name] from moving for [timespan], or until manually unjailed.");
            p.Message("&HFor <reason>, @number can be used as a shortcut for that rule.");
        }
    }
}