Param(
    [switch]$doBuild
)
$tags = Get-Content -Path C:\Projects\Sitecore-StandaloneShowConfig\tags.json | ConvertFrom-Json

foreach ($tag in $tags.tags) {
    $buildOptions = $tag.buildOptions
    $buildString = "docker build -t $($tag.tag)"

    foreach($buildOption in $buildOptions) {
       $buildString += " $buildOption"
    }

    $buildString += " ."
    $buildString

    if ($doBuild) {
        Invoke-Expression $buildString
        docker push $tag.tag
    }
}