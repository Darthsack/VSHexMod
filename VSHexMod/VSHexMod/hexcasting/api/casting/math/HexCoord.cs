using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.Client.NoObf;
using Vintagestory.GameContent;

namespace VSHexMod.hexcasting.api.casting.math
{
    public class HexCoord
    {
        public double q;
        public double r;
        public HexCoord(int col, int row)
        {
            q = col;
            r = row;
        }

        public HexCoord(double col, double row)
        {
            q = col;
            r = row;
        }

        public HexCoord(HexCoord _copy)
        {
            q = _copy.q;
            r = _copy.r;
        }

        public void Deconstruct(out int col,out int row)
        {
            col = (int)q;
            row = (int)r;
        }

        public float x { 
            get { return (float)q * 2f + (float)r ; } 
        }
        public float y { 
            get { return (float)r * 1.5f; } 
        }

        //public int s {
        //    get { return (int)q - (int)r; }
        //}

        public double s
        {
            get { return q - r; }
        }

        public HexCoord shiftedBy(HexCoord coord) => new HexCoord(this.q + coord.q, this.r + coord.r);

        public HexCoord shiftedBy(HexDir dir) => this.shiftedBy(dir.asDelta());
        
        public HexCoord Delta(HexCoord x) => new HexCoord(this.q - x.q, this.r - x.r);


        public static HexCoord operator +(HexCoord a, HexCoord b) => a.shiftedBy(b);
        public static HexCoord operator +(HexCoord a, HexDir b) => a.shiftedBy(b);
        public static HexCoord operator -(HexCoord a, HexCoord b) => a.Delta(b);

        public static HexCoord operator *(HexCoord a, int b) => new HexCoord((double)((float)a.q * (float)b), (int)((float)a.r * (float)b));

        public static HexCoord operator /(HexCoord a, int b) => new HexCoord((double)((float)a.q/(float)b),(int)((float)a.r/ (float)b));

        public double distanceTo(HexCoord x) => 
            (Math.Abs(this.q - x.q) + Math.Abs(this.q + this.r - x.q - x.r) + Math.Abs(this.r - x.r)) / 2;

        public IEnumerator<HexCoord> rangeAround(int radius) => (IEnumerator < HexCoord >) new RingIter(this, radius);

        public HexDir immediateDelta(HexCoord neighbor)
        {
            switch ((neighbor - this))
            {
                case (1, 0):
                    return HexDir.EAST;
                case (0, 1):
                    return HexDir.SOUTH_EAST;
                case (-1, 1):
                    return HexDir.SOUTH_WEST;
                case (-1, 0):
                    return HexDir.WEST;
                case (0, -1):
                    return HexDir.NORTH_WEST;
                case (1, -1):
                    return HexDir.NORTH_EAST;
            }
            return null;
        }

        private class RingIter : IEnumerator
        {
            private int q;
            private int r;
            private int radius;
            private HexCoord centre;

            public IEnumerator<HexCoord> GetEnumerator()
            {
                return (IEnumerator<HexCoord>) this;
            }

            internal RingIter(HexCoord _centre, int _radius)
            {
                centre = _centre;
                radius = _radius;
                q = -radius;
                r = Math.Max(-radius, 0);
            }

            public bool MoveNext()
            {
                
                if (!hasNext()){
                    return false;
                }

                if (r > radius + Math.Min(0, -q)) 
                {
                    q++;
                    r = -radius + Math.Max(0, -q);
                }
                r++;

                return true;
            }

            private bool hasNext() => r <= radius + Math.Min(0, -q) || q < radius;
            public void Reset() 
            {
                q = -radius;
                r = Math.Max(-radius, 0);
            }
            public HexCoord Current()
            {
                return new HexCoord(centre.q + q, centre.r + r - 1);
            }

            object IEnumerator.Current
            {
                get
                {
                    return Current();
                }
            }
        }
        public override string ToString()
        {
            return "HexCoord(" + q + "," + r + ")";
        }

        public static HexCoord Origin = new HexCoord(0, 0);
    }
}
