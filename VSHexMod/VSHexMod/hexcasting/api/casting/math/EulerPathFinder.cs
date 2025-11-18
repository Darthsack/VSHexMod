using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Util;

namespace VSHexMod.hexcasting.api.casting.math
{
    public static class EulerPathFinder
    {
        public static HexPattern findAltDrawing(HexPattern original, int seed)
        {
            Random rand = new Random(seed);

            int iterations = 100;
            HexPattern path = null;
            for (int i = 0; i < iterations; i++)
            {
                path = walkPath(original, rand);
                if (path != null)
                {
                    return path;
                }
            }

            return original;

        }

        private static HexPattern walkPath(HexPattern original, Random rand)
        {
            var graph = toGraph(original);
            List<HexCoord> oddnodes = graph.Keys.Where(k => graph[k].Count() == 1).ToList();

            HexCoord current = oddnodes.Count == 2 ? 
                oddnodes[rand.Next(oddnodes.Count - 1)] : //yes
                graph.Keys.ElementAt(rand.Next(graph.Keys.Count() - 1));//no
            

            Stack<HexCoord> stack = new Stack<HexCoord>();
            List<HexCoord> output = new List<HexCoord>();

            do
            {
                HashSet<HexDir> exits = graph[current]!;
                if (!exits.Any())
                {
                    output.Add(current);
                    current = stack.Pop();
                }
                else
                {
                    stack.Push(current);

                    HexDir burnDir = exits.ElementAt(rand.Next(exits.Count - 1));
                    exits.Remove(burnDir);
                    graph[current + burnDir].Remove(burnDir * HexAngle.BACK);
                    current += burnDir;
                }
            } while (graph.Any() || stack.Any());
            output.Add(current);

            List<HexDir> dirs = zipWithNext<HexCoord, HexDir>(output, (a, b) => a.immediateDelta(b));
            List<HexAngle> angles = zipWithNext<HexDir, HexAngle>(dirs, (a, b) => a.angleFrom(b));


            HexPattern result = new HexPattern(dirs[0], angles.ToArray());
            if (original.positions().Where(x => result.positions().Contains(x)).ToList().Count() == original.positions().Count())
                return result;
            return null;
        }
        private static List<TResult> zipWithNext<TSource, TResult>(List<TSource> a, Expression<Func<TSource, TSource, TResult>> expression)
        {
            Func<TSource, TSource, TResult> e = expression.Compile();
            List<TResult> result = new List<TResult>();
            for (int i = 0; i < a.Count-1; i++) 
            {
                result.Add(e.Invoke(a[i], a[i + 1]));
            }
            return result;
        }
        
        private static Dictionary<HexCoord, HashSet<HexDir>> toGraph(HexPattern pat)
        {
            Dictionary<HexCoord, HashSet<HexDir>> graph = new Dictionary<HexCoord, HashSet<HexDir>>();

            HexDir compass = pat.startDir;
            HexCoord cursor = HexCoord.Origin;

            foreach (HexAngle a in pat.angles) 
            {
                // You're telling me
                if (graph.Keys.Contains(cursor))
                    graph[cursor].Append(compass);
                else
                {
                    HexDir[] c = { compass };
                    graph.Add(cursor, new HashSet<HexDir>(c));
                }

                if (graph.Keys.Contains(cursor + compass))
                    graph[cursor].Append(compass * HexAngle.BACK);
                else
                {
                    HexDir[] c = { compass * HexAngle.BACK };
                    graph.Add(cursor, new HashSet<HexDir>(c));
                }

                cursor += compass;
                compass *= a;
            }
            if (graph.Keys.Contains(cursor))
                graph[cursor].Append(compass);
            else
            {
                HexDir[] c = { compass };
                graph.Add(cursor, new HashSet<HexDir>(c));
            }

            if (graph.Keys.Contains(cursor + compass))
                graph[cursor].Append(compass * HexAngle.BACK);
            else
            {
                HexDir[] c = { compass * HexAngle.BACK };
                graph.Add(cursor, new HashSet<HexDir>(c));
            }
            return graph;

        }

    }
}
