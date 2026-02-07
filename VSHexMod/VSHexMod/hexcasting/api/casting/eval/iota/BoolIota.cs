using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Common;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VSHexMod.hexcasting.api.casting.eval.iota
{
    public class BoolIota : Iota
    {
        public BoolIota(bool d) : base(typeof(IotaType<BoolIota>), d)
        {
        }
        public BoolIota() : base(typeof(IotaType<BoolIota>), false)
        {
        }

        public bool getBool()
        {
            return (bool)this.payload;
        }

        public override bool isTruthy()
        {
            return this.getBool();
        }

        public override bool toleratesOther(Iota that)
        {
            return typesMatch(this, that)
                && this.getBool() == ((BoolIota)that).getBool();
        }

        public override void ToBytes(BinaryWriter writer, ICoreAPI api)
        {
            writer.Write(api.GetIotaKey(this));

            bool val = getBool();

            writer.Write(val);
        }

        public override Iota FromBytes(BinaryReader reader, IWorldAccessor resolver)
        {

            payload = reader.ReadBoolean();

            return new BoolIota((bool)payload);
        }
    }
}
