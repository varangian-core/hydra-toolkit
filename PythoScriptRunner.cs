namespace HydraToolkit
{
    public static class PythonScriptRunner
    {
        public static void RunCommandQuery()
        {
            ProcessUtility.ExecuteCommand(@"python C:\path\to\command_query.py");
        }

        public static void RunLaunchCommand(string instructionPath)
        {
            if (string.IsNullOrEmpty(instructionPath))
            {
                Console.WriteLine("Instruction path is required.");
                return;
            }
            ProcessUtility.ExecuteCommand($@"python C:\path\to\launch_command.py {instructionPath} command_query.py");
        }
    }
}