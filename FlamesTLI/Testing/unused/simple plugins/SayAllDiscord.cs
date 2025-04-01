//reference SNDiscord.dll
//reference NEDiscord.dll
//reference NMDiscord.dll
//reference RelayDiscord.dll
//reference GSDiscord.dll
//reference DNDiscord.dll
//reference SNDiscord1.dll
//reference SNDiscord2.dll
//reference DNDiscord1.dll
//reference DNDiscord2.dll
//reference GlobalSNDiscord.dll
//reference GlobalDNDiscord.dll
//reference GlobalNEDiscord.dll
//reference GlobalNMDiscord.dll
using System;
using Flames.Modules.Relay.Discord;
using Flames.Modules.Relay.SNDiscord;
using Flames.Modules.Relay.NEDiscord;
using Flames.Modules.Relay.NMDiscord;
using Flames.Modules.Relay.RelayDiscord;
using Flames.Modules.Relay.DNDiscord;
using Flames.Modules.Relay.SNDiscord1;
using Flames.Modules.Relay.DNDiscord1;
using Flames.Modules.Relay.SNDiscord2;
using Flames.Modules.Relay.DNDiscord2;
using Flames.Modules.Relay.GlobalSNDiscord;
using Flames.Modules.Relay.GlobalDNDiscord;
using Flames.Modules.Relay.GlobalNEDiscord;
using Flames.Modules.Relay.GlobalNMDiscord;
using Flames.Modules.Relay;
using Flames.Commands;
namespace Flames
{
	public class SayAllDiscordPlugin  : Plugin_Simple
	{
		public override string Name { get { return "SayAllDiscord"; } }
        public override string Flames_Version { get { return "1.0.0.0"; } }
		public override string creator { get { return "Harmony Network"; } }
		public override void Load(bool startup)
		{
        Command.Register(new CmdSayAllDiscord());
		}
		public override void Unload(bool shutdown){
        Command.Unregister(Command.Find("SayAllDiscord"));
		}
		public override void Help(Player p)
		{
			p.Message("No help is available for this plugin.");
		}
    public sealed class CmdSayAllDiscord : Command2
    {
        public override string name { get { return "SayAllDiscord"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return CommandTypes.Chat; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public override CommandAlias[] Aliases { get { return new[] { new CommandAlias("sayda"), 
            new CommandAlias("dasay"), new CommandAlias("discordaann") }; } }
        public override void Use(Player p, string message, CommandData data)
        {
            if (message.Length == 0) { Help(p); return; }

            //message = Colors.Escape(message);
            //Flames.Modules.Relay.SNDiscord.DiscordPlugin.Bot.SendPublicMessage(message);		    
            /*Flames.Modules.Relay.NEDiscord.DiscordPlugin.Bot.SendPublicMessage(message);*/
            Flames.Modules.Relay.NMDiscord.DiscordPlugin.Bot.SendPublicMessage(message);
            //Flames.Modules.Relay.RelayDiscord.DiscordPlugin.Bot.SendPublicMessage(message);
            Flames.Modules.Relay.GSDiscord.DiscordPlugin.Bot.SendPublicMessage(message);
            Flames.Modules.Relay.Discord.DiscordPlugin.Bot.SendPublicMessage(message);
            Flames.Modules.Relay.DNDiscord.DiscordPlugin.Bot.SendPublicMessage(message);		    
            Flames.Modules.Relay.DNDiscord1.DiscordPlugin.Bot.SendPublicMessage(message);
            Flames.Modules.Relay.DNDiscord2.DiscordPlugin.Bot.SendPublicMessage(message);
            Flames.Modules.Relay.SNDiscord1.DiscordPlugin.Bot.SendPublicMessage(message);
            Flames.Modules.Relay.SNDiscord2.DiscordPlugin.Bot.SendPublicMessage(message);
            Flames.Modules.Relay.GlobalSNDiscord.DiscordPlugin.Bot.SendPublicMessage(message);
            Flames.Modules.Relay.GlobalDNDiscord.DiscordPlugin.Bot.SendPublicMessage(message);
            Flames.Modules.Relay.GlobalNEDiscord.DiscordPlugin.Bot.SendPublicMessage(message);
            Flames.Modules.Relay.GlobalNMDiscord.DiscordPlugin.Bot.SendPublicMessage(message);

        }

        public override void Help(Player p)
        {
            p.Message("&T/SayAllDiscord [message]");
            p.Message("&HBroadcasts a message to Discord servers using all available relay bots.");
        }
    }
}
	}