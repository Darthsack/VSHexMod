using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.Server;
using VSHexMod.hexcasting.api.casting.math;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VSHexMod.hexcasting.api.casting.eval.iota
{
    public class EntityIota : Iota
    {
        public EntityIota(Entity e) : base(typeof(IotaType<EntityIota>), e)
        {
        }
        public EntityIota() : base(typeof(IotaType<EntityIota>), null)
        {
        }

        public override string ToString()
        {
            return ((Entity)payload)?.GetName();
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

        public override void ToBytes(BinaryWriter writer, ICoreAPI api)
        {
            writer.Write(api.GetIotaKey(this));

            Entity val = getEntity();
            EntityPlayer e = val as EntityPlayer;
            if (e is not null)
            {
                writer.Write(true);
                writer.Write(e.PlayerUID);
            }
            else
            {
                writer.Write(false);
                writer.Write(val.EntityId);
            }
        }

        public override Iota FromBytes(BinaryReader reader, IWorldAccessor resolver)
        {
            if (!reader.ReadBoolean())
            {
                payload = resolver.GetEntityById(reader.ReadInt64());
            }
            else
            {
                payload = resolver.PlayerByUid(reader.ReadString())?.Entity;
            }
            return new EntityIota((Entity)payload);
        }
    }
}
