using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADInfosService
{
    public static class ADService
    {
        /// <summary>
        /// The main Directory Entry
        /// </summary>
        private static DirectoryEntry de = null;

        public static List<UserInfo> GetADInfos(string name)
        {
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("LDAPAddress")))
            {
                throw new Exception("No external LDAP resolution without an LDAP address");
            };

            if (de == null)
            {
                if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("LDAPLogin")))
                {
                    de = new DirectoryEntry(Environment.GetEnvironmentVariable("LDAPAddress"));
                }
                else
                {
                    string password = Environment.GetEnvironmentVariable("LDAPPassword");
                    de = new DirectoryEntry(Environment.GetEnvironmentVariable("LDAPAddress"), Environment.GetEnvironmentVariable("LDAPLogin"), password);
                }
            }

            DirectorySearcher ds = new DirectorySearcher(de);
            ds.PropertiesToLoad.Add("name");
            ds.PropertiesToLoad.Add("sn");
            ds.PropertiesToLoad.Add("ou");
            ds.PropertiesToLoad.Add("givenname");
            ds.PropertiesToLoad.Add("userPrincipalName");
            ds.Filter = string.Format("(&((&(objectCategory=Person)(objectClass=User)))(sn=*{0}*))", name.ToUpperInvariant());

            ds.SearchScope = SearchScope.Subtree;

            SearchResultCollection rs = ds.FindAll();

         
            List<UserInfo> ans = new List<UserInfo>();

            foreach(SearchResult result in rs)
            {
                var sn = result.GetDirectoryEntry().Properties["sn"].Value;
                var givenname = result.GetDirectoryEntry().Properties["givenname"].Value;
                var userPrincipalName = result.GetDirectoryEntry().Properties["userPrincipalName"].Value;
                var service = result.GetDirectoryEntry().Properties["ou"].Value;

                ans.Add(new UserInfo()
                    {
                        Name = sn,
                        FirstName = givenname,
                        Login = userPrincipalName.ToString().Split('@')[0].ToLowerInvariant(),
                        Service = service != null ?service.ToString():string.Empty
                    });
            }
           
            return ans;
        }
    }
}
