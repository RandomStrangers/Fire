using System;
using Flames.Util;
using Flames;
namespace Flame.Commands
{
    public class CmdDiscordEat : Command
    {
        public override string name { get { return "DiscordEat"; } }
        public override string type { get { return CommandTypes.Added; } }

        public override void Use(Player p, string message)
        {
            if (p.muted) 
            { 
                return; 
            }
            else
            {
                OnPurchase2(p, message);
            }
        }
        public override void Help(Player p)
        {
            p.Message("&T/DiscordEat &H- Eats a random snack. Only works for Discord.");
        }
        public void OnPurchase2(Player p, string args)
        {
            if (!p.IsSuper)
            {
                Help(p);
                p.cancelcommand = true;
                return;
            }
            if (DateTime.UtcNow < p.NextEat)
            {
                p.Message("You're still full - you need to wait at least 10 seconds between snacks.");
                p.cancelcommand = true;
                return;
            }
            TextFile discordeatFile = TextFile.Files["Eat"];
            discordeatFile.EnsureExists();
            string[] actions = discordeatFile.GetText();
            string action = "ate some food";
            if (actions.Length > 0)
            {
                action = actions[new Random().Next(actions.Length)];
            }
            p.NextEat = DateTime.UtcNow.AddSeconds(10);
            Find("Say").Use(Player.Flame, p.SuperName + " &S" + action);
        }
    }
}