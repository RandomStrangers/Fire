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
using System.Threading;

namespace Flames.UI
{
    /// <summary> Common functionality for a CLI or GUI server console </summary>
    public static class UIHelpers
    {
        public static string lastCMD = "";
        public static void HandleChat(string text)
        {
            if (text != null) text = text.Trim();
            if (string.IsNullOrEmpty(text)) return;

            Player p = Player.Flame;
            if (ChatModes.Handle(p, text)) return;

            Chat.MessageChat(ChatScope.Global, p, "λFULL: &f" + text, null, null, true);
        }

        public static void RepeatCommand()
        {
            if (lastCMD.Length == 0)
            {
                Logger.Log(LogType.CommandUsage, "(Flames): Cannot repeat command - no commands used yet.");
                return;
            }
            Logger.Log(LogType.CommandUsage, "Repeating &T/" + lastCMD);
            HandleCommand(lastCMD);
        }

        public static void HandleCommand(string text)
        {
            if (text != null) text = text.Trim();
            if (string.IsNullOrEmpty(text))
            {
                Logger.Log(LogType.CommandUsage, "(Flames): Whitespace commands are not allowed.");
                return;
            }
            if (text[0] == '/' && text.Length > 1)
                text = text.Substring(1);

            lastCMD = text;
            string name, args;
            text.Separate(' ', out name, out args);

            Command.Search(ref name, ref args);
            if (Server.Check(name, args)) 
            { 
                Server.cancelcommand = false; 
                return; 
            }
            Command cmd = Command.Find(name);

            if (cmd == null)
            {
                Logger.Log(LogType.CommandUsage, "(Flames): Unknown command \"{0}\"", name); 
                return;
            }
            if (!cmd.SuperUseable)
            {
                Logger.Log(LogType.CommandUsage, "(Flames): /{0} can only be used in-game.", cmd.name); 
                return;
            }

            Thread thread = new Thread(
                () => {
                    try
                    {
                        cmd.Use(Player.Flame, args);
                        if (args.Length == 0)
                        {
                            Logger.Log(LogType.CommandUsage, "(Flames) used /" + cmd.name);
                        }
                        else
                        {
                            Logger.Log(LogType.CommandUsage, "(Flames) used /" + cmd.name + " " + args);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(ex);
                        Logger.Log(LogType.CommandUsage, "(Flames): FAILED COMMAND");
                    }
                });
            thread.Name = "FlamesCMD_" + name;
            thread.IsBackground = true;
            thread.Start();
        }

        public static string Format(string message)
        {
            message = message.Replace("%S", "&f"); // We want %S to be treated specially when displayed in UI
            message = Colors.Escape(message);      // Need to Replace first, otherwise it's mapped by Colors.Escape
            return message;
        }

        public static string OutputPart(ref char nextCol, ref int start, string message)
        {
            int next = NextPart(start, message);
            string part;
            if (next == -1)
            {
                part = message.Substring(start);
                start = message.Length;
            }
            else
            {
                part = message.Substring(start, next - start);
                start = next + 2;
                nextCol = message[next + 1];
            }
            return part;
        }

        public static int NextPart(int start, string message)
        {
            for (int i = start; i < message.Length; i++)
            {
                if (message[i] != '&') continue;
                // No colour code follows this
                if (i == message.Length - 1) return -1;

                // Check following character is an actual colour code
                char col = Colors.Lookup(message[i + 1]);
                if (col != '\0') return i;
            }
            return -1;
        }
    }
}