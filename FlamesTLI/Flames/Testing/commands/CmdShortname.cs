//reference System.dll
using System.Text.RegularExpressions;
using System.Collections.Generic;
namespace Flames.Commands
{
    public class CmdShortname : Command
    {
        public override string name { get { return "Shortname"; } }
        public override string shortcut { get { return "Shortnick"; } }
        public override string type { get { return CommandTypes.Added; } }
        public override void Use(Player p, string message)
        {
            string[] args = message.SplitSpaces(2);
            //you can set the 3 here to be any number. 
            uint minLength = 3;
            if (args[0] == "") 
            { 
                Find("Nick").Use(p, "-own"); 
                return; 
            }
            //edit text/ValidFlairs.txt to change the flairs you can use. Note that ValidFlairs.txt must be saved in UTF-8 encoding
            List<string> validList = Utils.ReadAllLinesList("text/ValidFlairs.txt");
            if (args.Length > 1)
            {
                string nick = args[1];
                string UncoloredNick = Regex.Replace(nick, @"%.", "");
                string flair = args[0];
                string UncoloredFlair = Regex.Replace(flair, @"%.", "");
                if (!validList.Contains(UncoloredFlair)) 
                { 
                    p.Message("&cYou are using an invalid flair."); 
                    return; 
                }

                if (p.name.CaselessContains(UncoloredNick))
                {
                    if (UncoloredNick.Length >= minLength)
                    {
                        Find("Nick").Use(p, "-own " + flair + " " + nick);
                    }
                    else 
                    { 
                        p.Message("&cYou need to have your name be " + minLength.ToString() + " characters or longer."); 
                    }
                }

                else 
                { 
                    p.Message("&cNew name must be part of original name."); 
                }
            }

            else
            {
                string nick = args[0];
                string UncoloredNick = Regex.Replace(nick, @"%.", "");
                if (p.name.CaselessContains(UncoloredNick))
                {

                    if (UncoloredNick.Length >= 3)
                    {
                        Find("Nick").Use(p, "-own " + nick);
                    }
                    else 
                    { 
                        p.Message("&cYou need to have your name be " + minLength.ToString() + " characters or longer."); 
                    }

                }
                else 
                { 
                    p.Message("&cNew name must be part of original name."); 
                }
            }
        }
        public override void Help(Player p)
        {
            p.Message("&T/Shortname <flair> [name]");
            p.Message("&HSets your nick to [name]. You can include a flair if you want.");
        }
    }
}
