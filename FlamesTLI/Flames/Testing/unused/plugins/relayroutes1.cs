//reference plugins/Discord.dll
using System;
using System.Collections.Generic;
using System.IO;
using Flames;
using Flames.Events.ServerEvents;
using Flames.Modules.Relay;
using Flames.Discord2;
using Flames.Modules.Relay.IRC;

namespace PluginRelayRoute
{
	public class RelayRoutePlugin : Plugin
	{
		public override string name { get { return "Relay Routes"; } }
		public List<Route> routes = new List<Route>();
		
		public class Route {
			public RelayBot srcBot, dstBot;
			public string srcChan, dstChan;
		}

		public override void Load(bool startup) {
			OnChannelMessageEvent.Register(OnMessage, Priority.Low);
			OnConfigUpdatedEvent.Register(OnConfigUpdated, Priority.Low);
			OnConfigUpdated();
		}
		
		public override void Unload(bool shutdown) {
			OnChannelMessageEvent.Unregister(OnMessage);
			OnConfigUpdatedEvent.Unregister(OnConfigUpdated);
		}
		
		public void OnMessage(RelayBot bot, string channel, RelayUser user, string message, ref bool cancel) {
			// ignore messages from relay bots themselves
			if (cancel || user.ID == bot.UserID) return;
			
			foreach (Route route in routes)
			{
				if (route.srcBot != bot) continue;
				if (route.srcChan != channel) continue;
				string msg = message;
				route.dstBot.SendMessage(route.dstChan, msg);
			}
		}
		
		public void OnConfigUpdated() { LoadRoutes(); }
		
		
		public const string ROUTES_FILE = "text/harmony_server2.txt";
		public static string[] default_routes = new string[] {
			"# This file contains a list of routes for relay bots",
			"# Each route should be on a separate line",
			"#    Note: Only messages sent by other users are routed",
			"#",
			"# Each route must use the following format: ",
			"#    [source service] [source channel] : [destination service] [destination channel]",
			"#",
			"# Some examples:",
			"# - Route from Discord channel 123456789 to IRC channel #test",
			"#    Discord 123456789 : IRC #test",
			"# - Route from Discord channel 123456789 to Discord channel 987654321",
			"#    Discord 123456789 : Discord 987654321",
			"# - Route from IRC channel #test to Discord channel 123456789",
			"#    IRC #test : Discord 123456789",
		};
		
		public void LoadRoutes() {
			if (!File.Exists(ROUTES_FILE))
				File.WriteAllLines(ROUTES_FILE, default_routes);
			
			string[] lines = File.ReadAllLines(ROUTES_FILE);
			List<Route> r  = new List<Route>();
			
			foreach (string line in lines)
			{
				if (line.IsCommentLine()) continue;
				try {
					r.Add(ParseRouteLine(line));
				} catch (Exception ex) {
					Logger.LogError("Error parsing route '" + line + "'", ex);
				}
			}
			routes = r;
		}
		
		public Route ParseRouteLine(string line) {
			string[] bits = line.Split(':');
			if (bits.Length != 2)
				throw new ArgumentException("Route requires exactly 1 separating :");
			
			Route r = new Route();
			ParseRouteNode(bits[0], out r.srcBot, out r.srcChan);
			ParseRouteNode(bits[1], out r.dstBot, out r.dstChan);
			return r;
		}
		
		public void ParseRouteNode(string part, out RelayBot bot, out string chan) {
			string[] bits = part.Trim().Split(' ');
			if (bits.Length != 2)
				throw new ArgumentException("A space is required between route service and channel");
			
			bot  = GetRouteBot(bits[0]);
			chan = bits[1];
		}
		
		public RelayBot GetRouteBot(string service) {
			if (service.CaselessEq("IRC")) return IRCPlugin.Bot;
			if (service.CaselessEq("Discord")) return DiscordPlugin.Bot;			
			throw new ArgumentException("Unknown service '" + service + "'");
		}
	}
}