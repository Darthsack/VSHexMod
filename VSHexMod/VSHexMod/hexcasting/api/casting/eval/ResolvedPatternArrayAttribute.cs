using Cairo;
using HarmonyLib;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Util;
using Vintagestory.Client.NoObf;
using VSHexMod.hexcasting.api.casting.math;

namespace VSHexMod.hexcasting.api.casting.eval
{
    public class ResolvedPatternArrayAttribute : ArrayAttribute<ResolvedPattern>, IAttribute
    {
        public ResolvedPatternArrayAttribute() { }

        public ResolvedPatternArrayAttribute(ResolvedPattern[] value)
        {
            this.value = value;
        }

        public ResolvedPatternArrayAttribute(List<ResolvedPattern> value)
        {
            this.value = value.ToArray();
        }

        public void ToBytes(BinaryWriter stream)
        {
            stream.Write(value.Length);
            foreach (ResolvedPattern pattern in value)
            {
                pattern.ToBytes(stream);
            }
        }

        public int GetAttributeId()
        {
            return 20;
        }

        public void FromBytes(BinaryReader stream)
        {
            int quantity = stream.ReadInt32();
            List<ResolvedPattern> hexpatterns = new List<ResolvedPattern>();
            for (int i = 0; i < quantity; i++)
            {
                hexpatterns.Add(new ResolvedPattern(stream, null));
            }
        }

        public override string ToJsonToken()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");

            for (int i = 0; i < value.Length; i++)
            {
                if (i > 0) sb.Append(", ");

                sb.Append("\"" + value[i].ToString() + "\"");
            }
            sb.Append("]");

            return sb.ToString();
        }

        public static IAttribute FromJson(string json)
        {
            ResolvedPattern[] value = new ResolvedPattern[0];
            JToken token = JToken.Parse(json);
            JArray jarr = token as JArray;
            if (jarr != null)
            {
                if (!jarr.HasValues)
                    return new ResolvedPatternArrayAttribute(new ResolvedPattern[0]);

                
                foreach (JValue val in jarr)
                {

                    string o = (string)val;

                    value.AddToArray(ResolvedPattern.FromString(o));
                }

                value = new ResolvedPattern[jarr.Count];
            }
            return new ResolvedPatternArrayAttribute(value);
        }

        public IAttribute Clone()
        {
            return new ResolvedPatternArrayAttribute((ResolvedPattern[])value.Clone());
        }

    }
}
