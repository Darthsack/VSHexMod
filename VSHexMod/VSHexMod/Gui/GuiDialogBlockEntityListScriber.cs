
using ProtoBuf;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;
using VSHexMod.hexcasting.api.casting.eval;
using VSHexMod.hexcasting.api.casting.math;
using VSHexMod.hexcasting.client.gui;

namespace VSHexMod.Gui
{
    public class GuiDialogBlockEntityListScriber : GuiDialogBlockEntity
    {
        long lastRedrawMs;
        EnumPosFlag screenPos;

        List<ResolvedPattern> patterns;
        public GuiDialogBlockEntityListScriber(string dlgTitle, InventoryBase Inventory, BlockPos bePos, ICoreClientAPI capi,List<ResolvedPattern> patterns) : base(dlgTitle, Inventory, bePos, capi)
        {
            if (IsDuplicate) return;

            this.patterns = patterns;

            SetupDialog(dlgTitle, Inventory, bePos);
        }



        private void SetupDialog(string Title, InventoryBase Inventory, BlockPos bePos)
        {
            /*
             * ELementBounds are essentially a parented 2D rectangle which are used to determine the positions of UI elements.
             * In this case, we're using an autosized dialog that is centered in the center middle of the screen.
             */
            ElementBounds dialogBounds = ElementStdBounds.AutosizedMainDialog.WithAlignment(EnumDialogArea.CenterMiddle);

            //This creates a fixed position rectangle with a size of 300x100 pixels, and 40 pixels from the top of the gui box.
            ElementBounds textBounds = ElementBounds.Fixed(120, 20, 260, 355);

            //Inventory Handling
            ElementBounds PaperBounds = ElementStdBounds.SlotGrid(EnumDialogArea.LeftMiddle, 60, -60, 1, 1);
            ElementBounds ScrollBounds = ElementStdBounds.SlotGrid(EnumDialogArea.LeftMiddle, 0, 1, 1, 1);
            ElementBounds OutputBounds = ElementStdBounds.SlotGrid(EnumDialogArea.LeftMiddle, 60, 60, 1, 1);

            //Buttons Handling
            ElementBounds cancelButtonBounds = ElementBounds.FixedSize(0, 0).WithAlignment(EnumDialogArea.LeftBottom).WithFixedPadding(8, 2).WithFixedAlignmentOffset(0, -25);
            ElementBounds saveButtonBounds = ElementBounds.FixedSize(0, 0).WithAlignment(EnumDialogArea.LeftBottom).WithFixedPadding(8, 2);
            //Background boundaries. Again, just make it fit it's child elements, then add the text as a child element
            ElementBounds bgBounds = ElementBounds.Fill.WithFixedPadding(GuiStyle.ElementToDialogPadding);
            bgBounds.BothSizing = ElementSizing.FitToChildren;
            bgBounds.WithChildren(textBounds);

            //Using the composer will actually create the dialog itself using the bounds supplied.
            SingleComposer = capi.Gui.CreateCompo("ListScriber" + bePos, dialogBounds)
                //add the background...
                .AddShadedDialogBG(bgBounds)
                //now the title bar... We also need to register an event for when the X is clicked on the UI menu.
                .AddDialogTitleBar(Title, OnTitleBarCloseClicked)
                //and finally add the text in the text box.
                .BeginChildElements(bgBounds)
                    .AddHexGrid(textBounds, patterns)
                    .AddItemSlotGrid(Inventory, SendInvPacket, 1, new int[] { 0 }, PaperBounds, "paperslot")
                    .AddItemSlotGrid(Inventory, SendInvPacket, 1, new int[] { 1 }, ScrollBounds, "scrollslot")
                    .AddItemSlotGrid(Inventory, SendInvPacket, 1, new int[] { 2 }, OutputBounds, "outputslot")
                    .AddSmallButton(Lang.Get("Cancel"), OnButtonCancel, cancelButtonBounds)

                    .AddSmallButton(Lang.Get("Save"), OnButtonSave, saveButtonBounds)
                    
                .EndChildElements()
                //Calling compose is what actually 'builds' the Gui menu.
                .Compose();
        }

        private bool OnButtonSave()
        {
            using (MemoryStream ms = new MemoryStream()) { 
                int quantity = patterns.Count;

                using (BinaryWriter writer = new BinaryWriter(ms))
                {
                    writer.Write(quantity);
                    foreach (ResolvedPattern pattern in patterns)
                    {
                        pattern.ToBytes(writer);
                    }

                    capi.Network.SendBlockEntityPacket(BlockEntityPosition, 2000, ms.ToArray());
                }
            }
            return true;
        }

        private bool OnButtonCancel()
        {
            patterns.RemoveRange(0, patterns.Count);
            SingleComposer.ReCompose();
            return true;
        }

        private void SendInvPacket(object packet)
        {
            capi.Network.SendBlockEntityPacket(BlockEntityPosition.X, BlockEntityPosition.Y, BlockEntityPosition.Z, packet);
        }

        private void OnTitleBarCloseClicked()
        {
            //patterns.RemoveRange(0, patterns.Count);

            TryClose();
        }

    }
}
