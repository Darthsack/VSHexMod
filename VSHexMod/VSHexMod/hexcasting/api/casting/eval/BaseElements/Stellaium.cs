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
    public class Stellarium : Element
    {
        public Stellarium(int strength) : base(strength, "stella") {
            vel = strength;
        }
        public override bool Effect(Entity player, Entity target)
        {
            if (!player.CanUseMedia(20 * strength, type))
                return false;
            EntityBehaviorTemporalStabilityAffected TemporalStability = target.GetBehavior<EntityBehaviorTemporalStabilityAffected>();
            if (TemporalStability != null)
            {
                TemporalStability.OwnStability += strength / 20f;
            }
            return true;
        }
        public override bool Effect(Entity player, Vec3d pos)
        {
            if (!player.CanUseMedia(20 * strength, type))
                return false;

            return true;
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
                    blockAcc.SetBlock(player.World.GetBlock(new AssetLocation("vshexmod:magelight")).BlockId, bpos);

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
