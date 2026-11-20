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
    public class ScribeRecipe : RecipeBase, IByteSerializable, IConcreteCloneable<ScribeRecipe>
    {
        #region From JSON

        [DocumentAsJson("Required")]
        public CraftingRecipeIngredient? Output { get; set; }

        [DocumentAsJson("Required")]
        public CraftingRecipeIngredient? Ingredient { get; set; }

        #endregion
        public override IEnumerable<IRecipeIngredient> RecipeIngredients => [Ingredient ?? throw new InvalidOperationException($"Scribe recipe '{Name}' has no ingredient specified")];

        public override IRecipeOutput RecipeOutput => Output ?? throw new InvalidOperationException($"Scribe recipe '{Name}' has no output specified");


        public override void OnParsed(IWorldAccessor world)
        {
            if (Ingredient == null) return;

            Ingredient.Id ??= "input";
            
        }
        public bool Matches(ItemSlot[] inputSlots)
        {

            List<KeyValuePair<ItemSlot, CraftingRecipeIngredient>> matched = pairInput(inputSlots);
            if (matched is null) return false;

            return true;
        }

        public bool TryCraftNow(ICoreAPI api, double nowSealedHours, ItemSlot[] inputslots)
        {

            var matched = pairInput(inputslots);

            ItemStack mixedStack = Output.ResolvedItemStack.Clone();
            mixedStack.StackSize = 1;



            //ItemStack remainStack = null;
            //foreach (var val in matched)
            //{
            //    if (val.Value.Quantity != null)
            //    {
            //        remainStack = val.Key.Itemstack;
            //        remainStack.StackSize -= (int)val.Value.Quantity * (mixedStack.StackSize / Output.StackSize);
            //        if (remainStack.StackSize <= 0)
            //        {
            //            remainStack = null;
            //        }
            //        break;
            //    }
            //}

            // Slot 0: Input slot
            // Slot 1: Output slot

            inputslots[0].TakeOut(1);
            inputslots[1].Itemstack = mixedStack;
            
            

            inputslots[0].MarkDirty();
            inputslots[1].MarkDirty();

            return true;
        }

        List<KeyValuePair<ItemSlot, CraftingRecipeIngredient>> pairInput(ItemSlot[] inputStacks)
        {
            List<CraftingRecipeIngredient> ingredientList = [Ingredient];


            Queue<ItemSlot> inputSlotsList = new Queue<ItemSlot>();
            foreach (var val in inputStacks) if (!val.Empty) inputSlotsList.Enqueue(val);

            List<KeyValuePair<ItemSlot, CraftingRecipeIngredient>> matched = new List<KeyValuePair<ItemSlot, CraftingRecipeIngredient>>();

            while (inputSlotsList.Count > 0)
            {
                ItemSlot inputSlot = inputSlotsList.Dequeue();
                bool found = false;

                for (int i = 0; i < ingredientList.Count; i++)
                {
                    CraftingRecipeIngredient ingred = ingredientList[i];
                    var n = ingred.ResolvedItemStack;
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

        public override void ToBytes(BinaryWriter writer)
        {
            writer.Write(Name?.ToShortString() ?? "");
            Ingredient.ToBytes(writer);
            Output.ToBytes(writer);

        }
    

        public override void FromBytes(BinaryReader reader, IWorldAccessor resolver)
        {
            Name = reader.ReadString();
            Ingredient = new CraftingRecipeIngredient();
            Ingredient.FromBytes(reader, resolver);
            Ingredient.Resolve(resolver, "Scribe Recipe (FromBytes)");


            Output = new CraftingRecipeIngredient();
            Output.FromBytes(reader, resolver);
            Output.Resolve(resolver, "Scribe Recipe (FromBytes)");

        }


        
        override public bool Resolve(IWorldAccessor world, string sourceForErrorLogging)
        {
            bool ok = true;
            Ingredient.Resolve(world, sourceForErrorLogging);

            if(ok) Output.Resolve(world, sourceForErrorLogging);

            return ok;
        }


        public override ScribeRecipe Clone()
        {
            ScribeRecipe recipe = new();

            CloneTo(recipe);

            return recipe;
        }

        protected override void CloneTo(object recipe)
        {
            recipe = new ScribeRecipe()
            {
                Output = Output.Clone(),
                Name = Name,
                Enabled = Enabled,
                RecipeId = RecipeId,
                Ingredient = Ingredient.Clone()
            };
        }
    }
}
