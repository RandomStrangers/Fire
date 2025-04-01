using System.IO;
namespace Flames
{
    public class CommandsCompiler : Plugin
    {
        public override string name { get { return "CommandsCompiler"; } }
        public override string Flames_Version { get { return Server.Version; } }
        public override string creator { get { return "HarmonyNetwork"; } }
        public override void Load(bool startup)
        {
            Command.Register(new CmdCompileAll());
        }
        public override void Unload(bool shutdown)
        {
            Command.Unregister(Command.Find("CompileAll"));
        }
        public override void Help(Player p)
        {
            p.Message("");
        }
    }

    public class CmdCompileAll : Command
    {
        public override string name { get { return "CompileAll"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return CommandTypes.Added; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Flames; } }
        public override void Use(Player p, string message)
        {
            CompileCommands2();
        }

        public override void Help(Player p)
        {
            p.Message("&T/CompileAll &H- Compiles all commands in the commands source folder.");
        }
        public static string path = Directory.GetCurrentDirectory() + "/extra/commands/source";
        public static string FileName = "";
        public static void CompileCommands2()
        {
            string[] files = AtomicIO.TryGetFiles(path, "*.cs");

            if (files != null)
            {
                foreach (string file in files)
                {
                    FileName = file;
                    FileName = FileName.Replace("Cmd", "");
                    FileName = FileName.Replace(".cs", "");
                    FileName = FileName.Replace(path, "");
                    FileName = FileName.Replace("/", "");
                    Find("Compile").Use(Player.Flame, FileName);
                    Find("CmdLoad").Use(Player.Flame, FileName);
                }
            }
            else
            {
            }
        }
    }
}