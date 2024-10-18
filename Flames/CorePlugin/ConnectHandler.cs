/*
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
using Flames.Commands;

namespace Flames.Core
{
    public static class ConnectHandler
    {

        public static void HandleConnect(Player p)
        {
            CheckReviewList(p);
            if (p.CanUse("ReachDistance")) LoadReach(p);

            LoadWaypoints(p);
            p.Ignores.Load(p);
        }

        public static void CheckReviewList(Player p)
        {
            if (!p.CanUse("Review")) return;
            ItemPerms checkPerms = CommandExtraPerms.Find("Review", 1);
            if (!checkPerms.UsableBy(p)) return;

            int count = Server.reviewlist.Count;
            if (count == 0) return;

            string suffix = count == 1 ? " player is " : " players are ";
            p.Message(count + suffix + "waiting for a review. Type &T/Review view");
        }

        public static void LoadReach(Player p)
        {
            string reach = Server.reach.Get(p.name);
            if (string.IsNullOrEmpty(reach)) return;

            if (!short.TryParse(reach, out short reachDist)) return;

            p.ReachDistance = reachDist / 32f;
            p.Session.SendSetReach(p.ReachDistance);
        }

        public static void LoadWaypoints(Player p)
        {
            try
            {
                p.Waypoints.Filename = Paths.WaypointsDir + p.name + ".save";
                p.Waypoints.Load();
            }
            catch (Exception ex)
            {
                Logger.LogError("Error loading waypoints", ex);
            }
        }
    }
}