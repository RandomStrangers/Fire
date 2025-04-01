using Flames.Network;
namespace Flames.Commands
{
    public class CmdCinematicMode : Command
    {
        public override string name { get { return "CinematicMode"; } }
        public override string type { get { return CommandTypes.Added; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public static bool Enabled = false;
        public override void Use(Player p, string message)
        {
            /*if (message.Length == 0)
            {
                Help(p);
                return;
            }*/

            //string[] args = message.SplitSpaces();

            //float apertureSize = 1f;
            //ushort.TryParse(apertureSize, out ushort NewApertureSize);
            ushort NewApertureSize = 1;
            if (Enabled)
            {
                Enabled = false;
                p.Send(Packet.SetCinematicGui(false, false, false, 255, 0, 0, 128, NewApertureSize));
            }
            else
            {
                Enabled = true;
                p.Send(Packet.SetCinematicGui(true, true, true, 255, 0, 0, 128, NewApertureSize));
            }
            p.Message("Toggled cinematic mode.");
        }

        public override void Help(Player p)
        {
            p.Message("&T/CinematicMode &H- Toggles cinematic mode.");
        }
    }
}