using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cairo;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;

namespace ArthursJournal
{
    // Token: 0x02000002 RID: 2
    
    public class GuiDialogSketchbook : GuiDialog
    {
        // Token: 0x17000001 RID: 1
        // (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
        public override string ToggleKeyCombinationCode
        {
            get
            {
                return null;
            }
        }

        // Token: 0x17000002 RID: 2
        // (get) Token: 0x06000002 RID: 2 RVA: 0x00002053 File Offset: 0x00000253
        public override bool PrefersUngrabbedMouse
        {
            get
            {
                return true;
            }
        }

        // Token: 0x06000003 RID: 3 RVA: 0x00002058 File Offset: 0x00000258
        public GuiDialogSketchbook(ItemSlot slot, ICoreClientAPI capi, int maxPages) : base(capi)
        {
            sketchbookSlot = slot;
            this.maxPages = maxPages;
            pixels = new int[canvasWidth * canvasHeight];
            journalId = slot.Itemstack.Attributes.GetString("journalId", null);
            if (string.IsNullOrEmpty(journalId))
            {
                capi.Logger.Error("Client: Tried to open journal dialog but item has no journal ID. This should not happen - check ID assignment in ItemArthursJournal.");
            }
            else
            {
                ArthursJournalModSystem modSystem = capi.ModLoader.GetModSystem<ArthursJournalModSystem>(true);
                networkHandler = modSystem.GetNetworkHandler();
                LoadBackgroundImage();
                LoadDrawing();
            }
        }

        // Token: 0x06000004 RID: 4 RVA: 0x000021DC File Offset: 0x000003DC
        private void PushUndoSnapshot()
        {
            int[] array = new int[pixels.Length];
            Array.Copy(pixels, array, pixels.Length);
            undoStack.Add(array);
            if (undoStack.Count > 10)
            {
                undoStack.RemoveAt(0);
            }
        }

        // Token: 0x06000005 RID: 5 RVA: 0x0000223C File Offset: 0x0000043C
        private bool OnUndo()
        {
            bool flag = this.undoStack.Count == 0;
            bool result;
            if (flag)
            {
                result = true;
            }
            else
            {
                int index = this.undoStack.Count - 1;
                Array.Copy(this.undoStack[index], this.pixels, this.pixels.Length);
                this.undoStack.RemoveAt(index);
                this.isPageDirty = true;
                GuiComposerHelpers.GetCustomDraw(base.SingleComposer, "canvas").Redraw();
                result = true;
            }
            return result;
        }

        // Token: 0x06000006 RID: 6 RVA: 0x000022C0 File Offset: 0x000004C0
        private void SetupDialog()
        {
            ElementBounds elementBounds = ElementStdBounds.AutosizedMainDialog.WithAlignment(EnumDialogArea.CenterMiddle);
            ElementBounds elementBounds2 = ElementBounds.Fill.WithFixedPadding(GuiStyle.ElementToDialogPadding);
            elementBounds2.BothSizing = ElementSizing.FitToChildren;
            ElementBounds elementBounds3 = ElementBounds.Fixed(0.0, 30.0, (double)this.canvasWidth, (double)this.canvasHeight);
            ElementBounds elementBounds4 = ElementBounds.Fixed(0.0, (double)(this.canvasHeight + 40), 100.0, 30.0);
            ElementBounds elementBounds5 = ElementBounds.Fixed(110.0, (double)(this.canvasHeight + 40), 100.0, 30.0);
            ElementBounds elementBounds6 = ElementBounds.Fixed(220.0, (double)(this.canvasHeight + 45), 60.0, 25.0);
            ElementBounds elementBounds7 = ElementBounds.Fixed(280.0, (double)(this.canvasHeight + 45), (double)(this.canvasWidth - 370), 25.0);
            ElementBounds elementBounds8 = ElementBounds.Fixed(280.0, (double)(this.canvasHeight + 45), (double)(this.canvasWidth - 370), 25.0);
            ElementBounds elementBounds9 = ElementBounds.Fixed((double)(this.canvasWidth - 80), (double)(this.canvasHeight + 45), 70.0, 25.0);
            ElementBounds elementBounds10 = ElementBounds.Fixed(0.0, (double)(this.canvasHeight + 80), 100.0, 25.0);
            ElementBounds elementBounds11 = ElementBounds.Fixed(110.0, (double)(this.canvasHeight + 80), (double)(this.canvasWidth - 210), 25.0);
            ElementBounds elementBounds12 = ElementBounds.Fixed((double)(this.canvasWidth - 90), (double)(this.canvasHeight + 80), 80.0, 25.0);
            ElementBounds elementBounds13 = ElementBounds.Fixed(0.0, (double)(this.canvasHeight + 115), 100.0, 25.0);
            ElementBounds elementBounds14 = ElementBounds.Fixed(110.0, (double)(this.canvasHeight + 115), 200.0, 25.0);
            ElementBounds elementBounds15 = ElementBounds.Fixed(0.0, (double)(this.canvasHeight + 150), 100.0, 30.0);
            ElementBounds elementBounds16 = ElementBounds.Fixed(110.0, (double)(this.canvasHeight + 150), 100.0, 30.0);
            ElementBounds elementBounds17 = ElementBounds.Fixed(220.0, (double)(this.canvasHeight + 150), 100.0, 30.0);
            ElementBounds elementBounds18 = ElementBounds.Fixed(330.0, (double)(this.canvasHeight + 155), 430.0, 25.0);
            ElementBounds elementBounds19 = ElementBounds.Fixed(0.0, (double)(this.canvasHeight + 190), 100.0, 30.0);
            ElementBounds elementBounds20 = ElementBounds.Fixed(110.0, (double)(this.canvasHeight + 190), 100.0, 30.0);
            ElementBounds elementBounds21 = ElementBounds.Fixed(220.0, (double)(this.canvasHeight + 190), 100.0, 30.0);
            ElementBounds elementBounds22 = ElementBounds.Fixed(0.0, (double)(this.canvasHeight + 230), 100.0, 30.0);
            ElementBounds elementBounds23 = ElementBounds.Fixed(120.0, (double)(this.canvasHeight + 235), 520.0, 25.0);
            ElementBounds elementBounds24 = ElementBounds.Fixed((double)(this.canvasWidth - 100), (double)(this.canvasHeight + 230), 100.0, 30.0);
            ElementBounds elementBounds25 = ElementBounds.Fixed(0.0, (double)(this.canvasHeight + 270), 100.0, 30.0);
            ElementBounds elementBounds26 = ElementBounds.Fixed(110.0, (double)(this.canvasHeight + 270), 100.0, 30.0);
            ElementBounds elementBounds27 = ElementBounds.Fixed(220.0, (double)(this.canvasHeight + 270), 100.0, 30.0);
            ElementBounds elementBounds28 = ElementBounds.Fixed((double)(this.canvasWidth - 100), (double)(this.canvasHeight + 270), 100.0, 30.0);
            string[] array = new string[this.textSizeOptions.Length];
            for (int i = 0; i < this.textSizeOptions.Length; i++)
            {
                array[i] = textSizeOptions[i].ToString();
            }
            base.SingleComposer = this.capi.Gui.CreateCompo("sketchbook", elementBounds)
                .AddShadedDialogBG(elementBounds2, true, 5.0, 0.75f)
                .AddDialogTitleBar(Lang.Get("Sketchbook", Array.Empty<object>()), new Action(this.OnTitleBarClose))
                .BeginChildElements(elementBounds2)
                    .AddDynamicCustomDraw(elementBounds3, new DrawDelegateWithBounds(this.OnDrawCanvas), "canvas")
                    .AddSmallButton(Lang.Get("Chalk", Array.Empty<object>()), new ActionConsumable(this.OnSelectChalk), elementBounds4, EnumButtonStyle.Normal, "chalkbutton")
                    .AddSmallButton(Lang.Get("Sepia", Array.Empty<object>()), new ActionConsumable(this.OnSelectSepia), elementBounds5, EnumButtonStyle.Normal, "sepiabutton")
                    .AddDynamicCustomDraw(elementBounds7, new DrawDelegateWithBounds(this.OnDrawColorGradient), "colorgradient")
                    .AddStaticText(Lang.Get("Color:", Array.Empty<object>()), CairoFont.WhiteSmallText(), elementBounds6)
                    .AddSlider(new ActionConsumable<int>(this.OnColorChanged), elementBounds8, "colorslider")
                    .AddDynamicText(this.GetColorLabel(), CairoFont.WhiteSmallText(), elementBounds9, "colorvalue")
                    .AddStaticText(Lang.Get("Size:", Array.Empty<object>()), CairoFont.WhiteSmallText(), elementBounds10)
                    .AddSlider(new ActionConsumable<int>(this.OnBrushSizeChanged), elementBounds11, "brushsize")
                    .AddDynamicText(this.brushSize.ToString(), CairoFont.WhiteSmallText(), elementBounds12, "brushvalue")
                    .AddStaticText(Lang.Get("Text Size:", Array.Empty<object>()), CairoFont.WhiteSmallText(), elementBounds13)
                    .AddDropDown(array, array, Array.IndexOf<int>(this.textSizeOptions, this.textSize), new SelectionChangedDelegate(this.OnTextSizeSelected), elementBounds14, "textsizedropdown")
                    .AddSmallButton(Lang.Get("Ink", Array.Empty<object>()), new ActionConsumable(this.OnSelectInk), elementBounds15, EnumButtonStyle.Normal, "inkbutton")
                    .AddSmallButton(Lang.Get("Pencil", Array.Empty<object>()), new ActionConsumable(this.OnSelectPencil), elementBounds16, EnumButtonStyle.Normal, "pencilbutton")
                    .AddSmallButton(Lang.Get("Ink Wash", Array.Empty<object>()), new ActionConsumable(this.OnSelectInkWash), elementBounds17, EnumButtonStyle.Normal, "inkwashbutton")
                    .AddDynamicText("Current tool: Ink", CairoFont.WhiteSmallText(), elementBounds18, "toolstatus")
                    .AddSmallButton(Lang.Get("Eyedropper", Array.Empty<object>()), new ActionConsumable(this.OnToggleEyedropper), elementBounds19, EnumButtonStyle.Normal, "eyedropper")
                    .AddSmallButton(Lang.Get("Eraser", Array.Empty<object>()), new ActionConsumable(this.OnToggleEraser), elementBounds20, EnumButtonStyle.Normal, "eraser")
                    .AddSmallButton(Lang.Get("Add Text", Array.Empty<object>()), new ActionConsumable(this.OnToggleTextMode), elementBounds21, EnumButtonStyle.Normal, "addtext")
                    .AddButton(Lang.Get("Prev", Array.Empty<object>()), new ActionConsumable(this.OnPrevPage), elementBounds22, EnumButtonStyle.Normal, "prevbutton")
                    .AddDynamicText(Lang.Get("Page", Array.Empty<object>()) + " " + (this.currentPage + 1) + "/" + this.maxPages, CairoFont.WhiteSmallText(), elementBounds23, "pageinfo")
                    .AddButton(Lang.Get("Next", Array.Empty<object>()), new ActionConsumable(this.OnNextPage), elementBounds24, EnumButtonStyle.Normal, "nextbutton")
                    .AddSmallButton(Lang.Get("Clear", Array.Empty<object>()), new ActionConsumable(this.OnClear), elementBounds25, EnumButtonStyle.Normal, "clearbutton")
                    .AddSmallButton(Lang.Get("Undo", Array.Empty<object>()), new ActionConsumable(this.OnUndo), elementBounds26, EnumButtonStyle.Normal, "undobutton")
                    .AddSmallButton(Lang.Get("Save", Array.Empty<object>()), new ActionConsumable(this.OnSave), elementBounds27, EnumButtonStyle.Normal, "savebutton")
                    .AddSmallButton(Lang.Get("Close", Array.Empty<object>()), new ActionConsumable(this.OnClose), elementBounds28, EnumButtonStyle.Normal, "closebutton")
                .EndChildElements()
                .Compose(true);

            SingleComposer.GetSlider("colorslider").SetValues(grayscaleValue, 0, 255, 1);
            SingleComposer.GetSlider("brushsize").SetValues(brushSize, 1, 20, 1);
            UpdateToolModeButtons();
            UpdateColorModeButtons();
        }

        // Token: 0x06000007 RID: 7 RVA: 0x00002C6C File Offset: 0x00000E6C
        private void OnDrawColorGradient(Context ctx, ImageSurface surface, ElementBounds bounds)
        {
            using (LinearGradient linearGradient = new LinearGradient(0.0, 0.0, bounds.InnerWidth, 0.0))
            {
                if (isSepiaMode)
                {
                    linearGradient.AddColorStop(0.0, new Color(0.07058823529411765, 0.06274509803921569, 0.050980392156862744, 1.0));
                    linearGradient.AddColorStop(1.0, new Color(0.9372549019607843, 0.8352941176470589, 0.6549019607843137, 1.0));
                }
                else
                {
                    linearGradient.AddColorStop(0.0, new Color(0.0, 0.0, 0.0, 1.0));
                    linearGradient.AddColorStop(1.0, new Color(1.0, 1.0, 1.0, 1.0));
                }
                ctx.SetSource(linearGradient);
                ctx.Rectangle(0.0, 0.0, bounds.InnerWidth, bounds.InnerHeight);
                ctx.Fill();
            }
            ctx.SetSourceRGBA(0.3, 0.3, 0.3, 1.0);
            ctx.LineWidth = 1.0;
            ctx.Rectangle(0.0, 0.0, bounds.InnerWidth, bounds.InnerHeight);
            ctx.Stroke();
        }

        // Token: 0x06000008 RID: 8 RVA: 0x00002E5C File Offset: 0x0000105C
        public override void OnGuiOpened()
        {
            base.OnGuiOpened();
            this.SetupDialog();
        }

        // Token: 0x06000009 RID: 9 RVA: 0x00002E70 File Offset: 0x00001070
        public override void OnMouseDown(MouseEvent args)
        {
            base.OnMouseDown(args);
            ElementBounds bounds = SingleComposer.GetCustomDraw("canvas").Bounds;
            if (bounds.PointInside(args.X, args.Y))
            {
                int x = (int)((args.X - bounds.absX) / bounds.InnerWidth * canvasWidth);
                int y = (int)((args.Y - bounds.absY) / bounds.InnerHeight * canvasHeight);
                if (isTextMode)
                {
                    OpenTextInputDialog(x, y);
                }
                else
                {
                    if (isEyedropper)
                    {
                        if (x >= 0 && x < canvasWidth && y >= 0 && y < canvasHeight)
                        {
                            int pos = y * canvasWidth + x;
                            int color = pixels[pos];
                            if (color != 0)
                            {
                                currentColor = color;
                                if (isSepiaMode)
                                {
                                    int red = color >> 16 & 255;
                                    int green = color >> 8 & 255;
                                    int blue = color & 255;
                                    grayscaleValue = (int)(0.299 * red + 0.587 * green + 0.114 * blue);
                                }
                                else
                                {
                                    grayscaleValue = (color >> 16 & 255);
                                }
                                SingleComposer.GetSlider("colorslider").SetValues(grayscaleValue, 0, 255, 1);
                                SingleComposer.GetDynamicText("colorvalue").SetNewText(GetColorLabel());
                            }
                            else
                            {
                                grayscaleValue = 255;
                                UpdateColor();
                                SingleComposer.GetSlider("colorslider").SetValues(grayscaleValue, 0, 255, 1);
                                SingleComposer.GetDynamicText("colorvalue").SetNewText(GetColorLabel());
                            }
                        }
                        isEyedropper = false;
                        isEraserMode = false;
                        isTextMode = false;
                        UpdateToolModeButtons();
                    }
                    else
                    {
                        PushUndoSnapshot();
                        isDrawing = true;
                        lastDrawX = x;
                        lastDrawY = y;
                        DrawAt(x, y);
                    }
                }
            }
        }

        // Token: 0x0600000A RID: 10 RVA: 0x000030D8 File Offset: 0x000012D8
        public override void OnMouseUp(MouseEvent args)
        {
            base.OnMouseUp(args);
            isDrawing = false;
            lastDrawX = -1;
            lastDrawY = -1;
        }

        // Token: 0x0600000B RID: 11 RVA: 0x000030F8 File Offset: 0x000012F8
        public override void OnMouseMove(MouseEvent args)
        {
            base.OnMouseMove(args);
            if (isDrawing && !isEyedropper && !isTextMode)
            {
                ElementBounds bounds = SingleComposer.GetCustomDraw("canvas").Bounds;
                if (bounds.PointInside(args.X, args.Y))
                {
                    int x = (int)((args.X - bounds.absX) / bounds.InnerWidth * canvasWidth);
                    int y = (int)((args.Y - bounds.absY) / bounds.InnerHeight * canvasHeight);
                    if (lastDrawX >= 0 && lastDrawY >= 0)
                    {
                        DrawLine(lastDrawX, lastDrawY, x, y);
                    }
                    else
                    {
                        DrawAt(x, y);
                    }
                    lastDrawX = x;
                    lastDrawY = y;
                }
            }
        }

        // Token: 0x0600000C RID: 12 RVA: 0x000031F8 File Offset: 0x000013F8
        private void OpenTextInputDialog(int x, int y)
        {
            previewTextX = x;
            previewTextY = y;
            showTextPreview = true;
            GuiDialogGeneric guiDialogGeneric = new GuiDialogGeneric(Lang.Get("Enter Text", Array.Empty<object>()), 
                delegate (string text)
            {
                if (!string.IsNullOrEmpty(text))
                {
                    DrawText(text, x, y);
                }
                isTextMode = false;
                showTextPreview = false;
                previewText = "";
                UpdateToolModeButtons();
                SingleComposer.GetCustomDraw("canvas").Redraw();
            }, delegate
            {
                isTextMode = false;
                showTextPreview = false;
                previewText = "";
                UpdateToolModeButtons();
                SingleComposer.GetCustomDraw("canvas").Redraw();
            }, delegate (string text)
            {
                previewText = text;
                SingleComposer.GetCustomDraw("canvas").Redraw();
            }, capi);
            guiDialogGeneric.TryOpen();
        }

        // Token: 0x0600000D RID: 13 RVA: 0x00003288 File Offset: 0x00001488
        private void DrawText(string text, int x, int y)
        {
            PushUndoSnapshot();
            using (ImageSurface imageSurface = new ImageSurface(0, canvasWidth, canvasHeight))
            {
                using (Context context = new Context(imageSurface))
                {
                    context.SelectFontFace("Sans", 0, 0);
                    context.SetFontSize(textSize);
                    double red = (currentColor >> 16 & 255) / 255.0;
                    double green = (currentColor >> 8 & 255) / 255.0;
                    double blue = (currentColor & 255) / 255.0;
                    context.SetSourceRGB(red, green, blue);
                    context.MoveTo(x, y);
                    context.ShowText(text);
                    imageSurface.Flush();
                    IntPtr dataPtr = imageSurface.DataPtr;
                    int stride = imageSurface.Stride;
                    int num4 = stride * canvasHeight;
                    byte[] array = new byte[num4];
                    Marshal.Copy(dataPtr, array, 0, num4);

                    for (int i = 0; i < canvasHeight; i++)
                    {
                        for (int j = 0; j < canvasWidth; j++)
                        {
                            int num5 = i * stride + j * 4;
                            byte b = array[num5 + 3];
                            if (b > 128)
                            {
                                int num6 = i * canvasWidth + j;
                                pixels[num6] = currentColor;
                            }
                        }
                    }
                }
            }
            isPageDirty = true;
            SingleComposer.GetCustomDraw("canvas").Redraw();
        }

        // Token: 0x0600000E RID: 14 RVA: 0x00003464 File Offset: 0x00001664
        private void OnDrawCanvas(Context ctx, ImageSurface surface, ElementBounds bounds)
        {
            ctx.SetSourceRGBA(1.0, 1.0, 1.0, 1.0);
            ctx.Rectangle(0.0, 0.0, bounds.InnerWidth, bounds.InnerHeight);
            ctx.Fill();
            double num = bounds.InnerWidth / canvasWidth;
            double num2 = bounds.InnerHeight / canvasHeight;
            ctx.Save();
            ctx.Scale(num, num2);
            if (backgroundImage is not null)
            {
                double num3 = (double)canvasWidth / (double)backgroundImage.Width;
                double num4 = (double)canvasHeight / (double)backgroundImage.Height;
                ctx.Save();
                ctx.Scale(num3, num4);
                ctx.SetSourceSurface(this.backgroundImage, 0, 0);
                ctx.Paint();
                ctx.Restore();
            }
            using (ImageSurface imageSurface = new ImageSurface(0, canvasWidth, canvasHeight))
            {
                int stride = imageSurface.Stride;
                byte[] array = new byte[stride * canvasHeight];
                for (int y = 0; y < canvasHeight; y++)
                {
                    for (int x = 0; x < canvasWidth; x++)
                    {
                        int color = pixels[y * canvasWidth + x];
                        if (color != 0)
                        {
                            int pos = y * stride + x * 4;
                            array[pos] = (byte)(color & 255); //blue
                            array[pos + 1] = (byte)(color >> 8 & 255); // green
                            array[pos + 2] = (byte)(color >> 16 & 255); // red
                            array[pos + 3] = byte.MaxValue;
                        }
                    }
                }
                Marshal.Copy(array, 0, imageSurface.DataPtr, array.Length);
                imageSurface.MarkDirty();
                ctx.SetSourceSurface(imageSurface, 0, 0);
                ctx.Paint();
            }
            if (showTextPreview && !string.IsNullOrEmpty(previewText))
            {
                ctx.SelectFontFace("Sans", 0, 0);
                ctx.SetFontSize(textSize);
                double red = (currentColor >> 16 & 255) / 255.0;
                double green = (currentColor >> 8 & 255) / 255.0;
                double blue = (currentColor & 255) / 255.0;
                ctx.SetSourceRGB(red, green, blue);
                ctx.MoveTo(previewTextX, previewTextY);
                ctx.ShowText(previewText);
            }
            ctx.Restore();
            ctx.SetSourceRGBA(0.3, 0.3, 0.3, 1.0);
            ctx.LineWidth = 2.0;
            ctx.Rectangle(0.0, 0.0, bounds.InnerWidth, bounds.InnerHeight);
            ctx.Stroke();
        }

        // Token: 0x0600000F RID: 15 RVA: 0x000037B8 File Offset: 0x000019B8
        private bool OnColorChanged(int value)
        {
            grayscaleValue = value;
            UpdateColor();
            SingleComposer.GetDynamicText("colorvalue").SetNewText(GetColorLabel());
            return true;
        }

        // Token: 0x06000010 RID: 16 RVA: 0x000037F8 File Offset: 0x000019F8
        private void UpdateColor()
        {
            if (!isEraserMode)
            {
                if (isSepiaMode)
                {
                    float alpha = grayscaleValue / 255f;
                    int red = (int)(18f + 221f * alpha);
                    int green = (int)(16f + 197f * alpha);
                    int blue = (int)(13f + 154f * alpha);
                    currentColor = (-16777216 | red << 16 | green << 8 | blue);
                }
                else
                {
                    currentColor = (-16777216 | grayscaleValue << 16 | grayscaleValue << 8 | grayscaleValue);
                }
            }
        }

        // Token: 0x06000011 RID: 17 RVA: 0x000038A0 File Offset: 0x00001AA0
        private string GetColorLabel()
        {
            string result;
            if (isSepiaMode)
            {
                result = grayscaleValue switch
                {
                    <= 30 => "Dark",
                    >= 225 => "Light",
                    int i => $"Sepia {i}"
                };
            }
            else
            {
                result = grayscaleValue switch
                {
                    0 => "Black",
                    255 => "White",
                    int i => $"Gray {i}"
                };
            }
            return result;
        }

        // Token: 0x06000012 RID: 18 RVA: 0x00003984 File Offset: 0x00001B84
        private bool OnSelectChalk()
        {
            isSepiaMode = false;
            UpdateColor();
            UpdateColorModeButtons();
            SingleComposer.GetCustomDraw("colorgradient").Redraw();
            SingleComposer.GetDynamicText("colorvalue").SetNewText(GetColorLabel());
            return true;
        }

        // Token: 0x06000013 RID: 19 RVA: 0x000039E4 File Offset: 0x00001BE4
        private bool OnSelectSepia()
        {
            isSepiaMode = true;
            UpdateColor();
            UpdateColorModeButtons();
            SingleComposer.GetCustomDraw("colorgradient").Redraw();
            SingleComposer.GetDynamicText("colorvalue").SetNewText(GetColorLabel());
            return true;
        }

        // Token: 0x06000014 RID: 20 RVA: 0x00003A41 File Offset: 0x00001C41
        private void UpdateColorModeButtons()
        {
        }

        // Token: 0x06000015 RID: 21 RVA: 0x00003A44 File Offset: 0x00001C44
        private void DrawLine(int x0, int y0, int x1, int y1)
        {
            int dx = Math.Abs(x1 - x0);
            int dy = -Math.Abs(y1 - y0);
            int sx = (x0 < x1) ? 1 : -1;
            int sy = (y0 < y1) ? 1 : -1;
            int error = dx + dy;
            while (true)
            {
                DrawBrushStamp(x0, y0);
                int e2 = 2 * error;
                if (e2 > dy)
                {
                    if (x0 == x1) break;
                    error += dy;
                    x0 += sx;
                }
                if (e2 < dx)
                {
                    if (y0 == y1) break;
                    error += dx;
                    y0 += sy;
                }
            }
            SingleComposer.GetCustomDraw("canvas").Redraw();
        }

        // Token: 0x06000016 RID: 22 RVA: 0x00003AF0 File Offset: 0x00001CF0
        private void DrawAt(int x, int y)
        {
            DrawBrushStamp(x, y);
            SingleComposer.GetCustomDraw("canvas").Redraw();
        }

        // Token: 0x06000017 RID: 23 RVA: 0x00003B14 File Offset: 0x00001D14
        private void DrawBrushStamp(int x, int y)
        {
            isPageDirty = true;
            int num = isEraserMode ? 0 : currentColor;
            if (isInkWashMode && !isEraserMode)
            {
                Random random = new Random(x * 1000 + y);
                for (int i = -brushSize; i <= brushSize; i++)
                {
                    for (int j = -this.brushSize; j <= this.brushSize; j++)
                    {
                        float num2 = (float)Math.Sqrt((double)(j * j + i * i));
                        bool flag2 = num2 <= (float)this.brushSize;
                        if (flag2)
                        {
                            int num3 = x + j;
                            int num4 = y + i;
                            bool flag3 = num3 >= 0 && num3 < this.canvasWidth && num4 >= 0 && num4 < this.canvasHeight;
                            if (flag3)
                            {
                                int num5 = num4 * this.canvasWidth + num3;
                                int num6 = this.pixels[num5];
                                float num7 = 1f - num2 / (float)this.brushSize;
                                num7 = Math.Max(0.2f, num7);
                                float num8 = 0.8f + (float)random.NextDouble() * 0.4f;
                                float num9 = num7 * num8 * 0.3f;
                                bool flag4 = num6 == 0;
                                if (flag4)
                                {
                                    bool flag5 = random.NextDouble() < (double)(num9 * 2f);
                                    if (flag5)
                                    {
                                        this.pixels[num5] = num;
                                    }
                                }
                                else
                                {
                                    int num10 = num6 >> 16 & 255;
                                    int num11 = num6 >> 8 & 255;
                                    int num12 = num6 & 255;
                                    int num13 = num >> 16 & 255;
                                    int num14 = num >> 8 & 255;
                                    int num15 = num & 255;
                                    float num16 = (float)num10 * 0.299f + (float)num11 * 0.587f + (float)num12 * 0.114f;
                                    float num17 = (float)num13 * 0.299f + (float)num14 * 0.587f + (float)num15 * 0.114f;
                                    bool flag6 = num17 > num16;
                                    if (flag6)
                                    {
                                        bool flag7 = random.NextDouble() < (double)(num9 * 0.5f);
                                        if (flag7)
                                        {
                                            int num18 = (int)((float)num10 * 0.85f + (float)num13 * 0.15f);
                                            int num19 = (int)((float)num11 * 0.85f + (float)num14 * 0.15f);
                                            int num20 = (int)((float)num12 * 0.85f + (float)num15 * 0.15f);
                                            this.pixels[num5] = (-16777216 | num18 << 16 | num19 << 8 | num20);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (isPencilMode && !isEraserMode)
                {
                    Random random2 = new Random(x * 1000 + y);
                    for (int k = -brushSize; k <= brushSize; k++)
                    {
                        for (int l = -this.brushSize; l <= this.brushSize; l++)
                        {
                            float num21 = (float)Math.Sqrt((double)(l * l + k * k));
                            bool flag9 = num21 <= (float)this.brushSize;
                            if (flag9)
                            {
                                int num22 = x + l;
                                int num23 = y + k;
                                bool flag10 = num22 >= 0 && num22 < this.canvasWidth && num23 >= 0 && num23 < this.canvasHeight;
                                if (flag10)
                                {
                                    float num24 = 1f - num21 / (float)this.brushSize;
                                    num24 = Math.Max(0.1f, num24);
                                    float num25 = (float)random2.NextDouble();
                                    float num26 = num21 / (float)this.brushSize;
                                    float num27 = num24 * (1f - num26 * 0.3f * num25);
                                    bool flag11 = random2.NextDouble() < (double)num27;
                                    if (flag11)
                                    {
                                        int num28 = num23 * this.canvasWidth + num22;
                                        int num29 = this.pixels[num28];
                                        bool flag12 = num29 != 0 && num != 0;
                                        if (flag12)
                                        {
                                            int num30 = num29 >> 16 & 255;
                                            int num31 = num29 >> 8 & 255;
                                            int num32 = num29 & 255;
                                            int num33 = num >> 16 & 255;
                                            int num34 = num >> 8 & 255;
                                            int num35 = num & 255;
                                            int num36 = (int)((float)num30 * 0.7f + (float)num33 * 0.3f);
                                            int num37 = (int)((float)num31 * 0.7f + (float)num34 * 0.3f);
                                            int num38 = (int)((float)num32 * 0.7f + (float)num35 * 0.3f);
                                            this.pixels[num28] = (-16777216 | num36 << 16 | num37 << 8 | num38);
                                        }
                                        else
                                        {
                                            this.pixels[num28] = num;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    for (int m = -brushSize; m <= brushSize; m++)
                    {
                        for (int n = -this.brushSize; n <= this.brushSize; n++)
                        {
                            bool flag13 = n * n + m * m <= this.brushSize * this.brushSize;
                            if (flag13)
                            {
                                int num39 = x + n;
                                int num40 = y + m;
                                bool flag14 = num39 >= 0 && num39 < this.canvasWidth && num40 >= 0 && num40 < this.canvasHeight;
                                if (flag14)
                                {
                                    int num41 = num40 * this.canvasWidth + num39;
                                    this.pixels[num41] = num;
                                }
                            }
                        }
                    }
                }
            }
        }

        // Token: 0x06000018 RID: 24 RVA: 0x000040B8 File Offset: 0x000022B8
        private bool OnBrushSizeChanged(int value)
        {
            brushSize = value;
            SingleComposer.GetDynamicText("brushvalue").SetNewText(brushSize.ToString());
            return true;
        }

        // Token: 0x06000019 RID: 25 RVA: 0x000040F8 File Offset: 0x000022F8
        private void OnTextSizeSelected(string code, bool selected)
        {
            int num;
            if (int.TryParse(code, out num))
            {
                textSize = num;
                if (showTextPreview)
                {
                    SingleComposer.GetCustomDraw("canvas").Redraw();
                }
            }
        }

        // Token: 0x0600001A RID: 26 RVA: 0x0000413C File Offset: 0x0000233C
        private bool OnSelectInk()
        {
            isPencilMode = false;
            isInkWashMode = false;
            isEraserMode = false;
            isEyedropper = false;
            isTextMode = false;
            UpdateToolModeButtons();
            return true;
        }

        // Token: 0x0600001B RID: 27 RVA: 0x0000417C File Offset: 0x0000237C
        private bool OnSelectPencil()
        {
            isPencilMode = true;
            isInkWashMode = false;
            isEraserMode = false;
            isEyedropper = false;
            isTextMode = false;
            UpdateToolModeButtons();
            return true;
        }

        // Token: 0x0600001C RID: 28 RVA: 0x000041BC File Offset: 0x000023BC
        private bool OnSelectInkWash()
        {
            isPencilMode = false;
            isInkWashMode = true;
            isEraserMode = false;
            isEyedropper = false;
            isTextMode = false;
            UpdateToolModeButtons();
            return true;
        }

        // Token: 0x0600001D RID: 29 RVA: 0x000041FC File Offset: 0x000023FC
        private void UpdateToolModeButtons()
        {
            GuiElementDynamicText dynamicText = SingleComposer.GetDynamicText("toolstatus");
            if (dynamicText is not null)
            {
                string str = "Ink - solid brush";
                if (isTextMode)
                {
                    str = "Text - click on canvas to add";
                }
                else
                {
                    if (isEyedropper)
                    {
                        str = "Eyedropper - click on canvas to pick color";
                    }
                    else
                    {
                        if (isEraserMode)
                        {
                            str = "Eraser - will erase existing strokes";
                        }
                        else
                        {
                            if (isInkWashMode)
                            {
                                str = "Ink Wash - will go under existing strokes";
                            }
                            else
                            {
                                if (isPencilMode)
                                {
                                    str = "Pencil - textured brush";
                                }
                            }
                        }
                    }
                }
                dynamicText.SetNewText("Current tool: " + str, false, false, false);
            }
        }

        // Token: 0x0600001E RID: 30 RVA: 0x000042A0 File Offset: 0x000024A0
        private bool OnToggleEyedropper()
        {
            isEyedropper = !isEyedropper;
            if (isEyedropper)
            {
                isEraserMode = false;
                isTextMode = false;
                isPencilMode = false;
                isInkWashMode = false;
            }
            UpdateToolModeButtons();
            return true;
        }

        // Token: 0x0600001F RID: 31 RVA: 0x000042F4 File Offset: 0x000024F4
        private bool OnToggleEraser()
        {
            isEraserMode = !isEraserMode;
            if (isEraserMode)
            {
                isEyedropper = false;
                isTextMode = false;
                isPencilMode = false;
                isInkWashMode = false;
            }
            UpdateToolModeButtons();
            return true;
        }

        // Token: 0x06000020 RID: 32 RVA: 0x00004348 File Offset: 0x00002548
        private bool OnToggleTextMode()
        {
            isTextMode = !isTextMode;
            if (isTextMode)
            {
                this.isEyedropper = false;
                this.isEraserMode = false;
                this.isPencilMode = false;
                this.isInkWashMode = false;
            }
            UpdateToolModeButtons();
            return true;
        }

        // Token: 0x06000021 RID: 33 RVA: 0x0000439C File Offset: 0x0000259C
        private bool OnPrevPage()
        {
            PlayPageTurnSound();
            SaveDrawing();
            int num = currentPage;
            currentPage = Math.Max(0, currentPage - 1);
            if (num != currentPage)
            {
                LoadDrawing();
                UpdatePageInfo();
            }
            return true;
        }

        // Token: 0x06000022 RID: 34 RVA: 0x000043F8 File Offset: 0x000025F8
        private bool OnNextPage()
        {
            PlayPageTurnSound();
            SaveDrawing();
            int num = currentPage;
            currentPage = Math.Min(maxPages - 1, currentPage + 1);
            if (num != currentPage)
            {
                LoadDrawing();
                UpdatePageInfo();
            }
            return true;
        }

        // Token: 0x06000023 RID: 35 RVA: 0x0000445C File Offset: 0x0000265C
        private void PlayPageTurnSound()
        {
            Random random = new Random();
            int value = random.Next(1, 4);
            this.capi.World.PlaySoundAt(new AssetLocation("arthursjournal:sounds/pageturn" + value), this.capi.World.Player.Entity, this.capi.World.Player, volume: 0.5f);
        }

        // Token: 0x06000024 RID: 36 RVA: 0x000044E8 File Offset: 0x000026E8
        private void UpdatePageInfo()
        {
            GuiElementDynamicText dynamicText = GuiElementDynamicTextHelper.GetDynamicText(base.SingleComposer, "pageinfo");
            dynamicText.SetNewText(Lang.Get("Page", Array.Empty<object>()) + " "+ (this.currentPage + 1) + "/" + this.maxPages);
        }

        // Token: 0x06000025 RID: 37 RVA: 0x00004570 File Offset: 0x00002770
        private bool OnClear()
        {
            this.PushUndoSnapshot();
            Array.Clear(this.pixels, 0, this.pixels.Length);
            this.isPageDirty = true;
            GuiComposerHelpers.GetCustomDraw(base.SingleComposer, "canvas").Redraw();
            return true;
        }

        // Token: 0x06000026 RID: 38 RVA: 0x000045BC File Offset: 0x000027BC
        private bool OnSave()
        {
            this.SaveDrawing();
            return true;
        }

        // Token: 0x06000027 RID: 39 RVA: 0x000045D8 File Offset: 0x000027D8
        private bool OnClose()
        {
            SaveDrawing();
            TryClose();
            return true;
        }

        // Token: 0x06000028 RID: 40 RVA: 0x000045FC File Offset: 0x000027FC
        private void SaveDrawing()
        {
            if (isPageDirty)
            {
                byte[] array = new byte[pixels.Length * 4];
                Buffer.BlockCopy(pixels, 0, array, 0, array.Length);
                networkHandler.SendJournalData(journalId, currentPage, array);
                isPageDirty = false;
            }
        }

        // Token: 0x06000029 RID: 41 RVA: 0x0000465C File Offset: 0x0000285C
        private void LoadDrawing()
        {
            Array.Clear(pixels, 0, pixels.Length);
            undoStack.Clear();
            isPageDirty = false;
            networkHandler.RequestJournalData(journalId, currentPage);
        }

        // Token: 0x0600002A RID: 42 RVA: 0x000046AC File Offset: 0x000028AC
        public void OnLoadResponseReceived(string responseJournalId, int pageNumber, byte[] pixelData)
        {
            if (responseJournalId == journalId && pageNumber == currentPage)
            {
                if (pixelData != null && pixelData.Length == pixels.Length * 4)
                {
                    Buffer.BlockCopy(pixelData, 0, pixels, 0, pixelData.Length);
                }
                SingleComposer.GetCustomDraw("canvas").Redraw();
            }
        }

        // Token: 0x0600002B RID: 43 RVA: 0x00004720 File Offset: 0x00002920
        private void OnTitleBarClose()
        {
            SaveDrawing();
            TryClose();
        }

        // Token: 0x0600002C RID: 44 RVA: 0x00004734 File Offset: 0x00002934
        public override void OnGuiClosed()
        {
            base.OnGuiClosed();
            SaveDrawing();
            undoStack.Clear();
            if (backgroundImage != null)
            {
                backgroundImage.Dispose();
                backgroundImage = null;
            }
        }

        // Token: 0x0600002D RID: 45 RVA: 0x00004780 File Offset: 0x00002980
        private void LoadBackgroundImage()
        {
            try
            {
                IAsset asset = capi.Assets.TryGet(new AssetLocation("arthursjournal", "textures/gui/journalbackground.png"));
                if (asset != null)
                {
                    string text = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "journal_bg_temp.png");
                    File.WriteAllBytes(text, asset.Data);
                    backgroundImage = new ImageSurface(text);
                    try
                    {
                        File.Delete(text);
                    }
                    catch
                    {
                    }
                }
            }
            catch
            {
            }
        }

        // Token: 0x04000001 RID: 1
        private ItemSlot sketchbookSlot;

        // Token: 0x04000002 RID: 2
        private string journalId = "";

        // Token: 0x04000003 RID: 3
        private int canvasWidth = 760;

        // Token: 0x04000004 RID: 4
        private int canvasHeight = 512;

        // Token: 0x04000005 RID: 5
        private int[] pixels;

        // Token: 0x04000006 RID: 6
        private bool isDrawing = false;

        // Token: 0x04000007 RID: 7
        private bool isEyedropper = false;

        // Token: 0x04000008 RID: 8
        private int currentColor = -16777216;

        // Token: 0x04000009 RID: 9
        private int brushSize = 3;

        // Token: 0x0400000A RID: 10
        private int currentPage = 0;

        // Token: 0x0400000B RID: 11
        private int maxPages;

        // Token: 0x0400000C RID: 12
        private JournalNetworkHandler networkHandler = null;

        // Token: 0x0400000D RID: 13
        private int lastDrawX = -1;

        // Token: 0x0400000E RID: 14
        private int lastDrawY = -1;

        // Token: 0x0400000F RID: 15
        
        private ImageSurface backgroundImage = null;

        // Token: 0x04000010 RID: 16
        private bool isEraserMode = false;

        // Token: 0x04000011 RID: 17
        private bool isTextMode = false;

        // Token: 0x04000012 RID: 18
        private bool isPencilMode = false;

        // Token: 0x04000013 RID: 19
        private bool isInkWashMode = false;

        // Token: 0x04000014 RID: 20
        private int textSize = 16;

        // Token: 0x04000015 RID: 21
        private int grayscaleValue = 0;

        // Token: 0x04000016 RID: 22
        private string previewText = "";

        // Token: 0x04000017 RID: 23
        private int previewTextX = 0;

        // Token: 0x04000018 RID: 24
        private int previewTextY = 0;

        // Token: 0x04000019 RID: 25
        private bool showTextPreview = false;

        // Token: 0x0400001A RID: 26
        private bool isSepiaMode = false;

        // Token: 0x0400001B RID: 27
        private bool isPageDirty = false;

        // Token: 0x0400001C RID: 28
        private List<int[]> undoStack = new List<int[]>();

        // Token: 0x0400001D RID: 29
        private const int maxUndoSteps = 10;

        // Token: 0x0400001E RID: 30
        private readonly int[] textSizeOptions = new int[]
        {
            8,
            10,
            12,
            14,
            16,
            18,
            20,
            24,
            28,
            32,
            36,
            40,
            48
        };
    }
}
