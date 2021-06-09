Write-Host "Downloading Nuget Packages"
C:/tools/nuget.exe restore C:/tools/packages.config -PackagesDirectory C:/tools/packages -ConfigFile C:/tools/nuget.config

Write-Host "Copying Packages to Website"

Get-ChildItem Sitecore* -Recurse | 
    Where-Object { $_.Name -like '*.dll' } | 
    ForEach-Object { Copy-Item -Path $_.FullName -Destination C:\inetpub\wwwroot\bin\ }