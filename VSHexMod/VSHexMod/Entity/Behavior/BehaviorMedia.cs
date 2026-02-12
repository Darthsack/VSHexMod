using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;
using Vintagestory.Server;
using VSHexMod.hexcasting.api.casting.eval.iota;
using VSHexMod.hexcasting.api.casting.eval;
using static VSHexMod.VSHexModModSystem;
using Vintagestory.GameContent;
using VSHexMod.hexcasting.api.casting.eval.BaseElements;
using HarmonyLib;

namespace VSHexMod.EntityBehaviors
{
    public enum EnumHexStrain { 
        None,
        Stalled,
        Injured
    }
    public class BehaviorMedia : EntityBehavior
    {
        ITreeAttribute MediaTree;
        ITreeAttribute Affinities;
        EntityAgent entityAgent;

        float secondsSinceLastUpdate;


        public float Media
        {
            get { return MediaTree.GetFloat("currentmedia"); }
            set { MediaTree.SetFloat("currentmedia", value); entity.WatchedAttributes.MarkPathDirty("media"); }
        }
        public float TrueMax
        {
            get { return MediaTree.GetFloat("trueMax", 5000); }
            set { MediaTree.SetFloat("trueMax", value); entity.WatchedAttributes.MarkPathDirty("media"); }
        }

        public float MaxMedia
        {
            get { return MediaTree.GetFloat("maxmedia"); }
            set { MediaTree.SetFloat("maxmedia", value); entity.WatchedAttributes.MarkPathDirty("media"); }
        }
        

        public override string PropertyName()
        {
            return "media";
        }

        public void MarkDirty()
        {
            entity.WatchedAttributes.MarkPathDirty("media");
        }

        public Dictionary<string,int> GetAffinities()
        {
            string[] Keys = Array.Empty<string>();
            Affinities.Foreach((x) => Keys = Keys.AddToArray(x.Key));
            Dictionary<string, int> temp = new();

            foreach (string key in Keys) {
                if (key.Last() == 'X')
                    temp.Add(key.SkipLast(1).Join(delimiter: ""), Affinities.GetInt(key));
            }
            return temp;
        }
        public int GetAffinities(string element)
        {
            return Affinities.GetVec3i(element, new Vec3i(1,0,0)).X;
        }
        public void AddEXP(string element, int amount = 0)
        {
            Vec3i temp = Affinities.GetVec3i(element, new Vec3i());
            int total = amount + temp.Y;
            if (total >= GetEXPForNextLVL(temp.X))
            {
                if(temp.X < 5)
                {
                    total -= GetEXPForNextLVL(temp.X);
                    temp.X++;
                    temp.Y = total;
                    Affinities.SetVec3i(element, temp);
                    MarkDirty();
                    AddEXP(element);
                }
            }
            else if(amount != 0)
            {
                temp.Y = total;
                Affinities.SetVec3i(element, temp);
                MarkDirty();
            }
        }
        public static int GetEXPForNextLVL(int CurLVL)
        {
            return CurLVL * CurLVL * CurLVL * 100;
        }
        public void DamageAffinity(string element, int amount = 1)
        {
            Vec3i temp = Affinities.GetVec3i(element);
            temp.X -= amount;
            Affinities.SetVec3i(element, temp);
            MarkDirty();
        }

        public BehaviorMedia(Entity entity) : base(entity)
        {
            entityAgent = entity as EntityAgent;
        }

        public override void Initialize(EntityProperties properties = null, JsonObject typeAttributes = null)
        {
            this.MediaTree = new TreeAttribute();
            this.Affinities = new TreeAttribute();
            if (typeAttributes is null)
            {
                typeAttributes = new("");
            }
            var MediaTree = entity.WatchedAttributes.GetTreeAttribute("media");

            var Affinities = entity.WatchedAttributes.GetTreeAttribute("affinities");

            if (Affinities == null)
            {
                Affinities = this.Affinities;
                entity.WatchedAttributes.SetAttribute("affinities", this.Affinities);
                MarkDirty();
            }

            if (MediaTree == null)
            {
                
                MediaTree = this.MediaTree;
                entity.WatchedAttributes.SetAttribute("media", this.MediaTree);

                MaxMedia = typeAttributes["maxmedia"].AsFloat(20);
                Media = typeAttributes["currentmedia"].AsFloat(MaxMedia);
                MarkDirty();
                return;
            }

            

            float baseMaxHealth = MediaTree.GetFloat("maxmedia");
            if (baseMaxHealth == 0)
            {
                MaxMedia = typeAttributes["maxmedia"].AsFloat(20);
                MarkDirty();
            }
            this.MediaTree = MediaTree;
            // Otherwise we don't need to read and immediately set the same values back to the mediaTree, nor mark it as dirty: and a MarkDirty() here messes up EntityPlayer media on joining a game, if done prior to initialising BehaviorHunger and its MaxMediaModifiers
            // thanks health behavior!
            secondsSinceLastUpdate = (float)entity.World.Rand.NextDouble();   // Randomise which game tick these update, a starting server would otherwise start all loaded entities with the same zero timer
        }

        public override void OnGameTick(float deltaTime)
        {
            //if (entity.World.Side == EnumAppSide.Client) return;

            secondsSinceLastUpdate += deltaTime;

            if (secondsSinceLastUpdate >= 1)
            {
                if (entity.Alive)
                {
                    var media = Media;  // higher performance to read this TreeAttribute only once
                    var maxMedia = MaxMedia;
                    if (media < maxMedia)
                    {
                        var mediaRegenSpeed = entity is EntityPlayer ? entity.Api.World.Config.GetString("playerHealthRegenSpeed", "1").ToFloat() : entity.WatchedAttributes.GetFloat("regenSpeed", 1);
                        
                        if (entity.GetBehavior<EntityBehaviorTiredness>().IsSleeping)
                        {
                            mediaRegenSpeed *= maxMedia/10;
                        }
                        // previous value = 0.01 , -> 0.01 / 30 = 0.000333333f (60 * 0,5 = 30 (SpeedOfTime * CalendarSpeedMul))
                        var mediaRegenPerGameSecond = 0.000333333f * mediaRegenSpeed;
                        var multiplierPerGameSec = secondsSinceLastUpdate * entity.Api.World.Calendar.SpeedOfTime * entity.Api.World.Calendar.CalendarSpeedMul;

                        Media = Math.Min(media + multiplierPerGameSec * mediaRegenPerGameSecond, maxMedia);
                        MarkDirty();
                    }
                }
                secondsSinceLastUpdate = 0;
            }
        }

        public bool UseMedia(float Amount)
        {
            if (Amount > Media)
                return false;
            
            Media -= Amount;
            MaxMedia += (1f-(Media / MaxMedia)) * (Amount);

            if (entity.Api is ServerCoreAPI sapi)
            {
                if(entity is EntityPlayer player)
                {
                    sapi.Network.GetChannel("StartMedia").SendPacket(new UseMedia() { Amount =new float[] { Media, MaxMedia } }, player.Player as ServerPlayer);
                }
            }
            
            return true;
        }
        public bool UseMedia(float Amount, string element, EnumHexStrain pain = EnumHexStrain.None)
        {
            if (Amount / Math.Pow(1.08, GetAffinities(element) - 1) > Media ||(pain == EnumHexStrain.Injured && GetAffinities(element) <= 1))
                return false;

            Media -= (float)(Amount / Math.Pow(1.08,GetAffinities(element) - 1));
            if (pain == EnumHexStrain.None)
            {
                MaxMedia += (1f - (Media / MaxMedia)) * (Amount / (float)Math.Pow(1.1, GetAffinities(element) - 1));
                MaxMedia = Math.Max(Math.Min(MaxMedia, TrueMax), 20);
                Media = Math.Min(Media, MaxMedia);
                AddEXP(element, (int)Amount);
            }
            else if (pain == EnumHexStrain.Injured)
            {
                DamageAffinity(element);
                MaxMedia -= Amount / 2;
            }

            if (entity.Api is ServerCoreAPI sapi)
            {
                if (entity is EntityPlayer player)
                {
                    sapi.Network.GetChannel("StartMedia").SendPacket(new UseMedia() { Amount = new float[] { Media, MaxMedia } }, player.Player as ServerPlayer);
                }
            }
            return true;
        }

        public bool SetMedia(float Amount, float AmountMax)
        {
            if (entity.Api is ServerCoreAPI sapi)
            {
                if (entity is EntityPlayer player)
                {
                    sapi.Network.GetChannel("StartMedia").SendPacket(new UseMedia() { Amount = new float[] { Amount, AmountMax } }, player.Player as ServerPlayer);
                }
            }

            Media = Amount;
            MaxMedia = AmountMax;

            return true;
        }
        public bool CanUseMedia(float Amount)
        {
            
            if (Amount > Media)
                return false;

            return true;
        }
        public bool CanUseMedia(float Amount, string element, EnumHexStrain pain = EnumHexStrain.None)
        {

            if ((Amount / Math.Pow(1.08, GetAffinities(element) - 1)) > Media || (pain == EnumHexStrain.Injured && GetAffinities(element) <= 1))
                return false;

            return true;
        }

        public override void GetInfoText(StringBuilder infotext)
        {
            var capi = entity.Api as ICoreClientAPI;
            if (capi?.World.Player?.WorldData?.CurrentGameMode == EnumGameMode.Creative)
            {
                infotext.AppendLine(Lang.Get("Media: {0}/{1}", Media, MaxMedia));
            }
        }
    }
    public static class EntityHelper
    {
        public static bool CanUseMedia(this Entity entity, float Amount)
        {
            if (entity.HasBehavior<BehaviorMedia>())
            {
                BehaviorMedia med = entity.GetBehavior<BehaviorMedia>();
                return med.CanUseMedia(Amount);
            }
            return false;
        }
        public static bool CanUseMedia(this Entity entity, float Amount, string type, EnumHexStrain pain = EnumHexStrain.None)
        {
            if (entity.HasBehavior<BehaviorMedia>())
            {
                BehaviorMedia med = entity.GetBehavior<BehaviorMedia>();
                return med.CanUseMedia(Amount, type, pain);
            }
            return false;
        }
        public static bool UseMedia(this Entity entity, float Amount)
        {
            if (entity.HasBehavior<BehaviorMedia>())
            {
                BehaviorMedia med = entity.GetBehavior<BehaviorMedia>();
                return med.UseMedia(Amount);
            }
            return false;
        }
        public static bool UseMedia(this Entity entity, float Amount, string type, EnumHexStrain pain = EnumHexStrain.None)
        {
            if (entity.HasBehavior<BehaviorMedia>())
            {
                BehaviorMedia med = entity.GetBehavior<BehaviorMedia>();
                return med.UseMedia(Amount, type, pain);
            }
            return false;
        }
        public static float GetMedia(this Entity entity)
        {
            if (entity.HasBehavior<BehaviorMedia>())
            {
                BehaviorMedia med = entity.GetBehavior<BehaviorMedia>();
                return med.Media;
            }
            return 0;
        }
        public static Dictionary<string,int> GetAffinities(this Entity entity)
        {
            if (entity.HasBehavior<BehaviorMedia>())
            {
                BehaviorMedia med = entity.GetBehavior<BehaviorMedia>();
                return med.GetAffinities();
            }
            return new();
        }
        public static int GetAffinities(this Entity entity, string type)
        {
            if (entity.HasBehavior<BehaviorMedia>())
            {
                BehaviorMedia med = entity.GetBehavior<BehaviorMedia>();
                return med.GetAffinities(type);
            }
            return new();
        }
    }
}
