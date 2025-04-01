namespace Flames.Commands
{
    public sealed class CmdPlayer : Command
    {
        public override string name { get { return "Player"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return CommandTypes.Added; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Owner; } }
        public override void Use(Player p, string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                p = PlayerInfo.FindExact(message);
            }
            p.Message("p.Ignores: {0}", p.Ignores);
            p.Message("Player.lastMSG: {0}", Player.lastMSG);
            p.Message("p.persistentMessages: {0}", p.persistentMessages);
            p.Message("p.ZoneIn: {0}", p.ZoneIn);
            p.Message("p.Request: {0}", p.Request);
            p.Message("p.senderName: {0}", p.senderName);
            p.Message("p.currentTpa: {0}", p.currentTpa);
            p.Message("p.Socket: {0}", p.Socket);
            p.Message("p.Session: {0}", p.Session);
            p.Message("p.LastAction: {0}", p.LastAction);
            p.Message("p.AFKCooldown: {0}", p.AFKCooldown);
            p.Message("p.IsAfk: {0}", p.IsAfk);
            p.Message("p.AutoAfk: {0}", p.AutoAfk);
            p.Message("p.cmdTimer: {0}", p.cmdTimer);
            p.Message("p.UsingWom: {0}", p.UsingWom);
            p.Message("p.BrushName: {0}", p.BrushName);
            p.Message("p.DefaultBrushArgs: {0}", p.DefaultBrushArgs);
            p.Message("p.Transform: {0}", p.Transform);
            p.Message("p.afkMessage: {0}", p.afkMessage);
            p.Message("p.ClickToMark: {0}", p.ClickToMark);
            p.Message("p.name: {0}", p.name);
            p.Message("p.DisplayName: {0}", p.DisplayName);
            p.Message("p.warn: {0}", p.warn);
            p.Message("p.id: {0}", p.id);
            p.Message("p.IP: {0}", p.IP);
            p.Message("p.ip: {0}", p.ip);
            p.Message("p.color: {0}", p.color);
            p.Message("p.group: {0}", p.group);
            p.Message("p.hideRank: {0}", p.hideRank);
            p.Message("p.hidden: {0}", p.hidden);
            p.Message("p.painting: {0}", p.painting);
            p.Message("p.checkingBotInfo: {0}", p.checkingBotInfo);
            p.Message("p.muted: {0}", p.muted);
            p.Message("p.jailed: {0}", p.jailed);
            p.Message("p.agreed: {0}", p.agreed);
            p.Message("p.invincible: {0}", p.invincible);
            p.Message("p.prefix: {0}", p.prefix);
            p.Message("p.title: {0}", p.title);
            p.Message("p.titlecolor: {0}", p.titlecolor);
            p.Message("p.passtries: {0}", p.passtries);
            p.Message("p.hasreadrules: {0}", p.hasreadrules);
            p.Message("p.NextReviewTime: {0}", p.NextReviewTime);
            p.Message("p.NextEat: {0}", p.NextEat);
            p.Message("p.NextTeamInvite: {0}", p.NextTeamInvite);
            p.Message("p.ReachDistance: {0}", p.ReachDistance);
            p.Message("p.hackrank: {0}", p.hackrank);
            p.Message("p.SuperName: {0}", p.SuperName);
            p.Message("p.IsSuper: {0}", p.IsSuper);
            p.Message("p.IsFire: {0}", p.IsFire);
            p.Message("p.IsConsole: {0}", p.IsConsole);
            p.Message("p.IsNull: {0}", p.IsNull);
            p.Message("p.FullName: {0}", p.FullName);
            p.Message("p.ColoredName: {0}", p.ColoredName);
            p.Message("p.GroupPrefix: {0}", p.GroupPrefix);
            p.Message("p.deleteMode: {0}", p.deleteMode);
            p.Message("p.ignoreGrief: {0}", p.ignoreGrief);
            p.Message("p.parseEmotes: {0}", p.parseEmotes);
            p.Message("p.opchat: {0}", p.opchat);
            p.Message("p.adminchat: {0}", p.adminchat);
            p.Message("p.whisper: {0}", p.whisper);
            p.Message("p.whisperTo: {0}", p.whisperTo);
            p.Message("p.partialMessage: {0}", p.partialMessage);
            p.Message("p.trainGrab: {0}", p.trainGrab);
            p.Message("p.onTrain: {0}", p.onTrain);
            p.Message("p.trainInvincible: {0}", p.trainInvincible);
            p.Message("p.mbRecursion: {0}", p.mbRecursion);
            p.Message("p.frozen: {0}", p.frozen);
            p.Message("p.following: {0}", p.following);
            p.Message("p.possess: {0}", p.possess);
            p.Message("p.possessed: {0}", p.possessed);
            p.Message("p.AllowBuild: {0}", p.AllowBuild);
            p.Message("p.money: {0}", p.money);
            p.Message("p.TotalModified: {0}", p.TotalModified);
            p.Message("p.TotalDrawn: {0}", p.TotalDrawn);
            p.Message("p.TotalPlaced: {0}", p.TotalPlaced);
            p.Message("p.TotalDeleted: {0}", p.TotalDeleted);
            p.Message("p.TimesVisited: {0}", p.TimesVisited);
            p.Message("p.TimesBeenKicked: {0}", p.TimesBeenKicked);
            p.Message("p.TimesDied: {0}", p.TimesDied);
            p.Message("p.TotalMessagesSent: {0}", p.TotalMessagesSent);
            p.Message("p.startModified: {0}", p.startModified);
            p.Message("p.SessionModified: {0}", p.SessionModified);
            p.Message("p.startTime: {0}", p.startTime);
            p.Message("p.TotalTime: {0}", p.TotalTime);
            p.Message("p.SessionStartTime: {0}", p.SessionStartTime);
            p.Message("p.FirstLogin: {0}", p.FirstLogin);
            p.Message("p.LastLogin: {0}", p.LastLogin);
            p.Message("p.staticCommands: {0}", p.staticCommands);
            p.Message("p.lastAccessStatus: {0}", p.lastAccessStatus);
            p.Message("p.CriticalTasks: {0}", p.CriticalTasks);
            p.Message("p.isFlying: {0}", p.isFlying);
            p.Message("p.aiming: {0}", p.aiming);
            p.Message("p.weapon: {0}", p.weapon);
            p.Message("p.weaponBuffer: {0}", p.weaponBuffer);
            p.Message("p.joker: {0}", p.joker);
            p.Message("p.Unverified: {0}", p.Unverified);
            p.Message("p.verifiedPass: {0}", p.verifiedPass);
            p.Message("p.voice: {0}", p.voice);
            p.Message("p.DefaultCmdData: {0}", p.DefaultCmdData);
            p.Message("p.useCheckpointSpawn: {0}", p.useCheckpointSpawn);
            p.Message("p.lastCheckpointIndex: {0}", p.lastCheckpointIndex);
            p.Message("p.checkpointX: {0}", p.checkpointX);
            p.Message("p.checkpointY: {0}", p.checkpointY);
            p.Message("p.checkpointZ: {0}", p.checkpointZ);
            p.Message("p.checkpointRotX: {0}", p.checkpointRotX);
            p.Message("p.checkpointRotY: {0}", p.checkpointRotY);
            p.Message("p.voted: {0}", p.voted);
            p.Message("p.flipHead: {0}", p.flipHead);
            p.Message("p.infected: {0}", p.infected);
            p.Message("p.Game: {0}", p.Game);
            p.Message("p.DatabaseID: {0}", p.DatabaseID);
            p.Message("p.CopySlots: {0}", p.CopySlots);
            p.Message("p.CurrentCopySlot: {0}", p.CurrentCopySlot);
            p.Message("p.CurrentCopy: {0}", p.CurrentCopy);
            p.Message("p.gbStep: {0}", p.gbStep);
            p.Message("p.lbStep: {0}", p.lbStep);
            p.Message("p.gbBlock: {0}", p.gbBlock);
            p.Message("p.lbBlock: {0}", p.lbBlock);
            p.Message("p.DrawOps: {0}", p.DrawOps);
            p.Message("p.pendingDrawOpsLock: {0}", p.pendingDrawOpsLock);
            p.Message("p.PendingDrawOps: {0}", p.PendingDrawOps);
            p.Message("p.showPortals: {0}", p.showPortals);
            p.Message("p.showMBs: {0}", p.showMBs);
            p.Message("p.prevMsg: {0}", p.prevMsg);
            p.Message("p.oldIndex: {0}", p.oldIndex);
            p.Message("p.lastWalkthrough: {0}", p.lastWalkthrough);
            p.Message("p.startFallY: {0}", p.startFallY);
            p.Message("p.lastFallY: {0}", p.lastFallY);
            p.Message("p.drownTime: {0}", p.drownTime);
            p.Message("p.deathCooldown: {0}", p.deathCooldown);
            p.Message("p.ModeBlock: {0}", p.ModeBlock);
            p.Message("p.ClientHeldBlock: {0}", p.ClientHeldBlock);
            p.Message("p.BlockBindings: {0}", p.BlockBindings);
            p.Message("p.CmdBindings: {0}", p.CmdBindings);
            p.Message("p.lastCMD: {0}", p.lastCMD);
            p.Message("p.lastCmdTime: {0}", p.lastCmdTime);
            p.Message("p.c4circuitNumber: {0}", p.c4circuitNumber);
            p.Message("p.level: {0}", p.level);
            p.Message("p.Loading: {0}", p.Loading);
            p.Message("p.UsingGoto: {0}", p.UsingGoto);
            p.Message("p.GeneratingMap: {0}", p.GeneratingMap);
            p.Message("p.LoadingMuseum: {0}", p.LoadingMuseum);
            p.Message("p.lastClick: {0}", p.lastClick);
            p.Message("p.PreTeleportPos: {0}", p.PreTeleportPos);
            p.Message("p.PreTeleportRot: {0}", p.PreTeleportRot);
            p.Message("p.PreTeleportMap: {0}", p.PreTeleportMap);
            p.Message("p.summonedMap: {0}", p.summonedMap);
            p.Message("p._tempPos: {0}", p._tempPos);
            p.Message("p.Extras: {0}", p.Extras);
            p.Message("p.spamChecker: {0}", p.spamChecker);
            p.Message("p.cmdUnblocked: {0}", p.cmdUnblocked);
            p.Message("p.partialLog: {0}", p.partialLog);
            p.Message("p.Waypoints: {0}", p.Waypoints);
            p.Message("p.Rank: {0}", p.Rank);
            p.Message("p.loggedIn: {0}", p.loggedIn);
            p.Message("p.verifiedName: {0}", p.verifiedName);
            p.Message("p.VerifiedVia: {0}", p.VerifiedVia);
            p.Message("p.gotSQLData: {0}", p.gotSQLData);
            p.Message("p.cancelcommand: {0}", p.cancelcommand);
            p.Message("p.cancelchat: {0}", p.cancelchat);
            p.Message("p.cancellogin: {0}", p.cancellogin);
            p.Message("p.cancelconnecting: {0}", p.cancelconnecting);
            p.Message("p.serialCmds: {0}", p.serialCmds);
            p.Message("p.serialCmdsLock: {0}", p.serialCmdsLock);
            Player.SerialCommand serialCommand = new Player.SerialCommand();
            p.Message("p.SerialCommand.cmd: {0}", serialCommand.cmd);
            p.Message("p.SerialCommand.args: {0}", serialCommand.args);
            p.Message("p.SerialCommand.data: {0}", serialCommand.data);
            p.Message("p.blockchangeObject: {0}", p.blockchangeObject);
        }

        public override void Help(Player p)
        {
        }
    }
}