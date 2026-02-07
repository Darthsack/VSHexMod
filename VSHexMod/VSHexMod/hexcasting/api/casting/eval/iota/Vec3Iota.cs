using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using VSHexMod.hexcasting.common.lib.hex;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VSHexMod.hexcasting.api.casting.eval.iota
{
    public class Vec3Iota : Iota
    {
        public Vec3Iota(Vec3d datum) :base(typeof(IotaType<Vec3Iota>), datum)
        {
            
        }
        public Vec3Iota() : base(typeof(IotaType<Vec3Iota>), new Vec3d())
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

        public override void ToBytes(BinaryWriter writer, ICoreAPI api)
        {
            writer.Write(api.GetIotaKey(this));

            Vec3d vec3 = this.getVec3(); 

            writer.Write(vec3.X);
            writer.Write(vec3.Y);
            writer.Write(vec3.Z);
        }

        public override Iota FromBytes(BinaryReader reader, IWorldAccessor resolver)
        {
            Vec3d vec3 = new();

            vec3.X = reader.ReadDouble();
            vec3.Y = reader.ReadDouble();
            vec3.Z = reader.ReadDouble();

            return new Vec3Iota(vec3);
        }

    }
}
