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

namespace Flames.Commands.Scripting
{
    public class CmdCorePlugin : Command2
    {
        public override string name { get { return "CorePlugin"; } }
        public override string type { get { return CommandTypes.Other; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Owner; } }
        public override CommandAlias[] Aliases
        {
            get
            {
                return new[] { new CommandAlias("CorePlugins", "list") };
            }
        }
        public override bool MessageBlockRestricted { get { return true; } }

        public override void Use(Player p, string message)
        {
            string[] args = message.SplitSpaces(2);
            if (IsListAction(args[0]))
            {
                string modifier = args.Length > 1 ? args[1] : "";

                p.Message("Loaded core plugins:");
                Paginator.Output(p, Plugin.core, pl => pl.name,
                                 "CorePlugins", "core plugins", modifier);
                return;
            }
            else
            {
                Help(p);
            }
        }

        public override void Help(Player p)
        {
            p.Message("&T/CorePlugins");
            p.Message("&HLists all loaded core plugins");
        }
    }
}