namespace Hydra;

public interface IVirtualizationManager
{
    bool InstallTools(string remoteHost, string username);
    bool CreateVirtualMachine(string remoteHost, string username, string vmName);
    bool StartVirtualMachine(string remoteHost, string username, string vmName);
    bool StopVirtualMachine(string remoteHost, string username, string vmName);
    string ExecuteSshCommand(string remoteHost, string username, string command);
    bool CheckKvmSupport(string remoteHost, string username);
}
