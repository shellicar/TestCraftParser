using System.Collections.Generic;

namespace NordockCraft.Data.Model
{
    public class Ingredient
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public int ItemId { get; set; }

        public Item Item { get; set; }

        public List<RecipeIngredient> RecipeIngredients { get; set; }


        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Amount)}: {Amount}, {nameof(ItemId)}: {ItemId}";
        }
    }
}