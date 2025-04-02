using Flames.Added;
namespace Flames.Modules.Relay.DNDiscord
{
    public class DNDiscord : Addon
    {
        public override string Name { get { return "DNDiscord"; } }
        public override void Load()
        {
            Logger.Log(LogType.Debug, Name + " says hi.");
        }
        public override void Unload()
        {
            Logger.Log(LogType.Debug, Name + " says goodbye.");
        }
    }
}
