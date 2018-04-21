using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using NordockCraft.Data.Access;
using NordockCraft.Data.Logic;
using NordockCraft.Data.Model;

namespace NordockCraft.Data.Service
{
    public class CreateRecipeService : ICreateRecipeService
    {
        public CreateRecipeService(ICraftingAccess access)
        {
            Access = access;
        }

        private static ILog Log { get; } = LogManager.GetLogger(typeof(CreateRecipeService));
        private ICraftingAccess Access { get; }


        public RecipeDto CreateRecipe(string itemName, int? created, IEnumerable<IngredientDto> ingredientsDto)
        {
            var item = GetCreateItem(itemName);
            var ingredients = ingredientsDto.Select(GetCreateIngredient);

            var entries = ingredients.Select(CreateIngredient);

            var recipe = new Recipe {Item = item, RecipeIngredients = entries.ToList(), Created = created};

            ICreateRecipeCommand cmd = new CreateRecipeCommand(Access);
            cmd.CreateRecipe(recipe);

            Access.Save();

            var dto = new RecipeDto {Id = recipe.Id};
            return dto;
        }

        public void AddRecipeLocation(int id, string recipeLocation)
        {
            var loc = GetCreateLocation(recipeLocation);
            IAddRecipeLocationCommand cmd = new AddRecipeLocationCommand(Access);
            var recipe = Access.Recipes.SingleOrDefault(x => x.Id == id);
            if (recipe == null)
            {
                throw new InvalidOperationException();
            }

            var existing = Access.RecipeLocations.SingleOrDefault(x => x.Recipe == recipe && x.Location == loc);
            if (existing == null)
            {
                cmd.AddLocation(recipe, loc);
            }
        }

        public void UpdateRecipeWithLocation(string recipeItemCreated, string recipeLocation, int? createdCount, IEnumerable<IngredientDto> ingredients)
        {
            int recipeId;
            var recipe = Access.Recipes.SingleOrDefault(x => x.Item.Name == recipeItemCreated);
            if (recipe == null)
            {
                Log.Debug("Creating recipe");
                recipeId = CreateRecipe(recipeItemCreated, createdCount, ingredients).Id;
            }
            else
            {
                Log.Debug("Updating recipe");
                UpdateRecipe(recipe, createdCount, ingredients);
                recipeId = recipe.Id;
            }

            AddRecipeLocation(recipeId, recipeLocation);

            Access.Save();
        }

        public void Dispose()
        {
            Access?.Dispose();
        }

        private void UpdateRecipe(Recipe recipe, int? createdCount, IEnumerable<IngredientDto> ingredientsDto)
        {
            var ingredients = ingredientsDto.Select(GetCreateIngredient);
            var entries = ingredients.Select(x => GetCreateIngredient(recipe, x)).ToList();

            var toDelete = recipe.RecipeIngredients.Except(entries).ToList();
            foreach (var del in toDelete)
            {
                Access.RemoveRecipeIngredient(del);
            }
            foreach (var ingredient in toDelete.GroupBy(x => x.Ingredient))
            {
                var ing = ingredient.Key;
                if (ing.RecipeIngredients == null)
                {
                    throw new InvalidProgramException("Must include recipe ingredients");
                }

                if (ing.RecipeIngredients.All(x => x.Recipe == recipe))
                {
                    Access.RemoveIngredient(ing);
                }
            }


            recipe.Created = createdCount;
            recipe.RecipeIngredients = entries;

            Access.Save();
        }


        private Ingredient GetCreateIngredient(IngredientDto arg)
        {
            var item = GetCreateItem(arg.ItemName);
            var ingredient = Access.Ingredients.SingleOrDefault(x => x.Item == item && x.Amount == arg.Amount);
            if (ingredient == null)
            {
                ingredient = new Ingredient {Item = item, Amount = arg.Amount};
                Log.Debug($"Creating ingredient: {ingredient}");
            }
            else
            {
                Log.Debug($"Found ingredient: {ingredient}");
            }
            return ingredient;
        }

        private Location GetCreateLocation(string name)
        {
            var location = Access.Locations.SingleOrDefault(x => x.Name == name);
            if (location == null)
            {
                location = new Location {Name = name};
                Log.Debug($"Creating location: {location}");
            }
            else
            {
                Log.Debug($"Found location: {location}");
            }
            return location;
        }

        private Item GetCreateItem(string name)
        {
            var item = Access.Items.SingleOrDefault(x => x.Name == name);
            if (item == null)
            {
                item = new Item {Name = name};
                Log.Debug($"Creating item: {item}");
            }
            else
            {
                Log.Debug($"Found item: {item}");
            }
            return item;
        }

        private RecipeIngredient GetCreateIngredient(Recipe recipe, Ingredient ingredient)
        {
            var existing = Access.RecipeIngredients.SingleOrDefault(x => x.Recipe == recipe && x.Ingredient == ingredient);
            if (existing == null)
            {
                existing = new RecipeIngredient {Ingredient = ingredient};
                Log.Debug($"Creating recipe ingredient: {existing}");
            }
            else
            {
                Log.Debug($"Found recipe ingredient: {existing}");
            }
            return existing;
        }

        private RecipeIngredient CreateIngredient(Ingredient arg)
        {
            return new RecipeIngredient {Ingredient = arg};
        }
    }
}