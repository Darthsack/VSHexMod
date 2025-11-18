using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Client;

namespace VSHexMod.hexcasting.client.gui
{
    public class GuiHexGrid : GuiElement
    {
        private double gridscale;
        public GuiHexGrid(ICoreClientAPI capi, ElementBounds bounds, double gridscale) : base(capi, bounds)
        {
                this.gridscale = gridscale;
        }
    }
}
