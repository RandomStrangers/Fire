/*
    Copyright 2011 MCForge
        
    Dual-licensed under the Educational Community License, Version 2.0 and
    the GNU General Public License, Version 3 (the "Licenses"); you may
    not use this file except in compliance with the Licenses. You may
    obtain a copy of the Licenses at
    
    http://www.opensource.org/licenses/ecl2.php
    http://www.gnu.org/licenses/gpl-3.0.html
    
    Unless required by applicable law or agreed to in writing,
    software distributed under the Licenses are distributed on an "AS IS"
    BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
    or implied. See the Licenses for the specific language governing
    permissions and limitations under the Licenses.
 */
using Flames.Commands.Chatting;
using Flames.Maths;

namespace Flames.Core
{

    public static class ChatHandler
    {

        public static void HandleOnChat(ChatScope scope, Player source, string msg,
                                          object arg, ref ChatMessageFilter filter, bool irc)
        {
            msg = msg.Replace("λFULL", source.name).Replace("λNICK", source.name);
            LogType logType = LogType.PlayerChat;

            if (scope == ChatScope.Perms)
            {
                logType = LogType.StaffChat;
            }
            else if (scope == ChatScope.Rank)
            {
                logType = LogType.RankChat;
            }

            if (scope != ChatScope.PM) Logger.Log(logType, msg);
        }
        // Need to find a better way to do this
        public static void HandleCommand(Player p, string cmd, string args, CommandData data)
        {
            // Really clunky design, but it works
            Level lvl = p.level;
            Command command = Command.Find(cmd);
            if (command != null)
            {
                bool IsDrawingCmd = command.type.CaselessEq(CommandTypes.Building);

                if (IsDrawingCmd && !lvl.Config.Drawing)
                {
                    p.Message("Drawing commands are turned off on this map.");
                    p.cancelcommand = true;
                    Vec3S32 pos = p.Pos.BlockCoords;
                    p.RevertBlock((ushort)pos.X, (ushort)pos.Y, (ushort)pos.Z);
                }
            }
            if (!Server.Config.CoreSecretCommands) return;
            // DO NOT REMOVE THE TWO COMMANDS BELOW, /PONY AND /RAINBOWDASHLIKESCOOLTHINGS. -EricKilla
            if (cmd.ToLower() == "pony")
            {
                p.cancelcommand = true;
                if (!MessageCmd.CanSpeak(p, cmd)) return;
                int used = p.Extras.GetInt("F_PONY");

                if (used < 2)
                {
                    Chat.MessageFrom(p, "λNICK &Sjust so happens to be a proud brony! Everyone give λNICK &Sa brohoof!");
                    Logger.Log(LogType.CommandUsage, "{0} used /{1}", p.name, cmd);
                }
                else
                {
                    p.Message("You have used this command 2 times. You cannot use it anymore! Sorry, Brony!");
                }

                p.Extras["F_PONY"] = used + 1;
            }
            else if (cmd.ToLower() == "rainbowdashlikescoolthings")
            {
                p.cancelcommand = true;
                if (!MessageCmd.CanSpeak(p, cmd)) return;
                int used = p.Extras.GetInt("F_RD");

                if (used < 2)
                {
                    Chat.MessageGlobal("&4T&6H&eI&aS&3 S&9E&1R&4V&6E&eR &aJ&3U&9S&1T &4G&6O&eT &a2&30 &9P&1E&4R&6C&eE&aN&3T &9C&1O&4O&6L&eE&aR&3!");
                    Logger.Log(LogType.CommandUsage, "{0} used /{1}", p.name, cmd);
                }
                else
                {
                    p.Message("You have used this command 2 times. You cannot use it anymore! Sorry, Brony!");
                }

                p.Extras["F_RD"] = used + 1;
            }
            if (!Server.Config.MCLawlSecretCommands) return;
            if (cmd.ToLower() == "care")
            {
                p.cancelcommand = true;
                int used = p.Extras.GetInt("F_CARE");

                if (used < 2)
                {
                    Chat.MessageFrom(p, "λNICK is now loved by Harmony with all her heart.");
                    p.Message("Harmony now loves you with all her heart. ");
                    Logger.Log(LogType.CommandUsage, "{0} used /{1}", p.name, cmd);
                }
                else
                {
                    p.Message("You have used this command 2 times. You cannot use it anymore!");
                }

                p.Extras["F_CARE"] = used + 1;
            }
            else if (cmd.ToLower() == "facepalm")
            {
                p.cancelcommand = true;
                int used = p.Extras.GetInt("F_FACEPALM");

                if (used < 2)
                {
                    p.Message("Harmony's bot army just simultaneously facepalm'd at your use of this command.");
                    Logger.Log(LogType.CommandUsage, "{0} used /{1}", p.name, cmd);
                }
                else
                {
                    p.Message("You have used this command 2 times. You cannot use it anymore!");
                }

                p.Extras["F_FACEPALM"] = used + 1;
            }
        }
    }
}