using Microsoft.Extensions.Configuration;

namespace Hydra
{
    public static class PythonScriptRunner
    {
        public static void RunCommandQuery(IConfiguration configuration)
        {
            string commandQueryScriptPath = configuration["PythonScriptPaths:CommandQueryScriptPath"];
            ProcessUtility.ExecuteCommand($"python {commandQueryScriptPath}");
        }

        public static void RunLaunchCommand(IConfiguration configuration, string instructionPath)
        {
            if (string.IsNullOrEmpty(instructionPath))
            {
                Console.WriteLine("Instruction path is required.");
                return;
            }

            string launchCommandScriptPath = configuration["PythonScriptPaths:LaunchCommandScriptPath"];
            ProcessUtility.ExecuteCommand($"python {launchCommandScriptPath} {instructionPath} command_query.py");
        }
    }
}