namespace Hydra
{
    public interface ISshCommandStrategy
    {
        string ExecuteSshCommand(string remoteHost, string username, string command);
    }
}
