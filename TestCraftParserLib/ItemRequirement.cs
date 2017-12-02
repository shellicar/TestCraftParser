using System;
using System.Text.RegularExpressions;

namespace TestCraftParserLib
{
    public class ItemRequirement
    {
        public int Amount { get; set; }
        public string ItemName { get; set; }

        public override string ToString()
        {
            return $"{Amount} {ItemName}";
        }

        public static ItemRequirement Parse(string arg)
        {
            var reg = new Regex("^[0-9]+ of ([0-9]+) +(.+)$");
            var m = reg.Match(arg);
            if (!m.Success)
                throw new InvalidOperationException();

            var amount = int.Parse(m.Groups[1].Value);
            var name = m.Groups[2].Value;
            return new ItemRequirement {Amount = amount, ItemName = name};
        }
    }
}