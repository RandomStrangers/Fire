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

namespace Flames.Maths
{
       /// <summary> 3 component vector (unsigned 32 bit integer) </summary>
    public struct Vec3U32 : IEquatable<Vec3U32>
    {
        public uint X, Y, Z;
        public static Vec3U32 Zero = new Vec3U32(0);
        public static Vec3U32 MinVal = new Vec3U32(uint.MinValue);
        public static Vec3U32 MaxVal = new Vec3U32(uint.MaxValue);

        public Vec3U32(uint x, uint y, uint z)
        {
            X = x; 
            Y = y;
            Z = z;
        }

        public Vec3U32(uint value)
        {
            X = value; 
            Y = value; 
            Z = value;
        }

        public static explicit operator Vec3U32(Vec3S64 a)
        {
            return new Vec3U32((uint)a.X, (uint)a.Y, (uint)a.Z);
        }
        public static explicit operator Vec3U32(Vec3U64 a)
        {
            return new Vec3U32((uint)a.X, (uint)a.Y, (uint)a.Z);
        }
        public static explicit operator Vec3U32(Vec3S32 a)
        {
            return new Vec3U32((uint)a.X, (uint)a.Y, (uint)a.Z);
        }
        public static explicit operator Vec3U32(Vec3S16 a)
        {
            return new Vec3U32((uint)a.X, (uint)a.Y, (uint)a.Z);
        }
        public static explicit operator Vec3U32(Vec3U16 a)
        {
            return new Vec3U32(a.X, a.Y, a.Z);
        }
        public static explicit operator Vec3U32(Vec3S8 a)
        {
            return new Vec3U32((uint)a.X, (uint)a.Y, (uint)a.Z);
        }
        public static explicit operator Vec3U32(Vec3U8 a)
        {
            return new Vec3U32(a.X, a.Y, a.Z);
        }
        public uint LengthSquared { get { return X * X + Y * Y + Z * Z; } }

        public float Length { get { return (float)Math.Sqrt(X * X + Y * Y + Z * Z); } }

        public override bool Equals(object obj)
        {
            return (obj is Vec3U32) && Equals((Vec3U32)obj);
        }

        public bool Equals(Vec3U32 other)
        {
            return X == other.X & Y == other.Y && Z == other.Z;
        }

        public override int GetHashCode()
        {
            uint hashCode = 0;
            hashCode += 1000000007 * X;
            hashCode += 1000000009 * Y;
            hashCode += 1000000021 * Z;
            return (int)hashCode;
        }

        public static bool operator ==(Vec3U32 a, Vec3U32 b)
        {
            return a.X == b.X && a.Y == b.Y && a.Z == b.Z;
        }

        public static bool operator !=(Vec3U32 a, Vec3U32 b)
        {
            return a.X != b.X || a.Y != b.Y || a.Z != b.Z;
        }

        public override string ToString() 
        { 
            return X + ", " + Y + ", " + Z; 
        }
    }
    /// <summary> 3 component vector (unsigned 64 bit integer) </summary>
    public struct Vec3U64 : IEquatable<Vec3U64>
    {
        public ulong X, Y, Z;
        public static Vec3U64 Zero = new Vec3U64(0);
        public static Vec3U64 MinVal = new Vec3U64(ulong.MinValue);
        public static Vec3U64 MaxVal = new Vec3U64(ulong.MaxValue);

        public Vec3U64(ulong x, ulong y, ulong z)
        {
            X = x; 
            Y = y;
            Z = z;
        }

        public Vec3U64(ulong value)
        {
            X = value; 
            Y = value; 
            Z = value;
        }

        public static explicit operator Vec3U64(Vec3S64 a)
        {
            return new Vec3U64((ulong)a.X, (ulong)a.Y, (ulong)a.Z);
        }
        public static explicit operator Vec3U64(Vec3S32 a)
        {
            return new Vec3U64((ulong)a.X, (ulong)a.Y, (ulong)a.Z);
        }
        public static explicit operator Vec3U64(Vec3U32 a)
        {
            return new Vec3U64(a.X, a.Y, a.Z);
        }
        public static explicit operator Vec3U64(Vec3S16 a)
        {
            return new Vec3U64((ulong)a.X, (ulong)a.Y, (ulong)a.Z);
        }
        public static explicit operator Vec3U64(Vec3U16 a)
        {
            return new Vec3U64(a.X, a.Y, a.Z);
        }
        public static explicit operator Vec3U64(Vec3S8 a)
        {
            return new Vec3U64(a.X, a.Y, a.Z);
        }
        public static explicit operator Vec3U64(Vec3U8 a)
        {
            return new Vec3U64(a.X, a.Y, a.Z);
        }
        public ulong LengthSquared { get { return X * X + Y * Y + Z * Z; } }

        public float Length { get { return (float)Math.Sqrt(X * X + Y * Y + Z * Z); } }

        public override bool Equals(object obj)
        {
            return (obj is Vec3U64) && Equals((Vec3U64)obj);
        }

        public bool Equals(Vec3U64 other)
        {
            return X == other.X & Y == other.Y && Z == other.Z;
        }

        public override int GetHashCode()
        {
            ulong hashCode = 0;
            hashCode += 1000000007 * X;
            hashCode += 1000000009 * Y;
            hashCode += 1000000021 * Z;
            return (int)hashCode;
        }

        public static bool operator ==(Vec3U64 a, Vec3U64 b)
        {
            return a.X == b.X && a.Y == b.Y && a.Z == b.Z;
        }

        public static bool operator !=(Vec3U64 a, Vec3U64 b)
        {
            return a.X != b.X || a.Y != b.Y || a.Z != b.Z;
        }

        public override string ToString() 
        { 
            return X + ", " + Y + ", " + Z; 
        }
    }

       /// <summary> 3 component vector (signed 8 bit integer) </summary>
    public struct Vec3S8 : IEquatable<Vec3S8>
    {
        public sbyte X, Y, Z;
        public static Vec3S8 Zero = new Vec3S8(0);

        public Vec3S8(sbyte x, sbyte y, sbyte z)
        {
            X = x; 
            Y = y; 
            Z = z;
        }

        public Vec3S8(sbyte value)
        {
            X = value; 
            Y = value; 
            Z = value;
        }


        public sbyte LengthSquared { get { return X * X + Y * Y + Z * Z; } }

        public float Length { get { return (float)Math.Sqrt(X * X + Y * Y + Z * Z); } }


        public sbyte this[sbyte index]
        {
            get
            {
                if (index == 0) 
                { 
                    return X; 
                }
                else if (index == 1)
                { 
                    return Y; 
                }
                else 
                { 
                    return Z; 
                }
            }
            set
            {
                if (index == 0) 
                {
                    X = value; 
                }
                else if (index == 1) 
                { 
                    Y = value; 
                }
                else 
                { 
                    Z = value; 
                }
            }
        }


        public static Vec3S8 Max(Vec3S8 a, Vec3S8 b)
        {
            return new Vec3S8((sbyte)Math.Max(a.X, b.X), (sbyte)Math.Max(a.Y, b.Y), (sbyte)Math.Max(a.Z, b.Z));
        }

        public static Vec3S8 Min(Vec3S8 a, Vec3S8 b)
        {
            return new Vec3S8((sbyte)Math.Min(a.X, b.X), (sbyte)Math.Min(a.Y, b.Y), (sbyte)Math.Min(a.Z, b.Z));
        }
        public static implicit operator Vec3S8(Vec3S64 a)
        {
            return new Vec3S8((sbyte)a.X, (sbyte)a.Y, (sbyte_a.Z);
        }
        public static implicit operator Vec3S8(Vec3U64 a)
        {
            return new Vec3S8((sbyte)a.X, (sbyte)a.Y, (sbyte)a.Z);
        }
        public static implicit operator Vec3S8(Vec3S32 a)
        {
            return new Vec3S8((sbyte)a.X, (sbyte)a.Y, (sbyte)a.Z);
        }
        public static implicit operator Vec3S8(Vec3U32 a)
        {
            return new Vec3S8((sbyte)a.X, (sbyte)a.Y, (sbyte)a.Z);
        }
        public static implicit operator Vec3S8(Vec3S16 a)
        {
            return new Vec3S8((sbyte)a.X, (sbyte)a.Y, (sbyte_a.Z);
        }
        public static implicit operator Vec3S8(Vec3U16 a)
        {
            return new Vec3S8((sbyte)a.X, (sbyte)a.Y, (sbyte_a.Z);
        }
        public static implicit operator Vec3S8(Vec3U8 a)
        {
            return new Vec3S8((sbyte)a.X, (sbyte)a.Y, (sbyte)a.Z);
        }

        public static Vec3S8 operator +(Vec3S8 a, Vec3S8 b)
        {
            return new Vec3S8(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static Vec3S8 operator -(Vec3S8 a, Vec3S8 b)
        {
            return new Vec3S8(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static Vec3S8 operator *(Vec3S8 a, sbyte b)
        {
            return new Vec3S8(a.X * b, a.Y * b, a.Z * b);
        }

        public static Vec3S8 operator /(Vec3S8 a, sbyte b)
        {
            return new Vec3S8(a.X / b, a.Y / b, a.Z / b);
        }

        public static Vec3S8 operator *(Vec3S8 a, float b)
        {
            return new Vec3S8((a.X * b), (a.Y * b), (a.Z * b));
        }


        public override bool Equals(object obj)
        {
            return (obj is Vec3S64) && Equals((Vec3S8)obj);
        }

        public bool Equals(Vec3S8 other)
        {
            return X == other.X & Y == other.Y && Z == other.Z;
        }

        public override int GetHashCode()
        {
            sbyte hashCode = 0;
            hashCode += 1000000007 * X;
            hashCode += 1000000009 * Y;
            hashCode += 1000000021 * Z;
            return hashCode;
        }

        public static bool operator ==(Vec3S8 a, Vec3S8 b)
        {
            return a.X == b.X && a.Y == b.Y && a.Z == b.Z;
        }

        public static bool operator !=(Vec3S8 a, Vec3S8 b)
        {
            return a.X != b.X || a.Y != b.Y || a.Z != b.Z;
        }

        public override string ToString()
        {
            return X + ", " + Y + ", " + Z;
        }
    }
    
    /// <summary> 3 component vector (signed 64 bit integer) </summary>
    public struct Vec3S64 : IEquatable<Vec3S64>
    {
        public long X, Y, Z;
        public static Vec3S64 Zero = new Vec3S64(0);

        public Vec3S64(long x, long y, long z)
        {
            X = x; 
            Y = y; 
            Z = z;
        }

        public Vec3S64(long value)
        {
            X = value; 
            Y = value; 
            Z = value;
        }


        public long LengthSquared { get { return X * X + Y * Y + Z * Z; } }

        public float Length { get { return (float)Math.Sqrt(X * X + Y * Y + Z * Z); } }


        public long this[long index]
        {
            get
            {
                if (index == 0) 
                { 
                    return X; 
                }
                else if (index == 1)
                { 
                    return Y; 
                }
                else 
                { 
                    return Z; 
                }
            }
            set
            {
                if (index == 0) 
                {
                    X = value; 
                }
                else if (index == 1) 
                { 
                    Y = value; 
                }
                else 
                { 
                    Z = value; 
                }
            }
        }


        public static Vec3S64 Max(Vec3S64 a, Vec3S64 b)
        {
            return new Vec3S64(Math.Max(a.X, b.X), Math.Max(a.Y, b.Y), Math.Max(a.Z, b.Z));
        }

        public static Vec3S64 Min(Vec3S64 a, Vec3S64 b)
        {
            return new Vec3S64(Math.Min(a.X, b.X), Math.Min(a.Y, b.Y), Math.Min(a.Z, b.Z));
        }
        public static implicit operator Vec3S64(Vec3U64 a)
        {
            return new Vec3S64((long)a.X, (long)a.Y, (long)a.Z);
        }
        public static implicit operator Vec3S64(Vec3S32 a)
        {
            return new Vec3S64(a.X, a.Y, a.Z);
        }
        public static implicit operator Vec3S64(Vec3U32 a)
        {
            return new Vec3S64(a.X, a.Y, a.Z);
        }
        public static implicit operator Vec3S64(Vec3S16 a)
        {
            return new Vec3S64(a.X, a.Y, a.Z);
        }
        public static implicit operator Vec3S64(Vec3U16 a)
        {
            return new Vec3S64(a.X, a.Y, a.Z);
        }
        public static implicit operator Vec3S64(Vec3S8 a)
        {
            return new Vec3S64(a.X, a.Y, a.Z);
        }
        public static implicit operator Vec3S64(Vec3U8 a)
        {
            return new Vec3S64(a.X, a.Y, a.Z);
        }

        public static Vec3S64 operator +(Vec3S64 a, Vec3S64 b)
        {
            return new Vec3S64(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static Vec3S64 operator -(Vec3S64 a, Vec3S64 b)
        {
            return new Vec3S64(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static Vec3S64 operator *(Vec3S64 a, long b)
        {
            return new Vec3S64(a.X * b, a.Y * b, a.Z * b);
        }

        public static Vec3S64 operator /(Vec3S64 a, long b)
        {
            return new Vec3S64(a.X / b, a.Y / b, a.Z / b);
        }

        public static Vec3S64 operator *(Vec3S64 a, float b)
        {
            return new Vec3S64((long)(a.X * b), (long)(a.Y * b), (long)(a.Z * b));
        }


        public override bool Equals(object obj)
        {
            return (obj is Vec3S64) && Equals((Vec3S64)obj);
        }

        public bool Equals(Vec3S64 other)
        {
            return X == other.X & Y == other.Y && Z == other.Z;
        }

        public override int GetHashCode()
        {
            long hashCode = 0;
            hashCode += 1000000007 * X;
            hashCode += 1000000009 * Y;
            hashCode += 1000000021 * Z;
            return (int)hashCode;
        }

        public static bool operator ==(Vec3S64 a, Vec3S64 b)
        {
            return a.X == b.X && a.Y == b.Y && a.Z == b.Z;
        }

        public static bool operator !=(Vec3S64 a, Vec3S64 b)
        {
            return a.X != b.X || a.Y != b.Y || a.Z != b.Z;
        }

        public override string ToString()
        {
            return X + ", " + Y + ", " + Z;
        }
    }
    /// <summary> 3 component vector (unsigned 8 bit integer) </summary>
    public struct Vec3U8 : IEquatable<Vec3U8>
    {
        public byte X, Y, Z;
        public static Vec3U8 Zero = new Vec3U8(0);
        public static Vec3U8 MinVal = new Vec3U8(byte.MinValue);
        public static Vec3U8 MaxVal = new Vec3U8(byte.MaxValue);

        public Vec3U8(byte x, byte y, byte z)
        {
            X = x; 
            Y = y;
            Z = z;
        }

        public Vec3U8(byte value)
        {
            X = value; 
            Y = value; 
            Z = value;
        }
        public static explicit operator Vec3U8(Vec3S64 a)
        {
            return new Vec3U8((byte)a.X, (byte)a.Y, (byte)a.Z);
        }
        public static explicit operator Vec3U8(Vec3U64 a)
        {
            return new Vec3U8((byte)a.X, (byte)a.Y, (byte)a.Z);
        }
        public static explicit operator Vec3U8(Vec3S32 a)
        {
            return new Vec3U8((byte)a.X, (byte)a.Y, (byte)a.Z);
        }
        public static explicit operator Vec3U8(Vec3U32 a)
        {
            return new Vec3U8((byte)a.X, (byte)a.Y, (byte)a.Z);
        }
        public static explicit operator Vec3U8(Vec3S16 a)
        {
            return new Vec3U8((byte)a.X, (byte)a.Y, (byte)a.Z);
        }
        public static explicit operator Vec3U8(Vec3U16 a)
        {
            return new Vec3U8((byte)a.X, (byte)a.Y, (byte)a.Z);
        }
        public static explicit operator Vec3U8(Vec3S8 a)
        {
            return new Vec3U8((byte)a.X, (byte)a.Y, (byte)a.Z);
        }

        public byte LengthSquared { get { return X * X + Y * Y + Z * Z; } }

        public float Length { get { return (float)Math.Sqrt(X * X + Y * Y + Z * Z); } }

        public override bool Equals(object obj)
        {
            return (obj is Vec3U8) && Equals((Vec3U8)obj);
        }

        public bool Equals(Vec3U8 other)
        {
            return X == other.X & Y == other.Y && Z == other.Z;
        }

        public override int GetHashCode()
        {
            int hashCode = 0;
            hashCode += 1000000007 * X;
            hashCode += 1000000009 * Y;
            hashCode += 1000000021 * Z;
            return hashCode;
        }

        public static bool operator ==(Vec3U8 a, Vec3U8 b)
        {
            return a.X == b.X && a.Y == b.Y && a.Z == b.Z;
        }

        public static bool operator !=(Vec3U8 a, Vec3U8 b)
        {
            return a.X != b.X || a.Y != b.Y || a.Z != b.Z;
        }

        public override string ToString() 
        { 
            return X + ", " + Y + ", " + Z; 
        }
    }
    /// <summary> 3 component vector (unsigned 16 bit integer) </summary>
    public struct Vec3U16 : IEquatable<Vec3U16>
    {
        public ushort X, Y, Z;
        public static Vec3U16 Zero = new Vec3U16(0);
        public static Vec3U16 MinVal = new Vec3U16(ushort.MinValue);
        public static Vec3U16 MaxVal = new Vec3U16(ushort.MaxValue);

        public Vec3U16(ushort x, ushort y, ushort z)
        {
            X = x; 
            Y = y;
            Z = z;
        }

        public Vec3U16(ushort value)
        {
            X = value; 
            Y = value; 
            Z = value;
        }
        public static explicit operator Vec3U16(Vec3U64 a)
        {
            return new Vec3U16((ushort)a.X, (ushort)a.Y, (ushort)a.Z);
        }
        public static explicit operator Vec3U16(Vec3S64 a)
        {
            return new Vec3U16((ushort)a.X, (ushort)a.Y, (ushort)a.Z);
        }
        public static explicit operator Vec3U16(Vec3S32 a)
        {
            return new Vec3U16((ushort)a.X, (ushort)a.Y, (ushort)a.Z);
        }
        public static explicit operator Vec3U16(Vec3U32 a)
        {
            return new Vec3U16((ushort)a.X, (ushort)a.Y, (ushort)a.Z);
        }
        public static explicit operator Vec3U16(Vec3S16 a)
        {
            return new Vec3U16((ushort)a.X, (ushort)a.Y, (ushort)a.Z);
        }
        public static explicit operator Vec3U16(Vec3S8 a)
        {
            return new Vec3U16((ushort)a.X, (ushort)a.Y, (ushort)a.Z);
        }
        public static explicit operator Vec3U16(Vec3U8 a)
        {
            return new Vec3U16(a.X, a.Y, a.Z);
        }
        public int LengthSquared { get { return X * X + Y * Y + Z * Z; } }

        public float Length { get { return (float)Math.Sqrt(X * X + Y * Y + Z * Z); } }

        public override bool Equals(object obj)
        {
            return (obj is Vec3U16) && Equals((Vec3U16)obj);
        }

        public bool Equals(Vec3U16 other)
        {
            return X == other.X & Y == other.Y && Z == other.Z;
        }

        public override int GetHashCode()
        {
            int hashCode = 0;
            hashCode += 1000000007 * X;
            hashCode += 1000000009 * Y;
            hashCode += 1000000021 * Z;
            return hashCode;
        }

        public static bool operator ==(Vec3U16 a, Vec3U16 b)
        {
            return a.X == b.X && a.Y == b.Y && a.Z == b.Z;
        }

        public static bool operator !=(Vec3U16 a, Vec3U16 b)
        {
            return a.X != b.X || a.Y != b.Y || a.Z != b.Z;
        }

        public override string ToString() 
        { 
            return X + ", " + Y + ", " + Z; 
        }
    }

    /// <summary> 3 component vector (signed 32 bit integer) </summary>
    public struct Vec3S32 : IEquatable<Vec3S32>
    {
        public int X, Y, Z;
        public static Vec3S32 Zero = new Vec3S32(0);

        public Vec3S32(int x, int y, int z)
        {
            X = x; 
            Y = y; 
            Z = z;
        }

        public Vec3S32(int value)
        {
            X = value; 
            Y = value; 
            Z = value;
        }


        public int LengthSquared { get { return X * X + Y * Y + Z * Z; } }

        public float Length { get { return (float)Math.Sqrt(X * X + Y * Y + Z * Z); } }


        public int this[int index]
        {
            get
            {
                if (index == 0) 
                { 
                    return X; 
                }
                else if (index == 1)
                { 
                    return Y; 
                }
                else 
                { 
                    return Z; 
                }
            }
            set
            {
                if (index == 0) 
                {
                    X = value; 
                }
                else if (index == 1) 
                { 
                    Y = value; 
                }
                else 
                { 
                    Z = value; 
                }
            }
        }


        public static Vec3S32 Max(Vec3S32 a, Vec3S32 b)
        {
            return new Vec3S32(Math.Max(a.X, b.X), Math.Max(a.Y, b.Y), Math.Max(a.Z, b.Z));
        }

        public static Vec3S32 Min(Vec3S32 a, Vec3S32 b)
        {
            return new Vec3S32(Math.Min(a.X, b.X), Math.Min(a.Y, b.Y), Math.Min(a.Z, b.Z));
        }
        public static implicit operator Vec3S32(Vec3S64 a)
        {
            return new Vec3S32((int)a.X, (int)a.Y, (int)a.Z);
        }
        public static implicit operator Vec3S32(Vec3U64 a)
        {
            return new Vec3S32((int)a.X, (int)a.Y, (int)a.Z);
        }
        public static implicit operator Vec3S32(Vec3U32 a)
        {
            return new Vec3S32((int)a.X, (int)a.Y, (int)a.Z);
        }
        public static implicit operator Vec3S32(Vec3S16 a)
        {
            return new Vec3S32(a.X, a.Y, a.Z);
        }
        public static implicit operator Vec3S32(Vec3U16 a)
        {
            return new Vec3S32(a.X, a.Y, a.Z);
        }
        public static implicit operator Vec3S32(Vec3S8 a)
        {
            return new Vec3S32(a.X, a.Y, a.Z);
        }
        public static implicit operator Vec3S32(Vec3U8 a)
        {
            return new Vec3S32(a.X, a.Y, a.Z);
        }

        public static Vec3S32 operator +(Vec3S32 a, Vec3S32 b)
        {
            return new Vec3S32(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static Vec3S32 operator -(Vec3S32 a, Vec3S32 b)
        {
            return new Vec3S32(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static Vec3S32 operator *(Vec3S32 a, int b)
        {
            return new Vec3S32(a.X * b, a.Y * b, a.Z * b);
        }

        public static Vec3S32 operator /(Vec3S32 a, int b)
        {
            return new Vec3S32(a.X / b, a.Y / b, a.Z / b);
        }

        public static Vec3S32 operator *(Vec3S32 a, float b)
        {
            return new Vec3S32((int)(a.X * b), (int)(a.Y * b), (int)(a.Z * b));
        }


        public override bool Equals(object obj)
        {
            return (obj is Vec3S32) && Equals((Vec3S32)obj);
        }

        public bool Equals(Vec3S32 other)
        {
            return X == other.X & Y == other.Y && Z == other.Z;
        }

        public override int GetHashCode()
        {
            int hashCode = 0;
            hashCode += 1000000007 * X;
            hashCode += 1000000009 * Y;
            hashCode += 1000000021 * Z;
            return hashCode;
        }

        public static bool operator ==(Vec3S32 a, Vec3S32 b)
        {
            return a.X == b.X && a.Y == b.Y && a.Z == b.Z;
        }

        public static bool operator !=(Vec3S32 a, Vec3S32 b)
        {
            return a.X != b.X || a.Y != b.Y || a.Z != b.Z;
        }

        public override string ToString()
        {
            return X + ", " + Y + ", " + Z;
        }
    }
    /// <summary> 3 component vector (32 bit floating point) </summary>
    public struct Vec3F32 : IEquatable<Vec3F32>
    {
        public float X, Y, Z;

        public Vec3F32(float x, float y, float z)
        {
            X = x; 
            Y = y; 
            Z = z;
        }

        public Vec3F32(float value)
        {
            X = value; 
            Y = value; 
            Z = value;
        }

        public float LengthSquared
        {
            get { return X * X + Y * Y + Z * Z; }
        }

        public float Length
        {
            get { return (float)Math.Sqrt(X * X + Y * Y + Z * Z); }
        }

        public static float Dot(Vec3F32 a, Vec3F32 b)
        {
            return a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        }


        public static Vec3F32 Cross(Vec3F32 a, Vec3F32 b)
        {
            return new Vec3F32(a.Y * b.Z - a.Z * b.Y,
                               a.Z * b.X - a.X * b.Z,
                               a.X * b.Y - a.Y * b.X);
        }

        public static Vec3F32 Normalise(Vec3F32 a)
        {
            float invLen = 1 / a.Length;
            // handle zero vector
            if (invLen == float.PositiveInfinity) return a;

            a.X *= invLen; 
            a.Y *= invLen; 
            a.Z *= invLen;
            return a;
        }


        public static Vec3F32 operator *(float a, Vec3F32 b)
        {
            return new Vec3F32(a * b.X, a * b.Y, a * b.Z);
        }

        public static Vec3F32 operator *(Vec3F32 a, float b)
        {
            return new Vec3F32(a.X * b, a.Y * b, a.Y * b);
        }

        public static Vec3F32 operator -(Vec3F32 a, Vec3F32 b)
        {
            return new Vec3F32(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static Vec3F32 operator +(Vec3F32 a, Vec3F32 b)
        {
            return new Vec3F32(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static implicit operator Vec3F32(Vec3S32 a)
        {
            return new Vec3F32(a.X, a.Y, a.Z);
        }


        public override bool Equals(object obj)
        {
            return (obj is Vec3F32) && Equals((Vec3F32)obj);
        }

        public bool Equals(Vec3F32 other)
        {
            return X == other.X & Y == other.Y && Z == other.Z;
        }

        public override int GetHashCode()
        {
            int hashCode = 0;
            hashCode += 1000000007 * X.GetHashCode();
            hashCode += 1000000009 * Y.GetHashCode();
            hashCode += 1000000021 * Z.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(Vec3F32 a, Vec3F32 b)
        {
            return a.X == b.X && a.Y == b.Y && a.Z == b.Z;
        }

        public static bool operator !=(Vec3F32 a, Vec3F32 b)
        {
            return a.X != b.X || a.Y != b.Y || a.Z != b.Z;
        }

        public override string ToString()
        {
            return X + "," + Y + "," + Z;
        }
    }
}
