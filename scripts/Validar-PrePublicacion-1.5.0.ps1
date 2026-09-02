$ErrorActionPreference = "Stop"

$Version = "1.5.0"
$DatabaseBaselineVersion = "1.4.0"
$Root = Split-Path -Parent $PSScriptRoot
$Artifacts = Join-Path $Root "artifacts"
$ZipPath = Join-Path $Artifacts "ElegiBien-1.5.0.zip"
$ShaPath = Join-Path $Artifacts "ElegiBien-1.5.0.zip.sha256"
$DbInstall = Join-Path $Root "database\INSTALL-1.4.0.sql"
$Temp = Join-Path $env:TEMP ("ElegiBien-Validate-" + [guid]::NewGuid().ToString("N"))

function Fail([string]$Message) {
    throw $Message
}

if (-not (Test-Path (Join-Path $Root "VERSION.txt"))) {
    Fail "VERSION.txt not found."
}

$versionText = [System.IO.File]::ReadAllText((Join-Path $Root "VERSION.txt")).Trim()
if ($versionText -ne $Version) {
    Fail "VERSION.txt does not match $Version."
}

if (-not (Test-Path $ZipPath)) {
    Fail "Release ZIP not found: $ZipPath"
}

if (-not (Test-Path $ShaPath)) {
    Fail "SHA file not found: $ShaPath"
}

$expectedLine = [System.IO.File]::ReadAllText($ShaPath).Trim()
$expectedHash = ($expectedLine -split "\s+")[0].ToUpperInvariant()
$actualHash = (Get-FileHash -Path $ZipPath -Algorithm SHA256).Hash.ToUpperInvariant()

if ($expectedHash -ne $actualHash) {
    Fail "ZIP SHA-256 does not match the .sha256 file."
}

if (-not (Test-Path $DbInstall)) {
    Fail "Database baseline installer not found: $DbInstall"
}

if ((Get-Item $DbInstall).Length -eq 0) {
    Fail "Database baseline installer is empty."
}

New-Item -ItemType Directory -Path $Temp | Out-Null

try {
    Expand-Archive -Path $ZipPath -DestinationPath $Temp -Force

    $required = @(
        "ElegiBien.Web.dll",
        "appsettings.json",
        "web.config",
        "wwwroot\js\presentation-preferences.js",
        "wwwroot\js\presentation-language.js",
        "wwwroot\css\presentation-preferences.css",
        "wwwroot\service-worker.js"
    )

    foreach ($name in $required) {
        if (-not (Test-Path (Join-Path $Temp $name))) {
            Fail "Required deployment file missing from ZIP: $name"
        }
    }

    if (Test-Path (Join-Path $Temp "appsettings.Development.json")) {
        Fail "appsettings.Development.json must not be present in the deployment ZIP."
    }

    $appsettings = [System.IO.File]::ReadAllText((Join-Path $Temp "appsettings.json"))

    if ($appsettings -match 'Server=|User ID=|Password=|Pwd=') {
        Fail "appsettings.json appears to contain a database connection value."
    }
}
finally {
    if (Test-Path $Temp) {
        Remove-Item $Temp -Recurse -Force
    }
}

Write-Host ""
Write-Host "PRE-PUBLICATION VALIDATION: OK"
Write-Host "Version: $Version"
Write-Host "ZIP SHA256: $actualHash"
Write-Host "Database schema unchanged in 1.5.0; baseline installer: $DbInstall ($DatabaseBaselineVersion)"
Write-Host ""
Write-Host "Infrastructure-dependent steps remain intentionally pending:"
Write-Host "- hosting"
Write-Host "- production SQL Server"
Write-Host "- domain/HTTPS"
Write-Host "- production contact email"
Write-Host "- real deployment and public smoke test"
