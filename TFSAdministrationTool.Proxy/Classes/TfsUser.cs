#region Using Statements
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Security.Principal;
using System.Text;
#endregion

namespace TFSAdministrationTool.Proxy.Common
{
  public class TfsUserCollection
  {
    #region Fields
    List<TfsUser> m_TfsUser;
    #endregion

    #region Constructors
    public TfsUserCollection()
    {
      m_TfsUser = new List<TfsUser>();
    }
    #endregion

    #region Methods

    public void ApplyPendingChanges(Dictionary<Guid, PendingChange> pendingChanges)
    {
      // Loop through the pending changes for Server/Team Project
      foreach (PendingChange change in pendingChanges.Values)
      {
        // Get the Index of the user
        int index = GetUserIndex(change.UserName);
        if (index == -1)
        {
          m_TfsUser.Add(new TfsUser() { State = UserState.New, UserName = change.UserName, DisplayName = change.DisplayName });
          index = m_TfsUser.Count - 1;
        }

        if (change.ChangeType == ChangeType.Add)
        {
          m_TfsUser[index].AddRole(change.Role, change.Tier);
          if (m_TfsUser[index].State != UserState.New) m_TfsUser[index].State = UserState.Edited;
        }

        if (change.ChangeType == ChangeType.Delete)
        {
          if (change.Tier == SystemTier.TeamFoundation && change.Role == "All")
          {            
            m_TfsUser[index].State = UserState.Deleted;
          }
          else
          {
            // Normal Delete
            m_TfsUser[index].RemoveRole(change.Role, change.Tier);
            if (m_TfsUser[index].State != UserState.New) m_TfsUser[index].State = UserState.Edited;
          }
        }
      }
    }
    
    public void Add(TfsUser user)
    {
      m_TfsUser.Add(user);
    }

    public static TfsUserCollection Clone(TfsUserCollection source)
    {
      TfsUserCollection target = new TfsUserCollection();
      foreach (TfsUser user in source.Users){
        target.Add(TfsUser.Clone(user));
      }
      return target;
    }

    public void CopyUserRoles(TfsUserCollection source)
    {
      foreach (TfsUser user in source.Users)
      {
        int index = GetUserIndex(user.UserName);
        if (index == -1)
        {
          m_TfsUser.Add(user);
        }
        else
        {
          m_TfsUser[index].CopyRoles(user);
        }
      }
    }

    public TfsUser GetUser(string userName)
    {
      foreach (TfsUser user in m_TfsUser)
      {
        if (String.Compare(userName, user.UserName, true) == 0)
        {
          return user;
        }
      }

      return null;
    }
    
    public int GetUserIndex(string userName)
    {
      for (int i = 0; i < m_TfsUser.Count; i++)
      {
        if (String.Compare(userName, m_TfsUser[i].UserName, true) == 0)
        {
          return i;
        }
      }

      return -1;
    }

    public void Sort()
    {
      m_TfsUser.Sort();
    }

    public void Remove(string userName)
    {
      int index = GetUserIndex(userName);
      m_TfsUser.RemoveAt(index);
    }
    
    public List<TfsUser> Users
    {
      get
      {
        return m_TfsUser;
      }
    }
    #endregion
  } //End Class TfsUserCollection
    
  public class TfsUser: IComparable<TfsUser>
  {
    #region Fields
    private UserState m_UserState;
    private string m_UserName;
    private string m_DisplayName;
    private string m_Email;
    private Microsoft.TeamFoundation.Server.IdentityType m_IdentityType;
    private RoleInfoCollection m_TfsRoles;
    private RoleInfoCollection m_SpRoles;
    private RoleInfoCollection m_RsRoles;
    #endregion

    #region Constructors
    public TfsUser()
    {
      m_TfsRoles = new RoleInfoCollection();
      m_SpRoles = new RoleInfoCollection();
      m_RsRoles = new RoleInfoCollection();
    }

    public TfsUser(string userName)
    {
      m_UserName = userName;
      m_TfsRoles = new RoleInfoCollection();
      m_SpRoles = new RoleInfoCollection();
      m_RsRoles = new RoleInfoCollection();
    }
    #endregion

    #region Methods
    public void AddRole(string userRole, SystemTier tier)
    {
      if (userRole == null) throw new ArgumentNullException("userRole");

      switch (tier)
      {
        case SystemTier.TeamFoundation:
          if (!m_TfsRoles.Contains(userRole)) m_TfsRoles.Add(userRole);
          break;
        case SystemTier.SharePoint:
          if (!m_SpRoles.Contains(userRole)) m_SpRoles.Add(userRole);
          break;
        case SystemTier.ReportingServices:
          if (!m_RsRoles.Contains(userRole)) m_RsRoles.Add(userRole);
          break;
        default:
          throw new ArgumentException("Invalid Role Type");
      }
    }

    public static TfsUser Clone(TfsUser source)
    {
        TfsUser target = new TfsUser() { m_UserName = source.UserName, m_DisplayName = source.DisplayName, m_Email = source.Email, m_UserState = source.State, m_IdentityType = source.IdentityType };

      foreach (RoleInfo s in source.m_TfsRoles.All)
        target.m_TfsRoles.Add(s.Name);
      foreach (RoleInfo s in source.m_SpRoles.All)
        target.m_SpRoles.Add(s.Name);
      foreach (RoleInfo s in source.m_RsRoles.All)
        target.m_RsRoles.Add(s.Name);

      return target;
    }

    public void CopyRoles(TfsUser user)
    {
      m_TfsRoles = user.TfsRoles;
      m_SpRoles = user.SpRoles;
      m_RsRoles = user.RsRoles;
    }

    public static Dictionary<string, ChangeType> GetRoleChanges(TfsUser sourceUser, TfsUser targetUser, SystemTier tier)
    {
      // Dictionary containig a Role, ChangeType pair
      Dictionary<string, ChangeType> changes = new Dictionary<string, ChangeType>();

      switch (tier)
      {
        case SystemTier.TeamFoundation:
            changes = RoleInfoCollection.GetDelta((sourceUser != null) ? sourceUser.TfsRoles : null, targetUser.TfsRoles);
          break;
        case SystemTier.SharePoint:
          changes = RoleInfoCollection.GetDelta((sourceUser != null) ? sourceUser.SpRoles : null, targetUser.SpRoles);
          break;
        case SystemTier.ReportingServices:
          changes = RoleInfoCollection.GetDelta((sourceUser != null) ? sourceUser.RsRoles: null, targetUser.RsRoles);
          break;
      }

      return changes;
    }

    public RoleInfoCollection GetRolesBySystem(SystemTier Tier)
    {
      switch (Tier)
      {
        case SystemTier.TeamFoundation:
          return m_TfsRoles;
        case SystemTier.SharePoint:
          return m_SpRoles;
        case SystemTier.ReportingServices:
          return m_RsRoles;
      }

      return null;
    }

    public static bool IsCurrentUser(TfsUser user)
    {
      WindowsIdentity currentUser = WindowsIdentity.GetCurrent(false);

      if (String.Compare(user.UserName, currentUser.Name, true) == 0)
      {
        return true;
      }
      else
      {
        return false;      
      }      
    }

    public static bool IsUserInRole(TfsUser user, SystemTier tier, string userRole)
    {
      foreach (RoleInfo role in user.GetRolesBySystem(tier).All)
      {
        if (String.Compare(role.Name, userRole, true) == 0)
        {
          return true;
        }
      }

      return false;
    }

    public void RemoveRole(string userRole, SystemTier tier)
    {
      if (userRole == null) throw new ArgumentNullException("userRole");

      if (string.IsNullOrEmpty(userRole) == true) throw new ArgumentException("Invalid userRole");

      switch (tier)
      {
        case SystemTier.TeamFoundation:
          if (m_TfsRoles.Contains(userRole)) m_TfsRoles.Remove(userRole);
          break;
        case SystemTier.SharePoint:
          if (m_SpRoles.Contains(userRole)) m_SpRoles.Remove(userRole);
          break;
        case SystemTier.ReportingServices:
          if (m_RsRoles.Contains(userRole)) m_RsRoles.Remove(userRole);
          break;
        default:
          throw new ArgumentException("Invalid Role Type");
      }
    }
    #endregion

    #region Properties
    public UserState State
    {
      get
      {
        return m_UserState;
      }
      set
      {
        m_UserState = value;
      }
    }

    public string UserName
    {
      get
      {
        return m_UserName;
      }
      set
      {
        m_UserName = value;
      }
    }

    public string DisplayName
    {
      get
      {
        return m_DisplayName;
      }
      set
      {
        m_DisplayName = value;
      }
    }

    public string Email
    {
        get
        {
            return m_Email;
        }
        set
        {
            m_Email = value;
        }
    }

    public Microsoft.TeamFoundation.Server.IdentityType IdentityType
    {
      get
      {
        return m_IdentityType;
      }
      set
      {
        m_IdentityType = value;
      }
    }

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
        return m_SpRoles; ;
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

    public string TfsRolesString
    {
      get
      {
        return m_TfsRoles.ToString();
      }
    }

    public string SpRolesString
    {
      get
      {
        return m_SpRoles.ToString();
      }
    }

    public string RsRolesString
    {
      get
      {
        return m_RsRoles.ToString();
      }
    }
    #endregion

    #region IComparable<TfsUser> Members

    int IComparable<TfsUser>.CompareTo(TfsUser other)
    {
      return this.DisplayName.CompareTo(other.DisplayName); 
      
      throw new NotImplementedException();
    }

    #endregion
  } //End Class TfsUser
} //End Namespace