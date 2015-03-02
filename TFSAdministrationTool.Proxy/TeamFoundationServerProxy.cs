#region Using Statements
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Reflection;
using Microsoft.TeamFoundation;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Framework.Client;
using Microsoft.TeamFoundation.Server;
using Microsoft.Win32;
using TFSAdministrationTool.Proxy.Common;
#endregion

namespace TFSAdministrationTool.Proxy
{
  #region TeamFoundationServerProxyFactory
  public static class TeamFoundationServerProxyFactory
  {
    public delegate ITeamFoundationServerProxy Factory();
    public static Factory CreateProxy;

    static TeamFoundationServerProxyFactory()
    {
      CreateProxy = new Factory(CreateRealProxy);
    }

    public static ITeamFoundationServerProxy CreateTeamFoundationServerProxy()
    {
      return CreateProxy();
    }

    private static ITeamFoundationServerProxy CreateRealProxy()
    {
      return new TeamFoundationServerProxy();
    }
  }
  #endregion

  #region ITeamFoundationServerProxy
  public interface ITeamFoundationServerProxy
  {
    void Connect(Uri collectionUri, ICredentials credentials);
    void InitializeServerAndTeamProjectUsers();

    void AddUserToRole(string teamProject, string userName, string role);
    void RemoveUserFromRole(string teamProject, string userName, string role);
    void RemoveUser(string teamProject, string userName);

    ISharePointProxy GetSharePointProxy(string teamProject);
    void SharePointAddUserToRole(string teamProject, string userName, string role);
    void SharePointRemoveUserFromRole(string teamProject, string userName, string role);
    void SharePointRemoveUser(string teamProject, string userName);

    void ReportingServiceAddUserToRole(string teamProject, string userName, string role);
    void ReportingServiceRemoveUserFromRole(string teamProject, string userName, string role);
    void ReportingServiceRemoveUser(string teamProject, string userName);

    void SelectTeamProject(string name);
    void UpdateTeamProjects(ProjectInfo[] projects);

    string GetTeamProjectUri(string name);
    TfsUserCollection GetTeamProjectUsers(string name);
    TfsUser GetUser(string userName);

    bool IsUserServiceAccount(TfsUser user);
    bool IsUserTeamFoundationAdministrator(TfsUser user);

    Dictionary<string, string> ShowAddGroupControl(System.Windows.Forms.IWin32Window parentWin);

    TfsTeamProjectCollection Server { get; }

    ProjectInfo[] TeamProjects { get;}
    TfsVersion ServerVersion { get; }

    ItemType SelectedItemType { get; }
    string SelectedTeamProject { get; }

    IReportServiceProxy ReportServiceProxy { get; }
    RoleValidator RoleValidator { get; }

    TfsUserCollection UserCollection { get; }
    TfsUserCollection UserCollectionClean { get; }
  }
  #endregion

  public class TeamFoundationServerProxy : ITeamFoundationServerProxy
  {
    #region Fields
    // We store a SharePoint proxy object for each team project
    private Dictionary<string,ISharePointProxy> m_SharePointProxy;
    // All team projects are sharing the same SSRS proxy
    private IReportServiceProxy m_ReportServiceProxy;

    private TfsTeamProjectCollection m_TfsServer;
    private IIdentityManagementService m_TfsIdentityService;
    private IGroupSecurityService m_TfsSecurityService;
    private ICommonStructureService m_TfsProjectService;
    private IRegistration m_TfsRegistration;

    private RoleValidator m_RoleValidator;
    private ProjectInfo[] m_TeamProjects;
    private string m_SelectedTeamProject = String.Empty;
    private ItemType m_SelectedItemType;
    private TfsVersion m_ServerVersion;

    private TfsUserCollection m_TfsUsers;
    private TfsUserCollection m_TfsUsersClean;
    #endregion

    #region Team Foundation Server
    public void Connect(Uri collectionUri, ICredentials credentials)
    {
      try
      {
        m_TfsServer = new TfsTeamProjectCollection(collectionUri, credentials, new UICredentialsProvider());
        m_TfsServer.EnsureAuthenticated();

        m_TfsRegistration = m_TfsServer.GetService<IRegistration>();
        m_TfsProjectService = m_TfsServer.GetService<ICommonStructureService>();
        m_TfsSecurityService = m_TfsServer.GetService<IGroupSecurityService>();
        m_TfsIdentityService = m_TfsServer.GetService<IIdentityManagementService>();

        m_ServerVersion = GetTfsVersion();
        m_SharePointProxy = new Dictionary<string, ISharePointProxy>();
        m_ReportServiceProxy = ReportServiceProxyFactory.CreateReportServiceProxy(GetReportsServiceUrl(), GetReportsRoot(), GetReportsServiceSiteStatus(), m_TfsServer.Credentials);

        m_RoleValidator = new RoleValidator(this);
      }
      catch (TeamFoundationServerUnauthorizedException ex)
      {
        TfsAdminToolTracer.TraceException(TfsAdminToolTracer.TraceSwitch.TraceError, ex);
        throw;
      }
      catch (WebException ex)
      {
        TfsAdminToolTracer.TraceException(TfsAdminToolTracer.TraceSwitch.TraceError, ex);
        throw;
      }
    }

    /// <summary>
    /// Returns the list of users from the TeamProject. Since this is the first call 
    /// made to SharePoint and Reporting Services when a Team Project is selected the 
    /// method also detects if the SharePoint site and Reporting Services folder exists.
    /// </summary>
    /// <returns></returns>
    public void InitializeServerAndTeamProjectUsers()
    {
      if (SelectedItemType == ItemType.TeamProjectCollection)
        TfsAdminToolTracer.TraceMessage(TfsAdminToolTracer.TraceSwitch.TraceInfo, "Getting list of users for Team Foundation Server: " + Server.Name);
      else
        TfsAdminToolTracer.TraceMessage(TfsAdminToolTracer.TraceSwitch.TraceInfo, "Getting list of users for Team Foundation Server: " + Server.Name + ", Team Project: " + SelectedTeamProject);

      // get the Security Settings of the selected Team Project
      SecurityInfo tfsSecurityInfo = GetServerAndTeamProjectSecurityInfo(SelectedTeamProject);

      // get the Security Settings of the SharePoint Site      
      SecurityInfo spSecurityInfo = m_SharePointProxy[SelectedTeamProject].GetSecuritySettings();

      // SharePoint Tracing messages
      TfsAdminToolTracer.TraceMessage(TfsAdminToolTracer.TraceSwitch.TraceInfo, "UserGroup.Url: " + m_SharePointProxy[SelectedTeamProject].Url);
      TfsAdminToolTracer.TraceMessage(TfsAdminToolTracer.TraceSwitch.TraceInfo, "SharePoint site status: " + m_SharePointProxy[m_SelectedTeamProject].SiteStatus.ToString());
      TfsAdminToolTracer.TraceMessage(TfsAdminToolTracer.TraceSwitch.TraceInfo, "SharePoint version: " + m_SharePointProxy[m_SelectedTeamProject].WssVersion.ToString());
      if (ServerVersion == TfsVersion.TfsLegacy)
      {
        TfsAdminToolTracer.TraceMessage(TfsAdminToolTracer.TraceSwitch.TraceInfo, "PortalType: Unknown");
      }
      else
      {
        TfsAdminToolTracer.TraceMessage(TfsAdminToolTracer.TraceSwitch.TraceInfo, "PortalType: " + GetTeamProjectProperty(SelectedTeamProject, "PortalType"));
      }      

      // get the Security Settings of Reporting Services
      SecurityInfo rsSecurityInfo = m_ReportServiceProxy.GetSecuritySettings(SelectedTeamProject);
      
      // Reporting Services Tracing messages
      TfsAdminToolTracer.TraceMessage(TfsAdminToolTracer.TraceSwitch.TraceInfo, "ReportingService.Url: " + m_ReportServiceProxy.Url);
      TfsAdminToolTracer.TraceMessage(TfsAdminToolTracer.TraceSwitch.TraceInfo, "Reporting Services site status: " + m_ReportServiceProxy.SiteStatus.ToString());
      if (ServerVersion == TfsVersion.TfsLegacy)
      {
        TfsAdminToolTracer.TraceMessage(TfsAdminToolTracer.TraceSwitch.TraceInfo, "ReportsService: " + GetServiceUrlForTool(RegistrationUtilities.RosettaName, "ReportsService"));
      }
      else
      {
        TfsAdminToolTracer.TraceMessage(TfsAdminToolTracer.TraceSwitch.TraceInfo, "ReportWebServiceUrl: " + GetServiceUrlForTool(RegistrationUtilities.RosettaName, "ReportWebServiceUrl"));
      }      

      // Initialize the RoleValidator
      m_RoleValidator.Initialize(tfsSecurityInfo, spSecurityInfo, rsSecurityInfo);

      // Populate the list of TfsUsers
      m_TfsUsers = new TfsUserCollection();
      m_TfsUsers = GetUserListFromGroups(SelectedTeamProject, tfsSecurityInfo, spSecurityInfo, rsSecurityInfo);
      m_TfsUsers.Sort();

      // Save a copy of the users that we retrieved from the Server
      m_TfsUsersClean = new TfsUserCollection();
      m_TfsUsersClean = TfsUserCollection.Clone(m_TfsUsers);
    }

    private void InitializeTeamProjectProxy(string teamProject)
    {
      // Make sure that the SharePoint proxy exists
      if (!m_SharePointProxy.ContainsKey(teamProject))
      {
        ISharePointProxy spProxy = SharePointProxyFactory.CreateProxy(GetSharePointUrl(teamProject), GetSharePointSiteStatus(teamProject), m_TfsServer.Credentials);
        m_SharePointProxy[teamProject] = spProxy;
      }
    }

    private SecurityInfo GetServerAndTeamProjectSecurityInfo(string teamProject)
    {
      Identity[] groups;
      SecurityInfo tfsSecurityInfo = new SecurityInfo();

      if (SelectedItemType == ItemType.TeamProjectCollection)
      {
        groups = new Identity[2];

        //Team Foundation Administrators
        groups[0] = m_TfsSecurityService.ReadIdentity(SearchFactor.AdministrativeApplicationGroup, null, QueryMembership.Direct);
        //Service Accounts
        groups[1] = m_TfsSecurityService.ReadIdentity(SearchFactor.ServiceApplicationGroup, null, QueryMembership.Direct);
      }
      else
      {
        groups = m_TfsSecurityService.ListApplicationGroups(GetTeamProjectUri(teamProject));
      }

      foreach (Identity groupIdentity in groups)
      {
        Identity groupIdentityFull = m_TfsSecurityService.ReadIdentity(SearchFactor.Sid, groupIdentity.Sid, QueryMembership.Direct);

        SecurityGroup sGroup = new SecurityGroup() { DisplayName = groupIdentityFull.DisplayName };

        foreach (String userIdentitySid in groupIdentityFull.Members)
        {
          Identity userIdentity = m_TfsSecurityService.ReadIdentity(SearchFactor.Sid, userIdentitySid, QueryMembership.Direct);

          // We add only Windows users and groups
          if (userIdentity.Type == IdentityType.WindowsUser || userIdentity.Type == IdentityType.WindowsGroup)
          {
            sGroup.AddUser(userIdentity.Sid, userIdentity.Domain + "\\" + userIdentity.AccountName, userIdentity.DisplayName, userIdentity.Type);
          }
        }

        tfsSecurityInfo.AddGroup(sGroup);
      }

      return tfsSecurityInfo;
    }
    
    private TfsUserCollection GetUserListFromGroups(string teamProject, SecurityInfo tfsSecurityInfo, SecurityInfo spSecurityInfo, SecurityInfo rsSecurityInfo)
    {
      Hashtable userHashTable = new Hashtable();
      TfsUserCollection users = new TfsUserCollection();

      // loop through each group and each user of the group
      foreach (SecurityGroup sGroup in tfsSecurityInfo.SecuritySettings)
      {
        foreach (SecurityGroup.User member in sGroup.Users)
        {
          if (!userHashTable.ContainsKey(member.Sid))
          {
            TfsUser user = new TfsUser();
            user.UserName = member.UserName;
            user.DisplayName = member.DisplayName;
            user.IdentityType = member.IdentityType;
            user.AddRole(sGroup.DisplayName, SystemTier.TeamFoundation);

            if (m_SharePointProxy[teamProject].SiteStatus == SiteStatus.Available)
            {
              // If the site exists get the SharePoint roles 
              foreach (SecurityGroup spGroup in spSecurityInfo.GetGroupBySid(member.Sid))
              {
                user.AddRole(spGroup.DisplayName, SystemTier.SharePoint);
              }
            }

            if (m_ReportServiceProxy.SiteStatus == SiteStatus.Available)
            {
              // get the Reporting Services roles
              foreach (SecurityGroup rsGroup in rsSecurityInfo.GetGroupByUserName(member.UserName))
              {
                user.AddRole(rsGroup.DisplayName, SystemTier.ReportingServices);
              }
            }

            users.Add(user);
            userHashTable.Add(member.Sid, user);
          }
          else
          {
            //Since the user already exists add the new TfsRole
            TfsUser user = (TfsUser)userHashTable[member.Sid];
            users.Remove(user.UserName);

            user.AddRole(sGroup.DisplayName, SystemTier.TeamFoundation);
            users.Add(user);
            userHashTable[member.Sid] = user;
          }
        }
      }

      return users;
    }

    /// <summary>
    /// Builds a list of Team Project names
    /// Builds a Hashtable of Team Projects
    /// </summary>
    /// <returns></returns>
    public string GetTeamProjectUri(string name)
    {
      foreach (ProjectInfo info in m_TeamProjects)
      {
        if (String.Compare(name, info.Name, true) == 0)
        {
          return info.Uri;
        }
      }

      return String.Empty;
    }

    /// <summary>
    ///   Get the identity of a given user. It tries to get the information 
    ///   from the TFS databases but if the user is not known to TFS we will
    ///   get the details from the source
    /// </summary>
    /// <param name="userName">Name of the user</param>
    /// <returns>The users's Identity object or null</returns>
    private Identity GetUserIdentity(string userName)
    {
      Identity userIdentity = m_TfsSecurityService.ReadIdentity(SearchFactor.AccountName, userName, QueryMembership.Direct);
      if (userIdentity == null)
      {
        userIdentity = m_TfsSecurityService.ReadIdentityFromSource(SearchFactor.AccountName, userName);
      }

      return userIdentity;
    }

    public TfsUser GetUser(string userName)
    {
      Identity userIdentity = GetUserIdentity(userName);
      if (userIdentity != null)
      {
        return new TfsUser() { UserName = userIdentity.Domain + "\\" + userIdentity.AccountName, DisplayName = userIdentity.DisplayName };
      }

      return null;
    }

    public bool IsUserServiceAccount(TfsUser user)
    {
      //Service Accounts
      Identity identity = m_TfsSecurityService.ReadIdentity(SearchFactor.ServiceApplicationGroup, null, QueryMembership.Direct);

      return TfsUser.IsUserInRole(user, SystemTier.TeamFoundation, identity.DisplayName);
    }

    public bool IsUserTeamFoundationAdministrator(TfsUser user)
    {
      //Team Foundation Administrators
      Identity identity = m_TfsSecurityService.ReadIdentity(SearchFactor.AdministrativeApplicationGroup, null, QueryMembership.Direct);

      return TfsUser.IsUserInRole(user, SystemTier.TeamFoundation, identity.DisplayName);
    }

    public void AddUserToRole(string teamProject, string userName, string role)
    {
      TfsAdminToolTracer.TraceMessage(TfsAdminToolTracer.TraceSwitch.TraceInfo, "Adding user:" + userName + " to Team Foundation Server role:" + role);

      string groupName;

      if (SelectedItemType == ItemType.TeamProjectCollection)
      {
        groupName = "[SERVER]\\" + role;
      }
      else
      {
        groupName = "[" + teamProject + "]" + "\\" + role;
      }

      Identity userIdentity = m_TfsSecurityService.ReadIdentityFromSource(SearchFactor.AccountName, userName);
      Identity roleIdentity = m_TfsSecurityService.ReadIdentity(SearchFactor.AccountName, groupName, QueryMembership.Direct);

      if (userIdentity == null)
      {
        //InvalidTfsUserException iue = new InvalidTfsUserException(TFSResource.TFSInvalidUserErrorMsg, appTierHost);
        //iue.User = userName;
        //throw iue;
      }

      if (roleIdentity == null)
      {
        // InvalidTfsUserException iue = new InvalidTfsUserException(TFSResource.TFSInvalidUserErrorMsg, appTierHost);
        //iue.User = userName;
        // throw iue;
      }

      try
      {
        m_TfsSecurityService.AddMemberToApplicationGroup(roleIdentity.Sid, userIdentity.Sid);
      }
      catch (GroupSecuritySubsystemException ex)
      {
        TfsAdminToolTracer.TraceException(TfsAdminToolTracer.TraceSwitch.TraceError, ex);
        /// Member already exist. Ignore it.
      }
    }

    public void RemoveUser(string teamProject, string userName)
    {
      /// Get the details of the User
      Identity tUser = m_TfsSecurityService.ReadIdentity(SearchFactor.AccountName, userName, QueryMembership.Direct);

      if (tUser == null)
      {
        // InvalidTfsUserException iue = new InvalidTfsUserException(TFSResource.TFSInvalidUserErrorMsg, appTierHost);
        //iue.User = userName;
        // throw iue;
      }

      /// Build a collection with the MemberOf Sids
      string[] MemberOfSids = tUser.MemberOf;
      Collection<string> memberOfSidCollection = new Collection<string>(MemberOfSids);

      /// Get the list of Team Project Groups or Application Group
      Identity[] groups;
      if (SelectedItemType == ItemType.TeamProjectCollection)
      {
        groups = new Identity[2];

        //Team Foundation Administrators
        groups[0] = m_TfsSecurityService.ReadIdentity(SearchFactor.AdministrativeApplicationGroup, null, QueryMembership.Direct);
        //Service Accounts
        groups[1] = m_TfsSecurityService.ReadIdentity(SearchFactor.ServiceApplicationGroup, null, QueryMembership.Direct);
      }
      else
      {
        groups = m_TfsSecurityService.ListApplicationGroups(GetTeamProjectUri(teamProject));
      }

      /// Loop through the list of groups from the Team Project and 
      /// if the group can be found in the MemberOf list, than we 
      /// can go ahead and remove the user from the group
      foreach (Identity role in groups)
      {
        if (memberOfSidCollection.Contains(role.Sid))
        {
          m_TfsSecurityService.RemoveMemberFromApplicationGroup(role.Sid, tUser.Sid);
        }
      }
    }

    public void RemoveUserFromRole(string teamProject, string userName, string role)
    {
      string groupName = "[" + teamProject + "]" + "\\" + role;

      /// Get the details of the User and Role
      Identity userIdentity = m_TfsSecurityService.ReadIdentity(SearchFactor.AccountName, userName, QueryMembership.Direct);
      Identity roleIdentity = m_TfsSecurityService.ReadIdentity(SearchFactor.AccountName, groupName, QueryMembership.Direct);

      if (userIdentity == null)
      {
        // InvalidTfsUserException iue = new InvalidTfsUserException(TFSResource.TFSInvalidUserErrorMsg, appTierHost);
        //iue.User = userName;
        // throw iue;
      }

      if (roleIdentity == null)
      {
        // InvalidTfsUserException iue = new InvalidTfsUserException(TFSResource.TFSInvalidUserErrorMsg, appTierHost);
        //iue.User = userName;
        // throw iue;
      }

      m_TfsSecurityService.RemoveMemberFromApplicationGroup(roleIdentity.Sid, userIdentity.Sid);
    }

    public void SelectTeamProject(string name)
    {
      m_SelectedItemType = (name == String.Empty) ? ItemType.TeamProjectCollection : ItemType.TeamProject;
      m_SelectedTeamProject = name;

      InitializeTeamProjectProxy(name);

      if (m_SelectedItemType == ItemType.TeamProject)
        TfsAdminToolTracer.TraceMessage(TfsAdminToolTracer.TraceSwitch.TraceInfo, "Selecting Team Project: " + name);
    }

    private string GetServiceUrlForTool(string tool, string name)
    {
      RegistrationEntry[] entries = m_TfsRegistration.GetRegistrationEntriesFromServer(tool);

      foreach (RegistrationEntry entry in entries)
      {
        foreach (ServiceInterface sInterface in entry.ServiceInterfaces)
        {
          if (String.Compare(sInterface.Name, name, true) == 0)
          {
            return sInterface.Url;
          }
        }
      }

      return String.Empty;
    }

    private string GetTeamProjectProperty(string teamProject, string property)
    {
      // This call should be used only with Team Foundation Server 2010
      if (m_ServerVersion != TfsVersion.Tfs2010)
        return string.Empty;

      RegistrationEntry[] entries = m_TfsRegistration.GetRegistrationEntriesFromServer("TeamProjects");

      foreach (RegistrationEntry entry in entries)
      {
        foreach (ServiceInterface sInterface in entry.ServiceInterfaces)
        {
          if (teamProject == "*")
          {
            if (sInterface.Name.Contains(property))
            {
              return sInterface.Url;
            }
          }
          else
          {            
            if (String.Compare(sInterface.Name, String.Format("{0}:{1}", teamProject, property), true) == 0)
            {
              return sInterface.Url;
            }          
          }          
        }
      }

      return String.Empty;
    }

    private TfsVersion GetTfsVersion()
    {
        RegistrationEntry[] frameworkEntries = m_TfsRegistration.GetRegistrationEntriesFromServer("Framework");
      
      if (frameworkEntries.Length > 0)
      {
        return TfsVersion.Tfs2010;
      }
      else
      {
        return TfsVersion.TfsLegacy;
      }
    }

    public void UpdateTeamProjects(ProjectInfo[] projects)
    {
      m_TeamProjects = projects;

      // Make sure the selected team project was not deleted
      if (m_SelectedTeamProject != String.Empty)
      {
        bool found = false;
        
        foreach (ProjectInfo info in m_TeamProjects)
        {
          if (String.Compare(m_SelectedTeamProject, info.Name, true) == 0)
          {
            found = true;
            break;
          }
        }

        if (!found)
          m_SelectedTeamProject = String.Empty;
      }
    }

    public TfsUserCollection GetTeamProjectUsers(string name)
    {
      // Make sure the appropriate proxies exist
      InitializeTeamProjectProxy(name);

      // get the Security Settings of the Team Project
      SecurityInfo tfsSecurityInfo = GetServerAndTeamProjectSecurityInfo(name);

      // get the Security Settings of the SharePoint Site
      SecurityInfo spSecurityInfo = m_SharePointProxy[name].GetSecuritySettings();

      // get the Security Settings of Reporting Services
      SecurityInfo rsSecurityInfo = m_ReportServiceProxy.GetSecuritySettings(name);

      // Populate the list of TfsUsers
      TfsUserCollection users = new TfsUserCollection();
      users = GetUserListFromGroups(name, tfsSecurityInfo, spSecurityInfo, rsSecurityInfo);
      users.Sort();

      return users;
    }

    public Dictionary<string, string> ShowAddGroupControl(System.Windows.Forms.IWin32Window parentWin)
    {
      Dictionary<string, string> userGroupCollection = null;
      try
      {
        /// Loading Microsoft.TeamFoundation.Controls.dll from the GAC. As this assembly
        /// is new to Team Foundation SEerver 2010, it is safe to use LoadWithPartialName.
        Assembly asm = Assembly.LoadWithPartialName("Microsoft.TeamFoundation.Controls");
        Type addGroupCtrl = asm.GetType("Microsoft.TeamFoundation.Controls.WinForms.AddGroupControl");
        MethodInfo mi = addGroupCtrl.GetMethod("AddWindowsUsersOrGroups");

        /// Build the parameter list
        object[] p = new object[] { parentWin, m_TfsIdentityService, null };

        /// Invoke the internal method
        object result = mi.Invoke(null, p);

        /// Process the users
        if ((bool)result)
        {
          userGroupCollection = new Dictionary<string, string>();

          TeamFoundationIdentity[] identities = (TeamFoundationIdentity[])p[2];

          foreach (TeamFoundationIdentity identity in identities)
          {
            string domain = identity.GetAttribute(IdentityAttributeTags.Domain, String.Empty);
            string account = identity.GetAttribute(IdentityAttributeTags.AccountName, String.Empty);

            if (!String.IsNullOrEmpty(domain) && !String.IsNullOrEmpty(account))
            {
              userGroupCollection.Add(String.Concat(domain, "\\", account), identity.DisplayName);
            }
          }

        }
      }
      catch (Exception ex)
      {
        TfsAdminToolTracer.TraceException(TfsAdminToolTracer.TraceSwitch.TraceError, ex);
      }

      return userGroupCollection;
    }    
    #endregion

    #region SharePoint
    public void SharePointAddUserToRole(string teamProject, string userName, string role)
    {
        RoleInfo rInfo = this.RoleValidator.SpRoles[role];
        bool isSpGroup = rInfo != null && rInfo.IsSpGroup;
      if (m_SharePointProxy[teamProject].SiteStatus == SiteStatus.Available)
      {
        Identity userIdentity = GetUserIdentity(userName);
          if (!isSpGroup)
          {
              m_SharePointProxy[teamProject].AddUserToRole(userName, role, userIdentity.DisplayName,
                                                           userIdentity.MailAddress, userIdentity.Description);
          }
          else
          {
              m_SharePointProxy[teamProject].AddUserToGroup(userName, role, userIdentity.DisplayName,
                                             userIdentity.MailAddress, userIdentity.Description);
          }
      }
    }

    public void SharePointRemoveUser(string teamProject, string userName)
    {
      if (m_SharePointProxy[teamProject].SiteStatus == SiteStatus.Available)
      {
        m_SharePointProxy[teamProject].RemoveUser(userName);
      }
    }

    public void SharePointRemoveUserFromRole(string teamProject, string userName, string role)
    {
        RoleInfo rInfo = this.RoleValidator.SpRoles[role];
        bool isSpGroup = rInfo != null && rInfo.IsSpGroup;
      if (m_SharePointProxy[teamProject].SiteStatus == SiteStatus.Available)
      {
          if (!isSpGroup)
          {
              m_SharePointProxy[teamProject].RemoveUserFromRole(userName, role);
          }
          else
          {
              m_SharePointProxy[teamProject].RemoveUserFromGroup(userName, role);
          }
      }
    }

    public ISharePointProxy GetSharePointProxy(string teamProject)
    {
      return m_SharePointProxy[teamProject];
    }

    private string GetBaseSiteUrl()
    {
      return GetServiceUrlForTool(RegistrationUtilities.SharePointName, "BaseSiteUrl");
    }

    private string GetBaseAdminSiteUrl()
    {
      string adminServiceUrl = GetServiceUrlForTool(RegistrationUtilities.SharePointName, "WssAdminService");
      return adminServiceUrl.Substring(0, adminServiceUrl.IndexOf("/_vti_adm/"));
    }

    private string GetSharePointUrl(string teamProject)
    {
      if (SelectedItemType == ItemType.TeamProjectCollection)
      {
        return GetBaseAdminSiteUrl() + "/_vti_bin/usergroup.asmx";
      }
      else
      {
        if (m_ServerVersion == TfsVersion.TfsLegacy)
        {
          return GetBaseSiteUrl() + "/" + teamProject + "/_vti_bin/usergroup.asmx";
        }
        else
        {
          string portalType = GetTeamProjectProperty(teamProject, "PortalType");

          if (portalType == "WssSite")
          {
            return GetTeamProjectProperty(teamProject, "Portal") + "/_vti_bin/usergroup.asmx";          
          }
          else
          {
            /// The team project does not have a SharePoint site. 
            /// We return a dummy Url which will not be used.
            return GetBaseSiteUrl() + "/" + teamProject + "/_vti_bin/DummyUsergroup.asmx";
          }
        }
      }
    }

    private SiteStatus GetSharePointSiteStatus(string teamProject)
    {
      if (ServerVersion == TfsVersion.TfsLegacy)
      {
        /// In case of TFS2005/TFS2008 we assume that the site is available.
        /// The first call to SharePoint will determine the actual status of 
        /// the site.
        return SiteStatus.Available;
      }
      else
      {
        /// Get the portal type of the team project
        string portalType = GetTeamProjectProperty(teamProject, "PortalType");

        if (portalType == "WssSite")
        {
          /// The team project has a SharePoint site so we assume that it is 
          /// available. The first call to the site will further validate this.
          return SiteStatus.Available;
        }
        else
        {
          /// The team project does not have a SharePoint site.
          return SiteStatus.Unavailable;
        }
      }
    }
    
    #endregion

    #region Reporting Services
    public void ReportingServiceAddUserToRole(string teamProject, string userName, string role)
    {
      if (m_ReportServiceProxy.SiteStatus == SiteStatus.Available)
      {
        m_ReportServiceProxy.AddUserToRole(teamProject, userName, role);
      }
    }

    public void ReportingServiceRemoveUser(string teamProject, string userName)
    {
      if (m_ReportServiceProxy.SiteStatus == SiteStatus.Available)
      {
        m_ReportServiceProxy.RemoveUser(teamProject, userName);
      }
    }

    public void ReportingServiceRemoveUserFromRole(string teamProject, string userName, string role)
    {
      if (m_ReportServiceProxy.SiteStatus == SiteStatus.Available)
      {
        m_ReportServiceProxy.RemoveUserFromRole(teamProject, userName, role);
      }
    }

    private SiteStatus GetReportsServiceSiteStatus()
    {
      if (ServerVersion == TfsVersion.TfsLegacy)
      {
        /// In case of TFS2005/TFS2008 we assume that the site is available.
        /// The first call to Reporting Services will determine the actual 
        /// status of the site.
        return SiteStatus.Available;
      }
      else
      {
        /// Get the Url of Reporting Services from the Catalog
        string reportsServiceUrl = GetServiceUrlForTool(RegistrationUtilities.RosettaName, "ReportWebServiceUrl");

        if (reportsServiceUrl.Contains("ReportService.asmx") || reportsServiceUrl.Contains("ReportService2005.asmx"))
        {
          return SiteStatus.Available;
        }
        else
        {
          return SiteStatus.Unavailable;
        }
      }
    }

    private string GetReportsServiceUrl()
    {
      string reportsServiceUrl;

      if (m_ServerVersion == TfsVersion.TfsLegacy)
      {
        reportsServiceUrl = GetServiceUrlForTool(RegistrationUtilities.RosettaName, "ReportsService");
      }
      else
      {
        reportsServiceUrl = GetServiceUrlForTool(RegistrationUtilities.RosettaName, "ReportWebServiceUrl");
      }

      /// If the server is not using Reporting Services, the Url will not contain 
      /// ReportService. We create a dummy Url and at the first call to Reporting 
      /// Services this will be detected and the SiteStatus will set to Unavailable.
      if (!reportsServiceUrl.Contains("ReportService.asmx") && !reportsServiceUrl.Contains("ReportService2005.asmx"))
      {
        return reportsServiceUrl + "/DummyReportServiceUrl.asmx";
      }
      else
      {
        return reportsServiceUrl.Replace("ReportService.asmx", "ReportService2005.asmx");
      }
    }

    private string GetReportsRoot()
    {
      if (m_ServerVersion == TfsVersion.TfsLegacy)
      {
        /// For Team Foundation Server 2005 and 2008 ReportsRoot is "/"

        return "/";
      }
      else
      {
        /// For Team Foundation Server 2010 we use the ReportFolder property

        string reportFolder = GetTeamProjectProperty("*", "ReportFolder");

        if (reportFolder != String.Empty)
        {
          string[] tokens = reportFolder.Split('/');
          return String.Format("/{0}/{1}/", tokens[1], tokens[2]);
        }
        else
        {
          /// The server is not using Reporting Services, so we return the 
          /// default root. This value will not be used as at the first call
          /// to Reporting Services the SiteStatus will be set to Unavailable.
          
          return "/DummyRoot";
        }
      }
    }

    #endregion

    #region Properties
    public IReportServiceProxy ReportServiceProxy
    {
      get
      {
        return m_ReportServiceProxy;
      }
    }
    
    public RoleValidator RoleValidator
    {
      get
      {
        return m_RoleValidator;
      }
    }

    public ItemType SelectedItemType
    {
      get
      {
        return m_SelectedItemType;
      }
    }

    public string SelectedTeamProject
    {
      get
      {
        return m_SelectedTeamProject;
      }
    }

    public TfsTeamProjectCollection Server
    {
      get
      {
        return m_TfsServer;
      }
    }
    
    public TfsVersion ServerVersion
    {
      get
      {
        return m_ServerVersion;
      }
    }

    public ProjectInfo[] TeamProjects
    {
      get
      {
        return m_TeamProjects;
      }
    }

    public TfsUserCollection UserCollection
    {
      get
      {
        return m_TfsUsers;
      }
    }

    public TfsUserCollection UserCollectionClean
    {
      get
      {
        return m_TfsUsersClean;
      }
    }
    #endregion
  } //End Class
} //End Namespace