using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TFSAdministrationTool.Controllers;
using TFSAdministrationTool.Proxy;
using TFSAdministrationTool.Proxy.Common;

namespace TFSAdministrationTool.Tests
{
  [TestClass()]
  public class PendingChangesTest
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
    
    [TestMethod()]
    public void Add_NoConflictTest()
    {
      PendingChanges target = new PendingChanges();

      target.Add(false, false, "TU", "TestUser", "foo@bar.baz", ChangeType.Add, m_mockTFS.SelectedTeamProject, m_mockTFS.Server.Name, m_mockTFS.Server.InstanceId, SystemTier.TeamFoundation, "Role1");
      Assert.AreEqual<int>(1, target.All.Count, "Not Checked, Not Selected");

      target.Add(true, false, "TU", "TestUser", "foo@bar.baz", ChangeType.Add, m_mockTFS.SelectedTeamProject, m_mockTFS.Server.Name, m_mockTFS.Server.InstanceId, SystemTier.TeamFoundation, "Role1");
      Assert.AreEqual<int>(2, target.All.Count, "Checked, Not Selected");
      Assert.AreEqual<int>(1, target.Checked.Count, "Checked, Not Selected");

      target.Add(false, true, "TU", "TestUser", "foo@bar.baz", ChangeType.Add, m_mockTFS.SelectedTeamProject, m_mockTFS.Server.Name, m_mockTFS.Server.InstanceId, SystemTier.TeamFoundation, "Role1");
      Assert.AreEqual<int>(3, target.All.Count, "Not Checked, Selected");
      Assert.AreEqual<int>(1, target.Checked.Count, "Not Checked, Selected");
    }

    [TestMethod()]
    public void Add_OppositePendingChangeExistTest()
    {
      PendingChanges target = new PendingChanges();

      target.Add(false, false, "TU", "TestUser", "foo@bar.baz", ChangeType.Add, m_mockTFS.SelectedTeamProject, m_mockTFS.Server.Name, m_mockTFS.Server.InstanceId, SystemTier.TeamFoundation, "Role1");
      Assert.AreEqual<int>(1, target.All.Count, "Change Type Add");
      target.Add(false, false, "TU", "TestUser", "foo@bar.baz", ChangeType.Delete, m_mockTFS.SelectedTeamProject, m_mockTFS.Server.Name, m_mockTFS.Server.InstanceId, SystemTier.TeamFoundation, "Role1");
      Assert.AreEqual<int>(0, target.All.Count, "Change Type Delete removes Add");

      target.Add(false, false, "TU", "TestUser", "foo@bar.baz", ChangeType.Delete, m_mockTFS.SelectedTeamProject, m_mockTFS.Server.Name, m_mockTFS.Server.InstanceId, SystemTier.TeamFoundation, "Role1");
      Assert.AreEqual<int>(1, target.All.Count, "Change Type Delete");
      target.Add(false, false, "TU", "TestUser", "foo@bar.baz", ChangeType.Add, m_mockTFS.SelectedTeamProject, m_mockTFS.Server.Name, m_mockTFS.Server.InstanceId, SystemTier.TeamFoundation, "Role1");
      Assert.AreEqual<int>(0, target.All.Count, "Change Type Add removes Delete");
    }
  }
}
