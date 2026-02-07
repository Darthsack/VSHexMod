using Cairo;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.Client.NoObf;
using Vintagestory.GameContent;
using Vintagestory.Server;
using VSHexMod.EntityBehaviors;
using VSHexMod.hexcasting.api.casting.arithmetic.operators;
using VSHexMod.hexcasting.api.casting.math;
using VSHexMod.hexcasting.common.lib.hex;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static VSHexMod.VSHexModModSystem;

namespace VSHexMod.hexcasting.api.casting.eval.iota
{
    public class PatternIota : Iota
    {
        public PatternIota(HexPattern pattern) : base(typeof(IotaType<PatternIota>), pattern)
        {
        }
        public PatternIota() : base(typeof(IotaType<PatternIota>), new HexPattern(HexDir.EAST))
        {
        }

        public HexPattern getPattern()
        {
            return (HexPattern)this.payload;
        }

        protected PatternIota(Type type, object payload) : base(type, payload)
        {
        }

        public override bool isTruthy()
        {
            return true;
        }

        public override bool toleratesOther(Iota that)
        {
            return typesMatch(this, that)
                && that is PatternIota
                && this.getPattern().anglesSignature().Equals(((PatternIota)that).getPattern().anglesSignature());
        }

        public override CastResult execute(Entity player,ICoreAPI api, State Current)
        {
            if(Current >= 1000) 
                return new CastResult(
                    this,
                    Current,
                    ResolvedPatternType.ERRORED/*,
                    result.getSound()*/);
            Current++;

            api.ModLoader.GetModSystem<VSHexModModSystem>().HexRegistry.TryGetValue(this.getPattern().anglesSignature(), out Type lookup);

            if (lookup is null) 
            {
                string sig = this.getPattern().anglesSignature();

                if (sig.StartsWith("aqaa") || sig.StartsWith("dedd"))
                {
                    bool negate = sig.StartsWith("dedd");
                    double accumulator = 0.0;

                    foreach (char ch in sig.Skip(4))
                    {
                        switch (ch) {
                            case 'w':
                                accumulator += 1;
                                break;

                            case 'q':
                                accumulator += 5;
                                break;

                            case 'e':
                                accumulator += 10;
                                break;

                            case 'a':
                                accumulator *= 2;
                                break;

                            case 'd':
                                accumulator /= 2;
                                break;
                            // "ok funny man" I do like these funny words magic man
                            case 's':
                                break;
                        }
                    }
                    if (negate)
                    {
                        accumulator = -accumulator;
                    }
                    Current.Push(new DoubleIota(accumulator));

                    return new CastResult(
                    this,
                    Current,
                    ResolvedPatternType.EVALUATED/*,
                    result.getSound()*/);
                }
                else
                {
                    HexPattern pat = this.getPattern();

                    HexDir[] directions = pat.directions();

                    var flatDir = pat.startDir;
                    if (pat.angles.Length > 0 && pat.angles[0] == HexAngle.LEFT_BACK)
                    {
                        flatDir = directions[0].rotatedBy(HexAngle.LEFT);
                    }

                    List<bool> mask = new List<bool>();
                    int i = 0;

                    while (i < directions.Length)           
                    {
                        HexAngle angle = directions[i].angleFrom(flatDir);
                        if (angle == HexAngle.FORWARD)
                        {
                            mask.Add(true);
                            i++;
                            continue;
                        }
                        if (i >= directions.Length - 1)
                        {
                            // then we're out of angles!
                            return new CastResult(
                                this,
                                Current,
                                ResolvedPatternType.ERRORED/*,
                                result.getSound()*/);
                        }
                        HexAngle angle2 = directions[i + 1].angleFrom(flatDir);
                        if (angle == HexAngle.RIGHT && angle2 == HexAngle.LEFT)
                        {
                            mask.Add(false);
                            // skip both segments of the dip
                            i += 2;
                            continue;
                        }
                        return new CastResult(
                            this,
                            Current,
                            ResolvedPatternType.ERRORED/*,
                            result.getSound()*/);
                    }
                    mask.Reverse();
                    List<Iota> temp = new List<Iota>();
                    foreach(bool b in mask)
                    {
                        if(!Current.stack.TryPop(out Iota n))
                            return new CastResult(
                            this,
                            Current,
                            ResolvedPatternType.ERRORED/*,
                            result.getSound()*/);
                        if (b)
                            temp.Add(n);
                    }
                    temp.Reverse();
                    foreach (Iota n in temp)
                    {
                        Current.Push(n);
                    }

                    return new CastResult(
                        this,
                        Current,
                        ResolvedPatternType.EVALUATED/*,
                        result.getSound()*/);
                }
            }

            try
            {
                OperatorBase op = (OperatorBase)lookup.Assembly.CreateInstance(lookup.FullName,false,0,null, new object[] { player, api, Current} ,null ,null );

                return op.getResult();
            }
            catch (Exception e) 
            {
                return new CastResult(
                this,
                Current,
                ResolvedPatternType.ERRORED/*,
                result.getSound()*/);
            }
        }

        public override bool executable()
        {
            return true;
        }

        public override void ToBytes(BinaryWriter writer, ICoreAPI api)
        {
            writer.Write(api.GetIotaKey(this));

            HexPattern val = getPattern();
            val.ToBytes(writer);
        }

        public override Iota FromBytes(BinaryReader reader, IWorldAccessor resolver)
        {
            HexPattern val = new(HexDir.EAST);
            val.FromBytes(reader, resolver);
            payload = val;

            return new PatternIota(val);
        }

    }
}
