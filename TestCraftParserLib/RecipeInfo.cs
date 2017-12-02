using System.Collections.Generic;

namespace TestCraftParserLib
{
    public class RecipeInfo
    {
        public string ItemCreated { get; set; }
        public string Location { get; set; }

        public List<ItemRequirement> Requirements { get; set; }
        public override string ToString()
        {
            return $"{nameof(ItemCreated)}: {ItemCreated}, {nameof(Location)}: {Location}, {nameof(Requirements)}: {string.Join(", ", Requirements)}";
        }
    }
}