using System.IO;
namespace Flames.Commands
{
    public class CmdCreateDir : Command
    {
        public override string name { get { return "CreateDirectory"; } }
        public override string shortcut { get { return "cdir"; } }
        public override string type { get { return CommandTypes.Added; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Flames; } }
        public override CommandAlias[] Aliases { get { return new[] { new CommandAlias("createdir") }; } }
        public override void Use(Player p, string message)
        {
            bool messageEmpty = string.IsNullOrEmpty(message);
            if (messageEmpty)
            {
                Help(p);
                return;
            }
            else
            {
                if (Directory.Exists(message))
                {
                    p.Message(message + " directory already exists!.");
                    return;
                }
                if (!Directory.Exists(message))
                {
                    Directory.CreateDirectory(message);
                    p.Message(message + " directory created!");
                    return;
                }
                else
                {
                    Help(p);
                }
            }
        }
        public override void Help(Player p)
        {
            p.Message("&T/CreateDirectory [directory] &H- Creates [directory] if it doesn\'t exist.");
        }
    }
}