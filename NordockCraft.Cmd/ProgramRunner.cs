using System;
using System.Linq;
using NordockCraft.Data.Access;
using NordockCraft.Data.Model;
using NordockCraft.Data.Service;

namespace NordockCraft.Cmd
{
    internal class ProgramRunner : IDisposable
    {
        public ICreateRecipeService Service => new CreateRecipeService(Access);
        public ICraftingAccess Access => new CraftingAccess(Context);
        public ISpecialAccess Access2 => new SpecialAccess(Context);
        public CraftingContext Context => new CraftingContext();


        public void Dispose()
        {
        }

        public void Run()
        {
            using (var context = Context)
            {
                context.Database.EnsureCreated();
            }

            Console.WriteLine("Items by location");
            using (var access = Access)
            {
                foreach (var loc in access.Locations)
                {
                    Console.WriteLine($"Location: {loc.Name}");

                    foreach (var recipe in loc.RecipeLocations)
                    {
                        Console.WriteLine($"{recipe.Recipe.Item.Name}");
                    }
                    Console.WriteLine();
                }
            }

            Console.WriteLine("Items and ingredients");
            using (var access = Access)
            {
                var query = access.Items;
            }

            Console.WriteLine("Ingredients and items");
            using (var access = Access2)
            {
                var query = access.Items;

                query = query.Where(x => x.IngredientFor.Count > 0);
                query = query.OrderBy(x => x.IngredientFor.Count);

                foreach (var item in query)
                {
                    Console.WriteLine($"{item.Name}");

                    foreach (var use in item.IngredientFor)
                    {
                        Console.WriteLine($"{use.Amount}x");
                        foreach (var use2 in use.RecipeIngredients)
                        {
                            Console.WriteLine($" -> {use2.Recipe.Item.Name}");
                        }
                        Console.WriteLine("");
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}