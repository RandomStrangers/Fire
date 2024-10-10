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
using System.IO;
using System.IO.Compression;
using System.Net;
using Flames.Network;
using Flames.Tasks;

namespace Flames
{
    /// <summary> Checks for and applies software updates. </summary>
    public static class Updater
    {

        public static string SourceURL = "https://github.com/RandomStrangers/Fire/";
        public const string BaseURL = "https://github.com/RandomStrangers/Fire/blob/Flame/";
        public const string UploadsURL = "https://github.com/RandomStrangers/Fire/tree/Flame/Uploads";
        public const string UpdatesURL = "https://github.com/RandomStrangers/Fire/raw/Flame/Uploads/";
        public static string WikiURL = "https://github.com/ClassiCube/MCGalaxy/wiki/";
#if CORE
        const string CurrentVersionURL = UpdatesURL + "dev.txt";
        const string dllURL = UpdatesURL + "Flames_dev.dll";
        public static string ZipURL = "https://nightly.link/RandomStrangers/Fire/workflows/build/Flame/Harmony.zip";
#else
        const string CurrentVersionURL = UpdatesURL + "current.txt";
        const string dllURL = UpdatesURL + "Flames_.dll";
        public static string ZipURL = "https://nightly.link/RandomStrangers/Fire/workflows/build/Flame/Flames.zip";
#endif
        const string guiURL = UpdatesURL + "Flames.exe";
        const string cliURL = UpdatesURL + "FlamesCLI.exe";

        public static event EventHandler NewerVersionDetected;

        public static void UpdaterTask(SchedulerTask task)
        {
            UpdateCheck();
            task.Delay = TimeSpan.FromHours(2);
        }

        static void UpdateCheck()
        {
            if (!Server.Config.CheckForUpdates) return;
            WebClient client = HttpUtil.CreateWebClient();

            try
            {
                string latest = client.DownloadString(CurrentVersionURL);

                if (new Version(Server.Version) >= new Version(latest))
                {
                    Logger.Log(LogType.SystemActivity, "No update found!");
                }
                else if (NewerVersionDetected != null)
                {
                    NewerVersionDetected(null, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Error checking for updates", ex);
            }

            client.Dispose();
        }

        public static void PerformUpdate()
        {
            try
            {
                try
                {
                    DeleteFiles("Flames_.update", "Flames.update", "FlamesCLI.update",
                    "prev_Flames_.dll", "prev_Flames.exe", "prev_FlamesCLI.exe", 
                    "NewFlames.zip", "MySql.Data.dll", "Newtonsoft.Json.dll", 
                    "sqlite3_x64.dll", "sqlite3_x32.dll", "Flames.exe.config",
                    "Flames.pdb", "Flames_.pdb", "FlamesCLI.pdb", "FlamesCLI.exe.config");
                }
                catch (Exception ex)
                {
                    Logger.LogError("Error deleting files", ex);
                }

                WebClient client = HttpUtil.CreateWebClient();
                //client.DownloadFile(dllURL, "Flames_.update");
                //client.DownloadFile(guiURL, "Flames.update");
                //client.DownloadFile(cliURL, "FlamesCLI.update");
                if (Directory.Exists("New")) 
                {
                   Directory.Delete("New", true);
                }
                client.DownloadFile(ZipURL, "NewFlames.zip");

                Level[] levels = LevelInfo.Loaded.Items;
                foreach (Level lvl in levels)
                {
                    if (!lvl.SaveChanges) continue;
                    lvl.Save();
                    lvl.SaveBlockDBChanges();
                }

                Player[] players = PlayerInfo.Online.Items;
                foreach (Player pl in players) pl.SaveStats();

                // Move current files to previous files (by moving instead of copying, 
                //  can overwrite original the files without breaking the server)

                AtomicIO.TryMove("Flames_.dll", "prev_Flames_.dll");
                AtomicIO.TryMove("Flames.exe", "prev_Flames.exe");
                AtomicIO.TryMove("FlamesCLI.exe", "prev_FlamesCLI.exe");
                ZipFile.ExtractToDirectory("NewFlames.zip", "New");
                string Dir = Directory.GetCurrentDirectory() + "/";
                string[] Files = Directory.GetFiles("New");
                foreach (string file in Files)
                {
                    File.Move(file, Dir + file);
                }
                
                //File.Move("Flames_.update", "Flames_.dll");
                //File.Move("Flames.update", "Flames.exe");
                //File.Move("FlamesCLI.update", "FlamesCLI.exe");
                Server.Stop(true, "Updating server.");
            }
            catch (Exception ex)
            {
                Logger.LogError("Error performing update", ex);
            }
        }

        static void DeleteFiles(params string[] paths)
        {
            foreach (string path in paths) { AtomicIO.TryDelete(path); }
        }
    }
}
