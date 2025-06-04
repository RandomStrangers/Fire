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
using System.Collections.Generic;
using Flames.Blocks;
using Flames.Maths;
using Flames.Added;
namespace Flames.Commands
{
    /// <summary> Provides helper methods for parsing arguments for commands. </summary>
    public static class CommandParser
    {
        /// <summary> Attempts to parse the given argument as a boolean. </summary>
        public static bool GetBool(Player p, string input, ref bool result)
        {
            return OrderParser.GetBool(p, input, ref result);
        }

        /// <summary> Attempts to parse the given argument as an enumeration member. </summary>
        public static bool GetEnum<TEnum>(Player p, string input, string argName,
                                          ref TEnum result) where TEnum : struct
        {
            return OrderParser.GetEnum(p, input, argName, ref result);
        }

        /// <summary> Attempts to parse the given argument as an timespan in short form. </summary>
        public static bool GetTimespan(Player p, string input, ref TimeSpan span,
                                       string action, string defUnit)
        {
            return OrderParser.GetTimespan(p, input, ref span, action, defUnit);
        }
        public const string TimespanHelp = "For example, to {0} 25 and a half hours, use \"1d1h30m\".";
        /// <summary> Returns whether the given value lies within the given range </summary>
        /// <remarks> If given value is not in range, messages the player the valid range of values </remarks>
        public static bool CheckRange(Player p, int value, string argName, int min, int max)
        {
            return OrderParser.CheckRange(p, value, argName, min, max);
        }

        /// <summary> Attempts to parse the given argument as an integer. </summary>
        public static bool GetInt(Player p, string input, string argName, ref int result,
                                  int min = int.MinValue, int max = int.MaxValue)
        {
            return OrderParser.GetInt(p, input, argName, ref result, min, max);
        }

        /// <summary> Attempts to parse the given argument as a real number. </summary>
        public static bool GetReal(Player p, string input, string argName, ref float result,
                                   float min = float.NegativeInfinity, float max = float.MaxValue)
        {
            return OrderParser.GetReal(p, input, argName, ref result, min, max);
        }


        /// <summary> Attempts to parse the given argument as an byte. </summary>
        public static bool GetByte(Player p, string input, string argName, ref byte result,
                                   byte min = byte.MinValue, byte max = byte.MaxValue)
        {
            return OrderParser.GetByte(p, input, argName, ref result, min, max);
        }

        /// <summary> Attempts to parse the given argument as a ushort. </summary>
        public static bool GetUShort(Player p, string input, string argName, ref ushort result,
                                     ushort min = ushort.MinValue, ushort max = ushort.MaxValue)
        {
            return OrderParser.GetUShort(p, input, argName, ref result, min, max);
        }


        /// <summary> Attempts to parse the given argument as a hex color. </summary>
        public static bool GetHex(Player p, string input, ref ColorDesc col)
        {
            return OrderParser.GetHex(p, input, ref col);
        }

        /// <summary> Attempts to parse the 3 given arguments as coordinates. </summary>
        public static bool GetCoords(Player p, string[] args, int argsOffset, ref Vec3S32 P)
        {
            return OrderParser.GetCoords(p, args, argsOffset, ref P);
        }

        public static bool ParseRelative(ref string arg)
        {
            return OrderParser.ParseRelative(ref arg);
        }

        /// <summary> Attempts to parse the given argument as a coordinate integer. </summary>
        public static bool GetCoordInt(Player p, string arg, string argName, ref int value)
        {
           return OrderParser.GetCoordInt(p, arg, argName, ref value);
        }

        /// <summary> Attempts to parse the given argument as a coordinate real number. </summary>
        public static bool GetCoordFloat(Player p, string arg, string argName, ref float value)
        {
            return OrderParser.GetCoordFloat(p, arg, argName, ref value);
        }


        public static bool IsSkipBlock(string input, out ushort block)
        {
            return OrderParser.IsSkipBlock(input, out block);
        }

        /// <summary> Attempts to parse the given argument as either a block name or a block ID. </summary>
        /// <remarks> Also ensures the player is allowed to place the given block. </remarks>
        public static bool GetBlockIfAllowed(Player p, string input, string action,
                                             out ushort block, bool allowSkip = false)
        {
           return OrderParser.GetBlockIfAllowed(p, input, action, out block, allowSkip);
        }

        /// <summary> Attempts to parse the given argument as either a block name or a block ID. </summary>
        public static bool GetBlock(Player p, string input, out ushort block, bool allowSkip = false)
        {
            return OrderParser.GetBlock(p, input, out block, allowSkip);
        }

        /// <summary> Returns whether the player is allowed to place/modify/delete the given block. </summary>
        /// <remarks> Outputs information of which ranks can modify the block if not. </remarks>
        public static bool IsBlockAllowed(Player p, string action, ushort block)
        {
            return OrderParser.IsBlockAllowed(p, action, block);
        }


        public static int GetBlocks(Player p, string input,
                                    List<ushort> blocks, bool allowSkip)
        {
            return OrderParser.GetBlocks(p, input, blocks, allowSkip);
        }

        public static bool IsRawBlockRange(string input, out string[] bits)
        {
            return OrderParser.IsRawBlockRange(input, out bits);
        }
    }
}