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
namespace Flames.Commands.Moderation {
    public sealed class CmdCmdSet : ItemPermsCmd {
        public override string name { get { return "CmdSet"; } }
        public override string shortcut { get { return "SetCmd"; } }


        public override void Use(Player p, string message, CommandData data) {
            string[] args = message.SplitSpaces(3);
            if (args.Length < 2) { Help(p); return; }
            
            string cmdName = args[0], cmdArgs = "";
            Search(ref cmdName, ref cmdArgs);
            Command cmd = Find(cmdName);
            
            if (cmd == null) { p.Message("Could not find command entered"); return; }
            
            if (!p.CanUse(cmd)) {
                cmd.Permissions.MessageCannotUse(p);
                p.Message("Therefore you cannot change the permissions of &T/{0}", cmd.name); return;
            }
            
            if (args.Length == 2) {
                SetPerms(p, args, data, cmd.Permissions, "command");
            } else {
                int num = 0;
                if (!CommandParser.GetInt(p, args[2], "Extra permission number", ref num)) return;
                
                CommandExtraPerms perms = CommandExtraPerms.Find(cmd.name, num);
                if (perms == null) {
                    p.Message("This command has no extra permission by that number."); return;
                }
                SetPerms(p, args, data, perms, "extra permission");
            }
        }

        public override void UpdatePerms(ItemPerms perms, Player p, string msg) {
            if (perms is CommandPerms) {
                CommandPerms.Save();
                CommandPerms.ApplyChanges();
                Announce(p, perms.ItemName + msg);
            } else {
                CommandExtraPerms.Save();
                CommandExtraPerms ex = (CommandExtraPerms)perms;
                //Announce(p, cmd.name + "&S's extra permission " + idx + " was set to " + grp.ColoredName);
                Announce(p, ex.CmdName + " extra permission #" + ex.Num + msg);
            }
        }
        
        public override void Help(Player p) {
            p.Message("&T/CmdSet [cmd] [rank]");
            p.Message("&HSets lowest rank that can use [cmd] to [rank]");
            p.Message("&T/CmdSet [cmd] +[rank]");
            p.Message("&HAllows a specific rank to use [cmd]");
            p.Message("&T/CmdSet [cmd] -[rank]");
            p.Message("&HPrevents a specific rank from using [cmd]");
            p.Message("&T/CmdSet [cmd] [rank] [extra permission number]");
            p.Message("&HSet the lowest rank that has that extra permission for [cmd]");
            p.Message("&HTo see available ranks, type &T/ViewRanks");
        }
    }
}