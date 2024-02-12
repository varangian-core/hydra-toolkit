namespace HydraToolkit;

public class WindowsVM : VirtualMachine
{
    public override void Start()
    {
        Console.WriteLine("Starting Windows VM...");
        Status = "Running";
    }
    
    public override void Stop()
    {
        Console.WriteLine("Stopping Windows VM...");
        Status = "Stopped";
    }
    
    public override void Restart()
    {
        Console.WriteLine("Restarting Windows VM...");
        Status = "Running";
    }
}