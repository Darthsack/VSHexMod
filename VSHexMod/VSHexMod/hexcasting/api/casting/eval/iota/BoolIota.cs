using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VSHexMod.hexcasting.api.casting.eval.iota
{
    public class BoolIota : Iota
    {
        public BoolIota(bool d) : base(typeof(IotaType<BoolIota>), d)
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

    }
}
