#region Using Statements
using System.Collections.Generic;
#endregion

namespace TFSAdministrationTool.Proxy.Common
{
  public class History
  {
    #region Fields
    private List<HistoryItem> m_History = new List<HistoryItem>();
    private int m_LastIndex = 0;
    #endregion

    #region Methods
    public void AddHistoryItem(string userName, string displayName, ChangeType changeType, string server, string teamProject, SystemTier tier, string role, Status status)
    {
      m_History.Add(new HistoryItem(userName, displayName, changeType, server, teamProject, tier, role, status));
    }

    public void ClearHistory()
    {
      m_History.Clear();
      m_LastIndex = 0;
    }

    public List<HistoryItem> GetNewHistoryItems()
    {
      List<HistoryItem> list = new List<HistoryItem>();

      for (int index = m_LastIndex; index < m_History.Count; index++) {
        list.Add(m_History[index]);
      }
      
      // Update the index
      m_LastIndex = m_History.Count;

      return list;
    }
    #endregion
  } //End History Class

  public class HistoryItem
  {
    #region Fields
    private string m_UserName;
    private string m_DisplayName;
    private ChangeType  m_ChangeType;
    private string m_TeamProject;
    private string m_Server;
    private SystemTier m_Tier;
    private string m_Role;
    private Status m_Status;
    #endregion

    #region Constructors
    public HistoryItem(string userName, string displayName, ChangeType changeType, string teamProject, string server, SystemTier tier, string role, Status status)
    {
      m_UserName = userName;
      m_DisplayName = displayName;
      m_ChangeType = changeType;
      m_TeamProject = teamProject;
      m_Server = server;
      m_Tier = tier;
      m_Role = role;
      m_Status = status;
    }
    #endregion

    #region Properties
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
    
    public Status Status
    {
      get
      {
        return m_Status;
      }
    }
    #endregion
  } //End HistoryItem Class
} //End Namespace
