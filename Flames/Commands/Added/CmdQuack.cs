
using Flames.Commands.Chatting;

namespace Flames
{
    public class CmdQuack : MessageCmd
    {
        public override string name { get { return "Quack"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return CommandTypes.Added; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Guest; } }
        public override void Use(Player p, string message)
        {
            if (!CanSpeak(p, message))
            {
                return;
            }
            {
                Chat.MessageChat(ChatScope.Global, p, $"{p.color}{p.DisplayName}" + " &6QUACKED &elike a duck!", null, null);
            }
        }
        public override void Help(Player p)
        {
            p.Message("&T/Quack &H- Quack like a duck.");
        }
    }
}