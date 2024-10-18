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
using Flames.Drawing;
using Flames.Drawing.Transforms;
using Flames.Events.PlayerEvents;
using Flames.Games;
using Flames.Maths;
using Flames.Network;
using Flames.Tasks;
using Flames.Undo;

namespace Flames
{

    public partial class Player : IDisposable
    {

        public PlayerIgnores Ignores = new PlayerIgnores();
        public static string lastMSG = "";
        public PersistentMessages persistentMessages = new PersistentMessages();
        public Zone ZoneIn;
        public CinematicGui CinematicGui = new CinematicGui();

        //TpA
        public bool Request;
        public string senderName = "";
        public string currentTpa = "";

        /// <summary> Account name of the user </summary>
        /// <remarks> Use 'truename' for displaying/logging, use 'name' for storing data </remarks>
        public string truename;
        /// <summary> The underlying socket for sending/receiving raw data </summary>
        public INetSocket Socket;
        public IGameSession Session;

        public DateTime LastAction, AFKCooldown;
        public bool IsAfk, AutoAfk;
        public bool cmdTimer;
        public bool UsingWom;
        public string BrushName = "Normal", DefaultBrushArgs = "";
        public Transform Transform = NoTransform.Instance;
        public string afkMessage;
        public bool ClickToMark = true;

        /// <summary> Account name of the user, plus a trailing '+' if ClassiCubeAccountPlus is enabled </summary>
        /// <remarks> Use 'truename' for displaying/logging, use 'name' for storing data </remarks>
        public string name;
        public string DisplayName;
        public int warn;
        public byte id;
        public IPAddress IP;
        public string ip;
        public string color;
        public Group group;
        public LevelPermission hideRank = LevelPermission.Banned;
        public bool hidden;
        public bool painting;
        public bool checkingBotInfo;
        public bool muted;
        public bool jailed;
        public bool agreed = true;
        public bool invincible;
        public string prefix = "";
        public string title = "";
        public string titlecolor = "";
        public int passtries = 0;
        public bool hasreadrules;
        public DateTime NextReviewTime, NextEat, NextTeamInvite;
        public float ReachDistance = 5;
        public bool hackrank;
        public string SuperName;
        /// <summary> Whether this player is a 'Super' player (Flames, IRC, etc) </summary>
        public bool IsSuper;
        /// <summary> Whether this player is the Flames player instance. </summary>
        public bool IsFire { get { return this == Flame; } }
        /// <summary> Backwards compatibility with MCGalaxy plugins </summary>
        public bool IsConsole { get { return this == Console; } }
#if CORE
        /// <summary> Work on backwards compatibility with other cores </summary>
        public bool IsSparkie { get { return this == Sparks; } }
        /// <summary> Work on backwards compatibility with other cores </summary>
        public bool IsNova { get { return this == Nova; } }
        /// <summary> Work on backwards compatibility with other cores </summary>
        public bool IsRandom { get { return this == Random; } }
#endif
        public virtual bool IsNull { get { return this == null; } }
        public virtual string FullName { get { return color + prefix + DisplayName; } }
        public string ColoredName { get { return color + DisplayName; } }
        public string GroupPrefix { get { return group.Prefix.Length == 0 ? "" : "&f" + group.Prefix; } }

        public bool deleteMode;
        /// <summary> Whether automatic blockspam detection should be skipped for this player </summary>
        public bool ignoreGrief;
        public bool parseEmotes = Server.Config.ParseEmotes;
        public bool opchat;
        public bool adminchat;
        public bool whisper;
        public string whisperTo = "";
        public string partialMessage = "";

        public bool trainGrab;
        public bool onTrain, trainInvincible;
        public int mbRecursion;

        public bool frozen;
        public string following = "";
        public string possess = "";
        // Only used for possession.
        //Using for anything else can cause unintended effects!
        public bool possessed;

        /// <summary> Whether this player has permission to build in the current level. </summary>
        public bool AllowBuild = true;

        public int money;
        public long TotalModified, TotalDrawn, TotalPlaced, TotalDeleted;
        public int TimesVisited, TimesBeenKicked, TimesDied;
        public int TotalMessagesSent;

        public long startModified;
        public long SessionModified { get { return TotalModified - startModified; } }

        public DateTime startTime;
        public TimeSpan TotalTime
        {
            get { return DateTime.UtcNow - startTime; }
            set { startTime = DateTime.UtcNow.Subtract(value); }
        }
        public DateTime SessionStartTime;
        public DateTime FirstLogin, LastLogin;

        public bool staticCommands;
        public DateTime lastAccessStatus;
        public VolatileArray<SchedulerTask> CriticalTasks = new VolatileArray<SchedulerTask>();

        public bool isFlying;
        public bool aiming;
        public Weapon weapon;
        public BufferedBlockSender weaponBuffer;

        public bool joker;
        public bool Unverified, verifiedPass;
        /// <summary> Whether this player can speak even while chat moderation is on </summary>
        public bool voice;

        public CommandData DefaultCmdData
        {
            get
            {
                CommandData data = default;
                data.Rank = Rank; return data;
            }
        }

        public bool useCheckpointSpawn;
        public int lastCheckpointIndex = -1;
        public ushort checkpointX, checkpointY, checkpointZ;
        public byte checkpointRotX, checkpointRotY;
        public bool voted;
        public bool flipHead, infected;
        public GameProps Game = new GameProps();
        /// <summary> Persistent ID of this user in the Players table. </summary>
        public int DatabaseID;

        public List<CopyState> CopySlots = new List<CopyState>();
        public int CurrentCopySlot;
        public CopyState CurrentCopy
        {
            get { return CurrentCopySlot >= CopySlots.Count ? null : CopySlots[CurrentCopySlot]; }
            set
            {
                while (CurrentCopySlot >= CopySlots.Count) 
                { 
                    CopySlots.Add(null); 
                }
                CopySlots[CurrentCopySlot] = value;
            }
        }

        // BlockDefinitions
        public int gbStep = 0, lbStep = 0;
        public BlockDefinition gbBlock, lbBlock;

        //Undo
        public VolatileArray<UndoDrawOpEntry> DrawOps = new VolatileArray<UndoDrawOpEntry>();
        public object pendingDrawOpsLock = new object();
        public List<PendingDrawOp> PendingDrawOps = new List<PendingDrawOp>();

        public bool showPortals, showMBs;
        public string prevMsg = "";

        //Movement
        public int oldIndex = -1, lastWalkthrough = -1, startFallY = -1, lastFallY = -1;
        public DateTime drownTime = DateTime.MaxValue;

        public DateTime deathCooldown;

        public ushort ModeBlock = Block.Invalid;
        /// <summary> The block ID this player's client specifies it is currently holding in hand. </summary>
        /// <remarks> This ignores /bind and /mode. GetHeldBlock() is usually preferred. </remarks>
        public ushort ClientHeldBlock = Block.Stone;
        public ushort[] BlockBindings = new ushort[Block.SUPPORTED_COUNT];
        public Dictionary<string, string> CmdBindings = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public string lastCMD = "";
        public DateTime lastCmdTime;
        public sbyte c4circuitNumber = -1;

        public Level level;
        public bool Loading = true; //True if player is loading a map.
        public int UsingGoto = 0, GeneratingMap = 0, LoadingMuseum = 0;
        public Vec3U16 lastClick = Vec3U16.Zero;

        public Position PreTeleportPos;
        public Orientation PreTeleportRot;
        public string PreTeleportMap;

        public string summonedMap;
        public Position _tempPos;

        // Extra storage for custom commands
        public ExtrasCollection Extras = new ExtrasCollection();

        public SpamChecker spamChecker;
        public DateTime cmdUnblocked;
        public List<DateTime> partialLog;

        public WarpList Waypoints = new WarpList();
        public DateTime LastPatrol;
        public LevelPermission Rank { get { return group.Permission; } }

        /// <summary> Whether player has completed login process and has been sent initial map. </summary>
        public bool loggedIn;
        public bool verifiedName;
        /// <summary> The URL of the authentication service this player's name was verified via. Can be null </summary>
        /// <example> http://www.classicube.net/heartbeat.jsp </example>
        public string VerifiedVia;
        public bool gotSQLData;


        public bool cancelcommand, cancelchat;
        public bool cancellogin, cancelconnecting;

        public Queue<SerialCommand> serialCmds = new Queue<SerialCommand>();
        public object serialCmdsLock = new object();
        public struct SerialCommand
        {
            public Command cmd;
            public string args;
            public CommandData data;
        }

        /// <summary> Called when a player removes or places a block.
        /// NOTE: Currently this prevents the OnBlockChange event from being called. </summary>
        public event SelectionBlockChange Blockchange;

        public void ClearBlockchange() 
        { 
            ClearSelection(); 
        }
        public object blockchangeObject;

        /// <summary> Called when the player has finished providing all the marks for a selection. </summary>
        /// <returns> Whether to repeat this selection, if /static mode is enabled. </returns>
        public delegate bool SelectionHandler(Player p, Vec3S32[] marks, object state, ushort block);

        /// <summary> Called when the player has provided a mark for a selection. </summary>
        /// <remarks> i is the index of the mark, so the 'first' mark has 0 for i. </remarks>
        public delegate void SelectionMarkHandler(Player p, Vec3S32[] marks, int i, object state, ushort block);
    }
}
