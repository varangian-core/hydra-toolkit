namespace HydraToolkit;

public abstract class VirtualMachine
{
    public string Name { get; set; }
    public string Status { get; protected set; }

    public abstract void Start();
    public abstract void Stop();
    public abstract void Restart();
}
