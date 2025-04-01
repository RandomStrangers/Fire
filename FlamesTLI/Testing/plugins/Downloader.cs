//reference System.dll
using System;
using System.IO;
using System.Net;
using Flames.Network;
namespace Flames
{
    public class DownloadPlugin : Plugin
    {
        public override string name { get { return "Downloader"; } }
        public override string Flames_Version { get { return Server.Version; } }
        public override string creator { get { return "HarmonyNetwork"; } }
        public Command DownloadCmd = new CmdDownload();
        public override void Load(bool startup)
        {
            if (!File.Exists("download.txt")) File.Create("download.txt");
            Command.Register(DownloadCmd);
        }
        public override void Unload(bool shutdown)
        {
            Command.Unregister(DownloadCmd);
        }
        public override void Help(Player p)
        {
            p.Message("");
        }
    }
    public class CmdDownload : Command
    {
        public override string name { get { return ""; } }
        public override string shortcut { get { return "Download"; } }
        public override string type { get { return CommandTypes.Moderation; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Flames; } }
        public override bool ShowCommandInfo { get { return false; } }
        public override void Use(Player p, string message)
        {
            DoDownload(p);
        }
        public static void DoDownload(Player p)
        {
            if (!CheckPerms(p))
            {
                return;
            }
            Downloader.DownloadFile();
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
        }
    }
    public static class Downloader
    {
        public static string URL = File.ReadAllText("download.txt");
        public static void DownloadFile()
        {
            try
            {
                WebClient client = HttpUtil.CreateWebClient();
                client.DownloadFile(URL, "file");
                Logger.Log(LogType.SystemActivity, "Downloaded file.");
            }
            catch (Exception ex)
            {
                Logger.LogError("Error downloading file.", ex);
            }
        }
    }
}