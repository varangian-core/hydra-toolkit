namespace Hydra;

  public class SystemSpecificationHandler
    {
        private readonly ISshCommandStrategy sshCommandStrategy;

        public SystemSpecificationHandler(ISshCommandStrategy sshCommandStrategy)
        {
            this.sshCommandStrategy = sshCommandStrategy;
        }

        public SystemSpecification CheckHardwareSpecifications(string remoteHost, string username)
        {
            var result = new SystemSpecification();

            var command = "free -m";
            var output = sshCommandStrategy.ExecuteSshCommand(remoteHost, username, command);
            result.MemoryInfo = output; // Add this line
            
            command = "nproc";
            output = sshCommandStrategy.ExecuteSshCommand(remoteHost, username, command);
            result.ProcessorCount = output; // Add this line
            
            command = "cat /proc/cpuinfo"; 
            output = sshCommandStrategy.ExecuteSshCommand(remoteHost, username, command);
            result.ProcessorInfo = output;

            command = "uname -a";
            output = sshCommandStrategy.ExecuteSshCommand(remoteHost, username, command);
            result.NodeInfo = output; // Add this line

            command = "virsh version";
            output = sshCommandStrategy.ExecuteSshCommand(remoteHost, username, command);
            result.LibvirtInfo = output;  // Add this line

            command = "ovs-vsctl version";
            output = sshCommandStrategy.ExecuteSshCommand(remoteHost, username, command);
            result.OvsInfo = output; // Add this line

            // All node, libvirt, and OVS checks are now implemented using the "sshCommandStrategy.ExecuteSshCommand" method.

            return result;
        }
}