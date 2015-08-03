using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TFSAdministrationTool.Proxy;
using TFSAdministrationTool.Proxy.Common;
using TFSAdministrationTool.Tests.Helpers;
using Microsoft.TeamFoundation.Server;
using System.Collections;
using Microsoft.TeamFoundation.Client;
using System.Net;

namespace TFSAdministrationTool.Tests
{
  public class MockTeamFoundationServerProxy : ITeamFoundationServerProxy
  {
    #region Fields

    private ISharePointProxy m_SharePointProxy;
    private IReportServiceProxy m_ReportServiceProxy;

    private TfsUserCollection m_TfsUsers = new TfsUserCollection();
    private TfsUserCollection m_TfsUsersClean = new TfsUserCollection();

    private RoleValidator m_RoleValidator;

    public string m_SelectedServerName = "TestServer";
    public string m_SelectedProjectName = "TestProject1";
    private TfsVersion m_ServerVersion = TfsVersion.TfsLegacy;

    public List<string> m_Projects = new List<string>() { "TestProject1", "TestProject2" };

    #endregion

    #region Factory

    public static ITeamFoundationServerProxy CreateMockProxy()
    {
      return new MockTeamFoundationServerProxy();
    }

    #endregion

    #region Constructors

    public MockTeamFoundationServerProxy()
    {
      TfsAdminToolTracer.Initialize(new TextBox());

      m_SharePointProxy = SharePointProxyFactory.CreateProxy("@http://localhost:80/sites/Samples", SiteStatus.Available, CredentialCache.DefaultNetworkCredentials);
      m_ReportServiceProxy = ReportServiceProxyFactory.CreateReportServiceProxy("@http://localhost:80/ReportServer/ReportService2005.asmx", "/", SiteStatus.Available, CredentialCache.DefaultNetworkCredentials);

      m_RoleValidator = new RoleValidator(this);
      //m_RoleValidator.Initialize(SecurityInfoHelper.GetTfsSecurityInfo(), SecurityInfoHelper.GetSpSecurityInfo(), SecurityInfoHelper.GetRsSecurityInfo());
    }

    #endregion

    #region ITeamFoundationServerProxy Members

    void ITeamFoundationServerProxy.Connect(Uri serverUri, ICredentials credentials)
    {
    }

    void ITeamFoundationServerProxy.AddUserToRole(string teamProject, string userName, string role)
    {
      throw new NotImplementedException();
    }

    void ITeamFoundationServerProxy.RemoveUserFromRole(string teamProject, string userName, string role)
    {
      throw new NotImplementedException();
    }

    void ITeamFoundationServerProxy.RemoveUser(string teamProject, string userName)
    {
      throw new NotImplementedException();
    }

    void ITeamFoundationServerProxy.UpdateTeamProjects(ProjectInfo[] projects)
    {
      throw new NotImplementedException();
    }

    void ITeamFoundationServerProxy.SharePointAddUserToRole(string teamProject, string userName, string role)
    {
      throw new NotImplementedException();
    }

    void ITeamFoundationServerProxy.SharePointRemoveUserFromRole(string teamProject, string userName, string role)
    {
      throw new NotImplementedException();
    }

    void ITeamFoundationServerProxy.SharePointRemoveUser(string teamProject, string userName)
    {
      throw new NotImplementedException();
    }

    void ITeamFoundationServerProxy.ReportingServiceAddUserToRole(string teamProject, string userName, string role)
    {
      throw new NotImplementedException();
    }

    void ITeamFoundationServerProxy.ReportingServiceRemoveUserFromRole(string teamProject, string userName, string role)
    {
      throw new NotImplementedException();
    }

    void ITeamFoundationServerProxy.ReportingServiceRemoveUser(string teamProject, string userName)
    {
      throw new NotImplementedException();
    }

    void ITeamFoundationServerProxy.SelectTeamProject(string name)
    {
      throw new NotImplementedException();
    }

    TfsUser ITeamFoundationServerProxy.GetUser(string userName)
    {
      if (userName == "Ladislau Szomoru" || userName == "Peter Blomqvist")
        return new TfsUser(userName);

      return null;
    }

    void ITeamFoundationServerProxy.InitializeServerAndTeamProjectUsers()
    {
      throw new NotImplementedException();
    }

    string ITeamFoundationServerProxy.GetTeamProjectUri(string name)
    {
      throw new NotImplementedException();
    }

    TfsUserCollection ITeamFoundationServerProxy.GetTeamProjectUsers(string name)
    {
      throw new NotImplementedException();
    }

    bool ITeamFoundationServerProxy.IsUserServiceAccount(TfsUser user)
    {
      throw new NotImplementedException();
    }

    bool ITeamFoundationServerProxy.IsUserTeamFoundationAdministrator(TfsUser user)
    {
      throw new NotImplementedException();
    }

    Dictionary<string, string> ITeamFoundationServerProxy.ShowAddGroupControl(IWin32Window parentWin)
    {
      Dictionary<string, string> result = new Dictionary<string, string>();
      result.Add("pblomqvist", "Peter Blomqvist");
      result.Add("lszomoru", "Ladislau Szomoru");
      return result;
    }

    TfsTeamProjectCollection ITeamFoundationServerProxy.Server
    {
        get { throw new NotImplementedException(); }
    }
    
    ProjectInfo[] ITeamFoundationServerProxy.TeamProjects
    {
      get { throw new NotImplementedException();}
    }

    TfsVersion ITeamFoundationServerProxy.ServerVersion
    {
      get { return m_ServerVersion; }
    }

    ItemType ITeamFoundationServerProxy.SelectedItemType
    {
      get { throw new NotImplementedException(); }
    }

    string ITeamFoundationServerProxy.SelectedTeamProject
    {
      get { return m_SelectedProjectName; }
    }

    RoleValidator ITeamFoundationServerProxy.RoleValidator
    {
      get { return m_RoleValidator; }
    }

    TfsUserCollection ITeamFoundationServerProxy.UserCollection
    {
      get { return m_TfsUsers; }
    }

    TfsUserCollection ITeamFoundationServerProxy.UserCollectionClean
    {
      get { return m_TfsUsersClean; }
    }
    #endregion

    #region ITeamFoundationServerProxy Members
    ISharePointProxy ITeamFoundationServerProxy.GetSharePointProxy(string teamProject)
    {
      throw new NotImplementedException();
    }

    IReportServiceProxy ITeamFoundationServerProxy.ReportServiceProxy
    {
      get { throw new NotImplementedException(); }
    }

    #endregion


    public void SetSharePointClaimBasedAuthenticationMode(bool p)
    {
        throw new NotImplementedException();
    }


    public Guid ServerInstanceId
    {
        get { return new Guid(); }
    }


    public string ServerName
    {
        get { return "TestServer"; }
    }
  }
}