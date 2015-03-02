using System;
using System.Collections.Generic;
using System.Text;

namespace TFSAdministrationTool.Proxy.Common
{
  public class RoleInfo : IComparable<RoleInfo>
  {
    private string m_Name;
    private bool m_Enabled;
    private bool m_IsSpGroup;

    public RoleInfo(string name) : this(name, true) { }

    public RoleInfo(string name, bool enabled)
        : this(name, enabled, false)
    {
        m_Name = name;
        m_Enabled = enabled;
    }

    public RoleInfo(string name, bool enabled, bool isSpGroup)
    {
        m_Name = name;
        m_Enabled = enabled;
        m_IsSpGroup = isSpGroup;
    }

    public string Name
    {
      get
      {
        return m_Name;
      }
    }

    public bool Enabled
    {
      get
      {
        return m_Enabled;
      }
    }

    public bool IsSpGroup
    {
        get { return m_IsSpGroup; }
    }

    #region IComparable<RoleInfo> Members

    int IComparable<RoleInfo>.CompareTo(RoleInfo other)
    {
      return String.Compare(this.Name, other.Name, true);
    }

    #endregion
  }

  // TODO: Implement IEnumerable
  public class RoleInfoCollection
  {
    private List<RoleInfo> m_RoleCollection;

    public RoleInfoCollection()
    {
      m_RoleCollection = new List<RoleInfo>();
    }

    public void Add(string roleName, bool isSpGroup)
    {
        m_RoleCollection.Add(new RoleInfo(roleName, true, isSpGroup));
    }

    public void Add(string roleName)
    {
      m_RoleCollection.Add(new RoleInfo(roleName));
    }

    public void Clear()
    {
      m_RoleCollection.Clear();
    }
    
    public bool Contains(string roleName)
    {
      foreach (RoleInfo role in m_RoleCollection)
      {
        if (String.Compare(role.Name, roleName, true) == 0) return true;
      }

      return false;
    }

    public RoleInfo this[string name]
    {
        get
        {
            foreach (RoleInfo role in m_RoleCollection)
            {
                if (String.Compare(role.Name, name, true) == 0) return role;
            }
            return null;
        }
    }

    public static Dictionary<string, ChangeType> GetDelta(RoleInfoCollection sourceCollection, RoleInfoCollection targetCollection)
    {
      // Dictionary containig a Role, ChangeType pair
      Dictionary<string, ChangeType> changes = new Dictionary<string, ChangeType>();

      if (sourceCollection != null)
      {
        foreach (RoleInfo role in sourceCollection.All)
        {
          if (!targetCollection.Contains(role.Name))
          {
            changes.Add(role.Name, ChangeType.Delete);
          }
        }
      }
      foreach (RoleInfo role in targetCollection.All)
      {
        if (sourceCollection == null || !sourceCollection.Contains(role.Name))
        {
          changes.Add(role.Name, ChangeType.Add);
        }
      }

      return changes;    
    }

    public void Remove(string roleName)
    {
      int roleIndex = -1;

      for (int i = 0; i < m_RoleCollection.Count; i++)
      {
        if (String.Compare(m_RoleCollection[i].Name, roleName, true) == 0)
        {
          roleIndex = i;
          break;
        }
      }

      if (roleIndex != -1) m_RoleCollection.RemoveAt(roleIndex);
    }

    public void Sort()
    {
      m_RoleCollection.Sort();
    }

    public override string ToString()
    {
      StringBuilder sb = new StringBuilder();
      
      m_RoleCollection.Sort();
      for (int i = 0; i < m_RoleCollection.Count; i++)
      {
        sb.Append(m_RoleCollection[i].Name);
        if (i < m_RoleCollection.Count - 1) sb.Append(",");
      }

      return sb.ToString();
    }

    public List<RoleInfo> All
    {
      get
      {
        return m_RoleCollection;
      }
    }
  }
}