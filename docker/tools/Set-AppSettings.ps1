param (
    [string]$webConfig = "c:\inetpub\wwwroot\Web.config"
)

$doc = (Get-Content $webConfig) -as [Xml];
$modified = $FALSE;
$appSettingPrefix = "SITECORE_APPSETTINGS_";

Write-Host "Updating App Settings"

Get-ChildItem env:* | ForEach-Object {
    if ($_.Key.StartsWith($appSettingPrefix)) {
        $key = $_.Key.Substring($appSettingPrefix.Length);
        $appSetting = $doc.configuration.appSettings.add | Where-Object {$_.key -eq $key};
        if ($appSetting) {
            $appSetting.value = $_.Value;
            Write-Host "Replaced appSetting" $_.Key $_.Value;
            $modified = $TRUE;
        }
    }
}

if ($modified) {
    $doc.Save($webConfig);
}