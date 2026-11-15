using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;

namespace ArthursJournal
{
    // Token: 0x02000003 RID: 3
  
    public class GuiDialogGeneric : GuiDialog
    {
        // Token: 0x17000003 RID: 3
        // (get) Token: 0x0600002E RID: 46 RVA: 0x00004818 File Offset: 0x00002A18
        public override string ToggleKeyCombinationCode
        {
            get
            {
                return null;
            }
        }

        // Token: 0x0600002F RID: 47 RVA: 0x0000481B File Offset: 0x00002A1B
        public GuiDialogGeneric(string title, Action<string> onConfirm, Action onCancel, Action<string> onTextChanged, ICoreClientAPI capi) : base(capi)
        {
            this.title = title;
            this.onConfirm = onConfirm;
            this.onCancel = onCancel;
            this.onTextChanged = onTextChanged;
        }

        // Token: 0x06000030 RID: 48 RVA: 0x00004844 File Offset: 0x00002A44
        public override void OnGuiOpened()
        {
            base.OnGuiOpened();
            this.SetupDialog();
        }

        // Token: 0x06000031 RID: 49 RVA: 0x00004858 File Offset: 0x00002A58
        private void SetupDialog()
        {
            ElementBounds elementBounds = ElementStdBounds.AutosizedMainDialog.WithAlignment(EnumDialogArea.CenterMiddle);
            ElementBounds elementBounds2 = ElementBounds.Fill.WithFixedPadding(GuiStyle.ElementToDialogPadding);
            elementBounds2.BothSizing = ElementSizing.FitToChildren;
            ElementBounds elementBounds3 = ElementBounds.Fixed(0.0, 30.0, 400.0, 30.0);
            ElementBounds elementBounds4 = ElementBounds.Fixed(0.0, 70.0, 190.0, 30.0);
            ElementBounds elementBounds5 = ElementBounds.Fixed(210.0, 70.0, 190.0, 30.0);
            base.SingleComposer = this.capi.Gui.CreateCompo("textinputdialog", elementBounds)
                .AddShadedDialogBG(elementBounds2, true, 5.0, 0.75f)
                .AddDialogTitleBar(this.title, new Action(this.OnTitleBarClose))
                .BeginChildElements(elementBounds2)
                    .AddTextInput(elementBounds3, new Action<string>(this.OnTextInputChanged), CairoFont.TextInput(), "textinput")
                    .AddSmallButton(Lang.Get("OK", Array.Empty<object>()), new ActionConsumable(this.OnOkButton), elementBounds4, EnumButtonStyle.Normal)
                    .AddSmallButton(Lang.Get("Cancel", Array.Empty<object>()), new ActionConsumable(this.OnCancelButton), elementBounds5, EnumButtonStyle.Normal)
                .EndChildElements()
                .Compose(true);

            base.SingleComposer.FocusElement(base.SingleComposer.GetTextInput("textinput").TabIndex);
        }

        // Token: 0x06000032 RID: 50 RVA: 0x000049EA File Offset: 0x00002BEA
        private void OnTextInputChanged(string text)
        {
            Action<string> action = this.onTextChanged;
            if (action != null)
            {
                action(text);
            }
        }

        // Token: 0x06000033 RID: 51 RVA: 0x00004A00 File Offset: 0x00002C00
        private bool OnOkButton()
        {
            string text = SingleComposer.GetTextInput( "textinput").GetText();
            Action<string> action = this.onConfirm;
            if (action != null)
            {
                action(text);
            }
            this.TryClose();
            return true;
        }

        // Token: 0x06000034 RID: 52 RVA: 0x00004A44 File Offset: 0x00002C44
        private bool OnCancelButton()
        {
            Action action = this.onCancel;
            if (action != null)
            {
                action();
            }
            this.TryClose();
            return true;
        }

        // Token: 0x06000035 RID: 53 RVA: 0x00004A70 File Offset: 0x00002C70
        private void OnTitleBarClose()
        {
            Action action = this.onCancel;
            if (action != null)
            {
                action();
            }
            this.TryClose();
        }

        // Token: 0x0400001F RID: 31
        private string title;

        // Token: 0x04000020 RID: 32
        private Action<string> onConfirm;

        // Token: 0x04000021 RID: 33
        private Action onCancel;

        // Token: 0x04000022 RID: 34
        private Action<string> onTextChanged;
    }
}
