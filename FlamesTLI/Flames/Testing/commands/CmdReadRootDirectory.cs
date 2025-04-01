using System.IO;
namespace Flames.Commands
{
    public class CmdReadRootDirectory : Command
    {
        public override string name { get { return "ReadRootDirectory"; } }
        public override string shortcut { get { return "readrootdir"; } }
        public override string type { get { return CommandTypes.Added; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Flames; } }
        public override bool SuperUseable { get { return true; } }
        public override CommandAlias[] Aliases { get { return new[] { new CommandAlias("readr") }; } }
        public override void Use(Player p, string message)
        {
            bool Empty = string.IsNullOrEmpty(message);
            string directory;
            string path = Directory.GetDirectoryRoot(Directory.GetCurrentDirectory()).Replace("/home/container", "root/");
            if (Empty)
            {
                directory = Directory.GetDirectoryRoot(Directory.GetCurrentDirectory());
            }
            else
            {
                directory = Directory.GetDirectoryRoot(Directory.GetCurrentDirectory()) + "/" + message;

                path = directory.Replace("/home/container/", "root/");
            }
            if (!Directory.Exists(directory))
            {
                p.Message("Directory " + path + " does not exist!");
                return;
            }
            else
            {
                p.Message("Contents of " + path + ":");
                foreach (string file in Directory.GetFiles(directory))
                {
                    p.Message(file.Replace(directory + "/", ""));
                }
                foreach (string dir in Directory.GetDirectories(directory))
                {
                    p.Message(dir.Replace(directory + "/", "") + "/");

                }
            }
        }
        public override void Help(Player p)
        {
            p.Message("&T/ReadRootDirectory [directory] &H- Lists the files in [directory] starting from the root directory.");
        }
    }
}