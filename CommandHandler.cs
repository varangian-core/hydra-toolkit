using System;
using Microsoft.Extensions.Configuration; 


using System;
using Microsoft.Extensions.Configuration;

namespace Hydra
{
    public static class CommandHandler
    {
        private static IConfiguration _configuration;

        public static void HandleCommand(string command, IConfiguration configuration)
        {
            _configuration = configuration;
            string[] parts = command.Split(' ');
            switch (parts[0].ToLower())
            {
                case "help":
                    ShowHelp();
                    break;
                case "conda-install":
                    CondaManager.InstallEnvironment();
                    break;
                case "conda-activate":
                    CondaManager.ActivateEnvironment();
                    break;
                case "run-command-query":
                    PythonScriptRunner.RunCommandQuery(_configuration);
                    break;
                case "run-launch-command":
                    string instructionPath = parts.Length > 1 ? parts[1] : null;
                    PythonScriptRunner.RunLaunchCommand(_configuration, instructionPath);
                    break;
                case "setup-localdev": 
                    CondaManager.SetupLocalDevEnvironment();
                    break;
                default:
                    Console.WriteLine("Unknown command.");
                    break;
            }
        }

        private static void ShowHelp()
        {
            Console.WriteLine("Hydra Command Line Tool - Help");
            Console.WriteLine("Commands:");
            Console.WriteLine(" help - Shows this help information.");
            Console.WriteLine(" conda-install - Installs the environment using Conda.");
            Console.WriteLine(" conda-activate - Activates the Conda environment.");
            Console.WriteLine(" run-command-query - Runs a Python command query script.");
            Console.WriteLine(" run-launch-command [instructionPath] - Runs a Python launch command script with the specified instruction path.");
            Console.WriteLine(" setup-localdev - Sets up the local development environment with Anaconda."); // Add description for the new command
            Console.WriteLine(" exit - Exits the application.");
        }
    }
}
