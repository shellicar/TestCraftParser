using System.Collections.Immutable;

namespace NordockCraft.Data.Service
{
    public class RecipeDto
    {
        public int Id { get; set; }
        public string ItemCreated { get; set; }
        public IImmutableList<IngredientDto> Ingredients { get; set; }
    }
}