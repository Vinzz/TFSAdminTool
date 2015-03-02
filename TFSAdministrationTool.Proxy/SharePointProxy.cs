#region Using Statements
using System;
using System.Collections.Generic;
using System.Net;
using System.Xml;
using System.Web.Services.Protocols;

using TFSAdministrationTool.Proxy.Common;
using TFSAdministrationTool.Proxy.SharePoint;
#endregion

namespace TFSAdministrationTool.Proxy
{
  public static class SharePointProxyFactory
  {
    public delegate ISharePointProxy Factory(string url, SiteStatus status, ICredentials credentials);
    public static Factory CreateProxy;

    static SharePointProxyFactory()
    {
      CreateProxy = new Factory(CreateRealProxy);
    }

    public static ISharePointProxy CreateSharePointProxy(string url, SiteStatus status, ICredentials credentials)
    {
      return CreateProxy(url, status, credentials);
    }

    private static ISharePointProxy CreateRealProxy(string url, SiteStatus status, ICredentials credentials)
    {
      return new SharePointProxy(url, status, credentials);
    }
  }

  public interface ISharePointProxy
  {
    void AddUserToRole(string userName, string role, string displayName, string email, string description);
    void AddUserToGroup(string userName, string group, string displayName, string email, string description);
    void RemoveUserFromRole(string userName, string role);
    void RemoveUserFromGroup(string userName, string group);
    void RemoveUser(string userName);

    List<string> GetRoleCollectionFromUser(string userName);
    List<string> GetGroupCollectionFromUser(string userName);
    XmlNode GetRoleCollectionFromWeb();
    XmlNode GetGroupCollectionFromSite();
    SecurityInfo GetSecuritySettings();

    string Url { get; }
    SiteStatus SiteStatus { get; }
    WssVersion WssVersion { get; }
  }

  public class SharePointProxy : ISharePointProxy
  {
    #region Fields
    private SharePoint.UserGroup m_UserGroup;
    private ICredentials m_Credentials;
    private SiteStatus m_SiteStatus;
    private WssVersion m_WssVersion;
    #endregion

    #region Constructors
    public SharePointProxy(string url, SiteStatus status, ICredentials credentials)
    {
      m_UserGroup = new UserGroup();
      m_UserGroup.Url = url;
      m_UserGroup.Credentials = m_Credentials = credentials;

      m_SiteStatus = status;
      m_WssVersion = GetVersion();
    }
    #endregion

    #region Methods
    /// <summary>
    ///   Add a user with a given role to share point
    /// </summary>
    /// <param name="userName">Name of the user</param>
    /// <param name="siteName">SharePoint Site</param>
    /// <param name="role">Name of the role that he is assigned</param>ite
    /// <param name="displayName">Display Name</param>
    /// <param name="email">User's email address</param>
    /// <param name="description">A descripton/comments field about the user</param>
    public void AddUserToRole(string userName, string role, string displayName, string email, string description)
    {
      m_UserGroup.AddUserToRole(role, displayName, userName, email, description);
    }

    /// <summary>
    ///   Add a user with a given role to share point
    /// </summary>
    /// <param name="userName">Name of the user</param>
    /// <param name="siteName">SharePoint Site</param>
    /// <param name="group">Name of the group that he is assigned</param>ite
    /// <param name="displayName">Display Name</param>
    /// <param name="email">User's email address</param>
    /// <param name="description">A descripton/comments field about the user</param>
    public void AddUserToGroup(string userName, string group, string displayName, string email, string description)
    {
        m_UserGroup.AddUserToGroup(group, displayName, userName, email, description);
    }

    /// <summary>
    ///  This method returns a readonly role collection of roles that the user belongs to for a 
    /// given sharepoint site. Since these shouldn't get modified by the caller externally 
    /// they are made readonly.
    /// </summary>
    /// <param name="userName">User Name</param>
    /// <param name="siteName">SharePoint Site Name</param>
    /// <returns>ReadOnlyCollection of string</returns>
    public List<string> GetRoleCollectionFromUser(string userName)
    {
      List<string> spRoles = new List<string>(); ;
      XmlNode result;

      //if (userName == null)
      //throw new InvalidSPUserException(userName, siteName);

      /// Actual call to the SharePoint Web Service to get a list of roles the user belongs to. Its returned
      /// as XML data. 
      try
      {
        result = m_UserGroup.GetRoleCollectionFromUser(userName);

        foreach (XmlNode node in result.FirstChild.ChildNodes)
        {
          spRoles.Add(node.Attributes["Name"].Value);
        }
      }
      catch (SoapException ex)
      {
        TfsAdminToolTracer.TraceException(TfsAdminToolTracer.TraceSwitch.TraceError, ex);
      }

      return spRoles;
    }

    /// <summary>
    ///  This method returns a readonly group collection of groups that the user belongs to for a 
    /// given sharepoint site. Since these shouldn't get modified by the caller externally 
    /// they are made readonly.
    /// </summary>
    /// <param name="userName">User Name</param>
    /// <param name="siteName">SharePoint Site Name</param>
    /// <returns>ReadOnlyCollection of string</returns>
    public List<string> GetGroupCollectionFromUser(string userName)
    {
        List<string> spGroups = new List<string>();
        XmlNode result;

        //if (userName == null)
        //throw new InvalidSPUserException(userName, siteName);

        /// Actual call to the SharePoint Web Service to get a list of roles the user belongs to. Its returned
        /// as XML data. 
        try
        {
            result = m_UserGroup.GetGroupCollectionFromUser(userName);

            foreach (XmlNode node in result.FirstChild.ChildNodes)
            {
                spGroups.Add(node.Attributes["Name"].Value);
            }
        }
        catch (SoapException ex)
        {
            TfsAdminToolTracer.TraceException(TfsAdminToolTracer.TraceSwitch.TraceError, ex);
        }

        return spGroups;
    }

    public XmlNode GetRoleCollectionFromWeb()
    {
      return m_UserGroup.GetRoleCollectionFromWeb();
    }

    public XmlNode GetGroupCollectionFromSite()
    {
        return m_UserGroup.GetGroupCollectionFromSite();
    }

    /// <summary>
    /// Get the security settings of the site and formats them into an XmlNode.
    /// It also detects if the site for the TeamProject/Server exists. We do this
    /// so that no additional calls are made to the server in order to detect site
    /// existence.
    /// </summary>
    /// <returns></returns>
    public SecurityInfo GetSecuritySettings()
    {
      SecurityInfo spSecurityInfo = new SecurityInfo();

      // If the SiteStatus is not Available return
      if (m_SiteStatus != SiteStatus.Available)
        return spSecurityInfo;

      // Try to get the roles for the site. If this call results in a 
      // WebException with HttpStatusCode 404 than the site does not exist
      try
      {
        // If we managed to get the roles, loop through them
        foreach (XmlNode roleNode in GetRoleCollectionFromWeb().ChildNodes[0].ChildNodes)
        {
            // WSS2.0 role node does not contain a Hidden attribute
            // WSS3.0 role node needs to have Hidden="False"
            if (roleNode.Attributes["Hidden"] == null ||
               (roleNode.Attributes["Hidden"] != null && String.Compare(roleNode.Attributes["Hidden"].Value, "False", true) == 0))
            {
                SecurityGroup sGroup = new SecurityGroup() { DisplayName = roleNode.Attributes["Name"].Value };
                XmlNode usersNode = m_UserGroup.GetUserCollectionFromRole(roleNode.Attributes["Name"].Value);

                foreach (XmlNode userNode in usersNode.FirstChild.ChildNodes)
                {
                    sGroup.AddUser(userNode.Attributes["Sid"].Value, userNode.Attributes["Name"].Value, userNode.Attributes["Name"].Value);
                }
                spSecurityInfo.AddGroup(sGroup);
            }
        }

          try
          {
                // Wrap group retrieval in a nested try catch, so that failure to handle SP Groups will result in gracefully falling back to default functionality
              foreach (XmlNode groupNode in GetGroupCollectionFromSite().ChildNodes[0].ChildNodes)
              {
                  if (groupNode.Attributes["Hidden"] == null ||
                     (groupNode.Attributes["Hidden"] != null && String.Compare(groupNode.Attributes["Hidden"].Value, "False", true) == 0))
                  {
                      SecurityGroup sGroup = new SecurityGroup() { DisplayName = groupNode.Attributes["Name"].Value, IsSpGroup = true };
                      XmlNode usersNode = m_UserGroup.GetUserCollectionFromGroup(groupNode.Attributes["Name"].Value);

                      foreach (XmlNode userNode in usersNode.FirstChild.ChildNodes)
                      {
                          sGroup.AddUser(userNode.Attributes["Sid"].Value, userNode.Attributes["Name"].Value, userNode.Attributes["Name"].Value);
                      }
                      spSecurityInfo.AddGroup(sGroup);
                  }
              }
          }
          catch (Exception ex)
          {
              TfsAdminToolTracer.TraceException(TfsAdminToolTracer.TraceSwitch.TraceError, ex);
          }

        m_SiteStatus = SiteStatus.Available;
      }
      catch (WebException ex)
      {
        TfsAdminToolTracer.TraceException(TfsAdminToolTracer.TraceSwitch.TraceError, ex);

        if (ex.Response != null)
        {
          switch (((HttpWebResponse)ex.Response).StatusCode)
          {
            case HttpStatusCode.NotFound:
              m_SiteStatus = SiteStatus.Unavailable;
              break;
            case HttpStatusCode.Forbidden:
            case HttpStatusCode.Unauthorized:
              m_SiteStatus = SiteStatus.Unauthorized;
              break;
            default:
              m_SiteStatus = SiteStatus.Error;
              break;
          }
        }
        else
        {
          m_SiteStatus = SiteStatus.Error;
        }

        return spSecurityInfo;
      }
      catch (Exception ex)
      {
        TfsAdminToolTracer.TraceException(TfsAdminToolTracer.TraceSwitch.TraceError, ex);
        m_SiteStatus = SiteStatus.Error;
        return spSecurityInfo;
      }

      return spSecurityInfo;
    }

    public void RemoveUser(string userName)
    {
      List<string> assignedRoles = GetRoleCollectionFromUser(userName);

      foreach (string assignedRole in assignedRoles)
      {
        m_UserGroup.RemoveUserFromRole(assignedRole, userName);
      }

      List<string> assignedGroups = GetGroupCollectionFromUser(userName);

      foreach (string assignedGroup in assignedGroups)
      {
          m_UserGroup.RemoveUserFromGroup(assignedGroup, userName);
      }
    }

    /// <summary>
    ///  Remove the user from a given role assignment on the Share Point site.
    /// </summary>
    /// <param name="userName">User Name</param>
    /// <param name="siteName">SharePoint Site Name</param>
    /// <param name="role">Role Name</param>
    public void RemoveUserFromRole(string userName, string role)
    {
      m_UserGroup.RemoveUserFromRole(role, userName);
    }

    /// <summary>
    ///  Remove the user from a given role assignment on the Share Point site.
    /// </summary>
    /// <param name="userName">User Name</param>
    /// <param name="siteName">SharePoint Site Name</param>
    /// <param name="group">Group Name</param>
    public void RemoveUserFromGroup(string userName, string group)
    {
        m_UserGroup.RemoveUserFromGroup(group, userName);
    }

    /// <summary>
    /// This is the first call that we make against SharePoint. In case 
    /// we cannot navigate to the URL, we assume that WSS is not installed.
    /// This can happen in case we run the tool against TFS 2010 Basic.
    /// </summary>
    /// <returns></returns>
    private WssVersion GetVersion()
    {
      try
      {
        WebRequest request = WebRequest.Create(m_UserGroup.Url);
        request.Credentials = m_Credentials;
        WebResponse response = request.GetResponse();

        string[] headerVersion = response.Headers.Get("MicrosoftSharePointTeamServices").Split('.');

        /// Corrupt header version string
        if (headerVersion.Length != 4)
        {
          TfsAdminToolTracer.TraceException(TfsAdminToolTracer.TraceSwitch.TraceError, new Exception("Incorrect MicrosoftSharePointTeamServices format"));
          return WssVersion.Unknown;              
        }

        switch (headerVersion[0])
        {
          case "6": 
            return WssVersion.WSS2;
          case "12":
            return WssVersion.WSS3;
          case "14":
            return WssVersion.WSS4;
          default:
            return WssVersion.Unknown;
        }        
      }
      catch (WebException ex)
      {
        /// Running into a WebException means that we could not
        /// navigate to the SharePoint site. This might be expected
        /// as TFS 2010 Basic does not install SharePoint
        TfsAdminToolTracer.TraceException(TfsAdminToolTracer.TraceSwitch.TraceError, ex);
        return WssVersion.Unknown;
      }
      catch (NotSupportedException ex)
      {
        TfsAdminToolTracer.TraceException(TfsAdminToolTracer.TraceSwitch.TraceError, ex);
        return WssVersion.WSS2;
      }
      catch (NullReferenceException ex)
      {
        /// This exception is hit in case the MicrosoftSharePointTeamServices header is 
        /// missing on the Sharepoint web site. This does not block us from listing the 
        /// SharePoint roles, however we will not be able to validate SharePoint roles
        TfsAdminToolTracer.TraceException(TfsAdminToolTracer.TraceSwitch.TraceError, ex);        
        return WssVersion.Unknown;      
      }
    }
    #endregion

    #region Properties

    public string Url
    {
      get
      {
        return m_UserGroup.Url;
      }
    }

    public SiteStatus SiteStatus
    {
      get
      {
        return m_SiteStatus;
      }
    }

    public WssVersion WssVersion
    {
      get
      {
        return m_WssVersion;
      }
    }
    #endregion
  } //End Class
} //End Namespace