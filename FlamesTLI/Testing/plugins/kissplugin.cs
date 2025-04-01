//reference plugins/Discord.dll
using Flames.Discord2;

namespace Flames
{
    public class KissPlugin : Plugin
    {
        public override string name { get { return "Kiss"; } }

        public const string EXTRA_KEY = "__Kiss_Name";


        public override void Load(bool startup)
        {
            Command.Register(new CmdAccept());
            Command.Register(new CmdDeny());
            Command.Register(new CmdKiss());
        }

        public override void Unload(bool shutdown)
        {
            Command.Unregister(Command.Find("Accept"));
            Command.Unregister(Command.Find("Deny"));
            Command.Unregister(Command.Find("Kiss"));
        }


        public static Player CheckProposal(Player p)
        {
            string name = p.Extras.GetString(EXTRA_KEY);
            if (name == null)
            {
                p.Message("You do not have a pending kissing proposal.");
                return null;
            }

            Player src = PlayerInfo.FindExact(name);
            if (src == null)
            {
                p.Message("The person who proposed to kiss you isn't online.");
                return null;
            }
            return src;
        }
    }

    public class CmdAccept : Command
    {
        public override string name { get { return "Accept"; } }
        public override string type { get { return CommandTypes.Added; } }

        public override void Use(Player p, string message)
        {
            Player proposer = KissPlugin.CheckProposal(p);
            if (proposer == null) return;
            p.Message("&bYou &aaccepted &b{0}&b's proposal", p.FormatNick(proposer));
            DiscordPlugin.Bot.SendPublicMessage($"{proposer.color}{proposer.DisplayName}&S gave {p.color}{p.DisplayName}&S a kiss on the lips! O&c///&SO");
            Chat.MessageFrom(p, $"{proposer.color}{proposer.DisplayName}&S gave {p.color}{p.DisplayName}&S a kiss on the lips! O&c///&SO");
        }

        public override void Help(Player p)
        {
            p.Message("&T/Accept &H- Accepts a pending kissing proposal.");
        }
    }

    public class CmdDeny : Command
    {
        public override string name { get { return "Deny"; } }
        public override string type { get { return CommandTypes.Added; } }

        public override void Use(Player p, string message)
        {
            Player proposer = KissPlugin.CheckProposal(p);
            if (proposer == null) return;
            Chat.MessageFrom(p, $"{p.color}{p.DisplayName}&Sdenied {proposer.color}{proposer.DisplayName}&S's request.");

            p.Message("&bYou &cdenied &b{0}&b's proposal", p.FormatNick(proposer));
        }

        public override void Help(Player p)
        {
            p.Message("&T/Deny &H- Denies a pending kiss proposal.");
        }
    }

    public class CmdKiss : Command
    {
        public override string name { get { return "Kiss"; } }
        public override string type { get { return CommandTypes.Added; } }

        public override void Use(Player p, string message)
        {

            Player partner = PlayerInfo.FindMatches(p, message);
            if (partner == null) return;
            partner.Extras[KissPlugin.EXTRA_KEY] = p.name;
            //DiscordPlugin.Bot.SendPublicMessage($"{p.color}{p.DisplayName}&S is asking {partner.color}{partner.DisplayName}&S for permission to kiss on the lips! O&c///&SO");
            partner.Message($"{p.color}{p.DisplayName}&S is asking {partner.color}{partner.DisplayName}&S for permission to kiss on the lips! O&c///&SO");
            partner.Message("&bTo accept their proposal type &a/Accept");
            partner.Message("&bOr to deny it, type &c/Deny");
        }

        public override void Help(Player p)
        {
            p.Message("&T/Kiss [player] &H- Asks the given player permission to kiss them on the lips O&c///&HO.");
        }
    }
}