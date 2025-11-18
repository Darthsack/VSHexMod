using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Util;
using Vintagestory.API.Common;
using System.Xml.Linq;
using Vintagestory.API;
using Vintagestory.GameContent;

namespace VSHexMod.Crafting
{

    public class ScribeRecipeIngredient : CraftingRecipeIngredient
    {
        /// <summary>
        /// The character used in the grid recipe pattern that matches this ingredient. Generated when the recipe is loaded.
        /// </summary>
        public string PatternCode;
        

        public override void ToBytes(BinaryWriter writer)
        {
            base.ToBytes(writer);
            writer.Write(PatternCode);
        }

        public override void FromBytes(BinaryReader reader, IWorldAccessor resolver)
        {
            base.FromBytes(reader, resolver);
            PatternCode = reader.ReadString().DeDuplicate();
        }
    }

    [DocumentAsJson]
    public class ScribeRecipe : IByteSerializable, IRecipeBase<ScribeRecipe>
    {
        [DocumentAsJson] public int RecipeId;
        [DocumentAsJson] public CraftingRecipeIngredient[] Ingredients;
        [DocumentAsJson] public ScribeOutputStack Output;
        [DocumentAsJson] public AssetLocation Name { get; set; }
        [DocumentAsJson] public bool Enabled { get; set; } = true;
        [DocumentAsJson] public string Code;


        IRecipeIngredient[] IRecipeBase<ScribeRecipe>.Ingredients => Ingredients;
        IRecipeOutput IRecipeBase<ScribeRecipe>.Output => Output;


        public bool Matches(ItemSlot[] inputSlots)
        {

            List<KeyValuePair<ItemSlot, CraftingRecipeIngredient>> matched = pairInput(inputSlots);
            if (matched is null) return false;

            return true;
        }

        public bool TryCraftNow(ICoreAPI api, double nowSealedHours, ItemSlot[] inputslots)
        {

            var matched = pairInput(inputslots);

            ItemStack mixedStack = Output.ResolvedItemstack.Clone();
            mixedStack.StackSize = 1;


            // Carry over freshness

            ItemStack remainStack = null;
            foreach (var val in matched)
            {
                if (val.Value.Quantity != null)
                {
                    remainStack = val.Key.Itemstack;
                    remainStack.StackSize -= (int)val.Value.Quantity * (mixedStack.StackSize / Output.StackSize);
                    if (remainStack.StackSize <= 0)
                    {
                        remainStack = null;
                    }
                    break;
                }
            }

            // Slot 0: Input slot
            // Slot 1: Output slot

            inputslots[0].Itemstack = mixedStack;
            inputslots[2].Itemstack = remainStack;
            
            

            inputslots[0].MarkDirty();
            inputslots[2].MarkDirty();

            return true;
        }

        List<KeyValuePair<ItemSlot, CraftingRecipeIngredient>> pairInput(ItemSlot[] inputStacks)
        {
            List<CraftingRecipeIngredient> ingredientList = new List<CraftingRecipeIngredient>(Ingredients);


            Queue<ItemSlot> inputSlotsList = new Queue<ItemSlot>();
            foreach (var val in inputStacks) if (!val.Empty) inputSlotsList.Enqueue(val);

            if (inputSlotsList.Count != Ingredients.Length) return null;

            List<KeyValuePair<ItemSlot, CraftingRecipeIngredient>> matched = new List<KeyValuePair<ItemSlot, CraftingRecipeIngredient>>();

            while (inputSlotsList.Count > 0)
            {
                ItemSlot inputSlot = inputSlotsList.Dequeue();
                bool found = false;

                for (int i = 0; i < ingredientList.Count; i++)
                {
                    CraftingRecipeIngredient ingred = ingredientList[i];

                    if (ingred.SatisfiesAsIngredient(inputSlot.Itemstack))
                    {
                        matched.Add(new KeyValuePair<ItemSlot, CraftingRecipeIngredient>(inputSlot, ingred));
                        found = true;
                        ingredientList.RemoveAt(i);
                        break;
                    }
                }

                if (!found) return null;
            }

            // We're missing ingredients
            if (ingredientList.Count > 0)
            {
                return null;
            }

            return matched;
        }

        public void ToBytes(BinaryWriter writer)
        {
            writer.Write(Code);
            writer.Write(Ingredients.Length);
            for (int i = 0; i < Ingredients.Length; i++)
            {
                Ingredients[i].ToBytes(writer);
            }

            Output.ToBytes(writer);

        }
    

        public void FromBytes(BinaryReader reader, IWorldAccessor resolver)
        {
            Code = reader.ReadString();
            Ingredients = new ScribeRecipeIngredient[reader.ReadInt32()];

            for (int i = 0; i < Ingredients.Length; i++)
            {
                Ingredients[i] = new ScribeRecipeIngredient();
                Ingredients[i].FromBytes(reader, resolver);
                Ingredients[i].Resolve(resolver, "Barrel Recipe (FromBytes)");
            }

            Output = new ScribeOutputStack();
            Output.FromBytes(reader, resolver.ClassRegistry);
            Output.Resolve(resolver, "Barrel Recipe (FromBytes)");

        }


        public Dictionary<string, string[]> GetNameToCodeMapping(IWorldAccessor world)
        {
            Dictionary<string, string[]> mappings = new Dictionary<string, string[]>();

            if (Ingredients == null || Ingredients.Length == 0) return mappings;

            foreach (var ingred in Ingredients)
            {
                if (!ingred.Code.Path.Contains('*')) continue;

                int wildcardStartLen = ingred.Code.Path.IndexOf('*');
                int wildcardEndLen = ingred.Code.Path.Length - wildcardStartLen - 1;

                List<string> codes = new List<string>();

                if (ingred.Type == EnumItemClass.Block)
                {
                    foreach (Block block in world.Blocks)
                    {
                        if (block.IsMissing) continue;   // BlockList already performs the null check for us, in its enumerator

                        if (WildcardUtil.Match(ingred.Code, block.Code))
                        {
                            string code = block.Code.Path.Substring(wildcardStartLen);
                            string codepart = code.Substring(0, code.Length - wildcardEndLen);
                            if (ingred.AllowedVariants != null && !ingred.AllowedVariants.Contains(codepart)) continue;

                            codes.Add(codepart);

                        }
                    }
                }
                else
                {
                    foreach (Item item in world.Items)
                    {
                        if (item.Code == null || item.IsMissing) continue;

                        if (WildcardUtil.Match(ingred.Code, item.Code))
                        {
                            string code = item.Code.Path.Substring(wildcardStartLen);
                            string codepart = code.Substring(0, code.Length - wildcardEndLen);
                            if (ingred.AllowedVariants != null && !ingred.AllowedVariants.Contains(codepart)) continue;

                            codes.Add(codepart);
                        }
                    }
                }

                mappings[ingred.Name ?? "wildcard" + mappings.Count] = codes.ToArray();
            }

            return mappings;
        }

        public bool Resolve(IWorldAccessor world, string sourceForErrorLogging)
        {
            bool ok = true;

            return ok;
        }

        public ScribeRecipe Clone()
        {
            CraftingRecipeIngredient[] ingredients = new CraftingRecipeIngredient[Ingredients.Length];
            for (int i = 0; i < Ingredients.Length; i++)
            {
                ingredients[i] = Ingredients[i].Clone();
            }

            return new ScribeRecipe()
            {
                Output = Output.Clone(),
                Code = Code,
                Enabled = Enabled,
                Name = Name,
                RecipeId = RecipeId,
                Ingredients = ingredients
            };
        }
    }
}
