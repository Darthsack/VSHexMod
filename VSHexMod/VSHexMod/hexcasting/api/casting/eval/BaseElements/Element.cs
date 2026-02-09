using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using static System.Net.Mime.MediaTypeNames;
using Vintagestory.GameContent;
using static VSHexMod.hexcasting.api.casting.arithmetic.operators.Spells;

namespace VSHexMod.hexcasting.api.casting.eval.BaseElements
{
    public class Element
    {
        public int strength = 0;
        public string type = "";

        public Element(int strength, string type)
        {
            this.strength = strength;
            this.type = type;
        }
        public Element()
        {
        }

        public virtual bool Effect(Entity player, Entity target) { 
            return false;
        }
        public virtual bool Effect(Entity player, Vec3d target) {
            return false;
        }

        public int progdmg = 5;
        public int vel = 1;
        public virtual bool Project(Entity player)
        {
            var loc = new AssetLocation("vshexmod:magic_missile-" + this.type);

            EntityProperties type = player.World.GetEntityType(loc);

            if (type == null)
            {
                throw new Exception("No such projectile exists - " + loc);
            }


            var entityarrow = player.World.ClassRegistry.CreateEntity(type) as Magic_Missile;
            entityarrow.FiredBy = player;
            entityarrow.Damage = progdmg;
            entityarrow.DamageTier = strength;
            entityarrow.ProjectileStack = new ItemStack(player.World.GetItem(new AssetLocation("stone-granite")));
            entityarrow.NonCollectible = true;


            Vec3d pos = player.ServerPos.XYZ.Add(0, player.LocalEyePos.Y, 0);
            Vec3d aheadPos = pos.AheadCopy(1, player.SidedPos.Pitch , player.SidedPos.Yaw );
            Vec3d velocity = (aheadPos - pos) * vel;


            entityarrow.ServerPos.SetPosWithDimension(player.SidedPos.BehindCopy(0.21).XYZ.Add(0, player.LocalEyePos.Y, 0));
            entityarrow.ServerPos.Motion.Set(velocity);
            entityarrow.Pos.SetFrom(entityarrow.ServerPos);
            entityarrow.World = player.World;
            entityarrow.SetRotation();

            player.World.SpawnEntity(entityarrow);
            
            return true;
        }
        public virtual bool Explode(Entity player, Vec3d target)
        {
            return false;
        }
        public virtual bool Place(Entity player, Vec3d target)
        {
            return false;
        }
        public virtual bool Enchant(Entity player, ItemStack Item)
        {
            return false;
        }
        public virtual bool PotionP(Entity player, ItemStack Item)
        {
            return false;
        }
        public virtual bool PotionN(Entity player, ItemStack Item)
        {
            return false;
        }

        public override string ToString()
        {
            return strength.ToString() +": "+ type;
        }

        public static bool operator ==(Element a, Element b) => a.strength == b.strength && a.type == b.type;
        public static bool operator !=(Element a, Element b) => a.strength != b.strength || a.type != b.type;
    }
}
