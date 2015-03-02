using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TFSAdministrationTool.Controllers;
using TFSAdministrationTool.Proxy;
using TFSAdministrationTool.Proxy.Common;
using TFSAdministrationTool.Tests.Helpers;

namespace TFSAdministrationTool.Tests
{
  [TestClass()]
  public class RoleValidatorTest
  {
    public TestContext TestContext { get; set; }

    private MockTeamFoundationServerProxy m_mockTFS;

    [ClassInitialize()]
    public static void ClassInitialize(TestContext context)
    {
      //Hookup factories for mock objects
      TeamFoundationServerProxyFactory.CreateProxy = new TeamFoundationServerProxyFactory.Factory(MockTeamFoundationServerProxy.CreateMockProxy);
    }

    [TestInitialize()]
    public void TestInitialize()
    {
      m_mockTFS = (MockTeamFoundationServerProxy)TeamFoundationServerProxyFactory.CreateTeamFoundationServerProxy();
    }

    [TestMethod()]
    public void RoleValidatorConstructorTest()
    {
      RoleValidator target = new RoleValidator(m_mockTFS);

      Assert.AreEqual<int>(0, target.TfsRoles.All.Count, "TfsRoles collection not initialized correctly");
      Assert.AreEqual<int>(0, target.SpRoles.All.Count, "SpRoles collection not initialized correctly");
      Assert.AreEqual<int>(0, target.RsRoles.All.Count, "RsRoles collection not initialized correctly");
    }
    /*
    [TestMethod()]
    public void GetRolesByTier_SharePointSingleRoleTest()
    {
      RoleInfoCollection requestedRoles = new RoleInfoCollection();
      requestedRoles.Add("Contributors");

      RoleValidator target = new RoleValidator(m_mockTFS);
      target.Initialize(SecurityInfoHelper.GetTfsSecurityInfo(), SecurityInfoHelper.GetSpSecurityInfo(), SecurityInfoHelper.GetRsSecurityInfo());

      RoleInfoCollection actual = target.GetRolesByTier(requestedRoles, SystemTier.SharePoint);

      Assert.AreEqual(2, actual.All.Count);
      Assert.IsNotNull(actual.All.Find(delegate(RoleInfo ri) { return string.Compare(ri.Name, "Contribute", true) == 0; }));
      Assert.IsNotNull(actual.All.Find(delegate(RoleInfo ri) { return string.Compare(ri.Name, "Design", true) == 0; }));
    }

    [TestMethod()]
    public void GetRolesByTier_SharePointMultipleRoleTest()
    {
      RoleInfoCollection requestedRoles = new RoleInfoCollection();
      requestedRoles.Add("Contributors");
      requestedRoles.Add("Readers");

      RoleValidator target = new RoleValidator(m_mockTFS);
      target.Initialize(SecurityInfoHelper.GetTfsSecurityInfo(), SecurityInfoHelper.GetSpSecurityInfo(), SecurityInfoHelper.GetRsSecurityInfo());

      RoleInfoCollection actual = target.GetRolesByTier(requestedRoles, SystemTier.SharePoint);

      Assert.AreEqual(3, actual.All.Count);
      Assert.IsNotNull(actual.All.Find(delegate(RoleInfo ri) { return string.Compare(ri.Name, "Contribute", true) == 0; }));
      Assert.IsNotNull(actual.All.Find(delegate(RoleInfo ri) { return string.Compare(ri.Name, "Design", true) == 0; }));
      Assert.IsNotNull(actual.All.Find(delegate(RoleInfo ri) { return string.Compare(ri.Name, "Read", true) == 0; }));
    }

    [TestMethod()]
    public void GetRolesByTier_ReportingServiceSingleRoleTest()
    {
      RoleInfoCollection requestedRoles = new RoleInfoCollection();
      requestedRoles.Add("Contributors");

      RoleValidator target = new RoleValidator(m_mockTFS);
      target.Initialize(SecurityInfoHelper.GetTfsSecurityInfo(), SecurityInfoHelper.GetSpSecurityInfo(), SecurityInfoHelper.GetRsSecurityInfo());

      RoleInfoCollection actual = target.GetRolesByTier(requestedRoles, SystemTier.ReportingServices);

      Assert.AreEqual(2, actual.All.Count);
      Assert.IsNotNull(actual.All.Find(delegate(RoleInfo ri) { return string.Compare(ri.Name, "Publisher", true) == 0; }));
      Assert.IsNotNull(actual.All.Find(delegate(RoleInfo ri) { return string.Compare(ri.Name, "Report Builder", true) == 0; }));
    }

    [TestMethod()]
    public void GetRolesByTier_ReportingServiceMultipleRoleTest()
    {
      RoleInfoCollection requestedRoles = new RoleInfoCollection();
      requestedRoles.Add("Contributors");
      requestedRoles.Add("Readers");

      RoleValidator target = new RoleValidator(m_mockTFS);
      target.Initialize(SecurityInfoHelper.GetTfsSecurityInfo(), SecurityInfoHelper.GetSpSecurityInfo(), SecurityInfoHelper.GetRsSecurityInfo());

      RoleInfoCollection actual = target.GetRolesByTier(requestedRoles, SystemTier.ReportingServices);

      Assert.AreEqual(3, actual.All.Count);
      Assert.IsNotNull(actual.All.Find(delegate(RoleInfo ri) { return string.Compare(ri.Name, "Publisher", true) == 0; }));
      Assert.IsNotNull(actual.All.Find(delegate(RoleInfo ri) { return string.Compare(ri.Name, "Report Builder", true) == 0; }));
      Assert.IsNotNull(actual.All.Find(delegate(RoleInfo ri) { return string.Compare(ri.Name, "Browser", true) == 0; }));
    }*/
  }
}
