using Cairo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Client;
using VSHexMod.hexcasting.api.casting.eval;
using VSHexMod.hexcasting.api.casting.math;

namespace VSHexMod.hexcasting.client.gui
{
    public class GuiHexPattern : GuiElement 
    {
        private double gridscale;
        private ResolvedPattern pattern;
        private bool InPrograss;
        public GuiHexPattern(ICoreClientAPI capi, ElementBounds bounds, double gridscale, ResolvedPattern pattern, bool InPrograss = false) : base(capi, bounds)
        {
            this.gridscale = gridscale;
            this.pattern = pattern;
            this.api = capi;
            this.Bounds = bounds;
            this.InPrograss = InPrograss;
        }

        public override void ComposeElements(Context ctx, ImageSurface surface)
        {
            DrawHexPattern(api, ctx, Bounds, pattern.pattern, pattern.origin, gridscale);
            if (InPrograss) 
            { 
                ctx.LineTo(api.Input.MouseX - Bounds.absFixedX, api.Input.MouseY - Bounds.absFixedY);
                ctx.Stroke();
            }
        }

        public static void DrawHexPattern(ICoreClientAPI capi, Context ctx, ElementBounds bounds, HexPattern pattern, HexCoord origin, double scale = 0)
        {
            if (scale == 0)
                scale = scaled(1);
            //pattern = HexPattern.fromAngles(capi, "deaqq", HexDir.SOUTH_EAST);
            if (pattern == null)
                return;
            ctx.NewPath();
            ctx.LineWidth = scale * 4;


            double crossSize = scale * 10;
            double crossWidth = scale * 5;
            ctx.LineCap = LineCap.Round;
            ctx.SetSourceRGBA(0.6, 0.2, 0.2, 0.7);
            //capi.Logger.Event("Point: " + ctx.HasCurrentPoint);

            double xoff = bounds.drawX + (bounds.InnerWidth / 2f);
            double yoff = bounds.drawY + (bounds.InnerHeight / 2f);
            ctx.Translate(xoff, yoff);
            //capi.Logger.Event("Rendering: " + pattern.anglesSignature());
            HexDir compass = pattern.startDir;
            HexCoord cursor = origin;
            ctx.HexMoveTo(cursor, scale * 20);
            cursor += compass;
            ctx.HexLineTo(cursor, scale * 20);

            foreach (HexAngle a in pattern.angles)
            {
                //capi.Logger.Event("cursor: (" + cursor.r + "," + cursor.q + ")");
                //capi.Logger.Event("compass: " + compass.toString());
                //capi.Logger.Event("a: " + a.Angle.ToString());


                compass *= a;
                cursor += compass;

                ctx.HexLineTo(cursor, scale * 20);
            }
            ctx.Stroke();
            ctx.Translate(-xoff, -yoff);

        }
    }
}
