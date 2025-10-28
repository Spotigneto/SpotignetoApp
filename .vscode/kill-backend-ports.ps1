param()

$ports = @(5232, 7089, 5000)
foreach ($port in $ports) {
    try {
        $lines = netstat -ano | Select-String ":$port\s"
        foreach ($line in $lines) {
            $parts = ($line.ToString() -split '\s+')
            $pid = $parts[$parts.Length - 1]
            if ($pid -match '^[0-9]+$') {
                try { Stop-Process -Id [int]$pid -Force -ErrorAction SilentlyContinue } catch {}
            }
        }
    } catch {}
}