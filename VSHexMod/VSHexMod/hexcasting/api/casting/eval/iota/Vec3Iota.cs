using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using VSHexMod.hexcasting.common.lib.hex;
using Vintagestory.API.MathTools;
using Vintagestory.API.Common.Entities;

namespace VSHexMod.hexcasting.api.casting.eval.iota
{
    public class Vec3Iota : Iota
    {
        public Vec3Iota(Vec3d datum) :base(typeof(IotaType<Vec3Iota>), datum)
        {
            
        }

        public override string ToString()
        {
            return "(" + Math.Round(((Vec3d)payload).X,3) +","+ Math.Round(((Vec3d)payload).Y,3) +","+ Math.Round(((Vec3d)payload).Z,3 )+ ")";
        }

        public Vec3Iota(double x, double y, double z) : base(typeof(IotaType<Vec3Iota>) , new Vec3d(x, y, z))
        {
            
        }

        public Vec3Iota(Vec3i datum) : base(typeof(IotaType<Vec3Iota>), new Vec3d(datum.X, datum.Y, datum.Z))
        {

        }

        public Vec3d getVec3()
        {
            return (Vec3d)this.payload;
        }

        public override bool isTruthy()
        {
            var v = this.getVec3();
            return !(v.X == 0.0 && v.Y == 0.0 && v.Z == 0.0);
        }

        public override bool toleratesOther(Iota that)
        {
            return typesMatch(this, that)
                && that is Vec3Iota viota
                && this.getVec3().SquareDistanceTo(viota.getVec3()) < DoubleIota.TOLERANCE * DoubleIota.TOLERANCE;
        }

    }
}
