//reference System.dll
using System;
using System.IO;
using System.Text.RegularExpressions;
namespace Flames.Commands
{
    public class CmdRant : Command
    {
        public override string name { get { return "Rant"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return CommandTypes.Added; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Guest; } }
        public override void Use(Player p, string message)
        {
            bool Empty = string.IsNullOrEmpty(message);
            if (!Directory.Exists("Rant/"))
            {
                Directory.CreateDirectory("Rant");
            }
            if (Empty)
            {
                Help(p);
                return;
            }
            try
            {
                string filename = "Rant.txt";
                string path = "Rant/" + filename;
                string contents = message;
                if (File.Exists(path))
                {
                    contents = Environment.NewLine + contents;
                }
                File.AppendAllText(path, contents);
                p.Message("Added text to: " + filename);
            }
            catch 
            { 
                Help(p);
                return;
            }
        }
        public override void Help(Player p)
        {
            p.Message("&T/Rant [message] &H- Makes a file viewable by /ViewRant.");
            p.Message("&HThe [message] is entered into the text file.");
            p.Message("&HIf the file already exists, text will be added to the end.");
        }

        public string SanitizeFileName(string filename)
        {
            return Regex.Replace(filename, @"[^\d\w\-]", "");
        }
    }
    public class CmdViewRant : Command
    {
        public override string name { get { return "ViewRant"; } }
        public override string type { get { return CommandTypes.Added; } }
        public override string shortcut { get { return "VR"; } }
        public override bool UseableWhenFrozen { get { return true; } }
        public override void Use(Player p, string message)
        {
            if (!Directory.Exists("Rant/"))
            {
                Directory.CreateDirectory("Rant");
            }
            if (File.Exists("Rant/Rant.txt"))
            {
                string[] lines = File.ReadAllLines("Rant/Rant.txt");
                p.Message("Contents of the rant file:");
                p.MessageLines(lines);
            }
            else
            {
                p.Message("Rant file doesn't exist! Create it using /Rant!");
            }
        }
        public override void Help(Player p)
        {
            p.Message("&T/ViewRant &H- Views the contents of the /Rant file.");
        }
    }
}