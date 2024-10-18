/*
    Copyright 2010 MCLawl Team - Written by Valek (Modified by MCGalaxy)

    Edited for use with MCGalaxy
 
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
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Flames.Scripting
{
    /// <summary> Utility methods for loading assemblies, and simple plugins </summary>
    public static class IScripting_Simple
    {

        /// <summary> Returns the default .dll path for the simple plugin with the given name </summary>

        public static string SimplePluginPath(string name)
        {
            return "" + name + ".dll";
        }

        /// <summary> Constructs instances of all types which derive from T in the given assembly. </summary>
        /// <returns> The list of constructed instances. </returns>

        public sealed class AlreadyLoadedException : Exception
        {
            public AlreadyLoadedException(string msg) : base(msg) 
            { 
            }
        }
        public static Assembly LoadAssembly(string path)
        {
            byte[] data = File.ReadAllBytes(path);
            byte[] debug = GetDebugData(path);
            return Assembly.Load(data, debug);
        }
        public static byte[] GetDebugData(string path)
        {
            if (Server.RunningOnMono())
            {
                // test.dll -> test.dll.mdb
                path += ".mdb";
            }
            else
            {
                // test.dll -> test.pdb
                path = Path.ChangeExtension(path, ".pdb");
            }

            if (!File.Exists(path)) return null;
            try
            {
                return File.ReadAllBytes(path);
            }
            catch (Exception ex)
            {
                Logger.LogError("Error loading .pdb " + path, ex);
                return null;
            }
        }
        public static List<T> LoadTypes<T>(Assembly lib)
        {
            List<T> instances = new List<T>();

            foreach (Type t in lib.GetTypes())
            {
                if (t.IsAbstract || t.IsInterface || !t.IsSubclassOf(typeof(T))) continue;
                object instance = Activator.CreateInstance(t);

                if (instance == null)
                {
                    Logger.Log(LogType.Warning, "{0} \"{1}\" could not be loaded", typeof(T).Name, t.Name);
                    throw new BadImageFormatException();
                }
                instances.Add((T)instance);
            }
            return instances;
        }
        //TODO: Some system files might not contain these
        public static bool IsSystemDLL(string file)
        {
            if (file.CaselessContains("SQL"))
            {
                return true;
            }
            if (file.CaselessContains("Newtonsoft"))
            {
                return true;
            }
            if (file.CaselessContains("System"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static void AutoloadSimplePlugins()
        {
            string simplepluginpath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            string[] files = AtomicIO.TryGetFiles(simplepluginpath, "*.dll");
            if (files != null)
            {
                foreach (string file in files)
                {
                    if (!IsSystemDLL(file))
                    {
                        LoadSimplePlugin(file, true);
                    }
                    else
                    {
                        return;
                    }
                }
            }
            else
            {
                Directory.CreateDirectory("");
            }
        }

        /// <summary> Loads all simple plugins from the given .dll path. </summary>
        public static bool LoadSimplePlugin(string path, bool auto)
        {
            try
            {
                Assembly lib = LoadAssembly(path);
                List<Plugin_Simple> simpleplugins = LoadTypes<Plugin_Simple>(lib);

                foreach (Plugin_Simple simpleplugin in simpleplugins)
                {
                    if (!Plugin_Simple.Load(simpleplugin, auto)) return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError("Error loading simple plugins from " + path, ex);
                return false;
            }
        }
    }

    /// <summary> Compiles source code files for a particular programming language into a .dll </summary>
    public abstract class ICompiler_Simple
    {
        public const string ErrorPath = "logs/errors/compiler_simple.log";

        /// <summary> Default file extension used for source code files </summary>
        /// <example> .cs, .vb </example>
        public abstract string FileExtension { get; }
        /// <summary> The short name of this programming language </summary>
        /// <example> CS, VB </example>
        public abstract string ShortName { get; }
        /// <summary> The full name of this programming language </summary>
        /// <example> CSharp, Visual Basic </example>
        public abstract string FullName { get; }
        /// <summary> Returns source code for an example simple plugin </summary>
        public abstract string SimplePluginSkeleton { get; }

        public string SimplePluginPath(string name)
        {
            return "" + name + FileExtension;
        }

        /// <summary> C# compiler instance. </summary>
        public static ICompiler_Simple CS = new CSCompiler_Simple();
        /// <summary> Visual Basic compiler instance. </summary>
        public static ICompiler_Simple VB = new VBCompiler_Simple();

        public static List<ICompiler_Simple> Compilers = new List<ICompiler_Simple>() 
        { 
            CS, VB 
        };


        public static string FormatSource(string source, params string[] args)
        {
            // Always use \r\n line endings so it looks correct in Notepad
            source = source.Replace(@"\t", "\t");
            source = source.Replace("\n", "\r\n");
            return string.Format(source, args);
        }
        /// <summary> Generates source code for an example simple plugin, 
        /// preformatted with the given name and creator </summary>
        public string GenExamplePlugin(string simpleplugin, string creator)
        {
            return FormatSource(SimplePluginSkeleton, simpleplugin, creator, Server.Version);
        }

        /// <summary> Attempts to compile the given source code file to a .dll file. </summary>
        /// <remarks> If dstPath is null, compiles to an in-memory .dll instead. </remarks>
        /// <remarks> Logs errors to IScripting.ErrorPath. </remarks>
        public CompilerResults Compile(string srcPath, string dstPath)
        {
            return Compile(new[] { srcPath }, dstPath);
        }

        /// <summary> Attempts to compile the given source code files to a .dll file. </summary>
        /// <remarks> If dstPath is null, compiles to an in-memory .dll instead. </remarks>
        /// <remarks> Logs errors to IScripting.ErrorPath. </remarks>
        public CompilerResults Compile(string[] srcPaths, string dstPath)
        {
            CompilerResults results = DoCompile(srcPaths, dstPath);
            if (!results.Errors.HasErrors) return results;

            SimpleSourceMap sources = new SimpleSourceMap(srcPaths);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("############################################################");
            sb.AppendLine("Errors when compiling " + srcPaths.Join());
            sb.AppendLine("############################################################");
            sb.AppendLine();

            foreach (CompilerError err in results.Errors)
            {
                string type = err.IsWarning ? "Warning" : "Error";
                sb.AppendLine(DescribeError(err, srcPaths, "") + ":");

                if (err.Line > 0) sb.AppendLine(sources.Get(err.FileName, err.Line - 1));
                if (err.Column > 0) sb.Append(' ', err.Column - 1);
                sb.AppendLine("^-- " + type + " #" + err.ErrorNumber + " - " + err.ErrorText);

                sb.AppendLine();
                sb.AppendLine("-------------------------");
                sb.AppendLine();
            }

            using (StreamWriter w = new StreamWriter(ErrorPath, true))
            {
                w.Write(sb.ToString());
            }
            return results;
        }

        /// <summary> Compiles the given source code. </summary>
        public abstract CompilerResults DoCompile(string[] srcPaths, string dstPath);

        public static string DescribeError(CompilerError err, string[] srcs, string text)
        {
            string type = err.IsWarning ? "Warning" : "Error";
            string file = Path.GetFileName(err.FileName);
            if (err.Line <= 0)
            {
                return string.Format("{0}{1} in reference assemblies{2}", type, text,
                                     srcs.Length > 1 ? " in " + file : "");
            }
            // Include filename if compiling multiple source code files
            return string.Format("{0}{1} on line {2}{3}", type, text, err.Line,
                                 srcs.Length > 1 ? " in " + file : "");
        }
    }

    /// <summary> Compiles source code files from a particular language into a .dll file, using a CodeDomProvider for the compiler. </summary>
    public abstract class ICodeDomCompiler_Simple : ICompiler_Simple
    {
        public object compilerLock = new object();
        public CodeDomProvider compiler;

        /// <summary> Creates a CodeDomProvider instance for this programming language </summary>
        public abstract CodeDomProvider CreateProvider();
        /// <summary> Adds language-specific default arguments to list of arguments. </summary>
        public abstract void PrepareArgs(CompilerParameters args);
        /// <summary> Returns the starting characters for a comment </summary>
        /// <example> For C# this is "//" </example>
        public virtual string CommentPrefix { get { return "//"; } }

        // Lazy init compiler when it's actually needed
        public void InitCompiler()
        {
            lock (compilerLock)
            {
                if (compiler != null) return;
                compiler = CreateProvider();
                if (compiler != null) return;

                Logger.Log(LogType.Warning,
                           "WARNING: {0} compiler is missing, you will be unable to compile {1} files.",
                           FullName, FileExtension);
                // TODO: Should we log "You must have .net developer tools. (You need a visual studio)" ?
            }
        }

        public void AddReferences(string path, CompilerParameters args)
        {
            // Allow referencing other assemblies using '//reference [assembly name]' at top of the file
            using (StreamReader r = new StreamReader(path))
            {
                string refPrefix = CommentPrefix + "reference ";
                string line;

                while ((line = r.ReadLine()) != null)
                {
                    if (!line.CaselessStarts(refPrefix)) break;

                    int index = line.IndexOf(' ') + 1;
                    // For consistency with C#, treat '//reference X.dll;' as '//reference X.dll'
                    string assem = line.Substring(index).Replace(";", "");

                    args.ReferencedAssemblies.Add(assem);
                }
            }
        }

        public override CompilerResults DoCompile(string[] srcPaths, string dstPath)
        {
            CompilerParameters args = new CompilerParameters
            {
                GenerateExecutable = false,
                IncludeDebugInformation = true
            };

            if (dstPath != null) args.OutputAssembly = dstPath;
            if (dstPath == null) args.GenerateInMemory = true;

            for (int i = 0; i < srcPaths.Length; i++)
            {
                // CodeDomProvider doesn't work properly with relative paths
                string path = Path.GetFullPath(srcPaths[i]);

                AddReferences(path, args);
                srcPaths[i] = path;
            }
            args.ReferencedAssemblies.Add("Flames_.dll");

            PrepareArgs(args);
            InitCompiler();
            return compiler.CompileAssemblyFromFile(args, srcPaths);
        }
    }

    public class SimpleSourceMap
    {
        public string[] files;
        public List<string>[] sources;

        public SimpleSourceMap(string[] paths)
        {
            files = paths;
            sources = new List<string>[paths.Length];
        }

        public int FindFile(string file)
        {
            for (int i = 0; i < files.Length; i++)
            {
                if (file.CaselessEq(files[i])) return i;
            }
            return -1;
        }

        /// <summary> Returns the given line in the given source code file </summary>
        public string Get(string file, int line)
        {
            int i = FindFile(file);
            if (i == -1) return "";

            List<string> source = sources[i];
            if (source == null)
            {
                try
                {
                    source = Utils.ReadAllLinesList(file);
                }
                catch
                {
                    source = new List<string>();
                }
                sources[i] = source;
            }
            return line < source.Count ? source[line] : "";
        }
    }
}