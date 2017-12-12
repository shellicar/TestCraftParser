using System.Collections.Generic;
using System.IO;

namespace TestCraftParserLib
{
    public interface IRecipeParser
    {
        IEnumerable<RecipeInfo> ParseRecipes(FileInfo filename, string input);
    }
}