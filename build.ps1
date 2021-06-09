$tags = Get-Content -Path C:\Projects\Sitecore-StandaloneShowConfig\tags.json | ConvertFrom-Json

foreach ($tag in $tags.tags) {
    $buildOptions = $tag.buildOptions
    $buildOptions
    #docker build -t $tag.tag @buildOptions .

    $buildString = "docker build -t $($tag.tag)"

    foreach($buildOption in $buildOptions) {
       $buildString += " $buildOption"
    }

    $buildString += " ."
    $buildString

    Invoke-Expression $buildString
    docker push $tag.tag
}