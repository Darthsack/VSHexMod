using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Client;
using Vintagestory.API.Server;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.GameContent;
using VSHexMod.Gui;
using VSHexMod.hexcasting.api.casting.eval;
using VSHexMod.Inventory;
using VSHexMod.Crafting;
using HarmonyLib;
using Vintagestory.API.Util;
using System.IO;
using VSHexMod.hexcasting.api.casting.math;

namespace VSHexMod.BlockEntity
{
    public class BEListScriber : BlockEntityOpenableContainer
    {

        internal ICoreAPI locApi;

        internal InventoryHexScribe inventory;

        internal List<ResolvedPattern> hexpatterns = new List<ResolvedPattern>();

        internal string title;

        public ScribeRecipe CurrentRecipe;

        GuiDialogBlockEntityListScriber clientDialog;
        public override string InventoryClassName
        {
            get { return "hexcastingtable"; }
        }

        public virtual string DialogTitle
        {
            get { return Lang.Get("Scribebench"); }
        }

        public override InventoryBase Inventory
        {
            get { return inventory; }
        }

        public BEListScriber()
        {
            inventory = new InventoryHexScribe(null, null);
            inventory.SlotModified += OnSlotModifid;


        }

        public override void Initialize(ICoreAPI api)
        {
            base.Initialize(api);

            inventory.pos = Pos;
            inventory.LateInitialize("hexcastingtable-" + Pos.X + "/" + Pos.Y + "/" + Pos.Z, api);

            //RegisterGameTickListener(OnBurnTick, 100);
            //RegisterGameTickListener(On500msTick, 500);

            //if (api is ICoreClientAPI)
            //{
            //    renderer = new FirepitContentsRenderer(api as ICoreClientAPI, Pos);
            //    (api as ICoreClientAPI).Event.RegisterRenderer(renderer, EnumRenderStage.Opaque, "firepit");
            //
            //    UpdateRenderer();
            //}
            //api.Logger.Event("test");
            locApi = api;
        }

        private void OnSlotModifid(int slotid)
        {
            Block = Api.World.BlockAccessor.GetBlock(Pos);

            //UpdateRenderer();
            MarkDirty(Api.Side == EnumAppSide.Server); // Save useless triple-remesh by only letting the server decide when to redraw
            //shouldRedraw = true;

            //if (Api is ICoreClientAPI && clientDialog != null)
            //{
            //    SetDialogValues(clientDialog.Attributes);
            //}
            if (inventory.Slots[1]?.Itemstack is not null && inventory.Slots[1].Itemstack.Attributes.HasAttribute("spell") && Api.Side == EnumAppSide.Client)
            {
                string[] pats =  (inventory.Slots[1].Itemstack.Attributes["spell"]).GetValue() as string[];

                hexpatterns.Clear();

                hexpatterns.AddRange(pats.Select((r) => ResolvedPattern.FromString(r)));
                clientDialog.SingleComposer.ReCompose();
            }

            Api.World.BlockAccessor.GetChunkAtBlockPos(Pos)?.MarkModified();
            
        }

        public override bool OnPlayerRightClick(IPlayer byPlayer, BlockSelection blockSel)
        {
            //locApi.Logger.Event("player clicked block");
            if (Api.Side == EnumAppSide.Client)
            {
                toggleInventoryDialogClient(byPlayer, () => {
                    //SyncedTreeAttribute dtree = new SyncedTreeAttribute();
                    //SetDialogValues(dtree);
                    clientDialog = new GuiDialogBlockEntityListScriber(DialogTitle, Inventory, Pos, Api as ICoreClientAPI, hexpatterns);
                    return clientDialog;
                });

            }

            return true;
        }

        private bool FindMatchingRecipe()
        {
            ItemSlot[] inputSlots =  { paperSlot, outputSlot };
            CurrentRecipe = null;

            foreach (var recipe in Api.GetScribeRecipes())
            {

                if (recipe.Matches(inputSlots))
                {
                    CurrentRecipe = recipe;

                    if (Api?.Side == EnumAppSide.Server)
                    {
                        recipe.TryCraftNow(Api, 0, inputSlots);
                        MarkDirty(true);
                        Api.World.BlockAccessor.MarkBlockEntityDirty(Pos);
                    }

                    if (Api?.Side == EnumAppSide.Client)
                    {
                        MarkDirty(true);
                    }

                    return true;
                }
            }
            return false;
        }

        public override void OnReceivedClientPacket(IPlayer player, int packetid, byte[] data)
        {
            base.OnReceivedClientPacket(player, packetid, data);

            //if (packetid < 1000)
            //{
            //    Inventory.InvNetworkUtil.HandleClientPacket(player, packetid, data);

            //    // Tell server to save this chunk to disk again
            //    Api.World.BlockAccessor.GetChunkAtBlockPos(Pos).MarkModified();

            //    return;
            //}
            if (packetid == 2000)
            {
                //Api.Logger.Event("hello server");

                using (MemoryStream ms = new MemoryStream(data))
                {
                    using (BinaryReader reader = new BinaryReader(ms))
                    {
                        int quantity = reader.ReadInt32();
                        hexpatterns = new List<ResolvedPattern>();
                        for (int i = 0; i < quantity; i++)
                        {
                            hexpatterns.Add(new ResolvedPattern(reader,Api.World));
                            //Api.Logger.Event("Deserialized: "+ hexpatterns[i].pattern.ToString());
                        }
                        
                    }
                }

                InscribeSpell(Api, 0);

            } 
            else if (packetid == 3000)
            {
                string o = "";
                foreach (char n in data)
                {
                    o += n;
                }
                title = o;
            }
        }

        public bool InscribeSpell(ICoreAPI api, double blood)
        {
            if (inventory.Slots[2]?.Itemstack?.StackSize is not null && inventory.Slots[2].Itemstack.StackSize != 0)
                return false;

            //api.Logger.Event(inventory?.Slots[0]?.Itemstack?.Item?.Attributes?["spell"].ToString());


            bool crafted = false;
            if (inventory.Slots[0].Itemstack is null || !inventory.Slots[0].Itemstack.Item.Attributes.KeyExists("spell"))
            {
                if (!FindMatchingRecipe())
                    return false;
                crafted = true;
            }

            if (inventory.Slots[1]?.Itemstack is not null && inventory.Slots[1].Itemstack.Attributes.HasAttribute("spell"))
            {
                inventory.Slots[2].Itemstack = inventory.Slots[1].Itemstack.Clone();
                inventory.Slots[2].Itemstack.Attributes = inventory.Slots[1].Itemstack.Attributes.Clone();

                if (!crafted)
                    inventory.Slots[0].TakeOut(1);
                inventory.Slots[2].Itemstack.StackSize = 1;

                inventory.Slots[0].MarkDirty();
                inventory.Slots[2].MarkDirty();

                return true;
            }

            if (inventory?.Slots[2]?.Itemstack?.StackSize is null || inventory.Slots[2].Itemstack.StackSize < 1)
            {
                inventory.Slots[2].Itemstack = inventory.Slots[0].Itemstack.Clone();
                if (!crafted)
                    inventory.Slots[0].TakeOut(1);
                inventory.Slots[2].Itemstack.StackSize = 1;
            }

            //inventory.Slots[2].Itemstack.Attributes.GetOrAddTreeAttribute("spell");

            string[] att = hexpatterns.Select((r) => r.ToString()).ToArray();

            inventory.Slots[2].Itemstack.Attributes["spell"] = new StringArrayAttribute(att);
            inventory.Slots[2].Itemstack.Attributes["title"] = new StringAttribute(title);
            if (inventory.Slots[0].Itemstack?.StackSize == 0)
                inventory.Slots[0].Itemstack = null;
            
            inventory.Slots[0].MarkDirty();
            inventory.Slots[2].MarkDirty();
            return true;
        }



        public ItemSlot paperSlot
        {
            get { return inventory[0]; }
        }

        public ItemSlot scrollSlot
        {
            get { return inventory[1]; }
        }

        public ItemSlot outputSlot
        {
            get { return inventory[2]; }
        }




        public ItemStack paperStack
        {
            get { return inventory[0].Itemstack; }
            set { inventory[0].Itemstack = value; inventory[0].MarkDirty(); }
        }

        public ItemStack scrollStack
        {
            get { return inventory[1].Itemstack; }
            set { inventory[1].Itemstack = value; inventory[1].MarkDirty(); }
        }

        public ItemStack outputStack
        {
            get { return inventory[2].Itemstack; }
            set { inventory[2].Itemstack = value; inventory[2].MarkDirty(); }
        }

    }
}
