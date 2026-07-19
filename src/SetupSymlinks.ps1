$CD = $PSScriptRoot
Set-Location -Path $CD

$DocumentsFolder = [Environment]::GetFolderPath('MyDocuments')
$ONIFolder = "${DocumentsFolder}/Klei/OxygenNotIncluded"
$ModsFolder = "${ONIFolder}/mods"
$DevFolder = "${ModsFolder}/Dev"

if (!Test-Path -Path $DevFolder) {
	New-Item -ItemType "Directory" -Path $DevFolder
}

$Projects = @(
	'DumpingSign',
	'FanTiles',
	'GermicideLamp',
	'LightOverhaul',
	'LightPack1',
	'LightPack2',
	'LogicScheduleSensor',
	'MakeDirt',
	'PipedDeodorizer',
	'PlasticDoor',
	'PlasticTiles',
	'PlasticUtilities',
	'SoggyCarpets',
	'StirlingEngine',
	'TECBlock',
	'Thresholds'
)

foreach ($Project in $Projects) {
	$SrcFolder = "${CD}/${Project}/bin"
	$DestFolder = "${DevFolder}/${Project}"
	New-Item -ItemType Junction -Path $DestFolder -Target $SrcFolder
}
