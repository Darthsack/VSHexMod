using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Server;
using Vintagestory.API.Util;
using Vintagestory.GameContent;
using VSHexMod.Crafting;
using VSHexMod.Gui;
using VSHexMod.hexcasting.api.casting.eval;
using VSHexMod.hexcasting.api.casting.math;
using VSHexMod.Inventory;

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

            UpdateScroll();

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
            if (slotid == 1 && Api.Side == EnumAppSide.Client)
            {
                UpdateScroll();
            }

            Api.World.BlockAccessor.GetChunkAtBlockPos(Pos)?.MarkModified();
            
        }

        public void UpdateScroll()
        {
            if (scrollStack is not null && scrollStack.Attributes.HasAttribute("spell"))
            {
                string[] pats = (scrollStack.Attributes["spell"]).GetValue() as string[];

                if (scrollStack.Attributes.HasAttribute("title"))
                    title = scrollStack.Attributes["title"].GetValue() as string;

                hexpatterns.Clear();

                hexpatterns.AddRange(pats.Select((r) => ResolvedPattern.FromString(r)));

                clientDialog?.SingleComposer?.GetTextArea("text").SetValue(title);

                clientDialog?.SingleComposer?.ReCompose();
            }
        }

        public override bool OnPlayerRightClick(IPlayer byPlayer, BlockSelection blockSel)
        {
            //locApi.Logger.Event("player clicked block");
            if (Api.Side == EnumAppSide.Client)
            {
                toggleInventoryDialogClient(byPlayer, () => {
                    //SyncedTreeAttribute dtree = new SyncedTreeAttribute();
                    //SetDialogValues(dtree);
                    clientDialog = new GuiDialogBlockEntityListScriber(DialogTitle, Inventory, Pos, Api as ICoreClientAPI, hexpatterns, title);
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
           
            //api.Logger.Event(inventory?.Slots[0]?.Itemstack?.Item?.Attributes?["spell"].ToString());


            bool crafted = false;
            if ((paperStack is null || !paperStack.Item.Attributes.KeyExists("spell") ) && outputStack?.StackSize == 0)
            {
                if (!FindMatchingRecipe())
                    return false;
                crafted = true;
            }

            if (scrollStack is not null && scrollStack.Attributes.HasAttribute("spell") && (outputStack is null || outputStack.StackSize == 0 ))
            {
                outputStack = scrollStack.Clone();
                outputStack.Attributes = scrollStack.Attributes.Clone();

                if (!crafted)
                    paperSlot.TakeOut(1);
                outputStack.StackSize = 1;

                paperSlot.MarkDirty();
                outputSlot.MarkDirty();

                return true;
            }

            else if (outputStack?.StackSize is null || outputStack.StackSize < 1)
            {
                outputStack = paperStack.Clone();
                if (!crafted)
                    paperSlot.TakeOut(1);
                outputStack.StackSize = 1;
            }

            //inventory.Slots[2].Itemstack.Attributes.GetOrAddTreeAttribute("spell");

            string[] att = hexpatterns.Select((r) => r.ToString()).ToArray();

            outputStack.Attributes["spell"] = new StringArrayAttribute(att);
            outputStack.Attributes["title"] = new StringAttribute(title);
            if (paperStack?.StackSize == 0)
                paperStack = null;

            paperSlot.MarkDirty();
            outputSlot.MarkDirty();
            return true;
        }


        /// <summary>
        /// inventory[0]
        /// </summary>
        public ItemSlot paperSlot
        {
            get { return inventory[0]; }
        }
        /// <summary>
        /// inventory[1]
        /// </summary>
        public ItemSlot scrollSlot
        {
            get { return inventory[1]; }
        }
        /// <summary>
        /// inventory[2]
        /// </summary>
        public ItemSlot outputSlot
        {
            get { return inventory[2]; }
        }



        /// <summary>
        /// paperSlot?.Itemstack
        /// </summary>
        public ItemStack paperStack
        {
            get { return paperSlot?.Itemstack; }
            set { paperSlot.Itemstack = value; paperSlot.MarkDirty(); }
        }
        /// <summary>
        /// scrollSlot?.Itemstack
        /// </summary>
        public ItemStack scrollStack
        {
            get { return scrollSlot?.Itemstack; }
            set { scrollSlot.Itemstack = value; scrollSlot.MarkDirty(); }
        }
        /// <summary>
        /// outputSlot?.Itemstack
        /// </summary>
        public ItemStack outputStack
        {
            get { return outputSlot?.Itemstack; }
            set { outputSlot.Itemstack = value; outputSlot.MarkDirty(); }
        }

    }
}
