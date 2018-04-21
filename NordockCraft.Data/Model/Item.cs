using System.Collections.Generic;

namespace NordockCraft.Data.Model
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Recipe Recipe { get; set; }

        public List<Ingredient> IngredientFor { get; set; }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Name)}: {Name}";
        }
    }
}