using System;
using System.Collections.Generic;
using System.Text;

namespace TFSAdministrationTool.Proxy.Common
{
  public class SecurityInfo
  {
    private List<SecurityGroup> m_SecuritySettings;

    public SecurityInfo()
    {
      m_SecuritySettings = new List<SecurityGroup>();
    }

    public void AddGroup(SecurityGroup group)
    {
      m_SecuritySettings.Add(group);
    }

    public SecurityGroup GetGroupByName(string groupName)
    {
      SecurityGroup sGroup = null;

      foreach (SecurityGroup s in m_SecuritySettings)
      {
        if (String.Compare(s.DisplayName, groupName, true) == 0) {
          sGroup = s;
          break;
        }
      }

      return sGroup;
    }

    public List<SecurityGroup> GetGroupBySid(string sid)
    {
      List<SecurityGroup> groups = new List<SecurityGroup>();

      foreach (SecurityGroup sGroup in m_SecuritySettings)
      {
        foreach (SecurityGroup.User u in sGroup.Users)
        {
          if (String.Compare(sid, u.Sid, true) == 0)
          {
            groups.Add(sGroup);
            break;
          }
        }
      }

      return groups;
    }
    
    public List<SecurityGroup> GetGroupByUserName(string userName)
    {
      List<SecurityGroup> groups = new List<SecurityGroup>();

      foreach (SecurityGroup sGroup in m_SecuritySettings)
      {
        foreach (SecurityGroup.User u in sGroup.Users)
        {
          if (String.Compare(userName, u.UserName, true) == 0)
          {
            groups.Add(sGroup);
            break;
          }
        }
      }
            
      return groups;
    }

    public List<SecurityGroup> SecuritySettings
    {
      get
      {
        return m_SecuritySettings;
      }
    }
  }

  public class SecurityGroup
  {
      public bool IsSpGroup { get; set; }

    public string DisplayName { get; set; }

    List<User> m_Users = new List<User>();

    public void AddUser(string sid, string username, string displayname)
    {
      m_Users.Add(new User() { Sid = sid, UserName = username, DisplayName = displayname});
    }
    
    public void AddUser(string sid, string username, string displayname, string email, Microsoft.TeamFoundation.Server.IdentityType identityType)
    {
      m_Users.Add(new User() { Sid = sid, UserName = username, DisplayName = displayname, Email = email, IdentityType= identityType });
    }

    public List<User> Users
    {
      get
      {
        return m_Users;
      }
    }

    public class User
    {
      public string Sid { get; set; }
      public string UserName { get; set; }
      public string DisplayName { get; set; }
      public string Email { get; set; }
      public Microsoft.TeamFoundation.Server.IdentityType IdentityType { get; set; }
    }
  }
}