using Cairo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Common;
using VSHexMod.hexcasting.api.casting.math;

namespace VSHexMod.hexcasting.api.casting.math
{
    internal enum EnumHexDir
    {
        NORTH_EAST,
        EAST,
        SOUTH_EAST,
        SOUTH_WEST,
        WEST,
        NORTH_WEST
    }
    public class HexDir
    {
        internal static int size = 6;
        public static HexDir NORTH_EAST
        {
            get { return new HexDir(EnumHexDir.NORTH_EAST); }
        }

        public static HexDir EAST
        {
            get { return new HexDir(EnumHexDir.EAST); }
        }

        public static HexDir SOUTH_EAST
        {
            get { return new HexDir(EnumHexDir.SOUTH_EAST); }
        }

        public static HexDir SOUTH_WEST
        {
            get { return new HexDir(EnumHexDir.SOUTH_WEST); }
        }

        public static HexDir WEST
        {
            get { return new HexDir(EnumHexDir.WEST); }
        }

        public static HexDir NORTH_WEST
        {
            get { return new HexDir(EnumHexDir.NORTH_WEST); }
        }


        internal EnumHexDir Direction;
        internal HexDir(EnumHexDir Dir) {
            Direction = Dir;
        }

        public HexDir rotatedBy(HexAngle Ang)
        {
            EnumHexDir Dir = (EnumHexDir)(((int)this.Direction + (int)Ang.Angle)%size);
            return new HexDir(Dir);
        }

        public static HexDir operator *(HexDir a, HexAngle b) => a.rotatedBy(b);

        public HexAngle angleFrom(HexDir Dir)
        {
            HexAngle Ang = new HexAngle((EnumHexAngle)(((int)this.Direction - (int)Dir.Direction) % 6));
            if (Ang.Angle < 0)
            {
                Ang = new HexAngle((EnumHexAngle)(((int)Ang.Angle + 6)%6));
            }
            return Ang;
        }

        public static HexAngle operator -(HexDir a, HexDir b) => new HexAngle(a.angleFrom(b).Angle);
        public static bool operator ==(HexDir a, HexDir b) => a.Direction == b.Direction;
        public static bool operator !=(HexDir a, HexDir b) => a.Direction != b.Direction;

        public HexCoord asDelta()
        {
            switch (Direction)
            {
                case EnumHexDir.NORTH_EAST:
                    return new HexCoord(1, -1);
                case EnumHexDir.EAST:
                    return new HexCoord(1, 0);
                case EnumHexDir.SOUTH_EAST:
                    return new HexCoord(0, 1);
                case EnumHexDir.SOUTH_WEST:
                    return new HexCoord(-1, 1);
                case EnumHexDir.WEST:
                    return new HexCoord(-1, 0);
                case EnumHexDir.NORTH_WEST:
                    return new HexCoord(0, -1);
            }
            return new HexCoord(0, 0);
        }

        public string toString()
        {
            switch (Direction)
            {
                case EnumHexDir.NORTH_EAST:
                    return "NORTH_EAST";
                case EnumHexDir.EAST:
                    return "EAST";
                case EnumHexDir.SOUTH_EAST:
                    return "SOUTH_EAST";
                case EnumHexDir.SOUTH_WEST:
                    return "SOUTH_WEST";
                case EnumHexDir.WEST:
                    return "WEST";
                case EnumHexDir.NORTH_WEST:
                    return "NORTH_WEST";
            }
            return "";
        }

        public static HexDir FromString(string Dir)
        {
            switch (Dir)
            {
                case "NORTH_EAST":
                    return new HexDir(EnumHexDir.NORTH_EAST);
                case "EAST":
                    return new HexDir(EnumHexDir.EAST);
                case "SOUTH_EAST":
                    return new HexDir(EnumHexDir.SOUTH_EAST);
                case "SOUTH_WEST":
                    return new HexDir(EnumHexDir.SOUTH_WEST);
                case "WEST":
                    return new HexDir(EnumHexDir.WEST);
                case "NORTH_WEST":
                    return new HexDir(EnumHexDir.NORTH_WEST);
            }
            return null;
        }
        public override string ToString()
        {
            return toString();
        }

    }
}
