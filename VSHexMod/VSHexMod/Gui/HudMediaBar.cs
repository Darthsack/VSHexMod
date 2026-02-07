using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.Client.NoObf;
using Vintagestory.GameContent;
using VSHexMod.EntityBehaviors;

namespace VSHexMod.Gui
{
    public class HudMediaBar : HudElement
    {
        float lastMedia;
        float lastMaxMedia;


        GuiElementStatbar mediabar;
        long listenerId;


        public HudMediaBar(ICoreClientAPI capi) : base(capi)
        {
            listenerId = capi.Event.RegisterGameTickListener(this.OnGameTick, 20);


            ComposeGuis();
        }

        private void OnGameTick(float dt)
        {
            UpdateHealth();
        }


        void UpdateHealth()
        {
            BehaviorMedia med = capi.World.Player.Entity.GetBehavior<BehaviorMedia>();
            //ITreeAttribute mediaTree = capi.World.Player.Entity.WatchedAttributes.GetTreeAttribute("media");
            if (med == null) return;

            float? media = med.Media;
            float? maxMedia = med.MaxMedia;

            if (media == null || maxMedia == null) return;
            if (lastMedia == media && lastMaxMedia == maxMedia) return;
            if (mediabar == null) return;

            mediabar.SetLineInterval(1);
            mediabar.SetValues((float)media, 0, (float)maxMedia);

            lastMedia = (float)media;
            lastMaxMedia = (float)maxMedia;
        }

        public void ComposeGuis()
        {
            float width = 348;
            ElementBounds dialogBounds = new ElementBounds()
            {
                Alignment = EnumDialogArea.CenterBottom,
                BothSizing = ElementSizing.Fixed,
                fixedWidth = width,
                fixedHeight = 10,
                fixedY = -75,
                fixedX = 424
            }.WithFixedAlignmentOffset(0, 5);

            ElementBounds mediaBarBounds = ElementBounds.Fixed(0, 0, width, 10);
            dialogBounds.WithChildren(mediaBarBounds);
            ITreeAttribute mediaTree = capi.World.Player.Entity.WatchedAttributes.GetTreeAttribute("health");
            string key = "mediabar-" + capi.World.Player.PlayerUID;
            Composers["mediabar"] =
                capi.Gui
                .CreateCompo(key, dialogBounds.FixedGrow(width, 20))
                    .AddIf(mediaTree != null)
                        .AddInvStatbar(mediaBarBounds, new double[] { 0.5,0,0.7 }, "mediastatbar")
                    .EndIf()
                .Compose()
            ;

            mediabar = Composers["mediabar"].GetStatbar("mediastatbar");
            TryOpen();
        }

        // Can't be closed
        public override bool TryClose()
        {
            return base.TryClose();
        }

        public override bool ShouldReceiveKeyboardEvents()
        {
            return false;
        }


        public override void OnRenderGUI(float deltaTime)
        {
            base.OnRenderGUI(deltaTime);
        }


        // Can't be focused
        public override bool Focusable => false;

        // Can't be focused
        protected override void OnFocusChanged(bool on)
        {

        }

        public override void OnMouseDown(MouseEvent args)
        {
            // Can't be clicked
        }

        public override void Dispose()
        {
            base.Dispose();

            capi.Event.UnregisterGameTickListener(listenerId);
        }
    }
}
