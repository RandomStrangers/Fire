using Flames;
namespace Botnick
{
    public sealed class CmdBotnick : Command2
    {
        public override string name { get { return "Botnick"; } }
        public override string shortcut { get { return "Botname"; } }
        public override string type { get { return CommandTypes.Added; } }

        public override void Use(Player p, string message, CommandData data)
        {
            Find("Nick").Use(p, "bot " + message);
        }

        public override void Help(Player p)
        {
            p.Message("%T/Botnick [bot] [name] %H- Sets the name shown above that bot in game. If [name] is 'empty', the bot will not have a name shown. ");
        }
    }
}