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
using System.IO;
using Flames.DB;
using Flames.Events.LevelEvents;
using Flames.SQL;
using System.Linq;
namespace Flames
{

    public static class LevelInfo
    {

        /// <summary> Array of all current loaded levels. </summary>
        /// <remarks> Note this field is highly volatile, you should cache references to the items array. </remarks>
        public static VolatileArray<Level> Loaded = new VolatileArray<Level>();

        public static Level FindExact(string name)
        {
            Level[] loaded = Loaded.Items;
            foreach (Level lvl in loaded)
            {
                if (lvl.name.CaselessEq(name)) return lvl;
            }
            return null;
        }

        public static void Add(Level lvl)
        {
            Loaded.Add(lvl);
            OnLevelAddedEvent.Call(lvl);
        }

        public static void Remove(Level lvl)
        {
            Loaded.Remove(lvl);
            OnLevelRemovedEvent.Call(lvl);
        }


        // TODO: support loading other map files eventually
        public static string[] AllMapFiles()
        {
            List<string> files = new List<string>()
            {
            };
            string[] allMapFiles = Directory.GetFiles("levels", "");
            foreach (string file in allMapFiles)
            {
                if (!file.CaselessEnds(".backup"))
                {
                    files.Add(file);
                }
            }
            string[] allFiles = files.ToArray();
            return allFiles;
        }

        public static string[] AllMapNames()
        {
            string[] files = AllMapFiles();
            for (int i = 0; i < files.Length; i++)
            {
                files[i] = MapNameExt(files[i]);
            }
            return files;
        }

        public static bool MapExists(string name)
        {
            return LevelExists(name);
        }
        public static string MapNameNoExt(string name)
        {
            if (name.CaselessContains("("))
            {
                string[] array = name.Split('(');
                string a = array[0].Replace("(", "").Replace(")", "");
                string lvlPath = a.ToLower();
                return lvlPath;
            }
            else                 
            {
                return name.ToLower();
            }
        }
        public static bool LevelExists(string name)
        {
            return File.Exists("levels/" + MapNameNoExt(name) + ".cw") 
                || File.Exists("levels/" + MapNameNoExt(name) + ".dat")
                || File.Exists("levels/" + MapNameNoExt(name) + ".mcf") 
                || File.Exists("levels/" + MapNameNoExt(name) + ".fcm")
                || File.Exists("levels/" + MapNameNoExt(name) + ".lvl") 
                || File.Exists("levels/" + MapNameNoExt(name) + ".flvl")
                || File.Exists("levels/" + MapNameNoExt(name) + ".mclevel") 
                || File.Exists("levels/" + MapNameNoExt(name) + ".map");
        }
        public static string MapNameExt(string name)
        {
            foreach (string file in AllMapFiles())
            {
                if (name.CaselessContains("("))
                {
                    return name.ToLower();
                }
                else                 
                {
                    string ext = Path.GetExtension(file);
                    string extNoPeriod = ext.Replace(".", "");
                    string lvlPath = name.ToLower() + "(" + extNoPeriod + ")";
                    return lvlPath;
                }
            }
            return null;
        }
        public static string MapName_Ext(string name)
        {
            foreach (string file in AllMapNames())
            {
                if (name.CaselessContains("("))
                {
                    string[] array = name.Split('(');
                    string a = array[0].Replace("(", "").Replace(")", "");
                    string ext = array[1].Replace("(","").Replace(")","");
                    ext = "." + ext;
                    string lvlPath = a.ToLower() + ext;
                    return lvlPath;
                }
                else                 
                {
                    string ext = Path.GetExtension(file);
                    string lvlPath = name.ToLower() + ext;
                    return lvlPath;
                }
            }
            return null;
        }
        /// <summary> Relative path of a level's map file </summary>
        public static string MapPath(string name)
        {
            string lvlPath = "levels/" + MapName_Ext(name);
            return lvlPath;
        }


        /// <summary> Relative path of a level's backup folder </summary>
        public static string BackupBasePath(string name)
        {
            return Server.Config.BackupDirectory + "/" + name;
        }

        /// <summary> Relative path of a level's backup map directory </summary>
        public static string BackupDirPath(string name, string backup)
        {
            return BackupBasePath(name) + "/" + backup;
        }

        /// <summary> Relative path of a level's backup map file </summary>
        public static string BackupFilePath(string name, string backup)
        {
            string[] files = AllMapFiles();
            foreach (string file in files)
            {
                return BackupDirPath(name, backup) + "/" + MapName_Ext(name);
            }
            return null;
        }

        public static string BackupNameFrom(string path)
        {
            return path.Substring(path.LastIndexOf(Path.DirectorySeparatorChar) + 1);
        }

        public static int LatestBackup(string map)
        {
            string root = BackupBasePath(map);
            string[] backups = Directory.GetDirectories(root);
            int latest = 0;

            foreach (string path in backups)
            {
                string backupName = BackupNameFrom(path);
                int num;

                if (!int.TryParse(backupName, out num)) continue;
                latest = Math.Max(num, latest);
            }
            return latest;
        }

        public static string NextBackup(string map)
        {
            string root = BackupBasePath(map);
            Directory.CreateDirectory(root);

            return (LatestBackup(map) + 1).ToString();
        }

        /// <summary> Relative path of a level's property file </summary>
        public static string PropsPath(string name)
        {
            return "levels/level properties/" + MapNameExt(name) + ".properties";
        }

        public static LevelConfig GetConfig(string map)
        {
            Level lvl; 
            return GetConfig(map, out lvl);
        }

        public static LevelConfig GetConfig(string map, out Level lvl)
        {
            lvl = FindExact(map);
            if (lvl != null) return lvl.Config;

            string propsPath = PropsPath(map);
            LevelConfig cfg = new LevelConfig();
            cfg.Load(propsPath);
            return cfg;
        }

        public static bool Check(Player p, LevelPermission plRank, string map, string action, out LevelConfig cfg)
        {
            Level lvl; cfg = GetConfig(map, out lvl);
#if CORE 
            if (p.IsNull) return true;
#else
            if (p.IsFire) return true;
            else if (p.IsConsole) return true;
#endif
            if (lvl != null) return Check(p, plRank, lvl, action);

            AccessController visit = new LevelAccessController(cfg, map, true);
            AccessController build = new LevelAccessController(cfg, map, false);
            if (!visit.CheckDetailed(p, plRank) || !build.CheckDetailed(p, plRank))
            {
                p.Message("Hence, you cannot {0}.", action); 
                return false;
            }
            return true;
        }

        public static bool Check(Player p, LevelPermission plRank, string map, string action)
        {
            LevelConfig ignored;
            return Check(p, plRank, map, action, out ignored);
        }

        public static bool Check(Player p, LevelPermission plRank, Level lvl, string action)
        {
#if CORE 
            if (p.IsNull) return true;
#else
            if (p.IsFire) return true;
            else if (p.IsConsole) return true;
#endif
            if (!lvl.VisitAccess.CheckDetailed(p, plRank) || !lvl.BuildAccess.CheckDetailed(p, plRank))
            {
                p.Message("Hence, you cannot {0}.", action); 
                return false;
            }
            return true;
        }

        public static bool ValidName(string map)
        {
            foreach (char c in map)
            {
                if (!Database.ValidNameChar(c)) return false;
            }
            return true;
        }


        public static bool IsRealmOwner(string name, string map)
        {
            LevelConfig cfg = GetConfig(map);
            return IsRealmOwner(map, cfg, name);
        }

        public static bool IsRealmOwner(Level lvl, string name)
        {
            return IsRealmOwner(lvl.name, lvl.Config, name);
        }

        public static bool IsRealmOwner(string map, LevelConfig cfg, string name)
        {
            string[] owners = cfg.RealmOwner.SplitComma();
            if (owners.Length > 0)
            {
                foreach (string owner in owners)
                {
                    if (owner.CaselessEq(name)) return true;
                }
                return false;
            }

            // For backwards compatibility, treat name+XYZ map names as belonging to name+
            // If no + though, don't use because otherwise people can register accounts and claim maps
            return Server.Config.ClassicubeAccountPlus && map.CaselessStarts(name);
        }

        public static string DefaultRealmOwner(string map)
        {
            bool plus = Server.Config.ClassicubeAccountPlus;
            // Early out when either
            //  1) accounts aren't using +
            //  2) map name doesn't include +
            if (!plus || map.IndexOf('+') == -1) return null;

            // Convert username+23 to username+
            while (map.Length > 0 && char.IsNumber(map[map.Length - 1]))
            {
                map = map.Substring(0, map.Length - 1);
            }

            // Match the backwards compatibilty case of IsRealmOwner
            return PlayerDB.FindName(map);
        }
    }
}
