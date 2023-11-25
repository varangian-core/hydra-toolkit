using System;

namespace HydraToolkit
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hydra Command Line Tool");
            while (true)
            {
                Console.Write("> ");
                string input = Console.ReadLine();

                if (string.IsNullOrEmpty(input)) continue;
                if (input.Equals("exit", StringComparison.OrdinalIgnoreCase)) break;

                CommandHandler.HandleCommand(input);
            }
        }
    }
}