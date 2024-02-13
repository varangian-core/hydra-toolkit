using System.Diagnostics;
using System.Net.Sockets;
using System.Text.Json.Serialization;
using Hydra;
using Newtonsoft.Json;

namespace HydraToolkit.orchestration;

public class VirtualMachineProvisioner
{
    private readonly string metadataPath = "metadata.json"; //need to figure out our conf file
    private class  MetaData
    {
        public VmMetadata[] Vms { get; set; }
    }
    
    private class VmMetadata
    {
        public string Name { get; set; }
        public bool isLinux { get; set; }
        public bool isWindows { get; set; }
        public bool SlurmConfig { get; set; }
    }
    
    public void ProvisionLinuxVM(string vmName, string image = "22.04", int cpus = 2 , string memory = "4G", string disk = "20G")
    {
        try
        {
            Console.WriteLine($"Launching VM '{vmName}' with Ubuntu {image}...");
            ProcessUtility.ExecuteCommand($"multipass launch -n {vmName} -c {cpus} -m {memory} -d {disk} {image}");

            Console.WriteLine($"Linux VM '{vmName}' has been successfully provisioned.");
            CheckAndTagLinuxVM(vmName);
            ExtractAndSaveSlurmConfig(vmName);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to provision Linux VM '{vmName}'.");
        }
    }

    private void CheckAndTagLinuxVM(string vmName)
    {
        var procOutput = ProcessUtility.ExecuteCommand($"multipass exec {vmName} -- cat /proc/version");
        if (procOutput.Contains("Linux"))
        {
            Console.WriteLine($"{vmName} is confirmed to be a Linux VM");
            TagVmAsLinux(vmName);
        }
        else
        {
            Console.WriteLine($"Unable to confirm {vmName} is a Linux VM");
        }
    }

    private void TagVmAsLinux(string vmName)
    {
        var metadata = LoadMetadata();
        var vmMetadata = metadata.Vms.FirstOrDefault(vm => vm.Name == vmName) ?? new VmMetadata() { Name = vmName };
        vmMetadata.isLinux = true;


        if (metadata.Vms.All(vm => vm.Name != vmName))
        {
            var vmsList = metadata.Vms.ToList();
            vmsList.Add(vmMetadata);
            metadata.Vms = vmsList.ToArray();
        }
        SaveMetadata(metadata);
    }
    
    private void ExtractAndSaveSlurmConfig(string vmName)
    {
        var slurmPath = ProcessUtility.ExecuteCommand($"multipass exec {vmName} -- find/ -type f -name slurm.conf 2>/dev/null");
        if (!string.IsNullOrEmpty(slurmPath))
        {
            var slurmConfig = ProcessUtility.ExecuteCommand($"multipass exec {vmName} -- cat {slurmPath}");
            UpdateSlurmConfigMetadata(vmName, slurmConfig);
        }
    }
    
    private void  UpdateSlurmConfigMetadata(string vmName, string slurmConfig)
    {
        var metadata = LoadMetadata();
        var vmMetadata = metadata.Vms.FirstOrDefault(vm => vm.Name == vmName) ?? new VmMetadata() { Name = vmName };
        vmMetadata.SlurmConfig = !string.IsNullOrEmpty(slurmConfig);

        if (metadata.Vms.All(vm => vm.Name != vmName))
        {
            var vmsList = metadata.Vms.ToList();
            vmsList.Add(vmMetadata);
            metadata.Vms = vmsList.ToArray();
        }
        
        SaveMetadata(metadata);
    }

    private void SaveMetadata(MetaData metadata)
    {
        File.WriteAllText(metadataPath, JsonConvert.SerializeObject(metadata, Formatting.Indented));
    }

    private MetaData LoadMetadata()
    {
        if (!File.Exists(metadataPath))
        {
            return new MetaData { Vms = new VmMetadata[] { } };
        }
        return JsonConvert.DeserializeObject<MetaData>(File.ReadAllText(metadataPath));
    }


    //TODO: separate Windows method provisioning. Options: Hyper-V, vSphere, vManage
    public void ProvisionWindowsVM()
    {
        throw new Exception("Not implemented");
    }

}