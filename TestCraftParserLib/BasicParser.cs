using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TestCraftParserLib
{
    public class BasicParser : IRecipeParser
    {
        private static Regex SplitRegex { get; } = new Regex(", [0-9]+", RegexOptions.Compiled);

        private static Regex NameRegex { get; } = new Regex("^(.+?)(\\(x([0-9]+)\\))? *?$", RegexOptions.Compiled);

        private static Regex IngredientRegex { get; } = new Regex("^([0-9]+) (.+)$", RegexOptions.Compiled);

        public IEnumerable<RecipeInfo> ParseRecipes(string input)
        {
            var pre = PreParse(input);
            return pre.Select(GetRecipe).Where(x => x != null);
        }

        public IEnumerable<string> PreParse(string input)
        {
            var split = input.Split(new[] {Environment.NewLine + Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);

            return split;
        }

        public RecipeInfo GetRecipe(string text)
        {
            var split = text.Split(new[] {Environment.NewLine}, StringSplitOptions.None);
            if (split.Length != 3)
            {
                throw new ArgumentException();
            }

            var location = split[0];
            int? createCount;
            var item = ParseName(split[1], out createCount);


            var split2 = SplitRegex;
            var matches = split2.Matches(split[2]);

            var idx = new List<string>();

            var index = 0;
            foreach (Match m in matches)
            {
                var thisString = split[2].Substring(index, m.Index - index);
                idx.Add(thisString);
                index = m.Index + 2;
            }
            idx.Add(split[2].Substring(index));

            var requirements = idx.Select(ParseIngredient);
            return new RecipeInfo(item, location, requirements, createCount);
        }

        private string ParseName(string line, out int? value)
        {
            var regex = NameRegex;
            var m = regex.Match(line);
            if (!m.Success)
            {
                throw new InvalidOperationException();
            }
            var name = m.Groups[1].Value;

            if (m.Groups[3].Success)
            {
                value = int.Parse(m.Groups[3].Value);
            }
            else
            {
                value = null;
            }

            return name.Trim();
        }

        private ItemRequirement ParseIngredient(string text)
        {
            var arg = text.Trim();

            var regex = IngredientRegex;
            var m = regex.Match(arg);
            if (!m.Success)
            {
                throw new ArgumentException();
            }

            var amount = int.Parse(m.Groups[1].Value);
            var name = m.Groups[2].Value;

            return new ItemRequirement(amount, name);
        }
    }
}