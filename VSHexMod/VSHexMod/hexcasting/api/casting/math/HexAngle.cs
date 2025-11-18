using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSHexMod.hexcasting.api.casting.math
{
    internal enum EnumHexAngle
    {
        FORWARD, 
        RIGHT, 
        RIGHT_BACK, 
        BACK, 
        LEFT_BACK, 
        LEFT
    }
    public class HexAngle
    {

        internal static int size = 6;
        public static HexAngle FORWARD {
            get { return new HexAngle(EnumHexAngle.FORWARD); }
        }
        public static HexAngle RIGHT
        {
            get { return new HexAngle(EnumHexAngle.RIGHT); }
        }
        public static HexAngle RIGHT_BACK
        {
            get { return new HexAngle(EnumHexAngle.RIGHT_BACK); }
        }
        public static HexAngle BACK
        {
            get { return new HexAngle(EnumHexAngle.BACK); }
        }
        public static HexAngle LEFT_BACK
        {
            get { return new HexAngle(EnumHexAngle.LEFT_BACK); }
        }
        public static HexAngle LEFT
        {
            get { return new HexAngle(EnumHexAngle.LEFT); }
        }

        

        internal EnumHexAngle Angle;
        internal HexAngle(EnumHexAngle Ang)
        {
            Angle = Ang;
        }

        public HexAngle rotatedBy(HexAngle ang)
        {
            this.Angle = (EnumHexAngle)(((int)this.Angle + (int)ang.Angle) % size);
            return this;
        }
        public override string ToString()
        {
            return Angle.ToString();
        }

        public static HexAngle operator *(HexAngle a, HexAngle b) => new HexAngle (a.rotatedBy(b).Angle);
        public static bool operator ==(HexAngle a, HexAngle b) => a.Angle == b.Angle;
        public static bool operator !=(HexAngle a, HexAngle b) => a.Angle != b.Angle;

    }
}
