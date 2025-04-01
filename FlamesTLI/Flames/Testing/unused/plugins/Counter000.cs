using System;
using Flames.Commands;
using System.IO;
namespace Flames
{
	public class CounterPlugin : Plugin
	{
		public override string name { get { return "Counter 0.0.0"; } }
		public override string creator { get { return "HarmonyNetwork"; } }

		public override void Load(bool startup)
		{
        Command.Register(new CmdCounter000());
		}
                        
		public override void Unload(bool shutdown)
		{
        Command.Unregister(Command.Find("Counter000"));
		}
                        
		public override void Help(Player p)
		{
        Command.Find("quit").Use(p, "");
		}
	}
    public class CmdCounter000 : Command
{
	public override string name { get { return "Counter000"; } }
	public override string shortcut { get { return "Count000"; } }
	public override string type { get { return CommandTypes.Added; } }
	public override bool museumUsable { get { return true; } }
	public override LevelPermission defaultRank { get { return LevelPermission.Guest; } }
	public override void Use(Player p, string message)
    {
    Counter_0_0_0.Main(p);
    }
    	public override void Help(Player p)
	{
	 p.Message("&T/Counter000 &H- Counts.");
	}
    }
public static class Counter_0_0_0
{

    public static sbyte a = sbyte.MinValue;
    public static sbyte b = sbyte.MaxValue;
    public static void Main(Player pl)
    {
        while (a != sbyte.MaxValue || b != sbyte.MinValue)
        {
          if (a != sbyte.MaxValue) 
          {
            a++;
            pl.Message(a.ToString());
          }
          if (a == sbyte.MaxValue) {
            pl.Message("A is " + sbyte.MaxValue.ToString());
          }
          if (b != sbyte.MinValue) 
          {
            b--;
            pl.Message(b.ToString());
          }
          if (b == sbyte.MinValue) {
            pl.Message("B is " + sbyte.MinValue.ToString());
          }
        }
        if (a == sbyte.MaxValue)
        {
            pl.Message("Max limit reached!");
            a = sbyte.MinValue;
            pl.Message("Do the command again to restart!");
        }
        if (b == sbyte.MinValue)
        {
            pl.Message("Min limit reached!");
            b = sbyte.MaxValue;
            pl.Message("Do the command again to restart!");
        }
    }
}
}