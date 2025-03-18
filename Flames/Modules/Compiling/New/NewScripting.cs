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
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Flames.NewScripting
{
    /// <summary> Exception raised when attempting to load a new plugin 
    /// that has the same name as an already loaded new plugin </summary>
    public class AlreadyLoadedException : Exception
    {
        public AlreadyLoadedException(string msg) : base(msg)
        {
        }
    }

    /// <summary> Utility methods for loading assemblies, and new plugins </summary>
    public static class IScripting
    {
#if F_DOTNET
        public const string COMMANDS_DLL_DIR = "extra/commands/dll/";
        public static string CommandPath(string name) { return COMMANDS_DLL_DIR + "Cmd" + name + ".dll"; }
#endif
        public static string GetExePath(string path)
        {
            return path;
        }

        public static Assembly ResolveNewPluginReference(string name)
        {
            return null;
        }

        public const string NEW_PLUGINS_DLL_DIR = "newplugins/";

        /// <summary> Returns the default .dll path for the new plugin with the given name </summary>
        public static string NewPluginPath(string name)
        {
            return NEW_PLUGINS_DLL_DIR + name + ".dll";
        }


        public static void Init()
        {
            #if F_DOTNET
            Directory.CreateDirectory(COMMANDS_DLL_DIR);
            #endif
            Directory.CreateDirectory(NEW_PLUGINS_DLL_DIR);
            AppDomain.CurrentDomain.AssemblyResolve += ResolveNewPluginAssembly;
        }

        // only used for resolving new plugin DLLs depending on other new plugin DLLs
        public static Assembly ResolveNewPluginAssembly(object sender, ResolveEventArgs args)
        {
            // This property only exists in .NET framework 4.0 and later
            Assembly requestingAssembly = args.RequestingAssembly;

            if (requestingAssembly == null) return null;
            if (!IsNewPluginDLL(requestingAssembly)) return null;

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assem in assemblies)
            {
                if (!IsNewPluginDLL(assem)) continue;

                if (args.Name == assem.FullName) return assem;
            }

            Assembly coreRef = ResolveNewPluginReference(args.Name);
            if (coreRef != null) return coreRef;

            Logger.Log(LogType.Warning, "Custom new plugin [{0}] tried to load [{1}], but it could not be found",
                       requestingAssembly.FullName, args.Name);
            return null;
        }

        public static bool IsNewPluginDLL(Assembly a)
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
            return Assembly.Load(data);
        }
        #if F_DOTNET
        public static void AutoloadCommands() {
            string[] files = FileIO.TryGetFiles(COMMANDS_DLL_DIR, "*.dll");
            if (files == null) return;
            
            foreach (string path in files) { AutoloadCommands(path); }
        }
        
        static void AutoloadCommands(string path) {
            List<Command> cmds;
            
            try {
                cmds = LoadCommands(path);
            } catch (Exception ex) {
                Logger.LogError("Error loading commands from " + path, ex);
                return;
            }
            
            Logger.Log(LogType.SystemActivity, "AUTOLOAD: Loaded {0} from {1}",
                       cmds.Join(c => "/" + c.name), Path.GetFileName(path));
        }
        
        /// <summary> Loads and registers all the commands from the given .dll path </summary>
        public static List<Command> LoadCommands(string path) {
            Assembly lib = LoadAssembly(path);
            List<Command> commands = LoadTypes<Command>(lib);
            
            if (commands.Count == 0)
                throw new InvalidOperationException("No commands in " + path);
            
            foreach (Command cmd in commands)
            {
                if (Command.Find(cmd.name) != null)
                    throw new AlreadyLoadedException("/" + cmd.name + " is already loaded");
                
                Command.Register(cmd);
            }
            return commands;
        }
#endif
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
            // p.Message("&WError loading new plugin. See error logs for more information.");
        }


        public static void AutoloadNewPlugins()
        {
            string[] files = AtomicIO.TryGetFiles(NEW_PLUGINS_DLL_DIR, "*.dll");
            if (files == null) return;

            // Ensure that new plugin files are loaded in a consistent order,
            //  in case new plugins have a dependency on other new plugins
            Array.Sort(files);

            foreach (string path in files)
            {
                try
                {
                    LoadNewPlugin(path, true);
                }
                catch (Exception ex)
                {
                    Logger.LogError("Error loading new plugins from " + path, ex);
                }
            }
        }

        /// <summary> Loads all new plugins from the given .dll path. </summary>
        public static List<NewPlugin> LoadNewPlugin(string path, bool auto)
        {
            Assembly lib = LoadAssembly(path);
            List<NewPlugin> newplugins = LoadTypes<NewPlugin>(lib);

            foreach (NewPlugin pl in newplugins)
            {
                if (NewPlugin.FindNewCustom(pl.name) != null)
                    throw new AlreadyLoadedException("New plugin " + pl.name + " is already loaded");

                NewPlugin.Load(pl, auto);
            }
            return newplugins;
        }
    }
}
