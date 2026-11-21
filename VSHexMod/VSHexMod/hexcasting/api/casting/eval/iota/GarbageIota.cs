using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace VSHexMod.hexcasting.api.casting.eval.iota
{
    internal class GarbageIota : Iota
    {
        public GarbageIota() : base(typeof(IotaType<NullIota>), null)
        {
        }

        public override string ToString()
        {
            return "{Garbage}";
        }

        public override bool isTruthy()
        {
            return false;
        }

        public override bool toleratesOther(Iota that)
        {
            return false;
        }
        public override void ToBytes(BinaryWriter writer, ICoreAPI api)
        {
            throw new NotImplementedException();
        }
        public override void ToBytes(BinaryWriter writer, ICoreAPI api, List<string> NameKey)
        {
            throw new NotImplementedException();
        }

        public override Iota FromBytes(BinaryReader reader, IWorldAccessor resolver)
        {
            throw new NotImplementedException();
        }

    }
}
