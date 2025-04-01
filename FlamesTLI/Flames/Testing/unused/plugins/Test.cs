//	Auto-generated plugin skeleton class
//	Use this as a basis for custom Flames plugins
// To reference other assemblies, put a "//reference [assembly filename]" at the top of the file
//   e.g. to reference the System.Data assembly, put "//reference System.Data.dll"
// Add any other using statements you need after this
using System;
namespace Flames
{
	public class Test : Plugin
	{
		// The plugin's name (i.e what shows in /Plugins)
		public override string name { get { return "Test"; } }
		// The oldest version of Flames this plugin is compatible with
		public override string Flames_Version { get { return Server.Version; } }
		// Message displayed in server logs when this plugin is loaded
		public override string welcome { get { return "Loaded Message!"; } }
		// Who created/authored this plugin
		public override string creator { get { return "HarmonyNetwork"; } }
		// Called when this plugin is being loaded (e.g. on server startup)
		public override void Load(bool startup)
		{
			//code to hook into events, load state/resources etc goes here
		}
		// Called when this plugin is being unloaded (e.g. on server shutdown)
		public override void Unload(bool shutdown)
		{
			//code to unhook from events, dispose of state/resources etc goes here
		}
		// Displays help for or information about this plugin
		public override void Help(Player p)
		{
			p.Message("No help is available for this plugin.");
		}
	}
}