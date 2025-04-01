//reference plugins/ClassiCube.dll
using Flames.Events.ServerEvents;
using System;
using System.Collections.Generic;

namespace Flames.Network.HeartBeat
{
    public class HeartbeatSettingsPlugin : Plugin
    {
        public override string name { get { return "HeartbeatSettingsPlugin"; } }
        public override string creator { get { return "icanttellyou"; } }
        public override string Flames_Version { get { return Server.Version; } }
        public Command cbs = new CmdChangeBeatSetting();
        public Command vbs = new CmdViewBeatSettings();
        public Command hb = new CmdHeartbeat();

        public override void Load(bool auto)
        {
            Command.Register(cbs, vbs, hb);
        }
        public override void Unload(bool auto)
        {
            Command.Unregister(cbs, vbs, hb);
        }
        public static T GetValue<T>(Type type, string field)
        {
            object value = type.GetField(field).GetValue(null);
            return value != null ? (T)value : default(T);
        }
    }
    public class CmdChangeBeatSetting : Command
    {
        public override string name { get { return "ChangeBeatSetting"; } }
        public override string type { get { return CommandTypes.Added; } }
        public override string shortcut { get { return "cbs"; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Owner; } }
        public override void Use(Player p, string message)
        {
            string[] parts = message.SplitSpaces(2);
            if (parts.Length < 2) 
            { 
                Help(p); 
                return; 
            }
            string setting = parts[0];
            string value = parts[1];
            ConfigElement[] heartbeatConfig = HeartbeatSettingsPlugin.GetValue<ConfigElement[]>(typeof(HeartbeatConfig), "beatConfig");
            int elemI = Array.FindIndex(heartbeatConfig, e => e.Attrib.Name.CaselessEq(setting));
            if (elemI != -1)
            {
                ConfigElement elem = heartbeatConfig[elemI];
                value = value.Replace("%", "&");
                elem.Field.SetValue(ClassiCubePlugin.CCConfig, elem.Attrib.Parse(value));
				ClassiCubePlugin.CCConfig.Save(ClassiCubePlugin.CONFIG_FOLDER);
                OnConfigUpdatedEvent.Call();
                p.Message("Changed setting &T{0} &Sto &T{1}", setting, value);
                p.Message("&WYou may need to check the server logs if the setting changed properly!");
                Find("Server").Use(p, "reload");
                Find("heartbeat").Use(p, "");
            }
            else
            {
                p.Message("&W{0} is not a valid heartbeat setting!", setting);
            }
        }
        public override void Help(Player p)
        {
            p.Message("&T/ChangeBeatSetting [setting] [value]");
            p.Message("&HChanges values in heartbeat config.");
            p.Message("&WWarning: &HFeedback on values provided is only output to the logs due to limitations");
            p.Message("&HTo view all heartbeat properties use &T/ViewBeatSettings");
        }
    }
    public class CmdViewBeatSettings : Command
    {
        public override string name { get { return "ViewBeatSettings"; } }
        public override string type { get { return CommandTypes.Added; } }
        public override string shortcut { get { return "vbs"; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Owner; } }
        public override void Use(Player p, string message)
        {
            ConfigElement[] heartbeatConfig = HeartbeatSettingsPlugin.GetValue<ConfigElement[]>(typeof(HeartbeatConfig), "beatConfig");
            Dictionary<string, List<ConfigElement>> sections = new Dictionary<string, List<ConfigElement>>();
            foreach (ConfigElement elem in heartbeatConfig)
            {
                List<ConfigElement> members;
                if (!sections.TryGetValue(elem.Attrib.Section, out members))
                {
                    members = new List<ConfigElement>();
                    sections[elem.Attrib.Section] = members;
                }
                members.Add(elem);
            }
            foreach (var kvp in sections)
            {
                p.Message("{0} settings:", kvp.Key);
                foreach (ConfigElement elem in kvp.Value)
                {
                    p.Message("&T{0}&S: {1}", elem.Attrib.Name, elem.Attrib.Serialise(elem.Field.GetValue(ClassiCubePlugin.CCConfig)));
                }
            }
        }
        public override void Help(Player p)
        {
            p.Message("&T/ViewBeatSettings");
            p.Message("&HOutputs all server settings, based on category, and the values of the settings");
        }
    }
    public class CmdHeartbeat : Command
    {
        public override string name { get { return "heartbeat"; } }
        public override string shortcut { get { return "beat"; } }
        public override string type { get { return CommandTypes.Added; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Owner; } }

        public override void Use(Player p, string message)
        {
            try
            {
                Heartbeat.Heartbeats[0].Pump();
                Heartbeat.Heartbeats[1].Pump();
                p.Message("Heartbeat pumps sent.");
                p.Message("Server URL: " + ((ClassiCubeBeat)Heartbeat.Heartbeats[0]).LastResponse);
                p.Message("CC URL: " + ((ClassiCubeHeartbeat)Heartbeat.Heartbeats[1]).LastResponse);
            }
            catch (Exception e)
            {
                Logger.Log(LogType.Error, "Error with ClassiCube pump.", e);
                p.Message("Error with pumps: " + e + ".");
            }
        }
        public override void Help(Player p)
        {
            p.Message("&T/heartbeat &H- Forces a pump for the first two heartbeats.");
        }
    }
}