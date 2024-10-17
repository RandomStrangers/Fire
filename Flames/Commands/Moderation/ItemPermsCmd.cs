﻿/*
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

namespace Flames.Commands.Moderation
{
    public abstract class ItemPermsCmd : Command2
    {
        public override string type { get { return CommandTypes.Moderation; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }

        public void SetPerms(Player p, string[] args, CommandData data, ItemPerms perms, string type)
        {
            string grpName = args[1];
            if (!perms.UsableBy(data.Rank))
            {
                p.Message("You rank cannot use this {0}.", type); 
                return;
            }

            if (grpName[0] == '+')
            {
                Group grp = GetGroup(p, data, grpName.Substring(1));
                if (grp == null) return;

                perms.Allow(grp.Permission);
                UpdatePerms(perms, p, " &Scan now be used by " + grp.ColoredName);
            }
            else if (grpName[0] == '-')
            {
                Group grp = GetGroup(p, data, grpName.Substring(1));
                if (grp == null) return;

                if (data.Rank == grp.Permission)
                {
                    p.Message("You cannot disallow your own rank from using a {0}.", type); 
                    return;
                }

                perms.Disallow(grp.Permission);
                UpdatePerms(perms, p, " &Sis no longer usable by " + grp.ColoredName);
            }
            else
            {
                Group grp = GetGroup(p, data, grpName);
                if (grp == null) return;

                perms.MinRank = grp.Permission;
                UpdatePerms(perms, p, " &Sis now usable by " + grp.ColoredName + "&S+");
            }
        }

        public abstract void UpdatePerms(ItemPerms perms, Player p, string msg);

        public static Group GetGroup(Player p, CommandData data, string grpName)
        {
            Group grp = Matcher.FindRanks(p, grpName);
            if (grp == null) return null;

            if (grp.Permission > data.Rank)
            {
                p.Message("&WCannot set permissions to a rank higher than yours."); 
                return null;
            }
            return grp;
        }

        public static void Announce(Player p, string msg)
        {
            Chat.MessageAll("&d" + msg);
            if (p.IsSuper) p.Message(msg);
        }
    }
}