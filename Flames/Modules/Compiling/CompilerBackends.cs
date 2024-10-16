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

// Based on https://github.com/aspnet/RoslynCodeDomProvider
// Copyright(c) Microsoft Corporation All rights reserved.
// 
// MIT License
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the ""Software""), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED AS IS, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System.CodeDom.Compiler;
using System.Collections.Generic;


namespace Flames.Modules.Compiling
{
    /// <summary> Compiles source code files from a particular language, using a CodeDomProvider for the compiler </summary>
    public static class ICodeDomCompiler
    {   
        public static CompilerParameters PrepareInput(string[] srcPaths, string dstPath, string commentPrefix) {
            CompilerParameters args = new CompilerParameters();
            args.GenerateExecutable      = false;
            args.IncludeDebugInformation = true;
            args.OutputAssembly          = dstPath;

            List<string> referenced = ICompiler.ProcessInput(srcPaths, commentPrefix);
            foreach (string assembly in referenced)
            {
                args.ReferencedAssemblies.Add(assembly);
            }
            return args;
        }

        // Lazy init compiler when it's actually needed
        public static void InitCompiler(ICompiler c, string language, ref CodeDomProvider compiler) {
            if (compiler != null) return;
            compiler = CodeDomProvider.CreateProvider(language);
            if (compiler != null) return;
            Logger.Log(LogType.Warning,
                       "WARNING: {0} compiler is missing, you will be unable to compile {1} files.",
                       c.FullName, c.FileExtension);
                // TODO: Should we log "You must have .net developer tools. (You need a visual studio)" ?
                //Should only log this for Visual Basic.
                if (c.FileExtension == "vb" && Server.runningOnMono == true)
                {
                    Logger.Log(LogType.Warning,
                       "WARNING: Visual Basic compiler is missing, you will be unable to compile .vb files.");
                }
        }

        public static ICompilerErrors Compile(CompilerParameters args, string[] srcPaths, CodeDomProvider compiler) {
            CompilerResults results = compiler.CompileAssemblyFromFile(args, srcPaths);
            ICompilerErrors errors  = new ICompilerErrors();

            foreach (CompilerError error in results.Errors)
            {
                ICompilerError ce = new ICompilerError();
                ce.Line        = error.Line;
                ce.Column      = error.Column;
                ce.ErrorNumber = error.ErrorNumber;
                ce.ErrorText   = error.ErrorText;
                ce.IsWarning   = error.IsWarning;
                ce.FileName    = error.FileName;

                errors.Add(ce);
            }
            return errors;
        }
    }
}