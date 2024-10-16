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

namespace Flames.Modules.NewCompiling
{
    public class CmdNewPluginCompile : Command2
    {
        public override string name { get { return "NewPluginCompile"; } }
        public override string shortcut { get { return "NewPCompile"; } }
        public override string type { get { return CommandTypes.Other; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Owner; } }
        public override bool MessageBlockRestricted { get { return true; } }
        public override void Use(Player p, string message, CommandData data)
        {
            string[] args = message.SplitSpaces();
            string name, lang;
            // compile [name] <language>
            name = args[0];
            lang = args.Length > 1 ? args[1] : "";
            if (name.Length == 0)
            {
                Help(p);
                return;
            }
            if (!Formatter.ValidFilename(p, name)) return;
            ICompiler compiler = CompilerOperations.GetCompiler(p, lang);
            if (compiler == null) return;
            // either "source" or "source1,source2,source3"
            string[] paths = name.SplitComma();
            CompileNewPlugin(p, paths, compiler);
        }
        public virtual void CompileNewPlugin(Player p, string[] paths, ICompiler compiler)
        {
            string dstPath = IScripting.NewPluginPath(paths[0]);
            for (int i = 0; i < paths.Length; i++)
            {
                paths[i] = compiler.NewPluginPath(paths[i]);
            }
            CompilerOperations.Compile(p, compiler, "NewPlugin", paths, dstPath);
        }

        public override void Help(Player p)
        {
            ICompiler compiler = ICompiler.Compilers[0];
            p.Message("&T/NewPluginCompile [plugin name]");
            p.Message("&HCompiles a .cs file containing a  C# new plugin into a DLL");
            p.Message("&H  Compiles from &f{0}", compiler.NewPluginPath("&H<name>&f"));
        }
    }
}
