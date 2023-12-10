namespace Hydra
{
    public class SystemSpecification
    {
        public string MemoryInfo { get; set; }
        public string ProcessorCount { get; set; }
        public string ProcessorInfo { get; set; }
        public string NodeInfo { get; set; }
        public string LibvirtInfo { get; set; }
        public string OvsInfo { get; set; }

        // Existing properties
        public double TotalMemory { get; set; }
        public double FreeMemory { get; set; }
        public int CPUCores { get; set; }
        public bool IsHeadNode { get; set; }
        public bool HasLibvirt { get; set; }
        public bool HasOvs { get; set; }
    }
}