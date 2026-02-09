using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Server;
using Vintagestory.API.Datastructures;
using VSHexMod.Blocks;
using VSHexMod.BlockEntity;
using Vintagestory.ServerMods;
using VSHexMod.Crafting;
using Vintagestory.GameContent;
using VSHexMod.hexcasting.api.casting.eval;
using VSHexMod.hexcasting.common.items.magic;
using ProtoBuf;
using HarmonyLib;
using System.Collections.Generic;
using Vintagestory.API.Common.Entities;
using VSHexMod.hexcasting.common.lib.hex;
using VSHexMod.hexcasting.api.casting.eval.iota;
using System;
using System.Runtime.CompilerServices;
using VSHexMod.hexcasting.api.casting.arithmetic.operators;
using System.Threading;
using System.Linq;
using VSHexMod.EntityBehaviors;
using Vintagestory.Client.NoObf;
using Vintagestory.Server;
using VSHexMod.Gui;

namespace VSHexMod
{
    public class VSHexModModSystem : ModSystem
    {
        [ProtoContract]
        public class castPacket
        {
            [ProtoMember(1)]
            public string[] spell;
        }

        [ProtoContract]
        public class StartMediaBar
        {
            [ProtoMember(1)]
            public bool behaviorLoaded;
        }

        [ProtoContract]
        public class UseMedia
        {
            [ProtoMember(1)]
            public float[] Amount;


        }

        ICoreAPI api;

        public HexIotaTypes IotaTypesReg;

        public Dictionary<string, Type> HexRegistry = new Dictionary<string, Type>();
        public void RegisterHex(string signiture, Type eval)
        {
            HexRegistry.Add(signiture, eval);
        }
        public Dictionary<string, Type> IotaRegistry = new Dictionary<string, Type>();
        public void RegisterIota(string key, Type type)
        {
            IotaRegistry.Add(key, type);
        }


        // Called on server and client
        // Useful for registering block/entity classes on both sides
        public override void Start(ICoreAPI api)
        {
            this.api = api;

            //Mod.Logger.Notification("Hello from template mod: " + api.Side);
            api.RegisterBlockClass(Mod.Info.ModID + ".block.listscriber", typeof(BlockListScriber));
            api.RegisterBlockEntityClass(Mod.Info.ModID + ".blockentity.listscriber", typeof(BEListScriber));
            api.RegisterItemClass(Mod.Info.ModID + ".item.hexbook", typeof(ItemHexBook));
            api.RegisterItemClass(Mod.Info.ModID + ".item.focus", typeof(Focus));
            api.RegisterEntityBehaviorClass("media", typeof(BehaviorMedia));

            api.RegisterEntity("Magic_Missile", typeof(Magic_Missile));

            api.Network.RegisterChannel("castingbook").RegisterMessageType<castPacket>();
            api.Network.RegisterChannel("StartMedia").RegisterMessageType<StartMediaBar>().RegisterMessageType<UseMedia>();

            RegisterBaseHexxes();

            IotaTypesReg = new(api);

        }

        public override void StartServerSide(ICoreServerAPI api)
        {
            api.Network.GetChannel("castingbook").SetMessageHandler<castPacket>(onCastPacket);
            //Mod.Logger.Notification("Hello from template mod server side: " + Lang.Get("vshexmod:hello"));

            api.Event.PlayerNowPlaying += Event_PlayerNowPlaying;
        }

        private void Event_PlayerNowPlaying(IServerPlayer byPlayer)
        {
            bool b = false;
            if (byPlayer.Entity.WatchedAttributes.GetTreeAttribute("media") != null || byPlayer.Entity.HasBehavior<BehaviorMedia>())
            {
                BehaviorMedia media = new(byPlayer.Entity);
                media.Initialize();
                byPlayer.Entity.AddBehavior(media);
                byPlayer.BroadcastPlayerData();
                b = true;
            }
            ServerCoreAPI sapi = api as ServerCoreAPI;
            sapi.Network.GetChannel("StartMedia").SendPacket(new StartMediaBar() { behaviorLoaded = b }, byPlayer );
        }

        public override void StartClientSide(ICoreClientAPI api)
        {
            api.Network.GetChannel("StartMedia").SetMessageHandler<StartMediaBar>(onMediaStart).SetMessageHandler<UseMedia>(onMediaUsed);
            base.StartClientSide(api);
            //Mod.Logger.Notification("Hello from template mod client side: " + Lang.Get("vshexmod:hello"));
        }

        public void onCastPacket(IServerPlayer fromPlayer, castPacket packet)
        {
            //fromPlayer.SendMessage(0, "I see you!",EnumChatType.OwnMessage);

            if (!fromPlayer.Entity.HasBehavior<BehaviorMedia>())
            {
                BehaviorMedia media = new(fromPlayer.Entity);
                media.Initialize();
                fromPlayer.Entity.AddBehavior(media);
                fromPlayer.BroadcastPlayerData();
                ServerCoreAPI sapi = api as ServerCoreAPI;
                sapi.Network.GetChannel("StartMedia").SendPacket(new StartMediaBar() { behaviorLoaded = true }, fromPlayer);
            }


            //fromPlayer.SendMessage(0, string.Join(",", packet.spell),EnumChatType.OwnMessage);
            api.Logger.Event("StackSize: " + fromPlayer.Entity.ActiveHandItemSlot.Itemstack?.StackSize);
            
            ItemStack castingItem = fromPlayer.Entity.ActiveHandItemSlot.Itemstack;

            State castState = new(new ListIota(((string[])castingItem?.Attributes["spell"].GetValue()).Select((r) => (Iota)new PatternIota(ResolvedPattern.FromString(r).pattern)).ToList()));

            ThreadStart star = new(() => new Spells.Eval(fromPlayer.Entity,api, castState));
            Thread vm = new(star);
            vm.Start();

        }

        public void onMediaUsed(UseMedia packet)
        {
            ClientCoreAPI capi = api as ClientCoreAPI;
            IClientPlayer byPlayer = capi.World.Player;

            BehaviorMedia med = byPlayer.Entity.GetBehavior<BehaviorMedia>();

            med.SetMedia(packet.Amount[0], packet.Amount[1]);
            med.MarkDirty();
        }

        HudMediaBar mediaBar;

        public void onMediaStart(StartMediaBar packet)
        {
            ClientCoreAPI capi = api as ClientCoreAPI;
            IClientPlayer byPlayer = capi.World.Player;

            if (packet.behaviorLoaded && !byPlayer.Entity.HasBehavior<BehaviorMedia>())
            {
                BehaviorMedia media = new(byPlayer.Entity);
                media.Initialize();
                byPlayer.Entity.AddBehavior(media);

            }
            if (packet.behaviorLoaded && mediaBar is null)
            {
                mediaBar = new HudMediaBar(capi);
                mediaBar.ComposeGuis();
            }


        }
        private void RegisterBaseHexxes()
        {
            Spells.RegisterArithmetic(api);
        }
    }
    public static partial class apiHelper
    {
        public static void RegisterHex(this ICoreAPI api, string signiture, Type eval)
        {
            api.ModLoader.GetModSystem<VSHexModModSystem>().RegisterHex(signiture, eval);
        }

        public static void RegisterIota(this ICoreAPI api, string key, Iota type)
        {
            api.ModLoader.GetModSystem<VSHexModModSystem>().IotaTypesReg.registerType(key, type);
        }

        public static Iota GetEmptyIota(this ICoreAPI api, string key)
        {
            return api.ModLoader.GetModSystem<VSHexModModSystem>().IotaTypesReg.type(key);
        }

        public static string GetIotaKey(this ICoreAPI api, Iota key)
        {
            return api.ModLoader.GetModSystem<VSHexModModSystem>().IotaTypesReg.getKey(key);
        }

    }
}
