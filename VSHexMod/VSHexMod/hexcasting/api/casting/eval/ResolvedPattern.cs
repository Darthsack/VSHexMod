using Cairo;
using HarmonyLib;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Common;
using Vintagestory.API.Util;
using Vintagestory.Client.NoObf;
using Vintagestory.GameContent;
using VSHexMod.hexcasting.api.casting.math;

namespace VSHexMod.hexcasting.api.casting.eval
{
    public class ResolvedPattern : IByteSerializable
    {
        public HexPattern pattern;
        public HexCoord origin;

        public override string ToString()
        {
            return origin.ToString()+" "+pattern.ToString();
        }

        public static ResolvedPattern FromString(string Json)
        {
            
            string[] values = Json.Split(" ");

            string[] origin = values[0].Replace("HexCoord(", "").Replace(")", "").Split(",");

            double q = origin[0].ToDouble();
            double r = origin[1].ToDouble();

            string[] pat = values[1].Replace("HexPattern[", "").Replace("]", "").Split(",");
            HexDir startdir = HexDir.FromString(pat[0]);

            return new ResolvedPattern(HexPattern.fromAngles(pat[1], startdir), new HexCoord(q, r));
        }

        public ResolvedPattern(HexPattern _pattern, HexCoord _origin) 
        {
            pattern = _pattern;
            origin = _origin;
        }

        public void Deconstruct(out HexPattern _pattern, out HexCoord _origin)
        {
            _pattern = pattern;
            _origin = origin;
        }

        public ResolvedPattern(BinaryReader reader, IWorldAccessor resolver)
        {
            FromBytes(reader, resolver);
        }

        public void ToBytes(BinaryWriter writer) 
        {
            writer.Write(origin.q);
            writer.Write(origin.r);
            writer.Write((byte)pattern.startDir.Direction);
            writer.Write7BitEncodedInt(pattern.angles.Length);
            for (int i = 0; i < pattern.angles.Length; i += 2)
            {
                EnumHexAngle angle1 = pattern.angles[i].Angle;
                EnumHexAngle angle2 = pattern.angles.Length <= i + 1 ? 0 : pattern.angles[i + 1].Angle;

                byte writen = (byte)(((byte)angle1 << 4) | (byte)angle2);

                writer.Write(writen);
            }
        }

        public void FromBytes(BinaryReader reader, IWorldAccessor resolver)
        {
            double q = reader.ReadDouble();
            double r = reader.ReadDouble();
            origin = new HexCoord(q, r);
            HexDir startDir = new HexDir((EnumHexDir)reader.ReadByte());
            pattern = new HexPattern(startDir, new HexAngle[reader.Read7BitEncodedInt()]);

            for (int i = 0; i < pattern.angles.Length; i += 2)
            {
                byte Currentbyte = reader.ReadByte();
                pattern.angles[i] = new HexAngle((EnumHexAngle)(Currentbyte >> 4));
                if (pattern.angles.Length > i + 1)
                {
                    pattern.angles[i + 1] = new HexAngle((EnumHexAngle)(Currentbyte & 15));
                }
            }
        }
    }
}
