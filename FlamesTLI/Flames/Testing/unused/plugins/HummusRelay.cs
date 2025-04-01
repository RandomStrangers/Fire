//reference System.dll
//reference System.Net.dll
//reference plugins/RelayRoute.dll
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Flames.Config;
using Flames.Events.PlayerEvents;
using Flames.Events.ServerEvents;
using Flames.Games;
using Flames.Network;
using Flames.Tasks;
using Flames.Util;
using Flames.Modules.Relay.Discord;
using Flames.Modules.Relay;

namespace Flames.Modules.Relay.Hummus
{
    /// <summary> Represents an abstract Hummus API message </summary>
    public abstract class HummusApiMessage
    {
        /// <summary> The path/route that will handle this message </summary>
        /// <example> /channels/{channel id}/messages </example>
        public string Path;
        /// <summary> The HTTP method to handle the path/route with </summary>
        /// <example> POST, PATCH, DELETE </example>
        public string Method = "POST";

        /// <summary> Converts this message into its JSON representation </summary>
        public abstract JsonObject ToJson();

        /// <summary> Attempts to combine this message with a prior message to reduce API calls </summary>
        public virtual bool CombineWith(HummusApiMessage prior) { return false; }
    }

    /// <summary> Message for sending text to a channel </summary>
    public class ChannelSendMessage : HummusApiMessage
    {
        static JsonArray default_allowed = new JsonArray() { "users", "roles" };
        StringBuilder content;
        public JsonArray Allowed;

        public ChannelSendMessage(string channelID, string message)
        {
            Path = "/channels/" + channelID + "/messages";
            content = new StringBuilder(message);
        }

        public override JsonObject ToJson()
        {
            // only allow pinging certain groups
            JsonObject allowed = new JsonObject()
            {
                { "parse", Allowed ?? default_allowed }
            };

            return new JsonObject()
            {
                { "content", content.ToString() },
                { "allowed_mentions", allowed }
            };
        }

        public override bool CombineWith(HummusApiMessage prior)
        {
            ChannelSendMessage msg = prior as ChannelSendMessage;
            if (msg == null || msg.Path != Path) return false;

            if (content.Length + msg.content.Length > 1024) return false;

            // TODO: is stringbuilder even beneficial here
            msg.content.Append('\n');
            msg.content.Append(content.ToString());
            content.Length = 0; // clear this
            return true;
        }
    }

    public class ChannelSendEmbed : HummusApiMessage
    {
        public string Title;
        public Dictionary<string, string> Fields = new Dictionary<string, string>();
        public int Color;

        public ChannelSendEmbed(string channelID)
        {
            Path = "/channels/" + channelID + "/messages";
        }

        JsonArray GetFields()
        {
            JsonArray arr = new JsonArray();
            foreach (var raw in Fields)
            {
                JsonObject field = new JsonObject()
                {
                    { "name",   raw.Key  },
                    { "value", raw.Value }
                };
                arr.Add(field);
            }
            return arr;
        }

        public override JsonObject ToJson()
        {
            return new JsonObject()
            {
                { "embeds", new JsonArray()
                    {
                        new JsonObject()
                        {
                            { "title", Title },
                            { "color", Color },
                            { "fields", GetFields() }
                        }
                    }
                },
                // no pinging anything
                { "allowed_mentions", new JsonObject()
                    {
                        { "parse", new JsonArray() }
                    }
                }
            };
        }
    }

    /// <summary> Implements a basic web client for sending messages to the Hummus API </summary>
    /// <remarks> https://Hummus.com/developers/docs/reference </remarks>
    /// <remarks> https://Hummus.com/developers/docs/resources/channel#create-message </remarks>
    public class HummusApiClient : AsyncWorker<HummusApiMessage>
    {
        public string Token;
        public const string host = "https://hummus.sys42.net/api/v6";

        public HummusApiMessage GetNextRequest()
        {
            if (queue.Count == 0) return null;
            HummusApiMessage first = queue.Dequeue();

            // try to combine messages to minimise API calls
            while (queue.Count > 0)
            {
                HummusApiMessage next = queue.Peek();
                if (!next.CombineWith(first)) break;
                queue.Dequeue();
            }
            return first;
        }

        public override string ThreadName { get { return "Hummus-ApiClient"; } }
        public override void HandleNext()
        {
            HummusApiMessage msg = null;
            WebResponse res = null;

            lock (queueLock) { msg = GetNextRequest(); }
            if (msg == null) { WaitForWork(); return; }

            for (int retry = 0; retry < 10; retry++)
            {
                try
                {
                    HttpWebRequest req = HttpUtil.CreateRequest(host + msg.Path);
                    req.Method = msg.Method;
                    req.ContentType = "application/json";
                    req.Headers[HttpRequestHeader.Authorization] = "Bot " + Token;

                    string data = Json.SerialiseObject(msg.ToJson());
                    HttpUtil.SetRequestData(req, Encoding.UTF8.GetBytes(data));
                    res = req.GetResponse();

                    HttpUtil.GetResponseText(res);
                    break;
                }
                catch (WebException ex)
                {
                    string err = HttpUtil.GetErrorResponse(ex);
                    HttpUtil.DisposeErrorResponse(ex);
                    HttpStatusCode status = GetStatus(ex);

                    // 429 errors simply require retrying after sleeping for a bit
                    if (status == (HttpStatusCode)429)
                    {
                        SleepForRetryPeriod(ex.Response);
                        continue;
                    }

                    // 500 errors might be temporary Hummus outage, so still retry a few times
                    if (status >= (HttpStatusCode)500 && status <= (HttpStatusCode)504)
                    {
                        LogWarning(ex);
                        LogResponse(err);
                        if (retry >= 2) return;
                        continue;
                    }

                    // If unable to reach Hummus at all, immediately give up
                    if (ex.Status == WebExceptionStatus.NameResolutionFailure)
                    {
                        LogWarning(ex);
                        return;
                    }

                    // May be caused by connection dropout/reset, so still retry a few times
                    if (ex.InnerException is IOException)
                    {
                        LogWarning(ex);
                        if (retry >= 2) return;
                        continue;
                    }

                    LogError(ex, msg);
                    LogResponse(err);
                    return;
                }
                catch (Exception ex)
                {
                    LogError(ex, msg);
                    return;
                }
            }

            // Avoid triggering HTTP 429 error if possible
            string remaining = res.Headers["X-RateLimit-Remaining"];
            if (remaining == "1") SleepForRetryPeriod(res);
        }


        static HttpStatusCode GetStatus(WebException ex)
        {
            if (ex.Response == null) return 0;
            return ((HttpWebResponse)ex.Response).StatusCode;
        }

        public static void LogError(Exception ex, HummusApiMessage msg)
        {
            string target = "(" + msg.Method + " " + msg.Path + ")";
            Logger.LogError("Error sending request to Hummus API " + target, ex);
        }

        public static void LogWarning(Exception ex)
        {
            Logger.Log(LogType.Warning, "Error sending request to Hummus API - " + ex.Message);
        }

        public static void LogResponse(string err)
        {
            if (string.IsNullOrEmpty(err)) return;

            // Hummus sometimes returns <html>..</html> responses for internal server errors
            //  most of this is useless content, so just truncate these particular errors 
            if (err.Length > 200) err = err.Substring(0, 200) + "...";

            Logger.Log(LogType.Warning, "Hummus API returned: " + err);
        }


        public static void SleepForRetryPeriod(WebResponse res)
        {
            string resetAfter = res.Headers["X-RateLimit-Reset-After"];
            string retryAfter = res.Headers["Retry-After"];
            float delay;

            if (Utils.TryParseSingle(resetAfter, out delay) && delay > 0)
            {
                // Prefer Hummus "X-RateLimit-Reset-After" (millisecond precision)
            }
            else if (Utils.TryParseSingle(retryAfter, out delay) && delay > 0)
            {
                // Fallback to general "Retry-After" header
            }
            else
            {
                // No recommended retry delay.. 30 seconds is a good bet
                delay = 30;
            }

            Logger.Log(LogType.SystemActivity, "Hummus bot ratelimited! Trying again in {0} seconds..", delay);
            Thread.Sleep(TimeSpan.FromSeconds(delay + 0.5f));
        }
    }
    public class HummusBot : RelayBot
    {
        public HummusApiClient api;
        public HummusWebsocket socket;
        public HummusSession session;
        public string botUserID;

        public Dictionary<string, byte> channelTypes = new Dictionary<string, byte>();
        public const byte CHANNEL_DIRECT = 0;
        public const byte CHANNEL_TEXT = 1;

        public List<string> filter_triggers = new List<string>();
        public List<string> filter_replacements = new List<string>();
        public JsonArray allowed;

        public override string RelayName { get { return "Hummus"; } }
        public override bool Enabled { get { return Config.Enabled; } }
        public override string UserID { get { return botUserID; } }
        public HummusConfig Config;

        public TextFile replacementsFile = new TextFile("text/hummus/replacements.txt",
                                        "// This file is used to replace words/phrases sent to Hummus",
                                        "// Lines starting with // are ignored",
                                        "// Lines should be formatted like this:",
                                        "// example:http://example.org",
                                        "// That would replace 'example' in messages sent to Hummus with 'http://example.org'");


        public override bool CanReconnect
        {
            get { return canReconnect && (socket == null || socket.CanReconnect); }
        }

        public override void DoConnect()
        {
            socket = new HummusWebsocket();
            socket.Session = session;
            socket.Token = Config.BotToken;
            socket.Presence = Config.PresenceEnabled;
            socket.Status = Config.Status;
            socket.Activity = Config.Activity;
            socket.GetStatus = GetStatusMessage;

            socket.OnReady = HandleReadyEvent;
            socket.OnResumed = HandleResumedEvent;
            socket.OnMessageCreate = HandleMessageEvent;
            socket.OnChannelCreate = HandleChannelEvent;
            socket.Connect();
        }

        // mono wraps exceptions from reading in an AggregateException, e.g:
        //   * AggregateException - One or more errors occurred.
        //      * ObjectDisposedException - Cannot access a disposed object.
        // .NET sometimes wraps exceptions from reading in an IOException, e.g.:
        //   * IOException - The read operation failed, see inner exception.
        //      * ObjectDisposedException - Cannot access a disposed object.
        public static Exception UnpackError(Exception ex)
        {
            if (ex.InnerException is ObjectDisposedException)
                return ex.InnerException;
            if (ex.InnerException is IOException)
                return ex.InnerException;

            // TODO can we ever get an IOException wrapping an IOException?
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
                // throw a more specific exception if possible
                if (unpacked != null) throw unpacked;

                // rethrow original exception otherwise
                throw;
            }
        }

        public override void DoDisconnect(string reason)
        {
            try
            {
                socket.Disconnect();
            }
            catch
            {
                // no point logging disconnect failures
            }
        }


        public override void ReloadConfig()
        {
            Config.Load();
            base.ReloadConfig();
            LoadReplacements();

            if (!Config.CanMentionHere) return;
            Logger.Log(LogType.Warning, "can-mention-everyone option is enabled in {0}, " +
                       "which allows pinging all users on Hummus from in-game. " +
                       "It is recommended that this option be disabled.", HummusConfig.PROPS_PATH);
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
            if (Config.CanMentionUsers) mentions.Add("users");
            if (Config.CanMentionRoles) mentions.Add("roles");
            if (Config.CanMentionHere) mentions.Add("everyone");
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
                filter_replacements.Add(MarkdownToSpecial(replacement));
            });
        }

        public override void LoadControllers()
        {
            Controllers = PlayerList.Load("text/Hummus/controllers.txt");
        }


        public string GetNick(JsonObject data)
        {
            if (!Config.UseNicks) return null;
            object raw;
            if (!data.TryGetValue("member", out raw)) return null;

            // Make sure this is really a member object first
            JsonObject member = raw as JsonObject;
            if (member == null) return null;

            member.TryGetValue("nick", out raw);
            return raw as string;
        }

        public string GetUser(JsonObject author)
        {
            // User's chosen display name (configurable)
            object name = null;
            author.TryGetValue("global_name", out name);
            if (name != null) return (string)name;

            return (string)author["username"];
        }

        public RelayUser ExtractUser(JsonObject data)
        {
            JsonObject author = (JsonObject)data["author"];

            RelayUser user = new RelayUser();
            user.Nick = GetNick(data) ?? GetUser(author);
            user.ID = (string)author["id"];
            return user;
        }


        public void HandleReadyEvent(JsonObject data)
        {
            JsonObject user = (JsonObject)data["user"];
            botUserID = (string)user["id"];
            HandleResumedEvent(data);
        }

        public void HandleResumedEvent(JsonObject data)
        {
            // May not be null when reconnecting
            if (api == null)
            {
                api = new HummusApiClient();
                api.Token = Config.BotToken;
                api.RunAsync();
            }
            OnReady();
        }

        public void PrintAttachments(RelayUser user, JsonObject data, string channel)
        {
            object raw;
            if (!data.TryGetValue("attachments", out raw)) return;

            JsonArray list = raw as JsonArray;
            if (list == null) return;

            foreach (object entry in list)
            {
                JsonObject attachment = entry as JsonObject;
                if (attachment == null) continue;

                string url = (string)attachment["url"];
                HandleChannelMessage(user, channel, url);
            }
        }

        public void HandleMessageEvent(JsonObject data)
        {
            RelayUser user = ExtractUser(data);
            // ignore messages from self
            if (user.ID == botUserID) return;

            string channel = (string)data["channel_id"];
            string message = (string)data["content"];
            byte type;

            // Working out whether a channel is a direct message channel
            //  or not without querying the Hummus API is a bit of a pain
            // In v6 api, a CHANNEL_CREATE event was always emitted for
            //  direct message channels - hence the relatively simple
            //  solution was to treat every other channels as text channels
            // However, in v8 api changelog the following entry is noted:
            //  "Bots no longer receive Channel Create Gateway Event for DMs"
            // Therefore the code is now forced to instead calculate which
            //  channels are probably text channels, and which aren't        
            if (!channelTypes.TryGetValue(channel, out type))
            {
                type = GuessChannelType(data);
                // channel is definitely a text/normal channel
                if (type == CHANNEL_TEXT) channelTypes[channel] = type;
            }

            if (type == CHANNEL_DIRECT)
            {
                HandleDirectMessage(user, channel, message);
            }
            else
            {
                HandleChannelMessage(user, channel, message);
                PrintAttachments(user, data, channel);
            }
        }

        public void HandleChannelEvent(JsonObject data)
        {
            string channel = (string)data["id"];
            string type = (string)data["type"];

            // 1 = direct/private message channel type
            if (type == "1") channelTypes[channel] = CHANNEL_DIRECT;
        }

        byte GuessChannelType(JsonObject data)
        {
            // As per Hummus's documentation:
            //  "The member object exists in MESSAGE_CREATE and MESSAGE_UPDATE
            //   events from text-based guild channels, provided that the
            //   author of the message is not a webhook"
            if (data.ContainsKey("member")) return CHANNEL_TEXT;

            // As per Hummus's documentation
            //  "You can tell if a message is generated by a webhook by
            //   checking for the webhook_id on the message object."
            if (data.ContainsKey("webhook_id")) return CHANNEL_TEXT;

            // TODO are there any other cases to consider?
            return CHANNEL_DIRECT; // unknown
        }


        public static bool IsEscaped(char c)
        {
            // To match Hummus: \a --> \a, \* --> *
            return (c > ' ' && c <= '/') || (c >= ':' && c <= '@')
                || (c >= '[' && c <= '`') || (c >= '{' && c <= '~');
        }
        public override string ParseMessage(string input)
        {
            StringBuilder sb = new StringBuilder(input);
            SimplifyCharacters(sb);

            // remove variant selector character used with some emotes
            sb.Replace("\uFE0F", "");

            // unescape \ escaped characters
            //  -1 in case message ends with a \
            int length = sb.Length - 1;
            for (int i = 0; i < length; i++)
            {
                if (sb[i] != '\\') continue;
                if (!IsEscaped(sb[i + 1])) continue;

                sb.Remove(i, 1); length--;
            }

            StripMarkdown(sb);
            return sb.ToString();
        }

        public static void StripMarkdown(StringBuilder sb)
        {
            // TODO proper markdown parsing
            sb.Replace("**", "");
        }


        public object updateLocker = new object();
        public volatile bool updateScheduled;
        public DateTime nextUpdate;

        public void UpdateHummusStatus()
        {
            TimeSpan delay = default(TimeSpan);
            DateTime now = DateTime.UtcNow;

            // websocket gets disconnected with code 4008 if try to send too many updates too quickly
            lock (updateLocker)
            {
                // status update already pending?
                if (updateScheduled) return;
                updateScheduled = true;

                // slowdown if sending too many status updates
                if (nextUpdate > now) delay = nextUpdate - now;
            }

            Server.MainScheduler.QueueOnce(DoUpdateStatus, null, delay);
        }

        public void DoUpdateStatus(SchedulerTask task)
        {
            DateTime now = DateTime.UtcNow;
            // OK to queue next status update now
            lock (updateLocker)
            {
                updateScheduled = false;
                nextUpdate = now.AddSeconds(0.5);
                // ensures status update can't be sent more than once every 0.5 seconds
            }

            HummusWebsocket s = socket;
            // websocket gets disconnected with code 4003 if tries to send data before identifying
            //  https://Hummus.com/developers/docs/topics/opcodes-and-status-codes
            if (s == null || !s.SentIdentify) return;

            try { s.UpdateStatus(); } catch { }
        }

        public string GetStatusMessage()
        {
            fakeGuest.group = Group.DefaultRank;
            List<Player> online = PlayerInfo.GetOnlineCanSee(fakeGuest, fakeGuest.Rank);

            string numOnline = online.Count.ToString();
            return Config.StatusMessage.Replace("{PLAYERS}", numOnline);
        }


        public override void OnStart()
        {
            session = new HummusSession();
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

        public void HandlePlayerConnect(Player p) { UpdateHummusStatus(); }
        public void HandlePlayerDisconnect(Player p, string reason) { UpdateHummusStatus(); }

        public void HandlePlayerAction(Player p, PlayerAction action, string message, bool stealth)
        {
            if (action != PlayerAction.Hide && action != PlayerAction.Unhide) return;
            UpdateHummusStatus();
        }


        /// <summary> Asynchronously sends a message to the Hummus API </summary>
        public void Send(HummusApiMessage msg)
        {
            // can be null in gap between initial connection and ready event received
            if (api != null) api.QueueAsync(msg);
        }

        public override void DoSendMessage(string channel, string message)
        {
            message = ConvertMessage(message);
            const int MAX_MSG_LEN = 2000;

            // Hummus doesn't allow more than 2000 characters in a single message,
            //  so break up message into multiple parts for this extremely rare case
            //  https://Hummus.com/developers/docs/resources/channel#create-message
            for (int offset = 0; offset < message.Length; offset += MAX_MSG_LEN)
            {
                int partLen = Math.Min(message.Length - offset, MAX_MSG_LEN);
                string part = message.Substring(offset, partLen);

                ChannelSendMessage msg = new ChannelSendMessage(channel, part);
                msg.Allowed = allowed;
                Send(msg);
            }
        }

        /// <summary> Formats a message for displaying on Hummus </summary>
        /// <example> Escapes markdown characters such as _ and * </example>
        public string ConvertMessage(string message)
        {
            message = ConvertMessageCommon(message);
            message = Colors.StripUsed(message);
            message = EscapeMarkdown(message);
            message = SpecialToMarkdown(message);
            return message;
        }

        public static string[] markdown_special = { @"\", @"*", @"_", @"~", @"`", @"|", @"-", @"#" };
        public static string[] markdown_escaped = { @"\\", @"\*", @"\_", @"\~", @"\`", @"\|", @"\-", @"\#" };
        public static string EscapeMarkdown(string message)
        {
            // don't let user use bold/italic etc markdown
            for (int i = 0; i < markdown_special.Length; i++)
            {
                message = message.Replace(markdown_special[i], markdown_escaped[i]);
            }
            return message;
        }

        public override string PrepareMessage(string message)
        {
            // allow uses to do things like replacing '+' with ':green_square:'
            for (int i = 0; i < filter_triggers.Count; i++)
            {
                message = message.Replace(filter_triggers[i], filter_replacements[i]);
            }
            return message;
        }


        // all users are already verified by Hummus
        public override bool CheckController(string userID, ref string error) { return true; }

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
            int total;
            List<OnlineListEntry> entries = PlayerInfo.GetOnlineList(p, p.Rank, out total);

            embed.Color = Config.EmbedColor;
            embed.Title = string.Format("{0} player{1} currently online",
                                        total, total.Plural());

            foreach (OnlineListEntry e in entries)
            {
                if (e.players.Count == 0) continue;

                embed.Fields.Add(
                    ConvertMessage(FormatRank(e)),
                    ConvertMessage(FormatPlayers(p, e))
                );
            }
            AddGameStatus(embed);
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
                                 // level name must not have _ escaped as the level name is in a code block -
                                 //  otherwise the escaped "\_" actually shows as "\_" instead of "_" 
                                 pl.level.name.Replace('_', UNDERSCORE),
                                 flags);
        }

        public void AddGameStatus(ChannelSendEmbed embed)
        {
            if (!Config.EmbedGameStatuses) return;

            StringBuilder sb = new StringBuilder();
            IGame[] games = IGame.RunningGames.Items;

            foreach (IGame game in games)
            {
                Level lvl = game.Map;
                if (!game.Running || lvl == null) continue;
                sb.Append(BOLD + game.GameName + BOLD + " is running on " + lvl.name + "\n");
            }

            if (sb.Length == 0) return;
            embed.Fields.Add("Running games", ConvertMessage(sb.ToString()));
        }


        // these characters are chosen specifically to lie within the unspecified unicode range,
        //  as those characters are "application defined" (EDCX = Escaped Hummus Character #X)
        //  https://en.wikipedia.org/wiki/Private_Use_Areas
        public const char UNDERSCORE = '\uEDC1'; // _
        public const char TILDE = '\uEDC2'; // ~
        public const char STAR = '\uEDC3'; // *
        public const char GRAVE = '\uEDC4'; // `
        public const char BAR = '\uEDC5'; // |

        public const string UNDERLINE = "\uEDC1\uEDC1"; // __
        public const string BOLD = "\uEDC3\uEDC3"; // **
        public const string ITALIC = "\uEDC1"; // _
        public const string CODE = "\uEDC4"; // `
        public const string SPOILER = "\uEDC5\uEDC5"; // ||
        public const string STRIKETHROUGH = "\uEDC2\uEDC2"; // ~~

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

    public class HummusConfig
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

        public const string PROPS_PATH = "properties/Hummusbot.properties";
        public static ConfigElement[] cfg;

        public void Load()
        {
            // create default config file
            if (!File.Exists(PROPS_PATH)) Save();

            if (cfg == null) cfg = ConfigElement.GetAll(typeof(HummusConfig));
            ConfigElement.ParseFile(cfg, PROPS_PATH, this);
        }

        public void Save()
        {
            if (cfg == null) cfg = ConfigElement.GetAll(typeof(HummusConfig));

            using (StreamWriter w = new StreamWriter(PROPS_PATH))
            {
                w.WriteLine("# Hummus relay bot configuration");
                w.WriteLine("# See " + Updater.WikiURL + "Discord-relay-bot/");
                w.WriteLine();
                ConfigElement.Serialise(cfg, w, this);
            }
        }
    }

    public enum PresenceStatus { online, dnd, idle, invisible }
    public enum PresenceActivity { Playing = 0, Listening = 2, Watching = 3, Competing = 5 }

    public class HummusPlugin : Plugin
    {
        public override string name { get { return "HummusRelay"; } }

        public static HummusConfig Config = new HummusConfig();
        public static HummusBot Bot = new HummusBot();
        public override void Load(bool startup)
        {
		EnsureDirectoryExists("text/Hummus");
			Command.Register(new CmdHummusBot());
			Command.Register(new CmdHummusControllers());            
        Server.Background.QueueOnce(Load2, null, TimeSpan.FromSeconds(5));
        }
        public void Load2(SchedulerTask task) {
			Bot.Config = Config;
            Bot.ReloadConfig();
            Bot.Connect();
            OnConfigUpdatedEvent.Register(OnConfigUpdated, Priority.Low);
        }
		public static void EnsureDirectoryExists(string dir) {
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
        }

        public override void Unload(bool shutdown)
        {
            OnConfigUpdatedEvent.Unregister(OnConfigUpdated);
            Bot.Disconnect("Disconnecting Hummus bot");
        }

        public void OnConfigUpdated() { Bot.ReloadConfig(); }
    }

    public class CmdHummusBot : RelayBotCmd
    {
        public override string name { get { return "HummusBot"; } }
        public override RelayBot Bot { get { return HummusPlugin.Bot; } }
    }

    public class CmdHummusControllers : BotControllersCmd
    {
        public override string name { get { return "HummusControllers"; } }
        public override RelayBot Bot { get { return HummusPlugin.Bot; } }
    }
    public class HummusSession
    {
        public string ID, LastSeq;
        public int Intents = HummusWebsocket.DEFAULT_INTENTS;
    }
    public delegate string HummusGetStatus();

    /// <summary> Implements a basic websocket for communicating with Hummus's gateway </summary>
    /// <remarks> https://Hummus.com/developers/docs/topics/gateway </remarks>
    /// <remarks> https://i.imgur.com/Lwc5Wde.png </remarks>
    public class HummusWebsocket : ClientWebSocket
    {
        /// <summary> Authorisation token for the bot account </summary>
        public string Token;
        public bool CanReconnect = true, SentIdentify;
        public HummusSession Session;

        /// <summary> Whether presence support is enabled </summary>
        public bool Presence = true;
        /// <summary> Presence status (E.g. online) </summary>
        public PresenceStatus Status;
        /// <summary> Presence activity (e.g. Playing) </summary>
        public PresenceActivity Activity;
        /// <summary> Callback function to retrieve the activity status message </summary>
        public HummusGetStatus GetStatus;

        /// <summary> Callback invoked when a ready event has been received </summary>
        public Action<JsonObject> OnReady;
        /// <summary> Callback invoked when a resumed event has been received </summary>
        public Action<JsonObject> OnResumed;
        /// <summary> Callback invoked when a message created event has been received </summary>
        public Action<JsonObject> OnMessageCreate;
        /// <summary> Callback invoked when a channel created event has been received </summary>
        public Action<JsonObject> OnChannelCreate;

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
        // public const int OPCODE_VOICE_STATE_UPDATE = 4; //Unused
        public const int OPCODE_RESUME = 6;
        // public const int OPCODE_REQUEST_SERVER_MEMBERS = 8; //Unused
       public const int OPCODE_INVALID_SESSION = 9;
       public const int OPCODE_HELLO = 10;
        // public const int OPCODE_HEARTBEAT_ACK   = 11; //Unused


     public HummusWebsocket() 
	{
            path = "/?v=6&encoding=json";
        }
        
        public const string host = "hummus-gateway.sys42.net";
        // stubs
        public override bool LowLatency { set { } }
        public override IPAddress IP { get { return null; } }

        public void Connect()
        {
            client = new TcpClient();
            client.Connect(host, 443);
            readable = true;

            stream = HttpUtil.WrapSSLStream(client.GetStream(), host);
            protocol = this;
            Init();
        }

        public override void WriteCustomHeaders()
        {
            WriteHeader("Authorization: Bot " + Token);
            WriteHeader("Host: " + host);
        }

        public override void Close()
        {
            readable = false;
            Server.Heartbeats.Cancel(heartbeat);
            try
            {
                client.Close();
            }
            catch
            {
                // ignore errors when closing socket
            }
        }

        public const int REASON_INVALID_TOKEN = 4004;
        public const int REASON_DENIED_INTENT = 4014;

       public override void OnDisconnected(int reason)
        {
            SentIdentify = false;
            if (readable) Logger.Log(LogType.SystemActivity, "Hummus relay bot closing: " + reason);
            Close();

            if (reason == REASON_INVALID_TOKEN)
            {
                CanReconnect = false;
                throw new InvalidOperationException("Hummus relay: Invalid bot token provided - unable to connect");
            }
            else if (reason == REASON_DENIED_INTENT)
            {
                // privileged intent since August 2022 https://support-dev.Hummus.com/hc/en-us/articles/4404772028055
                CanReconnect = false;
                throw new InvalidOperationException("Hummus relay: Message Content Intent is not enabled in Bot Account settings, " +
                    "therefore Hummus will prevent the bot from being able to see the contents of Hummus messages\n" +
                    "(See " + Updater.SourceURL + "/wiki/Hummus-relay-bot#read-permissions)");
            }
        }


        public void ReadLoop()
        {
            byte[] data = new byte[4096];
            readable = true;

            while (readable)
            {
                int len = stream.Read(data, 0, 4096);
                if (len == 0) throw new IOException("stream.Read returned 0");

                HandleReceived(data, len);
            }
        }

     public override void HandleData(byte[] data, int len)
        {
            string value = Encoding.UTF8.GetString(data, 0, len);
            JsonReader ctx = new JsonReader(value);
            JsonObject obj = (JsonObject)ctx.Parse();
            if (obj == null) return;

            int opcode = int.Parse((string)obj["op"]);
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
                // See notes at https://Hummus.com/developers/docs/topics/gateway#resuming
                //  (note that in this implementation, if resume fails, the bot just
                //   gives up altogether instead of trying to resume again later)
                Session.ID = null;
                Session.LastSeq = null;

                Logger.Log(LogType.Warning, "Hummus relay: Resuming failed, trying again in 5 seconds");
                Thread.Sleep(5 * 1000);
                Identify();
            }
        }


        public void HandleHello(JsonObject obj)
        {
            JsonObject data = (JsonObject)obj["d"];
            string interval = (string)data["heartbeat_interval"];
            int msInterval = int.Parse(interval);

            heartbeat = Server.Heartbeats.QueueRepeat(SendHeartbeat, null,
                                          TimeSpan.FromMilliseconds(msInterval));
            Identify();
        }

        public void HandleDispatch(JsonObject obj)
        {
            // update last sequence number
            object sequence;
            if (obj.TryGetValue("s", out sequence))
                Session.LastSeq = (string)sequence;

            string eventName = (string)obj["t"];
            JsonObject data;

            if (eventName == "READY")
            {
                data = (JsonObject)obj["d"];
                HandleReady(data);
                OnReady(data);
            }
            else if (eventName == "RESUMED")
            {
                data = (JsonObject)obj["d"];
                OnResumed(data);
            }
            else if (eventName == "MESSAGE_CREATE")
            {
                data = (JsonObject)obj["d"];
                OnMessageCreate(data);
            }
            else if (eventName == "CHANNEL_CREATE")
            {
                data = (JsonObject)obj["d"];
                OnChannelCreate(data);
            }
        }

        public void HandleReady(JsonObject data)
        {
            object session;
            if (data.TryGetValue("session_id", out session))
                Session.ID = (string)session;
        }


        public void SendMessage(int opcode, JsonObject data)
        {
            JsonObject obj = new JsonObject()
            {
                { "op", opcode },
                { "d",  data }
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
            lock (sendLock) stream.Write(data);
        }

        public void SendHeartbeat(SchedulerTask task)
        {
            JsonObject obj = new JsonObject();
            obj["op"] = OPCODE_HEARTBEAT;

            if (Session.LastSeq != null)
            {
                obj["d"] = int.Parse(Session.LastSeq);
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
                { "token",      Token },
                { "session_id", Session.ID },
                { "seq",        int.Parse(Session.LastSeq) }
            };
        }

        public JsonObject MakeIdentify()
        {
            JsonObject props = new JsonObject()
            {
                { "$os",      "linux" },
                { "$browser", Server.SoftwareName },
                { "$device",  Server.SoftwareName }
            };

            return new JsonObject()
            {
                { "token",      Token },
                { "intents",    Session.Intents },
                { "properties", props },
                { "presence",   MakePresence() }
            };
        }

        public JsonObject MakePresence()
        {
            if (!Presence) return null;

            JsonObject activity = new JsonObject()
            {
                { "name", GetStatus() },
                { "type", (int)Activity }
            };
            return new JsonObject()
            {
                { "since",      null },
                { "activities", new JsonArray() { activity } },
                { "status",     Status.ToString() },
                { "afk",        false }
            };
        }
    }
}