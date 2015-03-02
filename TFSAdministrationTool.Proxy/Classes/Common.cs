#region Using Statements
using System;
#endregion

namespace TFSAdministrationTool.Proxy.Common
{
  public enum ItemType
  {
    TeamProjectCollection,
    TeamProject
  }

  public enum TfsVersion
  {
    TfsLegacy,  /// Team Foundation Server 2005 or Team Foundation Server 2008
    Tfs2010     /// Team Foundation Server 2010
  }

  public enum WssVersion
  {
    WSS2,     /// Windows SharePoint Services 2.0
    WSS3,     /// Windows SharePoint Services 3.0 or MOSS 2007
    WSS4,     /// SharePoint Foundation 2010 or SharePoint Server 2010
    Unknown   /// Error while determining the version of SharePoint
  }
  
  public enum UserState
  {
    Default,
    New,
    Edited,
    Deleted
  }
  
  public enum SystemTier
  {
    TeamFoundation,
    SharePoint,
    ReportingServices
  }

  public enum ChangeType
  {
    Add,
    Delete
  }

  public enum Status
  {
    Passed,
    Failed
  }

  public enum SiteStatus
  {
    Available,
    Unavailable,
    Unauthorized,
    Error
  }
} //End Namespace