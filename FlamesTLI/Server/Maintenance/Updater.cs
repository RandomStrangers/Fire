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
using System.Net;
using Flames.Network;
using Flames.Tasks;

namespace Flames
{
    /// <summary> Checks for and applies software updates. </summary>
    public static class Updater
    {

        public static string SourceURL = "https://github.com/SuperNova-DeadNova/Fire-Debug/";
        public const string BaseURL = "https://github.com/SuperNova-DeadNova/Fire-Debug/blob/debug/";
        public const string UploadsURL = "https://github.com/SuperNova-DeadNova/Fire-Debug/tree/debug/Uploads";
        public const string UpdatesURL = "https://github.com/SuperNova-DeadNova/Fire-Debug/raw/debug/Uploads/";
        public static string WikiURL = "https://github.com/ClassiCube/MCGalaxy/wiki/";
#if CORE
        public const string CurrentVersionURL = UpdatesURL + "dev_TLI.txt";
        public const string dllURL = UpdatesURL + "FlamesTLI_dev.dll";
#elif CORE && F_DOTNET_DEV
        public const string CurrentVersionURL = UpdatesURL + "dev_TLI.txt";
        public const string dllURL = UpdatesURL + "FlamesTLI_dotnet_core_dev.dll";
#elif F_DOTNET_DEV
        public const string CurrentVersionURL = UpdatesURL + "current_TLI.txt";
        public const string dllURL = UpdatesURL + "FlamesTLI_dotnet_dev.dll";
#else
        public const string CurrentVersionURL = UpdatesURL + "current_TLI.txt";
        public const string dllURL = UpdatesURL + "FlamesTLI_.dll";
#endif
        public const string TLIURL = UpdatesURL + "FlamesTLI.exe";

        public static event EventHandler NewerVersionDetected;

        public static void UpdaterTask(SchedulerTask task)
        {
            UpdateCheck();
            task.Delay = TimeSpan.FromHours(2);
        }
        public static void UpdateCheck()
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
                else
                {
                    NewerVersionDetected?.Invoke(null, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Error checking for updates", ex);
            }

            client.Dispose();
        }
        public static bool NeedsUpdating()
        {
            using (WebClient client = HttpUtil.CreateWebClient())
            {
                string latest = client.DownloadString(CurrentVersionURL);
                return new Version(latest) > new Version(Server.Version);
            }
        }
        public static void PerformUpdate()
        {
            try
            {
                try
                {
                    DeleteFiles("FlamesTLI_.update", "FlamesTLI.update",
                    "prev_FlamesTLI_.dll", "prev_FlamesTLI.exe");
                }
                catch (Exception ex)
                {
                    Logger.LogError("Error deleting files", ex);
                }

                WebClient client = HttpUtil.CreateWebClient();
                client.DownloadFile(dllURL, "Flames_.update");
                client.DownloadFile(TLIURL, "FlamesTLI.update");

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
                AtomicIO.TryMove("FlamesTLI_.dll", "prev_FlamesTLI_.dll");
                AtomicIO.TryMove("FlamesTLI.exe", "prev_FlamesTLI.exe");
                File.Move("FlamesTLI.update", "FlamesTLI.exe");
                File.Move("FlamesTLI_.update", "FlamesTLI_.dll");
                Server.Stop(true, "Updating server.");
            }
            catch (Exception ex)
            {
                Logger.LogError("Error performing update", ex);
            }
        }
        public static void DeleteFiles(params string[] paths)
        {
            foreach (string path in paths) 
            {
                AtomicIO.TryDelete(path);
            }
        }
    }
}