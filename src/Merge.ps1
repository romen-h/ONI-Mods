param
(
    [Parameter(Mandatory=$true)]
    [String]$SolutionDir,
    [Parameter(Mandatory=$true)]
    [String]$TargetDir,
    [Parameter(Mandatory=$true)]
    [String]$MainAssemblyPath,
    [Parameter(Mandatory=$true)]
    [String]$LibsDir
)

# Variables
$ILRepack = Join-Path -Path (Split-Path -Parent $SolutionDir) -ChildPath "tools\ILRepack.exe"
$MainAssembly = Get-Item $MainAssemblyPath
$Version = $MainAssembly.VersionInfo.FileVersion
$Debug = $false

# Run ILRepack
$Options = "/ver:${Version} /targetplatform:v4 /wildcards /lib:${LibsDir}"

if ($Debug) {
    $Options += " /log:${TargetDir}mergelog.txt /verbose"
}
Invoke-Expression "${ILRepack} ${Options} /out:${MainAssemblyPath} ${MainAssemblyPath} ${TargetDir}*.dll"

# Clean up merged libs
$FilesToDelete = Get-ChildItem -Path $TargetDir* -Include *.dll -Exclude $MainAssembly.Name
foreach ($f in $FilesToDelete) {
    Remove-Item -Path $f.Fullname -Force -Confirm:$false
}