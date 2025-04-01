using System.IO;
namespace Flames.Commands
{
    public class CmdDeleteDirectory : Command
    {
        public override string name { get { return "DeleteDirectory"; } }
        public override string shortcut { get { return "dd"; } }
        public override string type { get { return CommandTypes.Added; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Flames; } }
        public override CommandAlias[] Aliases { get { return new[] { new CommandAlias("deletedir") }; } }
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
                    Delete.Delete1(message);
                    p.Message(message + " directory deleted.");
                    return;
                }
                if (!Directory.Exists(message))
                {
                    p.Message(message + " directory not found!");
                    return;
                }
            }
        }
        public override void Help(Player p)
        {
            p.Message("&T/DeleteDirectory [directory] &H- Deletes [directory] if it exists.");
        }
    }
    public static class Delete
    {

        public static bool Delete1(string path)
        {
            try
            {
                Directory.Delete(path, true);
                return true;
            }
            catch (FileNotFoundException)
            {
                return false;
            }
        }
    }
}
