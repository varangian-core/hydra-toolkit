using System.Diagnostics;

namespace Hydra;

public class DockerUtility
{
    public static bool IsDockerLoggedIn()
    {
        var checkLoginCommand = "docker pull hello-world";
        var result = ProcessUtility.ExecuteCommand(checkLoginCommand);
        return !result.Contains("Please login");
    }
    
    
    public static void DockerLogin(string username, string password)
    {
        var loginCommand = $"echo {password} | docker login --username {username} --password-stdin";
        ProcessUtility.ExecuteCommand(loginCommand);
    }
    
    public static bool CheckImageExists(string imageName)
    {
        var checkImageCommand = $"docker image inspect {imageName}";
        var result = ProcessUtility.ExecuteCommand(checkImageCommand);
        return !result.Contains("Error response from daemon");
    }
    
}