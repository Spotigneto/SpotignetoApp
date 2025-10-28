# Script per il cleanup automatico delle porte occupate da processi dotnet orfani
# Uso: .\cleanup-ports.ps1

Write-Host "Controllo processi dotnet.exe attivi..." -ForegroundColor Yellow

# Ottieni tutti i processi dotnet.exe
$dotnetProcesses = Get-Process -Name "dotnet" -ErrorAction SilentlyContinue

if ($dotnetProcesses) {
    Write-Host "Processi dotnet.exe trovati:" -ForegroundColor Cyan
    $dotnetProcesses | ForEach-Object {
        Write-Host "  - PID: $($_.Id) | Memoria: $([math]::Round($_.WorkingSet64/1MB, 2)) MB" -ForegroundColor White
    }
    
    # Controlla le porte specifiche del progetto
    $targetPorts = @(5000, 5001, 5232, 7007)
    $portsInUse = @()
    
    foreach ($port in $targetPorts) {
        $netstatOutput = netstat -ano | Select-String ":$port "
        if ($netstatOutput) {
            $portsInUse += $port
            Write-Host "Porta $port e' occupata" -ForegroundColor Red
            
            # Estrai il PID dalla riga netstat
            $netstatOutput | ForEach-Object {
                if ($_ -match "\s+(\d+)$") {
                    $pid = $matches[1]
                    $process = Get-Process -Id $pid -ErrorAction SilentlyContinue
                    if ($process -and $process.ProcessName -eq "dotnet") {
                        Write-Host "  Processo dotnet.exe (PID: $pid) sta usando la porta $port" -ForegroundColor Yellow
                    }
                }
            }
        }
    }
    
    if ($portsInUse.Count -gt 0) {
        Write-Host ""
        $response = Read-Host "Vuoi terminare i processi dotnet.exe che occupano le porte del progetto? (y/N)"
        
        if ($response -eq 'y' -or $response -eq 'Y') {
            foreach ($port in $portsInUse) {
                $netstatOutput = netstat -ano | Select-String ":$port.*LISTENING"
                $netstatOutput | ForEach-Object {
                    if ($_ -match "\s+(\d+)$") {
                        $pid = $matches[1]
                        $process = Get-Process -Id $pid -ErrorAction SilentlyContinue
                        if ($process -and $process.ProcessName -eq "dotnet") {
                            try {
                                Stop-Process -Id $pid -Force
                                Write-Host "Terminato processo dotnet.exe (PID: $pid) che occupava la porta $port" -ForegroundColor Green
                            }
                            catch {
                                Write-Host "Errore nel terminare il processo PID $pid : $($_.Exception.Message)" -ForegroundColor Red
                            }
                        }
                    }
                }
            }
            
            # Verifica finale
            Start-Sleep -Seconds 2
            Write-Host ""
            Write-Host "Verifica finale delle porte..." -ForegroundColor Yellow
            
            $stillInUse = @()
            foreach ($port in $targetPorts) {
                $netstatOutput = netstat -ano | Select-String ":$port.*LISTENING"
                if ($netstatOutput) {
                    $stillInUse += $port
                }
            }
            
            if ($stillInUse.Count -eq 0) {
                Write-Host "Tutte le porte del progetto sono ora libere!" -ForegroundColor Green
            } else {
                Write-Host "Le seguenti porte sono ancora occupate: $($stillInUse -join ', ')" -ForegroundColor Yellow
            }
        } else {
            Write-Host "Nessun processo terminato." -ForegroundColor Blue
        }
    } else {
        Write-Host "Nessuna porta del progetto e' occupata da processi dotnet.exe" -ForegroundColor Green
    }
} else {
    Write-Host "Nessun processo dotnet.exe attivo trovato" -ForegroundColor Green
}

Write-Host ""
Write-Host "Riepilogo porte del progetto:" -ForegroundColor Cyan
Write-Host "  - Backend:  http://localhost:5232" -ForegroundColor White
Write-Host "  - Frontend: http://localhost:5001, https://localhost:7007" -ForegroundColor White
Write-Host ""
Write-Host "Suggerimento: Esegui questo script prima di avviare i progetti per evitare conflitti di porte." -ForegroundColor Blue