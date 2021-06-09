C:\tools\Set-AppSettings.ps1
C:\tools\Get-NugetResources.ps1

Write-Host "Starting IIS"

C:\ServiceMonitor.exe w3svc

Write-Host "Ready!"