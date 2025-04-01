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
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Flames.Added;
using Flames.Commands;
using Flames.Maths;
#if !F_DOTNET
using Flames.Scripting;
#else
using Flames.NewScripting;
#endif
namespace Flames
{
    public partial class Command : Order
    {
        public Dictionary<string, CommandAlias[]> aliases = new Dictionary<string, CommandAlias[]>();
        public override string Name { get { return name; } }
        /// <summary> The full name of this command (e.g. 'Copy') </summary>
        public virtual string name { get; }
        /// <summary> The shortcut/short name of this command (e.g. `"c"`) </summary>
        public virtual string shortcut { get { return ""; } }
        public override string Shortcut { get { return shortcut; } }
        /// <summary> The type/group of this command (see `CommandTypes` class) </summary>
        public virtual string type { get; }
        /// <summary> Whether this command can be used in museums </summary>
        /// <remarks> Level altering (e.g. places a block) commands should return false </remarks>
        public virtual bool museumUsable { get { return true; } }
        public override bool MuseumUsable { get { return museumUsable; } }

        /// <summary> Whether to show full command info in /help (cmd) </summary>
        public virtual bool ShowCommandInfo { get { return true; } }
        public override bool ShowOrderInfo { get { return ShowCommandInfo; } }
        public override LevelPermission DefaultRank { get { return defaultRank; } }


        /// <summary> The default minimum rank that is required to use this command </summary>
        public virtual LevelPermission defaultRank { get { return LevelPermission.Guest; } }
        public override void Execute(Player p, string message)
        {
            Use(p, message);
        }
        public override void Execute(Player p, string message, OrderData data)
        {
            Use(p, message, (CommandData)data);
        }
        public virtual void Use(Player p, string message)
        {
        }
        public virtual void Use(Player p, string message, CommandData data) 
        {
            Use(p, message); 
        }
        public override void OrderHelp(Player p)
        {
            Help(p);
        }
        public override void OrderHelp(Player p, string message)
        {
            Help(p, message);
        }
        public virtual void Help(Player p)
        {
        }
        public virtual void Help(Player p, string message) { Help(p); Formatter.PrintCommandInfo(p, this); }
        public override OrderPerm[] OrdExtraPerms { get { return OrderPerms.CMDToORD(ExtraPerms); } }
        public override OrderDesignation[] Designations
        {
            get
            {
                return Designation.CMDToORD(Aliases);
            }
        }
        public virtual CommandPerm[] ExtraPerms { get { return null; } }
        public virtual CommandAlias[] Aliases { get { return null; } }

        /// <summary> Whether this command is usable by 'super' players (Flames, IRC, etc) </summary>
        public virtual bool SuperUseable { get { return true; } }
        public override bool OrderSuperUseable { get { return SuperUseable; } }
        public override bool OrderMessageBlockRestricted { get { return MessageBlockRestricted; } }

        public virtual bool MessageBlockRestricted { get { return type.CaselessContains("mod"); } }


        /// <summary> Whether this command can be used when a player is frozen </summary>
        /// <remarks> Only informational commands should override this to return true </remarks>
        public virtual bool UseableWhenFrozen { get { return false; } }
        public override bool OrderUseableWhenFrozen { get { return UseableWhenFrozen; } }

        /// <summary> Whether using this command is logged to server logs </summary>
        /// <remarks> return false to prevent this command showing in logs (e.g. /pass) </remarks>
        public virtual bool LogUsage { get { return true; } }
        public override bool OrderLogUsage { get { return LogUsage; } }

        /// <summary> Whether this commands updates the 'most recent command used' by players </summary>
        /// <remarks> return false to prevent this command showing in /last (e.g. /pass, /hide) </remarks>
        public virtual bool UpdatesLastCmd { get { return true; } }
        public override bool UpdatesLastOrd { get { return UpdatesLastCmd; } }

        public virtual CommandParallelism Parallelism
        {
            get { return type.CaselessEq(CommandTypes.Information) ? CommandParallelism.NoAndWarn : CommandParallelism.Yes; }
        }
        public override OrderParallelism OrdParallelism
        {
            get { return (OrderParallelism)Parallelism; }
        }
        public CommandPerms Permissions;

        public static List<Command> allCmds = new List<Command>();
        public static bool IsCore(Command cmd)
        {
            return cmd.GetType().Assembly == Assembly.GetExecutingAssembly(); // TODO common method
        }

        public static List<Command> CopyAll()
        {
            return new List<Command>(allCmds);
        }


        public static void InitAll()
        {
            allCmds.Clear();
            Alias.coreAliases.Clear();
            Type[] types = Assembly.GetExecutingAssembly().GetTypes();
            for (int i = 0; i < types.Length; i++)
            {
                Type type = types[i];
                if (!type.IsSubclassOf(typeof(Command)) || type.IsAbstract || !type.IsPublic) continue;

                Command cmd = (Command)Activator.CreateInstance(type);
                if (Server.Config.DisabledCommands.CaselessContains(cmd.name)) continue;
                Register(cmd);
            }

            IScripting.AutoloadCommands();
        }

        public static void Register(Command cmd)
        {
            Order.Register(cmd);
            allCmds.Add(cmd);
            cmd.Permissions = CommandPerms.GetOrAdd(cmd.name, cmd.defaultRank);
            CommandPerm[] extra = cmd.ExtraPerms;
            if (extra != null)
            {
                for (int i = 0; i < extra.Length; i++)
                {
                    CommandExtraPerms exPerms = CommandExtraPerms.GetOrAdd(cmd.name, i + 1, extra[i].Perm);
                    exPerms.Desc = extra[i].Description;
                }
            }
            Alias.RegisterDefaults(cmd);
        }
        public static void Register(params Command[] commands)
        {
            foreach (Command cmd in commands) Register(cmd);
        }
        public static void TryRegister(bool announce, params Command[] commands)
        {
            foreach (Command cmd in commands)
            {
                if (Find(cmd.name) != null) continue;

                Register(cmd);
                if (announce) Logger.Log(LogType.SystemActivity, "Command /{0} loaded", cmd.name);
            }
        }

        public static bool Unregister(Command cmd)
        {
            // typical usage: Command.Unregister(Command.Find("xyz"))
            // So don't throw exception if Command.Find returned null
            if (cmd != null) Alias.UnregisterDefaults(cmd);
            return Order.Unregister(cmd);
        }

        public static void Unregister(params Command[] commands)
        {
            foreach (Command cmd in commands) Unregister(cmd);
        }


        public static string GetColoredName(Command cmd)
        {
            LevelPermission perm = cmd.Permissions.MinRank;
            return Group.GetColor(perm) + cmd.name;
        }

        public static Command Find(string name)
        {
            foreach (Command cmd in allCmds)
            {
                if (cmd.name.CaselessEq(name)) return cmd;
            }
            return null;
        }

        public static void Search(ref string cmdName, ref string cmdArgs)
        {
            if (cmdName.Length == 0) return;
            Alias alias = Alias.Find(cmdName);

            // Aliases override built in command shortcuts
            if (alias == null)
            {
                foreach (Command cmd in allCmds)
                {
                    if (!cmd.shortcut.CaselessEq(cmdName)) continue;
                    cmdName = cmd.name;
                    return;
                }
                return;
            }

            cmdName = alias.Target;
            string format = alias.Format;
            if (format == null) return;

            if (format.Contains("{args}"))
            {
                cmdArgs = format.Replace("{args}", cmdArgs);
            }
            else
            {
                cmdArgs = format + " " + cmdArgs;
            }
            cmdArgs = cmdArgs.Trim();
        }
    }

    public enum CommandContext : byte
    {
        Normal, Static, SendCmd, Purchase, MessageBlock
    }

    public struct CommandData
    {
        public LevelPermission Rank;
        public CommandContext Context;
        public Vec3S32 MBCoords;
        public static explicit operator CommandData(OrderData v)
        {
            CommandData data = new CommandData();
            data.Rank = v.OrderRank;
            data.Context = (CommandContext)v.orderContext;
            data.MBCoords = v.OrderMBCoords;
            return data;
        }
    }

    // Clunky design, but needed to stay backwards compatible with custom commands
    public abstract class Command2 : Command
    {
        public override void Use(Player p, string message)
        {
            Use(p, message, p.DefaultCmdData);
        }
    }

    public enum CommandParallelism
    {
        NoAndSilent, NoAndWarn, Yes
    }
}

namespace Flames.Commands
{
    public struct CommandPerm
    {
        public LevelPermission Perm;
        public string Description;
        public CommandPerm(LevelPermission perm, string desc)
        {
            Perm = perm;
            Description = desc;
        }
        public static explicit operator CommandPerm(OrderPerm v)
        {
            CommandPerm perm = new CommandPerm();
            perm.Perm = v.Perm;
            perm.Description = v.Description;
            return perm;
        }
        public static bool operator ==(CommandPerm a, CommandPerm[] permB)
        {
            foreach (CommandPerm b in permB)
            {
                return a.Perm == b.Perm && a.Description == b.Description;
            }
            return false;
        }
        public static bool operator !=(CommandPerm a, CommandPerm[] permB)
        {
            foreach (CommandPerm b in permB)
            {
                return a.Perm != b.Perm && a.Description != b.Description;
            }
            return false;
        }
        public static bool operator ==(CommandPerm[] permA, CommandPerm b)
        {
            foreach (CommandPerm a in permA)
            {
                return a.Perm == b.Perm && a.Description == b.Description;
            }
            return false;
        }
        public static bool operator !=(CommandPerm[] permA, CommandPerm b)
        {
            foreach (CommandPerm a in permA)
            {
                return a.Perm != b.Perm && a.Description != b.Description;
            }
            return false;
        }
        public static bool operator ==(CommandPerm a, OrderPerm[] permB)
        {
            foreach (OrderPerm b in permB)
            {
                return a.Perm == b.Perm && a.Description == b.Description;
            }
            return false;
        }
        public static bool operator !=(CommandPerm a, OrderPerm[] permB)
        {
            foreach (OrderPerm b in permB)
            {
                return a.Perm != b.Perm && a.Description != b.Description;
            }
            return false;
        }
        public static explicit operator CommandPerm(OrderPerm[] ordPerms)
        {
            CommandPerm perm = new CommandPerm();
            foreach (OrderPerm v in ordPerms)
            {
                perm.Perm = v.Perm;
                perm.Description = v.Description;
                return perm;
            }
            return perm;
        }
    }

    public struct CommandAlias : IEnumerable<CommandAlias[]>
    {
        public string Trigger, Format;

        public CommandAlias(string cmd, string format = null)
        {
            Trigger = cmd;
            Format = format;
        }
        public IEnumerator<CommandAlias[]> GetEnumerator()
        {
            Command cmd = new Command();
            return cmd.aliases.Values.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            Command cmd = new Command();
            return cmd.aliases.Values.GetEnumerator();
        }
        public static bool operator ==(CommandAlias a, OrderDesignation[] permB)
        {
            foreach (OrderDesignation b in permB)
            {
                return a.Trigger == b.Trigger && a.Format == b.Format;
            }
            return false;
        }
        public static bool operator !=(CommandAlias a, OrderDesignation[] permB)
        {
            foreach (OrderDesignation b in permB)
            {
                return a.Trigger != b.Trigger && a.Format != b.Format;
            }
            return false;
        }
        public static explicit operator CommandAlias(OrderDesignation v)
        {
            CommandAlias alias = new CommandAlias();
            alias.Trigger = v.Trigger;
            alias.Format = v.Format;
            return alias;
        }
    }
}
