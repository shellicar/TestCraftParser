using System.Linq;
using Microsoft.EntityFrameworkCore;
using NordockCraft.Data.Model;

namespace NordockCraft.Data.Access
{
    public class CraftingAccess : ICraftingAccess
    {
        public CraftingAccess(CraftingContext context)
        {
            Context = context;
        }

        public CraftingContext Context { get; }

        public IQueryable<RecipeLocation> RecipeLocations => Context.RecipeLocations.Include(x => x.Recipe).Include(x => x.Location);

        public IQueryable<Item> Items => Context.Items
            .Include(x => x.Recipe).ThenInclude(x => x.RecipeIngredients).ThenInclude(x => x.Ingredient).ThenInclude(x => x.Item)
            .Include(x => x.Recipe).ThenInclude(x => x.RecipeLocations).ThenInclude(x => x.Location)
            .Include(x => x.IngredientFor).ThenInclude(x => x.RecipeIngredients).ThenInclude(x => x.Recipe).ThenInclude(x => x.Item)
            .Include(x => x.IngredientFor).ThenInclude(x => x.RecipeIngredients).ThenInclude(x => x.Recipe).ThenInclude(x => x.RecipeLocations).ThenInclude(x => x.Location);


        public IQueryable<Location> Locations => Context.Locations;
        public IQueryable<Ingredient> Ingredients => Context.Ingredients.Include(x => x.Item);
        public IQueryable<Recipe> Recipes => Context.Recipes.Include(x => x.RecipeIngredients).ThenInclude(x => x.Ingredient).ThenInclude(x => x.RecipeIngredients);
        public IQueryable<RecipeIngredient> RecipeIngredients => Context.RecipeIngredients.Include(x => x.Ingredient);

        public void AddRecipe(Recipe recipe)
        {
            Context.Recipes.Add(recipe);
        }

        public void Save()
        {
            Context.SaveChanges();
        }

        public void AddRecipeLocation(RecipeLocation recipeLocation)
        {
            Context.RecipeLocations.Add(recipeLocation);
        }

        public void RemoveRecipeIngredient(RecipeIngredient del)
        {
            Context.RecipeIngredients.Remove(del);
        }

        public void RemoveIngredient(Ingredient ing)
        {
            Context.Ingredients.Remove(ing);
        }

        public void Dispose()
        {
            Context?.Dispose();
        }

        public void RemoveIngredient(RecipeIngredient del)
        {
            Context.RecipeIngredients.Remove(del);
        }
    }
}