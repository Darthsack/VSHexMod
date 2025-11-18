using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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


    }
}
