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
            ProcessUtility.ExecuteCommand("conda activate hydraEnv");
        }

        public static void SetupLocalDevEnvironment()
        {
            string environmentName = "hydraDev";
            string pythonVersion = "3.10";
            ProcessUtility.ExecuteCommand($"conda create --name {environmentName} python={pythonVersion} -y");
            Console.WriteLine($"Please activate the '{environmentName}' environment manually using: conda activate {environmentName}");
            ProcessUtility.ExecuteCommand($"conda install --name {environmentName} flask -y");
            Console.WriteLine($"Local development environment '{environmentName}' has been set up with Python {pythonVersion}.");
        }
    }
}