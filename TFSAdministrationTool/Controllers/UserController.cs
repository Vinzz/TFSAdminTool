﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using TFSAdministrationTool.Proxy;
using TFSAdministrationTool.Proxy.Common;
using TFSAdministrationTool.Controllers;
using TFSAdministrationTool.Properties;

namespace TFSAdministrationTool.Controllers
{
  public class UserAlreadyExistException : ApplicationException
  {
    public UserAlreadyExistException(string msg) : base(msg) {}
  }

  public class UserHasPendingChangesException : ApplicationException
  {
    public UserHasPendingChangesException(string msg) : base(msg) { }
  }

  public static class UserController
  {
    private static ITeamFoundationServerProxy m_TeamFoundationServer;

    private static string m_TeamProject;
    private static TfsUserCollection m_Users = new TfsUserCollection();

    public static string TeamProject
    {
      get
      {
        return m_TeamProject;
      }
      set
      {
        m_TeamProject = value;
      }
    }
    
    public static TfsUserCollection UserCollection
    {
      get
      {
        return m_Users;
      }
    }

    public static void Initialize(ITeamFoundationServerProxy tfs, TfsUserCollection users)
    {
      m_TeamFoundationServer = tfs;

      if (users != null)
        m_Users = users;
      else
        m_Users = new TfsUserCollection();
    }

    public static bool ImportUser(TfsUser user)
    {
      if (!m_Users.Users.Exists(delegate(TfsUser u) { return string.Compare(u.UserName, user.UserName, true) == 0; }))
      {
        m_Users.Add(user);
        return true;
      }
      return false;
    }
    
    public static bool AddUser(TfsUser user, bool PerformExistingUserControl)
    {
      bool shouldAddUser = false;

      // Make sure the user that we are about to add does not have any pending changes
      // Not tool classy as we make a call to the controller of the Main Form but we can change it later
      if (MainController.PendingChanges.GetPendingChangesForUser(user.UserName, m_TeamFoundationServer.Server.InstanceId, m_TeamFoundationServer.SelectedTeamProject).Count == 0)
      {
        if (PerformExistingUserControl)
        {
          // Check if the user is already defined for this Server/Team Project
          if (m_TeamFoundationServer.UserCollection.GetUser(user.UserName) != null)
          {
            throw new UserAlreadyExistException(String.Format(Resources.UserAlreadyExistPrompt, user.UserName, m_TeamFoundationServer.SelectedTeamProject));
          }
        }

        shouldAddUser = true;
      }
      else
      {
        throw new UserHasPendingChangesException(String.Format(Resources.UserHasPendingChangesPrompt, user.UserName));
      }

      if (shouldAddUser)
      {
        if (!m_Users.Users.Exists(delegate(TfsUser u) { return string.Compare(u.UserName, user.UserName, true) == 0; }))
        {
          m_Users.Add(user);
          return true;
        }
      }

      return false;
    }

    public static TfsUserCollection BrowseUsers(System.Windows.Forms.IWin32Window parentWin)
    {
      Dictionary<string, string> usersGroupsCollection = m_TeamFoundationServer.ShowAddGroupControl(parentWin);
      TfsUserCollection users = new TfsUserCollection();

      if (usersGroupsCollection != null)
      {
        foreach (KeyValuePair<string, string> kvp in usersGroupsCollection)
        {
          users.Add(new TfsUser() { UserName = kvp.Key, DisplayName = kvp.Value });
        }
      }

      return users;
    }

    public static TfsUserCollection ResolveUsers(string[] users)
    {
      /// Keep track of users that we cannot resolve
      List<string> unResolvedUsers = new List<string>();
      
      TfsUserCollection result = new TfsUserCollection();

      foreach (string user in users)
      {
        if (!string.IsNullOrEmpty(user))
        {
          TfsUser tfsUser = m_TeamFoundationServer.UserCollection.GetUser(user);
          if (tfsUser == null)
          {
            tfsUser = m_TeamFoundationServer.GetUser(user);
          }

          if (tfsUser != null)
          {
            result.Add(tfsUser);
          }
          else
          {
            unResolvedUsers.Add(user);
          }
        }
      }

      /// Display unresolved users
      if (unResolvedUsers.Count > 0)
      {
        MessageBox.Show(String.Format(Resources.UsersUnresolvedMessage, String.Join(",", unResolvedUsers.ToArray())));
      }

      return result;
    }

    public static RoleInfoCollection GetAvailableRolesBySystem(SystemTier subSystem)
    {
      switch (subSystem)
      {
        case SystemTier.TeamFoundation:
          return m_TeamFoundationServer.RoleValidator.TfsRoles;
        case SystemTier.SharePoint:
          return m_TeamFoundationServer.RoleValidator.SpRoles;
        case SystemTier.ReportingServices:
          return m_TeamFoundationServer.RoleValidator.RsRoles;
      }

      return null;
    }

    public static RoleInfoCollection GetMappedRolesBySystem(string roles, SystemTier subSystem)
    {
      RoleInfoCollection requestedRoles = new RoleInfoCollection();
      foreach (string role in roles.Split(';')) {
        requestedRoles.Add(role);
      }

      return m_TeamFoundationServer.RoleValidator.GetRolesByTier(requestedRoles, subSystem);
    }
  }
}