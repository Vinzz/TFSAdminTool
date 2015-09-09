
# Extract current version
$version = (Get-Item "TFSAdministrationTool.exe").VersionInfo.FileVersion

# Get installer Name
$Name = (Get-Item "TFSAdministrationTool.msi").Name


# Compute new name
$Name = $Name.Replace('msi', $version + '.msi')

# Rename
Rename-Item -NewName $Name -Path "C:\Users\Vinzz\AppData\Local\Temp\TFSAdministrationTool.msi"