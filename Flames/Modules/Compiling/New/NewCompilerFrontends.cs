/*
    Copyright 2010 MCLawl Team - Written by Valek (Modified by MCGalaxy)

    Edited for use with MCGalaxy
 
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
using System.Collections.Generic;

namespace Flames.Modules.NewCompiling
{
    public sealed class CSCompiler : ICompiler 
    {
        public override string FileExtension { get { return ".cs"; } }
        public override string ShortName     { get { return "C#"; } }  
        public override string FullName      { get { return "CSharp"; } }

        public override ICompilerErrors DoCompile(string[] srcPaths, string dstPath) {
            List<string> referenced = ProcessInput(srcPaths, "//");
            
            CommandLineCompiler compiler = new ClassicCSharpCompiler();
            return compiler.Compile(srcPaths, dstPath, referenced);
        }
        
        public override string NewPluginSkeleton {
            get {
                return @"//\tAuto-generated plugin skeleton class
//\tUse this as a basis for custom Flames new plugins

// To reference other assemblies, put a ""//reference [assembly filename]"" at the top of the file
//   e.g. to reference the System.Data assembly, put ""//reference System.Data.dll""

// Add any other using statements you need after this
using System;

namespace Flames
{{
\tpublic class {0} : NewPlugin
\t{{
\t\t// The new plugin's name (i.e what shows in /NewPlugins)
\t\tpublic override string name {{ get {{ return ""{0}""; }} }}

\t\t// The oldest version of Flames this new plugin is compatible with
\t\tpublic override string Flames_Version {{ get {{ return ""{2}""; }} }}

\t\t// Message displayed in server logs when this new plugin is loaded
\t\tpublic override string welcome {{ get {{ return ""Loaded Message!""; }} }}

\t\t// Who created/authored this new plugin
\t\tpublic override string creator {{ get {{ return ""{1}""; }} }}

\t\t// Called when this new plugin is being loaded (e.g. on server startup)
\t\tpublic override void Load(bool startup)
\t\t{{
\t\t\t//code to hook into events, load state/resources etc goes here
\t\t}}

\t\t// Called when this new plugin is being unloaded (e.g. on server shutdown)
\t\tpublic override void Unload(bool shutdown)
\t\t{{
\t\t\t//code to unhook from events, dispose of state/resources etc goes here
\t\t}}

\t\t// Displays help for or information about this new plugin
\t\tpublic override void Help(Player p)
\t\t{{
\t\t\tp.Message(""No help is available for this new plugin."");
\t\t}}
\t}}
}}";
            }
        }
    }
}