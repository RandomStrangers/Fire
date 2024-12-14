#if CORE
using System;
using System.Collections.Generic;
using Flames.SuperNovaScripting;
using Flames.Events.ServerEvents;
using Flames.Modules.SuperNovaCompiling;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using Flames.Commands;
namespace Flames.Modules.SuperNovaCompiling
{
    public class CmdSuperNovaPluginCompile : Command
    {
        public override string name { get { return "SuperNovaPluginCompile"; } }
        public override string shortcut { get { return "SNPCompile"; } }
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
            CompileSuperNovaPlugin(p, paths, compiler);
        }
        public virtual void CompileSuperNovaPlugin(Player p, string[] paths, ICompiler compiler)
        {
            string dstPath = IScripting.SuperNovaPluginPath(paths[0]);
            for (int i = 0; i < paths.Length; i++)
            {
                paths[i] = compiler.SuperNovaPluginPath(paths[i]);
            }
            CompilerOperations.Compile(p, compiler, "SuperNovaPlugin", paths, dstPath);
        }

        public override void Help(Player p)
        {
            ICompiler compiler = ICompiler.Compilers[0];
            p.Message("&T/SuperNovaPluginCompile [plugin name]");
            p.Message("&HCompiles a .cs file containing a  C# SuperNova plugin into a DLL");
            p.Message("&H  Compiles from &f{0}", compiler.SuperNovaPluginPath("&H<name>&f"));
        }
    }
    public class CmdSuperNovaPluginCompLoad : CmdSuperNovaPluginCompile
    {
        public override string name { get { return "SuperNovaPluginCompLoad"; } }
        public override string shortcut { get { return "sncml"; } }
        public override CommandAlias[] Aliases
        {
            get
            {
                return new[] {
                    new CommandAlias("snpcompload"),
                    new CommandAlias("snpcml"),
                };
            }
        }
        public override void CompileSuperNovaPlugin(Player p, string[] paths, ICompiler compiler)
        {
            string dst = IScripting.SuperNovaPluginPath(paths[0]);
            UnloadSuperNovaPlugin(p, paths[0]);
            base.CompileSuperNovaPlugin(p, paths, compiler);
            ScriptingOperations.LoadSuperNovaPlugins(p, dst);
        }
        public static void UnloadSuperNovaPlugin(Player p, string name)
        {
            SuperNovaPlugin SuperNovaplugin = SuperNovaPlugin.FindSuperNovaCustom(name);

            if (SuperNovaplugin == null) return;
            ScriptingOperations.UnloadSuperNovaPlugin(p, SuperNovaplugin);
        }
        public override void Help(Player p)
        {
            p.Message("&T/SuperNovaPluginCompLoad [plugin]");
            p.Message("&HCompiles and loads (or reloads) a C# SuperNova plugin into the server");
        }
    }
    public class CmdSuperNovaPlugin : Command
    {
        public override string name { get { return "SuperNovaPlugin"; } }
        public override string type { get { return CommandTypes.Added; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Owner; } }
        public override CommandAlias[] Aliases
        {
            get
            {
                return new[] { new CommandAlias("SNPLoad", "load"), new CommandAlias("SNPUnload", "unload"),
                    new CommandAlias("SNPlugins", "list") };
            }
        }
        public override bool MessageBlockRestricted { get { return true; } }
        public override void Use(Player p, string message)
        {
            string[] args = message.SplitSpaces(2);
            if (IsListAction(args[0]))
            {
                string modifier = args.Length > 1 ? args[1] : "";

                p.Message("Loaded SuperNova plugins:");
                Paginator.Output(p, SuperNovaPlugin.CustomSuperNovaPlugins, pl => pl.name,
                                 "SuperNova", "supernovaplugins", modifier);
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
                string path = IScripting.SuperNovaPluginPath(name);
                ScriptingOperations.LoadSuperNovaPlugins(p, path);
            }
            else if (cmd.CaselessEq("unload"))
            {
                UnloadSuperNovaPlugin(p, name);
            }
            else if (cmd.CaselessEq("create"))
            {
                Find("SuperNovaPluginCreate").Use(p, name);
            }
            else if (cmd.CaselessEq("compile"))
            {
                Find("SuperNovaPluginCompile").Use(p, name);
            }
            else
            {
                Help(p);
            }
        }
        public static void UnloadSuperNovaPlugin(Player p, string name)
        {
            int matches;
            SuperNovaPlugin SuperNovaplugin = Matcher.Find(p, name, out matches, SuperNovaPlugin.CustomSuperNovaPlugins,
                                         null, pln => pln.name, "supernovaplugins");
            if (SuperNovaplugin == null) return;
            ScriptingOperations.UnloadSuperNovaPlugin(p, SuperNovaplugin);
        }
        public override void Help(Player p)
        {
            p.Message("&T/SuperNovaPlugin load [filename]");
            p.Message("&HLoad a compiled plugin from the &fSuperNova plugins &Hfolder");
            p.Message("&T/SuperNovaPlugin unload [name]");
            p.Message("&HUnloads a currently loaded SuperNova plugin");
            p.Message("&T/SuperNovaPlugin list");
            p.Message("&HLists all loaded SuperNova plugins");
        }
    }
    public class CmdSuperNovaPluginCreate : CmdSuperNovaPluginCompile
    {
        public override string name { get { return "SuperNovaPluginCreate"; } }
        public override string shortcut { get { return "SNPCreate"; } }


        public override void CompileSuperNovaPlugin(Player p, string[] paths, ICompiler compiler)
        {
            foreach (string cmd in paths)
            {
                CompilerOperations.CreateSuperNovaPlugin(p, cmd, compiler);
            }
        }

        public override void Help(Player p)
        {
            p.Message("&T/SuperNovaPluginCreate [name]");
            p.Message("&HCreate an example C# SuperNova plugin named [name]");
        }
    }
    /// <summary> Compiles source code files for a particular programming language into a .dll </summary>
    public abstract class ICompiler
    {
        public const string SUPERNOVA_PLUGINS_SOURCE_DIR = "supernovaplugins/";
        public const string ERROR_LOG_PATH = "logs/errors/compiler_sn.log";

        /// <summary> Default file extension used for source code files </summary>
        /// <example> .cs, .vb </example>
        public abstract string FileExtension { get; }
        /// <summary> The short name of this programming language </summary>
        /// <example> C#, VB </example>
        public abstract string ShortName { get; }
        /// <summary> The full name of this programming language </summary>
        /// <example> CSharp, Visual Basic </example>
        public abstract string FullName { get; }
        /// <summary> Returns source code for an example SuperNova plugin </summary>
        public abstract string SuperNovaPluginSkeleton { get; }

        public string SuperNovaPluginPath(string name)
        {
            return SUPERNOVA_PLUGINS_SOURCE_DIR + name + FileExtension;
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

        /// <summary> Generates source code for an example SuperNova plugin, 
        /// preformatted with the given name and creator </summary>
        public string GenExampleSuperNovaPlugin(string SuperNovaplugin, string creator)
        {
            return FormatSource(SuperNovaPluginSkeleton, SuperNovaplugin, creator, Server.Version);
        }


        /// <summary> Attempts to compile the given source code files to a .dll file. </summary>
        /// <param name="logErrors"> Whether to log compile errors to ERROR_LOG_PATH </param>
        public ICompilerErrors Compile(string[] srcPaths, string dstPath, bool logErrors)
        {
            foreach (string path in srcPaths)
            {
                string contents = File.ReadAllText(path);
                contents = contents.Replace(" : Plugin", " : SuperNovaPlugin");
                contents = contents.Replace("using SuperNova", "using Flames");
                contents = contents.Replace("namespace SuperNova", "namespace Flames");
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
                string newplgPrefix = commentPrefix + "supernovapluginref ";
                string line;

                while ((line = r.ReadLine()) != null)
                {
                    if (line.CaselessStarts(refPrefix))
                    {
                        referenced.Add(GetDLL(line));
                    }
                    else if (line.CaselessStarts(plgPrefix))
                    {
                        path = Path.Combine(IScripting.SUPERNOVA_PLUGINS_DLL_DIR, GetDLL(line));
                        referenced.Add(Path.GetFullPath(path));
                    }
                    else if (line.CaselessStarts(newplgPrefix))
                    {
                        path = Path.Combine(IScripting.SUPERNOVA_PLUGINS_DLL_DIR, GetDLL(line));
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

        public override string SuperNovaPluginSkeleton
        {
            get
            {
                return @"//\tAuto-generated plugin skeleton class
//\tUse this as a basis for custom Flames SuperNova plugins

// To reference other assemblies, put a ""//reference [assembly filename]"" at the top of the file
//   e.g. to reference the System.Data assembly, put ""//reference System.Data.dll""

// Add any other using statements you need after this
using System;

namespace Flames
{{
\tpublic class {0} : SuperNovaPlugin
\t{{
\t\t// The SuperNova plugin's name (i.e what shows in /SNPlugins)
\t\tpublic override string name {{ get {{ return ""{0}""; }} }}

\t\t// The oldest version of Flames this SuperNova plugin is compatible with
\t\tpublic override string Flames_Version {{ get {{ return ""{2}""; }} }}

\t\t// Message displayed in server logs when this SuperNova plugin is loaded
\t\tpublic override string welcome {{ get {{ return ""Loaded Message!""; }} }}

\t\t// Who created/authored this SuperNova plugin
\t\tpublic override string creator {{ get {{ return ""{1}""; }} }}

\t\t// Called when this SuperNova plugin is being loaded (e.g. on server startup)
\t\tpublic override void Load(bool startup)
\t\t{{
\t\t\t//code to hook into events, load state/resources etc goes here
\t\t}}

\t\t// Called when this SuperNova plugin is being unloaded (e.g. on server shutdown)
\t\tpublic override void Unload(bool shutdown)
\t\t{{
\t\t\t//code to unhook from events, dispose of state/resources etc goes here
\t\t}}

\t\t// Displays help for or information about this SuperNova plugin
\t\tpublic override void Help(Player p)
\t\t{{
\t\t\tp.Message(""No help is available for this SuperNova plugin."");
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

        public static bool CreateSuperNovaPlugin(Player p, string name, ICompiler compiler)
        {
            string path = compiler.SuperNovaPluginPath(name);
            string creator = p.IsSuper ? Colors.Strip(Server.Config.Name) : p.truename;
            string source = compiler.GenExampleSuperNovaPlugin(name, creator);

            return CreateFile(p, name, path, "supernovaplugin &f", source);
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
        /// <param name="type"> Type of files being compiled (e.g. SuperNova plugin) </param>
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
    public class SuperNovaCompilerPlugin : SuperNovaPlugin
    {
        public override string name { get { return "SuperNovaCompiler"; } }

        public Command SuperNovaCmdCreate = new CmdSuperNovaPluginCreate();
        public Command SuperNovaCmdCompile = new CmdSuperNovaPluginCompile();
        public Command SuperNovaCmdCompLoad = new CmdSuperNovaPluginCompLoad();
        public Command SuperNovaCmdPlugin = new CmdSuperNovaPlugin();

        public override void Load(bool startup)
        {
            Server.EnsureDirectoryExists(ICompiler.SUPERNOVA_PLUGINS_SOURCE_DIR);
            Command.Register(SuperNovaCmdCreate, SuperNovaCmdCompile, SuperNovaCmdCompLoad, SuperNovaCmdPlugin);
        }

        public override void Unload(bool shutdown)
        {
            Command.Unregister(SuperNovaCmdCreate, SuperNovaCmdCompile, SuperNovaCmdCompLoad, SuperNovaCmdPlugin);
        }
    }
}
namespace Flames.SuperNovaScripting
{
    public static class ScriptingOperations
    {
        public static bool LoadSuperNovaPlugins(Player p, string path)
        {
            if (!File.Exists(path))
            {
                p.Message("File &9{0} &Snot found.", path);
                return false;
            }

            try
            {
                List<SuperNovaPlugin> SuperNovaplugins = IScripting.LoadSuperNovaPlugin(path, false);

                p.Message("SuperNova plugin {0} loaded successfully",
                          SuperNovaplugins.Join(pl => pl.name));
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
                Logger.LogError("Error loading SuperNova plugins from " + path, ex);
                return false;
            }
        }

        public static bool UnloadSuperNovaPlugin(Player p, SuperNovaPlugin SuperNovaplugin)
        {
            if (!SuperNovaPlugin.Unload(SuperNovaplugin))
            {
                p.Message("&WError unloading SuperNova plugin. See error logs for more information.");
                return false;
            }

            p.Message("SuperNova plugin {0} &Sunloaded successfully", SuperNovaplugin.name);
            return true;
        }
    }
    /// <summary> Exception raised when attempting to load a SuperNova plugin 
    /// that has the same name as an already loaded SuperNova plugin </summary>
    public class AlreadyLoadedException : Exception
    {
        public AlreadyLoadedException(string msg) : base(msg)
        {
        }
    }

    /// <summary> Utility methods for loading assemblies, and SuperNova plugins </summary>
    public static class IScripting
    {
        public static string GetExePath(string path)
        {
            return path;
        }

        public static Assembly ResolveSuperNovaPluginReference(string name)
        {
            return null;
        }

        public const string SUPERNOVA_PLUGINS_DLL_DIR = "supernovaplugins/";

        /// <summary> Returns the default .dll path for the SuperNova plugin with the given name </summary>
        public static string SuperNovaPluginPath(string name)
        {
            return SUPERNOVA_PLUGINS_DLL_DIR + name + ".dll";
        }


        public static void Init()
        {
            Directory.CreateDirectory(SUPERNOVA_PLUGINS_DLL_DIR);
            AppDomain.CurrentDomain.AssemblyResolve += ResolveSuperNovaPluginAssembly;
        }

        // only used for resolving SuperNova plugin DLLs depending on other SuperNova plugin DLLs
        public static Assembly ResolveSuperNovaPluginAssembly(object sender, ResolveEventArgs args)
        {
            // This property only exists in .NET framework 4.0 and later
            Assembly requestingAssembly = args.RequestingAssembly;

            if (requestingAssembly == null) return null;
            if (!IsSuperNovaPluginDLL(requestingAssembly)) return null;

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assem in assemblies)
            {
                if (!IsSuperNovaPluginDLL(assem)) continue;

                if (args.Name == assem.FullName) return assem;
            }

            Assembly coreRef = ResolveSuperNovaPluginReference(args.Name);
            if (coreRef != null) return coreRef;

            Logger.Log(LogType.Warning, "Custom SuperNova plugin [{0}] tried to load [{1}], but it could not be found",
                       requestingAssembly.FullName, args.Name);
            return null;
        }

        public static bool IsSuperNovaPluginDLL(Assembly a)
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
            // p.Message("&WError loading SuperNova plugin. See error logs for more information.");
        }


        public static void AutoloadSuperNovaPlugins()
        {
            string[] files = AtomicIO.TryGetFiles(SUPERNOVA_PLUGINS_DLL_DIR, "*.dll");
            if (files == null) return;

            // Ensure that SuperNova plugin files are loaded in a consistent order,
            //  in case SuperNova plugins have a dependency on other SuperNova plugins
            Array.Sort(files);

            foreach (string path in files)
            {
                try
                {
                    LoadSuperNovaPlugin(path, true);
                }
                catch (Exception ex)
                {
                    Logger.LogError("Error loading SuperNova plugins from " + path, ex);
                }
            }
        }

        /// <summary> Loads all SuperNova plugins from the given .dll path. </summary>
        public static List<SuperNovaPlugin> LoadSuperNovaPlugin(string path, bool auto)
        {
            Assembly lib = LoadAssembly(path);
            List<SuperNovaPlugin> SuperNovaplugins = LoadTypes<SuperNovaPlugin>(lib);

            foreach (SuperNovaPlugin pl in SuperNovaplugins)
            {
                if (SuperNovaPlugin.FindSuperNovaCustom(pl.name) != null)
                    throw new AlreadyLoadedException("SuperNova plugin " + pl.name + " is already loaded");

                SuperNovaPlugin.Load(pl, auto);
            }
            return SuperNovaplugins;
        }
    }
}

namespace Flames
{
    public class SuperNovaPluginLoader : Plugin
    {
        public override string name { get { return "SuperNovaPluginLoader"; } }
        public override string creator { get { return Colors.Strip(Server.SoftwareName + " team"); } }
        public static SuperNovaCompilerPlugin supernovacompilerplugin = new SuperNovaCompilerPlugin();
        public override void Load(bool startup)
        {
            SuperNovaPlugin.LoadCoreSuperNovaPlugin(supernovacompilerplugin, startup);
            SuperNovaPlugin.LoadAll();
            OnShuttingDownEvent.Register(OnShutdown, Priority.Critical);
        }
        public void OnShutdown(bool restarting, string message)
        {
            SuperNovaPlugin.UnloadAll();
        }
        public override void Unload(bool shutdown)
        {
            SuperNovaPlugin.UnloadAll();
            SuperNovaPlugin.UnloadSuperNovaPlugin(supernovacompilerplugin, shutdown);
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
    public abstract class SuperNovaPlugin
    {
        /// <summary> Hooks into events and initalises states/resources etc </summary>
        /// <param name="auto"> True if SuperNova plugin is being automatically loaded (e.g. on server startup), false if manually. </param>
        public abstract void Load(bool auto);

        /// <summary> Unhooks from events and disposes of state/resources etc </summary>
        /// <param name="auto"> True if SuperNova plugin is being auto unloaded (e.g. on server shutdown), false if manually. </param>
        public abstract void Unload(bool auto);

        /// <summary> Called when a player does /Help on the SuperNova plugin. Typically tells the player what this SuperNova plugin is about. </summary>
        /// <param name="p"> Player who is doing /Help. </param>
        public virtual void Help(Player p)
        {
            p.Message("No help is available for this SuperNova plugin.");
        }

        /// <summary> Name of the SuperNova plugin. </summary>
        public abstract string name { get; }
        /// <summary> The oldest version of Flames this SuperNova plugin is compatible with. </summary>
        public virtual string Flames_Version { get { return "9.0.6.4"; } }
        /// <summary> The oldest version of Flames this SuperNova plugin is compatible with. </summary>
        public virtual string SuperNova_Version { get { return null; } }
        /// <summary> Work on backwards compatibility with MCGalaxy </summary>
        public virtual string MCGalaxy_Version { get { return null; } }
        /// <summary> Version of this SuperNova plugin. </summary>
        public virtual int build { get { return 0; } }
        /// <summary> Message to display once this SuperNova plugin is loaded. </summary>
        public virtual string welcome { get { return ""; } }
        /// <summary> The creator/author of this SuperNova plugin. (Your name) </summary>
        public virtual string creator { get { return ""; } }
        /// <summary> Whether or not to auto load this SuperNova plugin on server startup. </summary>
        public virtual bool LoadAtStartup { get { return true; } }


        /// <summary> List of SuperNova plugins/modules included in the server software </summary>
        public static List<SuperNovaPlugin> CoreSuperNovaPlugins = new List<SuperNovaPlugin>();
        public static List<SuperNovaPlugin> CustomSuperNovaPlugins = new List<SuperNovaPlugin>();


        public static SuperNovaPlugin FindSuperNovaCustom(string name)
        {
            foreach (SuperNovaPlugin pl in CustomSuperNovaPlugins)
            {
                if (pl.name.CaselessEq(name)) return pl;
            }
            return null;
        }

        public static void Load(SuperNovaPlugin pl, bool auto)
        {
            string ver = pl.Flames_Version;
            string MCGalaxy_Ver = "1.9.4.9";
            // Version different in Dev build, use normal for SuperNova plugins
            string CurrentVersion = Server.FlamesVersion;

            if (!string.IsNullOrEmpty(pl.MCGalaxy_Version) && new Version(pl.MCGalaxy_Version) > new Version(MCGalaxy_Ver))
            {
                string msg = string.Format("SuperNova plugin '{0}' cannot be loaded on this version of {1}!", pl.name, Server.SoftwareName);
                throw new InvalidOperationException(msg);
            }
            if (!string.IsNullOrEmpty(ver) && new Version(ver) > new Version(CurrentVersion) && new Version(ver) > new Version(Server.Version))
            {
                string msg = string.Format("SuperNova plugin '{0}' requires a more recent version of {1}!", pl.name, Server.SoftwareName);
                throw new InvalidOperationException(msg);
            }

            try
            {
                CustomSuperNovaPlugins.Add(pl);

                if (pl.LoadAtStartup || !auto)
                {
                    pl.Load(auto);
                    Logger.Log(LogType.SystemActivity, "SuperNova plugin {0} loaded...build: {1}", pl.name, pl.build);
                }
                else
                {
                    Logger.Log(LogType.SystemActivity, "SuperNova plugin {0} was not loaded, you can load it with /snpload", pl.name);
                }

                if (!string.IsNullOrEmpty(pl.welcome)) Logger.Log(LogType.SystemActivity, pl.welcome);
            }
            catch
            {
                if (!string.IsNullOrEmpty(pl.creator)) Logger.Log(LogType.Warning, "You can go bug {0} about {1} failing to load.", pl.creator, pl.name);
                throw;
            }
        }
        public static bool Unload(SuperNovaPlugin pl)
        {
            bool success = UnloadSuperNovaPlugin(pl, false);

            // TODO only remove if successful?
            CustomSuperNovaPlugins.Remove(pl);
            CoreSuperNovaPlugins.Remove(pl);
            return success;
        }

        public static bool UnloadSuperNovaPlugin(SuperNovaPlugin pl, bool auto)
        {
            try
            {
                pl.Unload(auto);
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError("Error unloading SuperNova plugin " + pl.name, ex);
                return false;
            }
        }
        public static void UnloadAll()
        {
            for (int i = 0; i < CustomSuperNovaPlugins.Count; i++)
            {
                UnloadSuperNovaPlugin(CustomSuperNovaPlugins[i], true);
            }
            CustomSuperNovaPlugins.Clear();

            for (int i = 0; i < CoreSuperNovaPlugins.Count; i++)
            {
                UnloadSuperNovaPlugin(CoreSuperNovaPlugins[i], true);
            }
        }
        public static void LoadAll()
        {
            LoadCoreSuperNovaPlugin(new SuperNovaCompilerPlugin());
            IScripting.AutoloadSuperNovaPlugins();
        }
        public static void LoadCoreSuperNovaPlugin(SuperNovaPlugin SuperNovaplugin, bool startup)
        {
            LoadCoreSuperNovaPlugin(SuperNovaplugin);
        }
        public static void LoadCoreSuperNovaPlugin(SuperNovaPlugin SuperNovaplugin)
        {
            List<string> disabled = Server.Config.DisabledModules;
            if (disabled.CaselessContains(SuperNovaplugin.name)) return;
            SuperNovaplugin.Load(true);
            CoreSuperNovaPlugins.Add(SuperNovaplugin);
        }
    }
}
#endif