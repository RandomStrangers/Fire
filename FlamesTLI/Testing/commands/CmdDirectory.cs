namespace Flames.Commands
{

    public class CmdDirectory : Command
    {
        public override string name { get { return "Directory"; } }
        public override string shortcut { get { return "dir"; } }
        public override string type { get { return CommandTypes.Added; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Flames; } }
        public override void Use(Player p, string message)
        {
            string[] args = message.SplitSpaces(2);
            bool IsCreate = args[0].CaselessContains("create");
            bool IsDelete = args[0].CaselessContains("delete");
            bool IsRead = args[0].CaselessContains("read");
            if (args.Length < 2 && !IsRead)
            {
                Help(p);
                return;
            }
            else if (IsRead && args.Length == 1)
            {
                Find("ReadDirectory").Use(p, "");
                return;
            }
            else
            {
                if (IsCreate)
                {
                    Find("CreateDirectory").Use(p, args[1]);
                    return;
                }
                else if (IsDelete)
                {
                    Find("DeleteDirectory").Use(p, args[1]);
                    return;
                }
                else if (IsRead)
                {
                    Find("ReadDirectory").Use(p, args[1]);
                    return;
                }
                else
                {
                    Help(p);
                    return;
                }
            }
        }
        public override void Help(Player p)
        {
            p.Message("&T/Directory Create [directory] &H- Creates [directory] if it doesn\'t exist.");
            p.Message("&T/Directory Delete [directory] &H- Deletes [directory] if it exists.");
            p.Message("&T/Directory Read [directory] &H- Lists the files in [directory].");
        }
    }
}