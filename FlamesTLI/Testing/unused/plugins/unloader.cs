using System;
using Flames.Commands;
namespace Flames
{
	public class PluginUnloader  : Plugin
	{
		public override string name { get { return "Unloader"; } }
       // public override string welcome { get { return "Loaded Message!"; } }
		public override string creator { get { return "HarmonyNetwork"; } }
		public override void Load(bool startup)
		{
        Command.Register(new CmdUnloadPlugins());
		}
		public override void Unload(bool shutdown){
        Command.Unregister(Command.Find("UnloadPlugins"));
		}
		public override void Help(Player p)
		{
			p.Message("No help is available for this plugin.");
		}
public class CmdUnloadPlugins : Command
{
	public override string name { get { return "UnloadPlugins"; } }
	public override string shortcut { get { return "upa"; } }
	public override string type { get { return CommandTypes.Added; } }
	public override bool museumUsable { get { return true; } }
	public override LevelPermission defaultRank { get { return LevelPermission.Owner; } }
	public override void Use(Player p, string message) {
    Plugin.UnloadAll();
    p.Message("All plugins unloaded!");
	}

	public override void Help(Player p)
	{
		p.Message("/UnloadPlugins - Unloads all loaded plugins");
	}
}
	}
}