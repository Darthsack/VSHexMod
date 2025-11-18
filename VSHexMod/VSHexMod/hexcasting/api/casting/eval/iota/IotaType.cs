using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Common;
using static HarmonyLib.Code;

namespace VSHexMod.hexcasting.api.casting.eval.iota
{
    public abstract class IotaType<T> : IotaType where T : Iota
    {
        public abstract T deserialize(string tag, IWorldAccessor world);

    }

    public abstract class IotaType
    {
        public abstract int color();
    }
}
