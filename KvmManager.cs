using System.Net;
using System.Runtime.InteropServices;
using System.Security;
using Renci.SshNet;

namespace Hydra
{
    public class KvmManager : IVirtualizationManager, ISshCommandStrategy
    {
        private SecureString password;

        public KvmManager(SecureString password)
        {
            this.password = password ?? throw new ArgumentNullException(nameof(password), "Password cannot be null.");
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

        public string ExecuteSshCommand(string remoteHost, string username, string command)
        {
            if (string.IsNullOrWhiteSpace(remoteHost))
            {
                throw new ArgumentNullException(nameof(remoteHost), "Remote Host cannot be null or whitespace.");
            }

            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentNullException(nameof(username), "Username cannot be null or whitespace.");
            }

            if (string.IsNullOrWhiteSpace(command))
            {
                throw new ArgumentNullException(nameof(command), "Command cannot be null or whitespace.");
            }

            try
            {
                using (var sshClient = new SshClient(remoteHost, username, ConvertToUNSecureString(this.password)))
                {
                    sshClient.Connect();
                    var sshCommand = sshClient.RunCommand(command);
                    sshClient.Disconnect();

                    return sshCommand.Result;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"There was a problem executing the SSH command: {command}", ex);
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
                throw new ArgumentException($"The server response, '{result}', could not be parsed into an integer. Command: '{command}'");
            }
        }
        
        private string ConvertToUNSecureString(SecureString securePassword)
        {
            if (securePassword == null)
                throw new ArgumentNullException("securePassword");

            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(securePassword);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }
    }
}