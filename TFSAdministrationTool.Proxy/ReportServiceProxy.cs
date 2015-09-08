#region Using Statements
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Services.Protocols;
using System.Xml;

using TFSAdministrationTool.Proxy.Common;
using TFSAdministrationTool.Proxy.ReportServer;
#endregion

namespace TFSAdministrationTool.Proxy
{
  #region ReportServiceProxyFactory
  public static class ReportServiceProxyFactory
  {
    public delegate IReportServiceProxy Factory(string url, string root, SiteStatus status, ICredentials credentials);
    public static Factory CreateProxy;

    static ReportServiceProxyFactory()
    {
      CreateProxy = new Factory(CreateRealProxy);
    }

    public static IReportServiceProxy CreateReportServiceProxy(string url, string root, SiteStatus status, ICredentials credentials)
    {
      return CreateProxy(url, root, status, credentials);
    }

    private static IReportServiceProxy CreateRealProxy(string url, string root, SiteStatus status, ICredentials credentials)
    {
      return new ReportServiceProxy(url, root, status, credentials);
    }
  }
  #endregion

  #region IReportServiceProxy
  public interface IReportServiceProxy
  {
    SecurityInfo GetSecuritySettings(string name);

    void AddUserToRole(string nodeName, string userName, string roleName);
    void RemoveUserFromRole(string nodeName, string userName, string roleName);
    void RemoveUser(string nodeName, string userName);

    string Url { get; }
    string RootFolder { get; set; }
    SiteStatus SiteStatus { get; }
  }
  #endregion

  public class ReportServiceProxy : IReportServiceProxy
  {
    #region Fields
    private ReportServer.ReportingService2005 m_ReportingService;
    private SiteStatus m_SiteStatus;
    private string m_RootFolder;
    #endregion

    #region Constructor
    public ReportServiceProxy(string url, string root, SiteStatus status, ICredentials credentials)
    {
      m_ReportingService = new ReportingService2005();
      m_ReportingService.Url = url;
      m_ReportingService.Credentials = credentials;

      m_SiteStatus = status;

      m_RootFolder = root;
    }
    #endregion

    #region Methods
    /// <summary>
    ///    Add a user with given role to a project or folder as its called on the 
    /// reporting services site. The exceptions needs to be caught by the caller
    /// as SOAP exceptions would be thrown by the web service calls.
    /// </summary>
    /// <param name="userName">Name of the User</param>
    /// <param name="nodeName">Project or Folder to which its getting added</param>
    /// <param name="Role">Role assigned to the user</param>
    public void AddUserToRole(string nodeName, string userName, string roleName)
    {
      ValidateUserProjectRole(userName, nodeName, roleName);
      bool foundUser = false;

      Policy[] policies = GetPolicies(RootFolder + nodeName);
      System.Collections.ArrayList policylist = new System.Collections.ArrayList(policies);

      /// New role for the user.
      Role role = new Role();
      role.Name = roleName;

      /// Needed in case this is a brand new user.
      /// Its moved here so that GetType could use it below.
      Policy uPolicy = new Policy();

      foreach (Policy userPolicy in policylist)
      {
        /// case where the user already exist on the server and you are just adding a new role
        /// to that user.
        if (String.Compare(userPolicy.GroupUserName, userName, StringComparison.CurrentCultureIgnoreCase) == 0)
        {
          /// Get the current user role list.
          System.Collections.ArrayList rolelist = new System.Collections.ArrayList(userPolicy.Roles);

          /// rolelist.Contains(role) doesn't return true if exist. 
          /// Looks like they have not implemented the == operator.
          /// So have to use foreach.
          foreach (Role r in rolelist)
          {
            if (String.Compare(r.Name, roleName, StringComparison.CurrentCultureIgnoreCase) == 0)
              return;
          }

          /// Add the new role 
          rolelist.Add(role);

          /// Reset the role list to the userPolicy
          userPolicy.Roles = (Role[])rolelist.ToArray(role.GetType());

          foundUser = true;
          break;
        }
      }

      ///Case where the user doesn't exist yet on this site with any role
      if (foundUser == false)
      {
        uPolicy.GroupUserName = userName;

        /// Create an role list with one role.
        uPolicy.Roles = new Role[0];

        /// Get the current user role list.
        System.Collections.ArrayList rolelist = new System.Collections.ArrayList(uPolicy.Roles);

        /// Add the new role 
        rolelist.Add(role);

        uPolicy.Roles = (Role[])rolelist.ToArray(role.GetType());

        /// Add policy to policy list.
        policylist.Add(uPolicy);
      }

      /// Update the policy list on the server
      Policy[] newpolicies = (Policy[])policylist.ToArray(uPolicy.GetType());
      SetPolicies(RootFolder + nodeName, newpolicies);
    }

    /// <summary>
    ///  Get policies that hold the user name, his roles for a given project.
    /// </summary>
    /// <param name="nodeName">Project or Folder Name</param>
    /// <returns>An array of ReportServer2008.Policy</returns>
    private Policy[] GetPolicies(string nodeName)
    {
      bool inheritParent;
      Policy[] policies = m_ReportingService.GetPolicies(nodeName, out inheritParent);

      return policies;
    }

    public SecurityInfo GetSecuritySettings(string name)
    {
      bool inheritParent;
      Policy[] policies = null;
      SecurityInfo rsSecurityInfo = new SecurityInfo();

      /// In case the site is not available return
      if (m_SiteStatus != SiteStatus.Available)
        return rsSecurityInfo;

      try
      {
        policies = m_ReportingService.GetPolicies(RootFolder + name, out inheritParent);
      }
      catch (SoapException ex)
      {
        TfsAdminToolTracer.TraceException(TfsAdminToolTracer.TraceSwitch.TraceError, ex);

        if (ex.Detail.FirstChild.Name == "ErrorCode")
        {
          // Handle the Error
          switch (ex.Detail.FirstChild.InnerText)
          {
            case "rsItemNotFound": m_SiteStatus = SiteStatus.Unavailable;
              break;
            case "rsAccessDenied": m_SiteStatus = SiteStatus.Unauthorized;
              break;
            default: m_SiteStatus = SiteStatus.Error;
              break;
          }
        }
        else
        {
          // Unexpected response
          m_SiteStatus = SiteStatus.Error;
        }

        return rsSecurityInfo;
      }
      catch (WebException ex)
      {
        // Make sure the Response object exists
        if (ex.Response != null)
        {
          switch (((HttpWebResponse)ex.Response).StatusCode)
          {
            case HttpStatusCode.NotFound:
              TfsAdminToolTracer.TraceException(TfsAdminToolTracer.TraceSwitch.TraceError, ex);
              m_SiteStatus = SiteStatus.Unavailable;
              break;
            default:
              TfsAdminToolTracer.TraceException(TfsAdminToolTracer.TraceSwitch.TraceError, ex);
              m_SiteStatus = SiteStatus.Error;
              break;
          }
        }
        else
        {
          TfsAdminToolTracer.TraceException(TfsAdminToolTracer.TraceSwitch.TraceError, ex);
          m_SiteStatus = SiteStatus.Error;
        }
        return rsSecurityInfo;
      }
      catch (Exception ex)
      {
        TfsAdminToolTracer.TraceException(TfsAdminToolTracer.TraceSwitch.TraceError, ex);
        m_SiteStatus = SiteStatus.Error;

        return rsSecurityInfo;
      }

      foreach (string role in ListRoles(RootFolder + name))
      {
        SecurityGroup sGroup = new SecurityGroup() { DisplayName = role };
        rsSecurityInfo.AddGroup(sGroup);
      }

      foreach (Policy p in policies)
      {
        foreach (Role r in p.Roles)
        {
          SecurityGroup sGroup = rsSecurityInfo.GetGroupByName(r.Name);
          sGroup.AddUser(String.Empty, p.GroupUserName, p.GroupUserName, String.Empty);
        }
      }

      m_SiteStatus = SiteStatus.Available;
      return rsSecurityInfo;
    }

    private List<string> ListRoles(string name)
    {
      Role[] roles;
      List<string> rolesList = new List<string>();

      // Get the roles on the catalog
      roles = m_ReportingService.ListRoles(SecurityScopeEnum.Catalog);
      foreach (Role r in roles)
      {
        rolesList.Add(r.Name);
      }

      return rolesList;
    }

    /// <summary>
    ///  This removes the user from the Reporting Services project site.The exceptions needs 
    ///  to be caught by the caller as SOAP exceptions would be thrown by the web service calls. 
    ///  The exception denotes a failure.
    /// </summary>
    /// <param name="userName">Name of the user</param>
    /// <param name="nodeName">Name of the project or folder</param>
    public void RemoveUser(string nodeName, string userName)
    {
      ValidateUserProject(userName, nodeName);

      // Remove Catalog policies
      Policy[] policies = GetPolicies(RootFolder + nodeName);

      if (policies.Length > 0)
      {
        System.Collections.ArrayList policylist = new System.Collections.ArrayList(policies);

        foreach (Policy userPolicy in policies)
        {
          if (String.Compare(userPolicy.GroupUserName, userName, StringComparison.CurrentCultureIgnoreCase) == 0)
          {
            policylist.Remove(userPolicy);
            Policy[] newpolicies = (Policy[])policylist.ToArray(policies[0].GetType());
            SetPolicies(RootFolder + nodeName, newpolicies);
            break;
          }
        }
      }
    }

    /// <summary>
    ///   Remove the user from an assigned role for the given project or folder. The underlying web service
    /// call will throw a SOAP exception which denotes failure to remove user from role. It needs to be
    /// caught by the caller as its the caller who needs to decide what to do with that exception.
    /// </summary>
    /// <param name="userName">User Name</param>
    /// <param name="nodeName">Project or Folder Name</param>
    /// <param name="roleName">Role name</param>
    public void RemoveUserFromRole(string nodeName, string userName, string roleName)
    {
      ValidateUserProjectRole(userName, nodeName, roleName);
      Policy[] policies = GetPolicies(RootFolder + nodeName);

      if (policies.Length > 0)
      {
        System.Collections.ArrayList policylist = new System.Collections.ArrayList(policies);

        foreach (Policy userPolicy in policies)
        {

          if (String.Compare(userPolicy.GroupUserName, userName, StringComparison.CurrentCultureIgnoreCase) == 0)
          {
            System.Collections.ArrayList rolelist = new System.Collections.ArrayList(userPolicy.Roles);

            foreach (Role uRole in userPolicy.Roles)
            {
              if (String.Compare(uRole.Name, roleName, StringComparison.CurrentCultureIgnoreCase) == 0)
              {
                rolelist.Remove(uRole);

                /// You could directly assign this to userPolicy.Roles, but helps in the debugger 
                /// while debugging to compare the old roles and the new roles easily.
                Role[] newroles = (Role[])rolelist.ToArray(uRole.GetType());

                userPolicy.Roles = newroles;

                /// No roles associated with this user anymore.
                /// in which case remove the user from the site.
                if (rolelist.Count < 1)
                {
                  policylist.Remove(userPolicy);
                }
                Policy[] newpolicies = (Policy[])policylist.ToArray(policies[0].GetType());
                SetPolicies(RootFolder + nodeName, newpolicies);

                break;
              }
            }
          }
        }
      }
    }

    /// <summary>
    ///  Sets the policies for a given project or folder name on the report server
    /// </summary>
    /// <param name="nodeName"></param>
    /// <param name="policies"></param>
    private void SetPolicies(string nodeName, Policy[] policies)
    {
      m_ReportingService.SetPolicies(nodeName, policies);
    }

    /// <summary>
    ///  Validation helper function for often used parameters
    /// </summary>
    /// <param name="userName">User Name</param>
    /// <param name="nodeName">Project or Folder name</param>
    /// <param name="roleName">Role of the user</param>
    private static void ValidateUserProjectRole(string userName, string nodeName, string roleName)
    {
      ValidateUserProject(userName, nodeName);

      if (roleName == null)
        throw new ArgumentNullException("roleName");
    }

    /// <summary>
    ///  Validation helper function for often used parameters
    /// </summary>
    /// <param name="nodeName">Project or Folder name</param>
    /// <param name="roleName">Role of the user</param>
    private static void ValidateUserProject(string userName, string nodeName)
    {
      if (userName == null)
        throw new ArgumentNullException("userName");

      if (nodeName == null)
        throw new ArgumentNullException("nodeName");
    }
    #endregion

    #region Properties
    public string Url
    {
      get
      {
        return m_ReportingService.Url;
      }
    }

    public SiteStatus SiteStatus
    {
      get
      {
        return m_SiteStatus;
      }
    }

    public string RootFolder
    {
      get
      {
        return m_RootFolder;
      }
      set
      {
        m_RootFolder = value;
      }
    }
    #endregion
  } //End Class
} //End Namespace