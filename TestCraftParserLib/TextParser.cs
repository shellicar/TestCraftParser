using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TestCraftParserLib
{
    public class TextParser
    {
        public IEnumerable<RecipeInfo> ParseAll(string input)
        {
            var pre = PreParse(input);
            return pre.Select(GetRecipe);
        }

        public IEnumerable<string> PreParse(string input)
        {
            List<string> currentLines = new List<string>();

            string[] lines = input.Split(
                new[] { "\r\n", "\r", "\n" },
                StringSplitOptions.None
            );

            Regex reg = new Regex("^\\[CHAT WINDOW TEXT\\] \\[.*?\\] (.*)$");

            foreach (var l in lines)
            {
                var m = reg.Match(l);
                if (m.Success)
                {
                    if (currentLines.Count > 0)
                    {
                        yield return string.Join(Environment.NewLine, currentLines);
                        currentLines.Clear();
                    }
                }
                    currentLines.Add(l);
            }

            if (currentLines.Count > 0)
            {
                yield return string.Join(Environment.NewLine, currentLines);
            }
        }

        public bool ContainsRecipe(string text)
        {
            return text.IndexOf("Components available/required...") >= 0;
        }

        public RecipeInfo GetRecipe(string text)
        {
            if (!ContainsRecipe(text)) return null;

            string[] lines = text.Split(
                new[] { "\r\n", "\r", "\n" },
                StringSplitOptions.None
            );

            Regex locRegex = new Regex("^\\[CHAT WINDOW TEXT\\] \\[[^\\]]*\\] (.*):");

            var preLoc = lines[0];
            var created = lines[1];

            var m = locRegex.Match(preLoc);

            var location = m.Groups[1].Value;



            int idx = 6;

            List<string> requirements = new List<string>();
            while (idx < lines.Length && !string.IsNullOrEmpty(lines[idx]))
            {
                requirements.Add(lines[idx++]);
            }

            var reqs = requirements.Select(ItemRequirement.Parse);

            var recipe = new RecipeInfo { ItemCreated = created, Location = location, Requirements = reqs.ToList() };
            return recipe;
        }
    }
}
