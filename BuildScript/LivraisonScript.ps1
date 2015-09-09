write-host 'Begin post build script'

write-host 'env var: ' + $Env:TF_BUILD_BINARIESDIRECTORY

# Extract current version
$version = (Get-Item $Env:TF_BUILD_BINARIESDIRECTORY + "TFSAdministrationTool.exe").VersionInfo.FileVersion

# Get installer Name
$Name = (Get-Item $Env:TF_BUILD_BINARIESDIRECTORY + "TFSAdministrationTool.msi").Name


# Compute new name
$NewName = $Name.Replace('msi', $version + '.msi')

# Rename
Rename-Item -NewName $NewName -Path $Name

write-host 'End post build script'