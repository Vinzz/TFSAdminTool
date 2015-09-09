'Begin post build script'

# Extract current version
$version = (Get-Item $Env:TF_BUILD_BINARIESDIRECTORY + "TFSAdministrationTool.exe").VersionInfo.FileVersion

# Get installer Name
$Name = (Get-Item $Env:TF_BUILD_BINARIESDIRECTORY + "TFSAdministrationTool.msi").Name


# Compute new name
$NewName = $Name.Replace('msi', $version + '.msi')

# Rename
Rename-Item -NewName $NewName -Path $Name

'End post build script'