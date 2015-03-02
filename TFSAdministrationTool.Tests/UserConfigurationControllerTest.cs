using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TFSAdministrationTool.Controllers;
using TFSAdministrationTool.Proxy;
using TFSAdministrationTool.Proxy.Common;
using System.Windows.Forms;

namespace TFSAdministrationTool.Tests
{
  [TestClass()]
  public class UserConfigurationControllerTest
  {
    public TestContext TestContext { get; set; }
    
    private ITeamFoundationServerProxy m_mockTFS;

    [ClassInitialize()]
    public static void ClassInitialize(TestContext context)
    {
      //Hookup factories for mock objects
      TeamFoundationServerProxyFactory.CreateProxy = new TeamFoundationServerProxyFactory.Factory(MockTeamFoundationServerProxy.CreateMockProxy);
      SharePointProxyFactory.CreateProxy = new SharePointProxyFactory.Factory(MockSharePointProxy.CreateMockProxy);
      ReportServiceProxyFactory.CreateProxy = new ReportServiceProxyFactory.Factory(MockReportServiceProxy.CreateMockProxy);
    }

    [TestInitialize()]
    public void TestInitialize()
    {
      m_mockTFS = (MockTeamFoundationServerProxy)TeamFoundationServerProxyFactory.CreateTeamFoundationServerProxy();

      UserController.Initialize(m_mockTFS, new TfsUserCollection());
      MainController.PendingChanges.All.Clear();
    }

    [TestMethod()]
    public void AddUser_WithNoConflictTest()
    {
      TfsUser user = new TfsUser("TestUser");
            
      bool expected = true;
      bool actual = UserController.AddUser(user, true);

      Assert.AreEqual<bool>(expected, actual);
      Assert.AreEqual<int>(1, UserController.UserCollection.Users.Count, "Number of users after test differs");
    }

    [TestMethod()]
    public void AddUser_NoOpControlWhenAlreadtInEditSessionTest()
    {
      TfsUser user = new TfsUser("TestUser");
      UserController.UserCollection.Users.Add(user);

      bool expected = false;
      bool actual = UserController.AddUser(user, true);

      Assert.AreEqual<bool>(expected, actual);
      Assert.AreEqual<int>(1, UserController.UserCollection.Users.Count, "Number of users after test differs");
    }

    [TestMethod()]
    [ExpectedException(typeof(UserAlreadyExistException))]
    public void AddUser_AlreadyExistsInProjectCheckTest()
    {
      TfsUser user = new TfsUser("TestUser");

      m_mockTFS.UserCollection.Add(user); 
      
      UserController.AddUser(user, true);
    }

    [TestMethod()]
    public void AddUser_ByPassAlreadyExistsInProjectCheckTest()
    {
      TfsUser user = new TfsUser("TestUser");

      m_mockTFS.UserCollection.Add(user);

      bool expected = true;
      bool actual = UserController.AddUser(user, false);

      Assert.AreEqual<bool>(expected, actual);
      Assert.AreEqual<int>(1, UserController.UserCollection.Users.Count, "Number of users after test differs");
    }

    [TestMethod()]
    [ExpectedException(typeof(UserHasPendingChangesException))]
    public void AddUserWithPendingChangesTest()
    {
      TfsUser user = new TfsUser("TestUser");

      MainController.PendingChanges.Add(true, true, user.UserName, user.DisplayName, user.Email, ChangeType.Add, m_mockTFS.SelectedTeamProject, m_mockTFS.Server.Name, m_mockTFS.Server.InstanceId, SystemTier.TeamFoundation, "TestRole");
      
      UserController.AddUser(user,true);
    }

    [TestMethod()]
    public void GetAvailableRolesBySystem_TeamFoundationTierTest()
    {
      RoleInfoCollection actual = UserController.GetAvailableRolesBySystem(SystemTier.TeamFoundation);

      Assert.AreEqual<int>(4, actual.All.Count);
      Assert.IsNotNull(actual.All.Find(delegate(RoleInfo ri) { return string.Compare(ri.Name, "Readers", true) == 0; }));
      Assert.IsNotNull(actual.All.Find(delegate(RoleInfo ri) { return string.Compare(ri.Name, "Contributors", true) == 0; }));
      Assert.IsNotNull(actual.All.Find(delegate(RoleInfo ri) { return string.Compare(ri.Name, "Project Administrators", true) == 0; }));
      Assert.IsNotNull(actual.All.Find(delegate(RoleInfo ri) { return string.Compare(ri.Name, "Build Services", true) == 0; }));
    }

    [TestMethod()]
    public void GetAvailableRolesBySystem_SharePointTierTest()
    {
      RoleInfoCollection actual = UserController.GetAvailableRolesBySystem(SystemTier.SharePoint);

      Assert.AreEqual(4, actual.All.Count);
      Assert.IsNotNull(actual.All.Find(delegate(RoleInfo ri) { return string.Compare(ri.Name, "Full Control", true) == 0; }));
      Assert.IsNotNull(actual.All.Find(delegate(RoleInfo ri) { return string.Compare(ri.Name, "Design", true) == 0; }));
      Assert.IsNotNull(actual.All.Find(delegate(RoleInfo ri) { return string.Compare(ri.Name, "Contribute", true) == 0; }));
      Assert.IsNotNull(actual.All.Find(delegate(RoleInfo ri) { return string.Compare(ri.Name, "Read", true) == 0; }));
    }

    [TestMethod()]
    public void GetAvailableRolesBySystem_ReportingServiceTierTest()
    {
      RoleInfoCollection actual = UserController.GetAvailableRolesBySystem(SystemTier.ReportingServices);

      Assert.AreEqual(5, actual.All.Count);
      Assert.IsNotNull(actual.All.Find(delegate(RoleInfo ri) { return string.Compare(ri.Name, "Browser", true) == 0; }));
      Assert.IsNotNull(actual.All.Find(delegate(RoleInfo ri) { return string.Compare(ri.Name, "Content Manager", true) == 0; }));
      Assert.IsNotNull(actual.All.Find(delegate(RoleInfo ri) { return string.Compare(ri.Name, "My Reports", true) == 0; }));
      Assert.IsNotNull(actual.All.Find(delegate(RoleInfo ri) { return string.Compare(ri.Name, "Publisher", true) == 0; }));
      Assert.IsNotNull(actual.All.Find(delegate(RoleInfo ri) { return string.Compare(ri.Name, "Report Builder", true) == 0; }));
    }

    [TestMethod()]
    public void GetAvailableRolesBySystem_UnknownTierTest()
    {
      RoleInfoCollection actual = UserController.GetAvailableRolesBySystem((SystemTier)6);
      Assert.IsNull(actual);
    }

    [TestMethod()]
    public void InitializeForAddTest()
    {
      UserController.Initialize(m_mockTFS, null);
      Assert.IsNotNull(UserController.UserCollection.Users);
    }

    [TestMethod()]
    public void InitializeForEditTest()
    {
      TfsUserCollection users = new TfsUserCollection();
      users.Add(new TfsUser("TestUser1"));
      UserController.Initialize(m_mockTFS, users);

      Assert.IsNotNull(UserController.UserCollection.Users);
      Assert.AreEqual<int>(1, UserController.UserCollection.Users.Count);
    }

    [TestMethod()]
    public void GetMappedRolesBySystem_TeamFoundationTierForSingleRoleTest()
    {
      string roles = "Contributors";

      RoleInfoCollection actual = UserController.GetMappedRolesBySystem(roles, SystemTier.TeamFoundation);

      Assert.AreEqual(0, actual.All.Count);
    }

    [TestMethod()]
    public void GetMappedRolesBySystem_TeamFoundationTierForMultipleRoleTest()
    {
      string roles = "Contributors;Readers";

      RoleInfoCollection actual = UserController.GetMappedRolesBySystem(roles, SystemTier.TeamFoundation);

      Assert.AreEqual(0, actual.All.Count);
    }

    [TestMethod()]
    public void GetMappedRolesBySystem_SharePointTierForSingleRoleTest()
    {
      string roles = "Contributors";

      RoleInfoCollection actual = UserController.GetMappedRolesBySystem(roles, SystemTier.SharePoint);

      Assert.AreEqual(2, actual.All.Count);
      Assert.IsNotNull(actual.All.Find(delegate(RoleInfo ri) { return string.Compare(ri.Name, "Contribute", true) == 0; }));
      Assert.IsNotNull(actual.All.Find(delegate(RoleInfo ri) { return string.Compare(ri.Name, "Design", true) == 0; }));
    }

    [TestMethod()]
    public void GetMappedRolesBySystem_SharePointTierForMultipleRoleTest()
    {
      string roles = "Contributors;Readers";

      RoleInfoCollection actual = UserController.GetMappedRolesBySystem(roles, SystemTier.SharePoint);

      Assert.AreEqual(3, actual.All.Count);
      Assert.IsNotNull(actual.All.Find(delegate(RoleInfo ri) { return string.Compare(ri.Name, "Contribute", true) == 0; }));
      Assert.IsNotNull(actual.All.Find(delegate(RoleInfo ri) { return string.Compare(ri.Name, "Design", true) == 0; }));
      Assert.IsNotNull(actual.All.Find(delegate(RoleInfo ri) { return string.Compare(ri.Name, "Read", true) == 0; }));
    }

    [TestMethod()]
    public void GetMappedRolesBySystem_ReportingServiceTierForSingleRoleTest()
    {
      string roles = "Contributors";

      RoleInfoCollection actual = UserController.GetMappedRolesBySystem(roles, SystemTier.ReportingServices);

      Assert.AreEqual(2, actual.All.Count);
      Assert.IsNotNull(actual.All.Find(delegate(RoleInfo ri) { return string.Compare(ri.Name, "Publisher", true) == 0; }));
      Assert.IsNotNull(actual.All.Find(delegate(RoleInfo ri) { return string.Compare(ri.Name, "Report Builder", true) == 0; }));
    }

    [TestMethod()]
    public void GetMappedRolesBySystem_ReportingServiceForMultipleRoleTest()
    {
      string roles = "Contributors;Readers";

      RoleInfoCollection actual = UserController.GetMappedRolesBySystem(roles, SystemTier.ReportingServices);

      Assert.AreEqual(3, actual.All.Count);
      Assert.IsNotNull(actual.All.Find(delegate(RoleInfo ri) { return string.Compare(ri.Name, "Publisher", true) == 0; }));
      Assert.IsNotNull(actual.All.Find(delegate(RoleInfo ri) { return string.Compare(ri.Name, "Report Builder", true) == 0; }));
      Assert.IsNotNull(actual.All.Find(delegate(RoleInfo ri) { return string.Compare(ri.Name, "Browser", true) == 0; }));
    }

    [TestMethod()]
    public void ResolveUsers_NoMatchTest()
    {
      string[] users = { "Jane Doe", "John Doe"};

      TfsUserCollection actual = UserController.ResolveUsers(users);

      Assert.AreEqual<int>(0, actual.Users.Count);
    }

    [TestMethod()]
    public void ResolveUsers_PartialMatchTest()
    {
      string[] users = { "Ladislau Szomoru", "Jane Doe" };

      TfsUserCollection actual = UserController.ResolveUsers(users);

      Assert.AreEqual<int>(1, actual.Users.Count);
    }

    [TestMethod()]
    public void ResolveUsers_PartialMatchFromExistingUsersTest()
    {
      m_mockTFS.UserCollection.Add(new TfsUser("Brian Harry"));
      m_mockTFS.UserCollection.Add(new TfsUser("Buck Hodges"));

      string[] users = { "Brian Harry", "Jane Doe" };

      TfsUserCollection actual = UserController.ResolveUsers(users);

      Assert.AreEqual<int>(1, actual.Users.Count);
    }

    [TestMethod()]
    public void BrowseUsersTest()
    {
      TfsUserCollection actual = UserController.BrowseUsers(null);
      Assert.AreEqual<int>(2, actual.Users.Count);
    }
  }
}
