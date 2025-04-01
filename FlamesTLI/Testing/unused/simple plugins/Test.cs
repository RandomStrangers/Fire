//This is an example simple plugin source!
using System;
namespace Flames
{
	public class Test : Plugin_Simple
	{
		public override string name { get { return "Test"; } }
		public override string creator { get { return "Harmony Network"; } }
		public override void Load(bool startup)
		{
			//LOAD YOUR SIMPLE PLUGIN WITH EVENTS OR OTHER THINGS!
		}                        
		public override void Unload(bool shutdown)
		{
			//UNLOAD YOUR SIMPLE PLUGIN BY SAVING FILES OR DISPOSING OBJECTS!
		}                       
		public override void Help(Player p)
		{
			//HELP INFO!
		}
	}
}