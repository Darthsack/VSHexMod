using System;
using System.Collections;
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
using static VSHexMod.VSHexModModSystem;

namespace VSHexMod.hexcasting.api.casting.eval.BaseElements
{
    public class Caelium : Element
    {
        public Caelium(int strength) : base(strength, "caelum") { }

        public override bool Effect(Entity player, Entity target)
        {
            if (!player.CanUseMedia(20 * strength, type))
                return false;


            float cosPitch = GameMath.Cos(target.SidedPos.Pitch);
            float sinPitch = GameMath.Sin(target.SidedPos.Pitch);

            float cosYaw = GameMath.Cos(target.SidedPos.Yaw);
            float sinYaw = GameMath.Sin(target.SidedPos.Yaw);

            Vec3d launch = new Vec3d(cosPitch * -sinYaw, sinPitch, cosPitch * -cosYaw) * strength;

            if ((target as EntityPlayer)?.Player is IServerPlayer plr)
            {
                (target.Api as ICoreServerAPI).Network.GetChannel("MiscHexStuff").SendPacket(new LaunchPacket() { Vec = launch }, plr);
            }
            else
                target.SidedPos.Motion -= launch ;
            player.UseMedia(20 * strength, type);

            return true;
        }
        public override bool Effect(Entity player, Vec3d pos)
        {
            if (!player.CanUseMedia(20 * strength, type))
                return false;
            IBlockAccessor blockAcc = player.World.GetBlockAccessor(true, true, true);
            BlockEntityBarrel BE = blockAcc.GetBlockEntity<BlockEntityBarrel>(new BlockPos((int)pos.X, (int)pos.Y, (int)pos.Z));
            if (BE != null)
            {
                if (BE.Sealed)
                {
                    BE.SealedSinceTotalHours -= strength ;
                    BE.MarkDirty();
                    player.UseMedia(20 * strength, type);
                    return true;
                }
            }
            return false;
        }

        public override bool Place(Entity player, Vec3d target)
        {

            //IBlockAccessor blockAcc = player.World.GetBlockAccessor(true, true, true);
            //BlockPos bpos = new BlockPos((int)target.X, (int)target.Y, (int)target.Z);

            WeatherSystemBase weatherSys = player.Api.ModLoader.GetModSystem<WeatherSystemBase>();

            weatherSys.SpawnLightningFlash(target);

            return true;
        }
    }
}
