<# Simple script that run nvidia-smi parameters #>
# Initialize a list to hold the last 10 sets of GPU data
$history = New-Object Collections.Generic.Queue[Object]

while ($true) {
    Clear-Host  # Clear the screen before each update

    $nvidiaSmiOutput = & "nvidia-smi"
    $lines = $nvidiaSmiOutput -split "`n"
    $currentGpus = @()

    for ($i = 0; $i -lt $lines.Length; $i++) {
    
        if ($lines[$i] -match "\|\s+(\d+)\s+NVIDIA\s+([\w\s]+)\s+\|") {
            $gpuId = $matches[1].Trim()
            $gpuName = $matches[2].Trim()

            if ($lines[$i + 1] -match "\|\s+\d+%.*?\|\s+(\d+MiB) \/ (\d+MiB) \|") {
                $memoryUsage = $matches[1]
                $totalMemory = $matches[2]
            }

            if ($lines[$i + 1] -match "\|\s+\d+MiB \/ \d+MiB \|.*?(\d+)%") {
                $gpuUsage = $matches[1] + "%"
            } else {
                $gpuUsage = "N/A"
            }

            $gpu = New-Object PSObject -Property @{
                ID = $gpuId
                Name = $gpuName
                MemoryUsage = $memoryUsage
                GPUUtilization = $gpuUsage
            }

            $currentGpus += $gpu
        }
    }

#We are only displaying the last 10 entries
    $history.Enqueue($currentGpus)
    while ($history.Count -gt 10) {
        $history.Dequeue()
    }

    $history | Format-Table -AutoSize

#Sampling occurs every 5 seconds
    Start-Sleep -Seconds 5  
}
