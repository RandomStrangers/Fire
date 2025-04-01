using System;
using Flames;
using Flames.Events;
using Flames.Events.PlayerEvents;

public class DevOnly : Plugin 
{
	public override string creator { get { return "HarmonyNetwork"; } } 
	public override string Flames_Version { get { return Server.Version; } }
	public override string name { get { return "DevOnlyPlugin"; } }
	
	public override void Load(bool startup) {
		OnPlayerFinishConnectingEvent.Register(KickPlayer, Priority.High);
	}
	
	public override void Unload(bool shutdown) {
		OnPlayerFinishConnectingEvent.Unregister(KickPlayer);
	}
    public override void Help(Player p)
    {
    	if (PlayerInfo.IsDev(p)) 
        {
    		p.Message("&TDevOnlyPlugin &H- Kicks players who aren't a developer of {0}.", Server.SoftwareName);
        }
    }
	public void KickPlayer(Player p) {
		string clientName = p.Session.ClientName();
		if (!PlayerInfo.IsDev(p)) {
		
		p.Leave("Error contacting web server: 403", true);
		p.cancelconnecting = true;
        }
	}
}