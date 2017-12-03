using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestCraftParserLib.Tests
{
    [TestClass]
    public class EnumerableStringUntilFacts
    {
        [TestMethod]
        public void Can_get_entire_list()
        {
            var input = new[] {"asd1", "asd2", "asd3"};
            var actual = input.Until(string.IsNullOrEmpty).ToArray();

            var expected = input;
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Can_get_until_null()
        {
            var input = new[] {"asd1", "asd2", null, "asd3"};
            var actual = input.Until(string.IsNullOrEmpty).ToArray();

            var expected = new[] {"asd1", "asd2"};
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Can_get_until_empty()
        {
            var input = new[] {"asd1", "asd2", "", "asd3"};
            var actual = input.Until(string.IsNullOrEmpty).ToArray();

            var expected = new[] {"asd1", "asd2"};
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Can_get_nothing()
        {
            var input = new[] {null, "asd1", "asd2", "", "asd3"};
            var actual = input.Until(string.IsNullOrEmpty).ToArray();

            var expected = new string[0];
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}