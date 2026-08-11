$ErrorActionPreference = "Stop"

$Version = "1.4.0"
$Root = Split-Path -Parent $PSScriptRoot
$VersionFile = Join-Path $Root "VERSION.txt"
$InfrastructureProject = Join-Path $Root "ElegiBien.Infrastructure\ElegiBien.Infrastructure.csproj"
$WebProject = Join-Path $Root "ElegiBien.Web\ElegiBien.Web.csproj"
$Output = Join-Path $Root "database\INSTALL-1.4.0.sql"

if (-not (Get-Command dotnet -ErrorAction SilentlyContinue)) {
    throw "dotnet CLI not found."
}

if (-not (Test-Path $VersionFile)) {
    throw "VERSION.txt not found."
}

$actualVersion = [System.IO.File]::ReadAllText($VersionFile).Trim()
if ($actualVersion -ne $Version) {
    throw "Expected version $Version but VERSION.txt contains $actualVersion."
}

if (-not (Test-Path $InfrastructureProject)) {
    throw "Infrastructure project not found: $InfrastructureProject"
}

if (-not (Test-Path $WebProject)) {
    throw "Web project not found: $WebProject"
}

Write-Host "Generating idempotent SQL migration script for ElegiBien $Version..."

& dotnet ef migrations script `
    --idempotent `
    --project $InfrastructureProject `
    --startup-project $WebProject `
    --output $Output

if ($LASTEXITCODE -ne 0) {
    throw "dotnet ef migrations script failed."
}

if (-not (Test-Path $Output)) {
    throw "Database install script was not created: $Output"
}

$file = Get-Item $Output
if ($file.Length -eq 0) {
    throw "Database install script is empty."
}

Write-Host "Database install script created successfully."
Write-Host "File: $Output"
Write-Host "Size: $($file.Length) bytes"
