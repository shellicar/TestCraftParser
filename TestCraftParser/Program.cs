using System;

namespace TestCraftParser
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            using (var program = new ParserProgram("nwclientLog*.txt", "parsed.txt"))
            {
                Console.WriteLine("Starting program");
                program.Run();
            }
            Console.WriteLine("Finished");
            while (Console.KeyAvailable)
                Console.ReadKey(true);
            Console.Write("Press any key to continue . . . ");
            Console.ReadKey(true);
            Console.WriteLine();
        }
    }
}