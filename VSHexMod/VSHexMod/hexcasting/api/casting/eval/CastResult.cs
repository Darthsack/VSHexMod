using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.Client.NoObf;
using VSHexMod.hexcasting.api.casting.eval.iota;

namespace VSHexMod.hexcasting.api.casting.eval
{
    public class CastResult
    {
        public Iota cast;
        public State continuation;
        public ResolvedPatternType resolutionType;
        //public EvalSound sound;

        public CastResult(Iota cast, State continuation,  ResolvedPatternType resolutionType /*,EvalSound sound*/)
        {
            this.cast = cast;
            this.continuation = continuation;
            this.resolutionType = resolutionType;
            //this.sound = sound;
        }
    }
}
