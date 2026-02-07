using Cairo;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vintagestory.API.Common;

using Vintagestory.API.Common.Entities;

using VSHexMod.hexcasting.api.casting.math;

namespace VSHexMod.hexcasting.api.casting.eval.iota
{
    
    public class Iota
    {
        protected object payload;
        protected Type type;

        
        public override string ToString()
        {
            return payload.ToString();
        }

        protected Iota(Type type, Object payload) 
        {
            this.type = type;
            this.payload = payload;
        }
        public Iota()
        {

        }

        public Type getType() {
            return this.type;
        }

        public virtual bool isTruthy() => false;

        public virtual bool toleratesOther(Iota that) => false;

        public virtual CastResult execute(Entity player, ICoreAPI api, State Current)
        {
            return new CastResult(
                this,
                Current,
                ResolvedPatternType.INVALID
                /*HexEvalSounds.MISHAP*/);
        }

        public static bool typesMatch(Iota a, Iota b)
        {
            var resA = a.getType();
            var resB = b.getType();
            return resA != null && resA.Equals(resB);
        }

        public virtual int size()
        {
            return 1;
        }

        public virtual int depth()
        {
            return 1;
        }

        public override bool Equals(object b)
        {
            return toleratesOther((Iota)b);
        }

        public override int GetHashCode()
        {
            return type.GetHashCode() + payload.GetHashCode();
        }
        public virtual bool executable()
        {
            return false;
        }

        public virtual Iota Copy()
        {
            return new Iota(type,payload);
        }

        public virtual void ToBytes(BinaryWriter writer, ICoreAPI api)
        {
            writer.Write(api.GetIotaKey(this));
        }

        public virtual Iota FromBytes(BinaryReader reader, IWorldAccessor resolver)
        {
            Iota type = resolver.Api.GetEmptyIota(reader.ReadString());
            Iota o = type.FromBytes(reader, resolver);

            return o;
        }

        public static bool operator ==(Iota a, Iota b) => a.toleratesOther(b);
        public static bool operator !=(Iota a, Iota b) => !a.toleratesOther(b);
    }

}
