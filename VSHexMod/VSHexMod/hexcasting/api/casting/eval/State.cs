using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Vintagestory.API.MathTools;
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
        private Dictionary<string, int> MaxPow;
        public int BaseMax = 0;


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
            E.strength = GameMath.Min(GameMath.Max(GetMaxPower(E.type),BaseMax,1), E.strength);
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

        public void inc()
        {
            exp *= mult;
        }
        public int GetMaxPower(string element)
        {
            if (!MaxPow.ContainsKey(element))
                return Math.Max(BaseMax,1);
            return BaseMax + MaxPow[element];
        }
        public void SetMaxPower(string element, int power)
        {
            if (MaxPow.ContainsKey(element))
                MaxPow[element] = power;
            else
                MaxPow.Add(element, power);
        }
        public void AddMaxPower(string element, int power)
        {
            if (MaxPow.ContainsKey(element))
                MaxPow[element] += power;
            else
                MaxPow.Add(element, power);
        }
        public void SetMaxPower(Dictionary<string, int> elements)
        {
            MaxPow = elements;
        }
        public void AddMaxPower(Dictionary<string, int> elements)
        {
            string[] finalKeys = MaxPow.Keys.ToArray();
            foreach (string key in elements.Keys)
            {
                if (!finalKeys.Contains(key))
                    finalKeys.AddToArray(key);
            }
            foreach (string key in finalKeys)
            {
                if (MaxPow.ContainsKey(key))
                {
                    if (elements.ContainsKey(key))
                    {
                        MaxPow[key] += elements[key];
                    }
                }
                else
                {
                    MaxPow.Add(key, elements[key]);
                }
            }
        }


        public static bool operator ==(State s, int i) => (s.evals == i);
        public static bool operator !=(State s, int i) => (s.evals != i);
        public static bool operator >(State s, int i) => (s.evals > i);
        public static bool operator <(State s, int i) => (s.evals < i);
        public static bool operator >=(State s, int i) => (s.evals >= i);
        public static bool operator <=(State s, int i) => (s.evals <= i);
    }
}
