/*
    Copyright 2011 MCForge modified by headdetect

    Dual-licensed under the Educational Community License, Version 2.0 and
    the GNU General Public License, Version 3 (the "Licenses"); you may
    not use this file except in compliance with the Licenses. You may
    obtain a copy of the Licenses at
    
    http://www.opensource.org/licenses/ecl2.php
    http://www.gnu.org/licenses/gpl-3.0.html
    
    Unless required by applicable law or agreed to in writing,
    software distributed under the Licenses are distributed on an "AS IS"
    BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
    or implied. See the Licenses for the specific language governing
    permissions and limitations under the Licenses.
*/
namespace Flames.Commands.Scripting
{
    public sealed class CmdCompLoad_Simple : Command2
    {
        public override string name { get { return "SimpleCompLoad"; } }
        public override string shortcut { get { return "scml"; } }
        public override string type { get { return CommandTypes.Added; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Owner; } }
        public override bool MessageBlockRestricted { get { return true; } }
        public override CommandAlias[] Aliases
        {
            get
            {
                return new[] {
                    new CommandAlias("pscompload"),
                    new CommandAlias("pscml"),
                };
            }
        }

        public override void Use(Player p, string message, CommandData data)
        {
            string[] args = message.SplitSpaces();
            if (message.Length == 0)
            {
                Help(p);
                return;
            }

            if (args.Length == 1 || args[1].CaselessEq("vb"))
            {
                Find("PSCompile").Use(p, message, data);
                Find("PSLoad").Use(p, args[0], data);
            }
            else
            {
                Help(p);
            }
        }

        public override void Help(Player p)
        {
            p.Message("&T/SimpleCompLoad [plugin_simple]");
            p.Message("&HCompiles and loads a C# simple plugin into the server for use.");
            p.Message("&T/SimpleCompLoad [plugin_simple] vb");
            p.Message("&HCompiles and loads a Visual basic simple plugin into the server for use.");
        }
    }
}