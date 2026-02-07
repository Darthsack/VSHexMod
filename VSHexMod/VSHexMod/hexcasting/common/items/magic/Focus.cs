using Cairo;
using Microsoft.VisualBasic;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;
using Vintagestory.Common;
using Vintagestory.GameContent;
using VSHexMod.hexcasting.api.casting.eval;
using VSHexMod.hexcasting.api.casting.eval.iota;
using VSHexMod.hexcasting.api.casting.math;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VSHexMod.hexcasting.common.items.magic
{
    public class Focus : ItemRollable
    {
        bool editable;
        ICoreClientAPI capi;
        WorldInteraction[] interactions;
        VSHexModModSystem HexModSys;

        private Iota storedIota;

        public Iota iota
        {
            get { return storedIota; } 
        }
        public Iota GetIota(ItemStack IS)
        {
            if (!reload)
                return storedIota;
            reload = !reload;

            if (IS.Attributes["iota"] is null || IS.Attributes["iota"]?.ToString() == "")
            {
                storedIota = new NullIota();
                return storedIota;
            }
            using (MemoryStream ms = new MemoryStream((byte[])IS.Attributes["iota"].GetValue()))
            {
                using (BinaryReader reader = new BinaryReader(ms))
                {
                    Iota tmp = new();
                    
                    Iota output = tmp.FromBytes(reader, api.World);

                    api.Logger.Notification(output.ToString());
                    storedIota = output;

                }
            }
            return storedIota;
        }
        public void SetIota(Iota iota, ItemStack IS)
        {
            
            using (MemoryStream ms = new MemoryStream())
            {

                using (BinaryWriter writer = new BinaryWriter(ms))
                {
                    iota.ToBytes(writer, api);
                    byte[] o = ms.ToArray();
                    if (o.Length > 32768)
                    {
                        storedIota = new NullIota();
                        SetIota(storedIota, IS);
                    }
                    else
                    {
                        storedIota = iota;
                        IS.Attributes["iota"] = new ByteArrayAttribute(o);
                    }
                }
            }
        }

        bool reload = false;
        public override void OnLoaded(ICoreAPI api)
        {
            base.OnLoaded(api);
            capi = api as ICoreClientAPI;
            this.api = api;
            if (Attributes is not null)
                editable = Attributes["editable"].AsBool(false);
            HexModSys = api.ModLoader.GetModSystem<VSHexModModSystem>();
            reload = true;
        }

        public override void OnHeldIdle(ItemSlot slot, EntityAgent byEntity)
        {
            if (byEntity.World is IClientWorldAccessor)
            {
                FpHandTransform.Rotation.X = GameMath.Mod(-byEntity.World.ElapsedMilliseconds / 50f, 360);
                TpHandTransform.Rotation.X = GameMath.Mod(-byEntity.World.ElapsedMilliseconds / 50f, 360);
                TpHandTransform.Translation.X = -GameMath.Sin(-byEntity.World.ElapsedMilliseconds / 600f) / 8f - 1.10f;
                FpHandTransform.Translation.X = -GameMath.Sin(-byEntity.World.ElapsedMilliseconds / 600f) / 8f;
            }

        }
    }
}
