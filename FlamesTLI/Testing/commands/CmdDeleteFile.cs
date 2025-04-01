using System.IO;
namespace Flames.Commands
{
    public class CmdDeleteFile : Command
    {
        public override string name { get { return "DeleteFile"; } }
        public override string shortcut { get { return "df"; } }
        public override string type { get { return CommandTypes.Added; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Flames; } }
        public override CommandAlias[] Aliases { get { return new[] { new CommandAlias("FileDelete") }; } }
        public override void Use(Player p, string message)
        {
            bool messageEmpty = string.IsNullOrEmpty(message);
            if (!messageEmpty)
            {
                if (File.Exists(message))
                {
                    AtomicIO.TryDelete(message);
                    p.Message(message + " deleted.");
                    return;
                }
                if (!File.Exists(message))
                {
                    p.Message(message + " not found!");
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
            p.Message("&T/DeleteFile [file] &H- Deletes [file] if it exists.");
        }
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