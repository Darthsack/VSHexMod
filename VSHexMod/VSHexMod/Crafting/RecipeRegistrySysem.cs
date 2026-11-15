using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Server;
using Vintagestory.GameContent;

namespace VSHexMod.Crafting
{
    public static class ApiAdditions
    {
        public static List<ScribeRecipe> GetScribeRecipes(this ICoreAPI api)
        {
            return api.ModLoader.GetModSystem<ScribeRecipeRegistrySysem>().ScribeRecipes;
        }

        /// <summary>
        /// Registers a Scribe bench recipe. this was hard coded, but I want to make it proper
        /// </summary>
        /// <param name="api"></param>
        /// <param name="r"></param>
        public static void RegisterScribeRecipe(this ICoreServerAPI api, ScribeRecipe r)
        {
            api.ModLoader.GetModSystem<ScribeRecipeRegistrySysem>().RegisterScribeRecipe(r);
        }
    }

    public class DisableScribeRecipeRegisteringSystem : ModSystem
    {
        public override double ExecuteOrder() => 99999;
        public override bool ShouldLoad(EnumAppSide forSide) => forSide == EnumAppSide.Server;
        public override void AssetsFinalize(ICoreAPI api)
        {
            ScribeRecipeRegistrySysem.canRegister = false;
        }
    }

    public class ScribeRecipeRegistrySysem : ModSystem
    {
        public static bool canRegister = true;

        /// <summary>
        /// List of all loaded Scribe recipes
        /// </summary>
        public List<ScribeRecipe> ScribeRecipes = new List<ScribeRecipe>();


        ICoreAPI api;
        public override double ExecuteOrder()
        {
            return 0.6;
        }

        public override void StartPre(ICoreAPI api)
        {
            this.api = api;
            canRegister = true;
        }

        public override void Start(ICoreAPI api)
        {
            ScribeRecipes = api.RegisterRecipeRegistry<RecipeRegistryGeneric<ScribeRecipe>>("scriberecipes").Recipes;
        }

        

        public void RegisterScribeRecipe(ScribeRecipe recipe)
        {
            if (!canRegister) throw new InvalidOperationException("Coding error: Can no long register cooking recipes. Register them during AssetsLoad/AssetsFinalize and with ExecuteOrder < 99999");
            
            ScribeRecipes.Add(recipe);
        }
    }
}
