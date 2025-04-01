using System;
using System.Diagnostics;
using System.Threading;
using Flames.Platform;
using Flames.SQL;

namespace Flames.Commands
{
    public class CmdServerInfo : Command2
    {

        public override string name { get { return "ServerInfo"; } }
        public override string shortcut { get { return "SInfo"; } }
        public override string type { get { return CommandTypes.Information; } }
        public override bool UseableWhenFrozen { get { return true; } }
        public override CommandAlias[] Aliases
        {
            get { return new[] { new CommandAlias("Host"), new CommandAlias("ZAll") }; }
        }
        public override CommandPerm[] ExtraPerms
        {
            get { return new[] { new CommandPerm(LevelPermission.Admin, "can see server CPU and memory usage") }; }
        }
        public string UpdateIntervalMessage()
        {
            if (Server.Config.PositionUpdateInterval <= 0)
            {
                return "  Player positions are updated &aconstantly&S.";
            }
            else
            {
                return "  Player positions are updated &a" +
                    (1000 / Server.Config.PositionUpdateInterval) + " &Stimes/second.";
            }
        }
        public override void Use(Player p, string message, CommandData data)
        {
            int count = Database.CountRows("Players");
            p.Message("About &b{0}&S", Server.Config.Name);
            p.Message("  &a{0} &Splayers total. (&a{1} &Sonline, &8{2} banned&S)",
                      count, PlayerInfo.GetOnlineCanSee(p, data.Rank).Count, Group.BannedRank.Players.Count);
            p.Message("  &a{0} &Slevels total (&a{1} &Sloaded). Currency is &3{2}&S.",
                      LevelInfo.AllMapFiles().Length, LevelInfo.Loaded.Count, Server.Config.Currency);
            Server.UpTime = DateTime.UtcNow - Server.StartTime;
            p.Message("  Been up for &a{0}&S, running &b{1} &av{2} &f" + Updater.SourceURL,
                      Server.UpTime.Shorten(true), Server.SoftwareName, Server.Version);
            p.Message(UpdateIntervalMessage());
            string owner = Server.Config.OwnerName;
            if (!owner.CaselessEq("Notch") && !owner.CaselessEq("the owner"))
            {
                p.Message("  Owner is &3{0}", owner);
            }

            if (HasExtraPerm(p, data.Rank, 1)) OutputResourceUsage(p);
            if (CheckPerms(p))
            {
                string User = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                p.Message("Running under {0}.", User);
            }
        }
        public static bool CheckPerms(Player p)
        {
#if CORE
            if (p.IsNull) return true;
#else
            if (p.IsFire) return true;
            else if (p.IsConsole) return true;
#endif

            if (Server.Config.OwnerName.CaselessEq("Notch")) return false;
            return p.name.CaselessEq(Server.Config.OwnerName);
        }

        public static DateTime startTime;
        public static ProcInfo startUsg;

        public static void OutputResourceUsage(Player p)
        {
            Process proc = Process.GetCurrentProcess();
            p.Message("Measuring resource usage...one second");
            IOperatingSystem os = IOperatingSystem.DetectOS();

            if (startTime == default)
            {
                startTime = DateTime.UtcNow;
                startUsg = os.MeasureResourceUsage(proc, false);
            }

            CPUTime allBeg = os.MeasureAllCPUTime();
            ProcInfo begUsg = os.MeasureResourceUsage(proc, false);

            // measure CPU usage over one second
            Thread.Sleep(1000);
            ProcInfo endUsg = os.MeasureResourceUsage(proc, true);
            CPUTime allEnd = os.MeasureAllCPUTime();

            p.Message("&a{0}% &SCPU usage now, &a{1}% &Soverall",
                MeasureCPU(begUsg.ProcessorTime, endUsg.ProcessorTime, TimeSpan.FromSeconds(1)),
                MeasureCPU(startUsg.ProcessorTime, endUsg.ProcessorTime, DateTime.UtcNow - startTime));

            ulong idl = allEnd.IdleTime - allBeg.IdleTime;
            ulong sys = allEnd.ProcessorTime - allBeg.ProcessorTime;
            double cpu = sys * 100.0 / (sys + idl);
            int cores = Environment.ProcessorCount;
            p.Message("  &a{0}% &Sby all processes across {1} CPU core{2}",
                double.IsNaN(cpu) ? "(unknown)" : cpu.ToString("F2"),
                cores, cores.Plural());

            // Private Bytes = memory the process has reserved just for itself
            int memory = (int)Math.Round(endUsg.PrivateMemorySize / 1048576.0);
            p.Message("&a{0} &Sthreads, using &a{1} &Smegabytes of memory",
                endUsg.NumThreads, memory);
            p.Message("Running a {0} operating system. ({1})", os.BitType, os.PlatformName);
            string User = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            p.Message("Running under {0}.", User);
        }

        public static string MeasureCPU(TimeSpan beg, TimeSpan end, TimeSpan interval)
        {
            if (end < beg) return "0.00"; // TODO: Can this ever happen
            int cores = Math.Max(1, Environment.ProcessorCount);

            TimeSpan used = end - beg;
            double elapsed = 100.0 * (used.TotalSeconds / interval.TotalSeconds);
            return (elapsed / cores).ToString("F2");
        }

        public override void Help(Player p)
        {
            p.Message("&T/ServerInfo");
            p.Message("&HDisplays the server information.");
        }
    }
}