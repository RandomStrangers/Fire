using System;
using Flames.Commands;
namespace Flames
{
	public class PluginReloader  : Plugin
	{
		public override string name { get { return "Reloader"; } }
       // public override string welcome { get { return "Loaded Message!"; } }
		public override string creator { get { return "Harmony Network"; } }
		public override void Load(bool startup)
		{
        Command.Register(new CmdReloadPlugins());
		}
		public override void Unload(bool shutdown){
        Command.Unregister(Command.Find("ReloadPlugins"));
		}
		public override void Help(Player p)
		{
			p.Message("No help is available for this plugin.");
		}
public class CmdReloadPlugins : Command
{
	public override string name { get { return "ReloadPlugins"; } }
	public override string shortcut { get { return "rpa"; } }
	public override string type { get { return CommandTypes.Added; } }
	public override bool museumUsable { get { return true; } }
	public override LevelPermission defaultRank { get { return LevelPermission.Owner; } }
	public override void Use(Player p, string message) {
    Plugin.UnloadAll();
    p.Message("All plugins unloaded!");
    Plugin.LoadAll();
    p.Message("All plugins loaded!");
	}

	public override void Help(Player p)
	{
		p.Message("/ReloadPlugins - Unloads and reloads all plugins");
	}
}
	}
}