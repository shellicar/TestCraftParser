using System.Collections.Generic;

namespace NordockCraft.Data.Model
{
    public class Recipe
    {
        public int Id { get; set; }
        public Item Item { get; set; }
        public int ItemId { get; set; }
        public int? Created { get; set; }

        // mapped navigation properties
        public List<RecipeIngredient> RecipeIngredients { get; set; }

        public List<RecipeLocation> RecipeLocations { get; set; }
    }
}