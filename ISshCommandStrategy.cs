namespace Hydra
{
    /// <summary>
    /// Represents a strategy for executing SSH commands on a remote host.
    /// </summary>
    public interface ISshCommandStrategy
    {
        string ExecuteSshCommand(string remoteHost, string username, string command);
    }
}
