using System.Net;
using System.Security;
using Renci.SshNet;

namespace Hydra
{
    public class KvmManager : IVirtualizationManager
    {
        private SecureString password;
        public KvmManager(SecureString password)
        {
            this.password = password;
        }

        public bool InstallTools(string remoteHost, string username)
        {
            throw new NotImplementedException();
        }

        public bool CreateVirtualMachine(string remoteHost, string username, string vmName)
        {
            throw new NotImplementedException();
        }

        public bool StartVirtualMachine(string remoteHost, string username, string vmName)
        {
            throw new NotImplementedException();
        }

        public bool StopVirtualMachine(string remoteHost, string username, string vmName)
        {
            throw new NotImplementedException();
        }

        string IVirtualizationManager.ExecuteSshCommand(string remoteHost, string username, string command)
        {
            return ExecuteSshCommand(remoteHost, username, command);
        }

        private string ExecuteSshCommand(string remoteHost, string username, string command)
        {
            try
            {
                using (var sshClient = new SshClient(remoteHost, username, new NetworkCredential(string.Empty, this.password).Password))
                {
                    sshClient.Connect();
                    var sshCommand = sshClient.RunCommand(command);
                    sshClient.Disconnect();

                    return sshCommand.Result;
                }
            }
            catch (Exception ex)
            {
                // This is a basic handling, you can extend it according to your needs.
                throw new InvalidOperationException("There was a problem executing the SSH command.", ex);
            }
        }

        public bool CheckKvmSupport(string remoteHost, string username)
        {
            var command = $"egrep -c '(vmx|svm)' /proc/cpuinfo";
            var result = ExecuteSshCommand(remoteHost, username, command);

            if (int.TryParse(result, out int numOfSupportedProcesses))
            {
                return numOfSupportedProcesses > 0;
            }
            else
            {
                throw new ArgumentException($"The server response could not be parsed into an integer: {result}");
            }
        }
    }
}