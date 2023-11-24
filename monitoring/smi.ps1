<# Simple script that run nvidia-smi parameters #>
# Initialize a list to hold the last 10 sets of GPU data
$history = New-Object Collections.Generic.Queue[Object]

while ($true) {
    Clear-Host  # Clear the screen before each update

    # Run nvidia-smi and capture output
    $nvidiaSmiOutput = & "nvidia-smi"

    # Split the output into lines
    $lines = $nvidiaSmiOutput -split "`n"

    # Initialize an array to hold current GPU info
    $currentGpus = @()

    for ($i = 0; $i -lt $lines.Length; $i++) {
        # Match the line with GPU ID and Name
        if ($lines[$i] -match "\|\s+(\d+)\s+NVIDIA\s+([\w\s]+)\s+\|") {
            $gpuId = $matches[1].Trim()
            $gpuName = $matches[2].Trim()

            # Assuming the next line contains Memory Usage and GPU Utilization information
            if ($lines[$i + 1] -match "\|\s+\d+%.*?\|\s+(\d+MiB) \/ (\d+MiB) \|") {
                $memoryUsage = $matches[1]
                $totalMemory = $matches[2]
            }

            if ($lines[$i + 1] -match "\|\s+\d+MiB \/ \d+MiB \|.*?(\d+)%") {
                $gpuUsage = $matches[1] + "%"
            } else {
                $gpuUsage = "N/A"
            }

            # Create an object to hold GPU info
            $gpu = New-Object PSObject -Property @{
                ID = $gpuId
                Name = $gpuName
                MemoryUsage = $memoryUsage
                GPUUtilization = $gpuUsage
            }

            # Add the GPU object to the current array
            $currentGpus += $gpu
        }
    }

    # Add current GPU data to history and keep only the last 10 entries
    $history.Enqueue($currentGpus)
    while ($history.Count -gt 10) {
        $history.Dequeue()
    }

    # Display the last 10 entries
    $history | Format-Table -AutoSize

    Start-Sleep -Seconds 5  # Wait for 5 seconds before refreshing
}
