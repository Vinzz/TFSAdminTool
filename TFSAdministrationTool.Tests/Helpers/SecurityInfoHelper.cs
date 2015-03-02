using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.IO;
using TFSAdministrationTool.Proxy.Common;

namespace TFSAdministrationTool.Tests.Helpers
{
  public static class SecurityInfoHelper
  {

    public static SecurityInfo GetTfsSecurityInfo()
    {
      SecurityInfo tfsSecurityInfo = new SecurityInfo();

      SecurityGroup gReaders = new SecurityGroup() { DisplayName = "Readers" };
      gReaders.AddUser("S-1-5-21-1721254763-462695806-1538882281-2742344", @"domain\ReaderUser", "ReaderUser");
      
      SecurityGroup gContributors = new SecurityGroup() { DisplayName = "Contributors" };
      gContributors.AddUser("S-1-5-21-1721254763-462695806-1538882281-2742355", @"domain\ContributorUser", "ContributorUser");

      SecurityGroup gProjectAdministrators = new SecurityGroup() { DisplayName = "Project Administrators" };
      gProjectAdministrators.AddUser("S-1-5-21-1721254763-462695806-1538882281-2742366", @"domain\AdministratorUser", "AdministratorUser");

      SecurityGroup gBuildServices = new SecurityGroup() { DisplayName = "Build Services" };
      
      tfsSecurityInfo.AddGroup(gReaders);
      tfsSecurityInfo.AddGroup(gContributors);
      tfsSecurityInfo.AddGroup(gProjectAdministrators);
      tfsSecurityInfo.AddGroup(gBuildServices);

      return tfsSecurityInfo;
    }

    public static SecurityInfo GetSpSecurityInfo()
    {
      SecurityInfo spSecurityInfo = new SecurityInfo();

      SecurityGroup gFullControl = new SecurityGroup() { DisplayName= "Full Control" };
      gFullControl.AddUser("S-1-5-21-1721254763-462695806-1538882281-2742366", String.Empty, "AdministratorUser");
      
      SecurityGroup gDesign = new SecurityGroup() { DisplayName = "Design" };
      
      SecurityGroup gContribute = new SecurityGroup() { DisplayName = "Contribute" };
      gContribute.AddUser("S-1-5-21-1721254763-462695806-1538882281-2742355", String.Empty, "ContributorUser");
      
      SecurityGroup gRead = new SecurityGroup() { DisplayName = "Read" };
      gRead.AddUser("S-1-5-21-1721254763-462695806-1538882281-2742344", String.Empty, "ReaderUser");

      spSecurityInfo.AddGroup(gFullControl);
      spSecurityInfo.AddGroup(gDesign);
      spSecurityInfo.AddGroup(gContribute);
      spSecurityInfo.AddGroup(gRead);

      return spSecurityInfo;
    }

    public static SecurityInfo GetRsSecurityInfo()
    {
      SecurityInfo rsSecurityInfo = new SecurityInfo();

      SecurityGroup gBrowser = new SecurityGroup() { DisplayName = "Browser" };
      gBrowser.AddUser(String.Empty, @"domain\ReaderUser", string.Empty);

      SecurityGroup gContentManager = new SecurityGroup() { DisplayName = "Content Manager" };
      gContentManager.AddUser(String.Empty, @"domain\ContributorUser", String.Empty);
      gContentManager.AddUser(String.Empty, @"domain\AdministratorUser", String.Empty);
            
      SecurityGroup gMyReports = new SecurityGroup() { DisplayName = "My Reports" };
      SecurityGroup gPublisher = new SecurityGroup() { DisplayName = "Publisher" };
      SecurityGroup gReportBuilder = new SecurityGroup() { DisplayName = "Report Builder" };

      rsSecurityInfo.AddGroup(gBrowser);
      rsSecurityInfo.AddGroup(gContentManager);
      rsSecurityInfo.AddGroup(gMyReports);
      rsSecurityInfo.AddGroup(gPublisher);
      rsSecurityInfo.AddGroup(gReportBuilder);

      return rsSecurityInfo;
    }
  }
}