/*
    Copyright 2010 MCSharp team (Modified for use with MCZall/MCLawl/MCForge)
    
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
using Flames.NewScripting;
using Flames.Modules.NewCompiling;
using Flames.Tasks;
using Flames.Events.ServerEvents;
namespace Flames.Core
{
    public class NewPluginLoader : Plugin
    {
        public override string name { get { return "NewPluginLoader"; } }
        public override string creator { get { return Colors.Strip(Server.SoftwareName) + " team"; } }
        public static void LoadAllNewPlugins(SchedulerTask task)
        {
            NewPlugin.LoadAll();
        }
        public override void Load(bool startup)
        {
            OnShuttingDownEvent.Register(OnShutdown, Priority.Critical);
            Server.Critical.QueueOnce(LoadAllNewPlugins);
        }
        public void OnShutdown(bool restarting, string message)
        {
            NewPlugin.UnloadAll();
        }
        public override void Unload(bool shutdown)
        {
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
    public abstract class NewPlugin
    {
        /// <summary> Hooks into events and initalises states/resources etc </summary>
        /// <param name="auto"> True if new plugin is being automatically loaded (e.g. on server startup), false if manually. </param>
        public abstract void Load(bool auto);

        /// <summary> Unhooks from events and disposes of state/resources etc </summary>
        /// <param name="auto"> True if new plugin is being auto unloaded (e.g. on server shutdown), false if manually. </param>
        public abstract void Unload(bool auto);

        /// <summary> Called when a player does /Help on the new plugin. Typically tells the player what this new plugin is about. </summary>
        /// <param name="p"> Player who is doing /Help. </param>
        public virtual void Help(Player p)
        {
            p.Message("No help is available for this new plugin.");
        }

        /// <summary> Name of the new plugin. </summary>
        public abstract string name { get; }
        /// <summary> The oldest version of Flames this new plugin is compatible with. </summary>
        public virtual string Flames_Version { get { return "9.0.4.8"; } }
        /// <summary> Work on backwards compatibility with MCGalaxy </summary>
        public virtual string MCGalaxy_Version { get { return null; } }
        /// <summary> Version of this new plugin. </summary>
        public virtual int build { get { return 0; } }
        /// <summary> Message to display once this new plugin is loaded. </summary>
        public virtual string welcome { get { return ""; } }
        /// <summary> The creator/author of this new plugin. (Your name) </summary>
        public virtual string creator { get { return ""; } }
        /// <summary> Whether or not to auto load this new plugin on server startup. </summary>
        public virtual bool LoadAtStartup { get { return true; } }


        /// <summary> List of new plugins/modules included in the server software </summary>
        public static List<NewPlugin> CoreNewPlugins = new List<NewPlugin>();
        public static List<NewPlugin> CustomNewPlugins = new List<NewPlugin>();


        public static NewPlugin FindNewCustom(string name)
        {
            foreach (NewPlugin pl in CustomNewPlugins)
            {
                if (pl.name.CaselessEq(name)) return pl;
            }
            return null;
        }

        public static void Load(NewPlugin pl, bool auto)
        {
            string ver = pl.Flames_Version;
            string MCGalaxy_Ver = "1.9.4.9";
            // Version different in Dev build, use normal for new plugins
            string CurrentVersion = Server.FlamesVersion;

            if (!string.IsNullOrEmpty(pl.MCGalaxy_Version) && new Version(pl.MCGalaxy_Version) > new Version(MCGalaxy_Ver))
            {
                string msg = string.Format("New plugin '{0}' cannot be loaded on this version of {1}!", pl.name, Server.SoftwareName);
                throw new InvalidOperationException(msg);
            }
            if (!string.IsNullOrEmpty(ver) && new Version(ver) > new Version(CurrentVersion) && new Version(ver) > new Version(Server.Version))
            {
                string msg = string.Format("New plugin '{0}' requires a more recent version of {1}!", pl.name, Server.SoftwareName);
                throw new InvalidOperationException(msg);
            }

            try
            {
                CustomNewPlugins.Add(pl);

                if (pl.LoadAtStartup || !auto)
                {
                    pl.Load(auto);
                    Logger.Log(LogType.SystemActivity, "New plugin {0} loaded...build: {1}", pl.name, pl.build);
                }
                else
                {
                    Logger.Log(LogType.SystemActivity, "New plugin {0} was not loaded, you can load it with /npload", pl.name);
                }

                if (!string.IsNullOrEmpty(pl.welcome)) Logger.Log(LogType.SystemActivity, pl.welcome);
            }
            catch
            {
                if (!string.IsNullOrEmpty(pl.creator)) Logger.Log(LogType.Warning, "You can go bug {0} about {1} failing to load.", pl.creator, pl.name);
                throw;
            }
        }
        public static bool Unload(NewPlugin pl)
        {
            bool success = UnloadNewPlugin(pl, false);

            // TODO only remove if successful?
            CustomNewPlugins.Remove(pl);
            CoreNewPlugins.Remove(pl);
            return success;
        }

        public static bool UnloadNewPlugin(NewPlugin pl, bool auto)
        {
            try
            {
                pl.Unload(auto);
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError("Error unloading new plugin " + pl.name, ex);
                return false;
            }
        }
        public static void UnloadAll()
        {
            for (int i = 0; i < CustomNewPlugins.Count; i++)
            {
                UnloadNewPlugin(CustomNewPlugins[i], true);
            }
            CustomNewPlugins.Clear();

            for (int i = 0; i < CoreNewPlugins.Count; i++)
            {
                UnloadNewPlugin(CoreNewPlugins[i], true);
            }
        }
        public static void LoadAll()
        {
            LoadCoreNewPlugin(new NewCompilerPlugin());
            IScripting.AutoloadNewPlugins();
        }
        public static void LoadCoreNewPlugin(NewPlugin newplugin)
        {
            List<string> disabled = Server.Config.DisabledModules;
            if (disabled.CaselessContains(newplugin.name)) return;
            newplugin.Load(true);
            CoreNewPlugins.Add(newplugin);
        }
    }
}
