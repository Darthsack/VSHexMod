using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.GameContent;
using VSHexMod.EntityBehaviors;
using VSHexMod.hexcasting.api.casting.eval.iota;
using static VSHexMod.hexcasting.api.casting.arithmetic.operators.Spells;

namespace VSHexMod.hexcasting.api.casting.eval.BaseElements
{
    public class Solarium : Element
    {
        public Solarium(int strength) : base(strength, "solar") { }

        public override bool Effect(Entity player, Entity target)
        {
            if (!player.CanUseMedia(1))
                return false;
            target.Ignite();
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

            IIgnitable ign = block?.GetInterface<IIgnitable>(player.World, block_pos);
            if (ign is not null)
            {
                ign.OnTryIgniteBlock((EntityAgent)player, block_pos, 100);
                EnumHandling handling = EnumHandling.PreventDefault;
                ign.OnTryIgniteBlockOver((EntityAgent)player, block_pos, 100, ref handling);
            }
            else
            {

                for (int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        for (int z = -1; z <= 1; z++)
                        {
                            BlockPos bpos = block_pos.AddCopy(x, y, z);

                            block = blockAcc.GetBlock(bpos);

                            if (block.BlockId == 0)
                            {
                                blockAcc.SetBlock(player.World.GetBlock(new AssetLocation("fire")).BlockId, bpos);
                                Vintagestory.API.Common.BlockEntity befire = blockAcc.GetBlockEntity(bpos);
                                befire?.GetBehavior<BEBehaviorBurning>()?.OnFirePlaced(bpos, block_pos, (player as EntityPlayer).PlayerUID);
                            }
                        }
                    }
                }
            }

            player.UseMedia(1);
            return true;
        }
    }
}
