using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VSHexMod.hexcasting.api.casting.eval.iota
{
    public class NullIota : Iota
    {
        private static object NULL_SUBSTITUTE = new object();
        public NullIota() : base(typeof(IotaType<NullIota>), NULL_SUBSTITUTE)
        {
        }

        public override string ToString()
        {
            return "Null";
        }

        public override bool isTruthy()
        {
            return false;
        }

        public override bool toleratesOther(Iota that)
        {
            return typesMatch(this, that);
        }
        public override void ToBytes(BinaryWriter writer, ICoreAPI api)
        {
            writer.Write(api.GetIotaKey(this));
        }

        public override Iota FromBytes(BinaryReader reader, IWorldAccessor resolver)
        {
            return this;
        }

    }
}
