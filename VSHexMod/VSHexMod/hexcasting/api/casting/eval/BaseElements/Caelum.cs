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
    public class Caelium : Element
    {
        public Caelium(int strength) : base(strength, "caelum") { }

        public override bool Place(Entity player, Vec3d target)
        {
            if (!player.CanUseMedia(10))
                return false;

            //IBlockAccessor blockAcc = player.World.GetBlockAccessor(true, true, true);
            //BlockPos bpos = new BlockPos((int)target.X, (int)target.Y, (int)target.Z);

            WeatherSystemBase weatherSys = player.Api.ModLoader.GetModSystem<WeatherSystemBase>();

            weatherSys.SpawnLightningFlash(target);

            player.UseMedia(10);
            return true;
        }
    }
}
