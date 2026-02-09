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
    public class Oceanium : Element
    {
        public Oceanium(int strength) : base(strength, "oceanium") {
            progdmg = 5 * strength;
        }
        public override bool Effect(Entity player, Entity target)
        {
            if (!player.CanUseMedia(1))
                return false;
            target.IsOnFire = false;
            player.UseMedia(1);
            return true;
        }

        public override bool Effect(Entity player, Vec3d pos)
        {
            if (!player.CanUseMedia(1))
                return false;
            BlockPos block_pos = new BlockPos((int)pos.X, (int)pos.Y, (int)pos.Z);

            //Vintagestory.API.Common.BlockEntity block = player.World.BlockAccessor.GetBlockEntity(block_pos);

            IBlockAccessor blockAcc = player.World.GetBlockAccessor(true, true, true);

            Block block = blockAcc.GetBlock(block_pos);

            BEBehaviorTemperatureSensitive beits = block?.GetBEBehavior<BEBehaviorTemperatureSensitive>(block_pos);
            BlockEntityFarmland be = blockAcc.GetBlockEntity(block_pos) as BlockEntityFarmland;
            BEBehaviorBurning beburningBh = blockAcc.GetBlockEntity(block_pos)?.GetBehavior<BEBehaviorBurning>();

            if (beburningBh != null) beburningBh.KillFire(false);
            else if (beits is not null)
            {
                beits.OnWatered(200);
            }
            else if (be != null)
            {
                be.WaterFarmland(10);
            }
            else
            {
                int fireId = player.World.GetBlock(new AssetLocation("fire")).BlockId;

                for (int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        for (int z = -1; z <= 1; z++)
                        {
                            BlockPos bpos = block_pos.AddCopy(x, y, z);

                            block = blockAcc.GetBlock(bpos);

                            if (block.BlockId == fireId)
                            {
                                blockAcc.SetBlock(0, bpos);
                                blockAcc.MarkBlockModified(bpos);
                            }
                        }
                    }
                }
            }

            player.UseMedia(1);
            return true;
        }

        public override bool Place(Entity player, Vec3d target)
        {

            IBlockAccessor blockAcc = player.World.GetBlockAccessor(true, true, true);
            BlockPos bpos = new BlockPos((int)target.X, (int)target.Y, (int)target.Z);
            Block block = blockAcc.GetBlock(bpos);

            switch (block.BlockMaterial)
            {
                case EnumBlockMaterial.Air:

                    blockAcc.SetBlock(player.World.GetBlock(new AssetLocation("water-still-7")).BlockId, bpos);
                    blockAcc.MarkBlockModified(bpos);
                    break;

                default:
                    return false;

            }

            return true;
        }
    }
}
