/*
    Copyright 2010 MCSharp team (Modified for use with MCZall/MCLawl/MCForge)
    
    Dual-licensed under the Educational Community License, Version 2.0 and
    the GNU General Public License, Version 3 (the "Licenses"); you may
    not use this file except in compliance with the Licenses. You may
    obtain a copy of the Licenses at
    
    https://opensource.org/license/ecl-2-0/
    https://www.gnu.org/licenses/gpl-3.0.html
    
    Unless required by applicable law or agreed to in writing,
    software distributed under the Licenses are distributed on an "AS IS"
    BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
    or implied. See the Licenses for the specific language governing
    permissions and limitations under the Licenses.
*/
using System;
using System.Collections.Generic;
using Flames.NewScripting;
using Flames.Modules.NewCompiling;
using Flames.Tasks;
using Flames.Events.ServerEvents;
using Flames.Modules.Games.Countdown;
using Flames.Modules.Games.CTF;
using Flames.Modules.Games.LS;
using Flames.Modules.Games.TW;
using Flames.Modules.Games.ZS;
using Flames.Modules.Moderation.Notes;
using Flames.Modules.Relay.Discord;
using Flames.Modules.Relay.IRC;
using Flames.Core;
using Flames.Modules.Security;
using Flames.Events.PlayerEvents;
using Flames.Commands;
using Flames.DB;
using Flames.Blocks;
using System.Text.RegularExpressions;
using Flames.Added;
namespace Flames.Core
{
    public class NewPluginLoader : NewPlugin
    {
        public override string name { get { return "NewPluginLoader"; } }
        public override string creator { get { return Colors.Strip(Server.SoftwareName + " team"); } }
        public static void LoadAllNewPlugins(SchedulerTask task)
        {
            LoadAll();
        }
        public override void Load(bool startup)
        {
            OnShuttingDownEvent.Register(OnShutdown, Priority.Critical);
            Server.Critical.QueueOnce(LoadAllNewPlugins);
        }
        public void OnShutdown(bool restarting, string message)
        {
            UnloadAll();
        }
        public override void Unload(bool shutdown)
        {
            OnShuttingDownEvent.Unregister(OnShutdown);
        }
        public override void Help(Player p)
        {
            p.Message("");
        }
    }
}
namespace Flames
{
    /// <summary> This class provides for more advanced modification to Flames 
    /// using MCGalaxy's newer compiler plugin </summary>
    public abstract class NewPlugin
    {
        /// <summary> Hooks into events and initalises states/resources etc </summary>
        /// <param name="auto"> True if new plugin is being automatically loaded (e.g. on server startup), false if manually. </param>
        public abstract void Load(bool auto);

        /// <summary> Unhooks from events and disposes of state/resources etc </summary>
        /// <param name="auto"> True if new plugin is being auto unloaded (e.g. on server shutdown), false if manually. </param>
        public abstract void Unload(bool auto);

        /// <summary> Called when a player does /Help on the new plugin. Typically tells the player what this new plugin is about. </summary>
        /// <param name="p"> Player who is doing /Help. </param>
        public virtual void Help(Player p)
        {
            p.Message("No help is available for this new plugin.");
        }

        /// <summary> Name of the new plugin. </summary>
        public abstract string name { get; }
        /// <summary> The oldest version of Flames this new plugin is compatible with. </summary>
        public virtual string Flames_Version { get { return "9.0.4.8"; } }
        /// <summary> Work on backwards compatibility with MCGalaxy </summary>
        public virtual string MCGalaxy_Version { get { return null; } }
#if CORE
        /// <summary> Work on backwards compatibility with other cores </summary>
        public virtual string GoldenSparks_Version { get { return null; } }
        /// <summary> Work on backwards compatibility with other cores </summary>
        public virtual string SuperNova_Version { get { return null; } }
        /// <summary> Work on backwards compatibility with other cores </summary>
        public virtual string DeadNova_Version { get { return null; } }

        /// <summary> Work on backwards compatibility with other cores </summary>
        public virtual string RandomStrangers_Version { get { return null; } }
#endif
        /// <summary> Version of this new plugin. </summary>
        public virtual int build { get { return 0; } }
        /// <summary> Message to display once this new plugin is loaded. </summary>
        public virtual string welcome { get { return ""; } }
        /// <summary> The creator/author of this new plugin. (Your name) </summary>
        public virtual string creator { get { return ""; } }
        /// <summary> Whether or not to auto load this new plugin on server startup. </summary>
        public virtual bool LoadAtStartup { get { return true; } }


        /// <summary> List of new plugins/modules included in the server software </summary>
        public static List<NewPlugin> CoreNewPlugins = new List<NewPlugin>();
        public static List<NewPlugin> CustomNewPlugins = new List<NewPlugin>();


        public static NewPlugin FindNewCustom(string name)
        {
            foreach (NewPlugin pl in CustomNewPlugins)
            {
                if (pl.name.CaselessEq(name)) return pl;
            }
            return null;
        }

        public static void Load(NewPlugin pl, bool auto)
        {
            string ver = pl.Flames_Version;
            string MCGalaxy_Ver = "1.9.4.9";
            // Version different in Dev build, use normal for new plugins
            string CurrentVersion = Server.FlamesVersion;

            if (!string.IsNullOrEmpty(pl.MCGalaxy_Version) && new Version(pl.MCGalaxy_Version) > new Version(MCGalaxy_Ver))
            {
                string msg = string.Format("New plugin '{0}' cannot be loaded on this version of {1}!", pl.name, Server.SoftwareName);
                throw new InvalidOperationException(msg);
            }
            if (!string.IsNullOrEmpty(ver) && new Version(ver) > new Version(CurrentVersion) && new Version(ver) > new Version(Server.Version))
            {
                string msg = string.Format("New plugin '{0}' requires a more recent version of {1}!", pl.name, Server.SoftwareName);
                throw new InvalidOperationException(msg);
            }

            try
            {
                CustomNewPlugins.Add(pl);

                if (pl.LoadAtStartup || !auto)
                {
                    pl.Load(auto);
                    Logger.Log(LogType.SystemActivity, "New plugin {0} loaded...build: {1}", pl.name, pl.build);
                }
                else
                {
                    Logger.Log(LogType.SystemActivity, "New plugin {0} was not loaded, you can load it with /npload", pl.name);
                }

                if (!string.IsNullOrEmpty(pl.welcome)) Logger.Log(LogType.SystemActivity, pl.welcome);
            }
            catch
            {
                if (!string.IsNullOrEmpty(pl.creator)) Logger.Log(LogType.Warning, "You can go bug {0} about {1} failing to load.", pl.creator, pl.name);
                throw;
            }
        }
        public static bool Unload(NewPlugin pl)
        {
            bool success = UnloadNewPlugin(pl, false);

            // TODO only remove if successful?
            CustomNewPlugins.Remove(pl);
            CoreNewPlugins.Remove(pl);
            return success;
        }

        public static bool UnloadNewPlugin(NewPlugin pl, bool auto)
        {
            try
            {
                pl.Unload(auto);
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError("Error unloading new plugin " + pl.name, ex);
                return false;
            }
        }
        public static void UnloadAll()
        {
            for (int i = 0; i < CustomNewPlugins.Count; i++)
            {
                UnloadNewPlugin(CustomNewPlugins[i], true);
            }
            CustomNewPlugins.Clear();

            for (int i = 0; i < CoreNewPlugins.Count; i++)
            {
                UnloadNewPlugin(CoreNewPlugins[i], true);
            }
        }
        public static void LoadAll()
        {
            LoadCoreNewPlugin(new CorePlugin());
            LoadCoreNewPlugin(new NotesPlugin());
            LoadCoreNewPlugin(new DiscordPlugin());
            LoadCoreNewPlugin(new IRCPlugin());
            LoadCoreNewPlugin(new IPThrottler());
            LoadCoreNewPlugin(new ServerURLSender());
            LoadCoreNewPlugin(new CountdownPlugin());
            LoadCoreNewPlugin(new CTFPlugin());
            LoadCoreNewPlugin(new LSPlugin());
            LoadCoreNewPlugin(new TWPlugin());
            LoadCoreNewPlugin(new ZSPlugin());
            LoadCoreNewPlugin(new NewCompilerPlugin());
            LoadCoreNewPlugin(new AddonLoader());
            LoadNewPlugin(new Commands_Plugins());
#if CORE
            LoadCoreNewPlugin(new GoldenSparksPluginLoader());
            LoadCoreNewPlugin(new SuperNovaPluginLoader());
            LoadCoreNewPlugin(new DeadNovaPluginLoader());
            LoadCoreNewPlugin(new RandomStrangersPluginLoader());
#endif
            IScripting.AutoloadNewPlugins();
        }
        public static void LoadCoreNewPlugin(NewPlugin newplugin)
        {
            List<string> disabled = Server.Config.DisabledModules;
            if (disabled.CaselessContains(newplugin.name)) return;
            newplugin.Load(true);
            CoreNewPlugins.Add(newplugin);
        }
        public static void LoadNewPlugin(NewPlugin newplugin)
        {
            List<string> disabled = Server.Config.DisabledModules;
            if (disabled.CaselessContains(newplugin.name)) return;
            newplugin.Load(true);
            CustomNewPlugins.Add(newplugin);
        }
    }
}
namespace Flames
{
    public class Commands_Plugins : NewPlugin
    {
        public override string name { get { return "Commands&Plugins"; } }
        public override string creator { get { return "HarmonyNetwork"; } }
        public static NewPlugin NewDrawCollabPlugin = new DrawCollabPlugin();
        public static NewPlugin NewMakePlugin = new Make();
        public static NewPlugin NewNoAFKPlugin = new NoAFK();
        public override void Load(bool startup)
        {
            LoadNewPlugin(NewDrawCollabPlugin);
            LoadNewPlugin(NewMakePlugin);
            LoadNewPlugin(NewNoAFKPlugin);
            Command.Register(new CmdCollabBan());
            Command.Register(new CmdSuper());
            Command.Register(new CmdMapHack());
            Command.Register(new CmdAdventure());
            Command.Register(new CmdOsGo());
            Command.Register(new CmdShortname());
        }
        public override void Unload(bool shutdown)
        {
            UnloadNewPlugin(NewDrawCollabPlugin);
            UnloadNewPlugin(NewMakePlugin);
            UnloadNewPlugin(NewNoAFKPlugin);
            Command.Unregister(Command.Find("CollabBan"));
            Command.Unregister(Command.Find("Super"));
            Command.Unregister(Command.Find("MapHack"));
            Command.Unregister(Command.Find("Adventure"));
            Command.Unregister(Command.Find("OsGo"));
            Command.Unregister(Command.Find("Shortname"));
        }
        public override void Help(Player p)
        {
            p.Message("");
        }
        public static void LoadNewPlugin(NewPlugin newplugin)
        {
            List<string> disabled = Server.Config.DisabledModules;
            if (disabled.CaselessContains(newplugin.name)) return;
            newplugin.Load(true);
            CustomNewPlugins.Add(newplugin);
        }
        public static bool UnloadNewPlugin(NewPlugin pl, bool auto)
        {
            try
            {
                pl.Unload(auto);
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError("Error unloading new plugin " + pl.name, ex);
                return false;
            }
        }
        public static bool UnloadNewPlugin(NewPlugin pl)
        {
            bool success = UnloadNewPlugin(pl, false);

            // TODO only remove if successful?
            CustomNewPlugins.Remove(pl);
            CoreNewPlugins.Remove(pl);
            return success;
        }
    }
	public class CmdCollabBan : Command2 
    {
		public override string name { get { return "CollabBan"; } }
        public override string shortcut { get { return "dcban"; } }
		public override string type { get { return CommandTypes.Added; } }
		public override void Use(Player p, string message, CommandData data) 
        {
            string[] args = message.SplitSpaces(2);
            if (args.Length < 2) 
            { 
                Help(p); 
                return; 
            }
        	string reason = args.Length < 2 ? "" :  "&c" + args[1];
        	string target = PlayerInfo.FindMatchesPreferOnline(p, args[0]);
  		    Find("warn").Use(p, target + " &fBlacklisted from the collab map for: " + reason);
		    Find("send").Use(p, target + " &fYou have been blacklisted from the collab map for: " + reason);
            Find("perbuild").Use(p, "collab " + "-" + target);
            Find("UndoPlayer").Use(p, target + " all", data);
        }
		
       public override void Help(Player p) 
       {
            p.Message("&T/CollabBan [player] [reason]");
           	p.Message("&HWarns, blacklists, and undoes a player's actions, and sends a message to said players inbox.");
       }
	}
    public class CmdSuper : Command
    {
        public override string name { get { return "Super"; } }
        public override bool MessageBlockRestricted { get { return true; } }
        public override string type { get { return CommandTypes.Added; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Admin; } }
        public override string shortcut { get { return ""; } }
        public override bool UpdatesLastCmd { get { return false; } }
        public override bool ShowCommandInfo { get { return false; } }

        public override void Use(Player p, string message)
        {
            Execute(p);
        }
        public void Execute(Player pl)
        {
            if (!pl.IsSuper)
            {
                pl.IsSuper = true;
                pl.Message("&cWarning: &5You can no longer be kicked and will remain connected until you use this command again!");
                pl.SuperName = pl.truename;
            }
            else
            {
                pl.IsSuper = false;
                pl.Message("&cWarning: &5You will be disconnected upon closing the client now.");
            }
        }
        public override void Help(Player p)
        {
            if (p.IsFire)
            {
                p.Message("Does nothing.");
            }
            else if (p.IsSuper)
            {
                p.Message("Does nothing.");
            }
            else
            {
                string msg = PlayerDB.GetLogoutMessage(p.name);
                p.Leave(msg);
            }
        }
    }
	public class CmdMapHack : Command2 
    {
		
		public override string name { get { return "MapHack"; } }
		public override string type { get { return CommandTypes.Added; } }
		public override CommandPerm[] ExtraPerms {
			get { return new[] { new CommandPerm(LevelPermission.Operator, "can bypass hack restrictions on all maps") }; }
		}
		
		public static bool hooked;
		public const string ext_allowed_key = "__MAPHACK_ALLOWED";
		
		public void EnableHacksBypass(Player p) 
        {
			p.Extras[ext_allowed_key] = true;
			p.SendMapMotd();
			p.Message("&aYou are now bypassing hacks restrictions on this map");
		}
		
		public void DisableHacksBypass(Player p) 
        {
			p.Extras[ext_allowed_key] = false;
			p.SendMapMotd();
			p.Message("&HHacks bypassing reset, use &T/MapHack &Hto turn on again");
		}
		
		public override void Use(Player p, string message, CommandData data) 
        {
			if (message.Length == 0) 
            {
				if (!hooked) 
                { // not thread-safe but meh
					OnSentMapEvent.Register(HandleOnSentMap, Priority.High);
					OnGettingMotdEvent.Register(HandleGettingMotd, Priority.High);
					hooked = true;
				}

				if (LevelInfo.IsRealmOwner(p.name, p.level.MapName) || CheckExtraPerm(p, data, 1)) 
                {
					EnableHacksBypass(p);
				}
                else 
                {
					p.Message("&cYou can only bypass hacks on your own realms.");
				}
			} 
            else if (message.CaselessEq("stop")) 
            {
				DisableHacksBypass(p);
			} 
            else 
            {
				Help(p);
			}
		}

		public void HandleOnSentMap(Player p, Level prevLevel, Level level) 
        {
			if (!p.Extras.GetBoolean(ext_allowed_key)) return;
			// disable /maphack when you reload or change maps
			DisableHacksBypass(p);
		}
		
		public void HandleGettingMotd(Player p, ref string motd) 
        {
			if (!p.Extras.GetBoolean(ext_allowed_key)) return;
			motd = "+hax";
		}
		
		public override void Help(Player p) 
        {
			p.Message("&T/MapHack &H- Lets you bypass hacks restrictions on your own map (e.g. for when making a parkour map with -hax on)");
			p.Message("&T/MapHack stop &H- Enables hacks restrictions again");
		}
	}
    public class CmdAdventure : Command
    {   
        public override string name { get { return "Adventure"; } }
        public override string shortcut { get { return "ad"; } }
        public override string type { get { return CommandTypes.Added; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Guest; } }
        public override void Use(Player p, string message)
        {
            if (p.group.Permission >= LevelPermission.Operator) 
            {
                Command.Find("map").Use(p, "buildable");
                Command.Find("map").Use(p, "deletable");
                return;
            }
            
            if (!LevelInfo.IsRealmOwner(p.name, p.level.name)) 
            { 
                p.Message("&cYou do not have permission to use /Adventure in this map."); 
                return;
            }
            
            Command.Find("overseer").Use(p, "map buildable");
            Command.Find("overseer").Use(p, "map deletable");
        }
        public override void Help(Player p) 
        {
            p.Message("&T/Adventure");
            p.Message("&HInstantly toggles buildable and deletable, to turn your map into an \"adventure\" map with unbreakable blocks.");
        }
    }
    public class CmdOsGo : Command
    {
        public override string name { get { return "OsGo"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return CommandTypes.Added; } }
        public override bool SuperUseable { get { return false; } }
        public override bool ShowCommandInfo { get { return false; } }

        public override void Use(Player p, string message)
        {
            if (!LevelInfo.MapExists(p.truename))
            {
                p.Message("You do not have a realm, try creating one using /os map add!");
                return;
            }
            else
            {
                Find("Overseer").Use(p, "go");
                return;
            }
        }

        public override void Help(Player p)
        {
            return;
        }
    }
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
                Command.Find("Nick").Use(p, "-own"); 
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
                if (!(validList.Contains(UncoloredFlair))) 
                {
                    p.Message("&cYou are using an invalid flair."); 
                    return;
                }
                if ((p.name).CaselessContains(UncoloredNick))  
                {
                    if (UncoloredNick.Length >= minLength) 
                    {
                        Command.Find("Nick").Use(p, "-own " + flair + " " + nick);
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
                if ((p.name).CaselessContains(UncoloredNick))  
                {

                    if (UncoloredNick.Length >= 3)
                    {
                        Command.Find("Nick").Use(p, "-own " + nick);
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
namespace Flames
{
    public class DrawCollabPlugin : NewPlugin
    {
        public override string name { get { return "DrawCollabPlugin"; } }

        public override void Load(bool startup)
        {
            Command.Register(new CmdDrawCollab());
            OnPlayerCommandEvent.Register(HandleCommand, Priority.Critical);
            OnGettingMotdEvent.Register(HandleMotd, Priority.Critical);
        }

        public override void Unload(bool shutdown)
        {
            Command.Unregister(Command.Find("DrawCollab"));
            OnPlayerCommandEvent.Unregister(HandleCommand);
            OnGettingMotdEvent.Unregister(HandleMotd);

        }
        public static void HandleCommand(Player p, string cmd, string args, CommandData data)
        {
            Command command = Command.Find(cmd);
            if (command != null && p != null)
            {
                Level lvl = p.level;
                bool IsPaintCmd = command.name.CaselessContains("paint");

                if (IsPaintCmd && lvl.Config.MOTD.CaselessContains("+collab"))
                {
                    p.Message("Cannot disable painting mode on this map.");
                    p.cancelcommand = true;
                }
            }
        }
       public void HandleMotd(Player p, ref string motd)
        {
            Level lvl = p.level;
            if (lvl.Config.MOTD.CaselessContains("+collab"))
            { 
                p.painting = true;
                p.Message("Painting mode: &aON&S, use /paint to disable.");            
            }
        }
    }
    public class CmdDrawCollab : Command
    {
        public override string name { get { return "DrawCollab"; } }
        public override string shortcut { get { return "DC"; } }
        public override string type { get { return CommandTypes.Added; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Owner; } }
        public override bool MessageBlockRestricted { get { return true; } }
        public override bool UseableWhenFrozen { get { return true; } }
        public override void Use(Player p, string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                Help(p);
                return;
            }
            Level lvl;
            string[] args = message.SplitSpaces();
            if (args.Length == 1)
            {
                lvl = p.Level;
            }
            else
            {
                lvl = Matcher.FindLevels(p, args[1]);
            }
            if (p.IsSuper) 
            { 
                SuperRequiresArgs(p, "level name"); 
                return; 
                }
            string config = args[0];
            if (config.CaselessEq("true"))
            {
                if (lvl.Config.MOTD == "ignore" || string.IsNullOrEmpty(lvl.Config.MOTD))
                {
                    lvl.Config.MOTD = "+collab";

                }
                else 
                {
                    lvl.Config.MOTD += "+collab";
                }
                p.Message("Added +collab to MOTD.");
            }
            else if (config.CaselessEq("false"))
            {
                if (lvl.Config.MOTD.CaselessContains("+collab"))
                {
                    lvl.Config.MOTD.Replace("+collab", "");
                }
            }
            else
            {
                Help(p);
            }
        }
        public override void Help(Player p)
        {
            p.Message("&T/DrawCollab true [level] &H- Appends +collab to the provided level's MOTD.");
            p.Message("&T/DrawCollab false [level] &H- Removes +collab from the provided level's MOTD.");
            p.Message("If no level is provided, uses current level.");
        }
    }
}

namespace Flames 
{
    public class Make : NewPlugin 
    {
        public override string name { get { return "Make"; } }
        public override string creator { get { return "Goodly"; } }
        public override string Flames_Version { get { return Server.Version; } }
        
        public static Command makeCommand = new CmdMake();
        public static Command makeGBCommand = new CmdMakeGB();
        public override void Load(bool startup) 
        {
            CmdMakeBase.Load();
            
            Command.Register(makeCommand);
            Command.Register(makeGBCommand);
        }
        public override void Unload(bool shutdown) 
        {
            Command.Unregister(makeCommand);
            Command.Unregister(makeGBCommand);
        }
    }
    
    public abstract class CmdMakeBase : Command2 
    {
        
        public delegate void MakeType(CmdMakeBase maker, Player p, int origin, string name, ushort requestedSlot);
        public static string TYPES = "";
        public static Dictionary<string, MakeType> makeActions = new Dictionary<string, MakeType>();
        
        public static object locker = new object();
        
        public static void Load() 
        {
            makeActions["slabs"] = MakeSlabs;
            makeActions["walls"] = MakeWalls;
            makeActions["stairs"] = MakeStairs;
            makeActions["flatstairs"] = MakeFlatStairs;
            makeActions["corners"] = MakeCorners;
            makeActions["eighths"] = MakeEighths;
            makeActions["panes"] = MakePanes;
            makeActions["poles"] = MakePoles;
            
            TYPES = string.Join(", ", makeActions.Keys);
        }
        
        public static string GetBlockName(Player p, ushort blockID) 
        {
            blockID = Block.FromRaw(blockID); //convert from clientside to server side ID
            
            if (Block.IsPhysicsType(blockID)) return "Physics block";
            
            BlockDefinition def = null;
            if (!p.IsSuper) 
            {
                def = p.level.GetBlockDef(blockID);
            } 
            else 
            {
                def = BlockDefinition.GlobalDefs[blockID];
            }
            if (def == null && blockID > 0 && blockID < Block.CPE_COUNT) 
            {
                p.Message("Making default");
                def = DefaultSet.MakeCustomBlock(blockID);
                p.Message("def.Name is {0}", def.Name);
            }
            
            if (def != null) 
            { 
                return def.Name; 
                }
            
            return "Unknown";
        }
        
        public abstract bool CanUse(Player p);
        public abstract Command BlockDefCommand { get; }
        public abstract int Dir { get; }
        public abstract int DefaultRequestedSlot { get; }
        public abstract ushort IterateTo { get; }
        public abstract BlockDefinition[] GetBlockDefs(Player p);
        public abstract string BlockPropsScope { get; }
        
        public Command _defCmd;
        public Command defCmd { get { return _defCmd; } set { _defCmd = value; } }
        public override void Use(Player p, string message, CommandData data) 
        {
            if (!CanUse(p)) 
            {
                return;
            }
            
            if (message == "") 
            { 
                Help(p); 
                return; 
            }
            string[] args = message.SplitSpaces(3);
            if (args.Length < 2) 
            { 
                Help(p); 
                return; 
            }
            
            string type = args[0].ToLower();
            ushort sourceID;
            if (!CommandParser.GetBlock(p, args[1], out sourceID)) 
            { 
                return; 
            }
            sourceID = Block.Convert(sourceID); // convert physics blocks to their visual form
            sourceID = Block.ToRaw(sourceID);   // convert server block IDs to client block IDs
            
            string name = GetBlockName(p, (ushort)sourceID);
            int requestedSlot = DefaultRequestedSlot;
            if (requestedSlot == -1) 
            {
                p.Message("&WThere are no unused custom block ids left.");
                return;
            }
            if (args.Length > 2) 
            {
                if (!CommandParser.GetInt(p, args[2], "starting ID", ref requestedSlot, Block.CPE_COUNT, Block.MaxRaw)) 
                { 
                    return; 
                } 
            }
            
            //p.Message("Requested slot is {0}", requestedSlot);
            //p.Message("IterateTo: {0}", IterateTo);
            
            if (makeActions.ContainsKey(type)) 
            {
                lock (locker) 
                {
                    defCmd = BlockDefCommand; //assign to temporary variable to avoid Command.Find spam
                    makeActions[type](this, p, sourceID, name, (ushort)requestedSlot);
                }
            } 
            else 
            {
                p.Message("&WInvalid type \"{0}\".", type);
                p.Message("Types can be: &b{0}", TYPES);
            }
        }
        
        public bool Iterator(ref ushort b) 
        {
            if (b > IterateTo) 
            { 
                b--; 
                return true; 
            }
            else if (b < IterateTo) 
            { 
                b++; 
                return true; 
            }
            //reached goal stop iterating
            return false;
        }
        public bool AreEnoughBlockIDsFree(Player p, int amountRequested, out int startID, ushort requestedSlot) 
        {
            startID = 0;
            int amountFound = 0;
            
            BlockDefinition[] defs = GetBlockDefs(p);
            
            ushort b = requestedSlot;
            do 
            {
                ushort cur = Block.FromRaw(b);
                if (defs[cur] == null) 
                {
                    if (startID == 0) 
                    {
                        startID = b;
                    }
                    amountFound++;
                    if (amountFound == amountRequested) 
                    {
                        return true;
                    }
                    continue;
                }
                
                if (startID != 0) 
                {
                    //if a starting point was already found and we run into a used slot before finding the amount requested, stop
                    break;
                }
            } while (Iterator(ref b));
            
            p.Message("&WThere are not enough free sequential block IDs to perform this command.");
            string start = (startID != 0) ? " starting at " + startID : "";
            p.Message("(found {0} free IDs{1}, but needed {2})", amountFound, start, amountRequested);
            return false;
        }
        
        public static void MakePanes(CmdMakeBase maker, Player p, int origin, string name, ushort requestedSlot) 
        {
            int dest;
            if (!maker.AreEnoughBlockIDsFree(p, 2, out dest, requestedSlot)) 
            { 
                return; 
            }
            
            //WE  (0, 0, 6) to (16, 16, 10)
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-WE");
            maker.defCmd.Use(p, "edit " + dest + " min 0 0 6");
            maker.defCmd.Use(p, "edit " + dest + " max 16 16 10");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
            dest += maker.Dir;
            //NS (6, 0, 0) to (10, 16, 16)
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-NS");
            maker.defCmd.Use(p, "edit " + dest + " min 6 0 0");
            maker.defCmd.Use(p, "edit " + dest + " max 10 16 16");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
        }
        public static void MakeSlabs(CmdMakeBase maker, Player p, int origin, string name, ushort requestedSlot) 
        {
            int dest;
            if (!maker.AreEnoughBlockIDsFree(p, 2, out dest, requestedSlot)) 
            { 
                return; 
            }
            
            //down
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-D");
            maker.defCmd.Use(p, "edit " + dest + " min 0 0 0");
            maker.defCmd.Use(p, "edit " + dest + " max 16 8 16");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 1");
            Command.Find("blockproperties").Use(p, maker.BlockPropsScope+" "+(dest)+" stackblock "+origin);
            dest += maker.Dir;
            //up
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-U");
            maker.defCmd.Use(p, "edit " + dest + " min 0 8 0");
            maker.defCmd.Use(p, "edit " + dest + " max 16 16 16");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 1");
            
        }
        public static void MakeWalls(CmdMakeBase maker, Player p, int origin, string name, ushort requestedSlot) 
        {
            int dest;
            if (!maker.AreEnoughBlockIDsFree(p, 4, out dest, requestedSlot)) 
            { 
                return; 
            }
            
            int height = 16;
            //north
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-N");
            maker.defCmd.Use(p, "edit " + dest + " min 0 0 0");
            maker.defCmd.Use(p, "edit " + dest + " max 16 "+height+" 8");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
            dest += maker.Dir;
            //south
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-S");
            maker.defCmd.Use(p, "edit " + dest + " min 0 0 8");
            maker.defCmd.Use(p, "edit " + dest + " max 16 "+height+" 16");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
            dest += maker.Dir;
            //west
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-W");
            maker.defCmd.Use(p, "edit " + dest + " min 0 0 0");
            maker.defCmd.Use(p, "edit " + dest + " max 8 "+height+" 16");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
            dest += maker.Dir;
            //east
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-E");
            maker.defCmd.Use(p, "edit " + dest + " min 8 0 0");
            maker.defCmd.Use(p, "edit " + dest + " max 16 "+height+" 16");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
        }
        public static void MakeStairs(CmdMakeBase maker, Player p, int origin, string name, ushort requestedSlot) 
        {
            int dest;
            if (!maker.AreEnoughBlockIDsFree(p, 8, out dest, requestedSlot)) 
            { 
                return; 
            }
            
            
            int height = 8;
            //north
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-D-N");
            maker.defCmd.Use(p, "edit " + dest + " min 0 0 0");
            maker.defCmd.Use(p, "edit " + dest + " max 16 "+height+" 8");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
            dest += maker.Dir;
            //south
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-D-S");
            maker.defCmd.Use(p, "edit " + dest + " min 0 0 8");
            maker.defCmd.Use(p, "edit " + dest + " max 16 "+height+" 16");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
            dest += maker.Dir;
            //west
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-D-W");
            maker.defCmd.Use(p, "edit " + dest + " min 0 0 0");
            maker.defCmd.Use(p, "edit " + dest + " max 8 "+height+" 16");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
            dest += maker.Dir;
            //east
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-D-E");
            maker.defCmd.Use(p, "edit " + dest + " min 8 0 0");
            maker.defCmd.Use(p, "edit " + dest + " max 16 "+height+" 16");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
            dest += maker.Dir;
            
            //--------------------------------------------------------------------------------upper
            height = 16;
            
            //north
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-U-N");
            maker.defCmd.Use(p, "edit " + dest + " min 0 8 0");
            maker.defCmd.Use(p, "edit " + dest + " max 16 "+height+" 8");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
            dest += maker.Dir;
            //south
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-U-S");
            maker.defCmd.Use(p, "edit " + dest + " min 0 8 8");
            maker.defCmd.Use(p, "edit " + dest + " max 16 "+height+" 16");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
            dest += maker.Dir;
            //west
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-U-W");
            maker.defCmd.Use(p, "edit " + dest + " min 0 8 0");
            maker.defCmd.Use(p, "edit " + dest + " max 8 "+height+" 16");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
            dest += maker.Dir;
            //east
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-U-E");
            maker.defCmd.Use(p, "edit " + dest + " min 8 8 0");
            maker.defCmd.Use(p, "edit " + dest + " max 16 "+height+" 16");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
        }
        public static void MakeFlatStairs(CmdMakeBase maker, Player p, int origin, string name, ushort requestedSlot) 
        {
            int dest;
            if (!maker.AreEnoughBlockIDsFree(p, 8, out dest, requestedSlot)) 
            { 
                return; 
            }
            
            
            int height = 8;
            //north
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-D-N");
            maker.defCmd.Use(p, "edit " + dest + " min 0 0 0");
            maker.defCmd.Use(p, "edit " + dest + " max 16 "+height+" 1");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
            dest += maker.Dir;
            //south
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-D-S");
            maker.defCmd.Use(p, "edit " + dest + " min 0 0 15");
            maker.defCmd.Use(p, "edit " + dest + " max 16 "+height+" 16");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
            dest += maker.Dir;
            //west
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-D-W");
            maker.defCmd.Use(p, "edit " + dest + " min 0 0 0");
            maker.defCmd.Use(p, "edit " + dest + " max 1 "+height+" 16");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
            dest += maker.Dir;
            //east
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-D-E");
            maker.defCmd.Use(p, "edit " + dest + " min 15 0 0");
            maker.defCmd.Use(p, "edit " + dest + " max 16 "+height+" 16");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
            dest += maker.Dir;
            
            //--------------------------------------------------------------------------------upper
            height = 16;
            
            //north
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-U-N");
            maker.defCmd.Use(p, "edit " + dest + " min 0 8 0");
            maker.defCmd.Use(p, "edit " + dest + " max 16 "+height+" 1");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
            dest += maker.Dir;
            //south
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-U-S");
            maker.defCmd.Use(p, "edit " + dest + " min 0 8 15");
            maker.defCmd.Use(p, "edit " + dest + " max 16 "+height+" 16");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
            dest += maker.Dir;
            //west
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-U-W");
            maker.defCmd.Use(p, "edit " + dest + " min 0 8 0");
            maker.defCmd.Use(p, "edit " + dest + " max 1 "+height+" 16");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
            dest += maker.Dir;
            //east
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-U-E");
            maker.defCmd.Use(p, "edit " + dest + " min 15 8 0");
            maker.defCmd.Use(p, "edit " + dest + " max 16 "+height+" 16");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
        }
        public static void MakeCorners(CmdMakeBase maker, Player p, int origin, string name, ushort requestedSlot) 
        {
            int dest;
            if (!maker.AreEnoughBlockIDsFree(p, 4, out dest, requestedSlot)) 
            { 
                return; 
            }
            
            int height = 16;
            //north
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-NW");
            maker.defCmd.Use(p, "edit " + dest + " min 0 0 0");
            maker.defCmd.Use(p, "edit " + dest + " max 8 "+height+" 8");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
            dest += maker.Dir;
            //south
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-SE");
            maker.defCmd.Use(p, "edit " + dest + " min 8 0 8");
            maker.defCmd.Use(p, "edit " + dest + " max 16 "+height+" 16");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
            dest += maker.Dir;
            //west
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-SW");
            maker.defCmd.Use(p, "edit " + dest + " min 0 0 8");
            maker.defCmd.Use(p, "edit " + dest + " max 8 "+height+" 16");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
            dest += maker.Dir;
            //east
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-NE");
            maker.defCmd.Use(p, "edit " + dest + " min 8 0 0");
            maker.defCmd.Use(p, "edit " + dest + " max 16 "+height+" 8");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
        }
        public static void MakeEighths(CmdMakeBase maker, Player p, int origin, string name, ushort requestedSlot) 
        {
            int dest;
            if (!maker.AreEnoughBlockIDsFree(p, 8, out dest, requestedSlot)) 
            { 
                return; 
            }
            
            int height = 8;
            //north
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-D-NW");
            maker.defCmd.Use(p, "edit " + dest + " min 0 0 0");
            maker.defCmd.Use(p, "edit " + dest + " max 8 "+height+" 8");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
            dest += maker.Dir;
            //south
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-D-SE");
            maker.defCmd.Use(p, "edit " + dest + " min 8 0 8");
            maker.defCmd.Use(p, "edit " + dest + " max 16 "+height+" 16");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
            dest += maker.Dir;
            //west
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-D-SW");
            maker.defCmd.Use(p, "edit " + dest + " min 0 0 8");
            maker.defCmd.Use(p, "edit " + dest + " max 8 "+height+" 16");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
            dest += maker.Dir;
            //east
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-D-NE");
            maker.defCmd.Use(p, "edit " + dest + " min 8 0 0");
            maker.defCmd.Use(p, "edit " + dest + " max 16 "+height+" 8");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
            dest += maker.Dir;
            
            
            height = 16;
            //north
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-U-NW");
            maker.defCmd.Use(p, "edit " + dest + " min 0 8 0");
            maker.defCmd.Use(p, "edit " + dest + " max 8 "+height+" 8");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
            dest += maker.Dir;
            //south
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-U-SE");
            maker.defCmd.Use(p, "edit " + dest + " min 8 8 8");
            maker.defCmd.Use(p, "edit " + dest + " max 16 "+height+" 16");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
            dest += maker.Dir;
            //west
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-U-SW");
            maker.defCmd.Use(p, "edit " + dest + " min 0 8 8");
            maker.defCmd.Use(p, "edit " + dest + " max 8 "+height+" 16");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
            dest += maker.Dir;
            //east
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-U-NE");
            maker.defCmd.Use(p, "edit " + dest + " min 8 8 0");
            maker.defCmd.Use(p, "edit " + dest + " max 16 "+height+" 8");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
        }
        public static void MakePoles(CmdMakeBase maker, Player p, int origin, string name, ushort requestedSlot) 
        {
            int dest;
            if (!maker.AreEnoughBlockIDsFree(p, 3, out dest, requestedSlot)) 
            { 
                return; 
            }
            
            //UD
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-UD");
            maker.defCmd.Use(p, "edit " + dest + " min 4 0 4");
            maker.defCmd.Use(p, "edit " + dest + " max 12 16 12");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
            dest += maker.Dir;
            //NS
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-NS");
            maker.defCmd.Use(p, "edit " + dest + " min 4 4 0");
            maker.defCmd.Use(p, "edit " + dest + " max 12 12 16");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
            dest += maker.Dir;
            //WE
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-WE");
            maker.defCmd.Use(p, "edit " + dest + " min 0 4 4");
            maker.defCmd.Use(p, "edit " + dest + " max 16 12 12");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
        }
        
        public override void Help(Player p) 
        {
            p.Message("&T/{0} [type] [block] <optional starting ID>", name);
            p.Message("&HCreates block variants out of [block] based on [type].");
            p.Message("&H[type] can be: &b{0}", TYPES);
            p.Message("&HFor example, &T/make eighths stone &Hwould give you 8 new stone eighth piece blocks.");
        }
    }
    
    public class CmdMake : CmdMakeBase 
    {
        public override string name { get { return "Make"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return CommandTypes.Added; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Guest; } }
        
        
        public override bool CanUse(Player p) 
        {
            bool canUse = false;
            if (LevelInfo.IsRealmOwner(p.name, p.level.name)) canUse = true;
            if (!LevelInfo.Check(p, p.group.Permission, p.level, "make new blocks in this level")) 
            { 
                return false; 
            } //restrict making new blocks if you can't edit blockprops
            if (p.group.Permission >= LevelPermission.Operator && p.group.Permission >= p.level.BuildAccess.Min) 
            { 
                canUse = true; 
            }
            if (!canUse) 
            {
                p.Message("&cYou can only use this command on your own maps."); 
            }
            return canUse;
        }
        public override Command BlockDefCommand { get { return Command.Find("levelblock"); } }
        public override int Dir { get { return -1; } }
        public override int DefaultRequestedSlot { get { return Block.MaxRaw; } }
        public override ushort IterateTo { get { return Block.CPE_COUNT; } }
        public override BlockDefinition[] GetBlockDefs(Player p) 
        {
            return p.level.CustomBlockDefs;
        }
        public override string BlockPropsScope { get { return "level"; } }
    }
    
    public class CmdMakeGB : CmdMakeBase 
    {
        public override string name { get { return "MakeGB"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return CommandTypes.Added; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Admin; } }
        
        
        public override bool CanUse(Player p) 
        { 
            return true; 
        }
        public override Command BlockDefCommand { get { return Command.Find("globalblock"); } }
        public override int Dir { get { return 1; } }
        public override int DefaultRequestedSlot 
        {
            get 
            {
                BlockDefinition[] defs = BlockDefinition.GlobalDefs;
                for (ushort b = Block.CPE_COUNT; b <= Block.MaxRaw; b++) 
                {
                    ushort block = Block.FromRaw(b);
                    if (defs[block] == null) return b;
                }
                return -1;
            }
        }
        public override ushort IterateTo { get { return Block.MaxRaw; } }
        public override BlockDefinition[] GetBlockDefs(Player p) 
        { 
            return BlockDefinition.GlobalDefs;
        }
        public override string BlockPropsScope { get { return "global"; } }
    }
}

namespace Flames
{
    public class NoAFK : NewPlugin
    {
        public override string name { get { return "NoAFK"; } }
        public override string creator { get { return "HarmonyNetwork"; } }
        public static SchedulerTask UnAFKTask;
        public override void Load(bool startup)
        {
            //Command.Register(new CmdNotAFK());
            UnAFKTask = Server.Background.QueueRepeat(UnAFK, null, TimeSpan.FromSeconds(660));
        }
        public void UnAFK(SchedulerTask task)
        {
            Player[] players = PlayerInfo.Online.Items;
           	foreach (Player p2 in players)
            {
            	string PlayerName = p2.truename;
				if (PlayerName.CaselessEq("HarmonyNetwork"))
                {
        			if (p2.IsAfk)
                	{
                        CmdNotAFK.ToggleNotAFK(p2);
						p2.AutoAfk = false;
                    	p2.IsAfk = false;
                        //Logger.Log(LogType.SystemActivity, "{0} is no longer AFK.", p2.truename);
					}
                }
            }
        }
        public override void Unload(bool shutdown)
        {
            //Command.Unregister(Command.Find("NotAFK"));
            Server.Background.Cancel(UnAFKTask);
        }
        public override void Help(Player p)
        {
            p.Message("");
        }
    }

    public class CmdNotAFK : Command
    {
        public override string name { get { return "NotAFK"; } }
        public override string type { get { return CommandTypes.Information; } }
        public override bool SuperUseable { get { return false; } }
        public override bool MessageBlockRestricted { get { return true; } }
        public override LevelPermission defaultRank { get { return DefaultRank; } }
        public virtual LevelPermission DefaultRank { get { return LevelPermission.Flames; } }
        public override bool UseableWhenFrozen { get { return true; } }

        public override void Use(Player p, string message) 
        { 
            ToggleNotAFK(p); 
        }
        public static void ToggleNotAFK(Player p)
        {
            p.AutoAfk = false;
            p.IsAfk = false;
            p.afkMessage = null;
            TabList.Update(p, true);
            p.LastAction = DateTime.UtcNow;

            bool cantSend = !p.CanSpeak();
                if (cantSend)
                {
                    p.Message("You are no longer marked as being AFK.");
                }
                else
                {
                    ShowMessage(p, "-λNICK&S- is no longer AFK");
                    p.CheckForMessageSpam();
                }
                OnPlayerActionEvent.Call(p, PlayerAction.UnAFK, null, cantSend);
            }

        public static void ShowMessage(Player p, string message)
        {
            bool announce = !p.hidden && Server.Config.IRCShowAFK;
            Chat.MessageFrom(p, message, Chat.FilterVisible(p), announce);
            Logger.Log(LogType.SystemActivity, "{0} is no longer AFK.", p.truename);       
        }

        public override void Help(Player p)
        {
            p.Message("&T/NotAFK");
            p.Message("&HMarks you as not AFK.");
        }
    }
}
