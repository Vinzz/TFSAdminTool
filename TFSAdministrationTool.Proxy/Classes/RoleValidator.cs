using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Windows.Forms;

namespace TFSAdministrationTool.Proxy.Common
{
  /// <summary>
  /// RoleValidator is always used in the context of the SelectedTeamProject
  /// </summary>
  public class RoleValidator
  {
    #region Fields
    const string m_RoleConfigFileName = "RoleConfig.xml";

    private ITeamFoundationServerProxy m_TfsProxy;

    private Dictionary<string, RoleInfoCollection> m_SpRoleMapping;
    private Dictionary<string, RoleInfoCollection> m_RsRoleMapping;

    private RoleInfoCollection m_TfsRoles;
    private RoleInfoCollection m_SpRoles;
    private RoleInfoCollection m_RsRoles;
    #endregion

    #region Constructors
    public RoleValidator(ITeamFoundationServerProxy proxy)
    {
      m_SpRoleMapping = new Dictionary<string, RoleInfoCollection>();
      m_RsRoleMapping = new Dictionary<string, RoleInfoCollection>();

      m_TfsRoles = new RoleInfoCollection();
      m_SpRoles = new RoleInfoCollection();
      m_RsRoles = new RoleInfoCollection();

      m_TfsProxy = proxy;
    }
    #endregion

    #region Methods
    private void AddRole(string userRole, SystemTier tier, bool isSpGroup)
    {
        if (userRole == null) throw new ArgumentNullException("userRole");

        if (string.IsNullOrEmpty(userRole) == true) throw new ArgumentException("Invalid userRole");

        switch (tier)
        {
            case SystemTier.TeamFoundation:
                m_TfsRoles.Add(userRole, false);
                break;
            case SystemTier.SharePoint:
                m_SpRoles.Add(userRole, isSpGroup);
                break;
            case SystemTier.ReportingServices:
                m_RsRoles.Add(userRole, false);
                break;
            default:
                throw new ArgumentException("Invalid Role Type");
        }
    }
    
    public void Initialize(SecurityInfo tfsSecurityInfo, SecurityInfo spSecurityInfo, SecurityInfo rsSecurityInfo)
    {
      // Initialize the roles collections
      InitializeRoles(tfsSecurityInfo, spSecurityInfo, rsSecurityInfo);
      
      // Initialize the role mappings from the XML file
      InitializeMapping();
    }

    private void InitializeMapping()
    {
      try
      {
        if (m_SpRoleMapping.Count == 0 && m_RsRoleMapping.Count == 0)
        {
          TfsAdminToolTracer.TraceMessage(TfsAdminToolTracer.TraceSwitch.TraceInfo, "Initializing roles mapping");
          
          XmlDocument mappingXmlDoc = new XmlDocument();
          mappingXmlDoc.Load(m_RoleConfigFileName);

          foreach (XmlNode tfsRoleNode in mappingXmlDoc.DocumentElement.SelectNodes("TFSRole"))
          {
            if (m_TfsProxy.GetSharePointProxy(m_TfsProxy.SelectedTeamProject).SiteStatus == SiteStatus.Available)
            {
              XmlNode spSystemNode = null;

              if (m_TfsProxy.GetSharePointProxy(m_TfsProxy.SelectedTeamProject).WssVersion == WssVersion.WSS2)
              {
                spSystemNode = tfsRoleNode.SelectSingleNode("Mappings/System[@Name = 'SharePoint' and @Version = '6']");
              }
              else if (m_TfsProxy.GetSharePointProxy(m_TfsProxy.SelectedTeamProject).WssVersion == WssVersion.WSS3 ||
                       m_TfsProxy.GetSharePointProxy(m_TfsProxy.SelectedTeamProject).WssVersion == WssVersion.WSS4)
              {
                spSystemNode = tfsRoleNode.SelectSingleNode("Mappings/System[@Name = 'SharePoint' and @Version = '12']");              
              }

              RoleInfoCollection spRoles = new RoleInfoCollection();

              if (spSystemNode != null && spSystemNode.ChildNodes.Count > 0)
              {
                foreach (XmlNode spRoleNode in spSystemNode.ChildNodes)
                {
                  spRoles.Add(spRoleNode.Attributes["Name"].Value);
                }
              }

              m_SpRoleMapping.Add(tfsRoleNode.Attributes["Name"].Value, spRoles);
            }

            if (m_TfsProxy.ReportServiceProxy.SiteStatus == SiteStatus.Available)
            {
              XmlNode rsSystemNode = null;

              if (m_TfsProxy.ServerVersion == TfsVersion.TfsLegacy)
              {
                rsSystemNode = tfsRoleNode.SelectSingleNode("Mappings/System[@Name = 'ReportServer' and @Version = '2008']");
              }
              else
              {
                rsSystemNode = tfsRoleNode.SelectSingleNode("Mappings/System[@Name = 'ReportServer' and @Version = '2010']");
              }

              RoleInfoCollection rsRoles = new RoleInfoCollection();

              if (rsSystemNode != null && rsSystemNode.ChildNodes.Count > 0)
              {
                foreach (XmlNode rsRoleNode in rsSystemNode.ChildNodes)
                {
                  rsRoles.Add(rsRoleNode.Attributes["Name"].Value);
                }
              }

              m_RsRoleMapping.Add(tfsRoleNode.Attributes["Name"].Value, rsRoles);
            }
          }
        }
      }
      catch (FileNotFoundException ex)
      {
        TfsAdminToolTracer.TraceException(TfsAdminToolTracer.TraceSwitch.TraceError, ex);
        // TODO
        // We could not load the XML file
        // Log that the file does not exist
      }
      catch (Exception ex)
      {
        TfsAdminToolTracer.TraceException(TfsAdminToolTracer.TraceSwitch.TraceError, ex);
        // TODO
        // We could not parse the XML file
        // Log that the file does not exist
      }
    }
    
    private void InitializeRoles(SecurityInfo tfsSecurityInfo, SecurityInfo spSecurityInfo, SecurityInfo rsSecurityInfo)
    {
      TfsAdminToolTracer.TraceMessage(TfsAdminToolTracer.TraceSwitch.TraceInfo, "Initializing roles");
      
      m_TfsRoles.Clear(); m_SpRoles.Clear(); m_RsRoles.Clear();
      // Add the TFS roles
      foreach (SecurityGroup sGroup in tfsSecurityInfo.SecuritySettings)
      {
          AddRole(sGroup.DisplayName, SystemTier.TeamFoundation, false);
      }
      // Add the SharePoint roles
      foreach (SecurityGroup sGroup in spSecurityInfo.SecuritySettings)
      {
          AddRole(sGroup.DisplayName, SystemTier.SharePoint, sGroup.IsSpGroup);
      }
      // Add the ReportingServices roles
      foreach (SecurityGroup sGroup in rsSecurityInfo.SecuritySettings)
      {
          AddRole(sGroup.DisplayName, SystemTier.ReportingServices, false);
      }
    }

    public RoleInfoCollection GetRolesByTier(RoleInfoCollection tfsRoles, SystemTier tier)
    {
      RoleInfoCollection requiredRoles = new RoleInfoCollection();

      if (tier != SystemTier.TeamFoundation)
      {
        for (int index = 0; index < tfsRoles.All.Count; index++)
        {
          RoleInfo role = tfsRoles.All[index];
          switch (tier)
          {
            case SystemTier.SharePoint:
              if (m_TfsProxy.GetSharePointProxy(m_TfsProxy.SelectedTeamProject).SiteStatus == SiteStatus.Available)
              {
                if (m_SpRoleMapping.ContainsKey(role.Name))
                {
                  foreach (RoleInfo mappedRole in m_SpRoleMapping[role.Name].All)
                  {
                    if (!requiredRoles.Contains(mappedRole.Name)) requiredRoles.Add(mappedRole.Name);
                  }
                }
              }
              break;
            case SystemTier.ReportingServices:
              if (m_TfsProxy.ReportServiceProxy.SiteStatus == SiteStatus.Available)
              {
                if (m_RsRoleMapping.ContainsKey(role.Name))
                {
                  foreach (RoleInfo mappedRole in m_RsRoleMapping[role.Name].All)
                  {
                    if (!requiredRoles.Contains(mappedRole.Name)) requiredRoles.Add(mappedRole.Name);
                  }
                }
              }            
              break;
            default:
              //TODO: log an exception
              break;
          }
        }
      }

      requiredRoles.Sort();
      
      return requiredRoles;
    }

    public bool ValidateTfsUser(TfsUser user, SystemTier tier)
    {
      // TODO: We might want to send back an warning message 
      //       about the permissions that are missing.
      
      Dictionary<string, RoleInfoCollection> mapping;

      if (tier == SystemTier.SharePoint)
      {
        // Return True if site is not available
        if (m_TfsProxy.GetSharePointProxy(m_TfsProxy.SelectedTeamProject).SiteStatus != SiteStatus.Available) return true;
        mapping = m_SpRoleMapping;
      }
      else
      {
        // Return True if site is not available
        if (m_TfsProxy.ReportServiceProxy.SiteStatus != SiteStatus.Available) return true;
        mapping = m_RsRoleMapping;
      }
      
      // Loop through the TFS roles of the user
      foreach (RoleInfo tfsRole in user.TfsRoles.All)
      {
        if (mapping.ContainsKey(tfsRole.Name))
        {
          // get the mapping for the particular role
          RoleInfoCollection requiredRoles = mapping[tfsRole.Name];

          foreach (RoleInfo role in requiredRoles.All)
          {
            // Return false on the first missing required role
            if (tier == SystemTier.SharePoint)
            {
              if (!user.SpRoles.Contains(role.Name)) return false;
            }
            else
            {
              if (!user.RsRoles.Contains(role.Name)) return false;
            }
          }
        }
      }

      return true;
    }
    
    #endregion

    #region Properties
    public RoleInfoCollection TfsRoles
    {
      get
      {
        m_TfsRoles.Sort();
        return m_TfsRoles;
      }
    }

    public RoleInfoCollection SpRoles
    {
      get
      {
        m_SpRoles.Sort();
        return m_SpRoles;
      }
    }

    public RoleInfoCollection RsRoles
    {
      get
      {
        m_RsRoles.Sort();
        return m_RsRoles;
      }
    }
    #endregion
  }
}