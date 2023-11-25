using System;

namespace HydraToolkit
{
    public static class CommandHandler
    {
        public static void HandleCommand(string command)
        {
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
                    PythonScriptRunner.RunCommandQuery();
                    break;
                case "run-launch-command":
                    PythonScriptRunner.RunLaunchCommand(parts.Length > 1 ? parts[1] : null);
                    break;
                default:
                    Console.WriteLine("Unknown command.");
                    break;
            }
        }
    }
}