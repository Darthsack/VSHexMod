using Cairo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.GameContent;
using Vintagestory.Server;
using VSHexMod.EntityBehaviors;
using VSHexMod.hexcasting.api.casting.eval;
using VSHexMod.hexcasting.api.casting.eval.BaseElements;
using VSHexMod.hexcasting.api.casting.eval.iota;
using VSHexMod.hexcasting.common.items.magic;
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
            api.RegisterHex("waaw", typeof(Add)); // Additive Distillation (num|vec, num|vec → num|vec) // Additive Distillation (list, list → list)
            api.RegisterHex("wddw", typeof(Sub)); // Subtractive Distillation (num|vec, num|vec → num|vec)
            api.RegisterHex("waqaw", typeof(Mul)); // Multiplicative Dstl. (num|vec, num|vec → num|vec)
            api.RegisterHex("wdedw", typeof(Div)); // Division Dstl. (num|vec, num|vec → num|vec)
            api.RegisterHex("wqaqw", typeof(Abs)); // Length Purification (num|vec → number) // Length Purification (list → num) // Length Purification (bool → number)
            api.RegisterHex("wedew", typeof(Pow)); // Power Distillation (num|vec, num|vec → num|vec)
            api.RegisterHex("ewq", typeof(Floor)); // Floor Purification (num|vec → num|vec)
            api.RegisterHex("qwe", typeof(Ceil)); // Ceiling Purification(num| vec → num | vec)
            api.RegisterHex("eqqq", typeof(Rand)); // Entropy Reflection(→ num)

            api.RegisterHex("qqqqqaa", typeof(Sin)); // Sine Purification (num → num)
            api.RegisterHex("qqqqqad", typeof(Cos)); // Cosine Purification (num → num)
            api.RegisterHex("wqqqqqadq", typeof(Tan)); // Tangent Purification (num → num)
            api.RegisterHex("ddeeeee", typeof(Arcsin)); // Inverse Sine Prfn. (num → num)
            api.RegisterHex("adeeeee", typeof(Arccos)); // Inverse Cosine Prfn. (num → num)
            api.RegisterHex("eadeeeeew", typeof(ArcTan)); // Inverse Tangent Prfn. (num → num)
            api.RegisterHex("deadeeeeewd", typeof(ArcTan2)); // Inverse Tan. Prfn. II (num, num → num)
            api.RegisterHex("eqaqe", typeof(Log)); // Logarithmic Distillation (num, num → num)
            api.RegisterHex("addwaad", typeof(Mod)); // Modulus Distillation (num|vec, num|vec → num|vec)

            //Vecs

            api.RegisterHex("eqqqqq", typeof(Pack)); // Vector Exaltation (num, num, num → vector)
            api.RegisterHex("qeeeee", typeof(UnPack)); // Vector Disintegration (vector → num, num, num)
            // Axial Purification (vec|num → vec|num)

            //Lists

            api.RegisterHex("deeed", typeof(Index)); // Selection Distillation (list, number → any)
            api.RegisterHex("qaeaqwded", typeof(Slice)); // Selection Exaltation (list, num, num → list)
            api.RegisterHex("edqde", typeof(Append)); // Integration Distillation (list, any → list)
            api.RegisterHex("qaeaq", typeof(UnAppend)); // Derivation Decomposition (list → list, any)
            api.RegisterHex("qqqaede", typeof(Rev)); // Retrograde Purification (list → list)
            api.RegisterHex("dedqde", typeof(Index_Of)); // Locator's Distillation (list, any → num)
            api.RegisterHex("edqdewaqa", typeof(Remove)); // Excisor's Distillation (list, num → list)
            api.RegisterHex("wqaeaqw", typeof(Replace)); // Surgeon's Exaltation (list, num, any → list)
            api.RegisterHex("ddewedd", typeof(Cons)); // Flock's Gambit (many, num → list)
            api.RegisterHex("aaqwqaa", typeof(UnCons)); // Flock's Disintegration (list → many)
            api.RegisterHex("qqaeaae", typeof(EMPTY_LIST)); // Vacant Reflection (→ list)
            // Single's Purification (any → list)
            // Speaker's Distillation (list, any → list)
            // Speaker's Decomposition (list → list, any)

            //Boolean Logic, Comparisons & Sets

            api.RegisterHex("wdw", typeof(And)); // Conjunction Distillation (bool, bool → bool)
            api.RegisterHex("waw", typeof(Or)); // Disjunction Distillation (bool, bool → bool)
            api.RegisterHex("dwa", typeof(Xor)); // Exclusion Distillation (bool, bool → bool)
            api.RegisterHex("e", typeof(Greater)); // Maximus Distillation (number, number → bool)
            api.RegisterHex("q", typeof(Less)); // Minimus Distillation (number, number → bool)
            api.RegisterHex("ee", typeof(Greater_Eq)); // Maximus Distillation II (number, number → bool)
            api.RegisterHex("qq", typeof(Less_Eq)); // Minimus Distillation II (number, number → bool)
            api.RegisterHex("ad", typeof(Equals)); // Equality Distillation (any, any → bool)
            api.RegisterHex("da", typeof(Not_Equal)); // Inequality Distillation (any, any → bool)
            api.RegisterHex("dw", typeof(Not)); // Negation Purification (bool → bool)
            api.RegisterHex("aw", typeof(Bool_Coerce)); // Augur's Purification (any → bool)
            api.RegisterHex("awdd", typeof(If)); // Augur's Exaltation (bool, any, any → any)
            
            api.RegisterHex("aweaqa", typeof(Unique)); // Uniqueness Purification (list → list)
            // Disjunction Distillation ((num, num)|(list, list) → num|list)
            // Conjunction Distillation ((num, num)|(list, list) → num|list)
            // Exclusion Distillation ((num, num)|(list, list) → num|list)
            // Negation Purification (num → num)

            // Meta-Eval

            api.RegisterHex("deaqq", typeof(Eval)); // Hermes' Gambit ([pattern] | pattern → many)
            // Iris' Gambit ([pattern] | pattern → many)
            api.RegisterHex("dadad", typeof(FOR_EACH)); // Thoth's Gambit (list of patterns, list → list)
            // Charon's Gambit
            // Thanatos' Reflection (→ number)

            // Read and Write

            api.RegisterHex("aqqqqq", typeof(Read));
            api.RegisterHex("deeeee", typeof(Write));

            // Basics

            api.RegisterHex("qaq", typeof(Get_caster));// Mind's Reflection (→ entity | null)
            api.RegisterHex("aa", typeof(Entity_PosEye));// Compass' Purification (entity → vector)
            api.RegisterHex("dd", typeof(Entity_PosFoot));// Compass' Purification II (entity → vector)
            api.RegisterHex("wa", typeof(Entity_Look));// Alidade's Purification (entity → vector)
            api.RegisterHex("wqaawdd", typeof(RayCast));// Archer's Distillation (vector, vector → vector | null)
            api.RegisterHex("weddwaa", typeof(RayCast_Axis));// Architect's Distillation (vector, vector → vector | null)
            api.RegisterHex("weaqa", typeof(RayCast_Entity));// Scout's Distillation (vector, vector → entity | null)
            api.RegisterHex("de", typeof(Print)); // Reveal (any → any)
            api.RegisterHex("awq", typeof(Entity_Height));// Stadiometer's Prfn. (entity → num)
            api.RegisterHex("wq", typeof(Entity_Velocity));// Pace Purification (entity → vector)

            // Constants

            api.RegisterHex("aqae", typeof(Const_True));// True Reflection (→ bool)
            api.RegisterHex("dedq", typeof(Const_False));// False Reflection (→ bool)
            api.RegisterHex("d", typeof(Const_Null));// Nullary Reflection (→ null)
            api.RegisterHex("qqqqq", typeof(Const_Vec_Zero));// Vector Reflection Zero (→ vector)
            api.RegisterHex("qqqqqea", typeof(Const_Vec_X_Pos));// Vector Rfln. +X/-X (→ vector)
            api.RegisterHex("eeeeeqa", typeof(Const_Vec_X_Neg));
            api.RegisterHex("qqqqqew", typeof(Const_Vec_Y_Pos));// Vector Rfln. +Y/-Y (→ vector)
            api.RegisterHex("eeeeeqw", typeof(Const_Vec_Y_Neg));
            api.RegisterHex("qqqqqed", typeof(Const_Vec_Z_Pos));// Vector Rfln. +Z/-Z (→ vector)
            api.RegisterHex("eeeeeqd", typeof(Const_Vec_Z_Neg));
            api.RegisterHex("eawae", typeof(Const_True));// Circle's Reflection (→ num)
            api.RegisterHex("qdwdq", typeof(Const_True));// Arc's Reflection (→ num)
            api.RegisterHex("aaq", typeof(Const_True));// Euler's Reflection (→ num)
            
            // Ellement Shift

            api.RegisterHex("qqqqqwaeaeaeaeaea", typeof(Const_Sol));
            api.RegisterHex("qqqae", typeof(Const_Lun));
            api.RegisterHex("qeaqwqad", typeof(Const_Cal));
            api.RegisterHex("wqqqqw", typeof(Const_Oce));
            api.RegisterHex("wdaq", typeof(Const_Ste));
            api.RegisterHex("qawa", typeof(Const_Tyr));

            // Stack Manipulation

            api.RegisterHex("aawdd", typeof(SWAP));// Jester's Gambit (any, any → any, any)
            api.RegisterHex("aaeaa", typeof(ROTATE));// Rotation Gambit (any, any, any → any, any, any)
            api.RegisterHex("ddqdd", typeof(ROTATE_REVERSE));// Rotation Gambit II (any, any, any → any, any, any)
            api.RegisterHex("aadaa", typeof(DUPLICATE));// Gemini Decomposition (any → any, any)
            api.RegisterHex("aaedd", typeof(OVER));// Prospector's Gambit (any, any → any, any, any)
            api.RegisterHex("ddqaa", typeof(TUCK));// Undertaker's Gambit (any, any → any, any, any)
            api.RegisterHex("aadadaaw", typeof(TWO_DUP));// Gemini Gambit (any, number → many)
            api.RegisterHex("qwaeawqaeaqa", typeof(STACK_LEN));// Dioscuri Gambit (any, any → any, any, any, any)
            api.RegisterHex("aadaadaa", typeof(DUPLICATE_N));// Flock's Reflection (→ number)
            api.RegisterHex("ddad", typeof(FISHERMAN));// Fisherman's Gambit (number → any)
            api.RegisterHex("aada", typeof(FISHERMAN_COPY));// Fisherman's Gambit II (number → any)
            // Swindler's Gambit (many, number → many)

            // Basic Spells

            api.RegisterHex("aaqawawa", typeof(Effect));// Effect (entity | vector →)
            api.RegisterHex("aqe", typeof(Project));// Project (→)
            api.RegisterHex("aawaawaa", typeof(Explode));// Explode (vector →)
            api.RegisterHex("qaqqqqq", typeof(Place));// Place (vector →)

            // Nadir (entity →)
            // Zenith  (entity →)

            // Enchant  (Item →)

            // Extra Spells 

            //api.RegisterHex("aaqawawa", typeof(Ignite));// Ignite (entity | vector →)
        }
        //Doubles
        public class Add : OperatorBase
        {
            public Add(Entity player, ICoreAPI api, State Current) : base(player,api, Current)
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
                    castResult = new CastResult(
                        new ListIota(new List<Iota>() { a, b }),
                        Current,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                    return;
                }
                castResult = new CastResult(
                        new ListIota(new List<Iota>() { a, b }),
                        Current,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
            }
        }
        public class Sub : OperatorBase
        {
            public Sub(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
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
                    castResult = new CastResult(
                        new ListIota(new List<Iota>() { a, b }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                    return;
                }
                castResult = new CastResult(
                        new ListIota(new List<Iota>() { a, b }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
            }
        }
        public class Mul : OperatorBase
        {
            public Mul(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {
                Iota a = stack.Pop();
                Iota b = stack.Pop();
                if ((a is null || b is null) || (a is not DoubleIota && b is not DoubleIota) && (a is not Vec3Iota && b is not Vec3Iota))
                {
                    castResult = new CastResult(
                        new ListIota(new List<Iota>() { a, b }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                    return;
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
                castResult = new CastResult(
                        new ListIota(new List<Iota>() { a, b }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
            }
        }
        public class Div : OperatorBase
        {
            public Div(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
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
                    castResult = new CastResult(
                        new ListIota(new List<Iota>() { a, b }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                    return;
                }
                    castResult = new CastResult(
                        new ListIota(new List<Iota>() { a, b }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                    result.getSound()*/);
            }
        }
        public class Abs : OperatorBase
        {
            public Abs(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
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
                    castResult = new CastResult(
                    new ListIota(new List<Iota>() { a }),
                    stack,
                    ResolvedPatternType.ERRORED/*,
                    result.getSound()*/);
                    return;
                }
                castResult = new CastResult(
                        new ListIota(new List<Iota>() { a }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
            }
        }
        public class Pow : OperatorBase
        {
            public Pow(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
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
                    castResult = new CastResult(
                        new ListIota(new List<Iota>() { a, b }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                    return;
                }
                castResult = new CastResult(
                    new ListIota(new List<Iota>() { a, b }),
                    stack,
                    ResolvedPatternType.EVALUATED/*,
                    result.getSound()*/);
            }
        }
        public class Floor : OperatorBase
        {
            public Floor(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
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
                    castResult = new CastResult(
                    new ListIota(new List<Iota>() { a }),
                    stack,
                    ResolvedPatternType.ERRORED/*,
                    result.getSound()*/);
                    return;
                }
                
                castResult = new CastResult(
                new ListIota(new List<Iota>() { a }),
                stack,
                ResolvedPatternType.EVALUATED/*,
                result.getSound()*/);
            }
        }
        public class Ceil : OperatorBase
        {
            public Ceil(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
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
                    castResult = new CastResult(
                    new ListIota(new List<Iota>() { a }),
                    stack,
                    ResolvedPatternType.ERRORED/*,
                    result.getSound()*/);
                    return;
                }

                castResult = new CastResult(
                new ListIota(new List<Iota>() { a }),
                stack,
                ResolvedPatternType.EVALUATED/*,
                result.getSound()*/);
            }
        }
        public class Sin : OperatorBase
        {
            public Sin(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {
                Iota a = stack.Pop();
                if (a is not DoubleIota)
                {
                    castResult = new CastResult(
                    new ListIota(new List<Iota>() { a }),
                    stack,
                    ResolvedPatternType.ERRORED/*,
                    result.getSound()*/);
                    return;
                }
                stack.Push(new DoubleIota(Math.Sin(((DoubleIota)a).getDouble())));
                castResult = new CastResult(
                new ListIota(new List<Iota>() { a }),
                stack,
                ResolvedPatternType.EVALUATED/*,
                result.getSound()*/);
            }
        }
        public class Cos : OperatorBase
        {
            public Cos(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {
                Iota a = stack.Pop();
                if (a is not DoubleIota)
                {
                    castResult = new CastResult(
                    new ListIota(new List<Iota>() { a }),
                    stack,
                    ResolvedPatternType.ERRORED/*,
                    result.getSound()*/);
                    return;
                }
                stack.Push(new DoubleIota(Math.Cos(((DoubleIota)a).getDouble())));
                castResult = new CastResult(
                new ListIota(new List<Iota>() { a }),
                stack,
                ResolvedPatternType.EVALUATED/*,
                result.getSound()*/);
            }
        }
        public class Tan : OperatorBase
        {
            public Tan(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {
                Iota a = stack.Pop();
                if (a is not DoubleIota)
                {
                    castResult = new CastResult(
                    new ListIota(new List<Iota>() { a }),
                    stack,
                    ResolvedPatternType.ERRORED/*,
                    result.getSound()*/);
                    return;
                }
                stack.Push(new DoubleIota(Math.Tan(((DoubleIota)a).getDouble())));
                castResult = new CastResult(
                new ListIota(new List<Iota>() { a }),
                stack,
                ResolvedPatternType.EVALUATED/*,
                result.getSound()*/);
            }
        }
        public class Arcsin : OperatorBase
        {
            public Arcsin(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {
                Iota a = stack.Pop();
                if (a is not DoubleIota)
                {
                    castResult = new CastResult(
                    new ListIota(new List<Iota>() { a }),
                    stack,
                    ResolvedPatternType.ERRORED/*,
                    result.getSound()*/);
                    return;
                }
                stack.Push(new DoubleIota(Math.Asin(((DoubleIota)a).getDouble())));
                castResult = new CastResult(
                new ListIota(new List<Iota>() { a}),
                stack,
                ResolvedPatternType.EVALUATED/*,
                result.getSound()*/);
            }
        }
        public class Arccos : OperatorBase
        {
            public Arccos(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {
                Iota a = stack.Pop();
                if (a is not DoubleIota)
                {
                    castResult = new CastResult(
                    new ListIota(new List<Iota>() { a }),
                    stack,
                    ResolvedPatternType.ERRORED/*,
                    result.getSound()*/);
                    return;
                }
                stack.Push(new DoubleIota(Math.Acos(((DoubleIota)a).getDouble())));
                castResult = new CastResult(
                new ListIota(new List<Iota>() { a }),
                stack,
                ResolvedPatternType.EVALUATED/*,
                result.getSound()*/);
            }
        }
        public class ArcTan : OperatorBase
        {
            public ArcTan(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {
                Iota a = stack.Pop();
                if (a is not DoubleIota)
                {
                    castResult = new CastResult(
                    new ListIota(new List<Iota>() { a }),
                    stack,
                    ResolvedPatternType.ERRORED/*,
                    result.getSound()*/);
                    return;
                }
                stack.Push(new DoubleIota(Math.Atan(((DoubleIota)a).getDouble())));
                castResult = new CastResult(
                new ListIota(new List<Iota>() { a }),
                stack,
                ResolvedPatternType.EVALUATED/*,
                result.getSound()*/);
            }
        }
        public class ArcTan2 : OperatorBase
        {
            public ArcTan2(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {
                Iota a = stack.Pop();
                Iota b = stack.Pop();
                if (a is not DoubleIota || b is not DoubleIota)
                {
                    castResult = new CastResult(
                    new ListIota(new List<Iota>() { a, b }),
                    stack,
                    ResolvedPatternType.ERRORED/*,
                    result.getSound()*/);
                    return;
                }

                stack.Push(new DoubleIota(Math.Atan2(((DoubleIota)b).getDouble(), ((DoubleIota)a).getDouble())));
                castResult = new CastResult(
                new ListIota(new List<Iota>() { a, b }),
                stack,
                ResolvedPatternType.EVALUATED/*,
                result.getSound()*/);
            }
        }
        public class Log : OperatorBase
        {
            public Log(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {
                Iota a = stack.Pop();
                Iota b = stack.Pop();
                if (a is not DoubleIota || b is not DoubleIota)
                {
                    castResult = new CastResult(
                    new ListIota(new List<Iota>() { a, b }),
                    stack,
                    ResolvedPatternType.ERRORED/*,
                    result.getSound()*/);
                    return;
                }

                stack.Push(new DoubleIota(Math.Log(((DoubleIota)b).getDouble(), ((DoubleIota)a).getDouble())));
                castResult = new CastResult(
                new ListIota(new List<Iota>() { a, b }),
                stack,
                ResolvedPatternType.EVALUATED/*,
                result.getSound()*/);
            }
        }
        public class Mod : OperatorBase
        {
            public Mod(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
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
                    castResult = new CastResult(
                    new ListIota(new List<Iota>() { a, b }),
                    stack,
                    ResolvedPatternType.ERRORED/*,
                    result.getSound()*/);
                    return;
                }

                
                castResult = new CastResult(
                    new ListIota(new List<Iota>() { a, b }),
                    stack,
                    ResolvedPatternType.EVALUATED/*,
                    result.getSound()*/);
            }
        }

        //Vecs

        public class Pack : OperatorBase
        {
            public Pack(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {
                Iota a = stack.Pop();
                Iota b = stack.Pop();
                Iota c = stack.Pop();
                if (a is not DoubleIota || b is not DoubleIota || c is not DoubleIota)
                {
                    castResult = new CastResult(
                    new ListIota(new List<Iota>() {a ,b ,c}),
                    stack,
                    ResolvedPatternType.ERRORED/*,
                    result.getSound()*/);
                    return;
                }

                stack.Push(new Vec3Iota(((DoubleIota)c).getDouble(), ((DoubleIota)b).getDouble() , ((DoubleIota)a).getDouble()));
                castResult = new CastResult(
                    new ListIota(new List<Iota>() { a, b, c }),
                    stack,
                    ResolvedPatternType.EVALUATED/*,
                    result.getSound()*/);
            }
        }
        public class UnPack : OperatorBase
        {
            public UnPack(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {
                Iota a = stack.Pop();
                if (a is not DoubleIota)
                {
                    castResult = new CastResult(
                        new ListIota(new List<Iota>() { a }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                        return;
                }

                Vec3d v = ((Vec3Iota)a).getVec3();

                stack.Push(new DoubleIota(v.X));
                stack.Push(new DoubleIota(v.Y));
                stack.Push(new DoubleIota(v.Z));
                castResult = new CastResult(
                new ListIota(new List<Iota>() { a }),
                stack,
                ResolvedPatternType.EVALUATED/*,
                result.getSound()*/);
            }
        }

        //Lists

        public class Index : OperatorBase
        {
            public Index(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {
                Iota num = stack.Pop();
                Iota list = stack.Pop();

                if (list is not ListIota || num is not DoubleIota)
                {
                    castResult = new CastResult(
                    new ListIota(new List<Iota>() { list , num }),
                    stack,
                    ResolvedPatternType.ERRORED/*,
                    result.getSound()*/);
                    return;
                }

                stack.Push(((ListIota)list).getList()[(int)((DoubleIota)num).getDouble()]);

                castResult = new CastResult(
                    new ListIota(new List<Iota>() { list, num }),
                    stack,
                    ResolvedPatternType.EVALUATED/*,
                    result.getSound()*/);
            }
        }
        public class Slice : OperatorBase
        {
            public Slice(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {
                Iota num2 = stack.Pop();
                Iota num1 = stack.Pop();
                Iota list = stack.Pop();

                if (list is not ListIota || num1 is not DoubleIota || num2 is not DoubleIota)
                {
                    castResult = new CastResult(
                        new ListIota(new List<Iota>() { list, num1, num2 }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                    return;
                }

                stack.Push(new ListIota(((ListIota)list).getList().GetRange((int)((DoubleIota)num1).getDouble(), (int)((DoubleIota)num1).getDouble() +(int)((DoubleIota)num1).getDouble())));

                castResult = new CastResult(
                    new ListIota(new List<Iota>() { list, num1, num2 }),
                    stack,
                    ResolvedPatternType.EVALUATED/*,
                    result.getSound()*/);
            }
        }
        public class Append : OperatorBase
        {
            public Append(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {
                Iota any = stack.Pop();
                Iota list = stack.Pop();

                if (list is not ListIota)
                {
                    castResult = new CastResult(
                        new ListIota(new List<Iota>() { list, any }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                    return;
                }

                ((ListIota)list).getList().Add(any);

                stack.Push(list);

                castResult = new CastResult(
                    new ListIota(new List<Iota>() { list, any }),
                    stack,
                    ResolvedPatternType.EVALUATED/*,
                    result.getSound()*/);
            }
        }
        public class UnAppend : OperatorBase
        {
            public UnAppend(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {
                Iota list2 = stack.Pop();
                Iota list1 = stack.Pop();

                if (list1 is not ListIota || list2 is not ListIota)
                {
                    castResult = new CastResult(
                        new ListIota(new List<Iota>() { list1, list2 }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                    return;
                }

                ((ListIota)list1).getList().AddRange(((ListIota)list2).getList());

                stack.Push(list1);

                castResult = new CastResult(
                    new ListIota(new List<Iota>() { list1, list2 }),
                    stack,
                    ResolvedPatternType.EVALUATED/*,
                    result.getSound()*/);
            }
        }
        public class Rev : OperatorBase
        {
            public Rev(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {
                Iota list = stack.Pop();

                if (list is not ListIota)
                {
                    castResult = new CastResult(
                    new ListIota(new List<Iota>() { list }),
                    stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                    return;
                }

                ((ListIota)list).getList().Reverse();

                stack.Push(list);

                castResult = new CastResult(
                    new ListIota(new List<Iota>() { list }),
                    stack,
                    ResolvedPatternType.EVALUATED/*,
                    result.getSound()*/);
            }
        }
        public class Index_Of : OperatorBase
        {
            public Index_Of(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {
                Iota any = stack.Pop();
                Iota list = stack.Pop();

                if (list is not ListIota)
                {
                    castResult = new CastResult(
                        new ListIota(new List<Iota>() { list, any }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                    return;
                }

                stack.Push(new DoubleIota(((ListIota)list).getList().IndexOf(any)));

                castResult = new CastResult(
                    new ListIota(new List<Iota>() { list, any }),
                    stack,
                    ResolvedPatternType.EVALUATED/*,
                    result.getSound()*/);
            }
        }
        public class Remove : OperatorBase
        {
            public Remove(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {
                Iota num = stack.Pop();
                Iota list = stack.Pop();

                if (list is not ListIota && num is not DoubleIota)
                {
                    castResult = new CastResult(
                        new ListIota(new List<Iota>() { list, num }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                    return;
                }

                ((ListIota)list).getList().RemoveAt((int)((DoubleIota)num).getDouble());

                stack.Push(list);

                castResult = new CastResult(
                    new ListIota(new List<Iota>() { list, num }),
                    stack,
                    ResolvedPatternType.EVALUATED/*,
                    result.getSound()*/);
            }
        }
        public class Replace : OperatorBase
        {
            public Replace(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {
                Iota any = stack.Pop();
                Iota num = stack.Pop();
                Iota list = stack.Pop();

                if (list is not ListIota && num is not DoubleIota)
                {
                    castResult = new CastResult(
                        new ListIota(new List<Iota>() { list, num, any }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                    return;
                }

                ((ListIota)list).getList()[((int)((DoubleIota)num).getDouble())] = any;

                stack.Push(list);

                castResult = new CastResult(
                    new ListIota(new List<Iota>() { list, num, any }),
                    stack,
                    ResolvedPatternType.EVALUATED/*,
                    result.getSound()*/);
            }
        }
        public class Cons : OperatorBase
        {
            public Cons(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {
                Iota num = stack.Pop();
                if (num is not DoubleIota || ((int)((DoubleIota)num).getDouble())>= stack.stack.Count)
                {
                    castResult = new CastResult(
                        new ListIota(new List<Iota>() { num }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                    return;
                }

                ListIota acc = new ListIota(new List<Iota>());

                for (int i = 0; 1 < ((int)((DoubleIota)num).getDouble()); i++)
                {
                    acc.getList().Add(stack.Pop());
                }

                stack.Push(acc);

                castResult = new CastResult(
                    new ListIota(new List<Iota>() { num , acc }),
                    stack,
                    ResolvedPatternType.EVALUATED/*,
                    result.getSound()*/);
            }
        }
        public class UnCons : OperatorBase
        {
            public UnCons(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {
                Iota list = stack.Pop();
                if (list is not ListIota )
                {
                    castResult = new CastResult(
                        new ListIota(new List<Iota>() { list }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                    return;
                }

                foreach (Iota I in ((ListIota)list).getList())
                {
                    stack.Push(I);
                }

                castResult = new CastResult(
                    new ListIota(new List<Iota>() { list }),
                    stack,
                    ResolvedPatternType.EVALUATED/*,
                    result.getSound()*/);
            }
        }
        public class EMPTY_LIST : OperatorBase
        {
            public EMPTY_LIST(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {
                stack.Push(new ListIota());
                castResult = new CastResult(
                    new ListIota(new List<Iota>() { }),
                    stack,
                    ResolvedPatternType.EVALUATED/*,
                    result.getSound()*/);
            }
        }


        //Boolian Logic,Comparisons, & Sets

        public class And : OperatorBase
        {
            public And(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {
                Iota a = stack.Pop();
                Iota b = stack.Pop();
                if (a is BoolIota && b is BoolIota)
                {
                    stack.Push(new BoolIota(((BoolIota)a).getBool() && ((BoolIota)b).getBool()));

                    castResult = new CastResult(
                        new ListIota(new List<Iota>() { a, b }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
                }
                else
                {
                    castResult = new CastResult(
                        new ListIota(new List<Iota>() { a, b }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                    return;
                }
            }
        }

        public class Or : OperatorBase
        {
            public Or(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {
                Iota a = stack.Pop();
                Iota b = stack.Pop();
                if (a is BoolIota && b is BoolIota)
                {
                    stack.Push(new BoolIota(((BoolIota)a).getBool() || ((BoolIota)b).getBool()));

                    castResult = new CastResult(
                        new ListIota(new List<Iota>() { a, b }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
                }
                else
                {
                    castResult = new CastResult(
                        new ListIota(new List<Iota>() { a, b }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                    return;
                }
            }
        }

        public class Xor : OperatorBase
        {
            public Xor(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {
                Iota a = stack.Pop();
                Iota b = stack.Pop();
                if (a is BoolIota && b is BoolIota)
                {
                    stack.Push(new BoolIota(((BoolIota)a).getBool() ^ ((BoolIota)b).getBool()));

                    castResult = new CastResult(
                        new ListIota(new List<Iota>() { a, b }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
                }
                else
                {
                    castResult = new CastResult(
                        new ListIota(new List<Iota>() { a, b }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                    return;
                }
            }
        }

        public class Greater : OperatorBase
        {
            public Greater(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {
                Iota a = stack.Pop();
                Iota b = stack.Pop();
                if (a is DoubleIota && b is DoubleIota)
                {
                    stack.Push(new BoolIota(((DoubleIota)b).getDouble() > ((DoubleIota)a).getDouble()));

                    castResult = new CastResult(
                        new ListIota(new List<Iota>() { a, b }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
                }
                else
                {
                    castResult = new CastResult(
                        new ListIota(new List<Iota>() { a, b }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                    return;
                }
            }
        }

        public class Less : OperatorBase
        {
            public Less(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {
                Iota a = stack.Pop();
                Iota b = stack.Pop();
                if (a is DoubleIota && b is DoubleIota)
                {
                    stack.Push(new BoolIota(((DoubleIota)b).getDouble() < ((DoubleIota)a).getDouble()));

                    castResult = new CastResult(
                        new ListIota(new List<Iota>() { a, b }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
                }
                else
                {
                    castResult = new CastResult(
                        new ListIota(new List<Iota>() { a, b }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                    return;
                }
            }
        }

        public class Greater_Eq : OperatorBase
        {
            public Greater_Eq(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {
                Iota a = stack.Pop();
                Iota b = stack.Pop();
                if (a is DoubleIota && b is DoubleIota)
                {
                    stack.Push(new BoolIota(((DoubleIota)b).getDouble() >= ((DoubleIota)a).getDouble()));

                    castResult = new CastResult(
                        new ListIota(new List<Iota>() { a, b }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
                }
                else
                {
                    castResult = new CastResult(
                        new ListIota(new List<Iota>() { a, b }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                    return;
                }
            }
        }

        public class Less_Eq : OperatorBase
        {
            public Less_Eq(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {
                Iota a = stack.Pop();
                Iota b = stack.Pop();
                if (a is DoubleIota && b is DoubleIota)
                {
                    stack.Push(new BoolIota(((DoubleIota)b).getDouble() <= ((DoubleIota)a).getDouble()));

                    castResult = new CastResult(
                        new ListIota(new List<Iota>() { a, b }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
                }
                else
                {
                    castResult = new CastResult(
                        new ListIota(new List<Iota>() { a, b }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                    return;
                }
            }
        }
        public class Equals : OperatorBase
        {
            public Equals(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {
                Iota a = stack.Pop();
                Iota b = stack.Pop();
                stack.Push(new BoolIota((b) == (a)));

                castResult = new CastResult(
                    new ListIota(new List<Iota>() { a, b }),
                    stack,
                    ResolvedPatternType.EVALUATED/*,
                    result.getSound()*/);
            }
        }
        public class Not_Equal : OperatorBase
        {
            public Not_Equal(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {
                Iota a = stack.Pop();
                Iota b = stack.Pop();
                
                stack.Push(new BoolIota((b) != (a)));

                castResult = new CastResult(
                    new ListIota(new List<Iota>() { a, b }),
                    stack,
                    ResolvedPatternType.EVALUATED/*,
                    result.getSound()*/);
                
            }
        }

        public class Not : OperatorBase
        {
            public Not(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {
                Iota any = stack.Pop();

                stack.Push(new BoolIota(!any.isTruthy()));

                castResult = new CastResult(
                    new ListIota(new List<Iota>() { any }),
                    stack,
                    ResolvedPatternType.EVALUATED/*,
                    result.getSound()*/);
            }
        }
        public class Bool_Coerce : OperatorBase
        {
            public Bool_Coerce(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {
                Iota any = stack.Pop();

                stack.Push(new BoolIota(any.isTruthy()));

                castResult = new CastResult(
                    new ListIota(new List<Iota>() { any }),
                    stack,
                    ResolvedPatternType.EVALUATED/*,
                    result.getSound()*/);
            }
        }
        public class If : OperatorBase
        {
            public If(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {
                Iota any2 = stack.Pop();
                Iota any1 = stack.Pop();
                Iota Bool = stack.Pop();

                if (Bool is BoolIota )
                {
                    stack.Push(((BoolIota)Bool).getBool() ? any1 : any2);

                    castResult = new CastResult(
                        new ListIota(new List<Iota>() { Bool, any1, any2 }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
                }
                else
                {
                    castResult = new CastResult(
                        new ListIota(new List<Iota>() { Bool, any1, any2 }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                    return;
                }
            }
        }
        public class Rand : OperatorBase
        {
            public Rand(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {

                stack.Push(new DoubleIota(Math.Round(api.World.Rand.Next(1000)/1000f,3)));

                castResult = new CastResult(
                    new ListIota(new List<Iota>() {  }),
                    stack,
                    ResolvedPatternType.EVALUATED/*,
                    result.getSound()*/);
                
            }
        }

        public class Unique : OperatorBase
        {
            public Unique(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
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

                castResult = new CastResult(
                    new ListIota(new List<Iota>() { list }),
                    stack,
                    ResolvedPatternType.EVALUATED/*,
                    result.getSound()*/);
            }
        }


        // Meta-Evaluation

        public class Eval : OperatorBase
        {
            public Eval(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {
                bool first = stack == 0;
                stack.stack.TryPop(out Iota list);
                if (list is not ListIota)
                {
                    castResult = new CastResult(
                        new ListIota(new List<Iota>() { list }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                    return;
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
                        castResult = new CastResult(
                            new ListIota(new List<Iota>() { list }),
                            stack,
                            ResolvedPatternType.ERRORED/*,
                            result.getSound()*/);
                        return;
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
                            castResult = pattern.execute(player, api, stack);
                            if (!castResult.resolutionType.success)
                            {
                                if (first)
                                {
                                    ((IServerPlayer)((EntityPlayer)player)?.Player)?.SendMessage(0, castResult.cast.ToString(), EnumChatType.CommandError);
                                }
                                return;
                            }
                        }
                    }
                }
                if (click || listing > 0)
                {
                    castResult = new CastResult(
                        new ListIota(new List<Iota>() { list }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                    return;
                }


                castResult = new CastResult(
                    new ListIota(new List<Iota>() { list }),
                    stack,
                    ResolvedPatternType.EVALUATED/*,
                    result.getSound()*/);


            }
        }
        public class FOR_EACH : OperatorBase
        {
            public FOR_EACH(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {
                stack.stack.TryPop(out Iota list);
                stack.stack.TryPop(out Iota Patterns);
                if (list is not ListIota || Patterns is not ListIota)
                {
                    castResult = new CastResult(
                        new ListIota(new List<Iota>() { list , Patterns }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                    return;
                }

                foreach(Iota i in ((ListIota)list).getList())
                {
                    State inner = new(i);
                    inner.evals = stack.evals;
                    inner.Push(Patterns);
                    Eval n = new Eval(player, api, inner);
                    stack.Push(inner.Open());
                    if (!n.castResult.resolutionType.success)
                    {
                        castResult = n.castResult;
                        
                        return;
                    }
                    stack.evals = inner.evals;
                }

                castResult = new CastResult(
                    new ListIota(new List<Iota>() { list }),
                    stack,
                    ResolvedPatternType.EVALUATED/*,
                    result.getSound()*/);
            }
        }


        //Read and Write

        public class Read : OperatorBase
        {
            public Read(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {
                ItemStack Offhand = ((EntityPlayer)player)?.Player.InventoryManager?.OffhandHotbarSlot?.Itemstack;
                if (Offhand is not null)
                {
                    stack.Push(((Focus)Offhand.Item)?.GetIota(Offhand));
                }
                else
                {
                    castResult = new CastResult(
                        new ListIota(new List<Iota>() { }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                    return;
                }
                castResult = new CastResult(
                        new ListIota(new List<Iota>() { }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
            }
        }
        public class Write : OperatorBase
        {
            public Write(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {
                Iota Data = stack.Pop();
                if (Data is not null)
                {
                    ItemStack Offhand = ((EntityPlayer)player)?.Player.InventoryManager?.OffhandHotbarSlot?.Itemstack;
                    ((Focus)Offhand.Item)?.SetIota(Data, Offhand);
                }
                else
                {
                    castResult = new CastResult(
                        new ListIota(new List<Iota>() { Data }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                    return;
                }
                castResult = new CastResult(
                        new ListIota(new List<Iota>() { Data }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
            }
        }

        // Basics
        public class Get_caster : OperatorBase
        {
            public Get_caster(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {
                stack.Push(new EntityIota(player));
                castResult = new CastResult(
                    new ListIota(new List<Iota>() { }),
                    stack,
                    ResolvedPatternType.EVALUATED/*,
                    result.getSound()*/);
            }
        }
        public class Entity_PosEye : OperatorBase
        {
            public Entity_PosEye(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {
                Iota e = stack.Pop();
                if (e is EntityIota)
                {
                    stack.Push(new Vec3Iota(((EntityIota)e).getEntity().LocalEyePos + ((EntityIota)e).getEntity().SidedPos.XYZ));
                }
                else
                {
                    castResult = new CastResult(
                        new ListIota(new List<Iota>() { e }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                    return;
                }
                castResult = new CastResult(
                        new ListIota(new List<Iota>() { e }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
            }
        }
        public class Entity_PosFoot : OperatorBase
        {
            public Entity_PosFoot(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {
                Iota e = stack.Pop();
                if (e is EntityIota)
                {
                    stack.Push(new Vec3Iota(((EntityIota)e).getEntity().SidedPos.XYZ));
                }
                else
                {
                    castResult = new CastResult(
                        new ListIota(new List<Iota>() { e }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                    return;
                }
                castResult = new CastResult(
                        new ListIota(new List<Iota>() { e }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
            }
        }
        
        public class Entity_Look : OperatorBase
        {
            public Entity_Look(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {
                Iota e = stack.Pop();
                if (e is EntityIota)
                {
                    float cosPitch = GameMath.Cos(((EntityIota)e).getEntity().SidedPos.Pitch );
                    float sinPitch = GameMath.Sin(((EntityIota)e).getEntity().SidedPos.Pitch );

                    float cosYaw = GameMath.Cos(((EntityIota)e).getEntity().SidedPos.Yaw );
                    float sinYaw = GameMath.Sin(((EntityIota)e).getEntity().SidedPos.Yaw );

                    stack.Push(new Vec3Iota(new Vec3d(-cosPitch * sinYaw, sinPitch, -cosPitch * cosYaw)));
                }
                else
                {
                    castResult = new CastResult(
                        new ListIota(new List<Iota>() { e }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                    return;
                }
                castResult = new CastResult(
                        new ListIota(new List<Iota>() { e }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
            }
        }
        

        public class RayCast : OperatorBase
        {
            public RayCast(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
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
                    castResult = new CastResult(
                        new ListIota(new List<Iota>() { Start, Dir }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                    return;
                }
                castResult = new CastResult(
                        new ListIota(new List<Iota>() { Start, Dir }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
            }
        }
        public class RayCast_Axis : OperatorBase
        {
            public RayCast_Axis(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
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
                    castResult = new CastResult(
                        new ListIota(new List<Iota>() { Start, Dir }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                    return;
                }
                castResult = new CastResult(
                        new ListIota(new List<Iota>() { Start, Dir }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
            }
        }
        public class RayCast_Entity : OperatorBase
        {
            public RayCast_Entity(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
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
                    castResult = new CastResult(
                        new ListIota(new List<Iota>() { Start, Dir }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                    return;
                }
                castResult = new CastResult(
                        new ListIota(new List<Iota>() { Start, Dir }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
            }
        }


        public class Print : OperatorBase
        {
            public Print(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {
                Iota any = stack.Pop();
                stack.Push(any);
                
                ((IServerPlayer)((EntityPlayer)player)?.Player)?.SendMessage(0,any.ToString(),EnumChatType.OwnMessage);
                castResult = new CastResult(
                    new ListIota(new List<Iota>() { any }),
                    stack,
                    ResolvedPatternType.EVALUATED/*,
                    result.getSound()*/);
            }
        }
        public class Entity_Height : OperatorBase
        {
            public Entity_Height(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {
                Iota e = stack.Pop();
                if (e is EntityIota)
                {
                    stack.Push(new DoubleIota(((EntityIota)e).getEntity().CollisionBox.Height));
                }
                else
                {
                    castResult = new CastResult(
                        new ListIota(new List<Iota>() { e }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                    return;
                }
                castResult = new CastResult(
                        new ListIota(new List<Iota>() { e }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
            }
        }

        
        public class Entity_Velocity : OperatorBase
        {
            public Entity_Velocity(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {
                Iota e = stack.Pop();
                if (e is EntityIota)
                {
                    stack.Push(new Vec3Iota(((EntityIota)e).getEntity().SidedPos.Motion));
                }
                else
                {
                    castResult = new CastResult(
                        new ListIota(new List<Iota>() { e }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                    return;
                }
                castResult = new CastResult(
                        new ListIota(new List<Iota>() { e }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
            }
        }

        // Constants

        // True Reflection (→ bool)
        public class Const_True : OperatorBase
        {
            public Const_True(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {
                
                stack.Push(new BoolIota(true));
                castResult = new CastResult(
                        new ListIota(new List<Iota>() {}),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
            }
        }
        // False Reflection (→ bool)
        public class Const_False : OperatorBase
        {
            public Const_False(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {

                stack.Push(new BoolIota(false));
                castResult = new CastResult(
                        new ListIota(new List<Iota>() { }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
            }
        }
        // Nullary Reflection (→ null)
        public class Const_Null : OperatorBase
        {
            public Const_Null(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {

                stack.Push(new NullIota());
                castResult = new CastResult(
                        new ListIota(new List<Iota>() { }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
            }
        }
        // Vector Reflection Zero (→ vector)
        public class Const_Vec_Zero : OperatorBase
        {
            public Const_Vec_Zero(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {

                stack.Push(new Vec3Iota(0,0,0));
                castResult = new CastResult(
                        new ListIota(new List<Iota>() {}),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
            }
        }
        // Vector Rfln. +X/-X (→ vector)
        public class Const_Vec_X_Pos : OperatorBase
        {
            public Const_Vec_X_Pos(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {

                stack.Push(new Vec3Iota(1, 0, 0));
                castResult = new CastResult(
                        new ListIota(new List<Iota>() { }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
            }
        }
        public class Const_Vec_X_Neg : OperatorBase
        {
            public Const_Vec_X_Neg(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {

                stack.Push(new Vec3Iota(-1, 0, 0));
                castResult = new CastResult(
                        new ListIota(new List<Iota>() { }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
            }
        }
        // Vector Rfln. +Y/-Y (→ vector)
        public class Const_Vec_Y_Pos : OperatorBase
        {
            public Const_Vec_Y_Pos(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {

                stack.Push(new Vec3Iota(0, 1, 0));
                castResult = new CastResult(
                        new ListIota(new List<Iota>() { }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
            }
        }
        public class Const_Vec_Y_Neg : OperatorBase
        {
            public Const_Vec_Y_Neg(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {

                stack.Push(new Vec3Iota(0, -1, 0));
                castResult = new CastResult(
                        new ListIota(new List<Iota>() { }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
            }
        }
        // Vector Rfln. +Z/-Z (→ vector)
        public class Const_Vec_Z_Pos : OperatorBase
        {
            public Const_Vec_Z_Pos(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {

                stack.Push(new Vec3Iota(0, 0, 1));
                castResult = new CastResult(
                        new ListIota(new List<Iota>() { }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
            }
        }
        public class Const_Vec_Z_Neg : OperatorBase
        {
            public Const_Vec_Z_Neg(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {

                stack.Push(new Vec3Iota(0, 0, -1));
                castResult = new CastResult(
                        new ListIota(new List<Iota>()   { }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
            }
        }
        // Circle's Reflection (→ num)
        public class Const_Tau : OperatorBase
        {
            public Const_Tau(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {

                stack.Push(new DoubleIota(Math.Tau));
                castResult = new CastResult(
                        new ListIota(new List<Iota>() { }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
            }
        }
        // Arc's Reflection (→ num)
        public class Const_Pi : OperatorBase
        {
            public Const_Pi(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {

                stack.Push(new DoubleIota(Math.PI));
                castResult = new CastResult(
                        new ListIota(new List<Iota>() { }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
            }
        }
        // Euler's Reflection (→ num)
        public class Const_E : OperatorBase
        {
            public Const_E(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {

                stack.Push(new DoubleIota(Math.E));
                castResult = new CastResult(
                        new ListIota(new List<Iota>() { }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
            }
        }

        // Stack Manipulation

        // Jester's Gambit (any, any → any, any)
        public class SWAP : OperatorBase
        {
            public SWAP(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {
                Iota a = stack.Pop();
                Iota b = stack.Pop();

                if ( a is null || b is null)
                {
                    castResult = new CastResult(
                        new ListIota(new List<Iota>() { a, b }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                    return;
                }

                stack.Push(a);
                stack.Push(b);
                castResult = new CastResult(
                        new ListIota(new List<Iota>() { a, b }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
            }
        }
        // Rotation Gambit (any, any, any → any, any, any)
        public class ROTATE : OperatorBase
        {
            public ROTATE(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {

                Iota a = stack.Pop();
                Iota b = stack.Pop();
                Iota c = stack.Pop();

                if (a is null || b is null || c is null)
                {
                    castResult = new CastResult(
                        new ListIota(new List<Iota>() { a, b, c }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                    return;
                }

                stack.Push(b);
                stack.Push(a);
                stack.Push(c);
                castResult = new CastResult(
                        new ListIota(new List<Iota>() { a, b, c }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);

            }
        }
        // Rotation Gambit II (any, any, any → any, any, any)
        public class ROTATE_REVERSE : OperatorBase
        {
            public ROTATE_REVERSE(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {

                Iota a = stack.Pop();
                Iota b = stack.Pop();
                Iota c = stack.Pop();

                if (a is null || b is null || c is null)
                {
                    castResult = new CastResult(
                        new ListIota(new List<Iota>() { a, b, c }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                    return;
                }

                stack.Push(a);
                stack.Push(c);
                stack.Push(b);
                castResult = new CastResult(
                        new ListIota(new List<Iota>() { a, b, c }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);


            }
        }
        // Gemini Decomposition (any → any, any)
        public class DUPLICATE : OperatorBase
        {
            public DUPLICATE(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {

                Iota a = stack.Pop();

                if (a is null )
                {
                    castResult = new CastResult(
                        new ListIota(new List<Iota>() { a}),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                    return;
                }

                stack.Push(a);
                stack.Push(a);
                castResult = new CastResult(
                        new ListIota(new List<Iota>() { a }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);


            }
        }
        // Prospector's Gambit (any, any → any, any, any)
        public class OVER : OperatorBase
        {
            public OVER(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {

                Iota a = stack.Pop();
                Iota b = stack.Pop();

                if (a is null || b is null)
                {
                    castResult = new CastResult(
                        new ListIota(new List<Iota>() { a, b }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                    return;
                }

                stack.Push(b);
                stack.Push(a);
                stack.Push(b);
                castResult = new CastResult(
                        new ListIota(new List<Iota>() { a, b }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);

            }
        }
        // Undertaker's Gambit (any, any → any, any, any)
        public class TUCK : OperatorBase
        {
            public TUCK(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {

                Iota a = stack.Pop();
                Iota b = stack.Pop();

                if (a is null || b is null)
                {
                    castResult = new CastResult(
                        new ListIota(new List<Iota>() { a, b }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                    return;
                }

                stack.Push(a);
                stack.Push(b);
                stack.Push(a);
                castResult = new CastResult(
                        new ListIota(new List<Iota>() { a, b }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);

            }
        }
        // Gemini Gambit (any, number → many)
        public class DUPLICATE_N : OperatorBase
        {
            public DUPLICATE_N(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {

                Iota num = stack.Pop();
                Iota any = stack.Pop();

                if (num is null || any is null || num is not DoubleIota)
                {
                    castResult = new CastResult(
                        new ListIota(new List<Iota>() { num, any }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                    return;
                }

                for (int i = 0; i < ((DoubleIota)num).getDouble(); i++)
                {
                    stack.Push(any);
                }

                castResult = new CastResult(
                    new ListIota(new List<Iota>() { num, any }),
                    stack,
                    ResolvedPatternType.EVALUATED/*,
                    result.getSound()*/);

            }
        }
        // Dioscuri Gambit (any, any → any, any, any, any)
        public class TWO_DUP : OperatorBase
        {
            public TWO_DUP(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {

                Iota a = stack.Pop();
                Iota b = stack.Pop();

                if (a is null || b is null)
                {
                    castResult = new CastResult(
                        new ListIota(new List<Iota>() { a, b }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                    return;
                }

                stack.Push(b);
                stack.Push(a);
                stack.Push(b);
                stack.Push(a);

                castResult = new CastResult(
                        new ListIota(new List<Iota>() { a, b }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);

            }
        }
        // Flock's Reflection (→ number)
        public class STACK_LEN : OperatorBase
        {
            public STACK_LEN(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {

                stack.Push(new DoubleIota(stack.stack.Count()));
                castResult = new CastResult(
                        new ListIota(new List<Iota>() {  }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
            }
        }
        // Fisherman's Gambit (number → any)
        public class FISHERMAN : OperatorBase
        {
            public FISHERMAN(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {

                Iota num = stack.Pop();

                if (num is null  || num is not DoubleIota || ((DoubleIota)num).getDouble() > stack.stack.Count())
                {
                    castResult = new CastResult(
                        new ListIota(new List<Iota>() { num }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                    return;
                }
                if (((DoubleIota)num).getDouble() == 0)
                {
                    castResult = new CastResult(
                        new ListIota(new List<Iota>() { num }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
                    return;
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

            }
        }
        // Fisherman's Gambit II (number → any)
        public class FISHERMAN_COPY : OperatorBase
        {
            public FISHERMAN_COPY(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {

                Iota num = stack.Pop();

                if (num is null || num is not DoubleIota || ((DoubleIota)num).getDouble() > stack.stack.Count())
                {
                    castResult = new CastResult(
                        new ListIota(new List<Iota>() { num }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                    return;
                }

                if (((DoubleIota)num).getDouble() == 0)
                {
                    castResult = new CastResult(
                        new ListIota(new List<Iota>() { num }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
                    return;
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

            }
        }
        // Elements Constants

        public class Const_Sol : OperatorBase
        {
            public Const_Sol(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {

                stack.Shift(new Solarium(1));
                castResult = new CastResult(
                        new ListIota(new List<Iota>() { }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
            }
        }
        public class Const_Lun : OperatorBase
        {
            public Const_Lun(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {

                stack.Shift(new Lunarium(1));
                castResult = new CastResult(
                        new ListIota(new List<Iota>() { }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
            }
        }
        public class Const_Tyr : OperatorBase
        {
            public Const_Tyr(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {

                stack.Shift(new Tyerraium(1));
                castResult = new CastResult(
                        new ListIota(new List<Iota>() { }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
            }
        }
        public class Const_Cal : OperatorBase
        {
            public Const_Cal(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {

                stack.Shift(new Caelium(1));
                castResult = new CastResult(
                        new ListIota(new List<Iota>() { }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
            }
        }
        public class Const_Oce : OperatorBase
        {
            public Const_Oce(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {

                stack.Shift(new Oceanium(1));
                castResult = new CastResult(
                        new ListIota(new List<Iota>() { }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
            }
        }
        public class Const_Ste : OperatorBase
        {
            public Const_Ste(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {

                stack.Shift(new Stellarium(1));
                castResult = new CastResult(
                        new ListIota(new List<Iota>() { }),
                        stack,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
            }
        }

        //Base Spells
        public class Effect : OperatorBase
        {
            public Effect(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
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
                    castResult = new CastResult(
                        new ListIota(new List<Iota>() { e }),
                        stack,
                        ResolvedPatternType.EVALUATED);
                }
                else
                {
                    castResult = new CastResult(
                        new ListIota(new List<Iota>() { e }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                }
            }
        }

        public class Explode : OperatorBase
        {
            public Explode(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {
                Iota e = stack.Pop();

                bool suc = false;

                if (e is Vec3Iota vec)
                {
                    suc = stack.Shift().Explode(player, vec.getVec3());
                }
                if (suc)
                {
                    castResult = new CastResult(
                        new ListIota(new List<Iota>() { e }),
                        stack,
                        ResolvedPatternType.EVALUATED);
                }
                else
                {
                    castResult = new CastResult(
                        new ListIota(new List<Iota>() { e }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                }
            }
        }
        public class Place : OperatorBase
        {
            public Place(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
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
                    player.UseMedia(10 * ((float)stack.exp));
                    stack.inc();
                    castResult = new CastResult(
                        new ListIota(new List<Iota>() { e }),
                        stack,
                        ResolvedPatternType.EVALUATED);
                }
                else
                {
                    castResult = new CastResult(
                        new ListIota(new List<Iota>() { e, new DoubleIota(Math.Round(player.GetMedia(), 2))}),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                }
            }
        }
        public class Project : OperatorBase
        {
            public Project(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {
                bool suc = false;

                if (player.CanUseMedia(stack.Shift().strength * 5 * ((float)stack.exp)))
                    suc = stack.Shift().Project(player);
                
                
                if (suc)
                {
                    player.UseMedia(stack.Shift().strength * 5 * ((float)stack.exp));
                    stack.inc();
                    castResult = new CastResult(
                        new ListIota(new List<Iota>() {  }),
                        stack,
                        ResolvedPatternType.EVALUATED);
                }
                else
                {
                    castResult = new CastResult(
                        new ListIota(new List<Iota>() { new DoubleIota(Math.Round(player.GetMedia(),2)) }),
                        stack,
                        ResolvedPatternType.ERRORED/*,
                        result.getSound()*/);
                }
            }
        }


        // Extra Spells 
        /*public class Ignite : OperatorBase
        {
            new static int cost = 1;
            public Ignite(Entity player, ICoreAPI api, State stack) : base(player, api, stack)
            {
                if (!player.CanUseMedia(cost))
                {
                    castResult = new CastResult(
                        new ListIota(new List<Iota>() { new DoubleIota(player.GetMedia()) }),
                        stack,
                        ResolvedPatternType.ERRORED);
                    return;
                }

                Iota e = stack.Pop();
                if (e is EntityIota)
                {
                    
                    ((EntityIota) e).getEntity().Ignite();
                }
                                else if (e is Vec3Iota)
                {


                    Vec3d pos = ((Vec3Iota)e).getVec3();

                    BlockPos block_pos = new BlockPos((int)pos.X, (int)pos.Y, (int)pos.Z);

                    //Vintagestory.API.Common.BlockEntity block = player.World.BlockAccessor.GetBlockEntity(block_pos);

                    IBlockAccessor blockAcc = player.World.GetBlockAccessor(true, true, true);

                    Block block = blockAcc.GetBlock(block_pos);

                    IIgnitable ign = block?.GetInterface<IIgnitable>(player.World, block_pos);
                    if (ign != null)
                    {
                        ign.OnTryIgniteBlock((EntityAgent)player, block_pos, 100);
                        EnumHandling handling = EnumHandling.PreventDefault;
                        ign.OnTryIgniteBlockOver((EntityAgent)player, block_pos, 100, ref handling);
                    }
                    else
                    {

                        for (int x = -1; x <= 1; x++)
                        {
                            for (int y = -1; y <= 1; y++)
                            {
                                for (int z = -1; z <= 1; z++)
                                {
                                    BlockPos bpos = block_pos.AddCopy(x, y, z);

                                    block = blockAcc.GetBlock(bpos);

                                    if (block.BlockId == 0)
                                    {
                                        blockAcc.SetBlock(player.World.GetBlock(new AssetLocation("fire")).BlockId, bpos);
                                        Vintagestory.API.Common.BlockEntity befire = blockAcc.GetBlockEntity(bpos);
                                        befire?.GetBehavior<BEBehaviorBurning>()?.OnFirePlaced(bpos, block_pos, (player as EntityPlayer).PlayerUID);
                                    }
                                }
                            }
                        }
                    }

                }
                else
                {

                    castResult = new CastResult(
                        new ListIota(new List<Iota>() { e }),
                        stack,
                        ResolvedPatternType.ERRORED);
                    return;
                }

                player.UseMedia(cost);

                castResult = new CastResult(
                        new ListIota(new List<Iota>() { e }),
                        stack,
                        ResolvedPatternType.EVALUATED);
            }
        }*/

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
