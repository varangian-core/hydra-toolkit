using System.Net;

namespace HydraToolkit;
using System;
using Renci.SshNet;
using System.IO;
using System.Security;

public class KvmManager
{
    private SecureString password; 

    public KvmManager(SecureString password)
    {
        this.password = password; 
    }

    private string ExecuteSshCommand(string remoteHost, string username, string command)
    {
        using (var sshClient = new SshClient(remoteHost, username, new NetworkCredential(string.Empty, password).Password)) 
        {
            sshClient.Connect();
            var sshCommand = sshClient.RunCommand(command);
            sshClient.Disconnect();

            return sshCommand.Result;
        }
    }

    public bool CheckKvmSupport(string remoteHost, string username)
    {
        var command = $"egrep -c '(vmx|svm)' /proc/cpuinfo";
        var result = ExecuteSshCommand(remoteHost, username, command);

        int numOfSupportedProcesses;
        if(int.TryParse(result, out numOfSupportedProcesses))
        {
            return numOfSupportedProcesses > 0;
        }
        else
        {
            // handle improperly formatted response
            throw new Exception("Could not parse the response from the remote host.");
        }