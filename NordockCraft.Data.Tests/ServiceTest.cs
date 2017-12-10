using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NordockCraft.Data.Access;
using NordockCraft.Data.Model;
using NordockCraft.Data.Service;

namespace NordockCraft.Data.Tests
{
    [TestClass]
    public class ServiceTest : SqliteDatabaseFacts
    {
        protected ICraftingAccess Access => new CraftingAccess(Context);
        protected ICreateRecipeService Service => new CreateRecipeService(Access);

        [TestMethod]
        public void Can_update_recipe_created()
        {
            using (var context = Context)
            {
                context.Recipes.Add(new Recipe {Item = new Item {Name = "hello"}});
                context.SaveChanges();
            }

            using (var service = Service)
            {
                service.UpdateRecipeWithLocation("hello", "location", 10, Enumerable.Empty<IngredientDto>());
            }

            using (var context = Context)
            {
                Assert.AreEqual(1, context.Recipes.Count());
                var recipe = context.Recipes.First();
                Assert.AreEqual(10, recipe.Created);
            }
        }

        [TestMethod]
        public void Can_update_recipe_ingredients()
        {
            var item1 = "helm";
            var first_ing = "ingredient";
            var second_ing = "new_ingredient";

            using (var context = Context)
            {
                var recipe = new Recipe {Item = new Item {Name = item1}};
                var ing = new Ingredient {Item = new Item {Name = first_ing}, Amount = 1};
                var link = new RecipeIngredient {Recipe = recipe, Ingredient = ing};
                context.RecipeIngredients.Add(link);
                context.SaveChanges();
            }

            using (var service = Service)
            {
                var ingdto = new IngredientDto {Amount = 11, ItemName = second_ing};
                service.UpdateRecipeWithLocation(item1, "location", 10, new[] {ingdto});
            }

            using (var context = Context)
            {
                Assert.AreEqual(1, context.Recipes.Count());
                Assert.AreEqual(1, context.RecipeIngredients.Count());
                Assert.AreEqual(1, context.Ingredients.Count());

                var recipe = context.Recipes.Include(x => x.RecipeIngredients).ThenInclude(x => x.Ingredient).ThenInclude(x => x.Item).First();
                Assert.AreEqual(second_ing, recipe.RecipeIngredients[0].Ingredient.Item.Name);
            }
        }

        [TestMethod]
        public void Recipes_share_ingredients()
        {
            var item1 = "helm";
            var item2 = "helm2";
            var location = "some_location";
            var created = 500;

            var ingredients = new[] {new IngredientDto {Amount = 50, ItemName = "ingredient"}};
            using (var service = Service)
            {
                service.UpdateRecipeWithLocation(item1, location, created, ingredients);
                service.UpdateRecipeWithLocation(item2, location, created, ingredients);
            }

            using (var context = Context)
            {
                Assert.AreEqual(1, context.Ingredients.Count());
                Assert.AreEqual(2, context.RecipeIngredients.Count());
                Assert.AreEqual(3, context.Items.Count());
                Assert.AreEqual(2, context.Recipes.Count());
            }
        }

        [TestMethod]
        public void Can_remove_recipe_ingredients()
        {
            var item1 = "helm";
            var first_ing = "ingredient";

            using (var context = Context)
            {
                var recipe = new Recipe {Item = new Item {Name = item1}};
                var ing = new Ingredient {Item = new Item {Name = first_ing}, Amount = 1};
                var link = new RecipeIngredient {Recipe = recipe, Ingredient = ing};
                context.RecipeIngredients.Add(link);
                context.SaveChanges();
            }

            using (var service = Service)
            {
                service.UpdateRecipeWithLocation(item1, "location", 10, Enumerable.Empty<IngredientDto>());
            }

            using (var context = Context)
            {
                Assert.AreEqual(1, context.Recipes.Count());
                Assert.AreEqual(0, context.RecipeIngredients.Count());
                Assert.AreEqual(0, context.Ingredients.Count());
            }
        }

        [TestMethod]
        public void Adding_recipe_with_same_location_only_creates_one_location()
        {
            using (var service = Service)
            {
                var itemName = "helm";
                var loc1 = "loc1";
                var ingredients = Enumerable.Empty<IngredientDto>();

                service.UpdateRecipeWithLocation(itemName, loc1, null, ingredients);
                service.UpdateRecipeWithLocation(itemName, loc1, null, ingredients);
            }
            using (var context = Context)
            {
                Assert.AreEqual(1, context.Recipes.Count());
                Assert.AreEqual(1, context.Locations.Count());
            }
        }
    }
}