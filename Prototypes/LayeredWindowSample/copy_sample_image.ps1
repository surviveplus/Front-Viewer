# PowerShell script to copy sample.png to build output directories

Write-Host "Copying sample.png to build output directories..." -ForegroundColor Green

# Copy to AppNet8
$net8Path = "AppNet8\bin\Debug\net8.0"
if (!(Test-Path $net8Path)) {
    New-Item -ItemType Directory -Path $net8Path -Force | Out-Null
}
Copy-Item "sample.png" -Destination $net8Path -Force
Write-Host "Copied to AppNet8" -ForegroundColor Yellow

# Copy to AppDotNetFramework48
$net48Path = "AppDotNetFramework48\bin\Debug\net48"
if (!(Test-Path $net48Path)) {
    New-Item -ItemType Directory -Path $net48Path -Force | Out-Null
}
Copy-Item "sample.png" -Destination $net48Path -Force
Write-Host "Copied to AppDotNetFramework48" -ForegroundColor Yellow

Write-Host "Done!" -ForegroundColor Green
Read-Host "Press Enter to continue"