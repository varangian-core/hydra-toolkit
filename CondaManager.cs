using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Hydra
{
    public static class CondaManager
    {
        public static void InstallEnvironment()
        {
            ProcessUtility.ExecuteCommand("conda create --name hydraEnv python=3.10");
        }

        public static void ActivateEnvironment()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Console.WriteLine("To activate the Conda environment on Windows, please use:");
                Console.WriteLine("conda activate hydraEnv");
                
                Console.WriteLine("If you are using PowerShell and haven't initialized Conda for it, please run:");
                Console.WriteLine("conda init powershell");
                Console.WriteLine("Then restart your PowerShell session before activating the environment.");
            }
            else
            {
                Console.WriteLine("To activate the Conda environment on Unix-like systems, please use:");
                Console.WriteLine("source activate hydraEnv");
            }
        }

        public static void SetupLocalDevEnvironment()
        {
            string environmentName = "hydraDev";
            string pythonVersion = "3.10";
            try
            {
                var checkEnvCommand = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? $"conda env list | findstr {environmentName}" : $"conda env list | grep {environmentName}";
                var checkEnvResult = ProcessUtility.ExecuteCommand(checkEnvCommand);
                if (!string.IsNullOrWhiteSpace(checkEnvResult))
                {
                    Console.WriteLine($"Environment '{environmentName}' already exists. Skipping creation.");
                    return;
                }

                ProcessUtility.ExecuteCommand($"conda create --name {environmentName} python={pythonVersion} -y");
                Console.WriteLine($"Environment '{environmentName}' created.");
                Console.WriteLine("To activate this environment, run the following command:");
                ActivateEnvironment();

                ProcessUtility.ExecuteCommand($"conda install --name {environmentName} flask -y");
                Console.WriteLine($"Local development environment '{environmentName}' has been set up with Python {pythonVersion}.");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }
    }
}
