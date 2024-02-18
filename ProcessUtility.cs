using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Hydra
{
    public static class ProcessUtility
    {
        public static string ExecuteCommand(string command)
        {
            if (command.Contains("conda"))
            {
                var condaCheckResult = CheckCondaInstalled();
                if (!condaCheckResult.Item1)
                {
                    throw new InvalidOperationException($"Conda is not installed or not found in PATH. Additional info: {condaCheckResult.Item2}");
                }
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && command.Contains("grep"))
            {
                command = command.Replace("grep", "findstr");
            }

            var shell = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "cmd.exe" : "/bin/bash";
            var argument = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "/c" : "-c";
            ProcessStartInfo processStartInfo = new ProcessStartInfo(shell, $"{argument} {command}")
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = new Process { StartInfo = processStartInfo })
            {
                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();
                if (!string.IsNullOrEmpty(error) && process.ExitCode != 0)
                {
                    throw new InvalidOperationException($"Command execution resulted in an error: {error}");
                }
                return output.Trim();
            }
        }

        public static void InitializeCondaForPowerShell()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                try
                {
                    ExecuteCommand("conda init powershell");
                    Console.WriteLine("Conda has been initialized for PowerShell. Please restart your PowerShell session for changes to take effect.");
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine($"An error occurred while initializing Conda for PowerShell: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Conda initialization for PowerShell is only applicable on Windows.");
            }
        }

        private static Tuple<bool, string> CheckCondaInstalled()
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "cmd.exe" : "/bin/bash",
                Arguments = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "/c conda --version" : "-c \"conda --version\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = Process.Start(psi))
            {
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();
                if (process.ExitCode == 0)
                {
                    return Tuple.Create(true, output.Trim());
                }
                else
                {
                    return Tuple.Create(false, "Conda command not found. Ensure Conda is installed and added to PATH.");
                }
            }
        }
    }
}
