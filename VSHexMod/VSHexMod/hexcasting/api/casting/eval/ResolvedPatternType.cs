using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSHexMod.hexcasting.api.casting.eval
{
    public class ResolvedPatternType
    {
        public static ResolvedPatternType UNRESOLVED = new ResolvedPatternType(0x7f7f7f, 0xcccccc, false);
        public static ResolvedPatternType EVALUATED = new ResolvedPatternType(0x7385de, 0xfecbe6, true);
        public static ResolvedPatternType ESCAPED = new ResolvedPatternType(0xddcc73, 0xfffae5, true);
        public static ResolvedPatternType UNDONE = new ResolvedPatternType(0xb26b6b, 0xcca88e, true);
        public static ResolvedPatternType ERRORED = new ResolvedPatternType(0xde6262, 0xffc7a0, false);
        public static ResolvedPatternType INVALID = new ResolvedPatternType(0xb26b6b, 0xcca88e, false);

        public readonly int colour;
        public readonly int fadeColour;
        public readonly bool success;
        public ResolvedPatternType(int colour, int fadeColour, bool success)
        {
            this.colour = colour;
            this.fadeColour = fadeColour;
            this.success = success;
        }
    }
}
