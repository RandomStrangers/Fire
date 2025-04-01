//reference plugins/Discord.dll
namespace Flames.Commands
{
    public class CmdSayDiscordPlugin : Command
    {
        public override string name { get { return "SayDiscordPlugin"; } }
        public override string shortcut { get { return "DiscordBroadcastPlugin"; } }
        public override string type { get { return CommandTypes.Added; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public override CommandAlias[] Aliases { get { return new[] { new CommandAlias("saydPlugin"), 
            new CommandAlias("dsayPlugin"), new CommandAlias("discordannPlugin") }; } }
        public override void Use(Player p, string message)
        {
            if (message.Length == 0) 
            {
                Help(p); 
                return; 
            }

            message = Colors.Escape(message);
            DiscordDev.DiscordPlugin.Bot.SendPublicMessage(message);
        }

        public override void Help(Player p)
        {
            p.Message("&T/SayDiscordPlugin [message]");
            p.Message("&HBroadcasts a message to the DiscordPlugin server");
        }
    }
}