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

    }
}