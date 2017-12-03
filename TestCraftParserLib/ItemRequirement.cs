using System;
using System.Text.RegularExpressions;

namespace TestCraftParserLib
{
    public class ItemRequirement
    {
        private ItemRequirement(int amount, string name)
        {
            Amount = amount;
            ItemName = name;
        }

        public int Amount { get; }
        public string ItemName { get; }

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
            return new ItemRequirement(amount, name);
        }
    }
}