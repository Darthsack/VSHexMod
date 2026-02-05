using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Server;
using Vintagestory.API.Util;
using Vintagestory.GameContent;
using static VSHexMod.VSHexModModSystem;

namespace VSHexMod.hexcasting.common.items.magic
{
    public class ItemHexBook : ItemRollable
    {
        int maxPageCount;
        bool editable;
        ICoreClientAPI capi;
        WorldInteraction[] interactions;
        VSHexModModSystem HexModSys;

        public override void OnLoaded(ICoreAPI api)
        {
            base.OnLoaded(api);
            capi = api as ICoreClientAPI;
            editable = Attributes["editable"].AsBool(false);
            maxPageCount = Attributes["maxPageCount"].AsInt(90);
            HexModSys = api.ModLoader.GetModSystem<VSHexModModSystem>();

            interactions = ObjectCacheUtil.GetOrCreate(api, "castInteractions", () =>
            {

                return new WorldInteraction[]
                {
                    new WorldInteraction
                    {
                        MouseButton = EnumMouseButton.Right,
                        ActionLangCode = "heldhelp-cast",
                        ShouldApply =  (wi, bs, es) => {
                            var slot = capi.World.Player.InventoryManager.ActiveHotbarSlot;
                            return isCastable(slot) && slot.Itemstack.Attributes.HasAttribute("spell");
                        }
                    }
                };
            });

        }

        public override void OnHeldInteractStart(ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, bool firstEvent, ref EnumHandHandling handling)
        {

            if (byEntity.Controls.CtrlKey)
            {
                ITreeAttribute spell = slot.Itemstack.Attributes?.Clone();

                slot.Itemstack = new ItemStack(byEntity.Api.World.GetItem(CodeWithPart(Variant["state"] != "unsealed" ? "unsealed" : "sealed", 2)), slot.Itemstack.StackSize);
                slot.MarkDirty();

                if (spell is not null)
                    slot.Itemstack.Attributes = spell;


                handling = EnumHandHandling.Handled;
                
            }
            else if (Variant["state"] == "unsealed")
            {
                //api.Logger.Event("Casting!");
                if (slot.Itemstack.Attributes.HasAttribute("spell"))
                    capi.Network.GetChannel("castingbook").SendPacket(new castPacket() { spell = (slot.Itemstack.Attributes["spell"]).GetValue() as string[] });
            }
            else
            {
                base.OnHeldInteractStart(slot, byEntity, blockSel, entitySel, firstEvent, ref handling);
            }

        }

        public override string GetHeldItemName(ItemStack itemStack)
        {
            string title = itemStack.Attributes.GetString("title");
            if (title != null && title.Length > 0) return title;

            return base.GetHeldItemName(itemStack);
        }

        public static bool isCastable(ItemSlot slot)
        {
            return slot.Itemstack.Collectible.Attributes["castable"].AsBool() == true;
        }
    }
}
