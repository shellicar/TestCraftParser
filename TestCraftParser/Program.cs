using System;
using log4net;

namespace TestCraftParser
{
    public static class Program
    {
        private static ILog Log { get; } = LogManager.GetLogger(typeof(Program));

        public static void Main()
        {
            Log.Info("TestCraftParser started");

            using (var program = new ParserProgram("*.txt"))
            {
                program.Run();
            }

            while (Console.KeyAvailable)
            {
                Console.ReadKey(true);
            }
            Console.Write("Press any key to continue . . . ");
            Console.ReadKey(true);
            Console.WriteLine();
        }
    }
}