# Setup-Environment.ps1
# Configures the environment for TFX50 development in Visual Studio 2022

Write-Host "Starting TFX50 Environment Setup..." -ForegroundColor Cyan

# 1. Check .NET 9 SDK
Write-Host "`n[1/4] Checking .NET 9 SDK..." -ForegroundColor Yellow
$dotnetVersion = dotnet --list-sdks | Select-String "9.0"
if ($dotnetVersion) {
    Write-Host "   [OK] .NET 9 SDK found." -ForegroundColor Green
} else {
    Write-Host "   [ERROR] .NET 9 SDK not found. Please install it from https://dotnet.microsoft.com/download/dotnet/9.0" -ForegroundColor Red
    exit 1
}

# 2. Check Node.js and Yarn
Write-Host "`n[2/4] Checking Node.js and Yarn..." -ForegroundColor Yellow
try {
    $nodeVersion = node --version
    Write-Host "   [OK] Node.js found: $nodeVersion" -ForegroundColor Green
} catch {
    Write-Host "   [ERROR] Node.js not found. Please install it." -ForegroundColor Red
    exit 1
}

try {
    $yarnVersion = yarn --version
    Write-Host "   [OK] Yarn found: $yarnVersion" -ForegroundColor Green
} catch {
    Write-Host "   [WARN] Yarn not found. Installing via npm..." -ForegroundColor Yellow
    npm install -g yarn
}

# 3. Install Frontend Dependencies
Write-Host "`n[3/4] Installing Frontend Dependencies (Core/TFX.Core.UI)..." -ForegroundColor Yellow
$uiPath = Join-Path $PSScriptRoot "Core\TFX.Core.UI"
if (Test-Path $uiPath) {
    Push-Location $uiPath
    yarn install
    Pop-Location
    Write-Host "   [OK] Dependencies installed." -ForegroundColor Green
} else {
    Write-Host "   [ERROR] UI Directory not found: $uiPath" -ForegroundColor Red
}

# 4. Check/Install DASE4VS Extension
Write-Host "`n[4/4] Checking DASE4VS Extension..." -ForegroundColor Yellow
$vsixPath = Join-Path $PSScriptRoot "releases\download\v1.0.0\DASE4VS.vsix"
if (Test-Path $vsixPath) {
    Write-Host "   [INFO] DASE4VS.vsix found at: $vsixPath" -ForegroundColor Cyan
    Write-Host "   To ensure the best experience, please ensure this extension is installed in Visual Studio 2022."
    
    $choice = Read-Host "   Do you want to launch the VSIX installer now? (Y/N)"
    if ($choice -eq 'Y' -or $choice -eq 'y') {
        Start-Process $vsixPath
        Write-Host "   [INFO] Installer launched. Please follow the prompts." -ForegroundColor Green
    }
} else {
    Write-Host "   [WARN] DASE4VS.vsix not found in releases folder." -ForegroundColor Yellow
}

Write-Host "`nSetup Complete! You can now open TFX50.sln in Visual Studio 2022." -ForegroundColor Green
Write-Host "Press any key to exit..."
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
