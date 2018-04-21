using System;

namespace NordockCraft.Cmd
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (var prog = new ProgramRunner())
            {
                prog.Run();
            }

            Console.ReadKey(true);
        }
    }
}