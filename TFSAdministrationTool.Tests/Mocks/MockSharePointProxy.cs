using System;
using System.Collections.Generic;
using System.Text;
using TFSAdministrationTool.Proxy;
using TFSAdministrationTool.Proxy.Common;
using System.Net;

namespace TFSAdministrationTool.Tests
{
  class MockSharePointProxy : ISharePointProxy

  {
    public static ISharePointProxy CreateMockProxy(string url, SiteStatus status, ICredentials credentials)
    {
      return new MockSharePointProxy();
    }

    #region ISharePointProxy Members

    void ISharePointProxy.AddUserToRole(string userName, string role, string displayName, string email, string description)
    {
      throw new NotImplementedException();
    }

    void ISharePointProxy.AddUserToGroup(string userName, string group, string displayName, string email, string description)
    {
        throw new NotImplementedException();
    }

    void ISharePointProxy.RemoveUserFromRole(string userName, string role)
    {
      throw new NotImplementedException();
    }

    void ISharePointProxy.RemoveUserFromGroup(string userName, string group)
    {
        throw new NotImplementedException();
    }

    void ISharePointProxy.RemoveUser(string userName)
    {
      throw new NotImplementedException();
    }

    List<string> ISharePointProxy.GetRoleCollectionFromUser(string userName)
    {
      throw new NotImplementedException();
    }

    List<string> ISharePointProxy.GetGroupCollectionFromUser(string userName)
    {
        throw new NotImplementedException();
    }

    System.Xml.XmlNode ISharePointProxy.GetRoleCollectionFromWeb()
    {
      throw new NotImplementedException();
    }

    System.Xml.XmlNode ISharePointProxy.GetGroupCollectionFromSite()
    {
        throw new NotImplementedException();
    }

    SecurityInfo ISharePointProxy.GetSecuritySettings()
    {
      throw new NotImplementedException();
    }

    string ISharePointProxy.Url
    {
      get { throw new NotImplementedException(); }
    }

    string ISharePointProxy.ClaimBasedAuthentPrefix
        {
        get { throw new NotImplementedException(); }
    }

    SiteStatus ISharePointProxy.SiteStatus
    {
      get { return SiteStatus.Available; }
    }

    WssVersion ISharePointProxy.WssVersion
    {
      get { return WssVersion.Unknown; }
    }

    #endregion

    public void SetClaimBasedAuthenticationMode(bool param)
    {
        throw new NotImplementedException();
    }
  }
}
