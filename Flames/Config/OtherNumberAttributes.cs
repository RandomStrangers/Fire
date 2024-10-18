using System;
namespace Flames.Config
{
    public abstract class ConfigRealAttribute : ConfigAttribute
    {
        public ConfigRealAttribute(string name, string section) : base(name, section)
        {
        }

        public double ParseReal(string raw, double def, double min, double max)
        {
            if (!Utils.TryParseDouble(raw, out double value))
            {
                Logger.Log(LogType.Warning, "Config key \"{0}\" has invalid number '{2}', using default of {1}", Name, def, raw);
                value = def;
            }

            if (value < min)
            {
                Logger.Log(LogType.Warning, "Config key \"{0}\" is too small a number, using {1}", Name, min);
                value = min;
            }
            if (value > max)
            {
                Logger.Log(LogType.Warning, "Config key \"{0}\" is too big a number, using {1}", Name, max);
                value = max;
            }
            return value;
        }

        public override string Serialise(object value)
        {
            if (value is float) return Utils.StringifyDouble((float)value);
            if (value is double) return Utils.StringifyDouble((double)value);
            return base.Serialise(value);
        }
    }

    public class ConfigFloatAttribute : ConfigRealAttribute
    {
        public float defValue, minValue, maxValue;

        public ConfigFloatAttribute() : this(null, null, 0, float.NegativeInfinity, float.PositiveInfinity)
        {
        }
        public ConfigFloatAttribute(string name, string section, float def,
            float min = float.NegativeInfinity, float max = float.PositiveInfinity) : base(name, section)
        {
            defValue = def;
            minValue = min;
            maxValue = max;
        }

        public override object Parse(string raw)
        {
            return (float)ParseReal(raw, defValue, minValue, maxValue);
        }
    }

    public class ConfigTimespanAttribute : ConfigRealAttribute
    {
        public bool mins;
        public int def;
        public ConfigTimespanAttribute(string name, string section, int def, bool mins) : base(name, section)
        {
            this.def = def;
            this.mins = mins;
        }

        public override object Parse(string raw)
        {
            double value = ParseReal(raw, def, 0, int.MaxValue);
            return ParseInput(value);
        }

        public TimeSpan ParseInput(double value)
        {
            if (mins)
            {
                return TimeSpan.FromMinutes(value);
            }
            else
            {
                return TimeSpan.FromSeconds(value);
            }
        }

        public override string Serialise(object value)
        {
            TimeSpan span = (TimeSpan)value;
            double time = mins ? span.TotalMinutes : span.TotalSeconds;
            return time.ToString();
        }
    }

    public class ConfigOptTimespanAttribute : ConfigTimespanAttribute
    {
        public ConfigOptTimespanAttribute(string name, string section, bool mins) : base(name, section, -1, mins)
        {
        }

        public override object Parse(string raw)
        {
            if (string.IsNullOrEmpty(raw)) return null;

            double value = ParseReal(raw, -1, -1, int.MaxValue);
            if (value < 0) return null;

            return ParseInput(value);
        }

        public override string Serialise(object value)
        {
            if (value == null) return "";

            return base.Serialise(value);
        }
    }
}
