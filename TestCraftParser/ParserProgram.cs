using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TestCraftParserLib;

namespace TestCraftParser
{
    internal class ParserProgram : IDisposable
    {
        public ParserProgram(string pattern, string filename)
        {
            Pattern = pattern;
            Parser = new FolderParser();
            Writer = new ParserWriter(filename);
            RecipeParser = new RecipeParser();
        }

        private string Pattern { get; }

        private IRecipeParser RecipeParser { get; }
        private IFolderParser Parser { get; }
        private IParserWriter Writer { get; }

        public void Dispose()
        {
            Writer?.Dispose();
        }

        public void Run()
        {
            // parse all files
            var query = from file in Parser.Files(Pattern)
                from recipe in RecipesFromFile(file)
                select recipe;

            foreach (var recipe in query)
            {
                Console.WriteLine(recipe.ToString());
                Writer.WriteLine(recipe.ToString());
            }
        }

        private IEnumerable<RecipeInfo> RecipesFromFile(FileInfo f)
        {
            return RecipeParser.ParseRecipes(File.ReadAllText(f.FullName));
        }
    }
}