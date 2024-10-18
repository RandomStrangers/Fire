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

namespace Flames.Config
{
    public abstract class ConfigSignedIntegerAttribute : ConfigAttribute
    {
        public ConfigSignedIntegerAttribute(string name, string section) : base(name, section) 
        { 
        }

        // separate function to avoid boxing in derived classes
        public long ParseLong(string raw, long def, long min, long max)
        {
            if (!long.TryParse(raw, out long value))
            {
                Logger.Log(LogType.Warning, "Config key \"{0}\" has invalid integer '{2}', using default of {1}", Name, def, raw);
                value = def;
            }

            if (value < min)
            {
                Logger.Log(LogType.Warning, "Config key \"{0}\" is too small an integer, using {1}", Name, min);
                value = min;
            }
            if (value > max)
            {
                Logger.Log(LogType.Warning, "Config key \"{0}\" is too big an integer, using {1}", Name, max);
                value = max;
            }
            return value;
        }
        public int ParseInteger(string raw, int def, int min, int max)
        {
            if (!int.TryParse(raw, out int value))
            {
                Logger.Log(LogType.Warning, "Config key \"{0}\" has invalid integer '{2}', using default of {1}", Name, def, raw);
                value = def;
            }

            if (value < min)
            {
                Logger.Log(LogType.Warning, "Config key \"{0}\" is too small an integer, using {1}", Name, min);
                value = min;
            }
            if (value > max)
            {
                Logger.Log(LogType.Warning, "Config key \"{0}\" is too big an integer, using {1}", Name, max);
                value = max;
            }
            return value;
        }
    }

    public sealed class ConfigIntAttribute : ConfigSignedIntegerAttribute
    {
        public int defValue, minValue, maxValue;

        public ConfigIntAttribute() : this(null, null, 0, int.MinValue, int.MaxValue) 
        { 
        }
        public ConfigIntAttribute(string name, string section, int def,
                                  int min = int.MinValue, int max = int.MaxValue)  : base(name, section) 
        { 
            defValue = def; 
            minValue = min; 
            maxValue = max; 
        }

        public override object Parse(string value)
        {
            return ParseInteger(value, defValue, minValue, maxValue);
        }
    }
    public class ConfigSByteAttribute : ConfigSignedIntegerAttribute
    {
        public ConfigSByteAttribute() : this(null, null) 
        { 
        }
        public ConfigSByteAttribute(string name, string section) : base(name, section) 
        { 
        }

        public override object Parse(string raw)
        {
            return (sbyte)ParseInteger(raw, 0, 0, sbyte.MaxValue);
        }
    }
    public class ConfigShortAttribute : ConfigSignedIntegerAttribute
    {
        public ConfigShortAttribute() : this(null, null) 
        { 
        }
        public ConfigShortAttribute(string name, string section) : base(name, section) 
        { 
        }

        public override object Parse(string raw)
        {
            return (short)ParseInteger(raw, 0, 0, short.MaxValue);
        }
    }
}