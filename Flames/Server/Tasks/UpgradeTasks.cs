﻿/*
    Copyright 2015 MCGalaxy
    
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
using System.IO;
using Flames.SQL;

namespace Flames.Tasks
{
    public static class UpgradeTasks
    {

        public static void UpgradeOldAgreed()
        {
            // agreed.txt format used to be names separated by spaces, we need to fix that up.
            if (!File.Exists("ranks/agreed.txt")) return;

            string data = null;
            using (FileStream fs = File.OpenRead("ranks/agreed.txt"))
            {
                if (fs.ReadByte() != ' ') return;
                data = new StreamReader(fs).ReadToEnd();
                data = data.Replace(" ", Environment.NewLine);
            }
            File.WriteAllText("ranks/agreed.txt", data);
        }

        public static void UpgradeOldTempranks(SchedulerTask task)
        {
            if (!File.Exists(Paths.TempRanksFile)) return;

            // Check if empty, or not old form
            using (StreamReader r = new StreamReader(Paths.TempRanksFile))
            {
                string line = r.ReadLine();
                if (line == null) return;
                string[] parts = line.SplitSpaces();
                if (parts.Length < 9) return;
            }

            string[] lines = File.ReadAllLines(Paths.TempRanksFile);
            for (int i = 0; i < lines.Length; i++)
            {
                string[] args = lines[i].SplitSpaces();
                if (args.Length < 9) continue;

                int min = int.Parse(args[4]), hour = int.Parse(args[5]);
                int day = int.Parse(args[6]), month = int.Parse(args[7]), year = int.Parse(args[8]);
                int periodH = int.Parse(args[3]), periodM = 0;
                if (args.Length > 10) periodM = int.Parse(args[10]);

                DateTime assigned = new DateTime(year, month, day, hour, min, 0);
                DateTime expiry = assigned.AddHours(periodH).AddMinutes(periodM);

                // Line format: name assigner assigntime expiretime oldRank tempRank
                lines[i] = args[0] + " " + args[9] + " " + assigned.ToUnixTime() +
                    " " + expiry.ToUnixTime() + " " + args[2] + " " + args[1];
            }
            File.WriteAllLines(Paths.TempRanksFile, lines);
        }


        public static void UpgradeDBTimeSpent(SchedulerTask task)
        {
            string time = Database.ReadString("Players", "TimeSpent", "LIMIT 1");
            if (time == null) return; // no players at all in DB
            if (time.IndexOf(' ') == -1) return; // already upgraded

            Logger.Log(LogType.SystemActivity, "Upgrading TimeSpent column in database to new format..");
            DumpPlayerTimeSpents();
            UpgradePlayerTimeSpents();
            Logger.Log(LogType.SystemActivity, "Upgraded {0} rows. ({1} rows failed)", playerCount, playerFailed);
        }

        public static List<int> playerIds;
        public static List<long> playerSeconds;
        public static int playerCount, playerFailed = 0;

        public static void DumpPlayerTimeSpents()
        {
            playerIds = new List<int>();
            playerSeconds = new List<long>();
            Database.ReadRows("Players", "ID,TimeSpent", ReadTimeSpent);
        }

        public static void ReadTimeSpent(ISqlRecord record)
        {
            playerCount++;
            try
            {
                int id = record.GetInt32(0);
                TimeSpan span = Database.ParseOldDBTimeSpent(record.GetString(1));

                playerIds.Add(id);
                playerSeconds.Add((long)span.TotalSeconds);
            }
            catch
            {
                playerFailed++;
            }
        }

        public static void UpgradePlayerTimeSpents()
        {
            using (SqlTransaction bulk = new SqlTransaction())
            {
                for (int i = 0; i < playerIds.Count; i++)
                {
                    bulk.Execute("UPDATE Players SET TimeSpent=@1 WHERE ID=@0",
                                 playerIds[i], playerSeconds[i]);
                }
                bulk.Commit();
            }
        }
    }
}