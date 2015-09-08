#region Using Statements
using System;
using System.Net;
using System.Collections;
using System.Collections.Generic;

using TFSAdministrationTool.Proxy;
using TFSAdministrationTool.Proxy.Common;
using TFSAdministrationTool.Properties;
using Microsoft.TeamFoundation.Proxy;
using Microsoft.TeamFoundation.Client;
#endregion

namespace TFSAdministrationTool.Controllers
{
  public static class MainController
  {
    #region Fields
    private static ServerStore m_ServerStore = new ServerStore();
    private static PendingChanges m_PendingChanges = new PendingChanges();
    private static History m_History = new History();
    #endregion

    #region Methods - Server related
    public static ServerInfo OnServerConnect()
    {
      ServerInfo selectedServerInfo = null;
      ITeamFoundationServerProxy tfsProxy = TeamFoundationServerProxyFactory.CreateTeamFoundationServerProxy();

      // Create the TeamProjectPicker
      TeamProjectPicker tpc = new TeamProjectPicker(TeamProjectPickerMode.MultiProject, false);
      tpc.SetDefaultSelectionProvider(m_ServerStore);
      
      if (tpc.ShowDialog() == System.Windows.Forms.DialogResult.OK)
      {
        /// Team Foundation Server 2005 & 2008 do not have a ConfigurationServer property 
        if (tpc.SelectedTeamProjectCollection.ConfigurationServer == null)
        {
          /// Team Foundation Server 2005 & 2008          
          selectedServerInfo = new ServerInfo(tpc.SelectedTeamProjectCollection.InstanceId, tpc.SelectedTeamProjectCollection.Uri, tpc.SelectedTeamProjectCollection.Uri, tpc.SelectedTeamProjectCollection.Name, tpc.SelectedProjects);
        }
        else
        {
          /// Team Foundation Server 2010
          selectedServerInfo = new ServerInfo(tpc.SelectedTeamProjectCollection.InstanceId, tpc.SelectedTeamProjectCollection.ConfigurationServer.Uri, tpc.SelectedTeamProjectCollection.Uri, tpc.SelectedTeamProjectCollection.Name, tpc.SelectedProjects);
        }
        
        TfsAdminToolTracer.TraceMessage(TfsAdminToolTracer.TraceSwitch.TraceInfo, "Connecting to Team Project Collection: " + selectedServerInfo.Name);
        tfsProxy.Connect(selectedServerInfo.CollectionUri, tpc.SelectedTeamProjectCollection.Credentials);
        TfsAdminToolTracer.TraceMessage(TfsAdminToolTracer.TraceSwitch.TraceInfo, "Successfully connected to Team Project Collection: " + selectedServerInfo.Name + " ("+tfsProxy.ServerVersion+")");

        // Connecting to an existing collection
        if (m_ServerStore.TeamProjectCollections.ContainsKey(selectedServerInfo.InstanceId)) {
          // Add/Remove Team Project(s) for an existing collection
          m_ServerStore.TeamProjectCollections[selectedServerInfo.InstanceId].UpdateTeamProjects(selectedServerInfo.Projects);        
        } else {
          // Connecting to the first server
          if (m_ServerStore.TeamProjectCollections.Count == 0) {
            tfsProxy.UpdateTeamProjects(selectedServerInfo.Projects);
            m_ServerStore.AddCollection(selectedServerInfo.ServerUri, selectedServerInfo.InstanceId, tfsProxy, true);          
          } else {
            // Connecting to a new server
            tfsProxy.UpdateTeamProjects(selectedServerInfo.Projects);
            m_ServerStore.AddCollection(selectedServerInfo.ServerUri, selectedServerInfo.InstanceId, tfsProxy, false);
          }
        }        
      }

      return selectedServerInfo;
    }

    /// <summary>
    /// Disconnects from the CurrentServer
    /// </summary>
    public static void OnServerDisconnect(Guid collectionId, Guid newCollectionId)
    {
      /// Remove the pending changes for the removed collection
      m_PendingChanges.RemoveByServer(collectionId);
            
      /// Remove the collection from the store
      m_ServerStore.RemoveCollection(collectionId, newCollectionId);
    }

    /// <summary>
    /// Returns all users from a team project with their roles
    /// </summary>
    /// <param name="serverName">Server name</param>
    /// <param name="projectName">Team Project name</param>
    /// <returns></returns>
    public static TfsUserCollection OnTeamProjectSelected(Uri serverUri, Guid collectionId, string projectName)
    {
      m_ServerStore.SelectServerAndCollection(serverUri, collectionId);

      CurrentServer.SelectTeamProject(projectName);      
      CurrentServer.InitializeServerAndTeamProjectUsers();

      return ApplyPendingChanges();
    }
    #endregion

    #region Methods - Pending Changes related
    public static Status OnCommitChange(PendingChange pc)
    {
      Status status = Status.Failed;
      ITeamFoundationServerProxy proxy = ((ITeamFoundationServerProxy) m_ServerStore.TeamProjectCollections[pc.InstanceId]);

      try
      {
        switch (pc.Tier)
        {
          case SystemTier.TeamFoundation:
            if (pc.ChangeType == ChangeType.Add)
            {
              proxy.AddUserToRole(pc.TeamProject, pc.UserName, pc.Role);
            }
            if (pc.ChangeType == ChangeType.Delete)
            {
              if (pc.Role == "All")
              {
                // Remove the user from Team Foundation Server
                proxy.RemoveUser(pc.TeamProject, pc.UserName);

                // Remove the user from SharePoint
                proxy.SharePointRemoveUser(pc.TeamProject, pc.UserName);

                // Remove the user from Reporting Services
                proxy.ReportingServiceRemoveUser(pc.TeamProject, pc.UserName);
              }
              else
              {
                proxy.RemoveUserFromRole(pc.TeamProject, pc.UserName, pc.Role);
              }
            }
            break;
          case SystemTier.SharePoint:
            if (pc.ChangeType == ChangeType.Add)
            {
              proxy.SharePointAddUserToRole(pc.TeamProject, pc.UserName, pc.Role);
            }
            if (pc.ChangeType == ChangeType.Delete)
            {
              proxy.SharePointRemoveUserFromRole(pc.TeamProject, pc.UserName, pc.Role);
            }
            break;

          case SystemTier.ReportingServices:
            if (pc.ChangeType == ChangeType.Add)
            {
              proxy.ReportingServiceAddUserToRole(pc.TeamProject, pc.UserName, pc.Role);
            }
            if (pc.ChangeType == ChangeType.Delete)
            {
              proxy.ReportingServiceRemoveUserFromRole(pc.TeamProject, pc.UserName, pc.Role);
            }
            break;
        }
        status = Status.Passed;
      }
      catch (Exception ex)
      {
        TfsAdminToolTracer.TraceException(TfsAdminToolTracer.TraceSwitch.TraceError, ex);
      }

      return status;
    }

    public static TfsUserCollection OnDeleteUser(TfsUserCollection deletedUsers)
    {
      // Loop through the users marked for deletion
      foreach (TfsUser user in deletedUsers.Users)
      {
        if (user.State == UserState.New)
        {
          // Remove the pending changes
          m_PendingChanges.Remove(user.UserName, CurrentServer.Server.InstanceId, CurrentServer.SelectedTeamProject);
        }
        
        if (user.State == UserState.Default)
        {
          // Add the pending change. We decided to keep this atomic so we add only one pending change
          m_PendingChanges.Add(true, false, user.UserName, user.DisplayName, user.Email, ChangeType.Delete, CurrentServer.SelectedTeamProject, CurrentServer.Server.Name, CurrentServer.Server.InstanceId, SystemTier.TeamFoundation, "All");
        }
      }
      
      return ApplyPendingChanges();
    }

    public static TfsUserCollection OnUndo()
    {
      /// Remove the selected Pending Changes
      foreach (Guid guid in m_PendingChanges.Selected.Keys)
      {
        PendingChange change = m_PendingChanges.All[guid];

        // Check if this is the last pending change for the user in this team project
        if (m_PendingChanges.GetPendingChangesForUser(change.UserName,change.InstanceId,change.TeamProject).Count <= 1)
        {
          // Check if this user exsits prior to the change being undone if not remove it
          if (m_ServerStore.TeamProjectCollections[change.InstanceId].UserCollectionClean.GetUser(change.UserName) == null)
          {
            m_ServerStore.TeamProjectCollections[change.InstanceId].UserCollection.Remove(change.UserName);
          }
        }

        m_PendingChanges.Remove(guid);
      }

      return ApplyPendingChanges();
    }

    public static TfsUserCollection OnUserEditCompleted(TfsUserCollection editedUsers, string sourceTeamProject)
    {
      // Loop through the edited users and calculate the Pending Changes
      foreach (TfsUser targetUser in editedUsers.Users)
      {
        // This is the source user. It can be NULL is we add a new user
        TfsUser sourceUser = MainController.CurrentServer.UserCollection.GetUser(targetUser.UserName);

        // Compute the pending changes
        Dictionary<string, ChangeType> tfsChanges = TfsUser.GetRoleChanges(sourceUser, targetUser, SystemTier.TeamFoundation);
        foreach (KeyValuePair<string, ChangeType> change in tfsChanges)
        {
            m_PendingChanges.Add(true, false, targetUser.UserName, targetUser.DisplayName, targetUser.Email, change.Value, CurrentServer.SelectedTeamProject, CurrentServer.Server.Name, CurrentServer.Server.InstanceId, SystemTier.TeamFoundation, change.Key);
        }
        
        /// This code is used by Import User feature as well and we need to make
        /// sure that the pending changes are computed only for the roles that 
        /// are available both in the source and in the target team project
        if (CurrentServer.GetSharePointProxy(CurrentServer.SelectedTeamProject).SiteStatus == SiteStatus.Available &&
            CurrentServer.GetSharePointProxy(sourceTeamProject).SiteStatus                 == SiteStatus.Available)
        {
          Dictionary<string, ChangeType> spChanges = TfsUser.GetRoleChanges(sourceUser, targetUser, SystemTier.SharePoint);
          foreach (KeyValuePair<string, ChangeType> change in spChanges)
          {
              m_PendingChanges.Add(true, false, targetUser.UserName, targetUser.DisplayName, targetUser.Email, change.Value, CurrentServer.SelectedTeamProject, CurrentServer.Server.Name, CurrentServer.Server.InstanceId, SystemTier.SharePoint, change.Key);
          }
        }

        /// This code is used by Import User feature as well and we need to make
        /// sure that the pending changes are computed only for the roles that 
        /// are available both in the source and in the target team project
        if (CurrentServer.ReportServiceProxy.SiteStatus == SiteStatus.Available)
        {
          Dictionary<string, ChangeType> rsChanges = TfsUser.GetRoleChanges(sourceUser, targetUser, SystemTier.ReportingServices);
          foreach (KeyValuePair<string, ChangeType> change in rsChanges)
          {
              m_PendingChanges.Add(true, false, targetUser.UserName, targetUser.DisplayName, targetUser.Email, change.Value, CurrentServer.SelectedTeamProject, CurrentServer.Server.Name, CurrentServer.Server.InstanceId, SystemTier.ReportingServices, change.Key);
          }
        }
      }

      return ApplyPendingChanges();
    }

    private static TfsUserCollection ApplyPendingChanges()
    {
      // Pending Changes are always applied on the clean set of TfsUsers
      TfsUserCollection userCollection = TfsUserCollection.Clone(CurrentServer.UserCollectionClean);

      // Get the list of all the pending change for this Server/Team Project
      Dictionary<Guid, PendingChange> dictionary = PendingChanges.GetPendingChangesForTeamProject(CurrentServer.Server.InstanceId, CurrentServer.SelectedTeamProject);

      // Apply the Pending Changes
      userCollection.ApplyPendingChanges(dictionary);

      // Update the Disconnected list of TfsUsers
      MainController.CurrentServer.UserCollection.CopyUserRoles(userCollection);

      return userCollection;    
    }
    
    #endregion

    #region Methods - History related
    public static void OnHistoryClear()
    {
      m_History.ClearHistory();
    }

    public static void OnHistoryItemAdd(string userName, string displayName, ChangeType changeType, string teamProject, string server, SystemTier tier, string role, Status status)
    {
      m_History.AddHistoryItem(userName, displayName, changeType, server, teamProject, tier, role, status);
    }
    #endregion

    #region Properties
    public static ITeamFoundationServerProxy CurrentServer
    {
      get
      {
        return (ITeamFoundationServerProxy)m_ServerStore.SelectedTeamProjectCollection;
      }
    }

    public static PendingChanges PendingChanges
    {
      get
      {
        return m_PendingChanges;
      }
    }

    public static History History
    {
      get
      {
        return m_History;
      }
    }
    #endregion
  } // End Class
} // End Namespace