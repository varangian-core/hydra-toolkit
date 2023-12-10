using System;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace HydraToolkit
{
    class Program
    {
        public static IConfiguration Configuration { get; set; }

        static void Main(string[] args)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            
            Console.WriteLine("Hydra Command Line Tool");
            while (true)
            {
                Console.Write("> ");
                string input = Console.ReadLine();

                if (string.IsNullOrEmpty(input)) continue;
                if (input.Equals("exit", StringComparison.OrdinalIgnoreCase)) break;

                CommandHandler.HandleCommand(input, Configuration); 
            }
        }
    }
}