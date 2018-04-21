namespace NordockCraft.Data.Model
{
    public class RecipeIngredient
    {
        public Recipe Recipe { get; set; }
        public int RecipeId { get; set; }

        public Ingredient Ingredient { get; set; }
        public int IngredientId { get; set; }

        public override string ToString()
        {
            return $"{nameof(RecipeId)}: {RecipeId}, {nameof(IngredientId)}: {IngredientId}";
        }
    }
}