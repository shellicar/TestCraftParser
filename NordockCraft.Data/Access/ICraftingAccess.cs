using System;
using System.Linq;
using NordockCraft.Data.Model;

namespace NordockCraft.Data.Access
{
    public interface ICraftingAccess : IDisposable
    {
        IQueryable<Location> Locations { get; }
        IQueryable<RecipeLocation> RecipeLocations { get; }
        IQueryable<Item> Items { get; }
        IQueryable<Ingredient> Ingredients { get; }
        IQueryable<Recipe> Recipes { get; }
        IQueryable<RecipeIngredient> RecipeIngredients { get; }

        void AddRecipe(Recipe recipe);
        void Save();
        void AddRecipeLocation(RecipeLocation recipeLocation);
        void RemoveRecipeIngredient(RecipeIngredient del);
        void RemoveIngredient(Ingredient ing);
    }
}