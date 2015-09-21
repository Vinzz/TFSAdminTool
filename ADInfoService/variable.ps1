# Set some Environment variables for the LDAP resolution to work
[Environment]::SetEnvironmentVariable("LDAPAddress", "LDAP://foo.bar.com:389", "User")
[Environment]::SetEnvironmentVariable("LDAPLogin", (Read-Host -Prompt LDAPLogin), "User")
[Environment]::SetEnvironmentVariable("LDAPPassword", (Read-Host  -Prompt LDAPPassword), "User")
