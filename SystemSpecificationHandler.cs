namespace Hydra;

/// <summary>
/// The SystemSpecificationHandler class is responsible for checking hardware specifications of a remote host using SSH commands.
/// </summary>
public class SystemSpecificationHandler
    {
        private readonly ISshCommandStrategy sshCommandStrategy;

        /// <summary>
        /// Initializes a new instance of the SystemSpecificationHandler class with the specified SSH command strategy.
        /// </summary>
        /// <param name="sshCommandStrategy">The SSH command strategy to use for executing commands.</param>
        public SystemSpecificationHandler(ISshCommandStrategy sshCommandStrategy)
        {
            this.sshCommandStrategy = sshCommandStrategy;
        }

        public SystemSpecification CheckHardwareSpecifications(string remoteHost, string username)
        {
            var result = new SystemSpecification();

            var command = "free -m";
            var output = sshCommandStrategy.ExecuteSshCommand(remoteHost, username, command);
            result.MemoryInfo = output; 
            
            command = "nproc";
            output = sshCommandStrategy.ExecuteSshCommand(remoteHost, username, command);
            result.ProcessorCount = output; 
            
            command = "cat /proc/cpuinfo"; 
            output = sshCommandStrategy.ExecuteSshCommand(remoteHost, username, command);
            result.ProcessorInfo = output;

            command = "uname -a";
            output = sshCommandStrategy.ExecuteSshCommand(remoteHost, username, command);
            result.NodeInfo = output; 

            command = "virsh version";
            output = sshCommandStrategy.ExecuteSshCommand(remoteHost, username, command);
            result.LibvirtInfo = output;  

            command = "ovs-vsctl version";
            output = sshCommandStrategy.ExecuteSshCommand(remoteHost, username, command);
            result.OvsInfo = output; 

            return result;
        }
}