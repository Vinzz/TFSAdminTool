write-host 'Begin post build script'

write-host ('env var: ' + $env:TF_BUILD_BINARIESDIRECTORY)

# Extract current version
$version = (Get-Item ($env:TF_BUILD_BINARIESDIRECTORY + "\TFSAdministrationTool.exe")).VersionInfo.FileVersion

# Get installer Name
$Name = (Get-Item ($env:TF_BUILD_BINARIESDIRECTORY + "\TFSAdministrationTool.msi")).FullName


# Compute new name
$NewName = $Name.Replace('msi', ($version + '.msi'))

# Rename
Rename-Item -NewName $NewName -Path $Name

$InstallFolder = "\\vtfs-drop01.ad-build.local\Install\TFS Administration Tool OAB\"

#remove old version, if any
GET-CHILDITEM $InstallFolder –recurse –include *.msi | REMOVE-ITEM

# Copy
Copy-Item $NewName -Destination $InstallFolder

write-host 'End post build script'
