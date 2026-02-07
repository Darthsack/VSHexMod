using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Util;
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
        public ListIota(): base(typeof(IotaType<ListIota>), new List<Iota>())
        {

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

        public override void ToBytes(BinaryWriter writer, ICoreAPI api)
        {
            writer.Write(api.GetIotaKey(this));

            List<Iota> val = getList();
            writer.Write((short)val.Count);

            foreach (Iota I in val)
            {
                I.ToBytes(writer, api);
                if (writer.BaseStream.Length > 32768)
                    return;
            }
        }

        public override Iota FromBytes(BinaryReader reader, IWorldAccessor resolver)
        {
            int count = reader.ReadInt16();

            List<Iota> val = new();

            for (int i = 0; i < count; i++)
            {
                string type = reader.ReadString();
                Iota io = resolver.Api.GetEmptyIota(type);
                val.Add(io.FromBytes(reader, resolver));
            }

            return new ListIota(val);
        }
    }
}
