if($env:PackageFeedPat) {
    # NuGet config named template.config to prevent Visual Studio from picking it up for local development
    (Get-Content .\build\template.config) `
      -replace '%PackageFeedPat%', "$env:PackageFeedPat" |
    Out-File NuGet.config
    Write-Host "Generated NuGet.config"
}
else {
    Write-Warning "There is no environment variable defined for the Package Feed"
}

Write-Host "Downloading latest nuget.exe"
Invoke-WebRequest -Uri https://dist.nuget.org/win-x86-commandline/latest/nuget.exe -OutFile .\nuget.exe

Write-Host "Performing NuGet Restore"
.\nuget.exe restore -ConfigFile .\NuGet.config

# Hack... Windows client capitalizes all environment variables wrecking the Mobile.BuildTools
$secrets = @{
    AppCenter_Android_Secret = "$env:AppCenterSecret"
}

$secrets | ConvertTo-Json | Out-File .\XamarinExplorer\secrets.json
