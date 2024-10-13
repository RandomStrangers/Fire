
using BlockID = System.UInt16;

namespace Flames.Config
{
    public abstract class ConfigUnsignedIntegerAttribute : ConfigAttribute
    {
        public ConfigUnsignedIntegerAttribute(string name, string section)
            : base(name, section) { }

        // separate function to avoid boxing in derived classes
        // Use ulong instead of uint to allow larger inputs
        public ulong ParseUnsignedLong(string raw, ulong def, ulong min, ulong max)
        {
            ulong value;
            if (!ulong.TryParse(raw, out value))
            {
                Logger.Log(LogType.Warning, "Config key \"{0}\" has invalid unsigned integer '{2}', using default of {1}", Name, def, raw);
                value = def;
            }

            if (value < min)
            {
                Logger.Log(LogType.Warning, "Config key \"{0}\" is too small an unsigned integer, using {1}", Name, min);
                value = min;
            }
            if (value > max)
            {
                Logger.Log(LogType.Warning, "Config key \"{0}\" is too big an unsigned integer, using {1}", Name, max);
                value = max;
            }
            return value;
        }
        public uint ParseUnsignedInteger(string raw, uint def, uint min, uint max)
        {
            uint value;
            if (!uint.TryParse(raw, out value))
            {
                Logger.Log(LogType.Warning, "Config key \"{0}\" has invalid unsigned integer '{2}', using default of {1}", Name, def, raw);
                value = def;
            }

            if (value < min)
            {
                Logger.Log(LogType.Warning, "Config key \"{0}\" is too small an unsigned integer, using {1}", Name, min);
                value = min;
            }
            if (value > max)
            {
                Logger.Log(LogType.Warning, "Config key \"{0}\" is too big an unsigned integer, using {1}", Name, max);
                value = max;
            }
            return value;
        }
    }
    public class ConfigSByteAttribute : ConfigSignedIntegerAttribute
    {
        public ConfigSByteAttribute() : this(null, null) { }
        public ConfigSByteAttribute(string name, string section) : base(name, section) { }

        public override object Parse(string raw)
        {
            return (sbyte)ParseInteger(raw, 0, 0, sbyte.MaxValue);
        }
    }

    public sealed class ConfigBlockAttribute : ConfigUnsignedIntegerAttribute
    {
        public BlockID defBlock;
        public ConfigBlockAttribute() : this(null, null, Block.Air) { }
        public ConfigBlockAttribute(string name, string section, BlockID def)
            : base(name, section) { defBlock = def; }

        public override object Parse(string raw)
        {
            BlockID block = (BlockID)ParseUnsignedInteger(raw, defBlock, 0, Block.SUPPORTED_COUNT - 1);
            if (block == Block.Invalid) return Block.Invalid;
            return Block.MapOldRaw(block);
        }
    }
    public class ConfigUShortAttribute : ConfigUnsignedIntegerAttribute
    {
        public ConfigUShortAttribute() : this(null, null) { }
        public ConfigUShortAttribute(string name, string section) : base(name, section) { }

        public override object Parse(string raw)
        {
            return (ushort)ParseUnsignedInteger(raw, 0, 0, ushort.MaxValue);
        }
    }
    public sealed class ConfigUIntAttribute : ConfigUnsignedIntegerAttribute
    {
        public uint defValue, minValue, maxValue;

        public ConfigUIntAttribute()
            : this(null, null, 0, uint.MinValue, uint.MaxValue) { }
        public ConfigUIntAttribute(string name, string section, uint def,
                                  uint min = uint.MinValue, uint max = uint.MaxValue)
            : base(name, section) { defValue = def; minValue = min; maxValue = max; }

        public override object Parse(string value)
        {
            return ParseUnsignedInteger(value, defValue, minValue, maxValue);
        }
    }
}
