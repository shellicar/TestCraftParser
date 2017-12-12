using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace TestCraftParserLib
{
    public class RecipeParser : IRecipeParser
    {
        private const string StartText = "[CHAT WINDOW TEXT]";

        // components should start at line 6
        private const int StartIndex = 6;

        private static string StartIdentifier => Regex.Escape(StartText);
        private static string DateIdentifier => Regex.Escape("[") + ".*?" + Regex.Escape("]");
        private static string MatchAnyGroup => "(.*)";

        public IEnumerable<RecipeInfo> ParseRecipes(FileInfo filename, string input)
        {
            var pre = PreParse(input);
            return pre.Select(GetRecipe).Where(x => x != null);
        }

        public IEnumerable<string> PreParse(string input)
        {
            var lines = SplitToLines(input);

            var list = GetStartIndexes(lines);

            foreach (var i in list.Derivatives())
            {
                var values = lines.Skip(i.Value).Take(i.Delta).ToList();

                var reg = new Regex($"{StartIdentifier} {DateIdentifier} {MatchAnyGroup}");
                var m = reg.Match(values[0]);
                if (!m.Success)
                {
                    throw new InvalidOperationException();
                }
                values[0] = m.Groups[1].Value;

                yield return string.Join(Environment.NewLine, values);
            }
        }

        // returns the indexes of 
        private static IEnumerable<int> GetStartIndexes(IEnumerable<string> lines)
        {
            // get list of the indexes of lines with CHAT WINDOW TEXT
            var enumerable = lines as string[] ?? lines.ToArray();

            var query = from pair in enumerable.Select((Value, Index) => new {Value, Index})
                where pair.Value.StartsWith(StartText)
                select pair.Index;

            return query.Concat(new[] {enumerable.Length});
        }

        private static string[] SplitToLines(string input)
        {
            return input.Split(new[] {"\r\n", "\r", "\n"}, StringSplitOptions.None);
        }

        public bool ContainsRecipe(string text)
        {
            return text.IndexOf("Components available/required...", StringComparison.Ordinal) >= 0;
        }

        public RecipeInfo GetRecipe(string text)
        {
            if (!ContainsRecipe(text))
            {
                return null;
            }

            var lines = SplitToLines(text);

            // get location name
            var location = GetLocation(lines[0]);
            // get created item name
            var created = lines[1];

            var reqs = ItemRequirements(lines);

            var recipe = new RecipeInfo(created, location, reqs, null);
            return recipe;
        }

        private static IEnumerable<ItemRequirement> ItemRequirements(string[] lines)
        {
            var toparse = lines.Skip(StartIndex)
                .Until(string.IsNullOrEmpty);

            var reqs = toparse.Select(ItemRequirement.Parse);
            return reqs;
        }

        private static string GetLocation(string input)
        {
            var locRegex = new Regex($"^({StartIdentifier} {DateIdentifier} )?{MatchAnyGroup}:");
            var location = locRegex.Match(input).Groups[2].Value;
            return location;
        }
    }
}