//reference plugins/Discord.dll
using System;
using System.Collections.Generic;
using System.IO;
using Flames;
using Flames.Events.ServerEvents;
using Flames.Modules.Relay;
using Flames.Modules.Relay.Discord;
using Flames.Discord2;


namespace PluginRelayRoute
{
    public class RelayRoutePlugin : Plugin
    {
        public override string name { get { return "Relay Routes2"; } }
        public List<Route> routes = new List<Route>();

        public class Route
        {
            public RelayBot srcBot, dstBot;
            public string srcChan, dstChan;
        }
    public override void Load(bool startup)
        {
            OnChannelMessageEvent.Register(OnMessage, Priority.Low);
            OnConfigUpdatedEvent.Register(OnConfigUpdated, Priority.Low);
            OnConfigUpdated();
        }

        public override void Unload(bool shutdown)
        {
            OnChannelMessageEvent.Unregister(OnMessage);
            OnConfigUpdatedEvent.Unregister(OnConfigUpdated);
        }
    public void OnMessage(RelayBot bot, string channel, RelayUser user, string message, ref bool cancel)
        {
            // ignore messages from relay bots themselves
            if (cancel || user.ID == bot.UserID) return;
            //bool ContainsSlash = message.CaselessContains("/");
           //if (ContainsSlash) return;
            bool IsProxy = message.CaselessStarts("rsp/")
                        || message.CaselessStarts("rs/")
                        || message.CaselessStarts("a/")
                        || message.CaselessStarts("al/")
                        || message.CaselessStarts("pd/")
                        || message.CaselessStarts("lu/")
                        || message.CaselessStarts("l/")
                        || message.CaselessStarts("hn/")
                        || message.CaselessStarts("sn/")
                        || message.CaselessStarts("dn/")
                        || message.CaselessStarts("m/")
                        || message.CaselessStarts("om/")
                        || message.CaselessStarts("dbf/")
                        || message.CaselessStarts("bf/")
                        || message.CaselessStarts("gs/")
                        || message.CaselessStarts("cf/")
                        || message.CaselessStarts("jb/")
                        || message.CaselessStarts("js/")
                        || message.CaselessStarts("as/")
                        || message.CaselessStarts("s/")
                        || message.CaselessStarts("k/")
                        || message.CaselessStarts("pk/")
                        || message.CaselessStarts("rs5/")
                        || message.CaselessStarts("sd/")
                        || message.CaselessStarts("sz/")
                        || message.CaselessStarts("dz/");
            if (IsProxy) return;

            foreach (Route route in routes)
            {
                if (route.srcBot != bot) continue;
                if (route.srcChan != channel) continue;
                string msg = "**" + user.Nick + "**" + ": " + message;
                route.dstBot.SendMessage(route.dstChan, msg);
            }
        }

        public void OnConfigUpdated() { LoadRoutes(); }


        public const string ROUTES_FILE = "text/harmony_server2.txt";
        public static string[] default_routes = new string[] {
            "# This file contains a list of routes for Discord relay bots",
            "# Each route should be on a separate line",
            "#    Note: Only messages sent by other users are routed",
            "#",
            "# Each route must use the following format: ",
            "#    Discord [source channel] : Discord [destination channel]",
            "#",
            "# An example:",
            "# - Route from Discord channel 123456789 to Discord channel 987654321",
            "#    Discord 123456789 : Discord 987654321",
        };

        public void LoadRoutes()
        {
            if (!File.Exists(ROUTES_FILE))
                File.WriteAllLines(ROUTES_FILE, default_routes);

            string[] lines = File.ReadAllLines(ROUTES_FILE);
            List<Route> r = new List<Route>();

            foreach (string line in lines)
            {
                if (line.IsCommentLine()) continue;
                try
                {
                    r.Add(ParseRouteLine(line));
                }
                catch (Exception ex)
                {
                    Logger.LogError("Error parsing route '" + line + "'", ex);
                }
            }
            routes = r;
        }

        public Route ParseRouteLine(string line)
        {
            string[] bits = line.Split(':');
            if (bits.Length != 2)
                throw new ArgumentException("Route requires exactly 1 separating :");

            Route r = new Route();
            ParseRouteNode(bits[0], out r.srcChan);
            ParseRouteNode(bits[1], out r.dstChan);
            return r;
        }

        public void ParseRouteNode(string part, out string chan)
        {
            string[] bits = part.Trim().Split(' ');
            if (bits.Length != 1)
                throw new ArgumentException("No spaces!");
            chan = bits[0];
        }
    }
}