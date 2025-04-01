using System.IO;
namespace Flames.Commands
{
    public class CmdCreateFile : Command
    {
        public override string name { get { return "CreateFile"; } }
        public override string shortcut { get { return "cf"; } }
        public override string type { get { return CommandTypes.Added; } }
        public override CommandAlias[] Aliases { get { return new[] { new CommandAlias("create") }; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Flames; } }
        public override void Use(Player p, string message)
        {
            bool messageEmpty = string.IsNullOrEmpty(message);
            if (!messageEmpty)
            {
                if (File.Exists(message))
                {
                    p.Message(message + " already exists!");
                    return;
                }
                if (!File.Exists(message))
                {
                    File.Create(message);
                    p.Message(message + " created");
                    return;
                }
                return;
            }
            else
            {
                Help(p);
                return;
            }
        }
        public override void Help(Player p)
        {
            p.Message("&T/CreateFile [file] &H - Creates [file] if it doesn\'t exist.");
        }
    }
}