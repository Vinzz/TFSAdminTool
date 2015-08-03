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
  public class HistoryTest
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
    public void GetNewHistoryItemsTest()
    {
      History target = new History();
      List<HistoryItem> actual;

      target.AddHistoryItem("TU", "TestUser", ChangeType.Add, m_mockTFS.ServerName, m_mockTFS.SelectedTeamProject, SystemTier.TeamFoundation, "Role1", Status.Passed);
      target.AddHistoryItem("TU", "TestUser", ChangeType.Delete, m_mockTFS.ServerName, m_mockTFS.SelectedTeamProject, SystemTier.TeamFoundation, "Role1", Status.Passed);
      target.AddHistoryItem("TU", "TestUser", ChangeType.Add, m_mockTFS.ServerName, m_mockTFS.SelectedTeamProject, SystemTier.TeamFoundation, "Role2", Status.Passed);

      actual = target.GetNewHistoryItems();
      Assert.AreEqual<int>(3, actual.Count, "Retrieving History using empty index");

      target.AddHistoryItem("TU", "TestUser", ChangeType.Delete, m_mockTFS.ServerName, m_mockTFS.SelectedTeamProject, SystemTier.TeamFoundation, "Role2", Status.Passed);

      actual = target.GetNewHistoryItems();
      Assert.AreEqual<int>(1, actual.Count, "Retrieving History using index");
    }
  }
}
