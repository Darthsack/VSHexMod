using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using VSHexMod.EntityBehaviors;

namespace VSHexMod.hexcasting.api.casting.eval.BaseElements
{
    public class Lunarium : Element
    {
        public Lunarium(int strength) : base(strength, "lunar") { }

        public override bool Place(Entity player, Vec3d target)
        {
            if (!player.CanUseMedia(10))
                return false;

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

            player.UseMedia(10);
            return true;
        }
    }
}
