using System;
using Flames.Commands;
namespace Flames
{
	public class DisconnectOtherDiscordsPlugin  : Plugin_Simple
	{
		public override string Name { get { return "DisconnectOtherDiscords"; } }
        public override string Flames_Version { get { return "1.0.0.0"; } }
		public override string creator { get { return "Harmony Network"; } }
		public override void Load(bool startup)
		{
        Command.Register(new CmdDisconnectOtherDiscords());
		}
		public override void Unload(bool shutdown){
        Command.Unregister(Command.Find("DisconnectOtherDiscords"));
		}
		public override void Help(Player p)
		{
			p.Message("No help is available for this plugin.");
		}
    public sealed class CmdDisconnectOtherDiscords : Command2
    {
        public override string name { get { return "DisconnectOtherDiscords"; } }
        public override string shortcut { get { return "ddiscord"; } }
        public override string type { get { return CommandTypes.Chat; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public override void Use(Player p, string message, CommandData data)
        {
            Command.Find("SimplePlugin").Use(p, "unload " + "SNDiscord");
            Command.Find("SimplePlugin").Use(p, "unload " + "SNDiscord1");
            Command.Find("SimplePlugin").Use(p, "unload " + "SNDiscord2");
            Command.Find("SimplePlugin").Use(p, "unload " + "DNDiscord");
            Command.Find("SimplePlugin").Use(p, "unload " + "DNDiscord1");
            Command.Find("SimplePlugin").Use(p, "unload " + "DNDiscord2");
            Command.Find("SimplePlugin").Use(p, "unload " + "NEDiscord");
            Command.Find("SimplePlugin").Use(p, "unload " + "NMDiscord");
            Command.Find("SimplePlugin").Use(p, "unload " + "RelayDiscord");
            Command.Find("SimplePlugin").Use(p, "unload " + "GSDiscord");
            Command.Find("SimplePlugin").Use(p, "unload " + "GlobalSNDiscord");
            Command.Find("SimplePlugin").Use(p, "unload " + "GlobalDNDiscord");
            Command.Find("SimplePlugin").Use(p, "unload " + "GlobalNEDiscord");
            Command.Find("SimplePlugin").Use(p, "unload " + "GlobalNMDiscord");
        }

        public override void Help(Player p)
        {
            p.Message("&T/DisconnectOtherDiscords");
            p.Message("&HDisconnects the other relay bots from this server.");
        }
    }
}
	}