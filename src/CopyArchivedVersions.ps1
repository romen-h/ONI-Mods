param (
    [Parameter(Mandatory=$true)]
    [String]$ModID,
    [Parameter(Mandatory=$true)]
    [String]$SolutionDir,
    [Parameter(Mandatory=$true)]
    [String]$TargetDir,
    [Parameter(Mandatory=$true)]
    [Int]$PreviousGameVersion
)

# Prepare paths
$ArchiveDir = Join-Path -Path (Split-Path -Parent $SolutionDir) -ChildPath "archive"
$BackupModDir = Join-Path -Path $ArchiveDir -ChildPath $ModID
if (!(Test-Path $BackupModDir)) {
    Write-Host "WARNING: No build backups found for mod." -ForegroundColor Yellow
    Return
}

# Find best match for previous game version
$Backups = Get-ChildItem -Path $BackupModDir | Sort-Object -Property Name
$SelectedBackup = ""
foreach ($z in $Backups) {
    $Cmps = $z.Basename -split "_"
    [Int]$GameVersion = $Cmps[1]
    if ($GameVersion -ge $PreviousGameVersion) {
        $SelectedBackup = $z.FullName
    }
    else {
        Break
    }
}

if ($SelectedBackup -eq "") {
    Write-Host "WARNING: Could not find acceptable backup for previous_update." -ForegroundColor Yellow
    Return
}

# Unzip to archived_versions
$ArchivedVersionsDir = Join-Path -Path $TargetDir -ChildPath "archived_versions"
if (!(Test-Path $ArchivedVersionsDir)) {
    New-Item $ArchivedVersionsDir -ItemType Directory
}
$PreviousUpdateDir = Join-Path -Path $ArchivedVersionsDir -ChildPath "previous_update"
if (!(Test-Path $PreviousUpdateDir)) {
    New-Item $PreviousUpdateDir -ItemType Directory
}
Expand-Archive -Path $SelectedBackup -DestinationPath $PreviousUpdateDir -Force