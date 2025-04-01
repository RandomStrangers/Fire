//reference System.dll
using MCGalaxy.Config;
using System;
using System.IO;
using MCGalaxy.Network;
using MCGalaxy.Tasks;
using System.Net;
namespace MCGalaxy.UpdatesPlugin
{
    public class UpdatePlugin : Plugin
    {
        public override string name { get { return "Updater"; } }
        public override string MCGalaxy_Version { get { return Server.Version; } }
        public override string creator { get { return "Random Strangers"; } }
        public override void Load(bool startup)
        {
            UpdaterProperties.Load2();
            Command.Register(new CmdUpdate2());
            Server.Background.QueueRepeat(Updater.UpdaterTask, null, TimeSpan.FromSeconds(10));

        }

        public override void Unload(bool shutdown)
        {
            Command.Unregister(Command.Find("Update2"));
            Server.Background.Cancel(Updater.UpdateTask);
        }
        public override void Help(Player p)
        {
            p.Message("");
        }
    }
    public static class UpdaterProperties
    {
        public static string UpdateProperties = "properties/update.properties";
        public static void Load2()
        {
            if (Updater.UpdateConfig == null) Updater.UpdateConfig = ConfigElement.GetAll(typeof(UpdateConfig));
            ConfigElement.ParseFile(Updater.UpdateConfig, UpdateProperties, Updater.Config);
            Save2();
        }

        public static void Save2()
        {
            using (StreamWriter w = new StreamWriter(UpdateProperties))
            {
                w.WriteLine("#Note: Do not add file extensions to any of these other then current-version!");
                w.WriteLine("#Otherwise the updater will not work!");
                ConfigElement.Serialise(Updater.UpdateConfig, w, Updater.Config);
            }
        }
    }

    public class UpdateConfig
    {
        [ConfigString("base-url", "Updater", "https://github.com/UnknownShadow200/MCGalaxy/blob/master/", true)]
        public static string BaseURL = "https://github.com/UnknownShadow200/MCGalaxy/blob/master/";
        [ConfigString("downloads-url", "Updater", "https://github.com/UnknownShadow200/MCGalaxy/raw/master/Uploads/", true)]
        public static string URL = "https://github.com/UnknownShadow200/MCGalaxy/raw/master/Uploads/";
        [ConfigString("cli-name", "Updater", "MCGalaxyCLI", true)]
        public static string CLI = "MCGalaxyCLI";
        [ConfigString("dll-name", "Updater", "MCGalaxy_", true)]
        public static string DLL = "MCGalaxy_";
        [ConfigString("current-version", "Updater", "Uploads/current_version.txt", true)]
        public static string VersionTXT = "Uploads/current_version.txt";
        [ConfigBool("check-updates", "Updater", true)]
        public static bool CheckForUpdates = true;
        [ConfigString("version", "Updater", "1.9.4.9", false)]
        public static string Version = "1.9.4.9";
    }
    public sealed class CmdUpdate2 : Command2
    {
        public override string name { get { return "Update2"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return CommandTypes.Moderation; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Nobody; } }

        public override void Use(Player p, string message, CommandData data)
        {
            DoUpdate(p);
        }
        public static void DoUpdate(Player p)
        {
            if (!CheckPerms(p))
            {
                p.Message("Only Console or the Server Owner can update the server."); return;
            }
            Updater.PerformUpdate();
        }

        public static bool CheckPerms(Player p)
        {
            if (p.IsConsole) return true;

            if (Server.Config.OwnerName.CaselessEq("Notch")) return false;
            return p.name.CaselessEq(Server.Config.OwnerName);
        }
        public override void Help(Player p)
        {
            p.Message("&T/Update2 &H- Force updates the server");
        }
    }
    public static class Updater
    {
        public static UpdateConfig Config = new UpdateConfig();
        public static string BaseURL = UpdatesPlugin.UpdateConfig.BaseURL;
        public static string UpdatesURL = UpdatesPlugin.UpdateConfig.URL;
        public static string CurrentVersionURL = BaseURL + UpdatesPlugin.UpdateConfig.VersionTXT;
        public static string dllURL = UpdatesURL + UpdatesPlugin.UpdateConfig.DLL + ".dll";
        public static string cliURL = UpdatesURL + UpdatesPlugin.UpdateConfig.CLI + ".exe";

        public static event EventHandler NewerVersionDetected;
        public static ConfigElement[] UpdateConfig;
        public static void UpdaterTask(SchedulerTask task)
        {
            UpdateTask = task;
            UpdateConfig = ConfigElement.GetAll(typeof(UpdateConfig));
            UpdateCheck();
            task.Delay = TimeSpan.FromHours(2);
        }
        public static void UpdateCheck()
        {
            if (!UpdatesPlugin.UpdateConfig.CheckForUpdates) return;
            WebClient client = HttpUtil.CreateWebClient();

            try
            {
                string latest = client.DownloadString(CurrentVersionURL);

                if (new Version(UpdatesPlugin.UpdateConfig.Version) >= new Version(latest))
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
        public static SchedulerTask UpdateTask;

        public static void PerformUpdate()
        {
            try
            {
                try
                {
                    DeleteFiles("Changelog.txt", "MCGalaxy_.update", "MCGalaxyCLI.update",
                        "prev_MCGalaxy_.dll", "prev_MCGalaxyCLI.exe");
                }
                catch
                {
                }

                WebClient client = HttpUtil.CreateWebClient();
                client.DownloadFile(dllURL, "MCGalaxy_.update");
                client.DownloadFile(cliURL, "MCGalaxyCLI.update");
                Level[] levels = LevelInfo.Loaded.Items;
                foreach (Level lvl in levels)
                {
                    if (!lvl.SaveChanges) continue;
                    lvl.Save();
                    lvl.SaveBlockDBChanges();
                }
                Player[] players = PlayerInfo.Online.Items;
                foreach (Player pl in players) pl.SaveStats();
                AtomicIO.TryMove("MCGalaxy_.dll", "prev_" + Colors.Strip(UpdatesPlugin.UpdateConfig.DLL) + ".dll");
                AtomicIO.TryMove("MCGalaxyCLI.exe", "prev_" + Colors.Strip(UpdatesPlugin.UpdateConfig.CLI) + ".exe");
                File.Move("MCGalaxy_.update", Colors.Strip(UpdatesPlugin.UpdateConfig.DLL) + ".dll");
                File.Move("MCGalaxyCLI.update", Colors.Strip(UpdatesPlugin.UpdateConfig.CLI) + ".exe");
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