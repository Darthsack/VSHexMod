using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Common.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VSHexMod.hexcasting.api.casting.eval.iota
{
    public class EntityIota : Iota
    {
        public EntityIota(Entity e) : base(typeof(IotaType<EntityIota>), e)
        {
        }

        public override string ToString()
        {
            return ((Entity)payload).GetName();
        }

        public Entity getEntity()
        {
            return (Entity)this.payload;
        }

        public override bool toleratesOther(Iota that)
        {
            return typesMatch(this, that)
                && this.getEntity() == ((EntityIota)that).getEntity();
        }

        public override bool isTruthy()
        {
            return true;
        }
    }
}
