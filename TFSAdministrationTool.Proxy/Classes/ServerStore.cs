using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.TeamFoundation.Server;
using Microsoft.TeamFoundation.Client;

namespace TFSAdministrationTool.Proxy.Common
{
  public class ServerStore : ITeamProjectPickerDefaultSelectionProvider
  {
    #region Fields
    public Dictionary<Guid, ITeamFoundationServerProxy> TeamProjectCollections {get; private set;} 
    public Guid SelectedTeamProjectCollectionGuid {get; private set;}
    public Uri SelectedTeamFoundationServerUri { get; private set; }
    #endregion

    #region Constructor
    public ServerStore()
    {
      TeamProjectCollections = new Dictionary<Guid, ITeamFoundationServerProxy>();
    }
    #endregion

    #region Methods
    public void AddCollection(Uri serverUri, Guid collectionId, ITeamFoundationServerProxy proxy, bool select)
    {
      TeamProjectCollections.Add(collectionId, proxy);
      
      /// Select the Collection if needed
      if (select)
        SelectServerAndCollection(serverUri, collectionId);
    }

    public void RemoveCollection(Guid collectionId, Guid newCollectionId)
    {
      /// Remove the collection from the store
      TeamProjectCollections.Remove(collectionId);

      /// Select the new collection
      if (TeamProjectCollections.ContainsKey(newCollectionId))
      {
        SelectedTeamProjectCollectionGuid = newCollectionId;
      }
      else
      {
        SelectedTeamProjectCollectionGuid = Guid.Empty; 
      }
    }

    public void SelectServerAndCollection(Uri serverUri, Guid collectionId)
    {
      if (TeamProjectCollections.ContainsKey(collectionId))
      {
        SelectedTeamFoundationServerUri = serverUri;
        SelectedTeamProjectCollectionGuid = collectionId;
      }
    }
    #endregion

    #region ITeamProjectPickerDefaultSelectionProvider Members
    Guid? ITeamProjectPickerDefaultSelectionProvider.GetDefaultCollectionId(Uri instanceUri)
    {
      return SelectedTeamProjectCollectionGuid;
    }

    IEnumerable<string> ITeamProjectPickerDefaultSelectionProvider.GetDefaultProjects(Guid collectionId)
    {
      List<string> projects = new List<string>();

      if (TeamProjectCollections.ContainsKey(collectionId))
      {
        foreach (ProjectInfo pInfo in TeamProjectCollections[collectionId].TeamProjects)
        {
          projects.Add(pInfo.Uri);
        }
      }
      
      return projects;
    }

    Uri ITeamProjectPickerDefaultSelectionProvider.GetDefaultServerUri()
    {
      return SelectedTeamFoundationServerUri;
    }
    #endregion

    #region Properties
    public ITeamFoundationServerProxy SelectedTeamProjectCollection
    {
      get
      {
        return TeamProjectCollections[SelectedTeamProjectCollectionGuid];
      }
    }
    #endregion
  }

  public class ServerInfo
  {
    public Guid InstanceId { get; private set; }
    public Uri ServerUri { get; private set; }
    public Uri CollectionUri { get; private set; }
    public string Name { get; private set; }
    public ProjectInfo[] Projects { get; private set; }

    public ServerInfo(Guid instanceId, Uri serverUri, Uri collectionUri, string serverName, ProjectInfo[] projects)
    {
      InstanceId = instanceId;
      ServerUri = serverUri;
      CollectionUri = collectionUri;
      Name = serverName;
      Projects = projects;
    }
  }
}
