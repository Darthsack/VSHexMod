using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;
using VSHexMod.EntityBehaviors;

namespace VSHexMod.hexcasting.api.casting.eval.BaseElements
{
    public class Lunarium : Element
    {
        public Lunarium(int strength) : base(strength, "lunar") { }

        public override bool Effect(Entity player, Entity target)
        {
            if (!player.CanUseMedia(30, type))
                return false;
            target.Revive();
            return true;
        }
        public override bool Effect(Entity player, Vec3d pos)
        {
            if (!player.CanUseMedia(20, type))
                return false;

            IBlockAccessor blockAcc = player.World.GetBlockAccessor(true, true, true);

            BlockEntityGeneric BE = blockAcc.GetBlockEntity<BlockEntityGeneric>(new BlockPos((int)pos.X, (int)pos.Y, (int)pos.Z));
            if (BE != null)
            {
                BEBehaviorShapeFromAttributes Reparable =  BE.GetBehavior<BEBehaviorShapeFromAttributes>();
                if(Reparable != null)
                {
                    if (Reparable.reparability > 0)
                    {
                        Reparable.repairState = 100;
                        Reparable.Blockentity.MarkDirty();
                        player.UseMedia(20, type);
                        return true;
                    }
                }
            }

            return false;
        }



        public override bool Place(Entity player, Vec3d target)
        {

            IBlockAccessor blockAcc = player.World.GetBlockAccessor(true, true, true);
            BlockPos bpos = new BlockPos((int)target.X, (int)target.Y, (int)target.Z);
            Block block = blockAcc.GetBlock(bpos);

            switch (block.BlockMaterial)
            {
                case EnumBlockMaterial.Liquid:
                case EnumBlockMaterial.Air:
                    blockAcc.SetBlock(player.World.GetBlock(new AssetLocation("vshexmod:mageblock")).BlockId, bpos);

                    break;
                //case EnumBlockMaterial.Liquid:
                //    if(block.LiquidCode == "water")
                //    {

                //    }
                //    break;
                default:
                    return false;
            }

            return true;
        }
    }
}
