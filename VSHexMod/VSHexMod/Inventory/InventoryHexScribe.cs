using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Datastructures;
using Vintagestory.GameContent;

namespace VSHexMod.Inventory
{
    public class InventoryHexScribe : InventoryBase, ISlotProvider
    {

        ItemSlot[] slots = new ItemSlot[0];
        public BlockPos pos;

        public ItemSlot[] Slots
        {
            get { return slots; }
        }

        int defaultStorageType = (int)(EnumItemStorageFlags.General | EnumItemStorageFlags.Agriculture | EnumItemStorageFlags.Alchemy | EnumItemStorageFlags.Jewellery | EnumItemStorageFlags.Metallurgy | EnumItemStorageFlags.Outfit);

        public InventoryHexScribe(string inventoryID, ICoreAPI api) : base(inventoryID, api)
        {
            // slot 0 = input
            // slot 1 = display
            // slot 2 = output
            // slot 3 = blood
            slots = GenEmptySlots(4);
            //cookingSlots = new ItemSlot[] { slots[3], slots[4], slots[5], slots[6] };
            baseWeight = 4f;

        }

        public override WeightedSlot GetBestSuitedSlot(ItemSlot sourceSlot, ItemStackMoveOperation op, List<ItemSlot> skipSlots = null)
        {
            WeightedSlot slot = base.GetBestSuitedSlot(sourceSlot, op, skipSlots);

            return slot;
        }

        public override float GetSuitability(ItemSlot sourceSlot, ItemSlot targetSlot, bool isMerge)
        {
            ItemStack stack = sourceSlot.Itemstack;

            if (targetSlot == slots[1] && (stack.Collectible is BlockSmeltingContainer || stack.Collectible is BlockCookingContainer)) return 2.2f;

            if (targetSlot == slots[0] && (stack.Collectible.CombustibleProps == null || stack.Collectible.CombustibleProps.BurnTemperature <= 0)) return 0;
            if (targetSlot == slots[1] && (stack.Collectible.CombustibleProps == null || stack.Collectible.CombustibleProps.SmeltedStack == null)) return 0.5f;


            return base.GetSuitability(sourceSlot, targetSlot, isMerge);
        }

        public override ItemSlot this[int slotId]
        {
            get
            {
                if (slotId < 0 || slotId >= Count) return null;
                return slots[slotId];
            }
            set
            {
                if (slotId < 0 || slotId >= Count) throw new ArgumentOutOfRangeException(nameof(slotId));
                if (value is null) throw new ArgumentNullException(nameof(value));
                slots[slotId] = value;
            }
        }

        public InventoryHexScribe(string className, string instanceID, ICoreAPI api) : base(className, instanceID, api)
        {
            slots = GenEmptySlots(4);
            baseWeight = 4f;
        }

        public override bool CanContain(ItemSlot sinkSlot, ItemSlot sourceSlot)
        {
            int slotid = GetSlotId(sinkSlot);
            return slotid < 3 || base.CanContain(sinkSlot, sourceSlot);
        }
        public override int Count
        {
            get { return slots.Length; }
        }

        public override void LateInitialize(string inventoryID, ICoreAPI api)
        {
            base.LateInitialize(inventoryID, api);

        }

        public override void FromTreeAttributes(ITreeAttribute tree)
        {
            List<ItemSlot> modifiedSlots = new List<ItemSlot>();
            slots = SlotsFromTreeAttributes(tree, slots, modifiedSlots);
            for (int i = 0; i < modifiedSlots.Count; i++) DidModifyItemSlot(modifiedSlots[i]);

        }
        public override void ToTreeAttributes(ITreeAttribute tree)
        {
            SlotsToTreeAttributes(slots, tree);
        }
        protected override ItemSlot NewSlot(int i)
        {
            if (i == 0) return new ItemSlotSurvival(this);
            if (i == 1) return new ItemSlotSurvival(this);
            if (i == 2) return new ItemSlotOutput(this);

            return new ItemSlotWatertight(this, 100);
        }
    }
}
