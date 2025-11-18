using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

namespace VSHexMod.Crafting
{
    [DocumentAsJson]
    public class ScribeOutputStack : JsonItemStack
    {
        [DocumentAsJson] public int Count;

        public override void FromBytes(BinaryReader reader, IClassRegistryAPI instancer)
        {
            base.FromBytes(reader, instancer);

            Count = reader.ReadInt32();
        }
        public override void ToBytes(BinaryWriter writer)
        {
            base.ToBytes(writer);

            writer.Write(Count);
        }
        public new ScribeOutputStack Clone()
        {
            ScribeOutputStack stack = new ScribeOutputStack()
            {
                Code = Code.Clone(),
                ResolvedItemstack = ResolvedItemstack?.Clone(),
                StackSize = StackSize,
                Type = Type,
                Count = Count
            };

            if (Attributes != null) stack.Attributes = Attributes.Clone();

            return stack;
        }
    }
}
