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
using Flames.Eco;
using Flames.Games;

namespace Flames.Modules.Games.LS
{    
    public partial class LSGame : RoundsGame 
    {
        public static void HookItems() {
            Economy.RegisterItem(itemLife);
            Economy.RegisterItem(itemSponges);
            Economy.RegisterItem(itemWater);
            Economy.RegisterItem(itemDoors);
        }

        public static void UnhookItems() {
            Economy.Items.Remove(itemLife);
            Economy.Items.Remove(itemSponges);
            Economy.Items.Remove(itemWater);
            Economy.Items.Remove(itemDoors);
        }

        public static Item itemLife    = new LifeItem();
        public static Item itemSponges = new SpongesItem();
        public static Item itemWater   = new WaterItem();
        public static Item itemDoors   = new DoorsItem();


        public static void HookCommands() {
            Command.TryRegister(true, cmdLives);
        }

        public static void UnhookCommands() {
            Command.Unregister(cmdLives);
        }

        public static Command cmdLives = new CmdLives();
    }
}