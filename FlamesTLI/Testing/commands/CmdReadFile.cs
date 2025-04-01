using System.IO;
namespace Flames.Commands
{
    public class CmdReadFile : Command
    {
        public override string name { get { return "ReadFile"; } }
        public override string shortcut { get { return "read"; } }
        public override string type { get { return CommandTypes.Added; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Flames; } }
        public override bool SuperUseable { get { return true; } }
        public override CommandAlias[] Aliases { get { return new[] { new CommandAlias("rf") }; } }
        public override void Use(Player p, string message)
        {
            bool Empty = string.IsNullOrEmpty(message);
            if (Empty)
            {
                Help(p);
                return;
            }
            string file = message;
            if (!File.Exists(file))
            {
                p.Message("File " + message + " does not exist!");
                return;
            }
            string contents = File.ReadAllText(file);
            p.Message("Contents of  " + message + ":");
            p.Message("");
            p.Message(contents);
            return;
        }
        public override void Help(Player p)
        {
            p.Message("&T/ReadFile [file] &H- Prints the contents of [file] if it exists.");
        }
    }
}