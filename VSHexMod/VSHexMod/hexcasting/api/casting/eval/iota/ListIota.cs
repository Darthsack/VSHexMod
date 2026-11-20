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
        public string[] containedPlayerUids = new string[0];

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
                if((iota as EntityIota)?.getEntity() is EntityPlayer player)
                {
                    containedPlayerUids = containedPlayerUids.AddToArray(player.PlayerUID);
                }
                else if (iota is ListIota nested)
                {
                    nested.containedPlayerUids.Foreach((x)=> containedPlayerUids = containedPlayerUids.AddToArray(x));
                    containedPlayerUids = containedPlayerUids.Distinct().ToArray();
                }
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

        public List<string> FindTypeKeys(ICoreAPI api, List<string> IotaKeys = null)
        {
            IotaKeys ??= [];
            List<Iota> val = getList();
            val.ForEach(o =>
            {
                if (!IotaKeys.Contains(api.GetIotaKey(o)))
                    IotaKeys.Add(api.GetIotaKey(o));
                if (o is ListIota n)
                    IotaKeys = n.FindTypeKeys(api, IotaKeys);
            });

            return IotaKeys;
        }


        public override void ToBytes(BinaryWriter writer, ICoreAPI api)
        {
            writer.Write(api.GetIotaKey(this));

            List<Iota> val = getList();

            List<string> IotaKeys = FindTypeKeys(api);
            writer.Write((short)IotaKeys.Count);
            foreach (string key in IotaKeys)
            {
                writer.Write(key);
            }

            writer.Write((short)val.Count);

            foreach (Iota I in val)
            {
                I.ToBytes(writer, api, IotaKeys);
                if (writer.BaseStream.Length > 32768)
                    return;
            }
        }
        public override void ToBytes(BinaryWriter writer, ICoreAPI api, List<string> NameKey)
        {
            writer.Write((byte)NameKey.FindIndex((k) => k == api.GetIotaKey(this)));

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
            short count = reader.ReadInt16();

            List<Iota> valTypes = [];

            for (int i = 0; i < count; i++)
            {
                string type = reader.ReadString();
                Iota io = resolver.Api.GetEmptyIota(type);
                valTypes.Add(io);
            }

            count = reader.ReadInt16();

            List<Iota> val = [];

            for (int i = 0; i < count; i++)
            {
                byte type = reader.ReadByte();
                Iota io = valTypes[type];

                if(io is ListIota)
                    val.Add(FromBytes(reader, resolver, valTypes));
                else
                    val.Add(io.FromBytes(reader, resolver));
            }
            
            return new ListIota(val);
        }

        public static Iota FromBytes(BinaryReader reader, IWorldAccessor resolver, List<Iota> valTypes)
        {
            short count = reader.ReadInt16();

            List<Iota> val = [];

            for (int i = 0; i < count; i++)
            {
                byte type = reader.ReadByte();
                Iota io = valTypes[type].Copy();

                if (io is ListIota)
                    val.Add(FromBytes(reader, resolver, valTypes));
                else
                    val.Add(io.FromBytes(reader, resolver));
            }

            return new ListIota(val);
        }
    }
}
