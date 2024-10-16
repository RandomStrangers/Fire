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
using Flames.NewScripting;
namespace Flames.Commands.NewScripting
{
    public sealed class CmdNewPlugin : Command2
    {
        public override string name { get { return "NewPlugin"; } }
        public override string type { get { return CommandTypes.Added; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Owner; } }
        public override CommandAlias[] Aliases
        {
            get
            {
                return new[] { new CommandAlias("NewPLoad", "load"), new CommandAlias("NewPUnload", "unload"),
                    new CommandAlias("NewPlugins", "list") };
            }
        }
        public override bool MessageBlockRestricted { get { return true; } }
        public override void Use(Player p, string message, CommandData data)
        {
            string[] args = message.SplitSpaces(2);
            if (IsListCommand(args[0]))
            {
                string modifier = args.Length > 1 ? args[1] : "";

                p.Message("Loaded new plugins:");
                Paginator.Output(p, NewPlugin.CustomNewPlugins, pl => pl.name,
                                 "NewPlugins", "newplugins", modifier);
                return;
            }
            if (args.Length == 1) 
            { 
                Help(p); 
                return; 
            }
            string cmd = args[0], name = args[1];
            if (!Formatter.ValidFilename(p, name)) return;
            if (cmd.CaselessEq("load"))
            {
                string path = IScripting.NewPluginPath(name);
                ScriptingOperations.LoadNewPlugins(p, path);
            }
            else if (cmd.CaselessEq("unload"))
            {
                UnloadNewPlugin(p, name);
            }
            else if (cmd.CaselessEq("create"))
            {
                Find("NewPluginCreate").Use(p, name);
            }
            else if (cmd.CaselessEq("compile"))
            {
                Find("NewPluginCompile").Use(p, name);
            }
            else
            {
                Help(p);
            }
        }
        public static void UnloadNewPlugin(Player p, string name)
        {
            int matches;
            NewPlugin newplugin = Matcher.Find(p, name, out matches, NewPlugin.CustomNewPlugins,
                                         null, pln => pln.name, "newplugins");
            if (newplugin == null) return;
            ScriptingOperations.UnloadNewPlugin(p, newplugin);
        }
        public override void Help(Player p)
        {
            p.Message("&T/NewPlugin load [filename]");
            p.Message("&HLoad a compiled plugin from the &fnew plugins &Hfolder");
            p.Message("&T/NewPlugin unload [name]");
            p.Message("&HUnloads a currently loaded new plugin");
            p.Message("&T/NewPlugin list");
            p.Message("&HLists all loaded new plugins");
        }
    }
}