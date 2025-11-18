using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;
using VSHexMod.hexcasting.api.casting.eval.iota;
using CompactExifLib;

namespace VSHexMod.hexcasting.api.casting.eval.BaseElements
{
    public class Tyerraium : Element
    {
        public Tyerraium(int strength) : base(strength, "tyerra") { }

        public override bool Project(Entity player)
        {
            var loc = new AssetLocation("vshexmod:magic_missile-" + this.type);

            EntityProperties type = player.World.GetEntityType(loc);

            if (type == null)
            {
                throw new Exception("No such projectile exists - " + loc);
            }


            int p = 4 * strength + 1;
            float dmg = 1.5f;

            Dictionary<Entity,int> targets = new();

            for (int i = 0; i < p; i++)
            {

                //var entityarrow = player.World.ClassRegistry.CreateEntity(type) as EntityProjectile;
                //entityarrow.FiredBy = player;
                //entityarrow.Damage = dmg;
                //entityarrow.DamageTier = strength;
                //entityarrow.ProjectileStack = new ItemStack(player.World.GetItem(new AssetLocation("stone-granite")));
                //entityarrow.NonCollectible = true;
                double rndx = player.World.Rand.NextDouble() * 2 - 1;
                double rndy = player.World.Rand.NextDouble() * 2 - 1;

                float acc = ((float)Math.Ceiling(i / 4f) / 8f);
                double rndpitch = rndx * acc * 0.75f;
                double rndyaw = rndy * acc * 0.75f;

                Vec3d pos = player.ServerPos.XYZ.Add(0, player.LocalEyePos.Y, 0);
                Vec3d aheadPos = pos.AheadCopy(15, player.SidedPos.Pitch + rndpitch, player.SidedPos.Yaw + rndyaw);
                //Vec3d velocity = (aheadPos - pos) * 1;

                BlockSelection B = new();
                EntitySelection E = new();
                player.World.RayTraceForSelection(pos, aheadPos, ref B, ref E, null, new EntityFilter((n) => n != player));

                if (E?.Entity is not null)
                {
                    if (targets.ContainsKey(E.Entity))
                    {
                        targets[E.Entity]++;
                    }
                    else
                    {
                        targets.Add(E.Entity, 1);
                    }
                } 
                else if (B?.Position is not null)
                {
                    player.World.PlaySoundAt(new AssetLocation("sounds/thud"), B.Position.X + B.HitPosition.X, B.Position.Y + B.HitPosition.Y, B.Position.Z + B.HitPosition.Z,null,false,32,0.25f);
                    player.World.SpawnCubeParticles(B.Position, B.Position.ToVec3d() + B.HitPosition, 0.2f, 5, 0.5f);
                }

                //entityarrow.ServerPos.SetPosWithDimension(player.SidedPos.BehindCopy(0.21).XYZ.Add(0, player.LocalEyePos.Y, 0));
                //entityarrow.ServerPos.Motion.Set(velocity);
                //entityarrow.Pos.SetFrom(entityarrow.ServerPos);
                //entityarrow.World = player.World;
                //entityarrow.SetRotation();

                //player.World.SpawnEntity(entityarrow);
            }
            if (targets.Count > 0)
            {
                bool didDamage = false;
                foreach (var target in targets)
                {
                    didDamage = didDamage || target.Key.ReceiveDamage(new DamageSource()
                    {
                        Source = player != null ? EnumDamageSource.Player : EnumDamageSource.Entity,
                        SourceEntity = null,
                        CauseEntity = player,
                        Type = EnumDamageType.PiercingAttack,
                        DamageTier = strength
                    }, dmg * target.Value);
                }

                if (player is EntityPlayer && didDamage)
                {
                    player.World.PlaySoundFor(new AssetLocation("sounds/player/projectilehit"), (player as EntityPlayer).Player, false, 24);
                }
            }

            return true;
        }
    }
}
