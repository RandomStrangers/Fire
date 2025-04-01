//reference System.dll
//reference System.Net.dll
//reference System.Relay.dll
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Flames.Commands;
using Flames.Events.ServerEvents;
using Flames.Relay;
using Sharkbite.Irc;

namespace Flames.Relay.IRC0
{
    public enum IRCControllerVerify { None, HalfOp, OpChannel };

    /// <summary> Manages a connection to an IRC server, and handles associated events. </summary>
    public sealed class IRCBot0 : RelayBot
    {
        internal Connection conn;
        string botNick;
        IRCNickList0 nicks;
        bool ready;

        public override string RelayName { get { return "IRC0"; } }
        public override bool Enabled { get { return Server.Config.UseIRC; } }
        public override string UserID { get { return conn == null ? null : conn.Nick; } }

        public override void LoadControllers()
        {
            Controllers = PlayerList.Load("ranks/IRC_Controllers0.txt");
        }

        public IRCBot0()
        {
            nicks = new IRCNickList0();
            nicks.bot = this;
        }


        static char[] newline = { '\n' };
        protected override void DoSendMessage(string channel, string message)
        {
            if (!ready) return;
            message = ConvertMessage(message);

            // IRC messages can't have \r or \n in them
            //  https://stackoverflow.com/questions/13898584/insert-line-breaks-into-an-irc-message
            if (message.IndexOf('\n') == -1)
            {
                conn.SendMessage(channel, message);
                return;
            }

            string[] parts = message.Split(newline, StringSplitOptions.RemoveEmptyEntries);
            foreach (string part in parts)
            {
                conn.SendMessage(channel, part.Replace("\r", ""));
            }
        }

        public void Raw(string message)
        {
            if (!Enabled || !Connected) return;
            conn.SendRaw(message);
        }

        void Join(string channel)
        {
            if (string.IsNullOrEmpty(channel)) return;
            conn.SendJoin(channel);
        }


        protected override bool CanReconnect { get { return canReconnect; } }

        protected override void DoConnect()
        {
            ready = false;
            botNick = Server.Config.IRCNick.Replace(" ", "");

            if (conn == null) conn = new Connection(new UTF8Encoding(false));
            conn.Hostname = Server.Config.IRCServer;
            conn.Port = Server.Config.IRCPort;
            conn.UseSSL = Server.Config.IRCSSL;

            // most IRC servers supporting SSL/TLS do so on port 6697
            if (conn.Port == 6697) conn.UseSSL = true;

            conn.Nick = botNick;
            conn.UserName = botNick;
            conn.RealName = Server.SoftwareNameVersioned;
            HookIRCEvents();

            bool usePass = Server.Config.IRCIdentify && Server.Config.IRCPassword.Length > 0;
            conn.ServerPassword = usePass ? Server.Config.IRCPassword : "*";
            conn.Connect();
        }

        protected override void DoReadLoop()
        {
            conn.ReceiveIRCMessages();
        }

        protected override void DoDisconnect(string reason)
        {
            nicks.Clear();
            try
            {
                conn.Disconnect(reason);
            }
            catch
            {
                // no point logging disconnect failures
            }
            UnhookIRCEvents();
        }

        protected override void UpdateConfig()
        {
            Channels = Server.Config.IRCChannels.SplitComma();
            OpChannels = Server.Config.IRCOpChannels.SplitComma();
            IgnoredUsers = Server.Config.IRCIgnored.SplitComma();
            LoadBannedCommands();
        }


        static readonly string[] ircColors = new string[] {
            "\u000300", "\u000301", "\u000302", "\u000303", "\u000304", "\u000305",
            "\u000306", "\u000307", "\u000308", "\u000309", "\u000310", "\u000311",
            "\u000312", "\u000313", "\u000314", "\u000315",
        };
        static readonly string[] ircSingle = new string[] {
            "\u00030", "\u00031", "\u00032", "\u00033", "\u00034", "\u00035",
            "\u00036", "\u00037", "\u00038", "\u00039",
        };
        static readonly string[] ircReplacements = new string[] {
            "&f", "&0", "&1", "&2", "&c", "&4", "&5", "&6",
            "&e", "&a", "&3", "&b", "&9", "&d", "&8", "&7",
        };
        static readonly Regex ircTwoColorCode = new Regex("(\x03\\d{1,2}),\\d{1,2}");

        protected override string ParseMessage(string input)
        {
            // get rid of background color component of some IRC color codes.
            input = ircTwoColorCode.Replace(input, "$1");
            StringBuilder sb = new StringBuilder(input);

            for (int i = 0; i < ircColors.Length; i++)
            {
                sb.Replace(ircColors[i], ircReplacements[i]);
            }
            for (int i = 0; i < ircSingle.Length; i++)
            {
                sb.Replace(ircSingle[i], ircReplacements[i]);
            }
            SimplifyCharacters(sb);

            // remove misc formatting chars
            sb.Replace(BOLD, "");
            sb.Replace(ITALIC, "");
            sb.Replace(UNDERLINE, "");

            sb.Replace("\x03", "&f"); // color reset
            sb.Replace("\x0f", "&f"); // reset
            return sb.ToString();
        }

        /// <summary> Formats a message for displaying on IRC </summary>
        /// <example> Converts colors such as &amp;0 into IRC color codes </example>
        string ConvertMessage(string message)
        {
            if (string.IsNullOrEmpty(message.Trim())) message = ".";
            const string resetSignal = "\x03\x0F";

            message = ConvertMessageCommon(message);
            message = message.Replace("%S", "&f"); // TODO remove
            message = message.Replace("&S", "&f");
            message = message.Replace("&f", resetSignal);
            message = ToIRCColors(message);
            return message;
        }

        static string ToIRCColors(string input)
        {
            input = Colors.Escape(input);
            input = LineWrapper.CleanupColors(input, true, false);

            StringBuilder sb = new StringBuilder(input);
            for (int i = 0; i < ircColors.Length; i++)
            {
                sb.Replace(ircReplacements[i], ircColors[i]);
            }
            return sb.ToString();
        }


        protected override bool CheckController(string userID, ref string error)
        {
            bool foundAtAll = false;
            foreach (string chan in Channels)
            {
                if (nicks.VerifyNick(chan, userID, ref error, ref foundAtAll)) return true;
            }
            foreach (string chan in OpChannels)
            {
                if (nicks.VerifyNick(chan, userID, ref error, ref foundAtAll)) return true;
            }

            if (!foundAtAll)
            {
                error = "You are not on the bot's list of known users for some reason, please leave and rejoin.";
            }
            return false;
        }

        void HookIRCEvents()
        {
            // Regster events for incoming
            conn.OnNick += OnNick;
            conn.OnRegistered += OnRegistered;
            conn.OnAction += OnAction;
            conn.OnPublic += OnPublic;
            conn.OnPrivate += OnPrivate;
            conn.OnError += OnError;
            conn.OnQuit += OnQuit;
            conn.OnJoin += OnJoin;
            conn.OnPart += OnPart;
            conn.OnChannelModeChange += OnChannelModeChange;
            conn.OnNames += OnNames;
            conn.OnKick += OnKick;
            conn.OnKill += OnKill;
            conn.OnPublicNotice += OnPublicNotice;
            conn.OnPrivateNotice += OnPrivateNotice;
            conn.OnPrivateAction += OnPrivateAction;
        }

        void UnhookIRCEvents()
        {
            // Regster events for incoming
            conn.OnNick -= OnNick;
            conn.OnRegistered -= OnRegistered;
            conn.OnAction -= OnAction;
            conn.OnPublic -= OnPublic;
            conn.OnPrivate -= OnPrivate;
            conn.OnError -= OnError;
            conn.OnQuit -= OnQuit;
            conn.OnJoin -= OnJoin;
            conn.OnPart -= OnPart;
            conn.OnChannelModeChange -= OnChannelModeChange;
            conn.OnNames -= OnNames;
            conn.OnKick -= OnKick;
            conn.OnKill -= OnKill;
            conn.OnPublicNotice -= OnPublicNotice;
            conn.OnPrivateNotice -= OnPrivateNotice;
            conn.OnPrivateAction -= OnPrivateAction;
        }


        void OnAction(string user, string channel, string description)
        {
            string nick = Connection.ExtractNick(user);
            MessageInGame(nick, string.Format("&I(IRC) * {0} {1}", nick, description));
        }

        void OnJoin(string user, string channel)
        {
            string nick = Connection.ExtractNick(user);
            conn.SendNames(channel);
            AnnounceJoinLeave(nick, "joined", channel);
        }

        void OnPart(string user, string channel, string reason)
        {
            string nick = Connection.ExtractNick(user);
            nicks.OnLeftChannel(nick, channel);

            if (nick == botNick) return;
            AnnounceJoinLeave(nick, "left", channel);
        }

        void AnnounceJoinLeave(string nick, string verb, string channel)
        {
            Logger.Log(LogType.RelayActivity, "{0} {1} channel {2}", nick, verb, channel);
            string which = OpChannels.CaselessContains(channel) ? " operator" : "";
            MessageInGame(nick, string.Format("&I(IRC) {0} {1} the{2} channel", nick, verb, which));
        }

        void OnQuit(string user, string reason)
        {
            string nick = Connection.ExtractNick(user);
            // Old bot was disconnected, try to reclaim it
            if (nick == botNick) conn.SendNick(botNick);
            nicks.OnLeft(nick);

            if (nick == botNick) return;
            Logger.Log(LogType.RelayActivity, nick + " left IRC");
            MessageInGame(nick, "&I(IRC) " + nick + " left");
        }

        void OnError(ReplyCode code, string message)
        {
            Logger.Log(LogType.RelayActivity, "IRC Error: " + message);
        }

        void OnPrivate(string user, string message)
        {
            string nick = Connection.ExtractNick(user);

            RelayUser rUser = new RelayUser();
            rUser.ID = nick;
            rUser.Nick = nick;
            HandleDirectMessage(rUser, nick, message);
        }

        void OnPublic(string user, string channel, string message)
        {
            string nick = Connection.ExtractNick(user);

            RelayUser rUser = new RelayUser();
            rUser.ID = nick;
            rUser.Nick = nick;
            HandleChannelMessage(rUser, channel, message);
        }

        void OnRegistered()
        {
            OnReady();
            Authenticate();
            JoinChannels();
        }

        void JoinChannels()
        {
            Logger.Log(LogType.RelayActivity, "Joining IRC channels...");
            foreach (string chan in Channels) { Join(chan); }
            foreach (string chan in OpChannels) { Join(chan); }
            ready = true;
        }

        void OnPublicNotice(string user, string channel, string notice)
        {
        }

        void OnPrivateNotice(string user, string notice)
        {
            if (!notice.CaselessStarts("You are now identified")) return;
            JoinChannels();
        }
        void OnPrivateAction(string user, string message)
        {
        }

        void Authenticate()
        {
            string nickServ = Server.Config.IRCNickServName;
            if (nickServ.Length == 0) return;

            if (Server.Config.IRCIdentify && Server.Config.IRCPassword.Length > 0)
            {
                Logger.Log(LogType.RelayActivity, "Identifying with " + nickServ);
                conn.SendMessage(nickServ, "IDENTIFY " + Server.Config.IRCPassword);
            }
        }

        void OnNick(string user, string newNick)
        {
            string nick = Connection.ExtractNick(user);
            // We have successfully reclaimed our nick, so try to sign in again.
            if (newNick == botNick) Authenticate();
            if (newNick.Trim().Length == 0) return;

            nicks.OnChangedNick(nick, newNick);
            MessageInGame(nick, "&I(IRC) " + nick + " &Sis now known as &I" + newNick);
        }

        void OnNames(string channel, string[] _nicks, bool last)
        {
            nicks.UpdateFor(channel, _nicks);
        }

        void OnChannelModeChange(string who, string channel)
        {
            conn.SendNames(channel);
        }

        void OnKick(string user, string channel, string kickee, string reason)
        {
            string nick = Connection.ExtractNick(user);
            nicks.OnLeftChannel(nick, channel);

            if (reason.Length > 0) reason = " (" + reason + ")";
            Logger.Log(LogType.RelayActivity, "{0} kicked {1} from IRC{2}", nick, kickee, reason);
            MessageInGame(nick, "&I(IRC) " + nick + " kicked " + kickee + reason);
        }

        void OnKill(string user, string killer, string reason)
        {
            string nick = Connection.ExtractNick(user);
            nicks.OnLeft(nick);
        }


        public const string BOLD = "\x02";
        public const string ITALIC = "\x1D";
        public const string UNDERLINE = "\x1F";
    }
    /// <summary> Manages a list of IRC nicks and asssociated permissions </summary>
    sealed class IRCNickList0
    {
        Dictionary<string, List<string>> userMap = new Dictionary<string, List<string>>();
        public IRCBot0 bot;

        public void Clear() { userMap.Clear(); }

        public void OnLeftChannel(string userNick, string channel)
        {
            List<string> chanNicks = GetNicks(channel);
            RemoveNick(userNick, chanNicks);
        }

        public void OnLeft(string userNick)
        {
            foreach (var chans in userMap)
            {
                RemoveNick(userNick, chans.Value);
            }
        }

        public void OnChangedNick(string userNick, string newNick)
        {
            foreach (var chans in userMap)
            {
                int index = GetNickIndex(userNick, chans.Value);
                if (index >= 0)
                {
                    string prefix = GetPrefix(chans.Value[index]);
                    chans.Value[index] = prefix + newNick;
                }
                else
                {
                    // should never happen, but just in case
                    bot.conn.SendNames(chans.Key);
                }
            }
        }

        public void UpdateFor(string channel, string[] nicks)
        {
            List<string> chanNicks = GetNicks(channel);
            foreach (string n in nicks)
                UpdateNick(n, chanNicks);
        }


        List<string> GetNicks(string channel)
        {
            foreach (var chan in userMap)
            {
                if (chan.Key.CaselessEq(channel)) return chan.Value;
            }

            List<string> nicks = new List<string>();
            userMap[channel] = nicks;
            return nicks;
        }

        void UpdateNick(string n, List<string> chanNicks)
        {
            string unprefixNick = Unprefix(n);
            for (int i = 0; i < chanNicks.Count; i++)
            {
                if (unprefixNick == Unprefix(chanNicks[i]))
                {
                    chanNicks[i] = n; return;
                }
            }
            chanNicks.Add(n);
        }

        void RemoveNick(string n, List<string> chanNicks)
        {
            int index = GetNickIndex(n, chanNicks);
            if (index >= 0) chanNicks.RemoveAt(index);
        }

        int GetNickIndex(string n, List<string> chanNicks)
        {
            if (chanNicks == null) return -1;
            string unprefixNick = Unprefix(n);

            for (int i = 0; i < chanNicks.Count; i++)
            {
                if (unprefixNick == Unprefix(chanNicks[i]))
                    return i;
            }
            return -1;
        }

        string Unprefix(string nick)
        {
            return nick.Substring(GetPrefixLength(nick));
        }

        string GetPrefix(string nick)
        {
            return nick.Substring(0, GetPrefixLength(nick));
        }

        int GetPrefixLength(string nick)
        {
            int prefixChars = 0;
            for (int i = 0; i < nick.Length; i++)
            {
                if (!IsNickChar(nick[i]))
                    prefixChars++;
                else
                    return prefixChars;
            }
            return prefixChars;
        }

        bool IsNickChar(char c)
        {
            return (c >= '0' && c <= '9') || (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') ||
                c == '[' || c == ']' || c == '{' || c == '}' || c == '^' || c == '`' || c == '_' || c == '|';
        }

        public bool VerifyNick(string channel, string userNick, ref string error, ref bool foundAtAll)
        {
            List<string> chanNicks = GetNicks(channel);
            if (chanNicks.Count == 0) return false;

            int index = GetNickIndex(userNick, chanNicks);
            if (index == -1) return false;
            foundAtAll = true;

            IRCControllerVerify verify = (IRCControllerVerify)Server.Config.IRCVerify;
            if (verify == IRCControllerVerify.None) return true;

            if (verify == IRCControllerVerify.HalfOp)
            {
                string prefix = GetPrefix(chanNicks[index]);
                if (prefix.Length == 0 || prefix == "+")
                { // + prefix is 'voiced user'
                    error = "You must be at least a half-op on the channel to use commands from IRC."; return false;
                }
                return true;
            }
            else
            {
                foreach (string chan in bot.OpChannels)
                {
                    chanNicks = GetNicks(chan);
                    if (chanNicks.Count == 0) continue;

                    index = GetNickIndex(userNick, chanNicks);
                    if (index != -1) return true;
                }
                error = "You must have joined the opchannel to use commands from IRC."; return false;
            }
        }
    }
    public sealed class IRCPlugin : Plugin_Simple
    {
    public override string Name { get { return name; } }
        public override string name { get { return "IRCRelay0"; } }

        public static IRCBot0 Bot = new IRCBot0();

        public override void Load(bool startup)
        {
        Command.Register(new CmdIRCBot0());
            Bot.ReloadConfig();
            Bot.Connect();
            OnConfigUpdatedEvent.Register(OnConfigUpdated, Priority.Low);
        }

        public override void Unload(bool shutdown)
        {
            OnConfigUpdatedEvent.Unregister(OnConfigUpdated);
            Bot.Disconnect("Disconnecting IRC bot");
           Command.Unregister(Command.Find("IRCBot0"));
        }

        void OnConfigUpdated() { Bot.ReloadConfig(); }
    }

    public sealed class CmdIRCBot0 : RelayBotCmd
    {
        public override string name { get { return "IRCBot0"; } }
        public override CommandAlias[] Aliases
        {
            get { return new[] { new CommandAlias("ResetBot0", "reset0"), new CommandAlias("ResetIRC0", "reset0") }; }
        }
        protected override RelayBot Bot { get { return IRCPlugin.Bot; } }
    }

    public sealed class CmdIrcControllers0 : BotControllersCmd
    {
        public override string name { get { return "IRCControllers0"; } }
        public override string shortcut { get { return "IRCCtrl0"; } }
        protected override RelayBot Bot { get { return IRCPlugin.Bot; } }
    }
}