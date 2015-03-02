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
  public class MainControllerTest
  {
    public TestContext TestContext { get; set; }

    private MockTeamFoundationServerProxy m_mockTFS;

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
    public void OnServerConnectTest()
    {
      //ServerInfo actual = MainController.OnServerConnect(m_mockTFS);

      //Assert.AreEqual<string>("TestServer", actual.Name);
      //Assert.AreEqual<int>(2, actual.Projects.Count);
      //Assert.AreEqual<bool>(true, actual.Projects.Contains("TestProject1"));
      //Assert.AreEqual<bool>(true, actual.Projects.Contains("TestProject2"));
    }

    [TestMethod()]
    public void OnServerConnect_NoServerSelectedTest()
    {
      //m_mockTFS.m_SelectedServerName = "";
      //Assert.IsNull(MainController.OnServerConnect(m_mockTFS));
    }

    [TestMethod()]
    public void OnServerRefreshTest()
    {
      //ServerInfo connect = MainController.OnServerConnect(m_mockTFS);
      //ServerInfo refresh = MainController.OnServerRefresh();

      //Assert.AreEqual<string>("TestServer", connect.Name);
      //Assert.AreEqual<int>(2, connect.Projects.Count);
      //Assert.AreEqual<bool>(true, connect.Projects.Contains("TestProject1"));
      //Assert.AreEqual<bool>(true, connect.Projects.Contains("TestProject2"));

      //Assert.AreEqual<string>("TestServer", refresh.Name);
      //Assert.AreEqual<int>(2, refresh.Projects.Count);
      //Assert.AreEqual<bool>(true, refresh.Projects.Contains("TestProject1"));
      //Assert.AreEqual<bool>(true, refresh.Projects.Contains("TestProject2"));
    }
  }
}
