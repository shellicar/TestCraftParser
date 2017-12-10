using System;
using System.Linq;
using NordockCraft.Data.Access;
using NordockCraft.Data.Model;

namespace NordockCraft.Data.Logic
{
    public class CreateRecipeCommand : ICreateRecipeCommand
    {
        public CreateRecipeCommand(ICraftingAccess access)
        {
            Access = access;
        }

        private ICraftingAccess Access { get; }

        public void CreateRecipe(Recipe recipe)
        {
            var existing = Access.Recipes.SingleOrDefault(x => x.Item == recipe.Item);
            if (existing != null)
            {
                throw new InvalidOperationException("Recipe already exists");
            }

            Access.AddRecipe(recipe);
        }
    }
}