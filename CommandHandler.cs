using System;
using Microsoft.Extensions.Configuration; 

namespace HydraToolkit
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
                default:
                    Console.WriteLine("Unknown command.");
                    break;
            }
        }
    }
}
