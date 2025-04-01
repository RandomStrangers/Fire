//reference System.dll
using System;
using System.IO;
using System.Text.RegularExpressions;
namespace Flames.Commands
{
    public static class StringExtensions
    {
        public static string ReplaceFirst(this string str, string term, string replace)
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
    public class CmdWriteFile : Command
    {
        public override string name { get { return "WriteFile"; } }
        public override string shortcut { get { return "wf"; } }
        public override string type { get { return CommandTypes.Added; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Flames; } }
        public override void Use(Player p, string message)
        {
            string[] args;
            string path;
            string contents;
            bool Empty = string.IsNullOrEmpty(message);
            if (!Empty)
            {
                args = message.Split(':');
                path = args[0].ReplaceFirst(" ", "");
                contents = args[1].ReplaceFirst(" ", "");
            }
            else
            {
                Help(p);
                return;
            }
            if (string.IsNullOrEmpty(args[1]) || string.IsNullOrEmpty(args[0]))
            {
                Help(p);
                return;
            }
            try
            {
                if (File.Exists(path))
                {
                    File.WriteAllText(path, contents);
                    p.Message("Edited " + path);
                }
                else
                {
                    File.WriteAllText(path, contents);
                    p.Message("Created " + path);
                }
            }
            catch (Exception ex)
            {
                Logger.Log(LogType.Error, "Error writing to " + path + ".", ex);
                p.Message("Error writing to " + path + "!");
                p.Message("See errors log for more details.");
            }
        }
        public override void Help(Player p)
        {
            p.Message("&T/WriteFile [path] : [contents] &H- Makes [path] with [contents]");
            p.Message("&HThe [contents] is written to [path].");
            p.Message("&HIf [path] already exists, it will overwrite the existing contents.");
        }

        public string SanitizeFileName(string filename)
        {
            return Regex.Replace(filename, @"[^\d\w\-]", "");
        }
    }
}