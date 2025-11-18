using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSHexMod.hexcasting.common.lib.hex;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VSHexMod.hexcasting.api.casting.eval.iota
{
    public class DoubleIota : Iota
    {
        public static double TOLERANCE = 0.0001;

        public DoubleIota(double d) : base(typeof(IotaType<DoubleIota>), d) { }

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
    }
}
