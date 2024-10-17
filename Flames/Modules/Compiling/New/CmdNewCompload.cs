/*
    Copyright 2011 MCForge modified by headdetect

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
using Flames.Commands;
using Flames.NewScripting;

namespace Flames.Modules.NewCompiling
{
    public sealed class CmdNewPluginCompLoad : CmdNewPluginCompile 
    {
        public override string name { get { return "NewPluginCompLoad"; } }
        public override string shortcut { get { return "newcml"; } }
        public override CommandAlias[] Aliases
        {
            get
            {
                return new[] {
                    new CommandAlias("newpcompload"),
                    new CommandAlias("newpcml"),
                };
            }
        }
        public override void CompileNewPlugin(Player p, string[] paths, ICompiler compiler) {
            string dst = IScripting.NewPluginPath(paths[0]);
            UnloadNewPlugin(p, paths[0]);
            base.CompileNewPlugin(p, paths, compiler);
            ScriptingOperations.LoadNewPlugins(p, dst);
        }
        public static void UnloadNewPlugin(Player p, string name) {
            NewPlugin newplugin = NewPlugin.FindNewCustom(name);
            
            if (newplugin == null) return;
            ScriptingOperations.UnloadNewPlugin(p, newplugin);
        }
        public override void Help(Player p) {
            p.Message("&T/NewPluginCompLoad [plugin]");
            p.Message("&HCompiles and loads (or reloads) a C# new plugin into the server");
        }        
    }
}