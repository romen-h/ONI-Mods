param(
    [Parameter(Mandatory=$true)]
    [String]$ProjectName
)

# Prepare paths
$SolutionDir = Get-Location
$ProjectDir = Join-Path -Path $SolutionDir -ChildPath $ProjectName
$ArchiveDir = Join-Path -Path (Split-Path -Parent $SolutionDir) -ChildPath "archive"
$TargetDir = Join-Path -Path $ProjectDir -ChildPath "bin"
$ModFile = Join-Path -Path $TargetDir -ChildPath "mod.yaml"
$ModInfoFile = Join-Path -Path $TargetDir -ChildPath "mod_info.yaml"

# Parse mod.yaml
if (!(Test-Path $ModFile)) {
    Write-Host "ERROR: mod.yaml not found" -ForegroundColor Red
    Return
}
$yaml = ""
[string[]]$content = Get-Content $ModFile
foreach ($line in $content) { $yaml = $yaml + "`n" + $line }
$ModYaml = ConvertFrom-YAML $yaml

# Parse mod_info.yaml
if (!(Test-Path $ModInfoFile)) {
    Write-Host "ERROR: mod_info.yaml not found" -ForegroundColor Red
    Return
}
$yaml = ""
[string[]]$content = Get-Content $ModInfoFile
foreach ($line in $content) { $yaml = $yaml + "`n" + $line }
$ModInfoYaml = ConvertFrom-YAML $yaml

# Set mod variables
$ModID = $ModYaml["staticID"]
#$ModVersion = $ModInfoYaml["version"]
$GameVersion = $ModInfoYaml["minimumSupportedBuild"]

# Read assembly Product Version
$TargetFile = Join-Path -Path $TargetDir -ChildPath "${ModID}.dll"
if (!(Test-Path $TargetFile)) {
    Write-Host "ERROR: Mod DLL not found" -ForegroundColor Red
    Return
}
$Assembly = Get-Item $TargetFile
$ModVersion = $Assembly.VersionInfo.ProductVersion

# Make mod archive
$BackupName = "${ModVersion}_${GameVersion}"

$BackupModDir = Join-Path -Path $ArchiveDir -ChildPath $ModID
if (!(Test-Path $BackupModDir)) {
    New-Item $BackupModDir -ItemType Directory
}

$BackupFile = Join-Path -Path $BackupModDir -ChildPath "${BackupName}.zip"
if (Test-Path $BackupFile) {
    Write-Host "release already exists" -ForegroundColor Yellow
    Return
}
Compress-Archive -Path $TargetDir\* -DestinationPath $BackupFile

# Launch ONI Mod Uploader
Invoke-Expression "X:\Steam\steamapps\common\OxygenNotIncludedUploader\OniUploader64.exe"