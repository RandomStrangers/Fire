/*
   Copyright 2015-2024 MCGalaxy

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
using System.ComponentModel;
using Flames.DB;
namespace Flames.Gui
{
    public class PlayerProperties
    {
        public Player P;
        public string InMsg, OutMsg;
        public PlayerProperties(Player player)
        {
            P = player;
            InMsg = PlayerDB.GetLoginMessage(player.name);
            OutMsg = PlayerDB.GetLogoutMessage(player.name);
        }
        [Category("Properties")]
        [DisplayName("Color")]
        [TypeConverter(typeof(ColorConverter))]
        public string Color 
        { 
            get 
            { 
                return Colors.Name(P.color); 
            } 
            set 
            { 
                DoOrd("Color", value); 
            } 
        }
        [Category("Properties")]
        [DisplayName("IP address")]
        public string IP 
        { 
            get 
            { 
                return P.ip; 
            } 
            set 
            { 
            } 
        }
        [Category("Properties")]
        [DisplayName("Login message")]
        public string LoginMsg 
        { 
            get 
            { 
                return InMsg; 
            } 
            set 
            { 
                InMsg = value;
                DoOrd("LoginMessage", value); 
            } 
        }
        [Category("Properties")]
        [DisplayName("Logout message")]
        public string LogoutMsg 
        { 
            get 
            { 
                return OutMsg; 
            } 
            set 
            { 
                OutMsg = value; 
                DoOrd("LogoutMessage", value); 
            } 
        }
        [Category("Properties")]
        [DisplayName("Rank")]
        [TypeConverter(typeof(RankConverter))]
        public string Rank 
        { 
            get 
            { 
                return P.group.Name; 
            } 
            set 
            { 
                DoOrd("SetRank", value); 
            } 
        }
        [Category("Properties")]
        [DisplayName("Title")]
        public string Title 
        { 
            get 
            { 
                return P.title; 
            } 
            set 
            { 
                DoOrd("Title", value); 
            } 
        }
        [Category("Properties")]
        [DisplayName("Title color")]
        [TypeConverter(typeof(ColorConverter))]
        public string TColor 
        { 
            get 
            { 
                return Colors.Name(P.titlecolor); 
            } 
            set 
            {
                DoOrd("TColor", value); 
            } 
        }
        [Category("Stats")]
        [DisplayName("Blocks modified")]
        public long BlocksModified 
        { 
            get 
            { 
                return P.TotalModified; 
            } 
            set 
            { 
                P.TotalModified = value; 
            } 
        }
        [Category("Stats")]
        [DisplayName("Number of deaths")]
        public int TimesDied 
        { 
            get 
            { 
                return P.TimesDied; 
            } 
            set 
            { 
                P.TimesDied = value; 
            } 
        }
        [Category("Stats")]
        [DisplayName("Times been kicked")]
        public int TimesKicked 
        { 
            get 
            { 
                return P.TimesBeenKicked; 
            } 
            set 
            { 
                P.TimesBeenKicked = value; 
            } 
        }
        [Category("Stats")]
        [DisplayName("Number of logins")]
        public int TimesLogins 
        { 
            get 
            { 
                return P.TimesVisited; 
            } 
            set 
            { 
                P.TimesVisited = value; 
            } 
        }
        [Category("Status")]
        [DisplayName("AFK")]
        public bool AFK 
        { 
            get 
            { 
                return P.IsAfk; 
            } 
            set 
            { 
                DoOrd("SendCmd", "afk"); 
            } 
        }
        [Category("Status")]
        [DisplayName("Hidden")]
        public bool Hidden 
        { 
            get 
            { 
                return P.hidden; 
            } 
            set 
            { 
                DoOrd("oHide"); 
            } 
        }
        [Category("Status")]
        [DisplayName("Jokered")]
        public bool Jokered 
        { 
            get 
            { 
                return P.joker; 
            } 
            set 
            { 
                DoOrd("Joker"); 
            } 
        }
        [Category("Status")]
        [DisplayName("Map")]
        [TypeConverter(typeof(LevelConverter))]
        public string Map 
        { 
            get 
            { 
                return P.level.name; 
            } 
            set 
            { 
                DoOrd("SendCmd", "goto " + value); 
            } 
        }
        [Category("Status")]
        [DisplayName("Voiced")]
        public bool Voiced 
        { 
            get 
            { 
                return P.voice; 
            } 
            set 
            { 
                DoOrd("Voice"); 
            } 
        }
        public void DoOrd(string ord) 
        { 
            DoOrd(ord, ""); 
        }
        public void DoOrd(string ord, string args)
        {
            // Is the player still on the server?
            Player pl = PlayerInfo.FindExact(P.name);
            if (pl == null)
            {
                return;
            }
            try
            {
                string ordArgs = args.Length == 0 ? P.name : P.name + " " + args;
                Order.Find(ord).Use(Player.Flame, ordArgs);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }
    }
}