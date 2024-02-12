namespace HydraToolkit;

public class LinuxVM : VirtualMachine
{
    public override void Start()
    {
        Console.WriteLine("Starting Linux VM...");
        Status = "Running";
    }
    
    public override void Stop()
    {
        Console.WriteLine("Stopping Linux VM...");
        Status = "Stopped";
    }
    
    public override void Restart()
    {
        Console.WriteLine("Restarting Linux VM...");
        Status = "Running";
    }
}