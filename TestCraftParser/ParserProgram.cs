using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using log4net;
using NordockCraft.Data.Exceptions;
using NordockCraft.Data.Service;
using TestCraftParserLib;

namespace TestCraftParser
{
    internal class ParserProgram : IDisposable
    {
        public ParserProgram(string pattern)
        {
            Pattern = pattern;
            Parser = new FolderParser();
            RecipeParser = new RecipeParser();
            BasicParser = new BasicParser();

            Provider = new ServiceProvider();
        }

        private static ILog Log { get; } = LogManager.GetLogger(typeof(ParserProgram));

        public IServiceProvider Provider { get; }

        private string Pattern { get; }

        private IRecipeParser RecipeParser { get; }
        private IRecipeParser BasicParser { get; }
        private IFolderParser Parser { get; }

        public void Dispose()
        {
            Provider?.Dispose();
        }

        public void Run()
        {
            Provider.CreateDatabase();

            // parse all files
            var query = from file in Parser.Files(Pattern)
                from recipe in RecipesFromFile(file)
                select recipe;

            using (var service = Provider.Service)
            {
                Log.Info("Parsing all text files");
                var recipes = query.ToList();

                Log.Info("Done parsing");
                foreach (var recipe in recipes)
                {
                    Log.Debug($"Updating recipe: {recipe.ItemCreated} - {recipe.Location}");
                    AddRecipe(service, recipe);
                }
            }
        }

        private void AddRecipe(ICreateRecipeService service, RecipeInfo recipe)
        {
            try
            {
                service.UpdateRecipeWithLocation(recipe.ItemCreated, recipe.Location, recipe.CreateCount, recipe.Requirements.Select(CreateIngredient));
            }
            catch (RecipeLocationAlreadyExistsError)
            {
            }
        }

        private IngredientDto CreateIngredient(ItemRequirement arg)
        {
            return new IngredientDto {Amount = arg.Amount, ItemName = arg.ItemName};
        }


        private IEnumerable<RecipeInfo> RecipesFromFile(FileInfo f)
        {
            Log.Debug($"Parsing file: {f.Name}");

            //var initial = RecipeParser.ParseRecipes(File.ReadAllText(f.FullName));
            return BasicParser.ParseRecipes(File.ReadAllText(f.FullName));
        }
    }
}