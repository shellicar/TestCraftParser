using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NordockCraft.Data.Model;

namespace NordockCraft.Data.Tests
{
    [TestClass]
    public class ContextFacts : SqliteDatabaseFacts
    {
        [TestMethod]
        public void Two_recipes_with_same_ingredients_creates_one_ingredient_entry()
        {
            WhenCreateRecipe("created1", "ingredient");
            WhenCreateRecipe("created2", "ingredient");

            using (var context = Context)
            {
                var i = context.Ingredients.ToList();
                Assert.AreEqual(1, context.Ingredients.Count());
                Assert.AreEqual(2, context.RecipeIngredients.Count());
            }
        }

        [TestMethod]
        public void Created_recipe_has_created_item()
        {
            WhenCreateRecipe("created", "ingredient");

            using (var context = Context)
            {
                var item = context.Items.Single(x => x.Name == "created");

                var recipe = context.Recipes.First();

                Assert.AreEqual(item, recipe.Item);
            }
        }

        [TestMethod]
        public void Created_recipe_has_ingredients()
        {
            WhenCreateRecipe("created", "ingredient");

            using (var context = Context)
            {
                var item = context.Items.Single(x => x.Name == "ingredient");

                var recipes = context.Recipes.Include(x => x.RecipeIngredients).ThenInclude(x => x.Ingredient);
                var recipe = recipes.First();

                var single = recipe.RecipeIngredients.Single(x => x.Ingredient.Item == item);
            }
        }

        private void WhenCreateRecipe(string itemName, params string[] ingredients)
        {
            using (var context = Context)
            {
                var item1 = GetCreateItem(context, itemName);

                var ingredList = ingredients.Select(x => GetCreateIngredient(context, 10, x));
                var recipeIngredients = ingredList.Select(x => new RecipeIngredient {Ingredient = x});

                var location = GetCreateLocation(context, "somewhere");
                var recipe = new Recipe
                {
                    Item = item1,
                    RecipeIngredients = recipeIngredients.ToList()
                };
                recipe.RecipeLocations = new List<RecipeLocation> {new RecipeLocation {Recipe = recipe, Location = location}};

                context.Recipes.Add(recipe);
                context.SaveChanges();
            }
        }

        private Location GetCreateLocation(CraftingContext context, string name)
        {
            return context.Locations.SingleOrDefault(x => x.Name == name) ?? new Location {Name = name};
        }

        private Ingredient GetCreateIngredient(CraftingContext context, int amount, string name)
        {
            var item = GetCreateItem(context, name);

            return context.Ingredients.SingleOrDefault(x => x.Amount == amount && x.Item == item) ?? new Ingredient {Amount = amount, Item = item};
        }

        private Item GetCreateItem(CraftingContext context, string itemName)
        {
            return context.Items.SingleOrDefault(x => x.Name == itemName) ?? new Item {Name = itemName};
        }
    }
}