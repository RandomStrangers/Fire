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
using System;
using Flames.Commands;
using Flames.NewScripting;

namespace Flames.Modules.NewCompiling
{
    public sealed class CmdCmdCreate : CmdNewPluginCompile
    {
        public override string name { get { return "CmdCreate"; } }
        public override string shortcut { get { return ""; } }
        public override CommandAlias[] Aliases {
            get { return new[] { new CommandAlias("newPCreate", "newplugin") }; }
        }

        public void CompileCommand(Player p, string[] paths, ICompiler compiler) {
            string dstPath = IScripting.CommandPath(paths[0]);
            
            for (int i = 0; i < paths.Length; i++) 
            {
                 paths[i] = compiler.CommandPath(paths[i]);
            }
            CompilerOperations.Compile(p, compiler, "Command", paths, dstPath);
        }
        
        public override void CompileNewPlugin(Player p, string[] paths, ICompiler compiler) {
            foreach (string cmd in paths)
            {
                CompilerOperations.CreateNewPlugin(p, cmd, compiler);
            }
        }

        public override void Help(Player p) {
            p.Message("&T/CmdCreate [name]");
            p.Message("&HCreates an example C# command named Cmd[name]");
            p.Message("&H  This can be used as the basis for creating a new command");
            p.Message("&T/CmdCreate newplugin [name]");
            p.Message("&HCreate a new example C# plugin named [name]");
        }
    }
}