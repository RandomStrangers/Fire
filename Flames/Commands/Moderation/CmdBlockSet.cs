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
using Flames.Blocks;
using BlockID = System.UInt16;

namespace Flames.Commands.Moderation {
    public sealed class CmdBlockSet : ItemPermsCmd {
        public override string name { get { return "BlockSet"; } }
        
        public override void Use(Player p, string message, CommandData data) {
            string[] args = message.SplitSpaces(2);
            if (args.Length < 2) { Help(p); return; }

            if (!CommandParser.GetBlockIfAllowed(p, args[0], "change permissions of", out ushort block)) return;

            BlockPerms perms = BlockPerms.Find(block);
            SetPerms(p, args, data, perms, "block");
        }

        public override void UpdatePerms(ItemPerms perms, Player p, string msg) {
            BlockPerms.Save();
            BlockPerms.ApplyChanges();
            
            BlockID block = ((BlockPerms)perms).ID;
            if (!Block.IsPhysicsType(block)) {
                BlockPerms.ResendAllBlockPermissions();
            }
            
            string name = Block.GetName(p, block);
            Announce(p, name + msg);
        }
        
        public override void Help(Player p) {
            p.Message("&T/BlockSet [block] [rank]");
            p.Message("&HSets lowest rank that can modify/use [block] to [rank]");
            p.Message("&T/BlockSet [block] +[rank]");
            p.Message("&HAllows a specific rank to modify/use [block]");
            p.Message("&T/BlockSet [block] -[rank]");
            p.Message("&HPrevents a specific rank from modifying/using [block]");
            p.Message("&HTo see available ranks, type &T/ViewRanks");
        }
    }
}