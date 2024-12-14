#if CORE
using System;
using System.Collections.Generic;
using Flames.GoldenSparksScripting;
using Flames.Events.ServerEvents;
using Flames.Modules.GoldenSparksCompiling;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using Flames.Commands;
namespace Flames.Modules.GoldenSparksCompiling
{
    public class CmdGoldenSparksPluginCompile : Command
    {
        public override string name { get { return "GoldenSparksPluginCompile"; } }
        public override string shortcut { get { return "GSPCompile"; } }
        public override string type { get { return CommandTypes.Other; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Owner; } }
        public override bool MessageBlockRestricted { get { return true; } }
        public override void Use(Player p, string message)
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
            CompileGoldenSparksPlugin(p, paths, compiler);
        }
        public virtual void CompileGoldenSparksPlugin(Player p, string[] paths, ICompiler compiler)
        {
            string dstPath = IScripting.GoldenSparksPluginPath(paths[0]);
            for (int i = 0; i < paths.Length; i++)
            {
                paths[i] = compiler.GoldenSparksPluginPath(paths[i]);
            }
            CompilerOperations.Compile(p, compiler, "GoldenSparksPlugin", paths, dstPath);
        }

        public override void Help(Player p)
        {
            ICompiler compiler = ICompiler.Compilers[0];
            p.Message("&T/GoldenSparksPluginCompile [plugin name]");
            p.Message("&HCompiles a .cs file containing a  C# GoldenSparks plugin into a DLL");
            p.Message("&H  Compiles from &f{0}", compiler.GoldenSparksPluginPath("&H<name>&f"));
        }
    }
    public class CmdGoldenSparksPluginCompLoad : CmdGoldenSparksPluginCompile
    {
        public override string name { get { return "GoldenSparksPluginCompLoad"; } }
        public override string shortcut { get { return "gscml"; } }
        public override CommandAlias[] Aliases
        {
            get
            {
                return new[] {
                    new CommandAlias("gspcompload"),
                    new CommandAlias("gspcml"),
                };
            }
        }
        public override void CompileGoldenSparksPlugin(Player p, string[] paths, ICompiler compiler)
        {
            string dst = IScripting.GoldenSparksPluginPath(paths[0]);
            UnloadGoldenSparksPlugin(p, paths[0]);
            base.CompileGoldenSparksPlugin(p, paths, compiler);
            ScriptingOperations.LoadGoldenSparksPlugins(p, dst);
        }
        public static void UnloadGoldenSparksPlugin(Player p, string name)
        {
            GoldenSparksPlugin GoldenSparksplugin = GoldenSparksPlugin.FindGoldenSparksCustom(name);

            if (GoldenSparksplugin == null) return;
            ScriptingOperations.UnloadGoldenSparksPlugin(p, GoldenSparksplugin);
        }
        public override void Help(Player p)
        {
            p.Message("&T/GoldenSparksPluginCompLoad [plugin]");
            p.Message("&HCompiles and loads (or reloads) a C# GoldenSparks plugin into the server");
        }
    }
    public class CmdGoldenSparksPlugin : Command
    {
        public override string name { get { return "GoldenSparksPlugin"; } }
        public override string type { get { return CommandTypes.Added; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Owner; } }
        public override CommandAlias[] Aliases
        {
            get
            {
                return new[] { new CommandAlias("GSPLoad", "load"), new CommandAlias("GSPUnload", "unload"),
                    new CommandAlias("GSPlugins", "list") };
            }
        }
        public override bool MessageBlockRestricted { get { return true; } }
        public override void Use(Player p, string message)
        {
            string[] args = message.SplitSpaces(2);
            if (IsListAction(args[0]))
            {
                string modifier = args.Length > 1 ? args[1] : "";

                p.Message("Loaded GoldenSparks plugins:");
                Paginator.Output(p, GoldenSparksPlugin.CustomGoldenSparksPlugins, pl => pl.name,
                                 "GoldenSparks", "goldensparksplugins", modifier);
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
                string path = IScripting.GoldenSparksPluginPath(name);
                ScriptingOperations.LoadGoldenSparksPlugins(p, path);
            }
            else if (cmd.CaselessEq("unload"))
            {
                UnloadGoldenSparksPlugin(p, name);
            }
            else if (cmd.CaselessEq("create"))
            {
                Find("GoldenSparksPluginCreate").Use(p, name);
            }
            else if (cmd.CaselessEq("compile"))
            {
                Find("GoldenSparksPluginCompile").Use(p, name);
            }
            else
            {
                Help(p);
            }
        }
        public static void UnloadGoldenSparksPlugin(Player p, string name)
        {
            int matches;
            GoldenSparksPlugin GoldenSparksplugin = Matcher.Find(p, name, out matches, GoldenSparksPlugin.CustomGoldenSparksPlugins,
                                         null, pln => pln.name, "goldensparksplugins");
            if (GoldenSparksplugin == null) return;
            ScriptingOperations.UnloadGoldenSparksPlugin(p, GoldenSparksplugin);
        }
        public override void Help(Player p)
        {
            p.Message("&T/GoldenSparksPlugin load [filename]");
            p.Message("&HLoad a compiled plugin from the &fGoldenSparks plugins &Hfolder");
            p.Message("&T/GoldenSparksPlugin unload [name]");
            p.Message("&HUnloads a currently loaded GoldenSparks plugin");
            p.Message("&T/GoldenSparksPlugin list");
            p.Message("&HLists all loaded GoldenSparks plugins");
        }
    }
    public class CmdGoldenSparksPluginCreate : CmdGoldenSparksPluginCompile
    {
        public override string name { get { return "GoldenSparksPluginCreate"; } }
        public override string shortcut { get { return "GSPCreate"; } }


        public override void CompileGoldenSparksPlugin(Player p, string[] paths, ICompiler compiler)
        {
            foreach (string cmd in paths)
            {
                CompilerOperations.CreateGoldenSparksPlugin(p, cmd, compiler);
            }
        }

        public override void Help(Player p)
        {
            p.Message("&T/GoldenSparksPluginCreate [name]");
            p.Message("&HCreate an example C# GoldenSparks plugin named [name]");
        }
    }
    /// <summary> Compiles source code files for a particular programming language into a .dll </summary>
    public abstract class ICompiler
    {
        public const string GOLDENSPARKS_PLUGINS_SOURCE_DIR = "goldensparksplugins/";
        public const string ERROR_LOG_PATH = "logs/errors/compiler_gs.log";

        /// <summary> Default file extension used for source code files </summary>
        /// <example> .cs, .vb </example>
        public abstract string FileExtension { get; }
        /// <summary> The short name of this programming language </summary>
        /// <example> C#, VB </example>
        public abstract string ShortName { get; }
        /// <summary> The full name of this programming language </summary>
        /// <example> CSharp, Visual Basic </example>
        public abstract string FullName { get; }
        /// <summary> Returns source code for an example GoldenSparks plugin </summary>
        public abstract string GoldenSparksPluginSkeleton { get; }

        public string GoldenSparksPluginPath(string name)
        {
            return GOLDENSPARKS_PLUGINS_SOURCE_DIR + name + FileExtension;
        }

        public static List<ICompiler> Compilers = new List<ICompiler>()
        {
            new CSCompiler()
        };


        public static string FormatSource(string source, params string[] args)
        {
            // Always use \r\n line endings so it looks correct in Notepad
            source = source.Replace(@"\t", "\t");
            source = source.Replace("\n", "\r\n");
            return string.Format(source, args);
        }

        /// <summary> Generates source code for an example GoldenSparks plugin, 
        /// preformatted with the given name and creator </summary>
        public string GenExampleGoldenSparksPlugin(string GoldenSparksplugin, string creator)
        {
            return FormatSource(GoldenSparksPluginSkeleton, GoldenSparksplugin, creator, Server.Version);
        }


        /// <summary> Attempts to compile the given source code files to a .dll file. </summary>
        /// <param name="logErrors"> Whether to log compile errors to ERROR_LOG_PATH </param>
        public ICompilerErrors Compile(string[] srcPaths, string dstPath, bool logErrors)
        {
            foreach (string path in srcPaths)
            {
                string contents = File.ReadAllText(path);
                contents = contents.Replace(" : Plugin", " : GoldenSparksPlugin");
                contents = contents.Replace("using GoldenSparks", "using Flames");
                contents = contents.Replace("namespace GoldenSparks", "namespace Flames");
                File.WriteAllText(path, contents);
            }
            ICompilerErrors errors = DoCompile(srcPaths, dstPath);
            if (!errors.HasErrors || !logErrors) return errors;

            SourceMap sources = new SourceMap(srcPaths);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("############################################################");
            sb.AppendLine("Errors when compiling " + srcPaths.Join());
            sb.AppendLine("############################################################");
            sb.AppendLine();

            foreach (ICompilerError err in errors)
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

            using (StreamWriter w = new StreamWriter(ERROR_LOG_PATH, true))
            {
                w.Write(sb.ToString());
            }
            return errors;
        }

        public static string DescribeError(ICompilerError err, string[] srcs, string text)
        {
            string type = err.IsWarning ? "Warning" : "Error";
            string file = Path.GetFileName(err.FileName);

            // Include filename if compiling multiple source code files
            return string.Format("{0}{1}{2}{3}", type, text,
                                 err.Line > 0 ? " on line " + err.Line : "",
                                 srcs.Length > 1 ? " in " + file : "");
        }


        /// <summary> Compiles the given source code. </summary>
        public abstract ICompilerErrors DoCompile(string[] srcPaths, string dstPath);


        /// <summary> Converts source file paths to full paths, 
        /// then returns list of parsed referenced assemblies </summary>
        public List<string> ProcessInput(string[] srcPaths, string commentPrefix)
        {
            List<string> referenced = new List<string>();

            for (int i = 0; i < srcPaths.Length; i++)
            {
                // CodeDomProvider doesn't work properly with relative paths
                string path = Path.GetFullPath(srcPaths[i]);

                AddReferences(path, commentPrefix, referenced);
                srcPaths[i] = path;
            }

            referenced.Add(Server.GetServerDLLPath());
            return referenced;
        }
        public void AddReferences(string path, string commentPrefix, List<string> referenced)
        {
            // Allow referencing other assemblies using '//reference [assembly name]' at top of the file
            using (StreamReader r = new StreamReader(path))
            {
                string refPrefix = commentPrefix + "reference ";
                string plgPrefix = commentPrefix + "pluginref ";
                string newplgPrefix = commentPrefix + "goldensparkspluginref ";
                string line;

                while ((line = r.ReadLine()) != null)
                {
                    if (line.CaselessStarts(refPrefix))
                    {
                        referenced.Add(GetDLL(line));
                    }
                    else if (line.CaselessStarts(plgPrefix))
                    {
                        path = Path.Combine(IScripting.GOLDENSPARKS_PLUGINS_DLL_DIR, GetDLL(line));
                        referenced.Add(Path.GetFullPath(path));
                    }
                    else if (line.CaselessStarts(newplgPrefix))
                    {
                        path = Path.Combine(IScripting.GOLDENSPARKS_PLUGINS_DLL_DIR, GetDLL(line));
                        referenced.Add(Path.GetFullPath(path));
                    }
                    else
                    {
                        ProcessInputLine(line, referenced);
                    }
                }
            }
        }

        public virtual void ProcessInputLine(string line, List<string> referenced)
        {
        }

        public static string GetDLL(string line)
        {
            int index = line.IndexOf(' ') + 1;
            // For consistency with C#, treat '//reference X.dll;' as '//reference X.dll'
            return line.Substring(index).Replace(";", "");
        }
    }

    public class ICompilerErrors : List<ICompilerError>
    {
        public bool HasErrors
        {
            get { return FindIndex(ce => !ce.IsWarning) >= 0; }
        }
    }

    public class ICompilerError
    {
        public int Line, Column;
        public string ErrorNumber, ErrorText;
        public bool IsWarning;
        public string FileName;
    }


    public class SourceMap
    {
        public string[] files;
        public List<string>[] sources;

        public SourceMap(string[] paths)
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
    /// <summary> Compiles C# source files into a .dll by invoking a compiler executable directly </summary>
    public abstract class CommandLineCompiler
    {
        public ICompilerErrors Compile(string[] srcPaths, string dstPath, List<string> referenced)
        {
            string args = GetCommandLineArguments(srcPaths, dstPath, referenced);
            string exe = GetExecutable();

            ICompilerErrors errors = new ICompilerErrors();
            List<string> output = new List<string>();
            int retValue = Compile(exe, GetCompilerArgs(exe, args), output);

            // Only look for errors/warnings if the compile failed
            // TODO still log warnings anyways error when success?
            if (retValue != 0)
            {
                foreach (string line in output)
                {
                    ProcessCompilerOutputLine(errors, line);
                }
            }
            return errors;
        }


        public virtual string GetCommandLineArguments(string[] srcPaths, string dstPath,
                                                         List<string> referencedAssemblies)
        {
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

        public virtual void AddCoreAssembly(StringBuilder sb)
        {
            string coreAssemblyFileName = typeof(object).Assembly.Location;

            if (!string.IsNullOrEmpty(coreAssemblyFileName))
            {
                sb.Append("/nostdlib+ ");
                sb.AppendFormat("/R:{0} ", Quote(coreAssemblyFileName));
            }
        }

        public abstract void AddReferencedAssemblies(StringBuilder sb, List<string> referenced);

        public static string Quote(string value)
        {
            return "\"" + value.Trim() + "\"";
        }

        public abstract string GetExecutable();
        public abstract string GetCompilerArgs(string exe, string args);


        public static int Compile(string path, string args, List<string> output)
        {
            // https://stackoverflow.com/questions/285760/how-to-spawn-a-process-and-capture-its-stdout-in-net
            ProcessStartInfo psi = CreateStartInfo(path, args);

            using (Process p = new Process())
            {
                p.OutputDataReceived += (s, e) => { if (e.Data != null) output.Add(e.Data); };
                p.ErrorDataReceived += (s, e) => { }; // swallow stderr output

                p.StartInfo = psi;
                p.Start();

                p.BeginOutputReadLine();
                p.BeginErrorReadLine();

                if (!p.WaitForExit(120 * 1000))
                    throw new InvalidOperationException("C# compiler ran for over two minutes! Giving up..");

                return p.ExitCode;
            }
        }

        public static ProcessStartInfo CreateStartInfo(string path, string args)
        {
            ProcessStartInfo psi = new ProcessStartInfo(path, args)
            {
                WorkingDirectory = Environment.CurrentDirectory,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
            return psi;
        }


        public static Regex outputRegWithFileAndLine;
        public static Regex outputRegSimple;

        public static void ProcessCompilerOutputLine(ICompilerErrors errors, string line)
        {
            if (outputRegSimple == null)
            {
                outputRegWithFileAndLine =
                    new Regex(@"(^(.*)(\(([0-9]+),([0-9]+)\)): )(error|warning) ([A-Z]+[0-9]+) ?: (.*)");
                outputRegSimple =
                    new Regex(@"(error|warning) ([A-Z]+[0-9]+) ?: (.*)");
            }

            //First look for full file info
            Match m = outputRegWithFileAndLine.Match(line);
            bool full;
            if (m.Success)
            {
                full = true;
            }
            else
            {
                m = outputRegSimple.Match(line);
                full = false;
            }

            if (!m.Success) return;
            ICompilerError ce = new ICompilerError();

            if (full)
            {
                ce.FileName = m.Groups[2].Value;
                ce.Line = NumberUtils.ParseInt32(m.Groups[4].Value);
                ce.Column = NumberUtils.ParseInt32(m.Groups[5].Value);
            }

            ce.IsWarning = m.Groups[full ? 6 : 1].Value.CaselessEq("warning");
            ce.ErrorNumber = m.Groups[full ? 7 : 2].Value;
            ce.ErrorText = m.Groups[full ? 8 : 3].Value;
            errors.Add(ce);
        }
    }

    public class ClassicCSharpCompiler : CommandLineCompiler
    {
        public override void AddCoreAssembly(StringBuilder sb)
        {
            string coreAssemblyFileName = typeof(object).Assembly.Location;

            if (!string.IsNullOrEmpty(coreAssemblyFileName))
            {
                sb.Append("/nostdlib+ ");
                sb.AppendFormat("/R:{0} ", Quote(coreAssemblyFileName));
            }
        }

        public override void AddReferencedAssemblies(StringBuilder sb, List<string> referenced)
        {
            foreach (string path in referenced)
            {
                sb.AppendFormat("/R:{0} ", Quote(path));
            }
        }


        public override string GetExecutable()
        {
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

        public override string GetCompilerArgs(string exe, string args)
        {
            return args;
        }
    }
    public class CSCompiler : ICompiler
    {
        public override string FileExtension { get { return ".cs"; } }
        public override string ShortName { get { return "C#"; } }
        public override string FullName { get { return "CSharp"; } }

        public override ICompilerErrors DoCompile(string[] srcPaths, string dstPath)
        {
            List<string> referenced = ProcessInput(srcPaths, "//");

            CommandLineCompiler compiler = new ClassicCSharpCompiler();
            return compiler.Compile(srcPaths, dstPath, referenced);
        }

        public override string GoldenSparksPluginSkeleton
        {
            get
            {
                return @"//\tAuto-generated plugin skeleton class
//\tUse this as a basis for custom Flames GoldenSparks plugins

// To reference other assemblies, put a ""//reference [assembly filename]"" at the top of the file
//   e.g. to reference the System.Data assembly, put ""//reference System.Data.dll""

// Add any other using statements you need after this
using System;

namespace Flames
{{
\tpublic class {0} : GoldenSparksPlugin
\t{{
\t\t// The GoldenSparks plugin's name (i.e what shows in /GSPlugins)
\t\tpublic override string name {{ get {{ return ""{0}""; }} }}

\t\t// The oldest version of Flames this GoldenSparks plugin is compatible with
\t\tpublic override string Flames_Version {{ get {{ return ""{2}""; }} }}

\t\t// Message displayed in server logs when this GoldenSparks plugin is loaded
\t\tpublic override string welcome {{ get {{ return ""Loaded Message!""; }} }}

\t\t// Who created/authored this GoldenSparks plugin
\t\tpublic override string creator {{ get {{ return ""{1}""; }} }}

\t\t// Called when this GoldenSparks plugin is being loaded (e.g. on server startup)
\t\tpublic override void Load(bool startup)
\t\t{{
\t\t\t//code to hook into events, load state/resources etc goes here
\t\t}}

\t\t// Called when this GoldenSparks plugin is being unloaded (e.g. on server shutdown)
\t\tpublic override void Unload(bool shutdown)
\t\t{{
\t\t\t//code to unhook from events, dispose of state/resources etc goes here
\t\t}}

\t\t// Displays help for or information about this GoldenSparks plugin
\t\tpublic override void Help(Player p)
\t\t{{
\t\t\tp.Message(""No help is available for this GoldenSparks plugin."");
\t\t}}
\t}}
}}";
            }
        }
    }
    public static class CompilerOperations
    {
        public static ICompiler GetCompiler(Player p, string name)
        {
            if (name.Length == 0) return ICompiler.Compilers[0];

            foreach (ICompiler comp in ICompiler.Compilers)
            {
                if (comp.ShortName.CaselessEq(name)) return comp;
            }

            p.Message("&WUnknown language \"{0}\"", name);
            p.Message("&HAvailable languages: &f{0}",
                      ICompiler.Compilers.Join(c => c.ShortName + " (" + c.FullName + ")"));
            return null;
        }

        public static bool CreateGoldenSparksPlugin(Player p, string name, ICompiler compiler)
        {
            string path = compiler.GoldenSparksPluginPath(name);
            string creator = p.IsSuper ? Colors.Strip(Server.Config.Name) : p.truename;
            string source = compiler.GenExampleGoldenSparksPlugin(name, creator);

            return CreateFile(p, name, path, "goldensparksplugin &f", source);
        }

        public static bool CreateFile(Player p, string name, string path, string type, string source)
        {
            if (File.Exists(path))
            {
                p.Message("File {0} already exists. Choose another name.", path);
                return false;
            }

            File.WriteAllText(path, source);
            p.Message("Successfully saved example {2}{0} &Sto {1}", name, path, type);
            return true;
        }


        /// <summary> Attempts to compile the given source code files into a .dll </summary>
        /// <param name="p"> Player to send messages to </param>
        /// <param name="type"> Type of files being compiled (e.g. GoldenSparks plugin) </param>
        /// <param name="srcs"> Path of the source code files </param>
        /// <param name="dst"> Path to the destination .dll </param>
        /// <returns> Whether compilation succeeded </returns>
        public static bool Compile(Player p, ICompiler compiler, string type, string[] srcs, string dst)
        {
            foreach (string path in srcs)
            {
                if (File.Exists(path)) continue;

                p.Message("File &9{0} &Snot found.", path);
                return false;
            }
            ICompilerErrors errors = compiler.Compile(srcs, dst, true);
            if (!errors.HasErrors)
            {
                p.Message("{0} compiled successfully from {1}",
                        type, srcs.Join(file => Path.GetFileName(file)));
                return true;
            }

            SummariseErrors(errors, srcs, p);
            return false;
        }

        public const int MAX_LOG = 5;
        public static void SummariseErrors(ICompilerErrors errors, string[] srcs, Player p)
        {
            int logged = 0;
            foreach (ICompilerError err in errors)
            {
                p.Message("&W{1} - {0}", err.ErrorText,
                          ICompiler.DescribeError(err, srcs, " #" + err.ErrorNumber));
                logged++;
                if (logged >= MAX_LOG) break;
            }

            if (logged < errors.Count)
            {
                p.Message(" &W.. and {0} more", errors.Count - logged);
            }
            p.Message("&WCompiling failed. See " + ICompiler.ERROR_LOG_PATH + " for more detail");
        }
    }
    public class GoldenSparksCompilerPlugin : GoldenSparksPlugin
    {
        public override string name { get { return "GoldenSparksCompiler"; } }

        public Command GoldenSparksCmdCreate = new CmdGoldenSparksPluginCreate();
        public Command GoldenSparksCmdCompile = new CmdGoldenSparksPluginCompile();
        public Command GoldenSparksCmdCompLoad = new CmdGoldenSparksPluginCompLoad();
        public Command GoldenSparksCmdPlugin = new CmdGoldenSparksPlugin();

        public override void Load(bool startup)
        {
            Server.EnsureDirectoryExists(ICompiler.GOLDENSPARKS_PLUGINS_SOURCE_DIR);
            Command.Register(GoldenSparksCmdCreate, GoldenSparksCmdCompile, GoldenSparksCmdCompLoad, GoldenSparksCmdPlugin);
        }

        public override void Unload(bool shutdown)
        {
            Command.Unregister(GoldenSparksCmdCreate, GoldenSparksCmdCompile, GoldenSparksCmdCompLoad, GoldenSparksCmdPlugin);
        }
    }
}
namespace Flames.GoldenSparksScripting
{
    public static class ScriptingOperations
    {
        public static bool LoadGoldenSparksPlugins(Player p, string path)
        {
            if (!File.Exists(path))
            {
                p.Message("File &9{0} &Snot found.", path);
                return false;
            }

            try
            {
                List<GoldenSparksPlugin> GoldenSparksplugins = IScripting.LoadGoldenSparksPlugin(path, false);

                p.Message("GoldenSparks plugin {0} loaded successfully",
                          GoldenSparksplugins.Join(pl => pl.name));
                return true;
            }
            catch (AlreadyLoadedException ex)
            {
                p.Message(ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                p.Message(IScripting.DescribeLoadError(path, ex));
                Logger.LogError("Error loading GoldenSparks plugins from " + path, ex);
                return false;
            }
        }

        public static bool UnloadGoldenSparksPlugin(Player p, GoldenSparksPlugin GoldenSparksplugin)
        {
            if (!GoldenSparksPlugin.Unload(GoldenSparksplugin))
            {
                p.Message("&WError unloading GoldenSparks plugin. See error logs for more information.");
                return false;
            }

            p.Message("GoldenSparks plugin {0} &Sunloaded successfully", GoldenSparksplugin.name);
            return true;
        }
    }
    /// <summary> Exception raised when attempting to load a GoldenSparks plugin 
    /// that has the same name as an already loaded GoldenSparks plugin </summary>
    public class AlreadyLoadedException : Exception
    {
        public AlreadyLoadedException(string msg) : base(msg)
        {
        }
    }

    /// <summary> Utility methods for loading assemblies, and GoldenSparks plugins </summary>
    public static class IScripting
    {
        public static string GetExePath(string path)
        {
            return path;
        }

        public static Assembly ResolveGoldenSparksPluginReference(string name)
        {
            return null;
        }

        public const string GOLDENSPARKS_PLUGINS_DLL_DIR = "goldensparksplugins/";

        /// <summary> Returns the default .dll path for the GoldenSparks plugin with the given name </summary>
        public static string GoldenSparksPluginPath(string name)
        {
            return GOLDENSPARKS_PLUGINS_DLL_DIR + name + ".dll";
        }


        public static void Init()
        {
            Directory.CreateDirectory(GOLDENSPARKS_PLUGINS_DLL_DIR);
            AppDomain.CurrentDomain.AssemblyResolve += ResolveGoldenSparksPluginAssembly;
        }

        // only used for resolving GoldenSparks plugin DLLs depending on other GoldenSparks plugin DLLs
        public static Assembly ResolveGoldenSparksPluginAssembly(object sender, ResolveEventArgs args)
        {
            // This property only exists in .NET framework 4.0 and later
            Assembly requestingAssembly = args.RequestingAssembly;

            if (requestingAssembly == null) return null;
            if (!IsGoldenSparksPluginDLL(requestingAssembly)) return null;

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assem in assemblies)
            {
                if (!IsGoldenSparksPluginDLL(assem)) continue;

                if (args.Name == assem.FullName) return assem;
            }

            Assembly coreRef = ResolveGoldenSparksPluginReference(args.Name);
            if (coreRef != null) return coreRef;

            Logger.Log(LogType.Warning, "Custom GoldenSparks plugin [{0}] tried to load [{1}], but it could not be found",
                       requestingAssembly.FullName, args.Name);
            return null;
        }

        public static bool IsGoldenSparksPluginDLL(Assembly a)
        {
            return string.IsNullOrEmpty(a.Location);
        }


        /// <summary> Constructs instances of all types which derive from T in the given assembly. </summary>
        /// <returns> The list of constructed instances. </returns>
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

        /// <summary> Loads the given assembly from disc (and associated .pdb debug data) </summary>
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


        public static string DescribeLoadError(string path, Exception ex)
        {
            string file = Path.GetFileName(path);

            if (ex is BadImageFormatException)
            {
                return "&W" + file + " is not a valid assembly, or has an invalid dependency. Details in the error log.";
            }
            else if (ex is FileLoadException)
            {
                return "&W" + file + " or one of its dependencies could not be loaded. Details in the error log.";
            }

            return "&WAn unknown error occured. Details in the error log.";
            // p.Message("&WError loading GoldenSparks plugin. See error logs for more information.");
        }


        public static void AutoloadGoldenSparksPlugins()
        {
            string[] files = AtomicIO.TryGetFiles(GOLDENSPARKS_PLUGINS_DLL_DIR, "*.dll");
            if (files == null) return;

            // Ensure that GoldenSparks plugin files are loaded in a consistent order,
            //  in case GoldenSparks plugins have a dependency on other GoldenSparks plugins
            Array.Sort(files);

            foreach (string path in files)
            {
                try
                {
                    LoadGoldenSparksPlugin(path, true);
                }
                catch (Exception ex)
                {
                    Logger.LogError("Error loading GoldenSparks plugins from " + path, ex);
                }
            }
        }

        /// <summary> Loads all GoldenSparks plugins from the given .dll path. </summary>
        public static List<GoldenSparksPlugin> LoadGoldenSparksPlugin(string path, bool auto)
        {
            Assembly lib = LoadAssembly(path);
            List<GoldenSparksPlugin> GoldenSparksplugins = LoadTypes<GoldenSparksPlugin>(lib);

            foreach (GoldenSparksPlugin pl in GoldenSparksplugins)
            {
                if (GoldenSparksPlugin.FindGoldenSparksCustom(pl.name) != null)
                    throw new AlreadyLoadedException("GoldenSparks plugin " + pl.name + " is already loaded");

                GoldenSparksPlugin.Load(pl, auto);
            }
            return GoldenSparksplugins;
        }
    }
}

namespace Flames
{
    public class GoldenSparksPluginLoader : Plugin
    {
        public override string name { get { return "GoldenSparksPluginLoader"; } }
        public override string creator { get { return Colors.Strip(Server.SoftwareName + " team"); } }
        public static GoldenSparksCompilerPlugin goldensparkscompilerplugin = new GoldenSparksCompilerPlugin();
        public override void Load(bool startup)
        {
            GoldenSparksPlugin.LoadCoreGoldenSparksPlugin(goldensparkscompilerplugin, startup);
            GoldenSparksPlugin.LoadAll();
            OnShuttingDownEvent.Register(OnShutdown, Priority.Critical);
        }
        public void OnShutdown(bool restarting, string message)
        {
            GoldenSparksPlugin.UnloadAll();
        }
        public override void Unload(bool shutdown)
        {
            GoldenSparksPlugin.UnloadAll();
            GoldenSparksPlugin.UnloadGoldenSparksPlugin(goldensparkscompilerplugin, shutdown);
            OnShuttingDownEvent.Unregister(OnShutdown);
        }
        public override void Help(Player p)
        {
            p.Message("");
        }
    }
}
namespace Flames
{
    /// <summary> This class provides for more advanced modification to Flames 
    /// using MCGalaxy's newer compiler plugin </summary>
    public abstract class GoldenSparksPlugin
    {
        /// <summary> Hooks into events and initalises states/resources etc </summary>
        /// <param name="auto"> True if GoldenSparks plugin is being automatically loaded (e.g. on server startup), false if manually. </param>
        public abstract void Load(bool auto);

        /// <summary> Unhooks from events and disposes of state/resources etc </summary>
        /// <param name="auto"> True if GoldenSparks plugin is being auto unloaded (e.g. on server shutdown), false if manually. </param>
        public abstract void Unload(bool auto);

        /// <summary> Called when a player does /Help on the GoldenSparks plugin. Typically tells the player what this GoldenSparks plugin is about. </summary>
        /// <param name="p"> Player who is doing /Help. </param>
        public virtual void Help(Player p)
        {
            p.Message("No help is available for this GoldenSparks plugin.");
        }

        /// <summary> Name of the GoldenSparks plugin. </summary>
        public abstract string name { get; }
        /// <summary> The oldest version of Flames this GoldenSparks plugin is compatible with. </summary>
        public virtual string Flames_Version { get { return "9.0.6.4"; } }
        /// <summary> The oldest version of Flames this GoldenSparks plugin is compatible with. </summary>
        public virtual string GoldenSparks_Version { get { return null; } }
        /// <summary> Work on backwards compatibility with MCGalaxy </summary>
        public virtual string MCGalaxy_Version { get { return null; } }
        /// <summary> Version of this GoldenSparks plugin. </summary>
        public virtual int build { get { return 0; } }
        /// <summary> Message to display once this GoldenSparks plugin is loaded. </summary>
        public virtual string welcome { get { return ""; } }
        /// <summary> The creator/author of this GoldenSparks plugin. (Your name) </summary>
        public virtual string creator { get { return ""; } }
        /// <summary> Whether or not to auto load this GoldenSparks plugin on server startup. </summary>
        public virtual bool LoadAtStartup { get { return true; } }


        /// <summary> List of GoldenSparks plugins/modules included in the server software </summary>
        public static List<GoldenSparksPlugin> CoreGoldenSparksPlugins = new List<GoldenSparksPlugin>();
        public static List<GoldenSparksPlugin> CustomGoldenSparksPlugins = new List<GoldenSparksPlugin>();


        public static GoldenSparksPlugin FindGoldenSparksCustom(string name)
        {
            foreach (GoldenSparksPlugin pl in CustomGoldenSparksPlugins)
            {
                if (pl.name.CaselessEq(name)) return pl;
            }
            return null;
        }

        public static void Load(GoldenSparksPlugin pl, bool auto)
        {
            string ver = pl.Flames_Version;
            string MCGalaxy_Ver = "1.9.4.9";
            // Version different in Dev build, use normal for GoldenSparks plugins
            string CurrentVersion = Server.FlamesVersion;

            if (!string.IsNullOrEmpty(pl.MCGalaxy_Version) && new Version(pl.MCGalaxy_Version) > new Version(MCGalaxy_Ver))
            {
                string msg = string.Format("GoldenSparks plugin '{0}' cannot be loaded on this version of {1}!", pl.name, Server.SoftwareName);
                throw new InvalidOperationException(msg);
            }
            if (!string.IsNullOrEmpty(ver) && new Version(ver) > new Version(CurrentVersion) && new Version(ver) > new Version(Server.Version))
            {
                string msg = string.Format("GoldenSparks plugin '{0}' requires a more recent version of {1}!", pl.name, Server.SoftwareName);
                throw new InvalidOperationException(msg);
            }

            try
            {
                CustomGoldenSparksPlugins.Add(pl);

                if (pl.LoadAtStartup || !auto)
                {
                    pl.Load(auto);
                    Logger.Log(LogType.SystemActivity, "GoldenSparks plugin {0} loaded...build: {1}", pl.name, pl.build);
                }
                else
                {
                    Logger.Log(LogType.SystemActivity, "GoldenSparks plugin {0} was not loaded, you can load it with /gspload", pl.name);
                }

                if (!string.IsNullOrEmpty(pl.welcome)) Logger.Log(LogType.SystemActivity, pl.welcome);
            }
            catch
            {
                if (!string.IsNullOrEmpty(pl.creator)) Logger.Log(LogType.Warning, "You can go bug {0} about {1} failing to load.", pl.creator, pl.name);
                throw;
            }
        }
        public static bool Unload(GoldenSparksPlugin pl)
        {
            bool success = UnloadGoldenSparksPlugin(pl, false);

            // TODO only remove if successful?
            CustomGoldenSparksPlugins.Remove(pl);
            CoreGoldenSparksPlugins.Remove(pl);
            return success;
        }

        public static bool UnloadGoldenSparksPlugin(GoldenSparksPlugin pl, bool auto)
        {
            try
            {
                pl.Unload(auto);
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError("Error unloading GoldenSparks plugin " + pl.name, ex);
                return false;
            }
        }
        public static void UnloadAll()
        {
            for (int i = 0; i < CustomGoldenSparksPlugins.Count; i++)
            {
                UnloadGoldenSparksPlugin(CustomGoldenSparksPlugins[i], true);
            }
            CustomGoldenSparksPlugins.Clear();

            for (int i = 0; i < CoreGoldenSparksPlugins.Count; i++)
            {
                UnloadGoldenSparksPlugin(CoreGoldenSparksPlugins[i], true);
            }
        }
        public static void LoadAll()
        {
            LoadCoreGoldenSparksPlugin(new GoldenSparksCompilerPlugin());
            IScripting.AutoloadGoldenSparksPlugins();
        }
        public static void LoadCoreGoldenSparksPlugin(GoldenSparksPlugin GoldenSparksplugin, bool startup)
        {
            LoadCoreGoldenSparksPlugin(GoldenSparksplugin);
        }
        public static void LoadCoreGoldenSparksPlugin(GoldenSparksPlugin GoldenSparksplugin)
        {
            List<string> disabled = Server.Config.DisabledModules;
            if (disabled.CaselessContains(GoldenSparksplugin.name)) return;
            GoldenSparksplugin.Load(true);
            CoreGoldenSparksPlugins.Add(GoldenSparksplugin);
        }
    }
}
#endif