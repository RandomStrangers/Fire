using System.IO;
namespace Flames
{
    public sealed class RelayLogPlugin : Plugin
    {
        public override string creator { get { return "HarmonyNetwork"; } }
        public override string name { get { return "RelayLogPlugin"; } }

        public override void Load(bool startup)
        {
            HookLogger();
        }

        public override void Unload(bool shutdown)
        {
            Logger.LogHandler -= LogMessage;
        }

        public static void HookLogger()
        {
            Logger.LogHandler += LogMessage;
        }
        public static void LogMessage(LogType type, string msg)
        {
            if (type != LogType.RelayChat)
            {
                return;
            }
            else
            {
                File.AppendAllText("discordlog.txt", msg + "\n");
            }
        }
    }
}