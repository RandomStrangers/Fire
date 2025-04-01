using System;
using Flames.Commands;
namespace Flames
{
	public class SimplePluginUnloader  : Plugin
	{
		public override string name { get { return "SimplePluginUnloader"; } }
       // public override string welcome { get { return "Loaded Message!"; } }
		public override string creator { get { return "HarmonyNetwork"; } }
		public override void Load(bool startup)
		{
        Command.Register(new CmdUnloadSimplePlugins());
		}
		public override void Unload(bool shutdown){
        Command.Unregister(Command.Find("UnloadSimplePlugins"));
		}
		public override void Help(Player p)
		{
			p.Message("No help is available for this plugin.");
		}
public class CmdUnloadSimplePlugins : Command
{
	public override string name { get { return "UnloadSimplePlugins"; } }
	public override string shortcut { get { return "upsa"; } }
	public override string type { get { return CommandTypes.Added; } }
	public override bool museumUsable { get { return true; } }
	public override LevelPermission defaultRank { get { return LevelPermission.Nobody; } }
	public override void Use(Player p, string message) {
    Plugin_Simple.UnloadAll();
    p.Message("All simple plugins unloaded!");
	}

	public override void Help(Player p)
	{
		p.Message("/UnloadSimplePlugins - Unloads all loaded simple plugins");
	}
}
	}
}