$ErrorActionPreference = "Stop"

$Version = "1.4.0"
$Root = Split-Path -Parent $PSScriptRoot
$Solution = Join-Path $Root "ElegiBien.slnx"
$WebProject = Join-Path $Root "ElegiBien.Web\ElegiBien.Web.csproj"
$Artifacts = Join-Path $Root "artifacts"
$PublishDir = Join-Path $Artifacts ("ElegiBien-" + $Version)
$ZipPath = Join-Path $Artifacts ("ElegiBien-" + $Version + ".zip")
$ShaPath = $ZipPath + ".sha256"

function Invoke-DotNet {
    param([Parameter(Mandatory = $true)][string[]]$Arguments)

    & dotnet @Arguments
    if ($LASTEXITCODE -ne 0) {
        throw "dotnet command failed: dotnet $($Arguments -join ' ')"
    }
}

if (-not (Get-Command dotnet -ErrorAction SilentlyContinue)) {
    throw "dotnet CLI not found."
}

if (-not (Test-Path $Solution)) {
    throw "Solution not found: $Solution"
}

if (-not (Test-Path $WebProject)) {
    throw "Web project not found: $WebProject"
}

$versionFiles = @(
    "ElegiBien.Web\ElegiBien.Web.csproj",
    "ElegiBien.Application\ElegiBien.Application.csproj",
    "ElegiBien.Domain\ElegiBien.Domain.csproj",
    "ElegiBien.Infrastructure\ElegiBien.Infrastructure.csproj",
    "ElegiBien.Tests.Unit\ElegiBien.Tests.Unit.csproj",
    "ElegiBien.Tests.Integration\ElegiBien.Tests.Integration.csproj"
)

foreach ($relativePath in $versionFiles) {
    $path = Join-Path $Root $relativePath
    $content = [System.IO.File]::ReadAllText($path)
    if (-not $content.Contains("<Version>$Version</Version>")) {
        throw "Version $Version not found in $relativePath"
    }
}

$versionText = [System.IO.File]::ReadAllText((Join-Path $Root "VERSION.txt")).Trim()
if ($versionText -ne $Version) {
    throw "VERSION.txt does not match $Version"
}

if (Test-Path $Artifacts) {
    Remove-Item $Artifacts -Recurse -Force
}
New-Item -ItemType Directory -Path $Artifacts | Out-Null

Push-Location $Root
try {
    Invoke-DotNet @("restore", $Solution)
    Invoke-DotNet @("build", $Solution, "-c", "Release", "--no-restore")
    Invoke-DotNet @("test", $Solution, "-c", "Release", "--no-build", "--no-restore")
    Invoke-DotNet @("publish", $WebProject, "-c", "Release", "--no-build", "--no-restore", "-o", $PublishDir)
}
finally {
    Pop-Location
}

$developmentSettings = Join-Path $PublishDir "appsettings.Development.json"
if (Test-Path $developmentSettings) {
    Remove-Item $developmentSettings -Force
}

$requiredFiles = @(
    (Join-Path $PublishDir "ElegiBien.Web.dll"),
    (Join-Path $PublishDir "appsettings.json")
)

foreach ($requiredFile in $requiredFiles) {
    if (-not (Test-Path $requiredFile)) {
        throw "Required publish file not found: $requiredFile"
    }
}

if (Test-Path $ZipPath) {
    Remove-Item $ZipPath -Force
}

Compress-Archive -Path (Join-Path $PublishDir "*") -DestinationPath $ZipPath -CompressionLevel Optimal

if (-not (Test-Path $ZipPath)) {
    throw "ZIP was not created: $ZipPath"
}

$hash = (Get-FileHash -Path $ZipPath -Algorithm SHA256).Hash.ToUpperInvariant()
$shaLine = $hash + "  " + [System.IO.Path]::GetFileName($ZipPath) + "`r`n"
[System.IO.File]::WriteAllText($ShaPath, $shaLine, [System.Text.Encoding]::ASCII)

$verifyHash = (Get-FileHash -Path $ZipPath -Algorithm SHA256).Hash.ToUpperInvariant()
if ($verifyHash -ne $hash) {
    throw "SHA-256 verification failed."
}

Write-Host "Release package created successfully."
Write-Host "ZIP: $ZipPath"
Write-Host "SHA256: $hash"
Write-Host "SHA file: $ShaPath"
