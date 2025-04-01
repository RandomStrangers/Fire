//reference System.dll
//reference System.Net.dll
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using Flames.Config;
using Flames.Network;
using Flames.Modules.Relay;
using Flames.Events.PlayerEvents;
using Flames.Games;
using Flames.Tasks;
using Flames.Util;
using System.Collections.Generic;
using Flames.Events;
using Flames.Events.ServerEvents;
using System.Net.Security;
using System.Net.Sockets;
namespace Flames.DiscordInverse
{
    public class DiscordInversePlugin : Plugin
    {
        public override string name
        {
            get
            {
                return "DiscordInverse";
            }
        }
        public override string creator
        {
            get
            {
                return "krowteNynomraH";
            }
        }
        public override string MCGalaxy_Version
        {
            get
            {
                return "1.9.4.9";
            }
        }
        public override int build
        {
            get
            {
                return 2;
            }
        }
        public override string welcome
        {
            get
            {
                return "";
            }
        }
        public override bool LoadAtStartup
        {
            get
            {
                return true;
            }
        }
        public override string Flames_Version
        {
            get
            {
                return Server.FlamesVersion;
            }
        }
        public static DiscordInverseConfig Config = new DiscordInverseConfig();
        public static DiscordInverseBot Bot = new DiscordInverseBot();
        public override void Load(bool startup)
        {
            Server.EnsureDirectoryExists("text/discord");
            Command.Register(new CmdDiscordInverseBot());
            Command.Register(new CmdDiscordInverseControllers());
            Bot.Config = Config;
            Bot.ReloadConfig();
            Bot.Connect();
            OnConfigUpdatedEvent.Register(OnConfigUpdated, Priority.Low);
        }
        public override void Unload(bool shutdown)
        {
            Command.Unregister(Command.Find("DiscordInverseBot"));
            Command.Unregister(Command.Find("DiscordInverseControllers"));
            OnConfigUpdatedEvent.Unregister(OnConfigUpdated);
            Bot.Disconnect("Disconnecting DiscordInverse bot");
        }
        public void OnConfigUpdated()
        {
            Bot.ReloadConfig();
        }
    }
    public class CmdDiscordInverseBot : RelayBotCmd
    {
        public override string name
        {
            get
            {
                return "DiscordInverseBot";
            }
        }
        public override RelayBot Bot
        {
            get
            {
                return DiscordInversePlugin.Bot;
            }
        }
    }
    public class CmdDiscordInverseControllers : BotControllersCmd
    {
        public override string name
        {
            get
            {
                return "DiscordInverseControllers";
            }
        }
        public override RelayBot Bot
        {
            get
            {
                return DiscordInversePlugin.Bot;
            }
        }
    }
    public class DiscordInverseConfig
    {
        [ConfigBool("enabled", "General", false)]
        public bool Enabled;
        [ConfigString("bot-token", "General", "", true)]
        public string BotToken = "";
        [ConfigBool("use-nicknames", "General", true)]
        public bool UseNicks = true;
        [ConfigString("channel-ids", "General", "", true)]
        public string Channels = "";
        [ConfigString("op-channel-ids", "General", "", true)]
        public string OpChannels = "";
        [ConfigString("ignored-user-ids", "General", "", true)]
        public string IgnoredUsers = "";
        [ConfigBool("presence-enabled", "Presence (Status)", true)]
        public bool PresenceEnabled = true;
        [ConfigEnum("presence-status", "Presence (Status)", PresenceStatus.online, typeof(PresenceStatus))]
        public PresenceStatus Status = PresenceStatus.online;
        [ConfigEnum("presence-activity", "Presence (Status)", PresenceActivity.Playing, typeof(PresenceActivity))]
        public PresenceActivity Activity = PresenceActivity.Playing;
        [ConfigString("status-message", "Presence (Status)", "with {PLAYERS} players")]
        public string StatusMessage = "with {PLAYERS} players";
        [ConfigBool("can-mention-users", "Mentions", true)]
        public bool CanMentionUsers = true;
        [ConfigBool("can-mention-roles", "Mentions", true)]
        public bool CanMentionRoles = true;
        [ConfigBool("can-mention-everyone", "Mentions", false)]
        public bool CanMentionHere;
        [ConfigInt("embed-color", "Embeds", 9758051)]
        public int EmbedColor = 9758051;
        [ConfigBool("embed-show-game-statuses", "Embeds", true)]
        public bool EmbedGameStatuses = true;
        [ConfigInt("extra-intents", "Intents", 0)]
        public int ExtraIntents;
        public const string PROPS_PATH = "properties/discordbot(Inverse).properties";
        public static ConfigElement[] cfg;
        public void Load()
        {
            if (!File.Exists(PROPS_PATH))
            {
                Save();
            }
            if (cfg == null)
            {
                cfg = ConfigElement.GetAll(typeof(DiscordInverseConfig));
            }
            ConfigElement.ParseFile(cfg, PROPS_PATH, this);
        }
        public void Save()
        {
            if (cfg == null)
            {
                cfg = ConfigElement.GetAll(typeof(DiscordInverseConfig));
            }
            using (StreamWriter w = new StreamWriter(PROPS_PATH))
            {
                w.WriteLine("# DiscordInverse relay bot configuration");
                w.WriteLine("# See " + Updater.WikiURL + "Discord-relay-bot/");
                w.WriteLine();
                ConfigElement.Serialise(cfg, w, this);
            }
        }
    }
    public enum PresenceStatus
    {
        online,
        dnd,
        idle,
        invisible
    }
    public enum PresenceActivity
    {
        Playing = 0,
        Listening = 2,
        Watching = 3,
        Competing = 5,
        Empty = 6
    }

    public class DiscordInverseApiClient : AsyncWorker<DiscordInverseApiMessage>
    {
        public string Token;
        public string Host;
        public DiscordInverseApiMessage GetNextRequest()
        {
            if (queue.Count == 0)
            {
                return null;
            }
            DiscordInverseApiMessage first = queue.Dequeue();
            while (queue.Count > 0)
            {
                DiscordInverseApiMessage next = queue.Peek();
                if (!next.CombineWith(first))
                {
                    break;
                }
                queue.Dequeue();
            }
            return first;
        }
        public override string ThreadName
        {
            get
            {
                return "Discord-ApiClient";
            }
        }
        public override void HandleNext()
        {
            DiscordInverseApiMessage msg = null;
            WebResponse res = null;
            lock (queueLock)
            {
                msg = GetNextRequest();
            }
            if (msg == null)
            {
                WaitForWork();
                return;
            }
            for (int retry = 0; retry < 10; retry++)
            {
                try
                {
                    HttpWebRequest req = HttpUtil.CreateRequest(Host + msg.Path);
                    req.Method = msg.Method;
                    req.ContentType = "application/json";
                    req.Headers[HttpRequestHeader.Authorization] = "Bot " + Token;
                    string data = Json.SerialiseObject(msg.ToJson());
                    HttpUtil.SetRequestData(req, Encoding.UTF8.GetBytes(data));
                    msg.OnRequest(req);
                    res = req.GetResponse();
                    string resp = HttpUtil.GetResponseText(res);
                    msg.ProcessResponse(resp);
                    break;
                }
                catch (WebException ex)
                {
                    bool canRetry = HandleErrorResponse(ex, msg, retry);
                    HttpUtil.DisposeErrorResponse(ex);
                    if (!canRetry)
                    {
                        return;
                    }
                }
                catch (Exception ex)
                {
                    LogError(ex, msg);
                    return;
                }
            }
            string remaining = res.Headers["X-RateLimit-Remaining"];
            if (remaining == "1")
            {
                SleepForRetryPeriod(res);
            }
        }
        public static bool HandleErrorResponse(WebException ex, DiscordInverseApiMessage msg, int retry)
        {
            string err = HttpUtil.GetErrorResponse(ex);
            HttpStatusCode status = GetStatus(ex);
            if (status == (HttpStatusCode)429)
            {
                SleepForRetryPeriod(ex.Response);
                return true;
            }
            if (status >= (HttpStatusCode)500 && status <= (HttpStatusCode)504)
            {
                LogWarning(ex);
                LogResponse(err);
                return retry < 2;
            }
            if (ex.Status == WebExceptionStatus.NameResolutionFailure)
            {
                LogWarning(ex);
                return false;
            }
            if (ex.InnerException is IOException)
            {
                LogWarning(ex);
                return retry < 2;
            }
            LogError(ex, msg);
            LogResponse(err);
            return false;
        }
        public static HttpStatusCode GetStatus(WebException ex)
        {
            if (ex.Response == null)
            {
                return 0;
            }
            return ((HttpWebResponse)ex.Response).StatusCode;
        }
        public static void LogError(Exception ex, DiscordInverseApiMessage msg)
        {
            string target = "(" + msg.Method + " " + msg.Path + ")";
            Logger.Log(LogType.Error, "Error sending request to DiscordInverse API {0}: {1}", target, ex);
        }
        public static void LogWarning(Exception ex)
        {
            Logger.Log(LogType.Warning, "Error sending request to DiscordInverse API - {0}", ex.Message);
        }
        public static void LogResponse(string err)
        {
            if (string.IsNullOrEmpty(err))
            {
                return;
            }
            if (err.Length > 200)
            {
                err = err.Substring(0, 200) + "...";
            }
            Logger.Log(LogType.Warning, "DiscordInverse API returned: {0}", err);
        }
        public static void SleepForRetryPeriod(WebResponse res)
        {
            string resetAfter = res.Headers["X-RateLimit-Reset-After"];
            string retryAfter = res.Headers["Retry-After"];
            float delay;
            if (NumberUtils.TryParseSingle(resetAfter, out delay) && delay > 0)
            {
            }
            else if (NumberUtils.TryParseSingle(retryAfter, out delay) && delay > 0)
            {
            }
            else
            {
                delay = 30;
            }
            Logger.Log(LogType.SystemActivity, "DiscordInverse bot ratelimited! Trying again in {0} seconds..", delay);
            Thread.Sleep(TimeSpan.FromSeconds(delay + 0.5f));
        }
    }
    public class DiscordInverseUser : RelayUser
    {
        public string ReferencedUser;
        public override string GetMessagePrefix()
        {
            if (string.IsNullOrEmpty(ReferencedUser))
            {
                return "";
            }
            return "@" + ReferencedUser + " ";
        }
    }
    public class DiscordInverseBot : RelayBot
    {
        public DiscordInverseApiClient api;
        public DiscordInverseWebsocket socket;
        public DiscordInverseSession session;
        public string botUserID;
        public Dictionary<string, byte> channelTypes = new Dictionary<string, byte>();
        public const byte CHANNEL_DIRECT = 0;
        public const byte CHANNEL_TEXT = 1;
        public List<string> filter_triggers = new List<string>();
        public List<string> filter_replacements = new List<string>();
        public JsonArray allowed;
        public override string RelayName
        {
            get
            {
                return "drocsiD";
            }
        }
        public override bool Enabled
        {
            get
            {
                return Config.Enabled;
            }
        }
        public override string UserID
        {
            get
            {
                return botUserID;
            }
        }
        public DiscordInverseConfig Config;
        public TextFile replacementsFile = new TextFile("text/discord/replacements(Inverse).txt",
                                        "// This file is used to replace words/phrases sent to DiscordInverse",
                                        "// Lines starting with // are ignored",
                                        "// Lines should be formatted like this:",
                                        "// example:http://example.org",
                                        "// That would replace 'example' in messages sent to DiscordInverse with 'http://example.org'");
        public string APIHost = "https://discord.com/api/v10";
        public string WSHost = "gateway.discord.gg";
        public string WSPath = "/?v=10&encoding=json";
        public override bool CanReconnect
        {
            get
            {
                return canReconnect && (socket == null || socket.CanReconnect);
            }
        }
        public override void DoConnect()
        {
            socket = new DiscordInverseWebsocket(WSPath)
            {
                Session = session,
                Token = Config.BotToken,
                Host = WSHost,
                Presence = Config.PresenceEnabled,
                Status = Config.Status,
                Activity = Config.Activity,
                GetStatus = GetStatusMessage,
                OnReady = HandleReadyEvent,
                OnResumed = HandleResumedEvent,
                OnMessageCreate = HandleMessageEvent,
                OnChannelCreate = HandleChannelEvent,
                OnGatewayEvent = HandleGatewayEvent
            };
            socket.Connect();
        }
        public static Exception UnpackError(Exception ex)
        {
            if (ex.InnerException is ObjectDisposedException)
            {
                return ex.InnerException;
            }
            if (ex.InnerException is IOException)
            {
                return ex.InnerException;
            }
            return null;
        }
        public override void DoReadLoop()
        {
            try
            {
                socket.ReadLoop();
            }
            catch (Exception ex)
            {
                Exception unpacked = UnpackError(ex);
                if (unpacked != null)
                {
                    throw unpacked;
                }
                throw;
            }
        }
        public override void DoDisconnect(string reason)
        {
            try
            {
                socket.Disconnect();
            }
            catch (Exception ex)
            {
                Logger.Log(LogType.Error, "Error disconnecting DiscordInverse socket: {0}", ex);
            }
        }
        public override void ReloadConfig()
        {
            Config.Load();
            base.ReloadConfig();
            LoadReplacements();
            if (!Config.CanMentionHere)
            {
                return;
            }
            Logger.Log(LogType.Warning, "can-mention-everyone option is enabled in {0}, " +
                       "which allows pinging all users on DiscordInverse from in-game. " +
                       "It is recommended that this option be disabled.", DiscordInverseConfig.PROPS_PATH);
        }
        public override void UpdateConfig()
        {
            Channels = Config.Channels.SplitComma();
            OpChannels = Config.OpChannels.SplitComma();
            IgnoredUsers = Config.IgnoredUsers.SplitComma();
            UpdateAllowed();
            LoadBannedCommands();
        }
        public void UpdateAllowed()
        {
            JsonArray mentions = new JsonArray();
            if (Config.CanMentionUsers)
            {
                mentions.Add("users");
            }
            if (Config.CanMentionRoles)
            {
                mentions.Add("roles");
            }
            if (Config.CanMentionHere)
            {
                mentions.Add("everyone");
            }
            allowed = mentions;
        }
        public void LoadReplacements()
        {
            replacementsFile.EnsureExists();
            string[] lines = replacementsFile.GetText();
            filter_triggers.Clear();
            filter_replacements.Clear();
            ChatTokens.LoadTokens(lines, (phrase, replacement) =>
            {
                filter_triggers.Add(phrase);
                filter_replacements.Add(DiscordInverseUtils.MarkdownToSpecial(replacement));
            });
        }
        public override void LoadControllers()
        {
            Controllers = PlayerList.Load("text/discord/controllers(Inverse).txt");
        }
        public DiscordInverseUser ExtractUser(JsonObject data)
        {
            JsonObject author = (JsonObject)data["author"];
            DiscordInverseUser user = new DiscordInverseUser
            {
                Nick = GetNick(data) ?? GetUser(author),
                ID = (string)author["id"],
                ReferencedUser = ExtractReferencedUser(data)
            };
            return user;
        }
        public string GetNick(JsonObject data)
        {
            if (!Config.UseNicks)
            {
                return null;
            }
            object raw;
            if (!data.TryGetValue("member", out raw))
            {
                return null;
            }
            if (!(raw is JsonObject member))
            {
                return null;
            }
            member.TryGetValue("nick", out raw);
            return raw as string;
        }
        public string GetUser(JsonObject author)
        {
            author.TryGetValue("global_name", out object name);
            if (name != null)
            {
                return (string)name;
            }
            return (string)author["username"];
        }
        public string ExtractReferencedUser(JsonObject data)
        {
            data.TryGetValue("referenced_message", out object refMsgRaw);
            if (!(refMsgRaw is JsonObject refMsgData))
            {
                return null;
            }
            refMsgData.TryGetValue("author", out object authorRaw);
            if (authorRaw == null)
            {
                return null;
            }
            return GetUser((JsonObject)authorRaw);
        }
        public void HandleMessageEvent(JsonObject data)
        {
            RelayUser user = ExtractUser(data);
            string channel = (string)data["channel_id"];
            string message = (string)data["content"];
            if (IsProxy(message) && user.ID == "999409543001931788")
            {
                return;
            }
            if (!channelTypes.TryGetValue(channel, out byte type))
            {
                type = GuessChannelType(data);
                if (type == CHANNEL_TEXT)
                {
                    channelTypes[channel] = type;
                }
            }
            if (user.ID == botUserID && type == CHANNEL_DIRECT)
            {
                return;
            }
            if (user.ID == botUserID && channel == "1164773079143157810")
            {
                return;
            }
            if (user.ID == botUserID && channel == "1165306166608404560")
            {
                return;
            }
            if (type == CHANNEL_DIRECT && user.ID != botUserID)
            {
                HandleDirectMessage(user, channel, message);
            }
            else
            {
                HandleChannelMessage(user, channel, message);
                PrintAttachments(user, data, channel);
            }
        }
        public bool IsProxy(string message)
        {
            foreach (char letter in message)
            {
                if (letter == ' ')
                {
                    break;
                }
                if (letter == '/')
                {
                    return true;
                }
            }
            return false;
        }
        public void PrintAttachments(RelayUser user, JsonObject data, string channel)
        {
            if (!data.TryGetValue("attachments", out object raw))
            {
                return;
            }
            if (!(raw is JsonArray list))
            {
                return;
            }
            foreach (object entry in list)
            {
                if (!(entry is JsonObject attachment))
                {
                    continue;
                }
                string url = (string)attachment["url"];
                HandleChannelMessage(user, channel, url);
            }
        }
        public void HandleChannelEvent(JsonObject data)
        {
            string channel = (string)data["id"];
            string type = (string)data["type"];
            if (type == "1")
            {
                channelTypes[channel] = CHANNEL_DIRECT;
            }
        }
        public byte GuessChannelType(JsonObject data)
        {
            if (data.ContainsKey("member"))
            {
                return CHANNEL_TEXT;
            }
            if (data.ContainsKey("webhook_id"))
            {
                return CHANNEL_TEXT;
            }
            return CHANNEL_DIRECT;
        }
        public void HandleReadyEvent(JsonObject data)
        {
            JsonObject user = (JsonObject)data["user"];
            botUserID = (string)user["id"];
            HandleResumedEvent(data);
        }
        public void HandleResumedEvent(JsonObject data)
        {
            if (api == null)
            {
                api = new DiscordInverseApiClient
                {
                    Token = Config.BotToken,
                    Host = APIHost
                };
                api.RunAsync();
            }
            OnReady();
        }
        public void HandleGatewayEvent(string eventName, JsonObject data)
        {
            OnGatewayEventReceivedEvent.Call(this, eventName, data);
        }
        public static bool IsEscaped(char c)
        {
            return (c > ' ' && c <= '/') || (c >= ':' && c <= '@')
                || (c >= '[' && c <= '`') || (c >= '{' && c <= '~');
        }
        public override string ParseMessage(string input)
        {
            StringBuilder sb = new StringBuilder(input);
            SimplifyCharacters(sb);
            sb.Replace("\uFE0F", "");
            int length = sb.Length - 1;
            for (int i = 0; i < length; i++)
            {
                if (sb[i] != '\\')
                {
                    continue;
                }
                if (!IsEscaped(sb[i + 1]))
                {
                    continue;
                }
                sb.Remove(i, 1);
                length--;
            }
            StripMarkdown(sb);
            return sb.ToString();
        }
        public static void StripMarkdown(StringBuilder sb)
        {
            sb.Replace("**", "");
        }
        public object updateLocker = new object();
        public volatile bool updateScheduled;
        public DateTime nextUpdate;
        public void UpdateDiscordInverseStatus()
        {
            TimeSpan delay = default(TimeSpan);
            DateTime now = DateTime.UtcNow;
            lock (updateLocker)
            {
                if (updateScheduled)
                {
                    return;
                }
                updateScheduled = true;
                if (nextUpdate > now)
                {
                    delay = nextUpdate - now;
                }
            }
            Server.MainScheduler.QueueOnce(DoUpdateStatus, null, delay);
        }
        public void DoUpdateStatus(SchedulerTask task)
        {
            DateTime now = DateTime.UtcNow;
            lock (updateLocker)
            {
                updateScheduled = false;
                nextUpdate = now.AddSeconds(0.5);
            }
            DiscordInverseWebsocket s = socket;
            if (s == null || !s.SentIdentify)
            {
                return;
            }
            try
            {
                s.UpdateStatus();
            }
            catch (Exception ex)
            {
                Logger.Log(LogType.Error, "Error updating DiscordInverse status: {0}", ex);
            }
        }
        public string GetStatusMessage()
        {
            string numOnline = PlayerInfo.NonHiddenUniqueIPCount().ToString();
            if (Config.StatusMessage.Contains("{PLAYERS}"))
            {
                return Config.StatusMessage.Replace("{PLAYERS}", numOnline);
            }
            if (Config.StatusMessage.Contains("{SERVER}"))
            {
                return Config.StatusMessage.Replace("{SERVER}", Colors.Strip(Server.Config.Name));
            }
            return Config.StatusMessage;
        }
        public override void OnStart()
        {
            DiscordInverseSession s = new DiscordInverseSession
            {
                Intents = DiscordInverseWebsocket.DEFAULT_INTENTS | Config.ExtraIntents
            };
            session = s;
            base.OnStart();
            OnPlayerConnectEvent.Register(HandlePlayerConnect, Priority.Low);
            OnPlayerDisconnectEvent.Register(HandlePlayerDisconnect, Priority.Low);
            OnPlayerActionEvent.Register(HandlePlayerAction, Priority.Low);
        }
        public override void OnStop()
        {
            socket = null;
            if (api != null)
            {
                api.StopAsync();
                api = null;
            }
            base.OnStop();
            OnPlayerConnectEvent.Unregister(HandlePlayerConnect);
            OnPlayerDisconnectEvent.Unregister(HandlePlayerDisconnect);
            OnPlayerActionEvent.Unregister(HandlePlayerAction);
        }
        public void HandlePlayerConnect(Player p)
        {
            UpdateDiscordInverseStatus();
        }
        public void HandlePlayerDisconnect(Player p, string reason)
        {
            UpdateDiscordInverseStatus();
        }

        public void HandlePlayerAction(Player p, PlayerAction action, string message, bool stealth)
        {
            if (action != PlayerAction.Hide && action != PlayerAction.Unhide)
            {
                return;
            }
            UpdateDiscordInverseStatus();
        }
        public void Send(DiscordInverseApiMessage msg)
        {
            api?.QueueAsync(msg);
        }
        public static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
        public override void DoSendMessage(string channel, string message)
        {
            message = ConvertMessage(message);
            const int MAX_MSG_LEN = 2000;
            for (int offset = 0; offset < message.Length; offset += MAX_MSG_LEN)
            {
                int partLen = Math.Min(message.Length - offset, MAX_MSG_LEN);
                string part = message.Substring(offset, partLen);
                ChannelSendMessage msg = new ChannelSendMessage(channel, part)
                {
                    Allowed = allowed
                };
                Send(msg);
            }
        }
        public string ConvertMessage(string message)
        {
            message = Colors.StripUsed(message);
            message = message.Replace("(", "《");
            message = message.Replace(")", "》");
            message = message.Replace("《", ")");
            message = message.Replace("》", "(");
            message = Reverse(message);
            message = message.Replace("[", "○");
            message = message.Replace("]", "●");
            message = message.Replace("○", "]");
            message = message.Replace("●", "[");
            message = DiscordInverseUtils.EscapeMarkdown(message);
            message = DiscordInverseUtils.SpecialToMarkdown(message);
            return message;
        }
        public override string PrepareMessage(string message)
        {
            for (int i = 0; i < filter_triggers.Count; i++)
            {
                message = message.Replace(filter_triggers[i], filter_replacements[i]);
            }
            return message;
        }
        public override bool CheckController(string userID, ref string error)
        {
            return true;
        }
        public override string UnescapeFull(Player p)
        {
            return BOLD + base.UnescapeFull(p) + BOLD;
        }
        public override string UnescapeNick(Player p)
        {
            return BOLD + base.UnescapeNick(p) + BOLD;
        }
        public override void MessagePlayers(RelayPlayer p)
        {
            ChannelSendEmbed embed = new ChannelSendEmbed(p.ChannelID);
            List<OnlineListEntry> entries = PlayerInfo.GetOnlineList(p, p.Rank, out int total);
            embed.Color = Config.EmbedColor;
            embed.Title = string.Format("enilno yltnerruc {1}reyalp {0}",
                                        total, total.Plural());
            foreach (OnlineListEntry e in entries)
            {
                if (e.players.Count == 0)
                {
                    continue;
                }
                embed.Fields.Add(
                    ConvertMessage(FormatRank(e)),
                    ConvertMessage(FormatPlayers(p, e))
                );
            }
            AddGameStatus(embed);
            OnSendingWhoEmbedEvent.Call(this, p.User, ref embed);
            Send(embed);
        }
        public static string FormatPlayers(Player p, OnlineListEntry e)
        {
            return e.players.Join(pl => FormatNick(p, pl), ", ");
        }
        public static string FormatRank(OnlineListEntry e)
        {
            return string.Format(UNDERLINE + "{0}" + UNDERLINE + " (" + CODE + "{1}" + CODE + ")",
                                 e.group.GetFormattedName(), e.players.Count);
        }
        public static string FormatNick(Player p, Player pl)
        {
            string flags = OnlineListEntry.GetFlags(pl);
            string format;
            if (flags.Length > 0)
            {
                format = BOLD + "{0}" + BOLD + ITALIC + "{2}" + ITALIC + " (" + CODE + "{1}" + CODE + ")";
            }
            else
            {
                format = BOLD + "{0}" + BOLD + " (" + CODE + "{1}" + CODE + ")";
            }
            return string.Format(format, p.FormatNick(pl),
                                 pl.level.name.Replace('_', DiscordInverseUtils.UNDERSCORE),
                                 flags);
        }
        public void AddGameStatus(ChannelSendEmbed embed)
        {
            if (!Config.EmbedGameStatuses)
            {
                return;
            }
            StringBuilder sb = new StringBuilder();
            IGame[] games = IGame.RunningGames.Items;
            foreach (IGame game in games)
            {
                Level lvl = game.Map;
                if (!game.Running || lvl == null)
                {
                    continue;
                }
                sb.Append(BOLD + game.GameName + BOLD + " is running on " + lvl.name + "\n");
            }
            if (sb.Length == 0)
            {
                return;
            }
            embed.Fields.Add("Running games", ConvertMessage(sb.ToString()));
        }
        public const string UNDERLINE = DiscordInverseUtils.UNDERLINE;
        public const string BOLD = DiscordInverseUtils.BOLD;
        public const string ITALIC = DiscordInverseUtils.ITALIC;
        public const string CODE = DiscordInverseUtils.CODE;
        public const string SPOILER = DiscordInverseUtils.SPOILER;
        public const string STRIKETHROUGH = DiscordInverseUtils.STRIKETHROUGH;
    }
    public delegate void OnSendingWhoEmbed(DiscordInverseBot bot, RelayUser user, ref ChannelSendEmbed embed);
    public class OnSendingWhoEmbedEvent : IEvent<OnSendingWhoEmbed>
    {
        public static void Call(DiscordInverseBot bot, RelayUser user, ref ChannelSendEmbed embed)
        {
            IEvent<OnSendingWhoEmbed>[] items = handlers.Items;
            for (int i = 0; i < items.Length; i++)
            {
                try
                {
                    items[i].method(bot, user, ref embed);
                }
                catch (Exception ex)
                {
                    LogHandlerException(ex, items[i]);
                }
            }
        }
    }
    public delegate void OnGatewayEventReceived(DiscordInverseBot bot, string eventName, JsonObject data);
    public class OnGatewayEventReceivedEvent : IEvent<OnGatewayEventReceived>
    {
        public static void Call(DiscordInverseBot bot, string eventName, JsonObject data)
        {
            IEvent<OnGatewayEventReceived>[] items = handlers.Items;
            for (int i = 0; i < items.Length; i++)
            {
                try
                {
                    items[i].method(bot, eventName, data);
                }
                catch (Exception ex)
                {
                    LogHandlerException(ex, items[i]);
                }
            }
        }
    }
    public abstract class DiscordInverseApiMessage
    {
        public string Path;
        public string Method = "POST";
        public abstract JsonObject ToJson();
        public virtual bool CombineWith(DiscordInverseApiMessage prior)
        {
            return false;
        }
        public virtual void OnRequest(HttpWebRequest req)
        {
        }
        public virtual void ProcessResponse(string response)
        {
        }
    }
    public class ChannelSendMessage : DiscordInverseApiMessage
    {
        public static JsonArray default_allowed = new JsonArray()
        {
            "users", "roles"
        };
        public StringBuilder content;
        public JsonArray Allowed;
        public ChannelSendMessage(string channelID, string message)
        {
            Path = "/channels/" + channelID + "/messages";
            content = new StringBuilder(message);
        }
        public override JsonObject ToJson()
        {
            JsonObject allowed = new JsonObject()
            {
                {
                    "parse", Allowed ?? default_allowed
                }
            };
            return new JsonObject()
            {
                {
                    "content", content.ToString()
                },
                {
                    "allowed_mentions", allowed
                }
            };
        }
        public override bool CombineWith(DiscordInverseApiMessage prior)
        {
            if (!(prior is ChannelSendMessage msg) || msg.Path != Path)
            {
                return false;
            }
            if (content.Length + msg.content.Length > 1024)
            {
                return false;
            }
            msg.content.Append('\n');
            msg.content.Append(content.ToString());
            content.Length = 0;
            return true;
        }
    }
    public class ChannelSendEmbed : DiscordInverseApiMessage
    {
        public string Title;
        public Dictionary<string, string> Fields = new Dictionary<string, string>();
        public int Color;
        public ChannelSendEmbed(string channelID)
        {
            Path = "/channels/" + channelID + "/messages";
        }
        public JsonArray GetFields()
        {
            JsonArray arr = new JsonArray();
            foreach (var raw in Fields)
            {
                JsonObject field = new JsonObject()
                {
                    {
                        "name", raw.Key
                    },
                    {
                        "value", raw.Value
                    }
                };
                arr.Add(field);
            }
            return arr;
        }
        public override JsonObject ToJson()
        {
            return new JsonObject()
            {
                {
                    "embeds", new JsonArray()
                    {
                        new JsonObject()
                        {
                            {
                                "title", Title
                            },
                            {
                                "color", Color
                            },
                            {
                                "fields", GetFields()
                            }
                        }
                    }
                },
                {
                    "allowed_mentions", new JsonObject()
                    {
                        {
                            "parse", new JsonArray()
                        }
                    }
                }
            };
        }
    }
    public class DiscordInverseSession
    {
        public string ID;
        public string LastSeq;
        public int Intents;
    }
    public delegate string DiscordInverseGetStatus();
    public delegate void GatewayEventCallback(string eventName, JsonObject data);
    public class DiscordInverseWebsocket : ClientWebSocket
    {
        public string Token;
        public string Host;
        public bool CanReconnect = true;
        public bool SentIdentify;
        public DiscordInverseSession Session;
        public bool Presence = true;
        public PresenceStatus Status;
        public PresenceActivity Activity;
        public DiscordInverseGetStatus GetStatus;
        public Action<JsonObject> OnReady;
        public Action<JsonObject> OnResumed;
        public Action<JsonObject> OnMessageCreate;
        public Action<JsonObject> OnChannelCreate;
        public GatewayEventCallback OnGatewayEvent;
        public object sendLock = new object();
        public SchedulerTask heartbeat;
        public TcpClient client;
        public SslStream stream;
        public bool readable;
        public const int DEFAULT_INTENTS = INTENT_GUILD_MESSAGES | INTENT_DIRECT_MESSAGES | INTENT_MESSAGE_CONTENT;
        public const int INTENT_GUILD_MESSAGES = 1 << 9;
        public const int INTENT_DIRECT_MESSAGES = 1 << 12;
        public const int INTENT_MESSAGE_CONTENT = 1 << 15;
        public const int OPCODE_DISPATCH = 0;
        public const int OPCODE_HEARTBEAT = 1;
        public const int OPCODE_IDENTIFY = 2;
        public const int OPCODE_STATUS_UPDATE = 3;
        public const int OPCODE_VOICE_STATE_UPDATE = 4;
        public const int OPCODE_RESUME = 6;
        public const int OPCODE_REQUEST_SERVER_MEMBERS = 8;
        public const int OPCODE_INVALID_SESSION = 9;
        public const int OPCODE_HELLO = 10;
        public const int OPCODE_HEARTBEAT_ACK = 11;
        public DiscordInverseWebsocket(string apiPath)
        {
            path = apiPath;
        }
        public override bool LowLatency
        {
            set
            {
            }
        }
        public override IPAddress IP
        {
            get
            {
                return null;
            }
        }
        public void Connect()
        {
            client = new TcpClient();
            client.Connect(Host, 443);
            readable = true;
            stream = HttpUtil.WrapSSLStream(client.GetStream(), Host);
            protocol = this;
            Init();
        }
        public override void WriteCustomHeaders()
        {
            WriteHeader("Authorization: Bot " + Token);
            WriteHeader("Host: " + Host);
        }
        public override void Close()
        {
            readable = false;
            Server.Heartbeats.Cancel(heartbeat);
            try
            {
                client.Close();
            }
            catch (Exception ex)
            {
                Logger.Log(LogType.Error, "Error closing DiscordInverse socket: {0}", ex);
            }
        }
        public const int REASON_INVALID_TOKEN = 4004;
        public const int REASON_DENIED_INTENT = 4014;
        public override void OnDisconnected(int reason)
        {
            SentIdentify = false;
            if (readable)
            {
                Logger.Log(LogType.SystemActivity, "DiscordInverse relay bot closing: " + reason);
            }
            Close();
            if (reason == REASON_INVALID_TOKEN)
            {
                CanReconnect = false;
                throw new InvalidOperationException("DiscordInverse relay: Invalid bot token provided - unable to connect");
            }
            else if (reason == REASON_DENIED_INTENT)
            {
                CanReconnect = false;
                throw new InvalidOperationException("DiscordInverse relay: Message Content Intent is not enabled in Bot Account settings, " +
                    "therefore Discord will prevent the bot from being able to see the contents of DiscordInverse messages\n" +
                    "(See " + Updater.WikiURL + "Discord-relay-bot#read-permissions)");
            }
        }
        public void ReadLoop()
        {
            byte[] data = new byte[4096];
            readable = true;
            while (readable)
            {
                int len = stream.Read(data, 0, 4096);
                if (len == 0)
                {
                    throw new IOException("stream.Read returned 0");
                }
                HandleReceived(data, len);
            }
        }
        public override void HandleData(byte[] data, int len)
        {
            string value = Encoding.UTF8.GetString(data, 0, len);
            JsonReader ctx = new JsonReader(value);
            JsonObject obj = (JsonObject)ctx.Parse();
            if (obj == null)
            {
                return;
            }
            int opcode = NumberUtils.ParseInt32((string)obj["op"]);
            DispatchPacket(opcode, obj);
        }
        public void DispatchPacket(int opcode, JsonObject obj)
        {
            if (opcode == OPCODE_DISPATCH)
            {
                HandleDispatch(obj);
            }
            else if (opcode == OPCODE_HELLO)
            {
                HandleHello(obj);
            }
            else if (opcode == OPCODE_INVALID_SESSION)
            {
                Session.ID = null;
                Session.LastSeq = null;
                Logger.Log(LogType.Warning, "DiscordInverse relay: Resuming failed, trying again in 5 seconds");
                Thread.Sleep(5 * 1000);
                Identify();
            }
        }
        public void HandleHello(JsonObject obj)
        {
            JsonObject data = (JsonObject)obj["d"];
            string interval = (string)data["heartbeat_interval"];
            int msInterval = NumberUtils.ParseInt32(interval);
            heartbeat = Server.Heartbeats.QueueRepeat(SendHeartbeat, null,
                                          TimeSpan.FromMilliseconds(msInterval));
            Identify();
        }
        public void HandleDispatch(JsonObject obj)
        {
            if (obj.TryGetValue("s", out object sequence))
            {
                Session.LastSeq = (string)sequence;
            }
            string eventName = (string)obj["t"];
            obj.TryGetValue("d", out object rawData);
            JsonObject data = rawData as JsonObject;
            if (eventName == "READY")
            {
                HandleReady(data);
                OnReady(data);
            }
            else if (eventName == "RESUMED")
            {
                OnResumed(data);
            }
            else if (eventName == "MESSAGE_CREATE")
            {
                OnMessageCreate(data);
            }
            else if (eventName == "CHANNEL_CREATE")
            {
                OnChannelCreate(data);
            }
            OnGatewayEvent(eventName, data);
        }
        public void HandleReady(JsonObject data)
        {
            if (data.TryGetValue("session_id", out object session))
            {
                Session.ID = (string)session;
            }
        }
        public void SendMessage(int opcode, JsonObject data)
        {
            JsonObject obj = new JsonObject()
            {
                {
                    "op", opcode
                },
                {
                    "d", data
                }
            };
            SendMessage(obj);
        }
        public void SendMessage(JsonObject obj)
        {
            string str = Json.SerialiseObject(obj);
            Send(Encoding.UTF8.GetBytes(str), SendFlags.None);
        }
        public override void SendRaw(byte[] data, SendFlags flags)
        {
            lock (sendLock)
            {
                stream.Write(data);
            }
        }
        public void SendHeartbeat(SchedulerTask task)
        {
            JsonObject obj = new JsonObject
            {
                ["op"] = OPCODE_HEARTBEAT
            };
            if (Session.LastSeq != null)
            {
                obj["d"] = NumberUtils.ParseInt32(Session.LastSeq);
            }
            else
            {
                obj["d"] = null;
            }
            SendMessage(obj);
        }
        public void Identify()
        {
            if (Session.ID != null && Session.LastSeq != null)
            {
                SendMessage(OPCODE_RESUME, MakeResume());
            }
            else
            {
                SendMessage(OPCODE_IDENTIFY, MakeIdentify());
            }
            SentIdentify = true;
        }
        public void UpdateStatus()
        {
            JsonObject data = MakePresence();
            SendMessage(OPCODE_STATUS_UPDATE, data);
        }
        public JsonObject MakeResume()
        {
            return new JsonObject()
            {
                {
                    "token", Token
                },
                {
                    "session_id", Session.ID
                },
                {
                    "seq", NumberUtils.ParseInt32(Session.LastSeq)
                }
            };
        }
        public JsonObject MakeIdentify()
        {
            JsonObject props = new JsonObject()
            {
                {
                    "$os", Platform.IOperatingSystem.DetectOS().ToString()
                },
                {
                    "$browser", Colors.Strip(Server.SoftwareName)
                },
                {
                    "$device", Colors.Strip(Server.SoftwareName)
                }
            };
            return new JsonObject()
            {
                {
                    "token", Token
                },
                {
                    "intents", Session.Intents
                },
                {
                    "properties", props
                },
                {
                    "presence", MakePresence()
                }
            };
        }
        public JsonObject MakePresence()
        {
            if (!Presence)
            {
                return null;
            }
            JsonObject activity = new JsonObject()
            {
                {
                    "name", GetStatus()
                },
                {
                    "type", (int)Activity
                }
            };
            return new JsonObject()
            {
                {
                    "since", Server.StartTime.ToString()
                },
                {
                    "activities", new JsonArray()
                    {
                        activity
                    }
                },
                {
                    "status", Status.ToString()
                },
                {
                    "afk", false
                }
            };
        }
    }
    public static class DiscordInverseUtils
    {
        public static string[] markdown_special =
        {
            @"\",
            @"*",
            @"_",
            @"~",
            @"`",
            @"|",
            @"-",
            @"#"
        };
        public static string[] markdown_escaped =
        {
            @"\\",
            @"\*",
            @"\_",
            @"\~",
            @"\`",
            @"\|",
            @"\-",
            @"\#"
        };
        public static string EscapeMarkdown(string message)
        {
            for (int i = 0; i < markdown_special.Length; i++)
            {
                message = message.Replace(markdown_special[i], markdown_escaped[i]);
            }
            return message;
        }
        public const char UNDERSCORE = '\uEDC1';
        public const char TILDE = '\uEDC2';
        public const char STAR = '\uEDC3';
        public const char GRAVE = '\uEDC4';
        public const char BAR = '\uEDC5';
        public const string UNDERLINE = "\uEDC1\uEDC1";
        public const string BOLD = "\uEDC3\uEDC3";
        public const string ITALIC = "\uEDC1";
        public const string CODE = "\uEDC4";
        public const string SPOILER = "\uEDC5\uEDC5";
        public const string STRIKETHROUGH = "\uEDC2\uEDC2";
        public static string MarkdownToSpecial(string input)
        {
            return input
                .Replace('_', UNDERSCORE)
                .Replace('~', TILDE)
                .Replace('*', STAR)
                .Replace('`', GRAVE)
                .Replace('|', BAR);
        }
        public static string SpecialToMarkdown(string input)
        {
            return input
                .Replace(UNDERSCORE, '_')
                .Replace(TILDE, '~')
                .Replace(STAR, '*')
                .Replace(GRAVE, '`')
                .Replace(BAR, '|');
        }
    }
}