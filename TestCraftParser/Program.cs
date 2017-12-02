using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace TestCraftParser
{
    class Program
    {
        static void Main(string[] args)
        {
            var dir = Directory.GetCurrentDirectory();
            var files = Directory.GetFiles(dir, "nwclientLog*.txt");

            using (var outfile = File.CreateText("parsed.txt"))
            {
                foreach (var f in files)
                {
                    var txt = File.ReadAllText(f);
                    var parser = new TestCraftParserLib.TextParser();
                    var recipes = parser.ParseAll(txt).Where(x => x != null);

                    foreach (var r in recipes)
                    {
                        Console.WriteLine(r);

                        outfile.WriteLine(r.Location);
                        outfile.WriteLine(r.ItemCreated);
                        foreach (var ir in r.Requirements)
                        {
                            outfile.WriteLine(ir);
                        }
                        outfile.WriteLine();
                    }
                }
            }

            Console.ReadKey(true);
        }
    }
}

