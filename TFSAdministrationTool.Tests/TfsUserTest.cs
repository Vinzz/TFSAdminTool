using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TFSAdministrationTool.Controllers;
using TFSAdministrationTool.Proxy;
using TFSAdministrationTool.Proxy.Common;
using System.Security.Principal;

namespace TFSAdministrationTool.Tests
{
  [TestClass()]
  public class TfsUserTest
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
    }

    #region TfsUser Class unit tests
    [TestMethod]
    public void TfsUserConstructorDefaultTest()
    {
      TfsUser tfsUser = new TfsUser();

      int expected = 0;
      Assert.AreEqual<int>(expected, tfsUser.TfsRoles.All.Count, "Constructor did not create empty TfsRoles collection");
      Assert.AreEqual<int>(expected, tfsUser.SpRoles.All.Count, "Constructor did not create empty SpRoles collection");
      Assert.AreEqual<int>(expected, tfsUser.RsRoles.All.Count, "Constructor did not create empty RsRoles collection");
    }

    [TestMethod]
    public void TfsUserConstructorTest()
    {
      TfsUser tfsUser = InitializeTfsUser(@"domain\username", "UserName",  null, null, null);

      string expected = @"domain\username";
      string actual = tfsUser.UserName;

      Assert.AreEqual<string>(expected, actual, "Constructor did not intialize the user name correctly");
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void TfsUserAddNullRoleTest()
    {
      TfsUser tfsUser = new TfsUser();
      tfsUser.AddRole(null, SystemTier.TeamFoundation);
    }

    [TestMethod]
    public void TfsUserAddRoleTest()
    {
      TfsUser tfsUser = InitializeTfsUser(@"domain\username", "UserName",
                        new string[] { "Project Administrator", "Contributors"},
                        new string[] { "Full Control", "Read" },
                        new string[] { "Content Manager", "Browser" });

      Assert.AreEqual<int>(2, tfsUser.TfsRoles.All.Count, "Role was not added to the TfsRoles collection");
      Assert.AreEqual<string>("Contributors,Project Administrator", tfsUser.TfsRolesString, "TfsRole string is not correct");

      Assert.AreEqual<int>(2, tfsUser.SpRoles.All.Count, "Role was not added to the SpRoles collection");
      Assert.AreEqual<string>("Full Control,Read", tfsUser.SpRolesString, "SpRole string is not correct");

      Assert.AreEqual<int>(2, tfsUser.RsRoles.All.Count, "Role was not added to the RsRoles collection");
      Assert.AreEqual<string>("Browser,Content Manager", tfsUser.RsRolesString, "RsRole string is not correct");
    }

    [TestMethod]
    public void TfsUserCloneTest()
    {
      TfsUser expected = InitializeTfsUser(@"domain\username", "UserName",
                        new string[] { "Project Administrator"},
                        new string[] { "Full Control"},
                        new string[] { "Content Manager"});

      TfsUser actual = TfsUser.Clone(expected);

      Assert.AreEqual<string>(expected.UserName, actual.UserName, "UserName is not correct after cloning");
      Assert.AreEqual<string>(expected.DisplayName, actual.DisplayName, "Display is not correct after cloning");
      Assert.AreEqual<UserState>(expected.State, actual.State, "State is not correct after cloning");

      Assert.AreEqual<string>(expected.TfsRolesString, actual.TfsRolesString, "TfsRoles is not correct after cloning");
      Assert.AreEqual<string>(expected.SpRolesString, actual.SpRolesString, "SpRoles is not correct after cloning");
      Assert.AreEqual<string>(expected.RsRolesString, actual.RsRolesString, "RsRoles is not correct after cloning");
    }

    [TestMethod]
    public void TfsUserCopyRolesTest()
    {
      TfsUser expected = InitializeTfsUser(@"domain\username", "UserName",
                        new string[] { "Project Administrator" },
                        new string[] { "Full Control" },
                        new string[] { "Content Manager" });

      TfsUser actual = new TfsUser(@"domain\username");
      actual.CopyRoles(expected);

      Assert.AreEqual<string>(expected.TfsRolesString, actual.TfsRolesString, "TfsRoles is not correct after cloning");
      Assert.AreEqual<string>(expected.SpRolesString, actual.SpRolesString, "SpRoles is not correct after cloning");
      Assert.AreEqual<string>(expected.RsRolesString, actual.RsRolesString, "RsRoles is not correct after cloning");
    }

    [TestMethod]
    public void TfsUserGetRoleChangesFromNullSourceTest()
    {
      TfsUser targetUser = InitializeTfsUser(@"domain\username", "UserName",
                  new string[] { "Project Administrators" },
                  new string[] { "Full Control" },
                  new string[] { "Content Manager" });

      Dictionary<string, ChangeType> tfsChanges = TfsUser.GetRoleChanges(null, targetUser, SystemTier.TeamFoundation);
      Dictionary<string, ChangeType> spChanges = TfsUser.GetRoleChanges(null, targetUser, SystemTier.SharePoint);
      Dictionary<string, ChangeType> rsChanges = TfsUser.GetRoleChanges(null, targetUser, SystemTier.ReportingServices);

      Assert.AreEqual<int>(1, tfsChanges.Count, "TfsChanges does not contain the correct number of changes");
      Assert.AreEqual<int>(1, spChanges.Count, "SpChanges does not contain the correct number of changes");
      Assert.AreEqual<int>(1, rsChanges.Count, "RsChanges does not contain the correct number of changes");

      Assert.AreEqual<ChangeType>(ChangeType.Add, tfsChanges["Project Administrators"], "TfsChanges does not contain the correct change");
      Assert.AreEqual<ChangeType>(ChangeType.Add, spChanges["Full Control"], "SpChanges does not contain the correct change");
      Assert.AreEqual<ChangeType>(ChangeType.Add, rsChanges["Content Manager"], "RsChanges does not contain the correct change");
    }

    [TestMethod]
    public void TfsUserGetRoleChangesTest()
    {
      TfsUser sourceUser = InitializeTfsUser(@"domain\username", "UserName",
                  new string[] { "Project Administrators" },
                  new string[] { "Full Control" },
                  new string[] { "Content Manager" });

      TfsUser targetUser = InitializeTfsUser(@"domain\username2", "UserName2",
                        new string[] { "Contributors" },
                        new string[] { "Read" },
                        new string[] { "Browser" });

      Dictionary<string, ChangeType> tfsChanges = TfsUser.GetRoleChanges(sourceUser, targetUser, SystemTier.TeamFoundation);
      Dictionary<string, ChangeType> spChanges = TfsUser.GetRoleChanges(sourceUser, targetUser, SystemTier.SharePoint);
      Dictionary<string, ChangeType> rsChanges = TfsUser.GetRoleChanges(sourceUser, targetUser, SystemTier.ReportingServices);

      Assert.AreEqual<int>(2, tfsChanges.Count, "TfsChanges does not contain the correct number of changes");
      Assert.AreEqual<int>(2, spChanges.Count, "SpChanges does not contain the correct number of changes");
      Assert.AreEqual<int>(2, rsChanges.Count, "RsChanges does not contain the correct number of changes");

      Assert.AreEqual<ChangeType>(ChangeType.Delete, tfsChanges["Project Administrators"], "TfsChanges does not contain the correct change");
      Assert.AreEqual<ChangeType>(ChangeType.Add, tfsChanges["Contributors"], "TfsChanges does not contain the correct change");

      Assert.AreEqual<ChangeType>(ChangeType.Delete, spChanges["Full Control"], "SpChanges does not contain the correct change");
      Assert.AreEqual<ChangeType>(ChangeType.Add, spChanges["Read"], "SpChanges does not contain the correct change");

      Assert.AreEqual<ChangeType>(ChangeType.Delete, rsChanges["Content Manager"], "RsChanges does not contain the correct change");
      Assert.AreEqual<ChangeType>(ChangeType.Add, rsChanges["Browser"], "RsChanges does not contain the correct change");
    }

    [TestMethod()]
    public void TfsUserIsUserInRoleTest()
    {
      TfsUser user = InitializeTfsUser(@"domain\username", "UserName",
                  new string[] { "Project Administrators" },
                  new string[] { "Full Control" },
                  new string[] { "Content Manager" });

      bool actual;

      actual = TfsUser.IsUserInRole(user, SystemTier.TeamFoundation, "Project Administrators");
      Assert.AreEqual<bool>(true, actual, "Checking for existing TFS role");

      actual = TfsUser.IsUserInRole(user, SystemTier.SharePoint, "Full Control");
      Assert.AreEqual<bool>(true, actual, "Checking for existing Sp role");

      actual = TfsUser.IsUserInRole(user, SystemTier.ReportingServices, "Content Manager");
      Assert.AreEqual<bool>(true, actual, "Checking for existing Rs role");

      actual = TfsUser.IsUserInRole(user, SystemTier.TeamFoundation, "NoneExistingRole");
      Assert.AreEqual(false, actual, "Checking for non-existing TFS role");

      actual = TfsUser.IsUserInRole(user, SystemTier.SharePoint, "NoneExistingRole");
      Assert.AreEqual(false, actual, "Checking for non-existing Sp role");

      actual = TfsUser.IsUserInRole(user, SystemTier.ReportingServices, "NoneExistingRole");
      Assert.AreEqual(false, actual, "Checking for non-existing Rs role");
    }

    [TestMethod()]
    public void TfsUserIsCurrentUserTest()
    {
      WindowsIdentity currentWindowsUser = WindowsIdentity.GetCurrent(false);

      TfsUser user = InitializeTfsUser(@"domain\user", "UserName", null, null, null);
      TfsUser currentUser = new TfsUser(currentWindowsUser.Name);

      bool actual;

      actual = TfsUser.IsCurrentUser(user);
      Assert.AreEqual<bool>(false, actual, "Incorrect user was detected as Current User");

      actual = TfsUser.IsCurrentUser(currentUser);
      Assert.AreEqual<bool>(true, actual, "Current User was not detected as Current User");
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void TfsUserRemoveNullRoleTest()
    {
      TfsUser user = InitializeTfsUser(@"domain\user", "UserName", null, null, null);
      user.RemoveRole(null, SystemTier.TeamFoundation);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void TfsUserRemoveEmptyRoleTest()
    {
      TfsUser user = InitializeTfsUser(@"domain\user", "UserName", null, null, null);
      user.RemoveRole(String.Empty, SystemTier.TeamFoundation);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void TfsUserRemoveUnknownTierRoleTest()
    {
      TfsUser user = InitializeTfsUser(@"domain\user", "UserName", null, null, null);
      user.RemoveRole("Crappy indata", (SystemTier)99);
    }

    [TestMethod()]
    public void TfsUserRemoveRoleTest()
    {
      TfsUser user = InitializeTfsUser(@"domain\username", "UserName",
                  new string[] { "Project Administrators" },
                  new string[] { "Full Control" },
                  new string[] { "Content Manager" });

      user.RemoveRole("Project Administrators", SystemTier.TeamFoundation);
      user.RemoveRole("Full Control", SystemTier.SharePoint);
      user.RemoveRole("Content Manager", SystemTier.ReportingServices);

      Assert.AreEqual<int>(0, user.TfsRoles.All.Count, "Tfs role was not removed correctly");
      Assert.AreEqual<int>(0, user.SpRoles.All.Count, "Sp role was not removed correctly");
      Assert.AreEqual<int>(0, user.RsRoles.All.Count, "Rs role was not removed correctly");
    }
    #endregion

    #region TfsUserCollection Class unit tests
    [TestMethod]
    public void TfsUserCollectionConstructorTest()
    {
      TfsUserCollection users = new TfsUserCollection();
      Assert.AreEqual<int>(0, users.Users.Count, "TfsUserCollection constructor does not create an empty collection");
    }

    [TestMethod]
    public void TfsUserCollectionNewUserApplyPendingChangesTest()
    {
      // Create empty list of users
      TfsUserCollection sourceUsers = new TfsUserCollection();
      
      // Initialize the pending changes for adding a new user
      Dictionary<Guid, PendingChange> pendingChanges = new Dictionary<Guid, PendingChange>();
      pendingChanges.Add(Guid.NewGuid(), new PendingChange(true, false, @"domain\username", "UserName", "foo@bar.baz", ChangeType.Add, m_mockTFS.SelectedTeamProject, m_mockTFS.Server.Name, m_mockTFS.Server.InstanceId, SystemTier.TeamFoundation, "Project Administrators"));
      pendingChanges.Add(Guid.NewGuid(), new PendingChange(true, false, @"domain\username", "UserName", "foo@bar.baz", ChangeType.Add, m_mockTFS.SelectedTeamProject, m_mockTFS.Server.Name, m_mockTFS.Server.InstanceId, SystemTier.SharePoint, "Full Control"));
      pendingChanges.Add(Guid.NewGuid(), new PendingChange(true, false, @"domain\username", "UserName", "foo@bar.baz", ChangeType.Add, m_mockTFS.SelectedTeamProject, m_mockTFS.Server.Name, m_mockTFS.Server.InstanceId, SystemTier.ReportingServices, "Content Manager"));

      // Apply the pending changes
      sourceUsers.ApplyPendingChanges(pendingChanges);

      // Check that the user was marked as new user
      Assert.AreEqual<UserState>(UserState.New, sourceUsers.Users[0].State, "Newly added user does not have the correct state");
      
      // Check that the role has been added
      Assert.AreEqual<int>(1, sourceUsers.Users[0].TfsRoles.All.Count, "TfsRole was not added");
      Assert.AreEqual<int>(1, sourceUsers.Users[0].SpRoles.All.Count, "SpRole was not added");
      Assert.AreEqual<int>(1, sourceUsers.Users[0].RsRoles.All.Count, "RsRole was not added");

      Assert.AreEqual<string>("Project Administrators", sourceUsers.Users[0].TfsRolesString, "Correct TfsRole was not added");
      Assert.AreEqual<string>("Full Control", sourceUsers.Users[0].SpRolesString, "Correct SpRole was not added");
      Assert.AreEqual<string>("Content Manager", sourceUsers.Users[0].RsRolesString, "Correct RsRole was not added");

      // Initialize the pending changes for deleting a role
      pendingChanges.Clear();
      pendingChanges.Add(Guid.NewGuid(), new PendingChange(true, false, @"domain\username", "UserName", "foo@bar.baz", ChangeType.Delete, m_mockTFS.SelectedTeamProject, m_mockTFS.Server.Name, m_mockTFS.Server.InstanceId, SystemTier.ReportingServices, "Content Manager"));

      sourceUsers.ApplyPendingChanges(pendingChanges);

      // Test that the role was removed from the collection
      Assert.AreEqual<UserState>(UserState.New, sourceUsers.Users[0].State, "Newly added user does not have the correct state");
      Assert.AreEqual<int>(0, sourceUsers.Users[0].RsRoles.All.Count, "RsRole was not removed correctly");
      Assert.AreEqual<string>(String.Empty, sourceUsers.Users[0].RsRolesString, "Correct RsRole was not added");

      // Initialize the pending changes for deleting the user
      pendingChanges.Clear();
      pendingChanges.Add(Guid.NewGuid(), new PendingChange(true, false, @"domain\username", "UserName", "foo@bar.baz", ChangeType.Delete, m_mockTFS.SelectedTeamProject, m_mockTFS.Server.Name, m_mockTFS.Server.InstanceId, SystemTier.TeamFoundation, "All"));

      sourceUsers.ApplyPendingChanges(pendingChanges);
    
      // Check that the user is marked as deleted
      Assert.AreEqual<UserState>(UserState.Deleted, sourceUsers.Users[0].State, "Deleted user does not have the correct state");
    }

    [TestMethod]
    public void TfsUserCollectionUserApplyPendingChangesTest()
    {
      // Create list of users
      TfsUserCollection sourceUsers = new TfsUserCollection();
      sourceUsers.Add(InitializeTfsUser(@"domain\username", "UserName", new string[] { "Project Administrators" }, new string[] { "Full Control" }, new string[] { "Content Manager" }));

      // Initialize the pending changes for adding a role
      Dictionary<Guid, PendingChange> pendingChanges = new Dictionary<Guid, PendingChange>();
      pendingChanges.Add(Guid.NewGuid(), new PendingChange(true, false, @"domain\username", "UserName", "foo@bar.baz", ChangeType.Add, m_mockTFS.SelectedTeamProject, m_mockTFS.Server.Name, m_mockTFS.Server.InstanceId, SystemTier.SharePoint, "Read"));

      // Apply the pending changes
      sourceUsers.ApplyPendingChanges(pendingChanges);

      // Check that the user was marked as new user
      Assert.AreEqual<UserState>(UserState.Edited, sourceUsers.Users[0].State, "Edited user does not have the correct state");

      // Check that the role has been added
      Assert.AreEqual<int>(2, sourceUsers.Users[0].SpRoles.All.Count, "SpRole does not contain the correct roles");
      Assert.AreEqual<string>("Full Control,Read", sourceUsers.Users[0].SpRolesString, "Correct SpRole was not added");

      // Initialize the pending changes for deleting a role
      pendingChanges.Clear();
      pendingChanges.Add(Guid.NewGuid(), new PendingChange(true, false, @"domain\username", "UserName", "foo@bar.baz", ChangeType.Delete, m_mockTFS.SelectedTeamProject, m_mockTFS.Server.Name, m_mockTFS.Server.InstanceId, SystemTier.ReportingServices, "Content Manager"));

      sourceUsers.ApplyPendingChanges(pendingChanges);

      // Test that the role was removed from the collection
      Assert.AreEqual<UserState>(UserState.Edited, sourceUsers.Users[0].State, "Edited user does not have the correct state");
      Assert.AreEqual<int>(0, sourceUsers.Users[0].RsRoles.All.Count, "RsRole was not removed correctly");
      Assert.AreEqual<string>(String.Empty, sourceUsers.Users[0].RsRolesString, "Correct RsRole was not added");
    }
    
    [TestMethod]
    public void TfsUserCollectionAddTest()
    {
      TfsUserCollection users = new TfsUserCollection();

      users.Add(InitializeTfsUser(@"domain\username", "UserName", null, null, null));
      Assert.AreEqual<int>(1, users.Users.Count, "TfsUserCollection add does not add the user to the collection");
    }

    [TestMethod]
    public void TfsUserCollectionCloneTest()
    {
      TfsUserCollection sourceUsers = new TfsUserCollection();
      sourceUsers.Add(InitializeTfsUser(@"domain\username", "UserName", null, null, null));

      TfsUserCollection targetUsers = TfsUserCollection.Clone(sourceUsers);
      
      Assert.AreEqual<int>(sourceUsers.Users.Count, targetUsers.Users.Count, "TfsUserCollection clone did not return the expected collection");
    }
    
    [TestMethod]
    public void TfsUserCollectionCopyUserRolesTest()
    {
      TfsUserCollection sourceUsers = new TfsUserCollection();
      sourceUsers.Add(InitializeTfsUser(@"domain\username", "UserName", new string[] { "Project Administrators" }, new string[] { "Full Control" }, new string[] { "Content Manager" }));
      sourceUsers.Add(InitializeTfsUser(@"domain\username2", "UserName2", new string[] { "Project Administrators" }, new string[] { "Full Control" }, new string[] { "Content Manager" }));

      TfsUserCollection targetUsers = new TfsUserCollection();
      targetUsers.Add(InitializeTfsUser(@"domain\username2", "UserName2", new string[] { "Contributors" }, new string[] { "Read" }, new string[] { "Browser" }));      
      targetUsers.CopyUserRoles(sourceUsers);

      Assert.AreEqual<int>(sourceUsers.Users.Count, targetUsers.Users.Count, "TfsUserCollection copy roles did not return the expected collection");
      Assert.AreEqual<string>(sourceUsers.Users[0].TfsRolesString, targetUsers.Users[0].TfsRolesString, "TfsUserCollection copy roles did copy the TfsRoles");
      Assert.AreEqual<string>(sourceUsers.Users[0].SpRolesString, targetUsers.Users[0].SpRolesString, "TfsUserCollection copy roles did copy the SpRoles");
      Assert.AreEqual<string>(sourceUsers.Users[0].RsRolesString, targetUsers.Users[0].RsRolesString, "TfsUserCollection copy roles did copy the RsRoles");

      Assert.AreEqual<string>(sourceUsers.Users[1].TfsRolesString, targetUsers.Users[1].TfsRolesString, "TfsUserCollection copy roles did copy the TfsRoles");
      Assert.AreEqual<string>(sourceUsers.Users[1].SpRolesString, targetUsers.Users[1].SpRolesString, "TfsUserCollection copy roles did copy the SpRoles");
      Assert.AreEqual<string>(sourceUsers.Users[1].RsRolesString, targetUsers.Users[1].RsRolesString, "TfsUserCollection copy roles did copy the RsRoles");    
    }

    [TestMethod]
    public void TfsUserCollectionGetUserNonExistentTest()
    {
      TfsUserCollection sourceUsers = new TfsUserCollection();
      sourceUsers.Add(InitializeTfsUser(@"domain\username", "UserName", new string[] { "Project Administrators" }, new string[] { "Full Control" }, new string[] { "Content Manager" }));

      TfsUser actual = sourceUsers.GetUser(@"domain\invalidusername");

      Assert.IsNull(actual, "TfsUserCollection GetUser returned an invalid user");
    }

    [TestMethod]
    public void TfsUserCollectionGetUserTest()
    {
      TfsUserCollection sourceUsers = new TfsUserCollection();
      sourceUsers.Add(InitializeTfsUser(@"domain\username", "UserName", new string[] { "Project Administrators" }, new string[] { "Full Control" }, new string[] { "Content Manager" }));

      TfsUser expected = sourceUsers.Users[0];
      TfsUser actual = sourceUsers.GetUser(@"domain\username");

      Assert.AreEqual<string>(expected.UserName, actual.UserName, "TfsUserCollection GetUser did not return the user with correct user name");
      Assert.AreEqual<string>(expected.DisplayName, actual.DisplayName, "TfsUserCollection GetUser did not return the user with correct display name");
      Assert.AreEqual<string>(expected.TfsRolesString, actual.TfsRolesString, "TfsUserCollection GetUser did not return the user with correct TfsRoles");
      Assert.AreEqual<string>(expected.SpRolesString, actual.SpRolesString, "TfsUserCollection GetUser did not return the user with correct SpRoles");
      Assert.AreEqual<string>(expected.RsRolesString, actual.RsRolesString, "TfsUserCollection GetUser did not return the user with correct RsRoles");
    }

    [TestMethod]
    public void TfsUserCollectionRemoveTest()
    {
      TfsUserCollection sourceUsers = new TfsUserCollection();
      sourceUsers.Add(InitializeTfsUser(@"domain\username", "UserName", new string[] { "Project Administrators" }, new string[] { "Full Control" }, new string[] { "Content Manager" }));
      sourceUsers.Add(InitializeTfsUser(@"domain\username2", "UserName2", new string[] { "Project Administrators" }, new string[] { "Full Control" }, new string[] { "Content Manager" }));

      sourceUsers.Remove(@"domain\username2");
      Assert.AreEqual<int>(1, sourceUsers.Users.Count, "TfsUserCollection Remove did not remove the second user");

      sourceUsers.Remove(@"domain\username");
      Assert.AreEqual<int>(0, sourceUsers.Users.Count, "TfsUserCollection Remove did not remove the first user");
    }
    
    #endregion

    #region Helper methods
    private TfsUser InitializeTfsUser(string userName, string displayName, string[] tfsRoles, string[] spRoles, string[] rsRoles)
    {
      TfsUser user = new TfsUser(userName);
      if (displayName != null) user.DisplayName = displayName;

      if (tfsRoles != null && tfsRoles.Length > 0)
      {
        foreach (string role in tfsRoles)
        {
          user.AddRole(role, SystemTier.TeamFoundation);
        }
      }
      if (spRoles != null && spRoles.Length > 0)
      {
        foreach (string role in spRoles)
        {
          user.AddRole(role, SystemTier.SharePoint);
        }
      }
      if (rsRoles != null && rsRoles.Length > 0)
      {
        foreach (string role in rsRoles)
        {
          user.AddRole(role, SystemTier.ReportingServices);
        }
      }

      return user;
    }
    #endregion
  }
}