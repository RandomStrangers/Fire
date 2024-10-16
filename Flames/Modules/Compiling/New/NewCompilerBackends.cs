/*
    Copyright 2015 MCGalaxy
 
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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace Flames.Modules.NewCompiling
{
    /// <summary> Compiles C# source files into a .dll by invoking a compiler executable directly </summary>
    public abstract class CommandLineCompiler
    {
        public ICompilerErrors Compile(string[] srcPaths, string dstPath, List<string> referenced) {
            string args = GetCommandLineArguments(srcPaths, dstPath, referenced);
            string exe  = GetExecutable();

            ICompilerErrors errors = new ICompilerErrors();
            List<string> output    = new List<string>();
            int retValue = Compile(exe, GetCompilerArgs(exe, args), output);

            // Only look for errors/warnings if the compile failed
            // TODO still log warnings anyways error when success?
            if (retValue != 0) {
                foreach (string line in output)
                {
                    ProcessCompilerOutputLine(errors, line);
                }
            }
            return errors;
        }


        public virtual string GetCommandLineArguments(string[] srcPaths, string dstPath,
                                                         List<string> referencedAssemblies) {
            StringBuilder sb = new StringBuilder();
            sb.Append("/t:library ");

            sb.Append("/utf8output /noconfig /fullpaths ");
            
            AddCoreAssembly(sb);
            AddReferencedAssemblies(sb, referencedAssemblies);
            sb.AppendFormat("/out:{0} ", Quote(dstPath));

            sb.Append("/D:DEBUG /debug+ /optimize- ");
            sb.Append("/warnaserror- /unsafe ");

            foreach (string path in srcPaths)
            {
                sb.AppendFormat("{0} ", Quote(path));
            }
            return sb.ToString();
        }

        public virtual void AddCoreAssembly(StringBuilder sb) {
            string coreAssemblyFileName = typeof(object).Assembly.Location;

            if (!string.IsNullOrEmpty(coreAssemblyFileName)) {
                sb.Append("/nostdlib+ ");
                sb.AppendFormat("/R:{0} ", Quote(coreAssemblyFileName));
            }
        }

        public abstract void AddReferencedAssemblies(StringBuilder sb, List<string> referenced);

        public static string Quote(string value) { return "\"" + value.Trim() + "\""; }

        public abstract string GetExecutable();
        public abstract string GetCompilerArgs(string exe, string args);


        public static int Compile(string path, string args, List<string> output) {
            // https://stackoverflow.com/questions/285760/how-to-spawn-a-process-and-capture-its-stdout-in-net
            ProcessStartInfo psi = CreateStartInfo(path, args);

            using (Process p = new Process())
            {
                p.OutputDataReceived += (s, e) => { if (e.Data != null) output.Add(e.Data); };
                p.ErrorDataReceived  += (s, e) => { }; // swallow stderr output

                p.StartInfo = psi;
                p.Start();

                p.BeginOutputReadLine();
                p.BeginErrorReadLine();

                if (!p.WaitForExit(120 * 1000))
                    throw new InvalidOperationException("C# compiler ran for over two minutes! Giving up..");

                return p.ExitCode;
            }
        }

        public static ProcessStartInfo CreateStartInfo(string path, string args) {
            ProcessStartInfo psi = new ProcessStartInfo(path, args);
            psi.WorkingDirectory       = Environment.CurrentDirectory;
            psi.UseShellExecute        = false;
            psi.CreateNoWindow         = true;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError  = true;
            return psi;
        }


        public static Regex outputRegWithFileAndLine;
        public static Regex outputRegSimple;

        public static void ProcessCompilerOutputLine(ICompilerErrors errors, string line) {
            if (outputRegSimple == null) {
                outputRegWithFileAndLine =
                    new Regex(@"(^(.*)(\(([0-9]+),([0-9]+)\)): )(error|warning) ([A-Z]+[0-9]+) ?: (.*)");
                outputRegSimple =
                    new Regex(@"(error|warning) ([A-Z]+[0-9]+) ?: (.*)");
            }

            //First look for full file info
            Match m = outputRegWithFileAndLine.Match(line);
            bool full;
            if (m.Success) {
                full = true;
            } else {
                m = outputRegSimple.Match(line);
                full = false;
            }

            if (!m.Success) return;
            ICompilerError ce = new ICompilerError();

            if (full) {
                ce.FileName = m.Groups[2].Value;
                ce.Line     = NumberUtils.ParseInt32(m.Groups[4].Value);
                ce.Column   = NumberUtils.ParseInt32(m.Groups[5].Value);
            }

            ce.IsWarning   = m.Groups[full ? 6 : 1].Value.CaselessEq("warning");
            ce.ErrorNumber = m.Groups[full ? 7 : 2].Value;
            ce.ErrorText   = m.Groups[full ? 8 : 3].Value;
            errors.Add(ce);
        }
    }

    public class ClassicCSharpCompiler : CommandLineCompiler
    {
        public override void AddCoreAssembly(StringBuilder sb) {
            string coreAssemblyFileName = typeof(object).Assembly.Location;

            if (!string.IsNullOrEmpty(coreAssemblyFileName)) {
                sb.Append("/nostdlib+ ");
                sb.AppendFormat("/R:{0} ", Quote(coreAssemblyFileName));
            }
        }

        public override void AddReferencedAssemblies(StringBuilder sb, List<string> referenced) {
            foreach (string path in referenced)
            {
                sb.AppendFormat("/R:{0} ", Quote(path));
            }
        }


        public override string GetExecutable() {
            string root = RuntimeEnvironment.GetRuntimeDirectory();
            
            string[] paths = new string[] {
                // First try new C# compiler
                Path.Combine(root, "csc.exe"),
                // Then fallback to old Mono C# compiler
                Path.Combine(root, @"../../../bin/mcs"), 
                Path.Combine(root, "mcs.exe"),
                "/usr/bin/mcs",
            };
            
            foreach (string path in paths)
            {
                if (File.Exists(path)) return path;
            }
            return paths[0];
        }

        public override string GetCompilerArgs(string exe, string args) {
            return args;
        }
    }
}