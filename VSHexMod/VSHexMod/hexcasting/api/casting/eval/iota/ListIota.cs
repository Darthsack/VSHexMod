using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSHexMod.hexcasting.common.lib.hex;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VSHexMod.hexcasting.api.casting.eval.iota
{
    public class ListIota : Iota
    {
        private int _depth;
        private int _size;

        public override string ToString()
        {
            return "[" + ((List<Iota>)payload).Select(x => x is null ? "Null":x.ToString()).ToArray().Join() + "]";
        }

        public ListIota(List<Iota> list) : base(typeof(IotaType<ListIota>), list)
        {
            int maxChildDepth = 0;
            int totalSize = 1;
            foreach (Iota iota in list)
            {
                totalSize += iota is null ? 1 : iota.size();
                maxChildDepth = Math.Max(maxChildDepth, iota is null ? 0 : iota.depth());
            }
            _depth = maxChildDepth + 1;
            _size = totalSize;
        }

        public List<Iota> getList()
        {
            return (List<Iota>)this.payload;
        }

        public override bool isTruthy()
        {
            return this.getList().Count > 0;
        }

        public override bool toleratesOther(Iota that)
        {
            if (!typesMatch(this, that))
            {
                return false;
            }
            var a = this.getList();
            
            var b = ((ListIota)that).getList();

            return a.Equals(b);

        }
        public override int size()
        {
            return _size;
        }
        public override int depth()
        {
            return _depth;
        }

    }
}
