﻿/*
    Copyright 2010 MCSharp team (Modified for use with MCZall/MCLawl/MCForge)
    
    Dual-licensed under the    Educational Community License, Version 2.0 and
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
using Flames.Network;
using Flames.Tasks;

namespace Flames 
{
    public sealed partial class Server 
    {
        public static bool cancelcommand;        
        public delegate void OnFlameCommand(string cmd, string message);
        public static event OnFlameCommand FlameCommand;
        public delegate void MessageEventHandler(string message);
        public delegate void VoidHandler();
        
        public static event MessageEventHandler OnURLChange;
        public static event VoidHandler OnSettingsUpdate;
        public static ServerConfig Config = new ServerConfig();
        public static DateTime StartTime;
        
        public static PlayerExtList AutoloadMaps;
        public static PlayerMetaList RankInfo = new PlayerMetaList("text/rankinfo.txt");
        public static PlayerMetaList Notes = new PlayerMetaList("text/notes.txt");
#if CORE
        /// <summary> *** DO NOT USE THIS! *** Use VersionString, as this field is a constant and is inlined if used. </summary>
        public const string InternalVersion = "1.0.0.2";
        public static string Version { get { return InternalVersion; } }
        public static string SoftwareName = "&dH&1a&er&cm&6o&6n&0y";
        static string fullName;
        public static string SoftwareNameVersioned {
            // By default, if SoftwareName gets externally changed, that is reflected in SoftwareNameVersioned too
            get { return fullName ?? SoftwareName + " " + Version; }
            set { fullName = value; }
        }
#else
        /// <summary> *** DO NOT USE THIS! *** Use VersionString, as this field is a constant and is inlined if used. </summary>
        public const string InternalVersion = "9.0.2.6";
        public static string Version { get { return InternalVersion; } }
        public static string SoftwareName = "&4F&cl&4a&cm&4e&cs";
        static string fullName;
        public static string SoftwareNameVersioned {
            // By default, if SoftwareName gets externally changed, that is reflected in SoftwareNameVersioned too
            get { return fullName ?? SoftwareName + " " + Version; }
            set { fullName = value; }
        }
#endif

        public static INetListen Listener = new TcpListen();

        //Other
        public static bool SetupFinished, CLIMode;
        
        public static PlayerList whiteList, invalidIds;
        public static PlayerList ignored, hidden, agreed, vip, noEmotes, lockdown;
        public static PlayerExtList models, skins, reach, rotations, modelScales;
        public static PlayerExtList bannedIP, frozen, muted, tempBans, tempRanks;
        
        public static readonly List<string> Devs = new List<string>() { 
            "DarkBurningFlame", "BurningFlame", "SuperNova", "DeadNova", 
            "HyperNova", "RandomStranger05", "GoldenSparks", "AurumStellae", 
            "sethbatman05", "sethbatman2005", "jackstage1", "Pattykaki45", 
            "jaketheidiot", "RandomStrangers", "ArgenteaeLunae", "Argenteae"};
        public static readonly List<string> Opstats = new List<string>() { "ban", "tempban", "xban", "banip", "kick", "warn", "mute", "freeze", "setrank" };

        public static Level mainLevel;

        public static PlayerList reviewlist = new PlayerList();
        static string[] announcements = new string[0];
        public static string RestartPath;

        // Extra storage for custom commands
        public static ExtrasCollection Extras = new ExtrasCollection();
        
        public static int YesVotes, NoVotes;
        public static bool voting;
        public const int MAX_PLAYERS = int.MaxValue;
        
        public static Scheduler MainScheduler = new Scheduler("F_MainScheduler");
        public static Scheduler Background = new Scheduler("F_BackgroundScheduler");
        public static Scheduler Critical = new Scheduler("F_CriticalScheduler");
        public static Scheduler Heartbeats = new Scheduler("F_HeartbeatsScheduler");
        public static Server s = new Server();

        public const byte VERSION_0016 = 3; // classic 0.0.16
        public const byte VERSION_0017 = 4; // classic 0.0.17 / 0.0.18
        public const byte VERSION_0019 = 5; // classic 0.0.19
        public const byte VERSION_0020 = 6; // classic 0.0.20 / 0.0.21 / 0.0.23
        public const byte VERSION_0030 = 7; // classic 0.30 (final)
        
        public static bool chatmod, flipHead;
        public static bool shuttingDown;
    }
}