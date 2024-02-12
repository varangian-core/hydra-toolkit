using System;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Hydra
{
    public class HealthCheck
    {
        private const int ConnectionTimeout = 2000; 

        public static async Task<bool> IsPortOpen(string host, int port)
        {
            using (var client = new TcpClient())
            {
                try
                {
                    var task = client.ConnectAsync(host, port);
                    if (await Task.WhenAny(task, Task.Delay(ConnectionTimeout)) == task)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch
                {
                    return false;
                }
            }
        }

        public static async Task<bool> IsServiceResponding(string url)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    var response = await httpClient.GetAsync(url);
                    return response.IsSuccessStatusCode;
                }
                catch
                {
                    return false;
                }
            }
        }

        public static bool IsCPUUsageUnderThreshold(int threshold)
        {
            var cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            cpuCounter.NextValue();
            Task.Delay(1000).Wait(); 
            var currentUsage = cpuCounter.NextValue();
            return currentUsage < threshold;
        }
    }
}