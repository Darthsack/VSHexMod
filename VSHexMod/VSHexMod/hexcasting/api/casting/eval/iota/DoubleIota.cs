using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using VSHexMod.hexcasting.common.lib.hex;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VSHexMod.hexcasting.api.casting.eval.iota
{
    public class DoubleIota : Iota
    {
        public static double TOLERANCE = 0.0001;

        public DoubleIota(double d) : base(typeof(IotaType<DoubleIota>), d) { }
        public DoubleIota() : base(typeof(IotaType<DoubleIota>), 0.0) { }

        public double getDouble()
        {
            return (double)this.payload;
        }

        public override bool isTruthy()
        {
            return this.getDouble() != 0.0;
        }

        public override bool toleratesOther(Iota that)
        {
            return typesMatch(this, that)
                && tolerates(this.getDouble(), ((DoubleIota)that).getDouble());
        }

        public static bool tolerates(double a, double b)
        {
            return Math.Abs(a - b) < TOLERANCE;
        }

        public override void ToBytes(BinaryWriter writer, ICoreAPI api)
        {
            writer.Write(api.GetIotaKey(this));

            double val = getDouble();

            writer.Write(val);
        }

        public override Iota FromBytes(BinaryReader reader, IWorldAccessor resolver)
        {

            payload = reader.ReadDouble();

            return new DoubleIota((double)payload);
        }
    }
}
