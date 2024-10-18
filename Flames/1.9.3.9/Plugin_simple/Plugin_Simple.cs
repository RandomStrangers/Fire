/*
    Copyright 2010 MCSharp team (Modified for use with MCZall/MCLawl/MCGalaxy)
    
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
using System.Collections.Generic;
using Flames.Scripting;

namespace Flames
{
    /// <summary> This class provides for simple modification to Flames </summary>
    public abstract class Plugin_Simple
    {
        /// <summary> Hooks into events and initalises states/resources etc </summary>
        /// <param name="auto"> True if simple plugin is being automatically loaded (e.g. on server startup), false if manually. </param>
        public abstract void Load(bool auto);

        /// <summary> Unhooks from events and disposes of state/resources etc </summary>
        /// <param name="auto"> True if simple plugin is being auto unloaded (e.g. on server shutdown), false if manually. </param>
        public abstract void Unload(bool auto);

        /// <summary> Called when a player does /Help on the simple plugin. Typically tells the player what this simple plugin is about. </summary>
        /// <param name="p"> Player who is doing /Help. </param>
        public virtual void Help(Player p)
        {
            p.Message("No help is available for this simple plugin.");
        }

        /// <summary> Name of the simple plugin. </summary>
        public abstract string name { get; }
        public virtual string Name { get { return name; } }

        /// <summary> Oldest version of Flames this simple plugin is compatible with. </summary>
        public virtual string Flames_Version { get { return null; } }
        /// <summary> Work on backwards compatibility with MCGalaxy </summary>
        public virtual string MCGalaxy_Version { get { return null; } }

#if CORE
        /// <summary> Work on backwards compatibility with other cores </summary>
        public virtual string SuperNova_Version { get { return null; } }
        /// <summary> Work on backwards compatibility with other cores </summary>
        public virtual string DeadNova_Version { get { return null; } }
        /// <summary> Work on backwards compatibility with other cores </summary>
        public virtual string GoldenSparks_Version { get { return null; } }
        /// <summary> Work on backwards compatibility with other cores </summary>
        public virtual string RandomStrangers_Version { get { return null; } }
#endif

        /// <summary> The creator/author of this simple plugin. (Your name) </summary>
        public virtual string Creator { get { return creator; } }
        public virtual string creator { get { return ""; } }

        /// <summary> Whether or not to auto load this simple plugin on server startup. </summary>
        public virtual bool LoadAtStartup { get { return true; } }


        public static List<Plugin_Simple> core = new List<Plugin_Simple>();
        public static List<Plugin_Simple> all = new List<Plugin_Simple>();

        public static bool Load(Plugin_Simple p, bool auto)
        {
            try
            {
                all.Add(p);

                if (p.LoadAtStartup || !auto)
                {
                    p.Load(auto);
                }
                else
                {
                    Logger.Log(LogType.SystemActivity, "Simple plugin {0} was not loaded, you can load it with /psload", p.Name);
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError("Error loading simple plugin " + p.Name, ex);
                if (!string.IsNullOrEmpty(p.Creator)) Logger.Log(LogType.Warning, "You can go bug {0} about it.", p.Creator);
                return false;
            }
        }

        public static bool Unload(Plugin_Simple p, bool auto)
        {
            bool success = true;
            try
            {
                p.Unload(auto);
                Logger.Log(LogType.SystemActivity, "Simple plugin {0} was unloaded.", p.Name);
            }
            catch (Exception ex)
            {
                Logger.LogError("Error unloading simple plugin " + p.Name, ex);
                success = false;
            }

            all.Remove(p);
            return success;
        }

        public static void UnloadAll()
        {
            for (int i = 0; i < all.Count; i++)
            {
                Unload(all[i], true); 
                i--;
            }
        }
        public static void LoadAll()
        {
            IScripting_Simple.AutoloadSimplePlugins();
        }
    }
}