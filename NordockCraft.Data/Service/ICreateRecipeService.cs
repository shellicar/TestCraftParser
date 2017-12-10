using System;
using System.Collections.Generic;

namespace NordockCraft.Data.Service
{
    public interface ICreateRecipeService : IDisposable
    {
        RecipeDto CreateRecipe(string item, int? created, IEnumerable<IngredientDto> ingredients);
        void AddRecipeLocation(int id, string recipeLocation);
        void UpdateRecipeWithLocation(string recipeItemCreated, string recipeLocation, int? createdCount, IEnumerable<IngredientDto> ingredients);
    }
}