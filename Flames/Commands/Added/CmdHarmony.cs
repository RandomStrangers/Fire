using Flames.Modules.Relay.Discord;
namespace Flames.Commands.Chatting
{
    public sealed class CmdHarmony : Command2
    {
        public override string name { get { return "Harmony"; } }
        public override string shortcut { get { return "ServerLogo"; } }
        public override string type { get { return CommandTypes.Chat; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public override CommandAlias[] Aliases { get { return new[] { new CommandAlias("Logo"), new CommandAlias("HarmonyLogo"), new CommandAlias("HarmonyServerLogo") }; } }
        public override void Use(Player p, string message, CommandData data)
        {
            //string imgorigin = "http://r.weavesilk.com/?v=4&id=bwf42w5obku";
            string img = "https://files.catbox.moe/6uiix1.png";
            DiscordPlugin.Bot.SendPublicMessage(img);
        }

        public override void Help(Player p)
        {
            p.Message("&T/Harmony &H- Sends the server logo to the Discord server");
        }
    }
}