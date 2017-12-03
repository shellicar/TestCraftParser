using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestCraftParserLib.Tests
{
    [TestClass]
    public class RecipeParserFacts
    {
        public RecipeParserFacts()
        {
            Parser = new RecipeParser();
        }

        private RecipeParser Parser { get; }

        [TestMethod]
        public void Can_get_first_line()
        {
            var text = @"[CHAT WINDOW TEXT] [Sun Dec 03 00:29:11] [Server] You are now in a Full PVP Area.
[CHAT WINDOW TEXT] [Sun Dec 03 00:29:22] [Server] You are now in a Full PVP Area.";

            var results = Parser.PreParse(text).ToList();
            Assert.AreEqual(2, results.Count);
            var expected1 = "[Server] You are now in a Full PVP Area.";
            Assert.AreEqual(expected1, results[0]);
        }

        [TestMethod]
        public void Can_get_second_line()
        {
            var text = @"[CHAT WINDOW TEXT] [Sun Dec 03 00:29:11] [Server] You are now in a Full PVP Area.
[CHAT WINDOW TEXT] [Sun Dec 03 00:29:22] [Server] You are now in a Full PVP Area.";

            var results = Parser.PreParse(text).ToList();
            Assert.AreEqual(2, results.Count);
            var expected1 = "[Server] You are now in a Full PVP Area.";
            Assert.AreEqual(expected1, results[1]);
        }

        [TestMethod]
        public void Can_get_text_on_multi_lines()
        {
            var text =
                @"[CHAT WINDOW TEXT] [Sun Dec 03 00:41:37] Weapon Crafting Anvil - Standard: Craftable Natural Resources CNR - Drow Wars Modified. Based on CNR V3.05
Automated Batch Restriction of 35 Craftable Items per craft. Exp Restrictions, Crafting Levels 1-4 = 250xp max, Levels 5-9 = 500xp max, Levels 10-14 = 750xp max, Levels 15-19 = 1000xp max 

What would you like to make?
Weapon Crafting Anvil - Standard: [Talk] <cþ>Craftable Natural Resources <czzþ>CNR - Drow Wars Modified. Based on CNR V3.05
Automated Batch Restriction of 35 Craftable Items per craft. Exp Restrictions, Crafting Levels 1-4 = 250xp max, Levels 5-9 = 500xp max, Levels 10-14 = 750xp max, Levels 15-19 = 1000xp max </c></c>

What would you like to make? 
[CHAT WINDOW TEXT] [Sun Dec 03 00:41:38] Lianna: Copper Weapons";

            var results = Parser.PreParse(text).ToList();
            Assert.AreEqual(2, results.Count);
            var expected =
                @"Weapon Crafting Anvil - Standard: Craftable Natural Resources CNR - Drow Wars Modified. Based on CNR V3.05
Automated Batch Restriction of 35 Craftable Items per craft. Exp Restrictions, Crafting Levels 1-4 = 250xp max, Levels 5-9 = 500xp max, Levels 10-14 = 750xp max, Levels 15-19 = 1000xp max 

What would you like to make?
Weapon Crafting Anvil - Standard: [Talk] <cþ>Craftable Natural Resources <czzþ>CNR - Drow Wars Modified. Based on CNR V3.05
Automated Batch Restriction of 35 Craftable Items per craft. Exp Restrictions, Crafting Levels 1-4 = 250xp max, Levels 5-9 = 500xp max, Levels 10-14 = 750xp max, Levels 15-19 = 1000xp max </c></c>

What would you like to make? ";
            Assert.AreEqual(expected, results[0]);
        }

        [TestMethod]
        public void Test_can_find_craft_location()
        {
            var text = @"[CHAT WINDOW TEXT] [Sun Dec 03 00:41:44] Weapon Crafting Anvil - Standard: 
Copper Dwarven Waraxe

Components available/required...
---------------------------

0 of 4   Ingot of Copper
0 of 1   Small Casting Mold
0 of 1   Shaft of Hickory";

            var recipe = Parser.GetRecipe(text);
            Assert.AreEqual("Weapon Crafting Anvil - Standard", recipe.Location);
        }

        [TestMethod]
        public void Test_can_find_craft_location_with_preparse()
        {
            var text = @"[CHAT WINDOW TEXT] [Sun Dec 03 00:41:44] Weapon Crafting Anvil - Standard: 
Copper Dwarven Waraxe

Components available/required...
---------------------------

0 of 4   Ingot of Copper
0 of 1   Small Casting Mold
0 of 1   Shaft of Hickory";

            var parsed = Parser.PreParse(text);
            var recipe = Parser.GetRecipe(parsed.First());
            Assert.AreEqual("Weapon Crafting Anvil - Standard", recipe.Location);
        }

        [TestMethod]
        public void Can_get_requirement_amount()
        {
            var text = @"[CHAT WINDOW TEXT] [Sun Dec 03 00:41:44] Weapon Crafting Anvil - Standard: 
Copper Dwarven Waraxe

Components available/required...
---------------------------

0 of 4   Ingot of Copper
0 of 1   Small Casting Mold
0 of 1   Shaft of Hickory";

            var recipe = Parser.GetRecipe(text);
            Assert.AreEqual(4, recipe.Requirements.First().Amount);
        }

        [TestMethod]
        public void Can_get_requirement_name()
        {
            var text = @"[CHAT WINDOW TEXT] [Sun Dec 03 00:41:44] Weapon Crafting Anvil - Standard: 
Copper Dwarven Waraxe

Components available/required...
---------------------------

0 of 4   Ingot of Copper
0 of 1   Small Casting Mold
0 of 1   Shaft of Hickory";

            var recipe = Parser.GetRecipe(text);
            Assert.AreEqual("Ingot of Copper", recipe.Requirements.First().ItemName);
        }

        [TestMethod]
        public void Can_get_a_recipe()
        {
            var text = @"[CHAT WINDOW TEXT] [Sun Dec 03 00:41:44] Weapon Crafting Anvil - Standard: 
Copper Dwarven Waraxe

Components available/required...
---------------------------

0 of 4   Ingot of Copper
0 of 1   Small Casting Mold
0 of 1   Shaft of Hickory

Given your strength and dexterity, this recipe is trivial for you to make.
Weapon Crafting Anvil - Standard: [Talk] 
Copper Dwarven Waraxe

Components available/required...
---------------------------

0 of 4   Ingot of Copper
0 of 1   Small Casting Mold
0 of 1   Shaft of Hickory

Given your strength and dexterity, this recipe is <cþ>trivial</c> for you to make. 
";

            var results = Parser.PreParse(text).ToList();
            var recipes = results.Select(x => Parser.GetRecipe(x)).Where(x => x != null).ToList();

            Assert.AreEqual(1, recipes.Count);
        }

        [TestMethod]
        public void Can_check_if_is_recipe()
        {
            var text = @"[CHAT WINDOW TEXT] [Sun Dec 03 00:41:44] Weapon Crafting Anvil - Standard: 
Copper Dwarven Waraxe

Components available/required...
---------------------------

0 of 4   Ingot of Copper
0 of 1   Small Casting Mold
0 of 1   Shaft of Hickory

Given your strength and dexterity, this recipe is trivial for you to make.
Weapon Crafting Anvil - Standard: [Talk] 
Copper Dwarven Waraxe

Components available/required...
---------------------------

0 of 4   Ingot of Copper
0 of 1   Small Casting Mold
0 of 1   Shaft of Hickory

Given your strength and dexterity, this recipe is <cþ>trivial</c> for you to make. 
";

            var actual = Parser.ContainsRecipe(text);
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void Can_check_if_isnt_recipe()
        {
            var text = @"[CHAT WINDOW TEXT] [Sun Dec 03 00:41:44] Weapon Crafting Anvil - Standard: 
Copper Dwarven Waraxe

Given your strength and dexterity, this recipe is trivial for you to make.
Weapon Crafting Anvil - Standard: [Talk] 
Copper Dwarven Waraxe
";
            var actual = Parser.ContainsRecipe(text);
            Assert.IsFalse(actual);
        }
    }
}