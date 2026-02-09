using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSHexMod.hexcasting.api.casting.eval.BaseElements;
using VSHexMod.hexcasting.api.casting.eval.iota;

namespace VSHexMod.hexcasting.api.casting.eval
{
    public class State
    {
        public Stack<Iota> stack;
        public int evals;
        public Element Element = new();
        public double exp = 1;
        private double mult = 1.5;

        public void inc()
        {
            exp *= mult;
        }

        public State()
        {
            this.stack = new Stack<Iota>();
            evals = 0;
        }

        public State(Stack<Iota> stack)
        {
            this.stack = stack;
            evals = 0;
        }

        public State(Iota iota)
        {
            this.stack = new Stack<Iota>();
            this.stack.Push(iota);
            evals = 0;
        }

        public static State operator +(State s, int i)
        {
            s.evals += i;
            return s;
        }
        public static State operator ++(State s)
        {
            s.evals++ ;
            return s;
        }
        public Iota Pop()
        {
            stack.TryPop(out Iota i);
            return i;
        }

        public void Push(Iota I)
        {
            stack.Push(I);
        }
        public void Shift(Element E)
        {
            Element = E;
        }

        public Element Shift()
        {
            return Element;
        }
        public ListIota Open()
        {
            return new ListIota(stack.ToList());
        }

        public static bool operator ==(State s, int i) => (s.evals == i);
        public static bool operator !=(State s, int i) => (s.evals != i);
        public static bool operator >(State s, int i) => (s.evals > i);
        public static bool operator <(State s, int i) => (s.evals < i);
        public static bool operator >=(State s, int i) => (s.evals >= i);
        public static bool operator <=(State s, int i) => (s.evals <= i);
    }
}
