using Cairo;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;
using Vintagestory.Common;
using VSHexMod.hexcasting.api.casting.eval;
using VSHexMod.hexcasting.api.casting.math;
using static System.Net.Mime.MediaTypeNames;

namespace VSHexMod.hexcasting.client.gui
{
    public class GuiSpellcastingElement : GuiElement
    {
        private List<ResolvedPattern> patterns= new List<ResolvedPattern>();

        private string[] usedSpots = new string[0];

        private HexDir[] inProgressDirs = { };
        private HexCoord inProgressOrigin;
        private bool PatternInProgress = false;

        private HexPattern inProgressPattern;

        Action reDraw;
        Action<List<ResolvedPattern>> UpdateSpell;

        MouseEvent LastMouse;

        public GuiSpellcastingElement(ICoreClientAPI capi, List<ResolvedPattern> _patterns, ElementBounds bounds, System.Action reDraw, Action<List<ResolvedPattern>> UpdateSpell) : base(capi, bounds)
        {
            this.api = capi;
            this.Bounds = bounds;
            this.patterns = _patterns;

            this.reDraw = reDraw;

            this.UpdateSpell = UpdateSpell;

            if (patterns == null)
            {
                patterns = new List<ResolvedPattern>();
                return;
            }

        }



        public void DrawHexPattern(ICoreClientAPI capi, Context ctx, ElementBounds bounds, HexPattern pattern, HexCoord origin, double scale = 0, bool Last = false)
        {
            if (scale == 0)
                scale = scaled(1);
            //pattern = HexPattern.fromAngles(capi, "deaqq", HexDir.SOUTH_EAST);

            ctx.NewPath();
            ctx.LineCap = LineCap.Round;
            ctx.SetSourceRGBA(0.6, 0.2, 0.2, 0.7);
            ctx.LineWidth = scale * 4;

            double xoff = bounds.drawX + (bounds.InnerWidth / 2f);
            double yoff = bounds.drawY + (bounds.InnerHeight / 2f);
            ctx.Translate(xoff, yoff);
            HexCoord cursor = origin;

            if (Last && pattern is null)
            {

                ctx.HexMoveTo(cursor, scale * 20);
                ctx.Translate(-xoff, -yoff);
                ctx.LineTo(capi.Input.MouseX - bounds.renderX + bounds.drawX, capi.Input.MouseY - bounds.renderY + bounds.drawY);
                ctx.Stroke();
                return;
            }

            if (pattern == null)
                return;

            double crossSize = scale * 10;
            double crossWidth = scale * 5;

            //capi.Logger.Event("Point: " + ctx.HasCurrentPoint);


            //capi.Logger.Event("Rendering: "+ pattern.anglesSignature());
            HexDir compass = pattern.startDir;

            ctx.HexMoveTo(cursor, scale * 20);
            HexCoord pos1 = new HexCoord(cursor);
            if (!usedSpots.Contains(cursor.ToString()) && !Last)
                this.usedSpots = this.usedSpots.AddToArray(cursor.ToString());
            cursor += compass;
            HexCoord pos2 = new HexCoord(cursor);
            ctx.HexLineTo(pos1 + cursor, scale * 20/2);

            foreach (HexAngle a in pattern.angles) {
                //capi.Logger.Event("cursor: (" + cursor.r +","+ cursor.q+")");
                //capi.Logger.Event("compass: " + compass.toString());
                //capi.Logger.Event("a: " + a.Angle.ToString());

                if (!usedSpots.Contains(cursor.ToString()) && !Last)
                    this.usedSpots = this.usedSpots.AddToArray(cursor.ToString());

                compass *= a;
                cursor += compass;
                ctx.HexCurveTo(pos1, pos2, cursor, scale * 20);

                pos1 = new HexCoord(pos2);
                pos2 = new HexCoord(cursor);
            }

            if (Last)
                ctx.HexLineTo(cursor, scale * 20);
            else
                ctx.HexLineTo(pos1 + cursor + cursor + cursor, scale * 20 / 4);

            ctx.HexMoveTo(cursor, scale * 20);

            

            ctx.Translate(-xoff, -yoff);

            if (Last) 
                ctx.LineTo(capi.Input.MouseX - bounds.renderX + bounds.drawX, capi.Input.MouseY - bounds.renderY + bounds.drawY);
            
            ctx.Stroke();

            if (!usedSpots.Contains(cursor.ToString()) && !Last)
            {
                ctx.Translate(xoff, yoff);
                this.usedSpots = this.usedSpots.AddToArray(cursor.ToString());
                ctx.HexDrawCircle(cursor.x, cursor.y, scale * 2.5, scale * 20);
                ctx.Translate(-xoff, -yoff);
            }


        }

       
        public static void DrawHexGrid(ICoreClientAPI capi, Context ctx, ElementBounds bounds, double scale = 0)
        {
            if (scale == 0)
                scale = scaled(1);
            double xoff = bounds.drawX + bounds.InnerWidth / 2f;
            double yoff = bounds.drawY + bounds.InnerHeight / 2f;
            ctx.Translate(xoff, yoff);
            ctx.SetSourceRGBA(0.0, 0.0, 0.0, 0.3);

            int gridT = (int)Math.Round(bounds.InnerHeight / 2f / (scale * 1.5f * 20f+5));
            int gridL = (int)Math.Round(bounds.InnerWidth / 2f / (scale * 2f * 20f+5));

            HexCoord test = new HexCoord(0,0);
            for (int r = -gridT; r <= gridT; r++) {
                for(int q = (int)(-gridL - r / 2f); q <= (int)(  gridL - r / 2f); q++) {
                    test.q = q;
                    test.r = r;
                    ctx.HexDrawCircle(test.x , test.y , scale * 2, scale * 20);
                }
            }
            ctx.Translate(-xoff, -yoff);
        }
        
        

        public override void ComposeElements(Context ctx, ImageSurface surface)
        {
            Bounds.CalcWorldBounds();

            usedSpots = new String[0];
            //ImageSurface tsurface = new ImageSurface(Format.Argb32, (int)Bounds.InnerWidth, (int)Bounds.InnerHeight);
            //Context tctx = new Context(tsurface);

            //tctx.SetSourceRGBA(GuiStyle.DialogSlotBackColor);
            //RoundRectangle(tctx, 0, 0, Bounds.InnerWidth, Bounds.InnerHeight, GuiStyle.ElementBGRadius);
            //tctx.Fill();
            //ctx.Clip();
            ctx.SetSourceRGBA(1, 1, 1, 0.6);
            ElementRoundRectangle(ctx, Bounds);
            ctx.Fill();
            EmbossRoundRectangleElement(ctx, Bounds, true);

            DrawHexGrid(api, ctx, Bounds, scaled(0.9));

            foreach (ResolvedPattern pat in patterns)
            {
                DrawHexPattern(api, ctx, Bounds, pat.pattern, pat.origin, scaled(0.9));
            }

            if (PatternInProgress)
            {
                DrawHexPattern(api, ctx, Bounds, inProgressPattern, inProgressOrigin, scaled(0.9), true);
            }

        }

        public override void OnMouseDownOnElement(ICoreClientAPI api, MouseEvent mouse)
        {
            base.OnMouseDownOnElement(api, mouse);
            
            double x = mouse.X;
            double y = mouse.Y;

            Vec2d relpos = Bounds.PositionInside((int)x, (int)y) - new Vec2d(Bounds.absInnerWidth/2, Bounds.absInnerHeight/2);

            relpos = toHexPos(relpos, 20 * scaled(0.9));

            //api.Logger.Event("Pos(" + relpos.X + "," + relpos.Y + ")");
            HexCoord test = new HexCoord((int)Math.Round((relpos.X - relpos.Y / 1.5f) / 2f), (int)Math.Round(relpos.Y / 1.5f));

            if (usedSpots.Contains(test.ToString()))
                return;

            api.Gui.PlaySound("menubutton_press");
            //HexCoord test = new HexCoord((int)Math.Round((relpos.X - relpos.Y / 2f) / (float)(scaled(0.9) * 2f * 20f + 5f)), (int)Math.Round(relpos.Y / (float)(scaled(0.9) * 20f * 1.5f + 5f)));
            //api.Logger.Event("HexCoord(" + test.q + "," + test.r + ")");

            PatternInProgress = true;
            inProgressOrigin = new HexCoord(test);
        }

        public override void OnMouseUp(ICoreClientAPI api, MouseEvent mouse)
        {
            base.OnMouseUp(api, mouse);
            if (!PatternInProgress)
                return;

            PatternInProgress = false;

            if (inProgressDirs.Length == 0) { reDraw.Invoke(); return; }

            HexDir startDir = inProgressDirs[0];

            HexAngle[] Angles = { };

            for(int i = 0; i < inProgressDirs.Length -1; i++)
            {
                Angles = Angles.AddToArray(inProgressDirs[i+1] - inProgressDirs[i]);
            }

            HexPattern Resolved = new HexPattern(startDir, Angles);

            patterns.Add(new ResolvedPattern(Resolved, inProgressOrigin));

            //api.Logger.Event("Resolved: "+ Resolved);
            UpdateSpell?.Invoke(patterns);
            inProgressDirs = new HexDir[0];
            inProgressOrigin = null;
            inProgressPattern = null;
            reDraw.Invoke();
        }

        public override void OnMouseMove(ICoreClientAPI api, MouseEvent mouse) 
        {
            base.OnMouseMove(api, mouse);
            if (!PatternInProgress)
                return;

            if (!Bounds.PointInside(mouse.X, mouse.Y))
            {
                //LastMouse = mouse;
                reDraw.Invoke();
                return;
            }

            double x = mouse.X;
            double y = mouse.Y;

            Vec2d relpos = Bounds.PositionInside((int)x, (int)y) - new Vec2d(Bounds.absInnerWidth / 2, Bounds.absInnerHeight / 2);

            relpos = toHexPos(relpos, 20 * scaled(0.9));

            //api.Logger.Event("Pos(" + relpos.X + "," + relpos.Y + ")");
            HexCoord test = new HexCoord((int)Math.Round((relpos.X - relpos.Y/1.5f) / 2f), (int)Math.Round(relpos.Y/1.5f));
            //api.Logger.Event("HexCoord(" + test.q + "," + test.r + ")");

            if (usedSpots is not null && usedSpots.Contains(test.ToString()))
            {
                reDraw.Invoke();
                return;
            }


            if (Math.Sqrt((test.x - relpos.X) *(test.x - relpos.X) + (test.y - relpos.Y) *(test.y - relpos.Y)) > scaled(0.9))
            {
                //LastMouse = mouse;
                reDraw.Invoke();
                return;
            }

            HexCoord cursor = new HexCoord(inProgressOrigin);
            HexAngle[] angles = new HexAngle[0];
            HexDir compass = null;
            foreach (HexDir dir in inProgressDirs)
            {
                cursor += dir;

                if(compass is not null)
                {
                    angles = angles.AddToArray(dir - compass);
                }

                compass = new HexDir(dir.Direction);
                //api.Logger.Event("HexCoord(" + test.q + "," + test.r + ")");
            }
            HexDir testDir = cursor.immediateDelta(test);

            

            if (testDir is not null)
            {

                if (inProgressPattern is not null && !inProgressPattern.tryAppendDir(testDir) && testDir - compass != HexAngle.BACK)
                {
                    reDraw.Invoke();
                    return;
                }

                api.Gui.PlaySound("menubutton_press");
                if (inProgressDirs.Length > 0 && (inProgressDirs.Last() - testDir) == HexAngle.BACK)
                {
                    inProgressDirs = inProgressDirs.Remove(inProgressDirs.Last());
                    if(angles.Length > 0)
                        angles = angles.Remove(angles.Last());
                }
                else
                    inProgressDirs = inProgressDirs.AddToArray(testDir);


                //api.Logger.Event("HexCoord(" + test.q + "," + test.r + ")");
                if (inProgressDirs.Length > 0)
                    inProgressPattern = new HexPattern(inProgressDirs[0], compass is not null && testDir - compass != HexAngle.BACK? angles.AddToArray(testDir - compass) : angles);
                else
                    inProgressPattern = null;
            }
            
            //api.Logger.Event("ReDrawing!");
            reDraw.Invoke();
        }

        private Vec2d toHexPos(Vec2d pos, double scale = 20)
        {
            return new Vec2d((pos.X / scale), (pos.Y / scale));
        }
        private Vec2d fromHexPos(Vec2d pos, double scale = 20)
        {
            return new Vec2d((pos.X * scale), (pos.Y * scale));
        }
    }
    public static partial class GuiComposerHelpers
    {
        public static GuiComposer AddHexGrid(this GuiComposer composer, ElementBounds bounds, List<ResolvedPattern> patterns = null, Action<List<ResolvedPattern>> updateSpell = null)
        {
            if (!composer.Composed)
            {
                composer.AddInteractiveElement(new GuiSpellcastingElement(composer.Api, patterns, bounds, composer.ReCompose, updateSpell),"Hexcaster");
            }

            return composer;
        }
    }

    public static class ContextExtensions
    {
        public static void HexLineTo(this Context ctx, HexCoord pos, double scale = 20)
        {
            ctx.LineTo((pos.x * scale ), (pos.y * scale ));
        }

        public static void HexMoveTo(this Context ctx, HexCoord pos, double scale = 20)
        {
            ctx.MoveTo((pos.x * scale ), (pos.y * scale ));
        }

        public static void HexCurveTo(this Context ctx, HexCoord pos1, HexCoord pos2, HexCoord pos3, double scale = 20)
        {
            

            ctx.CurveTo((pos1.x + pos2.x*3) /4 * scale, (pos1.y + pos2.y*3) / 4 * scale, (pos2.x) * scale, (pos2.y) * scale, (pos2.x*3 + pos3.x) / 4 * scale, (pos2.y*3 + pos3.y) / 4 * scale);
        //    double m = (pos1.y - pos2.y) / (pos1.x - pos2.x);
        //    double b = m * pos2.x - pos2.y;

        //    double A = -(pos1.y - pos2.y);
        //    double B = (pos1.x - pos2.x);
        //    double C = b/B;

        //    ctx.Arc((pos1.x + pos2.x + pos3.x) / 3 * scale, (pos1.y + pos2.y + pos3.y) / 3 * scale, Math.Abs(A * ((pos1.x + pos2.x + pos3.x) / 3) + B * ((pos1.y + pos2.y + pos3.y) / 3) + C)/Math.Sqrt((A*A)+(B*B)),);
        }


        public static void DrawCircle(this Context ctx, double x, double y,double r = 5)
        {
            ctx.NewPath();
            ctx.MoveTo(x, y + r);
            ctx.Arc(x,y,r,0,2*Math.PI);
            ctx.ClosePath();
            ctx.Fill();
        }
        public static void HexDrawCircle(this Context ctx, double x, double y, double r = 5, double scale = 20)
        {
            Vec2d pos = new Vec2d((x * scale), (y * scale));
            
            ctx.NewPath();
            ctx.MoveTo(pos.X, pos.Y + r);
            ctx.Arc(pos.X, pos.Y, r, 0, Math.Tau);
            ctx.ClosePath();
            ctx.Fill();

        }

    }
}

