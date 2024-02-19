using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Hydra {
    public static class CondaManager {

        public static void InstallEnvironment() {
            if (!IsCondaInstalled()) {
                Console.WriteLine("Conda is not installed. Installing Miniconda...");
                InstallMiniconda();
                // After installing Miniconda, prompt the user to manually add Conda to PATH or restart their shell.
                Console.WriteLine("Miniconda installation complete. Please ensure Conda is added to your PATH.");
            } else {
                Console.WriteLine("Conda is already installed.");
            }

            // Proceed to create the Conda environment.
            CreateCondaEnvironment("hydraEnv", "3.10");
        }

        private static bool IsCondaInstalled() {
            var condaCheckCommand = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "conda --version" : "conda --version";
            var result = ProcessUtility.ExecuteCommand(condaCheckCommand);
            return !string.IsNullOrEmpty(result) && !result.Contains("not found") && !result.Contains("command not found");
        }

        private static void InstallMiniconda() {
            // Determine the OS and set the appropriate Miniconda installer.
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                // Example command for Windows. Adjust as needed for actual Miniconda installation.
                ProcessUtility.ExecuteCommand("Invoke-WebRequest -Uri https://repo.anaconda.com/miniconda/Miniconda3-latest-Windows-x86_64.exe -OutFile Miniconda3Installer.exe");
                ProcessUtility.ExecuteCommand("Start-Process -FilePath .\\Miniconda3Installer.exe -Args '/InstallationType=JustMe /AddToPath=1 /S /D=C:\\Miniconda3' -Wait");
            } else {
                // Example commands for Unix-like OS. Adjust as needed.
                ProcessUtility.ExecuteCommand("wget https://repo.anaconda.com/miniconda/Miniconda3-latest-Linux-x86_64.sh -O Miniconda3Installer.sh");
                ProcessUtility.ExecuteCommand("bash Miniconda3Installer.sh -b -p $HOME/miniconda");
            }
        }

        private static void CreateCondaEnvironment(string environmentName, string pythonVersion) {
            ProcessUtility.ExecuteCommand($"conda create --name {environmentName} python={pythonVersion} -y");
            Console.WriteLine($"Conda environment '{environmentName}' with Python {pythonVersion} has been created.");
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
