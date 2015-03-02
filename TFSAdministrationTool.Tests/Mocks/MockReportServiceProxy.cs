using System;
using System.Collections.Generic;
using System.Text;
using TFSAdministrationTool.Proxy;
using TFSAdministrationTool.Proxy.Common;
using System.Net;

namespace TFSAdministrationTool.Tests
{
  class MockReportServiceProxy : IReportServiceProxy
  {
    public static IReportServiceProxy CreateMockProxy(string url, string root, SiteStatus status, ICredentials credentials)
    {
      return new MockReportServiceProxy();
    }

    #region MockReportServiceProxy Fields
    #endregion

    #region IReportServiceProxy Members

    void IReportServiceProxy.AddUserToRole(string nodeName, string userName, string roleName)
    {
      throw new NotImplementedException();
    }

    void IReportServiceProxy.RemoveUserFromRole(string nodeName, string userName, string roleName)
    {
      throw new NotImplementedException();
    }

    void IReportServiceProxy.RemoveUser(string nodeName, string userName)
    {
      throw new NotImplementedException();
    }

    SecurityInfo IReportServiceProxy.GetSecuritySettings(string name)
    {
      throw new NotImplementedException();
    }

    string IReportServiceProxy.Url
    {
      get { throw new NotImplementedException(); }
    }

    string IReportServiceProxy.RootFolder
    {
      get { throw new NotImplementedException(); }
      set { throw new NotImplementedException(); }
    }

    SiteStatus IReportServiceProxy.SiteStatus
    {
      get { return SiteStatus.Available; }
    }

    #endregion
  }
}
