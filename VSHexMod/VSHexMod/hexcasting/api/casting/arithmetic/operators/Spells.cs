using Cairo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;
using Vintagestory.GameContent;
using Vintagestory.Server;
using VSHexMod.EntityBehaviors;
using VSHexMod.hexcasting.api.casting.eval;
using VSHexMod.hexcasting.api.casting.eval.BaseElements;
using VSHexMod.hexcasting.api.casting.eval.iota;
using VSHexMod.hexcasting.common.items.magic;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static Vintagestory.Server.Timer;
using static VSHexMod.hexcasting.api.casting.arithmetic.operators.Spells;

namespace VSHexMod.hexcasting.api.casting.arithmetic.operators
{
    public interface Spells
    {
        public static void RegisterArithmetic(ICoreAPI api)
        {
            //Doubles & Vectors
            api.RegisterHex("waaw", new Add()); // Additive Distillation (num|vec, num|vec → num|vec) // Additive Distillation (list, list → list)
            api.RegisterHex("wddw", new Sub()); // Subtractive Distillation (num|vec, num|vec → num|vec)
            api.RegisterHex("waqaw", new Mul()); // Multiplicative Dstl. (num|vec, num|vec → num|vec)
            api.RegisterHex("wqaqw", new Abs()); // Length Purification (num|vec → number) // Length Purification (list → num) // Length Purification (bool → number)
            api.RegisterHex("wdedw", new Div()); // Division Dstl. (num|vec, num|vec → num|vec)
            api.RegisterHex("wedew", new Pow()); // Power Distillation (num|vec, num|vec → num|vec)
            api.RegisterHex("ewq", new Floor()); // Floor Purification (num|vec → num|vec)
            api.RegisterHex("qwe", new Ceil()); // Ceiling Purification(num| vec → num | vec)
            api.RegisterHex("eqqq", new Rand()); // Entropy Reflection(→ num)

            api.RegisterHex("qqqqqaa", new Sin()); // Sine Purification (num → num)
            api.RegisterHex("qqqqqad", new Cos()); // Cosine Purification (num → num)
            api.RegisterHex("wqqqqqadq", new Tan()); // Tangent Purification (num → num)
            api.RegisterHex("ddeeeee", new Arcsin()); // Inverse Sine Prfn. (num → num)
            api.RegisterHex("adeeeee", new Arccos()); // Inverse Cosine Prfn. (num → num)
            api.RegisterHex("eadeeeeew", new ArcTan()); // Inverse Tangent Prfn. (num → num)
            api.RegisterHex("deadeeeeewd", new ArcTan2()); // Inverse Tan. Prfn. II (num, num → num)
            api.RegisterHex("eqaqe", new Log()); // Logarithmic Distillation (num, num → num)
            api.RegisterHex("addwaad", new Mod()); // Modulus Distillation (num|vec, num|vec → num|vec)

            //Vecs

            api.RegisterHex("eqqqqq", new Pack()); // Vector Exaltation (num, num, num → vector)
            api.RegisterHex("qeeeee", new UnPack()); // Vector Disintegration (vector → num, num, num)
            // Axial Purification (vec|num → vec|num)

            //Lists

            api.RegisterHex("deeed", new Index()); // Selection Distillation (list, number → any)
            api.RegisterHex("qaeaqwded", new Slice()); // Selection Exaltation (list, num, num → list)
            api.RegisterHex("edqde", new Append()); // Integration Distillation (list, any → list)
            api.RegisterHex("qaeaq", new UnAppend()); // Derivation Decomposition (list → list, any)
            api.RegisterHex("qqqaede", new Rev()); // Retrograde Purification (list → list)
            api.RegisterHex("dedqde", new Index_Of()); // Locator's Distillation (list, any → num)
            api.RegisterHex("edqdewaqa", new Remove()); // Excisor's Distillation (list, num → list)
            api.RegisterHex("wqaeaqw", new Replace()); // Surgeon's Exaltation (list, num, any → list)
            api.RegisterHex("ddewedd", new Cons()); // Flock's Gambit (many, num → list)
            api.RegisterHex("aaqwqaa", new UnCons()); // Flock's Disintegration (list → many)
            api.RegisterHex("qqaeaae", new EMPTY_LIST()); // Vacant Reflection (→ list)
            // Single's Purification (any → list)
            // Speaker's Distillation (list, any → list)
            // Speaker's Decomposition (list → list, any)

            //Boolean Logic, Comparisons & Sets

            api.RegisterHex("wdw", new And()); // Conjunction Distillation (bool, bool → bool)
            api.RegisterHex("waw", new Or()); // Disjunction Distillation (bool, bool → bool)
            api.RegisterHex("dwa", new Xor()); // Exclusion Distillation (bool, bool → bool)
            api.RegisterHex("e", new Greater()); // Maximus Distillation (number, number → bool)
            api.RegisterHex("q", new Less()); // Minimus Distillation (number, number → bool)
            api.RegisterHex("ee", new Greater_Eq()); // Maximus Distillation II (number, number → bool)
            api.RegisterHex("qq", new Less_Eq()); // Minimus Distillation II (number, number → bool)
            api.RegisterHex("ad", new Equals()); // Equality Distillation (any, any → bool)
            api.RegisterHex("da", new Not_Equal()); // Inequality Distillation (any, any → bool)
            api.RegisterHex("dw", new Not()); // Negation Purification (bool → bool)
            api.RegisterHex("aw", new Bool_Coerce()); // Augur's Purification (any → bool)
            api.RegisterHex("awdd", new If()); // Augur's Exaltation (bool, any, any → any)
            
            api.RegisterHex("aweaqa", new Unique()); // Uniqueness Purification (list → list)
            // Disjunction Distillation ((num, num)|(list, list) → num|list)
            // Conjunction Distillation ((num, num)|(list, list) → num|list)
            // Exclusion Distillation ((num, num)|(list, list) → num|list)
            // Negation Purification (num → num)

            // Meta-Eval

            api.RegisterHex("deaqq", new Eval()); // Hermes' Gambit ([pattern] | pattern → many)
            // Iris' Gambit ([pattern] | pattern → many)
            api.RegisterHex("dadad", new FOR_EACH()); // Thoth's Gambit (list of patterns, list → list)
            // Charon's Gambit
            // Thanatos' Reflection (→ number)

            // Read and Write

            api.RegisterHex("aqqqqq", new Read());
            api.RegisterHex("deeeee", new Write());

            // Basics

            api.RegisterHex("qaq", new Get_caster());// Mind's Reflection (→ entity | null)
            api.RegisterHex("aa", new Entity_PosEye());// Compass' Purification (entity → vector)
            api.RegisterHex("dd", new Entity_PosFoot());// Compass' Purification II (entity → vector)
            api.RegisterHex("wa", new Entity_Look());// Alidade's Purification (entity → vector)
            api.RegisterHex("wqaawdd", new RayCast());// Archer's Distillation (vector, vector → vector | null)
            api.RegisterHex("weddwaa", new RayCast_Axis());// Architect's Distillation (vector, vector → vector | null)
            api.RegisterHex("weaqa", new RayCast_Entity());// Scout's Distillation (vector, vector → entity | null)
            api.RegisterHex("de", new Print()); // Reveal (any → any)
            api.RegisterHex("awq", new Entity_Height());// Stadiometer's Prfn. (entity → num)
            api.RegisterHex("wq", new Entity_Velocity());// Pace Purification (entity → vector)

            // Constants

            api.RegisterHex("aqae", new Const(new BoolIota(true)));// True Reflection (→ bool)
            api.RegisterHex("dedq", new Const(new BoolIota(false)));// False Reflection (→ bool)
            api.RegisterHex("d", new Const(new NullIota()));// Nullary Reflection (→ null)
            api.RegisterHex("qqqqq", new Const(new Vec3Iota()));// Vector Reflection Zero (→ vector)
            api.RegisterHex("qqqqqea", new Const(new Vec3Iota(1, 0, 0)));// Vector Rfln. +X/-X (→ vector)
            api.RegisterHex("eeeeeqa", new Const(new Vec3Iota(-1, 0, 0)));
            api.RegisterHex("qqqqqew", new Const(new Vec3Iota(0, 1, 0)));// Vector Rfln. +Y/-Y (→ vector)
            api.RegisterHex("eeeeeqw", new Const(new Vec3Iota(0, -1, 0)));
            api.RegisterHex("qqqqqed", new Const(new Vec3Iota(0, 0, 1)));// Vector Rfln. +Z/-Z (→ vector)
            api.RegisterHex("eeeeeqd", new Const(new Vec3Iota(0, 0, -1)));
            api.RegisterHex("eawae", new Const(new DoubleIota(Math.PI * 2)));// Circle's Reflection (→ num)
            api.RegisterHex("qdwdq", new Const(new DoubleIota(Math.PI)));// Arc's Reflection (→ num)
            api.RegisterHex("aaq", new Const(new DoubleIota(Math.E)));// Euler's Reflection (→ num)
            
            // Element Shift

            api.RegisterHex("qqqqqwaeaeaeaeaea", new Const_Sol());
            api.RegisterHex("qqqae", new Const_Lun());
            api.RegisterHex("qeaqwqad", new Const_Cal());
            api.RegisterHex("wqqqqw", new Const_Oce());
            api.RegisterHex("wdaq", new Const_Ste());
            api.RegisterHex("qawa", new Const_Tyr());

            // Stack Manipulation

            api.RegisterHex("aawdd", new SWAP());// Jester's Gambit (any, any → any, any)
            api.RegisterHex("aaeaa", new ROTATE());// Rotation Gambit (any, any, any → any, any, any)
            api.RegisterHex("ddqdd", new ROTATE_REVERSE());// Rotation Gambit II (any, any, any → any, any, any)
            api.RegisterHex("aadaa", new DUPLICATE());// Gemini Decomposition (any → any, any)
            api.RegisterHex("aaedd", new OVER());// Prospector's Gambit (any, any → any, any, any)
            api.RegisterHex("ddqaa", new TUCK());// Undertaker's Gambit (any, any → any, any, any)
            api.RegisterHex("aadadaaw", new TWO_DUP());// Gemini Gambit (any, number → many)
            api.RegisterHex("qwaeawqaeaqa", new STACK_LEN());// Dioscuri Gambit (any, any → any, any, any, any)
            api.RegisterHex("aadaadaa", new DUPLICATE_N());// Flock's Reflection (→ number)
            api.RegisterHex("ddad", new FISHERMAN());// Fisherman's Gambit (number → any)
            api.RegisterHex("aada", new FISHERMAN_COPY());// Fisherman's Gambit II (number → any)
            // Swindler's Gambit (many, number → many)

            // Basic Spells

            api.RegisterHex("aaqawawa", new Effect());// Effect (entity | vector →)
            api.RegisterHex("aqe", new Project());// Project (→)
            api.RegisterHex("aawaawaa", new Explode());// Explode (vector →)
            api.RegisterHex("qaqqqqq", new Place());// Place (vector →)
            api.RegisterHex("qwaawedwewdwe", new Charge());// Charge (num →)

            // Nadir (entity →)
            // Zenith  (entity →)

            // Enchant  (Item →)

            // Extra Spells 

            //api.RegisterHex("aaqawawa", typeof(Ignite));// Ignite (entity | vector →)
        }
        #region Doubles
        public class Add : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State Current) 
            {
                Iota a = Current.Pop();
                Iota b = Current.Pop();
                if (a is DoubleIota && b is DoubleIota)
                {
                    Current.Push(new DoubleIota(((DoubleIota)a).getDouble() + ((DoubleIota)b).getDouble()));
                }
                else if (a is DoubleIota && b is Vec3Iota)
                {
                    Current.Push(new Vec3Iota(((Vec3Iota)b).getVec3().X + ((DoubleIota)a).getDouble(), ((Vec3Iota)b).getVec3().Y + ((DoubleIota)a).getDouble(), ((Vec3Iota)b).getVec3().Z + ((DoubleIota)a).getDouble()));
                }
                else if (a is Vec3Iota && b is DoubleIota)
                {
                    Current.Push(new Vec3Iota(((Vec3Iota)a).getVec3().X + ((DoubleIota)b).getDouble(), ((Vec3Iota)a).getVec3().Y + ((DoubleIota)b).getDouble(), ((Vec3Iota)a).getVec3().Z + ((DoubleIota)b).getDouble()));
                }
                else if (a is Vec3Iota && b is Vec3Iota)
                {
                    Current.Push(new Vec3Iota(((Vec3Iota)a).getVec3() + ((Vec3Iota)b).getVec3()));
                }
                else if (a is ListIota && b is ListIota)
                {
                    ((ListIota)a).getList().AddRange(((ListIota)b).getList());
                    Current.Push(a);
                }
                else
                {
                    return new CastResult(
                        new ListIota(new List<Iota>() { a, b }),
                        Current,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                }
                return new CastResult(
                        new ListIota(new List<Iota>() { a, b }),
                        Current,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
            }
        }
        public class Sub : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack)
            {
                Iota a = stack.Pop();
                Iota b = stack.Pop();
                if (a is DoubleIota && b is DoubleIota)
                {
                    stack.Push(new DoubleIota(((DoubleIota)b).getDouble() - ((DoubleIota)a).getDouble()));
                    
                }
                else if (a is DoubleIota && b is Vec3Iota)
                {
                    stack.Push(new Vec3Iota(((Vec3Iota)b).getVec3().X - ((DoubleIota)a).getDouble(), ((Vec3Iota)b).getVec3().Y - ((DoubleIota)a).getDouble(), ((Vec3Iota)b).getVec3().Z - ((DoubleIota)a).getDouble()));
                }
                else if (a is Vec3Iota && b is DoubleIota)
                {
                    stack.Push(new Vec3Iota(((Vec3Iota)a).getVec3().X - ((DoubleIota)b).getDouble(), ((Vec3Iota)a).getVec3().Y - ((DoubleIota)b).getDouble(), ((Vec3Iota)a).getVec3().Z - ((DoubleIota)b).getDouble()));
                }
                else if (a is Vec3Iota && b is Vec3Iota)
                {
                    stack.Push(new Vec3Iota(((Vec3Iota)b).getVec3() - ((Vec3Iota)a).getVec3()));

                }
                else
                {
                    return new CastResult(
                        new ListIota(new List<Iota>() { a, b }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                    
                }
                return new CastResult(
                        new ListIota(new List<Iota>() { a, b }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
            }
        }
        public class Mul : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack)
            {
                Iota a = stack.Pop();
                Iota b = stack.Pop();
                if ((a is NullIota || b is NullIota) || (a is not DoubleIota && b is not DoubleIota) && (a is not Vec3Iota && b is not Vec3Iota))
                {
                    return new CastResult(
                        new ListIota(new List<Iota>() { a, b }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                    
                }
                if (a is DoubleIota && b is DoubleIota)
                {
                    stack.Push(new DoubleIota(((DoubleIota)b).getDouble() * ((DoubleIota)a).getDouble()));
                } 
                else if (a is Vec3Iota && b is DoubleIota)
                {
                    stack.Push(new Vec3Iota(((DoubleIota)b).getDouble() * ((Vec3Iota)a).getVec3()));
                }
                else if (a is DoubleIota && b is Vec3Iota)
                {
                    stack.Push(new Vec3Iota(((Vec3Iota)b).getVec3() * ((DoubleIota)a).getDouble()));
                }
                else
                {
                    stack.Push(new DoubleIota(((Vec3Iota)b).getVec3() * ((Vec3Iota)a).getVec3()));
                }
                return new CastResult(
                        new ListIota(new List<Iota>() { a, b }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
            }
        }
        public class Div : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack)
            {
                Iota a = stack.Pop();
                Iota b = stack.Pop();
                
                if (a is DoubleIota && b is DoubleIota)
                {
                    stack.Push(new DoubleIota(((DoubleIota)b).getDouble() / ((DoubleIota)a).getDouble()));
                    
                }
                else if (a is DoubleIota && b is Vec3Iota)
                {
                    stack.Push(new Vec3Iota(((Vec3Iota)b).getVec3().X / ((DoubleIota)a).getDouble(), ((Vec3Iota)b).getVec3().Y / ((DoubleIota)a).getDouble(), ((Vec3Iota)b).getVec3().Z / ((DoubleIota)a).getDouble()));
                }
                else if (a is Vec3Iota && b is DoubleIota)
                {
                    stack.Push(new Vec3Iota(((Vec3Iota)a).getVec3().X / ((DoubleIota)b).getDouble(), ((Vec3Iota)a).getVec3().Y / ((DoubleIota)b).getDouble(), ((Vec3Iota)a).getVec3().Z / ((DoubleIota)b).getDouble()));
                }
                else if (a is DoubleIota && b is Vec3Iota)
                {
                    stack.Push(new Vec3Iota(((Vec3Iota)b).getVec3() / ((DoubleIota)a).getDouble()));
                }
                else
                {
                    return new CastResult(
                        new ListIota(new List<Iota>() { a, b }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                    
                }
                return new CastResult(
                        new ListIota(new List<Iota>() { a, b }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                    result.getSound()*/);
            }
        }
        public class Abs : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack)
            {
                Iota a = stack.Pop();

                if (a is ListIota)
                {
                    stack.Push(new DoubleIota(((ListIota)a).getList().Count));
                }
                else if (a is BoolIota)
                {
                    stack.Push(new DoubleIota(((BoolIota)a).getBool() ? 1: 0 ));
                }
                else if (a is DoubleIota)
                {
                    stack.Push(new DoubleIota(Math.Abs(((DoubleIota)a).getDouble())));
                }
                else if (a is Vec3Iota)
                {
                    stack.Push(new DoubleIota(((Vec3Iota)a).getVec3().Length()));
                }
                else
                {
                    return new CastResult(
                        new ListIota(new List<Iota>() { a }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                    
                }
                return new CastResult(
                        new ListIota(new List<Iota>() { a }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
            }
        }
        public class Pow : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack) 
            {
                Iota a = stack.Pop();
                Iota b = stack.Pop();
                
                if (a is DoubleIota && b is DoubleIota)
                {
                    stack.Push(new DoubleIota(Math.Pow(((DoubleIota)b).getDouble(), ((DoubleIota)a).getDouble())));
                }
                else if (a is Vec3Iota && b is DoubleIota)
                {
                    stack.Push(new Vec3Iota(((Vec3Iota)a).getVec3().Pow(((DoubleIota)b).getDouble())));
                }
                else if (a is DoubleIota && b is Vec3Iota)
                {
                    stack.Push(new Vec3Iota(((Vec3Iota)b).getVec3().Pow(((DoubleIota)a).getDouble())));
                }
                else if (a is Vec3Iota && b is Vec3Iota)
                {
                    stack.Push(new Vec3Iota(((Vec3Iota)b).getVec3().proj(((Vec3Iota)a).getVec3())));
                }
                else 
                {
                    return new CastResult(
                        new ListIota(new List<Iota>() { a, b }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                    
                }
                return new CastResult(
                    new ListIota(new List<Iota>() { a, b }),
                    stack,
                    ResolvedPatternType.EVALUATED/*,
                    result.getSound()*/);
            }
        }
        public class Floor : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack)
            {
                Iota a = stack.Pop();
                if (a is DoubleIota)
                {
                    stack.Push(new DoubleIota(Math.Floor(((DoubleIota)a).getDouble())));
                }
                else if (a is Vec3Iota)
                {
                    stack.Push(new Vec3Iota(Math.Floor(((Vec3Iota)a).getVec3().X), Math.Floor(((Vec3Iota)a).getVec3().Y),Math.Floor(((Vec3Iota)a).getVec3().Z)));
                }
                else
                {
                    return new CastResult(
                    new ListIota(new List<Iota>() { a }),
                    stack,
                    ResolvedPatternType.ERRORED/*,
                    result.getSound()*/);
                }

                return new CastResult(
                new ListIota(new List<Iota>() { a }),
                stack,
                ResolvedPatternType.EVALUATED/*,
                result.getSound()*/);
            }
        }
        public class Ceil : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack) 
            {
                Iota a = stack.Pop();
                if (a is DoubleIota)
                {
                    stack.Push(new DoubleIota(Math.Ceiling(((DoubleIota)a).getDouble())));
                }
                else if (a is Vec3Iota)
                {
                    stack.Push(new Vec3Iota(Math.Ceiling(((Vec3Iota)a).getVec3().X), Math.Ceiling(((Vec3Iota)a).getVec3().Y), Math.Ceiling(((Vec3Iota)a).getVec3().Z)));
                }
                else
                {
                    return new CastResult(
                    new ListIota(new List<Iota>() { a }),
                    stack,
                    ResolvedPatternType.ERRORED/*,
                    result.getSound()*/);
                }

                return new CastResult(
                new ListIota(new List<Iota>() { a }),
                stack,
                ResolvedPatternType.EVALUATED/*,
                result.getSound()*/);
            }
        }
        public class Sin : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack) 
            {
                Iota a = stack.Pop();
                if (a is not DoubleIota)
                {
                    return new CastResult(
                    new ListIota(new List<Iota>() { a }),
                    stack,
                    ResolvedPatternType.ERRORED/*,
                    result.getSound()*/);
                }
                stack.Push(new DoubleIota(Math.Sin(((DoubleIota)a).getDouble())));

                return new CastResult(
                new ListIota(new List<Iota>() { a }),
                stack,
                ResolvedPatternType.EVALUATED/*,
                result.getSound()*/);
            }
        }
        public class Cos : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack)
            {
                Iota a = stack.Pop();
                if (a is not DoubleIota)
                {
                    return new CastResult(
                    new ListIota(new List<Iota>() { a }),
                    stack,
                    ResolvedPatternType.ERRORED/*,
                    result.getSound()*/);
                }
                stack.Push(new DoubleIota(Math.Cos(((DoubleIota)a).getDouble())));

                return new CastResult(
                new ListIota(new List<Iota>() { a }),
                stack,
                ResolvedPatternType.EVALUATED/*,
                result.getSound()*/);
            }
        }
        public class Tan : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack)
            {
                Iota a = stack.Pop();
                if (a is not DoubleIota)
                {
                    return new CastResult(
                    new ListIota(new List<Iota>() { a }),
                    stack,
                    ResolvedPatternType.ERRORED/*,
                    result.getSound()*/);
                }
                stack.Push(new DoubleIota(Math.Tan(((DoubleIota)a).getDouble())));

                return new CastResult(
                new ListIota(new List<Iota>() { a }),
                stack,
                ResolvedPatternType.EVALUATED/*,
                result.getSound()*/);
            }
        }
        public class Arcsin : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack)
            {
                Iota a = stack.Pop();
                if (a is not DoubleIota)
                {
                    return new CastResult(
                    new ListIota(new List<Iota>() { a }),
                    stack,
                    ResolvedPatternType.ERRORED/*,
                    result.getSound()*/);
                }
                stack.Push(new DoubleIota(Math.Asin(((DoubleIota)a).getDouble())));

                return new CastResult(
                new ListIota(new List<Iota>() { a}),
                stack,
                ResolvedPatternType.EVALUATED/*,
                result.getSound()*/);
            }
        }
        public class Arccos : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack)
            {
                Iota a = stack.Pop();
                if (a is not DoubleIota)
                {
                    return new CastResult(
                    new ListIota(new List<Iota>() { a }),
                    stack,
                    ResolvedPatternType.ERRORED/*,
                    result.getSound()*/);
                }
                stack.Push(new DoubleIota(Math.Acos(((DoubleIota)a).getDouble())));

                return new CastResult(
                new ListIota(new List<Iota>() { a }),
                stack,
                ResolvedPatternType.EVALUATED/*,
                result.getSound()*/);
            }
        }
        public class ArcTan : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack)
            {
                Iota a = stack.Pop();
                if (a is not DoubleIota)
                {
                    return new CastResult(
                    new ListIota(new List<Iota>() { a }),
                    stack,
                    ResolvedPatternType.ERRORED/*,
                    result.getSound()*/);
                }
                stack.Push(new DoubleIota(Math.Atan(((DoubleIota)a).getDouble())));
                
                return new CastResult(
                new ListIota(new List<Iota>() { a }),
                stack,
                ResolvedPatternType.EVALUATED/*,
                result.getSound()*/);
            }
        }
        public class ArcTan2 : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack) 
            {
                Iota a = stack.Pop();
                Iota b = stack.Pop();
                if (a is not DoubleIota || b is not DoubleIota)
                {
                    return new CastResult(
                    new ListIota(new List<Iota>() { a, b }),
                    stack,
                    ResolvedPatternType.ERRORED/*,
                    result.getSound()*/);
                }

                stack.Push(new DoubleIota(Math.Atan2(((DoubleIota)b).getDouble(), ((DoubleIota)a).getDouble())));

                return new CastResult(
                new ListIota(new List<Iota>() { a, b }),
                stack,
                ResolvedPatternType.EVALUATED/*,
                result.getSound()*/);
            }
        }
        public class Log : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack) 
            {
                Iota a = stack.Pop();
                Iota b = stack.Pop();
                if (a is not DoubleIota || b is not DoubleIota)
                {
                    return new CastResult(
                    new ListIota(new List<Iota>() { a, b }),
                    stack,
                    ResolvedPatternType.ERRORED/*,
                    result.getSound()*/);
                }

                stack.Push(new DoubleIota(Math.Log(((DoubleIota)b).getDouble(), ((DoubleIota)a).getDouble())));
                return new CastResult(
                new ListIota(new List<Iota>() { a, b }),
                stack,
                ResolvedPatternType.EVALUATED/*,
                result.getSound()*/);
            }
        }
        public class Mod : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack) 
            {
                Iota a = stack.Pop();
                Iota b = stack.Pop();
                if (a is  DoubleIota && b is  DoubleIota)
                {
                    stack.Push(new DoubleIota(((DoubleIota)b).getDouble() % ((DoubleIota)a).getDouble()));
                }
                else if(a is Vec3Iota && b is DoubleIota)
                {
                    stack.Push(new Vec3Iota(((DoubleIota)b).getDouble() % ((Vec3Iota)a).getVec3().X, ((DoubleIota)b).getDouble() % ((Vec3Iota)a).getVec3().Y, ((DoubleIota)b).getDouble() % ((Vec3Iota)a).getVec3().Z));
                }
                else if(a is DoubleIota && b is Vec3Iota)
                {
                    stack.Push(new Vec3Iota(((Vec3Iota)b).getVec3().X % ((DoubleIota)a).getDouble(), ((Vec3Iota)b).getVec3().Y % ((DoubleIota)a).getDouble(), ((Vec3Iota)b).getVec3().Z % ((DoubleIota)a).getDouble()));
                }
                else if(a is Vec3Iota && b is Vec3Iota)
                {
                    stack.Push(new Vec3Iota(((Vec3Iota)b).getVec3().X % ((Vec3Iota)a).getVec3().X, ((Vec3Iota)b).getVec3().Y % ((Vec3Iota)a).getVec3().Y, ((Vec3Iota)b).getVec3().Z % ((Vec3Iota)a).getVec3().Z));
                }
                else { 
                    return new CastResult(
                    new ListIota(new List<Iota>() { a, b }),
                    stack,
                    ResolvedPatternType.ERRORED/*,
                    result.getSound()*/);
                }

                
                return new CastResult(
                    new ListIota(new List<Iota>() { a, b }),
                    stack,
                    ResolvedPatternType.EVALUATED/*,
                    result.getSound()*/);
            }
        }
        #endregion

        #region Vecs

        public class Pack : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack) 
            {
                Iota a = stack.Pop();
                Iota b = stack.Pop();
                Iota c = stack.Pop();
                if (a is not DoubleIota || b is not DoubleIota || c is not DoubleIota)
                {
                    return new CastResult(
                    new ListIota(new List<Iota>() {a ,b ,c}),
                    stack,
                    ResolvedPatternType.ERRORED/*,
                    result.getSound()*/);
                }

                stack.Push(new Vec3Iota(((DoubleIota)c).getDouble(), ((DoubleIota)b).getDouble() , ((DoubleIota)a).getDouble()));
                return new CastResult(
                    new ListIota(new List<Iota>() { a, b, c }),
                    stack,
                    ResolvedPatternType.EVALUATED/*,
                    result.getSound()*/);
            }
        }
        public class UnPack : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack) 
            {
                Iota a = stack.Pop();
                if (a is not DoubleIota)
                {
                    return new CastResult(
                        new ListIota(new List<Iota>() { a }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                }

                Vec3d v = ((Vec3Iota)a).getVec3();

                stack.Push(new DoubleIota(v.X));
                stack.Push(new DoubleIota(v.Y));
                stack.Push(new DoubleIota(v.Z));
                return new CastResult(
                new ListIota(new List<Iota>() { a }),
                stack,
                ResolvedPatternType.EVALUATED/*,
                result.getSound()*/);
            }
        }

        #endregion

        #region Lists

        public class Index : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack) 
            {
                Iota num = stack.Pop();
                Iota list = stack.Pop();

                if (list is not ListIota || num is not DoubleIota)
                {
                    return new CastResult(
                    new ListIota(new List<Iota>() { list , num }),
                    stack,
                    ResolvedPatternType.ERRORED/*,
                    result.getSound()*/);
                }

                stack.Push(((ListIota)list).getList()[(int)((DoubleIota)num).getDouble()]);

                return new CastResult(
                    new ListIota(new List<Iota>() { list, num }),
                    stack,
                    ResolvedPatternType.EVALUATED/*,
                    result.getSound()*/);
            }
        }
        public class Slice : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack) 
            {
                Iota num2 = stack.Pop();
                Iota num1 = stack.Pop();
                Iota list = stack.Pop();

                if (list is not ListIota || num1 is not DoubleIota || num2 is not DoubleIota)
                {
                    return new CastResult(
                        new ListIota(new List<Iota>() { list, num1, num2 }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                }

                stack.Push(new ListIota(((ListIota)list).getList().GetRange((int)((DoubleIota)num1).getDouble(), (int)((DoubleIota)num1).getDouble() +(int)((DoubleIota)num1).getDouble())));

                return new CastResult(
                    new ListIota(new List<Iota>() { list, num1, num2 }),
                    stack,
                    ResolvedPatternType.EVALUATED/*,
                    result.getSound()*/);
            }
        }
        public class Append : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack) 
            {
                Iota any = stack.Pop();
                Iota list = stack.Pop();

                if (list is not ListIota)
                {
                    return new CastResult(
                        new ListIota(new List<Iota>() { list, any }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                    
                }

                ((ListIota)list).getList().Add(any);

                stack.Push(list);

                return new CastResult(
                    new ListIota(new List<Iota>() { list, any }),
                    stack,
                    ResolvedPatternType.EVALUATED/*,
                    result.getSound()*/);
            }
        }
        public class UnAppend : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack) 
            {
                Iota list2 = stack.Pop();
                Iota list1 = stack.Pop();

                if (list1 is not ListIota || list2 is not ListIota)
                {
                    return new CastResult(
                        new ListIota(new List<Iota>() { list1, list2 }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                    
                }

                ((ListIota)list1).getList().AddRange(((ListIota)list2).getList());

                stack.Push(list1);

                return new CastResult(
                    new ListIota(new List<Iota>() { list1, list2 }),
                    stack,
                    ResolvedPatternType.EVALUATED/*,
                    result.getSound()*/);
            }
        }
        public class Rev : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack) 
            {
                Iota list = stack.Pop();

                if (list is not ListIota)
                {
                    return new CastResult(
                    new ListIota(new List<Iota>() { list }),
                    stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                    
                }

                ((ListIota)list).getList().Reverse();

                stack.Push(list);

                return new CastResult(
                    new ListIota(new List<Iota>() { list }),
                    stack,
                    ResolvedPatternType.EVALUATED/*,
                    result.getSound()*/);
            }
        }
        public class Index_Of : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack) 
            {
                Iota any = stack.Pop();
                Iota list = stack.Pop();

                if (list is not ListIota)
                {
                    return new CastResult(
                        new ListIota(new List<Iota>() { list, any }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                    
                }

                stack.Push(new DoubleIota(((ListIota)list).getList().IndexOf(any)));

                return new CastResult(
                    new ListIota(new List<Iota>() { list, any }),
                    stack,
                    ResolvedPatternType.EVALUATED/*,
                    result.getSound()*/);
            }
        }
        public class Remove : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack) 
            {
                Iota num = stack.Pop();
                Iota list = stack.Pop();

                if (list is not ListIota && num is not DoubleIota)
                {
                    return new CastResult(
                        new ListIota(new List<Iota>() { list, num }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                    
                }

                ((ListIota)list).getList().RemoveAt((int)((DoubleIota)num).getDouble());

                stack.Push(list);

                return new CastResult(
                    new ListIota(new List<Iota>() { list, num }),
                    stack,
                    ResolvedPatternType.EVALUATED/*,
                    result.getSound()*/);
            }
        }
        public class Replace : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack) 
            {
                Iota any = stack.Pop();
                Iota num = stack.Pop();
                Iota list = stack.Pop();

                if (list is not ListIota && num is not DoubleIota)
                {
                    return new CastResult(
                        new ListIota(new List<Iota>() { list, num, any }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                    
                }

                ((ListIota)list).getList()[((int)((DoubleIota)num).getDouble())] = any;

                stack.Push(list);

                return new CastResult(
                    new ListIota(new List<Iota>() { list, num, any }),
                    stack,
                    ResolvedPatternType.EVALUATED/*,
                    result.getSound()*/);
            }
        }
        public class Cons : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack) 
            {
                Iota num = stack.Pop();
                if (num is not DoubleIota || ((int)((DoubleIota)num).getDouble())>= stack.stack.Count)
                {
                    return new CastResult(
                        new ListIota(new List<Iota>() { num }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                    
                }

                ListIota acc = new ListIota(new List<Iota>());

                for (int i = 0; 1 < ((int)((DoubleIota)num).getDouble()); i++)
                {
                    acc.getList().Add(stack.Pop());
                }

                stack.Push(acc);

                return new CastResult(
                    new ListIota(new List<Iota>() { num , acc }),
                    stack,
                    ResolvedPatternType.EVALUATED/*,
                    result.getSound()*/);
            }
        }
        public class UnCons : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack) 
            {
                Iota list = stack.Pop();
                if (list is not ListIota )
                {
                    return new CastResult(
                        new ListIota(new List<Iota>() { list }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                    
                }

                foreach (Iota I in ((ListIota)list).getList())
                {
                    stack.Push(I);
                }

                return new CastResult(
                    new ListIota(new List<Iota>() { list }),
                    stack,
                    ResolvedPatternType.EVALUATED/*,
                    result.getSound()*/);
            }
        }
        public class EMPTY_LIST : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack) 
            {
                stack.Push(new ListIota());
                return new CastResult(
                    new ListIota(new List<Iota>() { }),
                    stack,
                    ResolvedPatternType.EVALUATED/*,
                    result.getSound()*/);
            }
        }
#endregion

        #region Boolian Logic,Comparisons, & Sets

        public class And : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack) 
            {
                Iota a = stack.Pop();
                Iota b = stack.Pop();
                if (a is BoolIota && b is BoolIota)
                {
                    stack.Push(new BoolIota(((BoolIota)a).getBool() && ((BoolIota)b).getBool()));

                    return new CastResult(
                        new ListIota(new List<Iota>() { a, b }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
                }
                else
                {
                    return new CastResult(
                        new ListIota(new List<Iota>() { a, b }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                    
                }
            }
        }

        public class Or : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack) 
            {
                Iota a = stack.Pop();
                Iota b = stack.Pop();
                if (a is BoolIota && b is BoolIota)
                {
                    stack.Push(new BoolIota(((BoolIota)a).getBool() || ((BoolIota)b).getBool()));

                    return new CastResult(
                        new ListIota(new List<Iota>() { a, b }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
                }
                else
                {
                    return new CastResult(
                        new ListIota(new List<Iota>() { a, b }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                    
                }
            }
        }

        public class Xor : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack) 
            {
                Iota a = stack.Pop();
                Iota b = stack.Pop();
                if (a is BoolIota && b is BoolIota)
                {
                    stack.Push(new BoolIota(((BoolIota)a).getBool() ^ ((BoolIota)b).getBool()));

                    return new CastResult(
                        new ListIota(new List<Iota>() { a, b }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
                }
                else
                {
                    return new CastResult(
                        new ListIota(new List<Iota>() { a, b }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                    
                }
            }
        }

        public class Greater : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack) 
            {
                Iota a = stack.Pop();
                Iota b = stack.Pop();
                if (a is DoubleIota && b is DoubleIota)
                {
                    stack.Push(new BoolIota(((DoubleIota)b).getDouble() > ((DoubleIota)a).getDouble()));

                    return new CastResult(
                        new ListIota(new List<Iota>() { a, b }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
                }
                else
                {
                    return new CastResult(
                        new ListIota(new List<Iota>() { a, b }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                    
                }
            }
        }

        public class Less : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack) 
            {
                Iota a = stack.Pop();
                Iota b = stack.Pop();
                if (a is DoubleIota && b is DoubleIota)
                {
                    stack.Push(new BoolIota(((DoubleIota)b).getDouble() < ((DoubleIota)a).getDouble()));

                    return new CastResult(
                        new ListIota(new List<Iota>() { a, b }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
                }
                else
                {
                    return new CastResult(
                        new ListIota(new List<Iota>() { a, b }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                    
                }
            }
        }

        public class Greater_Eq : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack) 
            {
                Iota a = stack.Pop();
                Iota b = stack.Pop();
                if (a is DoubleIota && b is DoubleIota)
                {
                    stack.Push(new BoolIota(((DoubleIota)b).getDouble() >= ((DoubleIota)a).getDouble()));

                    return new CastResult(
                        new ListIota(new List<Iota>() { a, b }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
                }
                else
                {
                    return new CastResult(
                        new ListIota(new List<Iota>() { a, b }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                    
                }
            }
        }

        public class Less_Eq : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack) 
            {
                Iota a = stack.Pop();
                Iota b = stack.Pop();
                if (a is DoubleIota && b is DoubleIota)
                {
                    stack.Push(new BoolIota(((DoubleIota)b).getDouble() <= ((DoubleIota)a).getDouble()));

                    return new CastResult(
                        new ListIota(new List<Iota>() { a, b }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
                }
                else
                {
                    return new CastResult(
                        new ListIota(new List<Iota>() { a, b }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                    
                }
            }
        }
        public class Equals : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack) 
            {
                Iota a = stack.Pop();
                Iota b = stack.Pop();
                stack.Push(new BoolIota((b) == (a)));

                return new CastResult(
                    new ListIota(new List<Iota>() { a, b }),
                    stack,
                    ResolvedPatternType.EVALUATED/*,
                    result.getSound()*/);
            }
        }
        public class Not_Equal : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack) 
            {
                Iota a = stack.Pop();
                Iota b = stack.Pop();
                
                stack.Push(new BoolIota((b) != (a)));

                return new CastResult(
                    new ListIota(new List<Iota>() { a, b }),
                    stack,
                    ResolvedPatternType.EVALUATED/*,
                    result.getSound()*/);
                
            }
        }

        public class Not : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack) 
            {
                Iota any = stack.Pop();

                stack.Push(new BoolIota(!any.isTruthy()));

                return new CastResult(
                    new ListIota(new List<Iota>() { any }),
                    stack,
                    ResolvedPatternType.EVALUATED/*,
                    result.getSound()*/);
            }
        }
        public class Bool_Coerce : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack) 
            {
                Iota any = stack.Pop();

                stack.Push(new BoolIota(any.isTruthy()));

                return new CastResult(
                    new ListIota(new List<Iota>() { any }),
                    stack,
                    ResolvedPatternType.EVALUATED/*,
                    result.getSound()*/);
            }
        }
        public class If : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack) 
            {
                Iota any2 = stack.Pop();
                Iota any1 = stack.Pop();
                Iota Bool = stack.Pop();

                if (Bool is BoolIota )
                {
                    stack.Push(((BoolIota)Bool).getBool() ? any1 : any2);

                    return new CastResult(
                        new ListIota(new List<Iota>() { Bool, any1, any2 }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
                }
                else
                {
                    return new CastResult(
                        new ListIota(new List<Iota>() { Bool, any1, any2 }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                    
                }
            }
        }
        public class Rand : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack) 
            {

                stack.Push(new DoubleIota(Math.Round(api.World.Rand.Next(1000)/1000f,3)));

                return new CastResult(
                    new ListIota(new List<Iota>() {  }),
                    stack,
                    ResolvedPatternType.EVALUATED/*,
                    result.getSound()*/);
                
            }
        }

        public class Unique : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack) 
            {
                Iota list = stack.Pop();

                List<Iota> temp = new List<Iota>();

                foreach (Iota item in ((ListIota)list).getList())
                {
                    if (!temp.Contains(item)){
                        temp.Add(item);
                    }
                }

                stack.Push(new ListIota(temp));

                return new CastResult(
                    new ListIota(new List<Iota>() { list }),
                    stack,
                    ResolvedPatternType.EVALUATED/*,
                    result.getSound()*/);
            }
        }
        #endregion

        #region Meta-Evaluation
        public class Eval : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack) 
            {
                bool first = stack == 0;
                stack.stack.TryPop(out Iota list);
                if (list is not ListIota)
                {
                    return new CastResult(
                        new ListIota(new List<Iota>() { list }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                    
                }
                int listing = 0;
                bool click = false;
                bool fclick = false;
                List<Iota> temp = new();
                foreach (Iota i in ((ListIota)list).getList())
                {
                    if (click || listing > 0) //If not eval
                    {
                        if (click)
                        {
                            stack.Push(i);
                            click = false;
                            continue;
                        }
                        else if (fclick)
                        {
                            fclick = false;
                        }
                        else if(i is PatternIota)
                        {
                            PatternIota pattern = i as PatternIota;
                            if (pattern.getPattern().anglesSignature() == "qqqaw")
                            {
                                fclick = true;
                            } 
                            else if (pattern.getPattern().anglesSignature() == "qqq")
                            {
                                listing++;
                            }
                            else if (pattern.getPattern().anglesSignature() == "eee")
                            {
                                listing--;
                                if (listing == 0)
                                {
                                    stack.Push(new ListIota(temp));
                                    temp = new();
                                    continue;
                                }
                            }
                        }
                        temp.Add(i);
                        
                    }
                    else if (i is not PatternIota) //If eval, and can't
                    {
                        return new CastResult(
                            new ListIota(new List<Iota>() { list }),
                            stack,
                            ResolvedPatternType.ERRORED/*,
                            result.getSound()*/);
                        
                    }
                    else //If eval
                    { 
                        PatternIota pattern = i as PatternIota;
                        if (pattern.getPattern().anglesSignature() == "qqqaw")
                        {
                            click = true;
                        }
                        else if (pattern.getPattern().anglesSignature() == "qqq")
                        {
                            listing++;
                        }
                        else
                        {
                            CastResult castResult = pattern.execute(player, api, stack);
                            if (!castResult.resolutionType.success)
                            {
                                if (first)
                                {
                                    ((IServerPlayer)((EntityPlayer)player)?.Player)?.SendMessage(0, string.Format("<i><font color = \"{0}\">{1}</font></i>" ,castResult.resolutionType.colour.ToString("X6"), castResult.cast.ToString()), EnumChatType.Notification);
                                }
                                return castResult;
                            }
                        }
                    }
                }
                if (click || listing > 0)
                {
                    return new CastResult(
                        new ListIota(new List<Iota>() { list }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                }


                return new CastResult(
                    new ListIota(new List<Iota>() { list }),
                    stack,
                    ResolvedPatternType.EVALUATED/*,
                    result.getSound()*/);


            }
        }
        public class FOR_EACH : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack) 
            {
                stack.stack.TryPop(out Iota list);
                stack.stack.TryPop(out Iota Patterns);
                if (list is not ListIota || Patterns is not ListIota)
                {
                    return new CastResult(
                        new ListIota(new List<Iota>() { list , Patterns }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                }

                foreach(Iota i in ((ListIota)list).getList())
                {
                    State inner = new(i);
                    inner.evals = stack.evals;
                    inner.Push(Patterns);
                    Eval n = new Eval();
                    CastResult castResult = n.Run(player, api, inner);
                    stack.Push(inner.Open());
                    if (!castResult.resolutionType.success)
                    {
                        return castResult;
                    }
                    stack.evals = inner.evals;
                }

                return new CastResult(
                    new ListIota(new List<Iota>() { list }),
                    stack,
                    ResolvedPatternType.EVALUATED/*,
                    result.getSound()*/);
            }
        }
#endregion

        #region Read and Write

        public class Read : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack) 
            {
                ItemStack Offhand = ((EntityPlayer)player)?.Player.InventoryManager?.OffhandHotbarSlot?.Itemstack;
                if (Offhand is not null)
                {
                    stack.Push(((Focus)Offhand.Item)?.GetIota(Offhand));
                }
                else
                {
                    return new CastResult(
                        new ListIota(new List<Iota>() { }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                }
                return new CastResult(
                        new ListIota(new List<Iota>() { }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
            }
        }
        public class Write : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack) 
            {
                Iota Data = stack.Pop();
                if (Data is not null)
                {
                    if(Data is ListIota list)
                    {
                        if (list.containedPlayerUids.Length > 1 || (list.containedPlayerUids.Length > 0 && list.containedPlayerUids[0] != (player as EntityPlayer).PlayerUID))
                        {
                            return new CastResult(
                                 new ListIota(new List<Iota>() { Data }),
                                 stack,
                                 ResolvedPatternType.ERRORED/*,
                                 result.getSound()*/);
                        }
                    }
                    else if (Data is EntityIota entity)
                    {
                        if ((entity.getEntity() as EntityPlayer).PlayerUID != (player as EntityPlayer).PlayerUID) 
                        {
                            return new CastResult(
                                 new ListIota(new List<Iota>() { Data }),
                                 stack,
                                 ResolvedPatternType.ERRORED/*,
                                 result.getSound()*/);
                        }
                    }
                    ItemStack Offhand = ((EntityPlayer)player)?.Player.InventoryManager?.OffhandHotbarSlot?.Itemstack;
                    ((Focus)Offhand.Item)?.SetIota(Data, Offhand);
                }
                else
                {
                    return new CastResult(
                        new ListIota(new List<Iota>() { Data }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                }
                return new CastResult(
                        new ListIota(new List<Iota>() { Data }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
            }
        }
#endregion

        #region Basics
        public class Get_caster : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack) 
            {
                stack.Push(new EntityIota(player));
                return new CastResult(
                    new ListIota(new List<Iota>() { }),
                    stack,
                    ResolvedPatternType.EVALUATED/*,
                    result.getSound()*/);
            }
        }
        public class Entity_PosEye : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack) 
            {
                Iota e = stack.Pop();
                if (e is EntityIota)
                {
                    stack.Push(new Vec3Iota(((EntityIota)e).getEntity().LocalEyePos + ((EntityIota)e).getEntity().Pos.XYZ));
                }
                else
                {
                    return new CastResult(
                        new ListIota(new List<Iota>() { e }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                }
                return new CastResult(
                        new ListIota(new List<Iota>() { e }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
            }
        }
        public class Entity_PosFoot : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack) 
            {
                Iota e = stack.Pop();
                if (e is EntityIota)
                {
                    stack.Push(new Vec3Iota(((EntityIota)e).getEntity().Pos.XYZ));
                }
                else
                {
                    return new CastResult(
                        new ListIota(new List<Iota>() { e }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                }
                return new CastResult(
                        new ListIota(new List<Iota>() { e }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
            }
        }
        
        public class Entity_Look : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack) 
            {
                Iota e = stack.Pop();
                if (e is EntityIota)
                {
                    float cosPitch = GameMath.Cos(((EntityIota)e).getEntity().Pos.Pitch );
                    float sinPitch = GameMath.Sin(((EntityIota)e).getEntity().Pos.Pitch );

                    float cosYaw = GameMath.Cos(((EntityIota)e).getEntity().Pos.Yaw );
                    float sinYaw = GameMath.Sin(((EntityIota)e).getEntity().Pos.Yaw );

                    stack.Push(new Vec3Iota(new Vec3d(-cosPitch * sinYaw, sinPitch, -cosPitch * cosYaw)));
                }
                else
                {
                    return new CastResult(
                        new ListIota(new List<Iota>() { e }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                }
                return new CastResult(
                        new ListIota(new List<Iota>() { e }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
            }
        }
        

        public class RayCast : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack) 
            {
                Iota Dir = stack.Pop();
                Iota Start = stack.Pop();
                if (Start is Vec3Iota && Dir is Vec3Iota)
                {
                    Vec3d d = (Dir as Vec3Iota).getVec3();

                    d /= d.Length();
                    d *= 100;
                    BlockSelection B = new();
                    EntitySelection E = new();
                    api.World.RayTraceForSelection((Start as Vec3Iota).getVec3(), (Start as Vec3Iota).getVec3() + d, ref B, ref E,null,new EntityFilter((n) => false));

                    if (B?.Block is null)
                    {
                        stack.Push(new NullIota());
                    }
                    else
                    {
                        stack.Push(new Vec3Iota(B.Position.ToVec3d()));
                    }

                }
                else
                {
                    return new CastResult(
                        new ListIota(new List<Iota>() { Start, Dir }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                }
                return new CastResult(
                        new ListIota(new List<Iota>() { Start, Dir }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
            }
        }
        public class RayCast_Axis : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack) 
            {
                Iota Dir = stack.Pop();
                Iota Start = stack.Pop();
                if (Start is Vec3Iota && Dir is Vec3Iota)
                {
                    Vec3d d = (Dir as Vec3Iota).getVec3();

                    d /= d.Length();
                    d *= 100;
                    BlockSelection B = new();
                    EntitySelection E = new();
                    api.World.RayTraceForSelection((Start as Vec3Iota).getVec3(), (Start as Vec3Iota).getVec3() + d, ref B, ref E, null, new EntityFilter((n) => false));

                    if (B?.Block is null)
                    {
                        stack.Push(new NullIota());
                    }
                    else
                    {
                        stack.Push(new Vec3Iota(B.Face.Normald));
                    }

                }
                else
                {
                    return new CastResult(
                        new ListIota(new List<Iota>() { Start, Dir }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                }
                return new CastResult(
                        new ListIota(new List<Iota>() { Start, Dir }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
            }
        }
        public class RayCast_Entity : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack) 
            {
                Iota Dir = stack.Pop();
                Iota Start = stack.Pop();
                if (Start is Vec3Iota && Dir is Vec3Iota)
                {
                    Vec3d d = (Dir as Vec3Iota).getVec3();

                    d /= d.Length();
                    d *= 100;
                    BlockSelection B = new();
                    EntitySelection E = new();
                    api.World.RayTraceForSelection((Start as Vec3Iota).getVec3(), (Start as Vec3Iota).getVec3() + d, ref B, ref E, new BlockFilter((n,m) => false), new EntityFilter((n) => n.EntityId != player.EntityId));

                    if (E?.Entity is null)
                    {
                        stack.Push(new NullIota());
                    }
                    else
                    {
                        stack.Push(new EntityIota(E.Entity));
                    }

                }
                else
                {
                    return new CastResult(
                        new ListIota(new List<Iota>() { Start, Dir }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                }
                return new CastResult(
                        new ListIota(new List<Iota>() { Start, Dir }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
            }
        }


        public class Print : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack) 
            {
                Iota any = stack.Pop();

                if (any is not null)
                {
                    ((IServerPlayer)((EntityPlayer)player)?.Player)?.SendMessage(0, any?.ToString(), EnumChatType.OwnMessage);
                    stack.Push(any);
                }
                
                return new CastResult(
                    new ListIota(new List<Iota>() { any }),
                    stack,
                    ResolvedPatternType.EVALUATED/*,
                    result.getSound()*/);
            }
        }
        public class Entity_Height : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack) 
            {
                Iota e = stack.Pop();
                if (e is EntityIota)
                {
                    stack.Push(new DoubleIota(((EntityIota)e).getEntity().CollisionBox.Height));
                }
                else
                {
                    return new CastResult(
                        new ListIota(new List<Iota>() { e }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                }
                return new CastResult(
                        new ListIota(new List<Iota>() { e }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
            }
        }

        
        public class Entity_Velocity : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack) 
            {
                Iota e = stack.Pop();
                if (e is EntityIota)
                {
                    stack.Push(new Vec3Iota(((EntityIota)e).getEntity().Pos.Motion));
                }
                else
                {
                    return new CastResult(
                        new ListIota(new List<Iota>() { e }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                }
                return new CastResult(
                        new ListIota(new List<Iota>() { e }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
            }
        }
        #endregion

        #region Constants


        public class Const : OperatorBase
        {
            Iota output;
            public Const(Iota output) 
            {
                this.output = output;
            }
            public override CastResult Run(Entity player, ICoreAPI api, State stack)
            {
                stack.Push(output);
                return new CastResult(
                        new ListIota(new List<Iota>() { }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
            }
        }

        #endregion

        #region Stack Manipulation

        // Jester's Gambit (any, any → any, any)
        public class SWAP : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack) 
            {
                Iota a = stack.Pop();
                Iota b = stack.Pop();

                if ( a is null || b is null)
                {
                    return new CastResult(
                        new ListIota(new List<Iota>() { a, b }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                }

                stack.Push(a);
                stack.Push(b);
                return new CastResult(
                        new ListIota(new List<Iota>() { a, b }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
            }
        }
        // Rotation Gambit (any, any, any → any, any, any)
        public class ROTATE : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack) 
            {

                Iota a = stack.Pop();
                Iota b = stack.Pop();
                Iota c = stack.Pop();

                if (a is null || b is null || c is null)
                {
                    return new CastResult(
                        new ListIota(new List<Iota>() { a, b, c }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                }

                stack.Push(b);
                stack.Push(a);
                stack.Push(c);
                return new CastResult(
                        new ListIota(new List<Iota>() { a, b, c }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);

            }
        }
        // Rotation Gambit II (any, any, any → any, any, any)
        public class ROTATE_REVERSE : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack) 
            {

                Iota a = stack.Pop();
                Iota b = stack.Pop();
                Iota c = stack.Pop();

                if (a is null || b is null || c is null)
                {
                    return new CastResult(
                        new ListIota(new List<Iota>() { a, b, c }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                }

                stack.Push(a);
                stack.Push(c);
                stack.Push(b);
                return new CastResult(
                        new ListIota(new List<Iota>() { a, b, c }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);


            }
        }
        // Gemini Decomposition (any → any, any)
        public class DUPLICATE : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack) 
            {

                Iota a = stack.Pop();

                if (a is null )
                {
                    return new CastResult(
                        new ListIota(new List<Iota>() { a}),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                }

                stack.Push(a);
                stack.Push(a);
                return new CastResult(
                        new ListIota(new List<Iota>() { a }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);


            }
        }
        // Prospector's Gambit (any, any → any, any, any)
        public class OVER : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack) 
            {

                Iota a = stack.Pop();
                Iota b = stack.Pop();

                if (a is null || b is null)
                {
                    return new CastResult(
                        new ListIota(new List<Iota>() { a, b }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                }

                stack.Push(b);
                stack.Push(a);
                stack.Push(b);
                return new CastResult(
                        new ListIota(new List<Iota>() { a, b }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);

            }
        }
        // Undertaker's Gambit (any, any → any, any, any)
        public class TUCK : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack) 
            {

                Iota a = stack.Pop();
                Iota b = stack.Pop();

                if (a is null || b is null)
                {
                    return new CastResult(
                        new ListIota(new List<Iota>() { a, b }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                }

                stack.Push(a);
                stack.Push(b);
                stack.Push(a);
                return new CastResult(
                        new ListIota(new List<Iota>() { a, b }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);

            }
        }
        // Gemini Gambit (any, number → many)
        public class DUPLICATE_N : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack) 
            {

                Iota num = stack.Pop();
                Iota any = stack.Pop();

                if (num is null || any is null || num is not DoubleIota)
                {
                    return new CastResult(
                        new ListIota(new List<Iota>() { num, any }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                }

                for (int i = 0; i < ((DoubleIota)num).getDouble(); i++)
                {
                    stack.Push(any);
                }

                return new CastResult(
                    new ListIota(new List<Iota>() { num, any }),
                    stack,
                    ResolvedPatternType.EVALUATED/*,
                    result.getSound()*/);

            }
        }
        // Dioscuri Gambit (any, any → any, any, any, any)
        public class TWO_DUP : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack) 
            {

                Iota a = stack.Pop();
                Iota b = stack.Pop();

                if (a is null || b is null)
                {
                    return new CastResult(
                        new ListIota(new List<Iota>() { a, b }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                }

                stack.Push(b);
                stack.Push(a);
                stack.Push(b);
                stack.Push(a);

                return new CastResult(
                        new ListIota(new List<Iota>() { a, b }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);

            }
        }
        // Flock's Reflection (→ number)
        public class STACK_LEN : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack) 
            {

                stack.Push(new DoubleIota(stack.stack.Count()));
                return new CastResult(
                        new ListIota(new List<Iota>() {  }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
            }
        }
        // Fisherman's Gambit (number → any)
        public class FISHERMAN : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack) 
            {

                Iota num = stack.Pop();

                if (num is null  || num is not DoubleIota || ((DoubleIota)num).getDouble() > stack.stack.Count())
                {
                    return new CastResult(
                        new ListIota(new List<Iota>() { num }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                }
                if (((DoubleIota)num).getDouble() == 0)
                {
                    return new CastResult(
                        new ListIota(new List<Iota>() { num }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
                }


                Stack<Iota> tstack = new();

                for (int i = 0; i < ((DoubleIota)num).getDouble(); i++)
                {
                    tstack.Push(stack.Pop());
                }
                Iota tIota = tstack.Pop();

                for (int i = 1; i < ((DoubleIota)num).getDouble(); i++)
                {
                    stack.Push(tstack.Pop());
                }
                stack.Push(tIota);
                return new CastResult(
                        new ListIota(new List<Iota>() { num }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
            }
        }
        // Fisherman's Gambit II (number → any)
        public class FISHERMAN_COPY : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack) 
            {

                Iota num = stack.Pop();

                if (num is null || num is not DoubleIota || ((DoubleIota)num).getDouble() > stack.stack.Count())
                {
                    return new CastResult(
                        new ListIota(new List<Iota>() { num }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                }

                if (((DoubleIota)num).getDouble() == 0)
                {
                    return new CastResult(
                        new ListIota(new List<Iota>() { num }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
                }

                Stack<Iota> tstack = new();

                for (int i = 0; i < ((DoubleIota)num).getDouble(); i++)
                {
                    tstack.Push(stack.Pop());
                }
                Iota tIota = tstack.Pop();
                stack.Push(tIota);

                for (int i = 1; i < ((DoubleIota)num).getDouble(); i++)
                {
                    stack.Push(tstack.Pop());
                }
                stack.Push(tIota);

                return new CastResult(
                        new ListIota(new List<Iota>() { num }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
            }
        }
#endregion

        #region Elements Constants

        public class Const_Sol : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack) 
            {
                
                Iota pow = stack.Pop();
                if(pow is DoubleIota p)
                {
                    stack.Shift(new Solarium((int)p.getDouble()));
                }
                else
                {
                    stack.Shift(new Solarium(1));
                    if (pow is not null)
                        stack.Push(pow);
                }
                return new CastResult(
                        new ListIota(new List<Iota>() { }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
            }
        }
        public class Const_Lun : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack) 
            {
                Iota pow = stack.Pop();
                if (pow is DoubleIota p)
                {
                    stack.Shift(new Lunarium((int)p.getDouble()));
                }
                else
                {
                    stack.Shift(new Lunarium(1));
                    if (pow is not null)
                        stack.Push(pow);
                }
                return new CastResult(
                        new ListIota(new List<Iota>() { }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
            }
        }
        public class Const_Tyr : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack) 
            {
                Iota pow = stack.Pop();
                if (pow is DoubleIota p)
                {
                    stack.Shift(new Tyerraium((int)p.getDouble()));
                }
                else
                {
                    stack.Shift(new Tyerraium(1));
                    if (pow is not null)
                        stack.Push(pow);
                }
                return new CastResult(
                        new ListIota(new List<Iota>() { }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
            }
        }
        public class Const_Cal : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack) 
            {
                Iota pow = stack.Pop();
                if (pow is DoubleIota p)
                {
                    stack.Shift(new Caelium((int)p.getDouble()));
                }
                else
                {
                    stack.Shift(new Caelium(1));
                    if (pow is not null)
                        stack.Push(pow);
                }
                return new CastResult(
                        new ListIota(new List<Iota>() { }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
            }
        }
        public class Const_Oce : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack) 
            {
                Iota pow = stack.Pop();
                if (pow is DoubleIota p)
                {
                    stack.Shift(new Oceanium((int)p.getDouble()));
                }
                else
                {
                    stack.Shift(new Oceanium(1));
                    if (pow is not null)
                        stack.Push(pow);
                }
                return new CastResult(
                        new ListIota(new List<Iota>() { }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
            }
        }
        public class Const_Ste : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack) 
            {
                Iota pow = stack.Pop();
                if (pow is DoubleIota p)
                {
                    stack.Shift(new Stellarium((int)p.getDouble()));
                }
                else
                {
                    stack.Shift(new Stellarium(1));
                    if (pow is not null)
                        stack.Push(pow);
                }
                return new CastResult(
                        new ListIota(new List<Iota>() { }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
            }
        }
        #endregion
        
        #region Base Spells
        public class Effect : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack) 
            {
                Iota e = stack.Pop();

                bool suc = false;

                if (e is EntityIota ent)
                {
                    suc = stack.Shift().Effect(player, ent.getEntity());
                }
                else if (e is Vec3Iota vec)
                {
                    suc = stack.Shift().Effect(player, vec.getVec3());
                }
                if (suc)
                {
                    return new CastResult(
                        new ListIota(new List<Iota>() { e }),
                        stack,
                        ResolvedPatternType.EVALUATED);
                }
                else
                {
                    return new CastResult(
                        new ListIota(new List<Iota>() { e }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                }
            }
        }

        public class Explode : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack) 
            {
                Iota e = stack.Pop();

                bool suc = false;

                if (e is Vec3Iota vec)
                {
                    suc = stack.Shift().Explode(player, vec.getVec3());
                }
                if (suc)
                {
                    return new CastResult(
                        new ListIota(new List<Iota>() { e }),
                        stack,
                        ResolvedPatternType.EVALUATED);
                }
                else
                {
                    return new CastResult(
                        new ListIota(new List<Iota>() { e }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                }
            }
        }
        public class Place : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack) 
            {
                Iota e = stack.Pop();

                bool suc = false;

                if (e is Vec3Iota vec)
                {
                    if (player.CanUseMedia(10 * ((float)stack.exp)))
                        suc = stack.Shift().Place(player, vec.getVec3());
                }
                if (suc)
                {
                    player.UseMedia(10 * ((float)stack.exp), stack.Shift().type);
                    stack.inc();
                    return new CastResult(
                        new ListIota(new List<Iota>() { e }),
                        stack,
                        ResolvedPatternType.EVALUATED);
                }
                else
                {
                    return new CastResult(
                        new ListIota(new List<Iota>() { e, new DoubleIota(Math.Round(player.GetMedia(), 2))}),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                }
            }
        }
        public class Project : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack) 
            {
                bool suc = false;
                EnumHexStrain pain = (EnumHexStrain)Math.Clamp((stack.Shift().strength - player.GetAffinities(stack.Shift().type)) / 5 , 0, 2);

                if (player.CanUseMedia(stack.Shift().strength * 5 * ((float)stack.exp), stack.Shift().type, pain))
                    suc = stack.Shift().Project(player);
                
                
                if (suc)
                {
                    player.UseMedia(stack.Shift().strength * 5 * ((float)stack.exp), stack.Shift().type, pain);
                    stack.inc();
                    return new CastResult(
                        new ListIota(new List<Iota>() {  }),
                        stack,
                        ResolvedPatternType.EVALUATED);
                }
                else
                {
                    return new CastResult(
                        new ListIota(new List<Iota>() { new DoubleIota(Math.Round(player.GetMedia(),2)) }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                }
            }
        }

        public class Charge : OperatorBase
        {
            public override CastResult Run(Entity player, ICoreAPI api, State stack)
            {
                Iota e = stack.Pop();

                bool suc = false;

                if (e is DoubleIota num)
                {
                    if (player.CanUseMedia(10 * (Math.Min((float)stack.exp, (float)num.getDouble()*5))))
                    {
                        ItemStack Offhand = ((EntityPlayer)player)?.Player.InventoryManager?.OffhandHotbarSlot?.Itemstack;
                        if (Offhand is not null)
                        {
                            if ((Offhand.Attributes.TryGetInt("mediamaxcharge") ?? 0) is int maxcharge) 
                            {
                                
                                Element ele = stack.Shift();
                                int charge = (int)Math.Floor(Math.Min(num.getDouble(), (double)maxcharge));

                                IAttribute elementAtt;
                                Offhand.Attributes.TryGetAttribute("element", out elementAtt);

                                string currentelement = ((StringAttribute)elementAtt)?.value ?? "";

                                int currentcharge = Offhand.Attributes.TryGetInt("charge") ?? -1;

                                if (currentcharge < charge && charge <= maxcharge)
                                {
                                    if (currentelement == "" || (ele.type == "" ? "media" : ele.type) == currentelement)
                                    {
                                        Offhand.Attributes.SetString("element", (ele.type == "" ? "media" : ele.type));
                                        Offhand.Attributes.SetInt("charge", charge);
                                        suc = true;
                                    }
                                }
                            }
                        }
                    }
                        
                }

                if (suc)
                {
                    player.UseMedia(10 * (Math.Min((float)stack.exp, (float)((DoubleIota)e).getDouble() * 5)), stack.Shift().type);
                    stack.inc();
                    return new CastResult(
                        new ListIota(new List<Iota>() { e }),
                        stack,
                        ResolvedPatternType.EVALUATED);
                }
                else
                {
                    return new CastResult(
                        new ListIota(new List<Iota>() { e, new DoubleIota(Math.Round(player.GetMedia(), 2)) }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                }
            }
        }

        #endregion

        // Extra Spells 


    }

    public static class Vec3Helper
    {
        public static Vec3d proj(this Vec3d a, Vec3d b)
        {
            GeneralMatrix m = new GeneralMatrix(b);
            GeneralMatrix n = m.AxesFlip();

            return a * (n * m);

        }
        public static Vec3d Pow(this Vec3d a, double b)
        {
            return new Vec3d(Math.Pow(a.X , b), Math.Pow(a.Y, b), Math.Pow(a.Z, b));
        }

    }

    public class GeneralMatrix
    {
        public double[,] data = new double[,] { { 0 } };
        public int Height = 1;
        public int Width = 1;

        public GeneralMatrix()
        {
            
        }
        public GeneralMatrix(double[,] data)
        {
            this.data = data;
            Height = data.GetLength(0);
            Width = data.GetLength(1);
        }
        public GeneralMatrix(Vec3d data)
        {
            this.data = new double[,] { { data.X, data.Y , data.Z } };
            Height = 1;
            Width = 3;
        }

        public GeneralMatrix AxesFlip()
        {

            GeneralMatrix temp2 = new GeneralMatrix(new double[Width, Height]);

            for (int x = 0; x < data.GetLength(0); x++) 
            {
                for (int y = 0; y < data.GetLength(1); y++)
                {
                    temp2.data[y,x] = data[x,y];
                }
            }

            return temp2;
        }

        public GeneralMatrix Mult(GeneralMatrix b)
        {
            if (this.Width != b.Height)
                return null;
            GeneralMatrix temp = new GeneralMatrix(new double[this.Height, b.Width]);
            for (int x = 0; x < this.Height; x++)
            {
                for (int y = 0; y < b.Width; x++)
                {
                    double acc = 0;
                    for (int n = 0; n < b.Height; n++)
                    {
                        acc += data[x, y] + b.data[y, x];
                    }
                    temp.data[x, y] = acc;
                }
            }

            return temp;
        }

        public GeneralMatrix Add(GeneralMatrix b) 
        {
            if (this.Width != b.Width || this.Height != b.Height)
                return null;
            GeneralMatrix temp = new GeneralMatrix(new double[this.Height, b.Width]);
            for (int x = 0; x < this.Height; x++)
            {
                for (int y = 0; y < b.Width; x++)
                {
                    temp.data[x,y] = data[x, y] + b.data[x, y];
                }
            }

            return temp;
        }

        public double Det()
        {
            if (this.Width != this.Height)
                return 0;

            if (this.Width == 2)
                return (data[0, 0] * data[1, 1]) - (data[0, 1] * data[1, 0]);

            int neg = 1;
            double acc = 0;
            for (int x = 0; x < this.Width; x++)
            {
                GeneralMatrix temp = new GeneralMatrix(new double[Width-1, Width-1]);

                int flag = 0;
                for (int xt = 0; xt < this.Width; xt++)
                {
                    for (int yt = 0; yt < this.Width; yt++)
                    {
                        if (xt == x)
                        {
                            flag = 1;
                            continue;
                        }

                        temp.data[yt,xt+flag] = data[yt,xt];

                    }
                }

                acc += neg * (data[0, x] + temp.Det());

                neg *= -1;
            }

            return acc;
        }


        public static GeneralMatrix operator +(GeneralMatrix a, GeneralMatrix b) => a.Add(b);
        public static GeneralMatrix operator *(GeneralMatrix a, GeneralMatrix b) => a.Mult(b);
        public static GeneralMatrix operator *(Vec3d a, GeneralMatrix b) => new GeneralMatrix(a).Mult(b);

        public static implicit operator Vec3d(GeneralMatrix a) => new Vec3d(a.data[0, 0], a.data[0, 1], a.data[0, 2]);
    }
}
