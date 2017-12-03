using System.Collections.Generic;

namespace TestCraftParserLib
{
    public interface IRecipeParser
    {
        IEnumerable<RecipeInfo> ParseRecipes(string input);
    }
}