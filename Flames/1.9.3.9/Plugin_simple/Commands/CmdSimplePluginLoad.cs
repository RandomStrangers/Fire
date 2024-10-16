/*
    Copyright 2011 MCForge
    
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


namespace Flames.Commands.Scripting {
    public sealed class CmdPlugin_simpleLoad : Command2 {
        public override string name { get { return "PSLoad"; } }
        public override string shortcut { get { return "p_sload"; } }

        public override string type { get { return CommandTypes.Other; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Nobody; } }
        public override bool MessageBlockRestricted { get { return true; } }

        public override void Use(Player p, string message, CommandData data)
        {
            Command.Find("SimplePlugin").Use(p, "Load " + message, data);
        }
        
        public override void Help(Player p) {
            p.Message("&T/SimplePlugin create [name]");
            p.Message("&HCreate a example .cs simple plugin file");
            p.Message("&T/SimplePlugin compile [name]");
            p.Message("&HCompiles a .cs simple plugin file");
            p.Message("&T/SimplePlugin load [filename]");
            p.Message("&HLoad a simple plugin from your simple plugins folder");
            p.Message("&T/SimplePlugin unload [name]");
            p.Message("&HUnloads a currently loaded simple plugin");
            p.Message("&T/SimplePlugin list");
            p.Message("&HLists all loaded simple plugins");
        }
    }
}
