using System.Linq;
using Microsoft.EntityFrameworkCore;
using NordockCraft.Data.Model;

namespace NordockCraft.Data.Access
{
    public class SpecialAccess : ISpecialAccess
    {
        public SpecialAccess(CraftingContext context)
        {
            Context = context;
        }

        public CraftingContext Context { get; }

        public void Dispose()
        {
            Context?.Dispose();
        }

        public IQueryable<Item> Items => Context.Items.Include(x => x.IngredientFor).ThenInclude(x => x.RecipeIngredients).ThenInclude(x => x.Recipe).ThenInclude(x => x.Item);
        public IQueryable<Ingredient> Ingredients => Context.Ingredients;
    }
}