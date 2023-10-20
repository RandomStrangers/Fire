using Flames.Modules.Relay.Discord;
namespace Flames.Commands.Chatting
{
    public sealed class CmdSayDiscord : Command2
    {
        public override string name { get { return "SayDiscord"; } }
        public override string shortcut { get { return "DiscordBroadcast"; } }
        public override string type { get { return CommandTypes.Chat; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public override CommandAlias[] Aliases { get { return new[] { new CommandAlias("sayd"), 
            new CommandAlias("dsay"), new CommandAlias("discordann") }; } }
        public override void Use(Player p, string message, CommandData data)
        {
            if (message.Length == 0) { Help(p); return; }

            message = Colors.Escape(message);
            DiscordPlugin.Bot.SendPublicMessage(message);
        }

        public override void Help(Player p)
        {
            p.Message("&T/SayDiscord [message]");
            p.Message("&HBroadcasts a message to the Discord server");
        }
    }
}