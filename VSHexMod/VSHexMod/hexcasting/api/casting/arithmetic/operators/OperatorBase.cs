using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Common;
using VSHexMod.hexcasting.api.casting.eval;
using VSHexMod.hexcasting.api.casting.eval.iota;

namespace VSHexMod.hexcasting.api.casting.arithmetic.operators
{
    public abstract class OperatorBase
    {
        public static int cost = 0;
        public CastResult castResult;

        public OperatorBase(Entity player, ICoreAPI api, State Current) { }

        public CastResult getResult()
        {
            return castResult;
        }
        public int getCost() 
        { 
            return cost;
        }
    }
}
