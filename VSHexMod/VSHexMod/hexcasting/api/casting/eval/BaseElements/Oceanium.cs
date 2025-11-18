using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSHexMod.hexcasting.api.casting.eval.BaseElements
{
    public class Oceanium : Element
    {
        public Oceanium(int strength) : base(strength, "oceanium") {
            progdmg = 5 * strength;
        }
    }
}
