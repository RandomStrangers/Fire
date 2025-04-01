//reference System.dll
using System;
using System.IO;
using System.Net;
using Flames.Config;
using Flames.Network;
using Flames.Tasks;
namespace Flames.UpdatesPlugin
{
    public class UpdatePlugin : Plugin
    {
        public override string name { get { return "Updater"; } }
        public override string Flames_Version { get { return Server.Version; } }
        public override string creator { get { return "HarmonyNetwork"; } }
        public override void Load(bool startup)
        {
            UpdaterProperties.Load2();
            Command.Register(new CmdUpdate2());
        }

        public override void Unload(bool shutdown)
        {
            Command.Unregister(Command.Find("Update2"));
        }
        public override void Help(Player p)
        {
            p.Message("");
        }
    }
    public static class UpdaterProperties
    {
        public static string UpdateProperties = "properties/Update.properties";
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
                w.WriteLine("#Note: Do not add file extensions to any of these!");
                w.WriteLine("#Otherwise the updater will not work!");
                ConfigElement.Serialise(Updater.UpdateConfig, w, Updater.Config);
            }
        }
    }

    public class UpdateConfig
    {
        [ConfigString("base-url", "Updater", "https://github.com/RandomStrangers/Fire/blob/Flame/", true)]
        public static string BaseURL = "https://github.com/RandomStrangers/Fire/blob/Flame/";
        [ConfigString("downloads-url", "Updater", "https://github.com/RandomStrangers/Fire/raw/Flame/Uploads/", true)]
        public static string URL = "https://github.com/RandomStrangers/Fire/raw/Flame/Uploads/";
        [ConfigString("latest-url", "Updater", "https://github.com/RandomStrangers/Fire/raw/Flame/Uploads/dev.txt", true)]
        public static string Latest = "https://github.com/RandomStrangers/Fire/raw/Flame/Uploads/dev.txt";
        [ConfigString("cli-name", "Updater", "FlamesCLI", true)]
        public static string CLI = "FlamesCLI";
        [ConfigString("dll-name", "Updater", "Flames_dev", true)]
        public static string DLL = "Flames_dev";
        [ConfigBool("check-updates", "Updater", true)]
        public static bool CheckForUpdates = true;
        [ConfigString("version", "Updater", "1.0.3.3", false)]
        public static string Version = "1.0.3.3";
        [ConfigBool("update-gui", "Updater", false)]
        public static bool UpdateGUI = false;
        [ConfigString("gui-name", "Updater", "Flames", true)]
        public static string GUI = "Flames";
    }
    public class CmdUpdate2 : Command
    {
        public override string name { get { return "Update2"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return CommandTypes.Moderation; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Nobody; } }

        public override void Use(Player p, string message)
        {
            DoUpdate(p);
        }
        public static void DoUpdate(Player p)
        {
            if (!CheckPerms(p))
            {
                p.Message("Only Flames or the Server Owner can update the server."); return;
            }
            Updater.PerformUpdate();
        }

        public static bool CheckPerms(Player p)
        {
            if (p.IsFire) return true;
            if (p.IsSuper) return true;
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
        public static string UpdatesURL = UpdatesPlugin.UpdateConfig.URL;
        public static string CurrentVersionURL = UpdatesPlugin.UpdateConfig.Latest;
        public static string dllURL = UpdatesURL + UpdatesPlugin.UpdateConfig.DLL + ".dll";
        public static string cliURL = UpdatesURL + UpdatesPlugin.UpdateConfig.CLI + ".exe";
        public static string guiURL = UpdatesURL + UpdatesPlugin.UpdateConfig.GUI + ".exe";
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
                    DeleteFiles("Flames_.update", "FlamesCLI.update", "Flames.update", "prev_Flames_.dll", "prev_FlamesCLI.exe", "prev_Flames.exe");
                }
                catch
                {
                }

                WebClient client = HttpUtil.CreateWebClient();
                client.DownloadFile(dllURL, "Flames_.update");
                client.DownloadFile(cliURL, "FlamesCLI.update");
                if (UpdatesPlugin.UpdateConfig.UpdateGUI)
                {
                    client.DownloadFile(guiURL, "Flames.update");
                }
                Level[] levels = LevelInfo.Loaded.Items;
                foreach (Level lvl in levels)
                {
                    if (!lvl.SaveChanges) continue;
                    lvl.Save();
                    lvl.SaveBlockDBChanges();
                }
                Player[] players = PlayerInfo.Online.Items;
                foreach (Player pl in players) pl.SaveStats();
                AtomicIO.TryMove("Flames_.dll", "prev_Flames_.dll");
                AtomicIO.TryMove("FlamesCLI.exe", "prev_FlamesCLI.exe");
                if (UpdatesPlugin.UpdateConfig.UpdateGUI)
                {
                    AtomicIO.TryMove("Flames.exe", "prev_Flames.exe");
                }
                File.Move("Flames_.update", "Flames_.dll");
                File.Move("FlamesCLI.update", "FlamesCLI.exe");
                if (UpdatesPlugin.UpdateConfig.UpdateGUI)
                {
                    File.Move("Flames.update", "Flames.exe");
                }
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