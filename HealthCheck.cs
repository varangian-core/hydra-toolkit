using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace DefaultNamespace
{
    public class HealthCheck
    {
        private const int ConnectionTimeout = 5000; 

        public static async Task<bool> IsPortOpen(string host, int port)
        {
            using (var client = new TcpClient())
            {
                try
                {
                    var task = client.ConnectAsync(host, port);
                    if (await Task.WhenAny(task, Task.Delay(ConnectionTimeout)) == task)
                    {
                        // Connection successful
                        return true;
                    }
                    else
                    {
                        // Connection timed out
                        return false;
                    }
                }
                catch
                {
                    // Connection failed
                    return false;
                }
            }
        }
    }
}