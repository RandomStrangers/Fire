/*
    Copyright 2010 MCLawl Team - Written by Valek (Modified for use with MCForge)
 
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
namespace Flames.Modules.NewCompiling
{
    public sealed class CmdNewPluginCreate : CmdNewPluginCompile 
    {
        public override string name { get { return "NewPluginCreate"; } }
        public override string shortcut { get { return "NewPCreate"; } }
        

        public override void CompileNewPlugin(Player p, string[] paths, ICompiler compiler) {
            foreach (string cmd in paths)
            {
                CompilerOperations.CreateNewPlugin(p, cmd, compiler);
            }
        }

        public override void Help(Player p) {
            p.Message("&T/NewPluginCreate [name]");
            p.Message("&HCreate a example C# new plugin named [name]");
        }
    }
}