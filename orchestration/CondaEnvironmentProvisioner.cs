namespace HydraToolkit.orchestration;

public class CondaEnvironmentProvisioner
{
    public static class ConddaEnvironmentProvisioner
    {
        public static void CreateEnvironment(string environmentName, string pythonVersion)
        {
            ProcessUtility.ExecuteCommand($"conda create --name {environmentName} python={pythonVersion}");
        }
        
        public static void ActivateEnvironment(string environmentName)
        {
            ProcessUtility.ExecuteCommand($"conda activate {environmentName}");
        }
    }
}