#region Using Statements
using System;
using System.Collections.Generic;
#endregion

namespace TFSAdministrationTool.Proxy.Common
{
  public class PendingChanges
  {
    #region Fields
    Dictionary<Guid, PendingChange> m_PendingChanges = new Dictionary<Guid,PendingChange>();
    #endregion

    #region Methods
    public void Add(bool isChecked, bool isSelected, string userName, string displayName, ChangeType change, string teamProject, string server, Guid instanceId, SystemTier tier, string role)
    {
      // Check if there is an opposite pending changeset
      Guid guid = GetPendingChangeOpposite(userName, change, teamProject, instanceId, tier, role);
      if (guid == Guid.Empty)
      {
        m_PendingChanges.Add(Guid.NewGuid(), new PendingChange(isChecked, isSelected, userName, displayName, change, teamProject, server, instanceId, tier, role));
      }
      else
      {
        m_PendingChanges.Remove(guid);
      }
    }

    public Dictionary<Guid, PendingChange> GetPendingChangesForServer(Guid instanceId)
    {
      Dictionary<Guid, PendingChange> dictionary = new Dictionary<Guid, PendingChange>();

      foreach (Guid guid in m_PendingChanges.Keys)
      {
        if (m_PendingChanges[guid].InstanceId == instanceId)            
        {
          dictionary.Add(guid, m_PendingChanges[guid]);
        }
      }

      return dictionary;
    }

    public Dictionary<Guid, PendingChange> GetPendingChangesForTeamProject(Guid instanceId, string teamProject)
    {
      Dictionary<Guid, PendingChange> dictionary = new Dictionary<Guid, PendingChange>();

      foreach (Guid guid in m_PendingChanges.Keys)
      {
        if (m_PendingChanges[guid].InstanceId == instanceId &&
            String.Compare(m_PendingChanges[guid].TeamProject, teamProject, true) == 0)
        {
          dictionary.Add(guid, m_PendingChanges[guid]);
        }
      }

      return dictionary;
    }
    
    public Dictionary<Guid, PendingChange> GetPendingChangesForUser(string userName, Guid instanceId, string project)
    {
      Dictionary<Guid, PendingChange> dictionary = new Dictionary<Guid,PendingChange>();

      foreach (Guid guid in m_PendingChanges.Keys)
      {
        if (String.Compare(m_PendingChanges[guid].UserName, userName, true) == 0 &&
            m_PendingChanges[guid].InstanceId == instanceId &&
            String.Compare(m_PendingChanges[guid].TeamProject, project, true) == 0)
        {
          dictionary.Add(guid, m_PendingChanges[guid]);
        }
      }

      return dictionary;
    }

    public Guid GetPendingChangeOpposite(string userName, ChangeType change, string teamProject, Guid instanceId, SystemTier tier, string role)
    {
      ChangeType oppositeChange = (change == ChangeType.Add)? ChangeType.Delete:ChangeType.Add;

      // Loop through the pending changes for the user
      foreach (KeyValuePair<Guid, PendingChange> dictionary in GetPendingChangesForUser(userName, instanceId, teamProject))
      {
        if (dictionary.Value.ChangeType == oppositeChange && 
            dictionary.Value.Tier == tier && 
            dictionary.Value.Role == role)
        {
          return dictionary.Key;
        }
      }

      return Guid.Empty;
    }

    public List<string> GetTeamProjects()
    {
      List<string> projects = new List<string>();

      foreach (PendingChange pc in m_PendingChanges.Values)
      {
        if (!projects.Contains(pc.TeamProject))
        {
          projects.Add(pc.TeamProject);
        }
      }

      return projects;
    }

    public PendingChange Item(Guid guid)
    {
      return m_PendingChanges[guid];
    }

    public void Remove(Guid guid)
    {
      m_PendingChanges.Remove(guid);
    }

    public void Remove(string userName, Guid instanceId, string teamProject)
    {
      foreach (KeyValuePair<Guid, PendingChange> dictionary in GetPendingChangesForUser(userName, instanceId, teamProject))
      {
        m_PendingChanges.Remove(dictionary.Key);
      }
    }

    public void RemoveByServer(Guid instanceId)
    {
      foreach (KeyValuePair<Guid, PendingChange> dictionary in GetPendingChangesForServer(instanceId))
      {
        m_PendingChanges.Remove(dictionary.Key);
      }
    }

    public void RemoveByServerInfo(ServerInfo serverInfo)
    {
      foreach (KeyValuePair<Guid, PendingChange> dictionary in GetPendingChangesForServer(serverInfo.InstanceId))
      {
        bool found = false;

        foreach (Microsoft.TeamFoundation.Server.ProjectInfo info in serverInfo.Projects)
        {
          if (String.Compare(dictionary.Value.TeamProject, info.Name, true) == 0)
          {
            found = true;
            break;
          }
        }
        if (!found)
          m_PendingChanges.Remove(dictionary.Key);
      }
    }

    public void UpdateChecked(Guid guid, bool isChecked)
    {
      m_PendingChanges[guid].Checked = isChecked;
    }

    public void UpdateSelected(Guid guid, bool isChecked)
    {
      m_PendingChanges[guid].Selected = isChecked;
    }
    #endregion

    #region Properties
    public Dictionary<Guid, PendingChange> All
    {
      get
      {
        return m_PendingChanges;
      }
    }
    
    public Dictionary<Guid, PendingChange> Checked
    {
      get
      {
        Dictionary<Guid, PendingChange> dictionary = new Dictionary<Guid, PendingChange>();
        foreach (Guid guid in m_PendingChanges.Keys)
        {
          if (m_PendingChanges[guid].Checked)
          {
            dictionary.Add(guid, m_PendingChanges[guid]);
          }
        }
        return dictionary;
      }
    }

    public Dictionary<Guid, PendingChange> Selected
    {
      get
      {
        Dictionary<Guid, PendingChange> dictionary = new Dictionary<Guid, PendingChange>();
        foreach (Guid guid in m_PendingChanges.Keys)
        {
          if (m_PendingChanges[guid].Selected)
          {
            dictionary.Add(guid, m_PendingChanges[guid]);
          }
        }
        return dictionary;
      }
    }
    #endregion
  } // End PendingChanges Class

  public class PendingChange
  {
    #region Fields
    private bool m_Checked;
    private bool m_Selected;
    private string m_UserName;
    private string m_DisplayName;
    private ChangeType m_ChangeType;
    private string m_TeamProject;
    private string m_Server;
    private Guid m_InstanceId;
    private SystemTier m_Tier;
    private string m_Role;
    #endregion

    #region Constructors
    public PendingChange(bool isChecked, bool isSelected, string userName, string displayName, ChangeType changeType, string teamProject, string server, Guid instanceId, SystemTier tier, string role)
    {
      m_Checked = isChecked;
      m_Selected = isSelected;
      m_UserName = userName;
      m_DisplayName = displayName;
      m_ChangeType = changeType;
      m_TeamProject = teamProject;
      m_Server = server;
      m_InstanceId = instanceId;
      m_Tier = tier;
      m_Role = role;
    }
    #endregion

    #region Properties
    public bool Checked
    {
      get
      {
        return m_Checked;
      }
      set
      {
        m_Checked = value;
      }
    }
    
    public bool Selected
    {
      get
      {
        return m_Selected;
      }
      set
      {
        m_Selected = value;
      }
    }
    
    public string UserName
    {
      get
      {
        return m_UserName;
      }
    }
    
    public string DisplayName
    {
      get
      {
        return m_DisplayName;
      }
    }
    
    public ChangeType ChangeType
    {
      get
      {
        return m_ChangeType;
      }
    }
    
    public string TeamProject
    {
      get
      {
        return m_TeamProject;
      }
    }
    
    public string Server
    {
      get
      {
        return m_Server;
      }
    }

    public Guid InstanceId
    {
      get
      {
        return m_InstanceId;
      }
    }
    
    public SystemTier Tier
    {
      get
      {
        return m_Tier;
      }
    }
    
    public string Role
    {
      get
      {
        return m_Role;
      }
    }
    #endregion
  } //End PendingChange Class
} //End Namespace