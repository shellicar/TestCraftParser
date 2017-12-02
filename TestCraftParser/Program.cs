using System;
using System.IO;
using System.Linq;
using TestCraftParserLib;

namespace TestCraftParser
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var dir = Directory.GetCurrentDirectory();
            var files = Directory.GetFiles(dir, "nwclientLog*.txt");

            using (var outfile = File.CreateText("parsed.txt"))
            {
                foreach (var f in files)
                {
                    var txt = File.ReadAllText(f);
                    var parser = new TextParser();
                    var recipes = parser.ParseAll(txt).Where(x => x != null);

                    foreach (var r in recipes)
                    {
                        Console.WriteLine(r);

                        outfile.WriteLine(r.Location);
                        outfile.WriteLine(r.ItemCreated);
                        outfile.WriteLine(string.Join(", ", r.Requirements));
                        outfile.WriteLine();
                    }
                }
            }

            Console.ReadKey(true);
        }
    }
}