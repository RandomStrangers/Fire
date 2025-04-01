//reference plugins/Discord.dll
namespace Flames.Commands
{
    public class CmdSayDiscord2 : Command
    {
        public override string name { get { return "SayDiscord2"; } }
        public override string shortcut { get { return "DiscordBroadcast2"; } }
        public override string type { get { return CommandTypes.Added; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public override CommandAlias[] Aliases
        {
            get
            {
                return new[] { new CommandAlias("sayd2"),
            new CommandAlias("dsay2"), new CommandAlias("discordann2") };
            }
        }
        public override void Use(Player p, string message)
        {
            if (message.Length == 0) 
            { 
                Help(p);
                return;
            }

            message = Colors.Escape(message);
            Discord2.DiscordPlugin.Bot.SendPublicMessage(message);
        }

        public override void Help(Player p)
        {
            p.Message("&T/SayDiscord2 [message]");
            p.Message("&HBroadcasts a message to the Discord2 server");
        }
    }
}