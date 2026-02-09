using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using static System.Formats.Asn1.AsnWriter;
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

        public override bool Place(Entity player, Vec3d target)
        {

            IBlockAccessor blockAcc = player.World.GetBlockAccessor(true, true, true);
            BlockPos bpos = new BlockPos((int)target.X, (int)target.Y, (int)target.Z);
            Block block = blockAcc.GetBlock(bpos);

            switch (block.BlockMaterial)
            {
                case EnumBlockMaterial.Stone:
                    blockAcc.SetBlock(player.World.GetBlock(new AssetLocation("lava-still-7")).BlockId, bpos);
                    break;
                case EnumBlockMaterial.Air:
                    blockAcc.SetBlock(player.World.GetBlock(new AssetLocation("fire")).BlockId, bpos);

                    Vintagestory.API.Common.BlockEntity befire = blockAcc.GetBlockEntity(bpos);
                    befire?.GetBehavior<BEBehaviorBurning>()?.OnFirePlaced(bpos, bpos, (player as EntityPlayer).PlayerUID);
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
