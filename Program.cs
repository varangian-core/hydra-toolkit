using DefaultNamespace;
using System;

class Program
{
    static async Task Main(string[] args)
    {
        bool isPort5000Open = await HealthCheck.IsPortOpen("localhost", 5000);
        Console.WriteLine($"Port 5000 is {(isPort5000Open ? "open" : "closed")}.");

        bool isPort7160Open = await HealthCheck.IsPortOpen("localhost", 7160);
        Console.WriteLine($"Port 7160 is {(isPort7160Open ? "open" : "closed")}.");
    }
}