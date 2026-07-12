# Validate Arguments
if ($args.Count -ne 2) {
    Write-Host "Bad argument count." -ForegroundColor Red
    Return 1
}

# Prepare backup source folder
$ONIFolder = Join-Path -Path ([environment]::getfolderpath("mydocuments")) -ChildPath "Klei/OxygenNotIncluded"
$SteamModsFolder = Join-Path -Path $ONIFolder -ChildPath "mods/Steam"
$ModFolder = Join-Path -Path $SteamModsFolder -ChildPath $args[0]

# Get minimumSupportedBuild fromy mod_info.yaml
$ModVersionFile = Join-Path -Path $ModFolder -ChildPath "mod_info.yaml"
$yaml = ""
[string[]]$content = Get-Content $ModVersionFile
foreach ($line in $content) { $yaml = $yaml + "`n" + $line }
$ModInfo = ConvertFrom-YAML $yaml
$Version = $ModInfo["minimumSupportedBuild"]

# Prepare backup destination folder
$ProjFolder = $args[1]
$ArchiveFolder = Join-Path -Path $ProjFolder -ChildPath "Archive"
if (!(Test-Path $ArchiveFolder)) {
    New-Item $ArchiveFolder -ItemType Directory
}
$DestFolder = Join-Path -Path $ArchiveFolder -ChildPath $Version
if (Test-Path $DestFolder) {
    Write-Host "Backup already exists for ${Version}." -ForegroundColor Yellow
    Return
}
New-Item $DestFolder -ItemType Directory

# Do Backup
$ModFiles = Get-ChildItem $ModFolder

foreach ($f in $ModFiles) {
    if ($f -eq "archived_versions") { Continue }
    Copy-Item $f.FullName -Destination $DestFolder -Force -Confirm:$false
}