//reference System.dll
//reference System.Core.dll
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using Flames.Authentication;
using Flames.Config;
using Flames.Events.ServerEvents;
using Flames.Network;
using Flames.Events.PlayerEvents;
namespace Flames
{
    public class ClassiCubePlugin : Plugin
    {
        public static Mppass2Authenticator mppass2auth = new Mppass2Authenticator();
        public const string CONFIG_FOLDER = "plugins/cc";
        public static AuthService CCService = AuthService.GetOrCreate("ClassiCube", true);
        public override string Flames_Version { get { return Server.Version; } }
        public override string name { get { return "ClassiCube"; } }
        public override string creator { get { return "icanttellyou"; } }
        public static INetListen Listener = new TcpListen();
        public override void Load(bool auto)
        {
            //OnPlayerStartConnectingEvent.Unregister(ConnectingHandler.HandleConnecting);
            OnPlayerStartConnectingEvent.Register(LoginPlayer, Priority.Critical);
            LoginAuthenticator.Authenticators.Add(mppass2auth);
            if (!AuthService.Services.Contains(CCService))
            {
                AuthService.Services.Add(CCService);
            }
            OnConfigUpdatedEvent.Register(OnConfigUpdated, Priority.Low);
            OnConfigUpdated();
        }
        public static void LoginPlayer(Player p, string mppass)
        {
            if (p.cancelconnecting) return;
            bool success = LoginPlayerCore(p, mppass);
            if (!success) p.cancelconnecting = true;
        }

        public static bool LoginPlayerCore(Player p, string mppass)
        {
            if (!LoginAuthenticator.VerifyLogin(p, mppass))
            {
                if (!mppass2auth.Verify(p, mppass))
                {
                    Logger.Log(LogType.UserActivity, "&cLogin of {0} failed!", p.truename);
                    return true;
                };
                return false;
            }
            if (!CheckTempban(p)) return false;

            if (Server.Config.WhitelistedOnly && !Server.whiteList.Contains(p.name))
            {
                p.Leave(null, Server.Config.DefaultWhitelistMessage, true);
                return false;
            }

            p.group = Group.GroupIn(p.name);
            if (!CheckBanned(p)) return false;
            if (!CheckPlayersCount(p)) return false;
            return true;
        }

        public static bool CheckTempban(Player p)
        {
            try
            {
                string data = Server.tempBans.Get(p.name);
                if (data == null) return true;

                string banner, reason;
                DateTime expiry;
                Ban.UnpackTempBanData(data, out reason, out banner, out expiry);

                if (expiry < DateTime.UtcNow)
                {
                    Server.tempBans.Remove(p.name);
                    Server.tempBans.Save();
                }
                else
                {
                    reason = reason.Length == 0 ? "" : " (" + reason + ")";
                    string delta = (expiry - DateTime.UtcNow).Shorten(true);

                    p.Kick(null, "Banned by " + banner + " for another " + delta + reason, true);
                    return false;
                }
            }
            catch { } // TODO log error
            return true;
        }

        public static bool CheckPlayersCount(Player p)
        {
            if (Server.vip.Contains(p.name)) return true;

            Player[] online = PlayerInfo.Online.Items;
            if (online.Length >= Server.Config.MaxPlayers && !IPUtil.IsPrivate(p.IP))
            {
                p.Leave(null, "Server full!", true);
                return false;
            }
            if (p.Rank > LevelPermission.Guest) return true;

            online = PlayerInfo.Online.Items;
            int guests = 0;
            foreach (Player pl in online)
            {
                if (pl.Rank <= LevelPermission.Guest) guests++;
            }
            if (guests < Server.Config.MaxGuests) return true;

            if (Server.Config.GuestLimitNotify) Chat.MessageOps("Guest " + p.truename + " couldn't log in - too many guests.");
            Logger.Log(LogType.Warning, "Guest {0} couldn't log in - too many guests.", p.truename);
            p.Leave(null, "Server has reached max number of guests", true);
            return false;
        }

        public static bool CheckBanned(Player p)
        {
            string ipban = Server.bannedIP.Get(p.ip);
            if (ipban != null)
            {
                ipban = ipban.Length > 0 ? ipban : Server.Config.DefaultBanMessage;
                p.Kick(null, ipban, true);
                return false;
            }
            if (p.Rank != LevelPermission.Banned) return true;

            string banner, reason, prevRank;
            DateTime time;
            Ban.GetBanData(p.name, out banner, out reason, out time, out prevRank);

            if (banner != null)
            {
                p.Kick(null, "Banned by " + banner + ": " + reason, true);
            }
            else
            {
                p.Kick(null, Server.Config.DefaultBanMessage, true);
            }
            return false;
        }
        public override void Unload(bool auto)
        {
            OnPlayerStartConnectingEvent.Unregister(LoginPlayer);
            //OnPlayerStartConnectingEvent.Register(ConnectingHandler.HandleConnecting, Priority.Critical);
            LoginAuthenticator.Authenticators.Remove(new Mppass2Authenticator());
            OnConfigUpdatedEvent.Unregister(OnConfigUpdated);
            AuthService.Services.Remove(CCService);
            Heartbeat CCBeat = new ClassiCubeHeartbeat(ClassiCubeConfig);
            Heartbeat.Heartbeats.Remove(CCBeat);
        }
        public HeartbeatConfig ClassiCubeConfig = new HeartbeatConfig();
        public static HeartbeatConfig CCConfig = new HeartbeatConfig();

        public void OnConfigUpdated()
        {
            if (!Directory.Exists(CONFIG_FOLDER)) Directory.CreateDirectory(CONFIG_FOLDER);
            if (!File.Exists(CONFIG_FOLDER + "/ccheartbeat.properties")) ClassiCubeConfig.Save(CONFIG_FOLDER);
            ClassiCubeConfig.Load(CONFIG_FOLDER);
            CCService.Salt = Server.GenerateSalt();
            CCService.MojangAuth = false;
            CCService.NameSuffix = ClassiCubeConfig.NameSuffix;
            CCService.SkinPrefix = ClassiCubeConfig.SkinPrefix;
            if (Heartbeat.Heartbeats.Any(h => h is ClassiCubeHeartbeat)) return;
            Heartbeat CCBeat = new ClassiCubeHeartbeat(ClassiCubeConfig);
            Heartbeat.Register(CCBeat);
        }
    }
    public class Mppass2Authenticator : LoginAuthenticator
    {
        public override bool Verify(Player p, string mppass)
        {
            return Authenticate(p, mppass);
        }

        public static bool Authenticate(Player p, string mppass)
        {
            string calc = Server.CalcMppass(p.truename, ClassiCubePlugin.CCService.Salt);
            bool verified = mppass.CaselessEq(calc);
            if (!verified && !p.name.EndsWith(ClassiCubePlugin.CCConfig.NameSuffix))
            {
                return false;
            }
            else
            {
                ClassiCubePlugin.CCService.AcceptPlayer(p);
                return true;
            }
        }
    }
    public class ClassiCubeHeartbeat : Heartbeat
    {
        public HeartbeatConfig config;
        public new AuthService Auth = ClassiCubePlugin.CCService;
        public static string LastResponse;
        public ClassiCubeHeartbeat(HeartbeatConfig conf)
        {
            URL = "http://www.classicube.net/heartbeat.jsp";
            config = conf;
        }
        public string proxyUrl;
        public bool checkedAddr;
        public void CheckAddress()
        {
            string hostUrl;
            checkedAddr = true;

            try
            {
                hostUrl = GetHost();
                proxyUrl = EnsureIPv4Url(hostUrl);
            }
            catch (Exception ex)
            {
                Logger.LogError("Error retrieving DNS information for CC", ex);
            }
        }

        // only want ASCII alphanumerical characters for salt
        public static bool AcceptableSaltChar(char c)
        {
            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z')
                || (c >= '0' && c <= '9');
        }

        /// <summary> Generates a random salt that is used for calculating mppasses. </summary>
        public static string GenerateSalt()
        {
            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            char[] str = new char[32];
            byte[] one = new byte[1];

            for (int i = 0; i < str.Length;)
            {
                rng.GetBytes(one);
                if (!AcceptableSaltChar((char)one[0])) continue;

                str[i] = (char)one[0]; i++;
            }
            string salt = new string(str);
            File.WriteAllText("text/cc_salt.txt", salt);
            if (File.Exists("text/cc_salt2.txt") && salt == File.ReadAllText("text/cc_salt2.txt"))
            {
                GenerateNewSalt();
            }
            return salt;
        }
        public static void GenerateNewSalt()
        {
            GenerateSalt();
        }
        public override string GetHeartbeatData()
        {
            string name = config.Name;
            OnSendingHeartbeatEvent.Call(this, ref name);
            name = Colors.StripUsed(name);

            return
                "&port=" + config.Port +
                "&max=" + config.MaxPlayers +
                "&name=" + name +
                "&public=" + config.Public +
                "&version=7" +
                "&salt=" + GenerateSalt() +
                "&users=" + PlayerInfo.NonHiddenUniqueIPCount() +
                "&software=" + Uri.EscapeDataString(config.Software) +
                "&web=" + config.WebClient;
        }
        public override void OnRequest(HttpWebRequest request)
        {
            request.ContentType = "application/x-www-form-urlencoded";

            if (!checkedAddr) CheckAddress();

            if (proxyUrl == null) return;
            request.Proxy = new WebProxy(proxyUrl);
            Logger.Log(LogType.Debug, request.GetRequestStream().ToString());
        }

        public override void OnResponse(WebResponse response)
        {
            string text = HttpUtil.GetResponseText(response);
            if (!NeedsProcessing(text)) return;

            if (!text.Contains("\"errors\":"))
            {
                OnSuccess(text);
            }
            else
            {
                string error = GetError(text);
                if (error == null) error = "Error while finding CC URL. Is the port open?";
                OnError(error);
            }
        }
        public static void OnError(string error)
        {
            error = Truncate(error);
            Logger.Log(LogType.Warning, error);
        }


        public static string GetError(string json)
        {
            JsonReader reader = new JsonReader(json);
            // silly design, but form of json is:
            // {
            //   "errors": [ ["Error 1"], ["Error 2"] ],
            //   "response": "",
            //   "status": "fail"
            // }
            JsonObject obj = reader.Parse() as JsonObject;
            if (obj == null || !obj.ContainsKey("errors")) return null;

            JsonArray errors = obj["errors"] as JsonArray;
            if (errors == null) return null;

            foreach (object raw in errors)
            {
                JsonArray err = raw as JsonArray;
                if (err != null && err.Count > 0) return (string)err[0];
            }
            return null;
        }

        public static string Truncate(string text)
        {
            if (text.Length < 256) return text;

            return text.Substring(0, 256) + "..";
        }
        public bool NeedsProcessing(string text)
        {
            if (string.IsNullOrEmpty(text)) return false;
            if (text == LastResponse) return false;

            // only need to process responses that have changed
            LastResponse = text;
            return true;
        }

        public static void OnSuccess(string text)
        {
            text = Truncate(text);
            File.WriteAllText("text/ccexternalurl.txt", text);
            Logger.Log(LogType.SystemActivity, "CC URL found: " + text);
        }

        public override void OnFailure(string response)
        {
            if (!string.IsNullOrEmpty(response))
                Logger.Log(LogType.Warning, "[CC] Error: Server responded with " + response);
        }
    }

    public class ConfigLongAttribute : ConfigSignedIntegerAttribute
    {
        public long defValue, minValue, maxValue;

        public ConfigLongAttribute() : this(null, null, 0, long.MinValue, long.MaxValue)
        {
        }
        public ConfigLongAttribute(string name, string section, long def,
                                  long min = long.MinValue, long max = long.MaxValue) : base(name, section)
        {
            defValue = def;
            minValue = min;
            maxValue = max;
        }

        public override object Parse(string value)
        {
            return ParseLong(value, defValue, minValue, maxValue);
        }
    }

    public class HeartbeatConfig
    {
        [ConfigString("name", "Auth service", "[MCGalaxy] Default", false)]
        public string Name = "[MCGalaxy] Default";
        [ConfigString("software", "Auth service", "MCGalaxy 1.9.5.1", false)]
        public string Software = "MCGalaxy 1.9.5.1";
        [ConfigLong("max-players", "Auth service", 16, 0, long.MaxValue)]
        public long MaxPlayers = 16;
        [ConfigInt("port", "Auth service", 25565, 0, int.MaxValue)]
        public int Port = 25565;
        [ConfigBool("support-web-client", "Auth service", true)]
        public bool WebClient = true;
        [ConfigBool("public", "Auth service", true)]
        public bool Public = true;
        [ConfigString("name-suffix", "Auth service", " (CC)", true)]
        public string NameSuffix = " (CC)";
        [ConfigString("skin-prefix", "Auth service", "", true)]
        public string SkinPrefix = "";

        public static INetListen Listener = new TcpListen();
        public static ConfigElement[] cfg;
        public void Load(string path)
        {
            if (cfg == null) cfg = ConfigElement.GetAll(typeof(HeartbeatConfig));
            ConfigElement.ParseFile(cfg, path + "/ccheartbeat.properties", this);
            SetupSocket(Port);
        }
        public static void SetupSocket(int port)
        {
            IPAddress ip;
            if (!IPAddress.TryParse(Server.Config.ListenIP, out ip))
            {
                Logger.Log(LogType.Warning, "Unable to parse listen IP config key, listening on any IP");
                ip = IPAddress.Any;
            }
            Listener.Listen(ip, port);
        }
        public void Save(string path)
        {
            if (cfg == null) cfg = ConfigElement.GetAll(typeof(HeartbeatConfig));
            using (StreamWriter w = new StreamWriter(path + "/ccheartbeat.properties"))
            {
                w.WriteLine("# This file contains settings for configuring the CC heartbeat.");
                w.WriteLine("# name-suffix - See description in authservices.properties");
                w.WriteLine("# skin-prefix - See description in authservices.properties");
                w.WriteLine();
                ConfigElement.Serialise(cfg, w, this);
            }
        }

        public static string externalIP;
        public static string GetExternalIP()
        {
            if (externalIP != null) return externalIP;

            try
            {
                externalIP = HttpUtil.LookupExternalIP();
            }
            catch (Exception ex)
            {
                Logger.LogError("Retrieving external IP", ex);
            }
            return externalIP;
        }
    }
}