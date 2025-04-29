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

namespace Flames.Commands.World
{
    public class CmdSaveMap : Command
    {

        public override string name { get { return "SaveMap"; } }
        public override string type { get { return CommandTypes.World; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public override CommandAlias[] Aliases
        {
            get { return new[] { new CommandAlias("MapSaveMap"), new CommandAlias("WSaveMap"), new CommandAlias("WorldSaveMap") }; }
        }

        public override void Use(Player p, string message)
        {
            if (message.CaselessEq("all"))
            {
                Level lvl = Server.mainLevel;
                p.Message("Level {0} &Ssaved", lvl.ColoredName);
                SaveMap(p, lvl);
                return;
            }
            if (message.Length == 0)
            {
                if (p.IsSuper)
                {
                    SaveMap(p, Server.mainLevel);
                }
                else
                {
                    SaveMap(p, p.level);
                }
                return;
            }

            string[] args = message.SplitSpaces();
            if (args.Length <= 2)
            {
                Level lvl = Matcher.FindLevels(p, args[0]);
                if (lvl == null) return;
                SaveMap(p, lvl);
            }
            else
            {
                Help(p);
            }
        }

        public static bool TrySaveMap(Player p, Level lvl, bool force)
        {
            if (!force && !lvl.Changed) return false;

            if (!lvl.SaveChanges)
            {
                p.Message("Saving {0} &Sis currently disabled (most likely because a game is or was running on the level)", lvl.ColoredName);
                return false;
            }

            bool saved = SaveMap(lvl, force);
            if (!saved) p.Message("Saving of level {0} &Swas cancelled", lvl.ColoredName);
            return saved;
        }

        public static void SaveMap(Player p, Level lvl)
        {
            if (!TrySaveMap(p, lvl, true)) return;
            p.Message("Level {0} &Ssaved", lvl.ColoredName);
        }

        public override void Help(Player p)
        {
            p.Message("&T/SaveMap &H- Saves the level you are currently in as a .map file");
            p.Message("&T/SaveMap [level] &H- Saves the specified level as a .map file.");
        }
        
        /// <summary> Relative path of a level's map file </summary>
        public static string MapPath(string name)
        {
            return "maps/" + name.ToLower() + ".map";
        }
        /// <summary> Attempts to save this level (can be cancelled) </summary>
        /// <param name="force"> Whether to save even if nothing changed since last save </param>
        /// <returns> Whether this level was successfully saved to disc </returns>
        public static bool SaveMap(Level lvl, bool force = false)
        {
            if (lvl.blocks == null || lvl.IsMuseum) return false; // museums do not save changes

            string path = MapPath(lvl.MapName);
            bool cancel = false;
            OnLevelSaveEvent.Call(lvl, ref cancel);
            if (cancel) return false;

            try
            {
                if (!Directory.Exists("maps")) Directory.CreateDirectory("maps");
                if (!Directory.Exists("maps/map properties")) Directory.CreateDirectory("maps/map properties");
                if (!Directory.Exists("maps/prev")) Directory.CreateDirectory("levels/prev");

                if (lvl.Changed || force || !File.Exists(path))
                {
                    lock (lvl.saveLock) SaveCoreMap(path);
                }
                else
                {
                    Logger.Log(LogType.SystemActivity, "Skipping level save for " + lvl.name + ".");
                }
            }
            catch (Exception e)
            {
                Logger.Log(LogType.Warning, "FAILED TO SAVE :" + lvl.name);
                Chat.MessageGlobal("FAILED TO SAVE {0}", lvl.ColoredName);
                Logger.LogError(e);
                return false;
            }
            Server.DoGC();
            return true;
        }
        /// <summary> Relative path of a level's previous save map file. </summary>
        public static string PrevMapFile(string map) 
        { 
            return "maps/prev/" + map.ToLower() + ".map.prev"; 
        }
        public static void SaveCoreMap(Level lvl, string path)
        {
            if (lvl.blocks == null) return;
            if (File.Exists(path))
            {
                string prevPath = PrevMapFile(lvl.name);
                if (File.Exists(prevPath)) File.Delete(prevPath);
                File.Copy(path, prevPath, true);
                File.Delete(path);
            }

            IMapExporter.Formats[2].Write(path + ".backup", lvl);
            File.Copy(path + ".backup", path);
            lvl.SaveSettings();

            Logger.Log(LogType.SystemActivity, "SAVED: Level \"{0}\". ({1}/{2}/{3})",
                       lvl.name, lvl.players.Count, PlayerInfo.Online.Count, Server.Config.MaxPlayers);
            lvl.Changed = false;
        }
    }
}
