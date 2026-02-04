# PowerShell Setup Script for Gangasiri Tea Factory Billing

$projectPath = "D:\GangaSiri Tea Factory Billing\GangasiriTeaFactoryBilling"
$projectName = "GangasiriTeaFactoryBilling"
$outputPath = Join-Path $projectPath "bin\Release\net8.0-windows\publish\win-x64"
$setupDir = Join-Path $projectPath "Setup"
$innoSetupScript = Join-Path $projectPath "setup.iss"

Write-Host "Starting Setup Creation..." -ForegroundColor Cyan

# 1. Clean previous build
Write-Host "Cleaning previous build..."
if (Test-Path $outputPath) { Remove-Item $outputPath -Recurse -Force }
if (Test-Path $setupDir) { Remove-Item $setupDir -Recurse -Force }
New-Item -ItemType Directory -Force -Path $setupDir | Out-Null

# 2. Publish Application
Write-Host "Publishing Application..."
Set-Location $projectPath
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o $outputPath

if ($LASTEXITCODE -ne 0) {
    Write-Host "Build Failed!" -ForegroundColor Red
    exit
}

# 3. Compile Inno Setup Script
Write-Host "Looking for Inno Setup Compiler..."
$innoSetupPath = "C:\Program Files (x86)\Inno Setup 6\ISCC.exe"
if (Test-Path $innoSetupPath) {
    Write-Host "Inno Setup Compiler found. Creating GUI installer..." -ForegroundColor Green
    & $innoSetupPath $innoSetupScript
    Write-Host "Installer created successfully in the 'Setup' folder." -ForegroundColor Green
} else {
    Write-Host "Inno Setup Compiler not found. Creating a zip archive instead." -ForegroundColor Yellow
    $zipPath = Join-Path $setupDir "GangasiriBilling_Setup.zip"
    Compress-Archive -Path "$outputPath\*" -DestinationPath $zipPath -Force
    Write-Host "Zip archive created in the 'Setup' folder." -ForegroundColor Yellow
}

Write-Host "Setup creation process finished." -ForegroundColor Cyan