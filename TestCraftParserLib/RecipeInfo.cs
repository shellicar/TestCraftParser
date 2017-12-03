using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestCraftParserLib
{
    public class RecipeInfo
    {
        public RecipeInfo(string created, string location, IEnumerable<ItemRequirement> requirements)
        {
            ItemCreated = created;
            Location = location;
            Requirements = requirements.ToList();
        }


        public string ItemCreated { get; }
        public string Location { get; }

        public List<ItemRequirement> Requirements { get; }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine(Location);
            sb.AppendLine(ItemCreated);
            sb.AppendLine(string.Join(",", Requirements));

            return sb.ToString();
        }
    }
}