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
using System.Net;
using Flames.DB;
using Flames.Generator;
using Flames.Network;
using Flames.Tasks;

namespace Flames 
{
    public sealed partial class Server 
    {
        public static void LoadMainLevel(SchedulerTask task) {
            try {
                mainLevel = LevelActions.Load(Player.Flame, Config.MainLevel, false);
                if (mainLevel == null) GenerateMain();
            } catch (Exception ex) {
                Logger.LogError("Error loading main level", ex);
            }
        }

        public static void GenerateMain() {
            Logger.Log(LogType.SystemActivity, "main level not found, generating..");
            mainLevel = new Level(Config.MainLevel, 128, 64, 128);
            
            MapGen.Find("Flat").Generate(Player.Flame, mainLevel, "");
            mainLevel.Save();
            Level.LoadMetadata(mainLevel);
            LevelInfo.Add(mainLevel);
        }

        public static void LoadAllPlugins(SchedulerTask task) { Plugin.LoadAll(); }
        public static void LoadAllSimplePlugins(SchedulerTask task) { Plugin_Simple.LoadAll(); }

        public static void InitPlayerLists(SchedulerTask task) {
            try {
                UpgradeTasks.UpgradeOldAgreed();
            } catch (Exception ex) {
                Logger.LogError("Error upgrading agreed list", ex);
            }
            
            LoadPlayerLists();
            ModerationTasks.QueueTasks();
        }

        public static void LoadPlayerLists() {
            agreed = PlayerList.Load("ranks/agreed.txt");
            invalidIds = PlayerList.Load("extra/invalidids.txt");
            Player.Flame.DatabaseID = NameConverter.InvalidNameID("(flames)");
            
            hidden   = PlayerList.Load("ranks/hidden.txt");
            vip      = PlayerList.Load("text/vip.txt");
            noEmotes = PlayerList.Load("text/emotelist.txt");
            lockdown = PlayerList.Load("text/lockdown.txt");
            
            models = PlayerExtList.Load("extra/models.txt");
            skins  = PlayerExtList.Load("extra/skins.txt");
            reach  = PlayerExtList.Load("extra/reach.txt");
            rotations   = PlayerExtList.Load("extra/rotations.txt");
            modelScales = PlayerExtList.Load("extra/modelscales.txt");

            bannedIP  = PlayerExtList.Load("ranks/banned-ip.txt");
            muted     = PlayerExtList.Load("ranks/muted.txt");
            frozen    = PlayerExtList.Load("ranks/frozen.txt");
            jailed    = PlayerExtList.Load("ranks/jailed.txt");

            tempRanks = PlayerExtList.Load(Paths.TempRanksFile);
            tempBans  = PlayerExtList.Load(Paths.TempBansFile);
            whiteList = PlayerList.Load("ranks/whitelist.txt");
        }

        public static void LoadAutoloadMaps(SchedulerTask task) {
            AutoloadMaps = PlayerExtList.Load("text/autoload.txt", '=');
            List<string> maps = AutoloadMaps.AllNames();
            
            foreach (string map in maps) {
                if (map.CaselessEq(Config.MainLevel)) continue;
                LevelActions.Load(Player.Flame, map, false);
            }
        }

        public static void SetupSocket(SchedulerTask task) {        
            IPAddress ip;
            
            if (!IPAddress.TryParse(Config.ListenIP, out ip)) {
                Logger.Log(LogType.Warning, "Unable to parse listen IP config key, listening on any IP");
                ip = IPAddress.Any;
            }            
            Listener.Listen(ip, Config.Port);
        }

        public static void InitHeartbeat(SchedulerTask task) {
            Heartbeat.Start();
        }

        public static void InitTimers(SchedulerTask task) {
            MainScheduler.QueueRepeat(RandomMessage, null, 
                                      Config.AnnouncementInterval);
            Critical.QueueRepeat(ServerTasks.UpdateEntityPositions, null,
                                 TimeSpan.FromMilliseconds(Config.PositionUpdateInterval));
        }

        public static void InitRest(SchedulerTask task) {
            MainScheduler.QueueRepeat(BlockQueue.Loop, null, 
                                      TimeSpan.FromMilliseconds(BlockQueue.Interval));
            Critical.QueueRepeat(ServerTasks.TickPlayers, null,
                                 TimeSpan.FromMilliseconds(20));

            Logger.Log(LogType.SystemActivity, "Finished setting up server");
            SetupFinished = true;
        }
    }
}