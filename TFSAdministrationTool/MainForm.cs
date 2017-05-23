#region Using Statements
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using TFSAdministrationTool.Controllers;
using TFSAdministrationTool.Properties;
using TFSAdministrationTool.Proxy;
using TFSAdministrationTool.Proxy.Common;
using Microsoft.TeamFoundation.Server;
using TFSAdministrationTool.Controls;
using System.Reflection;
using System.Net.Mail;
#endregion

namespace TFSAdministrationTool
{
    public partial class mainForm : Form
    {
        #region Variables
        private Icon userIcon = new Icon(Resources.User, new Size(16, 16));
        private Icon groupIcon = new Icon(Resources.Group, new Size(16, 16));
        private Icon addedUserIcon = new Icon(Resources.User_Added, new Size(16, 16));
        private Icon deletedUserIcon = new Icon(Resources.User_Deleted, new Size(16, 16));
        private Icon dirtyUserIcon = new Icon(Resources.User_Edited, new Size(16, 16));

        private Color errorCellColor = Color.Salmon;
        private Color missingCellColor = Color.DarkGray;
        private TreeNode selectedNode = null;
        private Font selectedNodeFont;

        private TfsUserBindingList userBindingList = new TfsUserBindingList();
        #endregion

        #region Constructor
        public mainForm()
        {
            InitializeComponent();

            /// Loading the images in the ImageList objects
            serverTreeViewImageList.Images.Add("ServerNode", Resources.server);
            serverTreeViewImageList.Images.Add("ProjectLeaf", Resources.teamproject);

            mainTabControlImageList.Images.Add("PendingChanges", Resources.pendingchangesTab);
            mainTabControlImageList.Images.Add("History", Resources.historyTab);
            mainTabControlImageList.Images.Add("Output", Resources.outputTab);

            historyListViewImageList.Images.Add("Passed", Resources.passed);
            historyListViewImageList.Images.Add("Failed", Resources.failed);

            /// Setting the images for the TabControl
            mainTabControl.TabPages[0].ImageIndex = 0;
            mainTabControl.TabPages[1].ImageIndex = 1;
            mainTabControl.TabPages[2].ImageIndex = 2;

            /// Set the application icon
            this.Icon = Resources.App;
        }
        #endregion

        #region MainForm Event Handlers
        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                selectedNodeFont = new Font(serverTreeView.Font, FontStyle.Bold);

                TfsAdminToolTracer.Initialize(outputTextBox);
                TfsAdminToolTracer.TraceMessage(TfsAdminToolTracer.TraceSwitch.TraceVerbose, "TFS Administration Tool " + "(" + Assembly.GetExecutingAssembly().GetName().Version + ") has started");

                //Check for valid email informations
                if (Properties.Settings.Default.NotifyUsersByEmail)
                {
                    try
                    {
                        MailMessage m = new MailMessage();
                        checkNotify.Checked = true;
                    }
                    catch (Exception smtpEx)
                    {
                        MessageBox.Show(string.Format(Resources.BadSMTPPrompt, smtpEx.Message), Resources.BadSMTPCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        checkNotify.Checked = false;
                        checkNotify.Enabled = false;
                    }   
                }
                else
                {
                    checkNotify.Checked = false;
                    checkNotify.Enabled = false;
                }

            }
            catch (Exception ex)
            {
                TfsAdminToolTracer.TraceException(TfsAdminToolTracer.TraceSwitch.TraceError, ex);
                MessageBox.Show(this, Resources.UnexpectedExceptionPrompt, Resources.UnexpectedExceptionCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            TfsAdminToolTracer.Destroy();
        }
        #endregion

        #region TreeView ToolStrip Event Handlers
        private void serverConnectToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (MainController.PendingChanges.All.Count == 0 ||
                   (MainController.PendingChanges.All.Count > 0 &&
                    MessageBox.Show(String.Format(Resources.ConnectPendingChangesExistsPrompt, String.Join(",", MainController.PendingChanges.GetTeamProjects().ToArray())), Resources.WarningCaption, MessageBoxButtons.YesNo) == DialogResult.Yes))
                {
                    TfsAdminToolTracer.TraceMessage(TfsAdminToolTracer.TraceSwitch.TraceInfo, "Button clicked: Server Explorer CONNECT");

                    // Connect to the selected server
                    StartTask(Resources.ServerConnectStatusPrompt);

                    ServerInfo si = MainController.OnServerConnect();

                    if (si != null)
                    {
                        // Remove Pending changes based on ServerInfo
                        MainController.PendingChanges.RemoveByServerInfo(si);

                        // Refresh the list of pending changes
                        PendingChangesDataBind(MainController.PendingChanges);

                        // Update the Server Explorer
                        TreeViewAddServer(si);

                        if (MainController.CurrentServer.SelectedTeamProject == string.Empty)
                        {
                            // Disable DataGrid
                            DataGridViewEnabled(false);
                        }

                        // Enable user import if adding more than one project
                        if (MainController.CurrentServer.TeamProjects.Length > 1)
                        {
                            if (MainController.CurrentServer.SelectedTeamProject != String.Empty)
                            {
                                usersImportToolStripButton.Enabled = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TfsAdminToolTracer.TraceException(TfsAdminToolTracer.TraceSwitch.TraceError, ex);
                MessageBox.Show(this, Resources.UnexpectedExceptionPrompt, Resources.UnexpectedExceptionCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                EndTask();
            }
        }

        /// <summary>
        /// Disconnects the selected server 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void serverDisconnectToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                TfsAdminToolTracer.TraceMessage(TfsAdminToolTracer.TraceSwitch.TraceInfo, "Button clicked: Server Explorer DISCONNECT");

                TreeNode serverNode;
                if (((TFSAdminTreeNodeTag)serverTreeView.SelectedNode.Tag).ItemType == ItemType.TeamProjectCollection)
                {
                    serverNode = serverTreeView.SelectedNode;
                }
                else
                {
                    serverNode = serverTreeView.SelectedNode.Parent;
                }
                TFSAdminTreeNodeTag serverNodeTag = (TFSAdminTreeNodeTag)serverNode.Tag;

                // In the case the server has some pending changes we should display a warning
                if (MainController.PendingChanges.GetPendingChangesForServer(serverNodeTag.InstanceId).Count == 0 ||
                   (MainController.PendingChanges.GetPendingChangesForServer(serverNodeTag.InstanceId).Count > 0 &&
                    MessageBox.Show(String.Format(Resources.PendingChangesExistsPrompt, serverNode.Text), Resources.WarningCaption, MessageBoxButtons.YesNo) == DialogResult.Yes))
                {
                    StartTask(Resources.ServerDisconnectStatusPrompt);

                    // Disable the DataGridView if we disconnect from the CurrentServer
                    if (serverNodeTag.InstanceId == MainController.CurrentServer.Server.InstanceId) DataGridViewEnabled(false);

                    /// Remove the node from the TreeView
                    TreeViewRemoveServer(serverNode.Text);

                    /// Delete the server from the collection
                    MainController.OnServerDisconnect(serverNodeTag.InstanceId, (serverTreeView.SelectedNode != null) ? ((TFSAdminTreeNodeTag)serverTreeView.SelectedNode.Tag).InstanceId : Guid.Empty);

                    // Refresh the list of pending changes
                    PendingChangesDataBind(MainController.PendingChanges);

                    TfsAdminToolTracer.TraceMessage(TfsAdminToolTracer.TraceSwitch.TraceInfo, "Successfully disconnected from Team Foundation Server - " + serverNode.Text);
                }
            }
            catch (Exception ex)
            {
                TfsAdminToolTracer.TraceException(TfsAdminToolTracer.TraceSwitch.TraceError, ex);
                MessageBox.Show(this, Resources.UnexpectedExceptionPrompt, Resources.UnexpectedExceptionCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                EndTask();
            }
        }
        #endregion

        #region TreeView Event Handlers
        private void serverTreeView_DoubleClick(object sender, EventArgs e)
        {
            // Make sure we did not DoubleClick on the highlighted node
            if (serverTreeView.SelectedNode != selectedNode)
            {
                // Enforce a limitation not be edit Server/ Team Project Collection permissions
                if (((TFSAdminTreeNodeTag)serverTreeView.SelectedNode.Tag).ItemType == ItemType.TeamProject)
                {
                    try
                    {
                        TreeNode serverNode = serverTreeView.SelectedNode.Parent;
                        TFSAdminTreeNodeTag serverNodeTag = (TFSAdminTreeNodeTag)serverNode.Tag;
                        string teamProjectName = serverTreeView.SelectedNode.Text;

                        // Highlight the selected node
                        TreeViewHighlightNode(serverNode.Text, teamProjectName);

                        /// Load user data and populate GridView
                        LoadUserDataFromServer(serverNodeTag.Uri, serverNodeTag.InstanceId, teamProjectName);
                    }
                    catch (Exception ex)
                    {
                        TfsAdminToolTracer.TraceException(TfsAdminToolTracer.TraceSwitch.TraceError, ex);
                        MessageBox.Show(this, Resources.UnexpectedExceptionPrompt, Resources.UnexpectedExceptionCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show(Resources.ServerWarningPrompt);
                }
            }
        }
        #endregion

        #region TreeView Helpers
        /// <summary>
        /// And a new Server to the TreeView or refreshes the nodes an existing one
        /// In case a node is selected, the selection is kept
        /// </summary>
        /// <param name="name">Name of the Server</param>
        /// <param name="list">List of child nodes</param>
        private void TreeViewAddServer(ServerInfo si)
        {
            TreeNode serverNode;
            string selectedNodeName = String.Empty;

            serverTreeView.BeginUpdate();

            /// If server node does not exist already, create it
            if (serverTreeView.Nodes.Find(si.Name, true).Length == 0)
            {
                serverNode = new TreeNode(si.Name);
                serverNode.Name = si.Name;
                serverNode.Tag = new TFSAdminTreeNodeTag() { Uri = si.ServerUri, ItemType = ItemType.TeamProjectCollection, InstanceId = si.InstanceId };
                serverNode.ImageIndex = 0;
                serverNode.SelectedImageIndex = 0;
                serverTreeView.Nodes.Add(serverNode);
            }
            else
            {
                serverNode = serverTreeView.Nodes.Find(si.Name, true)[0];
            }

            /// Check if the selected node is a child of the refreshed server node
            if (selectedNode != null && selectedNode.Parent == serverNode)
            {
                /// Save the name of the selected node and reset it
                selectedNodeName = selectedNode.Name;
                selectedNode = null;
            }

            /// Clear the nodes of the Server node
            serverNode.Nodes.Clear();

            /// Add the team project to the Server node
            foreach (ProjectInfo project in si.Projects)
            {
                TreeNode projectNode = new TreeNode(project.Name);
                projectNode.Name = project.Name;
                projectNode.Tag = new TFSAdminTreeNodeTag() { Uri = new Uri(project.Uri), ItemType = ItemType.TeamProject, InstanceId = Guid.Empty };
                projectNode.ImageIndex = 1;
                projectNode.SelectedImageIndex = 1;
                serverNode.Nodes.Add(projectNode);
            }

            /// Restore the selected node
            if (selectedNodeName != String.Empty)
            {
                TreeViewHighlightNode(serverNode.Name, selectedNodeName);
            }

            /// Sort and Focus
            serverTreeView.Sort();
            serverNode.Expand();
            serverTreeView.SelectedNode = serverNode;
            serverTreeView.Focus();
            serverTreeView.EndUpdate();

            // Make sure the treeview buttons are enabled
            TreeViewButtonsEnabled(true);
        }

        private void TreeViewRemoveServer(string name)
        {
            serverTreeView.BeginUpdate();

            // Find the server node
            TreeNode serverNode = serverTreeView.Nodes.Find(name, true)[0];

            // Selected team project is from the server we disconnected from
            if (selectedNode != null && selectedNode.Parent == serverNode)
            {
                selectedNode = null;
            }

            // Remove the server node
            serverTreeView.Nodes.RemoveByKey(name);

            // No more servers, disable disconnect button
            if (serverTreeView.Nodes.Count == 0)
            {
                TreeViewButtonsEnabled(false);
            }
            else
            {
                serverTreeView.SelectedNode = serverTreeView.Nodes[0];
            }

            serverTreeView.EndUpdate();
        }

        private void TreeViewHighlightNode(string serverName, string teamProject)
        {
            if (serverTreeView.Nodes.Find(serverName, true).Length > 0 && serverTreeView.Nodes.Find(teamProject, true).Length > 0)
            {
                TreeNode serverNode = serverTreeView.Nodes.Find(serverName, true)[0];
                TreeNode teamProjectNode = serverNode.Nodes.Find(teamProject, true)[0];

                serverTreeView.BeginUpdate();
                if (selectedNode != null)
                    selectedNode.NodeFont = selectedNode.TreeView.Font;

                teamProjectNode.NodeFont = selectedNodeFont;
                teamProjectNode.Text = teamProjectNode.Text;
                selectedNode = teamProjectNode;
                serverTreeView.EndUpdate();
            }
        }

        private void TreeViewButtonsEnabled(bool state)
        {
            serverDisconnectToolStripButton.Enabled = state;
        }
        #endregion

        #region DataGrid
        private void usersDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Make sure we did not double click on the Header
            if (e.RowIndex != -1)
            {
                try
                {
                    StartTask(Resources.UserEdtingStatusPrompt);
                    EditSelectedUsers();
                }
                catch (Exception ex)
                {
                    TfsAdminToolTracer.TraceException(TfsAdminToolTracer.TraceSwitch.TraceError, ex);
                    MessageBox.Show(this, Resources.UnexpectedExceptionPrompt, Resources.UnexpectedExceptionCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    EndTask();
                }
            }
        }

        private void usersDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (e.ColumnIndex < 0) return;

                TfsUser user = (TfsUser)usersDataGridView.Rows[e.RowIndex].DataBoundItem;

                // Image Column
                if (usersDataGridView.Columns[e.ColumnIndex].Name == "State")
                {
                    switch (((UserState)e.Value))
                    {
                        case UserState.New:
                            e.Value = addedUserIcon;
                            break;
                        case UserState.Default:
                            if (user.IdentityType == IdentityType.WindowsUser)
                            {
                                e.Value = userIcon;
                            }
                            else
                            {
                                e.Value = groupIcon;
                            }
                            break;
                        case UserState.Deleted:
                            e.Value = deletedUserIcon;
                            break;
                        case UserState.Edited:
                            e.Value = dirtyUserIcon;
                            break;
                    }
                }

                // SharePoint column
                if (usersDataGridView.Columns[e.ColumnIndex].Name == "SpRoles")
                {
                    if (MainController.CurrentServer.GetSharePointProxy(MainController.CurrentServer.SelectedTeamProject).SiteStatus == SiteStatus.Available)
                    {
                        // Validate the permissions of the user
                        bool result = MainController.CurrentServer.RoleValidator.ValidateTfsUser(user, SystemTier.SharePoint);
                        if (!result)
                        {
                            e.CellStyle.BackColor = errorCellColor;
                        }
                        TfsAdminToolTracer.TraceMessage(TfsAdminToolTracer.TraceSwitch.TraceInfo, "Validating user:" + user.UserName + " against SharePoint mappings. Result:" + result);
                    }
                    else
                    {
                        e.CellStyle.BackColor = missingCellColor;
                    }
                }

                // Reporting Services column
                if (usersDataGridView.Columns[e.ColumnIndex].Name == "RsRoles")
                {
                    if (MainController.CurrentServer.ReportServiceProxy.SiteStatus == SiteStatus.Available)
                    {
                        // Validate the permissions of the user
                        bool result = MainController.CurrentServer.RoleValidator.ValidateTfsUser(user, SystemTier.ReportingServices);
                        if (!result)
                        {
                            e.CellStyle.BackColor = errorCellColor;
                        }
                        TfsAdminToolTracer.TraceMessage(TfsAdminToolTracer.TraceSwitch.TraceInfo, "Validating user:" + user.UserName + " against Reporting Services mappings. Result:" + result);
                    }
                    else
                    {
                        e.CellStyle.BackColor = missingCellColor;
                    }
                }
            }
            catch (Exception ex)
            {
                TfsAdminToolTracer.TraceException(TfsAdminToolTracer.TraceSwitch.TraceError, ex);
                MessageBox.Show(this, Resources.UnexpectedExceptionPrompt, Resources.UnexpectedExceptionCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DataGridViewRender(TfsUserCollection users)
        {
            userBindingList.InitializeList(users.Users);
            usersBindingSource.DataSource = userBindingList;
            usersBindingSource.CurrencyManager.Refresh();
            if (usersBindingSource.CurrencyManager.Count > 0)
                usersBindingSource.CurrencyManager.Position = 0;

            // Enable the DataGridView
            DataGridViewEnabled(true);
        }

        private void DataGridViewEnabled(bool state)
        {
            usersDataGridView.Visible = state;

            usersAddToolStripButton.Enabled = state;
            usersEditToolStripButton.Enabled = state;
            usersDeleteToolStripButton.Enabled = state;
            usersRefreshStripButton.Enabled = state;

            if (state == true)
            {
                if (MainController.CurrentServer.TeamProjects.Length > 1)
                {
                    usersImportToolStripButton.Enabled = true;
                }
                else
                {
                    usersImportToolStripButton.Enabled = false;
                }


            }
            else
            {
                usersImportToolStripButton.Enabled = false;
            }
        }
        #endregion

        #region DataGrid ToolStrip Event Handlers
        private void usersImportToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                TfsAdminToolTracer.TraceMessage(TfsAdminToolTracer.TraceSwitch.TraceInfo, "Button clicked: Users list IMPORT");

                UserImport dlgImportUsers = new UserImport();
                if (dlgImportUsers.ShowDialog() == DialogResult.OK)
                {
                    // This is where Magic happens
                    TfsUserCollection mergedUsers = MainController.OnUserEditCompleted(UserController.UserCollection, UserController.TeamProject);

                    // Refresh the list of pending changes
                    PendingChangesDataBind(MainController.PendingChanges);

                    // Refresh the list of users
                    DataGridViewRender(mergedUsers);
                }
            }
            catch (Exception ex)
            {
                TfsAdminToolTracer.TraceException(TfsAdminToolTracer.TraceSwitch.TraceError, ex);
                MessageBox.Show(this, Resources.UnexpectedExceptionPrompt, Resources.UnexpectedExceptionCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void usersAddToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                TfsAdminToolTracer.TraceMessage(TfsAdminToolTracer.TraceSwitch.TraceInfo, "Button clicked: Users list ADD");

                UserConfiguration dlgConfigUsers = new UserConfiguration(MainController.CurrentServer);
                if (dlgConfigUsers.ShowDialog() == DialogResult.OK)
                {
                    // This is where Magic happens
                    TfsUserCollection mergedUsers = MainController.OnUserEditCompleted(UserController.UserCollection, UserController.TeamProject);

                    // Refresh the list of pending changes
                    PendingChangesDataBind(MainController.PendingChanges);

                    // Refresh the list of users
                    DataGridViewRender(mergedUsers);
                }
            }
            catch (Exception ex)
            {
                TfsAdminToolTracer.TraceException(TfsAdminToolTracer.TraceSwitch.TraceError, ex);
                MessageBox.Show(this, Resources.UnexpectedExceptionPrompt, Resources.UnexpectedExceptionCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void usersEditToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                TfsAdminToolTracer.TraceMessage(TfsAdminToolTracer.TraceSwitch.TraceInfo, "Button clicked: Users list EDIT");

                StartTask(Resources.UserEdtingStatusPrompt);
                EditSelectedUsers();
            }
            catch (Exception ex)
            {
                TfsAdminToolTracer.TraceException(TfsAdminToolTracer.TraceSwitch.TraceError, ex);
                MessageBox.Show(this, Resources.UnexpectedExceptionPrompt, Resources.UnexpectedExceptionCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                EndTask();
            }
        }

        private void usersDeleteToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                TfsAdminToolTracer.TraceMessage(TfsAdminToolTracer.TraceSwitch.TraceInfo, "Button clicked: Users list DELETE");

                TfsUserCollection selectedUsers = new TfsUserCollection();
                List<string> invalidUsers = new List<string>();

                /// Loop through the selected rows
                /// We cannot delete a user if:
                ///   - User is marked for deletion
                ///   - User has pending change
                ///   
                ///   - User is the TFSSERVICE account
                ///   - User is a Team Foundation Administrator and it is the user who is running the tool
                foreach (DataGridViewRow row in usersDataGridView.SelectedRows)
                {
                    TfsUser user = TfsUser.Clone((TfsUser)row.DataBoundItem);

                    if ((user.State == UserState.Deleted) || // User is marked for deletion
                        (user.State == UserState.Edited) ||  // User has pending changes
                        (MainController.CurrentServer.SelectedItemType == ItemType.TeamProjectCollection && MainController.CurrentServer.IsUserServiceAccount(user)) || // User is the Service Account
                        (MainController.CurrentServer.SelectedItemType == ItemType.TeamProjectCollection && TfsUser.IsCurrentUser(user) && MainController.CurrentServer.IsUserTeamFoundationAdministrator(user))) // Current user from Team Foundation Administrators group
                    {
                        invalidUsers.Add(user.UserName);
                    }
                    else
                    {
                        selectedUsers.Add(user);
                    }
                }
                if (invalidUsers.Count > 0)
                {
                    MessageBox.Show(Resources.UsersGridDeleteWarningMessage.Replace("{0}", String.Join(",", invalidUsers.ToArray())), "Warning", MessageBoxButtons.OK);
                }

                if (selectedUsers.Users.Count > 0)
                {
                    /// Perform the Magic
                    TfsUserCollection mergedUsers = MainController.OnDeleteUser(selectedUsers);

                    // Refresh the list of pending changes
                    PendingChangesDataBind(MainController.PendingChanges);

                    // Refresh the Grid
                    DataGridViewRender(mergedUsers);
                }
            }
            catch (Exception ex)
            {
                TfsAdminToolTracer.TraceException(TfsAdminToolTracer.TraceSwitch.TraceError, ex);
                MessageBox.Show(this, Resources.UnexpectedExceptionPrompt, Resources.UnexpectedExceptionCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void usersRefreshStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                TfsAdminToolTracer.TraceMessage(TfsAdminToolTracer.TraceSwitch.TraceInfo, "Button clicked: Users list REFRESH");

                /// Load user data and populate GridView
                LoadUserDataFromServer(MainController.CurrentServer.Server.Uri, MainController.CurrentServer.Server.InstanceId, MainController.CurrentServer.SelectedTeamProject);
            }
            catch (Exception ex)
            {
                TfsAdminToolTracer.TraceException(TfsAdminToolTracer.TraceSwitch.TraceError, ex);
                MessageBox.Show(this, Resources.UnexpectedExceptionPrompt, Resources.UnexpectedExceptionCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void aboutToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                AboutBox about = new AboutBox();
                about.StartPosition = FormStartPosition.CenterParent;
                about.ShowDialog();
            }
            catch (Exception ex)
            {
                TfsAdminToolTracer.TraceException(TfsAdminToolTracer.TraceSwitch.TraceError, ex);
                MessageBox.Show(this, Resources.UnexpectedExceptionPrompt, Resources.UnexpectedExceptionCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Pending Changes Tab
        private void lvPendingChanges_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            try
            {
                // commentaire joli
                MainController.PendingChanges.UpdateSelected(new Guid(e.Item.Name), e.IsSelected);

                if (pendingChangesListView.SelectedItems.Count > 0)
                {
                    pendingChangesUndoToolStripButton.Enabled = true;
                }
                else
                {
                    pendingChangesUndoToolStripButton.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                TfsAdminToolTracer.TraceException(TfsAdminToolTracer.TraceSwitch.TraceError, ex);
                MessageBox.Show(this, Resources.UnexpectedExceptionPrompt, Resources.UnexpectedExceptionCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pendingChangesListView_Leave(object sender, EventArgs e)
        {
            try
            {
                foreach (ListViewItem item in pendingChangesListView.Items)
                {
                    item.Selected = false;
                }
            }
            catch (Exception ex)
            {
                TfsAdminToolTracer.TraceException(TfsAdminToolTracer.TraceSwitch.TraceError, ex);
                MessageBox.Show(this, Resources.UnexpectedExceptionPrompt, Resources.UnexpectedExceptionCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lvPendingChanges_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            try
            {
                MainController.PendingChanges.UpdateChecked(new Guid(e.Item.Name), e.Item.Checked);
            }
            catch (Exception ex)
            {
                TfsAdminToolTracer.TraceException(TfsAdminToolTracer.TraceSwitch.TraceError, ex);
                MessageBox.Show(this, Resources.UnexpectedExceptionPrompt, Resources.UnexpectedExceptionCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pendingChangesCommitToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                TfsAdminToolTracer.TraceMessage(TfsAdminToolTracer.TraceSwitch.TraceInfo, "Button clicked: Pending Changes COMMIT");

                // Do we have at least one checked item
                if (MainController.PendingChanges.Checked.Count > 0)
                {
                    bool isDirty = false;

                    NotificationService notifier = new NotificationService();



                    foreach (Guid guid in MainController.PendingChanges.Checked.Keys)
                    {
                        PendingChange pendingChange = MainController.PendingChanges.Item(guid);

                        /// Build the status message
                        string statusMessage = (pendingChange.ChangeType == TFSAdministrationTool.Proxy.Common.ChangeType.Add) ? Resources.PendingChangeActionAdd + " " : Resources.PendingChangeActionDelete + " ";
                        statusMessage = statusMessage + pendingChange.UserName + " - " + pendingChange.TeamProject + " - " + pendingChange.Role;
                        StartTask(statusMessage);

                        /// Construct the notification
                        if (pendingChange.Email != null && Properties.Settings.Default.NotifyUsersByEmail)
                        {
                            TfsAdminToolTracer.TraceMessage(TfsAdminToolTracer.TraceSwitch.TraceInfo, string.Format("Construct notification for {0} for {1}", pendingChange.Email, statusMessage));
                            NotificationLine notif = new NotificationLine(pendingChange);
                            if (!notifier.Notifications.ContainsKey(pendingChange.Email))
                            {
                                notifier.Notifications[pendingChange.Email] = new List<NotificationLine>();
                            }
                            notifier.Notifications[pendingChange.Email].Add(notif);
                        }


                        /// Perform the Task Using the Controller
                        Status status = MainController.OnCommitChange(pendingChange);

                        /// Add the item to the History
                        MainController.OnHistoryItemAdd(pendingChange.UserName,
                                   pendingChange.DisplayName,
                                   pendingChange.ChangeType,
                                   pendingChange.Server,
                                   pendingChange.TeamProject,
                                   pendingChange.Tier,
                                   pendingChange.Role,
                                   status);

                        if (status == Status.Passed)
                        {
                            /// Remove the item from the Pending Changes object
                            MainController.PendingChanges.Remove(guid);

                            /// Mark the user list dirty only if there was at least
                            /// one pending change to the currently selected team project
                            if (pendingChange.TeamProject == MainController.CurrentServer.SelectedTeamProject)
                            {
                                isDirty = true;
                            }
                        }
                    }


                    /// Append the newly added Pending Changes to the ListView
                    PendingChangesDataBind(MainController.PendingChanges);

                    /// Append the newly added History items
                    HistoryDataBind();

                    if (isDirty)
                    {
                        /// Send notifications, if available, and correctly configured
                        if (Properties.Settings.Default.NotifyUsersByEmail && checkNotify.Checked)
                        {
                            string currMail = string.Empty;

                            using (SmtpClient client = new SmtpClient())
                            {
                                /// sort the notifications by email, and send an aggregated note
                                foreach (MailMessage mail in notifier.PrepareNotifications())
                                {
                                    currMail = mail.To[0].ToString();
                                    try
                                    {
                                        client.Send(mail);
                                    }
                                    catch (Exception sendEx)
                                    {
                                        MessageBox.Show(string.Format(Resources.MailCouldNotBeSentError, currMail, sendEx.Message), Resources.UnexpectedExceptionCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                            }

                        }

                        /// Refresh the Grid if any changes were successfully commited
                        LoadUserDataFromServer(MainController.CurrentServer.Server.Uri, MainController.CurrentServer.Server.InstanceId, MainController.CurrentServer.SelectedTeamProject);
                    }
                }
            }
            catch (Exception ex)
            {
                TfsAdminToolTracer.TraceException(TfsAdminToolTracer.TraceSwitch.TraceError, ex);
                MessageBox.Show(this, Resources.UnexpectedExceptionPrompt, Resources.UnexpectedExceptionCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                EndTask();
            }
        }

        private void pendingChangesUndoToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                TfsAdminToolTracer.TraceMessage(TfsAdminToolTracer.TraceSwitch.TraceInfo, "Button clicked: Pending Changes UNDO");

                /// Do we have at least one Selected item      
                if (MainController.PendingChanges.Selected.Count > 0)
                {
                    // Perform the Undo operation
                    DataGridViewRender(MainController.OnUndo());

                    // Refresh the list of Pending Changes
                    PendingChangesDataBind(MainController.PendingChanges);
                }
            }
            catch (Exception ex)
            {
                TfsAdminToolTracer.TraceException(TfsAdminToolTracer.TraceSwitch.TraceError, ex);
                MessageBox.Show(this, Resources.UnexpectedExceptionPrompt, Resources.UnexpectedExceptionCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PendingChangesDataBind(PendingChanges pendingChanges)
        {
            pendingChangesListView.BeginUpdate();
            pendingChangesListView.Items.Clear();

            foreach (KeyValuePair<Guid, PendingChange> pendingChangesItem in pendingChanges.All)
            {
                PendingChange pendingChange = pendingChangesItem.Value;

                ListViewItem item = new ListViewItem(pendingChange.UserName);
                item.Name = pendingChangesItem.Key.ToString();
                item.SubItems.AddRange(new string[] {pendingChange.DisplayName,
                                             pendingChange.ChangeType.ToString(),
                                             pendingChange.Server,
                                             pendingChange.TeamProject,
                                             pendingChange.Tier.ToString(),
                                             pendingChange.Role});
                item.Checked = pendingChange.Checked;
                item.Selected = pendingChange.Selected;
                pendingChangesListView.Items.Add(item);
            }
            pendingChangesListView.EndUpdate();

            // Set the button states
            if (pendingChangesListView.CheckedItems.Count == 0)
            {
                pendingChangesCommitToolStripButton.Enabled = false;
            }
            else
            {
                pendingChangesCommitToolStripButton.Enabled = true;
            }

            pendingChangesUndoToolStripButton.Enabled = false;
        }
        #endregion

        #region History Tab
        private void historyClearToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                TfsAdminToolTracer.TraceMessage(TfsAdminToolTracer.TraceSwitch.TraceInfo, "Button clicked: History CLEAR");

                MainController.OnHistoryClear();
                historyListView.Items.Clear();
                historyClearToolStripButton.Enabled = false;
            }
            catch (Exception ex)
            {
                TfsAdminToolTracer.TraceException(TfsAdminToolTracer.TraceSwitch.TraceError, ex);
                MessageBox.Show(this, Resources.UnexpectedExceptionPrompt, Resources.UnexpectedExceptionCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void HistoryDataBind()
        {
            historyListView.BeginUpdate();

            foreach (HistoryItem historyItem in MainController.History.GetNewHistoryItems())
            {
                ListViewItem item = new ListViewItem(historyItem.UserName);
                item.ImageIndex = (historyItem.Status == Status.Passed) ? 0 : 1;
                item.SubItems.AddRange(new string[] {historyItem.DisplayName,
                                            historyItem.ChangeType.ToString(),
                                            DateTime.Now.ToString(),
                                            historyItem.Server,
                                            historyItem.TeamProject,
                                            historyItem.Tier.ToString(),
                                            historyItem.Role,
                                            historyItem.Status.ToString()});
                historyListView.Items.Add(item);
            }

            historyListView.EndUpdate();

            // Set the state of the button
            if (historyListView.Items.Count > 0)
            {
                historyClearToolStripButton.Enabled = true;
            }
        }
        #endregion

        #region Output Tab
        private void outputClearToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                outputTextBox.Clear();

                TfsAdminToolTracer.TraceMessage(TfsAdminToolTracer.TraceSwitch.TraceInfo, "Button clicked: Output CLEAR");
            }
            catch (Exception ex)
            {
                TfsAdminToolTracer.TraceException(TfsAdminToolTracer.TraceSwitch.TraceError, ex);
                MessageBox.Show(this, Resources.UnexpectedExceptionPrompt, Resources.UnexpectedExceptionCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Helpers
        private void EditSelectedUsers()
        {
            TfsUserCollection selectedUsers = new TfsUserCollection();
            List<string> invalidUsers = new List<string>();

            foreach (DataGridViewRow row in usersDataGridView.SelectedRows)
            {
                TfsUser user = TfsUser.Clone((TfsUser)row.DataBoundItem);

                // We should not allow editing: 
                //   - Users that are marked for deletion 
                //   - Users that is part of the Service 
                if ((user.State == UserState.Deleted) || // User is marked for deletion
                    (MainController.CurrentServer.SelectedItemType == ItemType.TeamProjectCollection && MainController.CurrentServer.IsUserServiceAccount(user)) || // User is the Service Account
                    (MainController.CurrentServer.SelectedItemType == ItemType.TeamProjectCollection && TfsUser.IsCurrentUser(user) && MainController.CurrentServer.IsUserTeamFoundationAdministrator(user))) // Current user from Team Foundation Administrators group  
                {
                    invalidUsers.Add(user.UserName);
                }
                else
                {
                    selectedUsers.Add(user);
                }
            }

            if (invalidUsers.Count > 0)
            {
                MessageBox.Show(Resources.UsersGridEditWarningMessage.Replace("{0}", String.Join(",", invalidUsers.ToArray())), "Warning", MessageBoxButtons.OK);
            }

            // This is necessary as there is a chance that the selected user
            // is marked for deletion. If so the user cannot be edited.
            if (selectedUsers.Users.Count > 0)
            {
                UserConfiguration dlgConfigUsers = new UserConfiguration(MainController.CurrentServer, selectedUsers);
                if (dlgConfigUsers.ShowDialog() == DialogResult.OK)
                {
                    // This is where Magic happens
                    TfsUserCollection mergedUsers = MainController.OnUserEditCompleted(UserController.UserCollection, UserController.TeamProject);

                    // Refresh the list of pending changes
                    PendingChangesDataBind(MainController.PendingChanges);

                    // Refresh the list of users
                    DataGridViewRender(mergedUsers);
                }
            }

            EndTask();
        }

        private void LoadUserDataFromServer(Uri serverUri, Guid collectionId, string teamProject)
        {
            // Get the users with the roles
            try
            {
                // Add (working...) from the TreeNode
                selectedNode.Text = selectedNode.Text + " (working...)";

                // Start the task
                StartTask(Resources.LoadingUserInfoStatusPrompt);

                TfsUserCollection users = MainController.OnTeamProjectSelected(serverUri, collectionId, teamProject);

                SiteStatus spStatus = MainController.CurrentServer.GetSharePointProxy(teamProject).SiteStatus;
                SiteStatus rsStatus = MainController.CurrentServer.ReportServiceProxy.SiteStatus;

                SystemAvailability dlg = new SystemAvailability(spStatus, rsStatus);

                //Test for invalid combinations and abort
                if (spStatus == SiteStatus.Error || spStatus == SiteStatus.Unauthorized ||
                    rsStatus == SiteStatus.Error || rsStatus == SiteStatus.Unauthorized)
                {
                    dlg.ShowDialog();
                    DataGridViewEnabled(false);
                }
                else
                {
                    //Prompt and inform user about lack of functionality
                    if (spStatus == SiteStatus.Unavailable || spStatus == SiteStatus.Unavailable)
                    {
                        if (Properties.Settings.Default.PromptForSystemAvaiablity)
                        {
                            dlg.ShowDialog();
                        }
                    }
                    DataGridViewRender(users);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                // Remove (working...) from the TreeNode
                selectedNode.Text = selectedNode.Text.Substring(0, selectedNode.Text.Length - 13);
                EndTask();
            }
        }

        private void StartTask(string task)
        {
            toolStripStatusLabel.Text = task;
            this.Cursor = Cursors.WaitCursor;
            Application.DoEvents();
        }

        private void EndTask()
        {
            toolStripStatusLabel.Text = Resources.TaskDoneStatusPrompt;
            this.Cursor = Cursors.Default;
            Application.DoEvents();
        }
        #endregion

        private void checkNotify_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.NotifyUsersByEmail = (sender as CheckBox).Checked;
        }

    } //End Class
} //End Namespace