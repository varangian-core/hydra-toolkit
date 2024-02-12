using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace HydraToolkit
{
    public static class ProcessUtility
    {
        public static string ExecuteCommand(string command)
        {
            var shell = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "cmd.exe" : "/bin/bash";
            var argument = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "/c" : "-c";
            
            ProcessStartInfo processStartInfo = new ProcessStartInfo(shell, $"{argument} {command}")
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            try
            {
                using (Process process = new Process { StartInfo = processStartInfo })
                {
                    process.Start();
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();
                    process.WaitForExit();


                    if (!string.IsNullOrEmpty(error))
                    {
                        throw new InvalidOperationException($"Command execution resulted in an error: {error}");
                    }

                    return output;
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Failed to execute command '{command}'. Error: {e.Message}");
                return string.Empty;
            }
        }
    }
}