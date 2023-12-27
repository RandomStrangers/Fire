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
using System.IO;
using Flames.Scripting;

namespace Flames.Commands.Scripting {
    public sealed class CmdPlugin_simple : Command2 {
        public override string name { get { return "SimplePlugin"; } }
        public override string shortcut { get { return "p_s"; } }
        public override string type { get { return CommandTypes.Other; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Nobody; } }
        public override CommandAlias[] Aliases {
            get
            {
                return new[] {
                    new CommandAlias("PSLoad", "load"),
                    new CommandAlias("PSUnload", "unload"),
                    new CommandAlias("PSCreate", "create"),
                    new CommandAlias("PSCompile", "compile"),
                    new CommandAlias("SimplePlugins", "list"),
                    new CommandAlias("Simple_Plugins", "list"),
                    new CommandAlias("Plugins_Simple", "list"),
                    new CommandAlias("PluginsSimple", "list"),
                    new CommandAlias("PSS", "list"),
                    new CommandAlias("P_SLoad", "load"),
                    new CommandAlias("P_SUnload", "unload"),
                    new CommandAlias("P_SCreate", "create"),
                    new CommandAlias("P_SCompile", "compile")
                };
            }
            }
        public override bool MessageBlockRestricted { get { return true; } }
        
        public override void Use(Player p, string message, CommandData data) {
            string[] args = message.SplitSpaces(3);
            if (IsListCommand(args[0])) {
                string modifier = args.Length > 1 ? args[1] : "";
                
                p.Message("Loaded simple plugins:");
                MultiPageOutput.Output(p, Plugin_Simple.all, pl => pl.Name,
                                      "Simple plugins", "simple plugins", modifier, false);
                return;
            }
            if (args.Length == 1) { Help(p); return; }
            
            string cmd = args[0], name = args[1];
            if (!Formatter.ValidFilename(p, name)) return;
            string language = args.Length > 2 ? args[2] : "";
            
            if (cmd.CaselessEq("load")) {
                LoadSimplePlugin(p, name);
            } else if (cmd.CaselessEq("unload")) {
                UnloadSimplePlugin(p, name);
            } else if (cmd.CaselessEq("create")) {
                CreateSimplePlugin(p, name, language);
            } else if (cmd.CaselessEq("compile")) {
                CompileSimplePlugin(p, name, language);
            } else {
                Help(p);
            }
        }
        
        static void CompileSimplePlugin(Player p, string name, string language) {
            ICompiler_Simple compiler = ScriptingOperations_Simple.GetCompiler(p, language);
            if (compiler == null) return;
            
            // either "source" or "source1,source2,source3"
            string[] paths = name.SplitComma();
            string dstPath = IScripting_Simple.SimplePluginPath(paths[0]);
            
            for (int i = 0; i < paths.Length; i++) {
                 paths[i] = compiler.SimplePluginPath(paths[i]);
            }
            ScriptingOperations_Simple.Compile(p, compiler, "Simple plugin", paths, dstPath);
        }
        
        static void LoadSimplePlugin(Player p, string name) {
            string path = IScripting_Simple.SimplePluginPath(name);
            if (!File.Exists(path)) {
                p.Message("File &9{0} &Snot found.", path); return;
            }
            
            if (IScripting_Simple.LoadSimplePlugin(path, false)) {
                p.Message("Simple plugin loaded successfully.");
            } else {
                p.Message("&WError loading simple plugin. See error logs for more information.");
            }
        }
        
        static void UnloadSimplePlugin(Player p, string name) {
            Plugin_Simple plugin = Matcher.Find(p, name, out int matches, Plugin_Simple.all,
                                         null, pln => pln.Name, "");
            if (plugin == null) return;
            
            if (Plugin_Simple.core.Contains(plugin)) {
                p.Message(plugin.Name + " is a core simple plugin and cannot be unloaded.");
                return;
            }
            
            if (plugin != null) {
                if (Plugin_Simple.Unload(plugin, false)) {
                    p.Message("Simple plugin unloaded successfully.");
                } else {
                    p.Message("&WError unloading simple plugin. See error logs for more information.");
                }
            } else {
                p.Message("Loaded simple plugins: " + Plugin_Simple.all.Join(pl => pl.Name));
            }
        }
        
        static void CreateSimplePlugin(Player p, string name, string language) {
            ICompiler_Simple engine = ScriptingOperations_Simple.GetCompiler(p, language);
            if (engine == null) return;
            
            string path = engine.SimplePluginPath(name);
            p.Message("Creating a simple plugin example source");
            
            string creator = p.IsSuper ? Server.Config.Name : p.truename;
            string source  = engine.GenExamplePlugin(name, creator);
            File.WriteAllText(path, source);
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