using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSHexMod.hexcasting.api.casting.eval.BaseElements;

namespace VSHexMod.hexcasting.api.casting.eval.iota
{
    public class ElementIota : Iota
    {
        public ElementIota(Element d) : base(typeof(IotaType<ElementIota>), d) { }

        public override bool isTruthy()
        {
            return true;
        }
        public Element getElement()
        {
            return (Element)this.payload;
        }

        public override bool toleratesOther(Iota that)
        {
            if (that is ElementIota e)
            {
                if (e.getElement() == this.getElement()) 
                    return true;
            }
            return false;
        }

    }
}
