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
                
                case "qodana-scan":
                    RunQodanaScan();
                    break;
                
                case "qodana-results":
                    ShowQodanaResults();
                    break;
                
                default:
                    Console.WriteLine("Unknown command.");
                    break;
            }
        }
        
        public static void RunQodanaScan()
        {
            var projectDir = _configuration["Qodana:ProjectDir"];
            var resultsDir = _configuration["Qodana:ResultsDir"];
            var linter = "jetbrains/dotnet:latest";
            var scanRunner = new QodanaScanRunner(linter, projectDir, resultsDir);
            var issues = scanRunner.RunScanAndGetIssuesWithCode();
            Console.WriteLine("Qodana scan completed.");
        }

        public static void ShowQodanaResults()
        {
            var resultDir = _configuration["Qodana:ResultsDir"];
            var resultsFilePath = Path.Combine(resultDir, "qodana-short.sarif.json");
            var issues = new QodanaResultParser().ParseResults(resultsFilePath);

            if (!issues.Any())
            {
                Console.WriteLine("No issues found.");
                return;
            }

            Console.WriteLine("Qodana results summary:");


            var issuesByType = issues.GroupBy(issue => issue.Type)
                .Select(group => new { Type = group.Key, Count = group.Count() })
                .OrderByDescending(group => group.Count);

            foreach (var issueGroup in issuesByType)
            {
                Console.Write($"{issueGroup.Type}: {issueGroup.Count} issues");
            }

            Console.WriteLine("\nDetailed issues list:");
            foreach ( var issue in issues)
            {
                Console.WriteLine($"Type: {issue.Type}, Description: {issue.Description}, File: {issue.FilePath}, Line: {issue.LineNumber}");
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