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
        /// Registers a knapping recipe. Only use it if you really want to avoid using json files for recipes. 
        /// </summary>
        /// <param name="api"></param>
        /// <param name="r"></param>
        public static void RegisterScribeRecipe(this ICoreServerAPI api, ScribeRecipe r)
        {
            api.ModLoader.GetModSystem<ScribeRecipeRegistrySysem>().RegisterScribeRecipe(r);
        }
    }
    public class ScribeRecipeRegistrySysem : ModSystem
    {
        public static bool canRegister = true;

        /// <summary>
        /// List of all loaded Scribe recipes
        /// </summary>
        public List<ScribeRecipe> ScribeRecipes = new List<ScribeRecipe>();

        public override double ExecuteOrder()
        {
            return 0.6;
        }

        public override void StartPre(ICoreAPI api)
        {
            canRegister = true;
        }

        public override void Start(ICoreAPI api)
        {
            ScribeRecipes = api.RegisterRecipeRegistry<RecipeRegistryGeneric<ScribeRecipe>>("scriberecipes").Recipes;
        }

        

        public void RegisterScribeRecipe(ScribeRecipe recipe)
        {
            if (!canRegister) throw new InvalidOperationException("Coding error: Can no long register cooking recipes. Register them during AssetsLoad/AssetsFinalize and with ExecuteOrder < 99999");
            
            if (recipe.Code == null)
            {
                throw new ArgumentException("Scribe recipes must have a non-null code! (choose freely)");
            }


            ScribeRecipes.Add(recipe);
        }
    }
}
