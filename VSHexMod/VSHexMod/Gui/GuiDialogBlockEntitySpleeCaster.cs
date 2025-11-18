
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using VSHexMod.hexcasting.client.gui;

namespace VSHexMod.Gui
{
    public class GuiDialogBlockEntitySpleeCaster : GuiDialogBlockEntity
    {
        long lastRedrawMs;
        EnumPosFlag screenPos;
        public GuiDialogBlockEntitySpleeCaster(string dlgTitle, InventoryBase Inventory, BlockPos bePos, ICoreClientAPI capi) : base(dlgTitle, Inventory, bePos, capi)
        {
            if (IsDuplicate) return;

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
            ElementBounds textBounds = ElementBounds.Fixed(0, 20, 300, 200);

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
                //.AddInteractiveElement(new GuiSpellcastingElement(capi, null, textBounds, redraw))
                //Calling compose is what actually 'builds' the Gui menu.
                .Compose();

        }

        private void redraw()
        {
            SingleComposer.ReCompose();
        }

        private void OnTitleBarCloseClicked()
        {
            TryClose();
        }

    }
}
