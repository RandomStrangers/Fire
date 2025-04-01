using System;
using System.IO;
using System.Threading;
namespace Flames.Commands
{
    public static class StringExt
    {
        public static string ReplaceFirst(ref string str, string term, string replace)
        {
            int position = str.IndexOf(term);
            if (position < 0)
            {
                return str;
            }
            str = str.Substring(0, position) + replace + str.Substring(position + term.Length);
            return str;
        }
    }
    public class CmdFile : Command
    {
        public override string name { get { return "File"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return CommandTypes.Added; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Flames; } }

        public override void Use(Player p, string message)
        {
            string[] args = message.SplitSpaces(2);
            bool IsCreate = args[0].CaselessContains("create");
            bool IsDelete = args[0].CaselessContains("delete");
            bool IsRead = args[0].CaselessContains("read");
            bool IsWrite = args[0].CaselessContains("write");
            if (args.Length < 2)
            {
                Help(p);
                return;
            }
            else
            {
                if (IsCreate)
                {
                    Find("CreateFile").Use(p, args[1]);
                    return;
                }
                else if (IsDelete)
                {
                    Find("DeleteFile").Use(p, args[1]);
                    return;
                }
                else if (IsRead)
                {
                    Find("ReadFile").Use(p, args[1]);
                    return;
                }
                else if (IsWrite)
                {

                    string WriteArgs = message.ReplaceFirst("Write ", "");
                    WriteArgs = WriteArgs.ReplaceFirst("write ", "");
                    Find("WriteFile").Use(p, WriteArgs);
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
            p.Message("&T/File Create [file] &H- Creates [file] if it doesn\'t exist.");
            p.Message("&T/File Delete [file] &H- Deletes [file] if it exists.");
            p.Message("&T/File Read [file] &H- Prints the contents of [file] if it exists.");
            p.Message("&T/File Write [path] : [contents] &H- Writes [contents] to [path].");
            p.Message("&HIf [path] already exists, it will overwrite the existing contents.");
        }
    }
}