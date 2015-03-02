using TFSAdministrationTool.Controls;

namespace TFSAdministrationTool
{
  partial class mainForm
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mainForm));
      this.splitContainer1 = new System.Windows.Forms.SplitContainer();
      this.splitContainer2 = new System.Windows.Forms.SplitContainer();
      this.serverTreeViewImageList = new System.Windows.Forms.ImageList(this.components);
      this.serverToolStrip = new System.Windows.Forms.ToolStrip();
      this.serverConnectToolStripButton = new System.Windows.Forms.ToolStripButton();
      this.serverDisconnectToolStripButton = new System.Windows.Forms.ToolStripButton();
      this.usersDataGridView = new System.Windows.Forms.DataGridView();
      this.State = new System.Windows.Forms.DataGridViewImageColumn();
      this.userNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.displayNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.TfsRoles = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.SpRoles = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.RsRoles = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.usersBindingSource = new System.Windows.Forms.BindingSource(this.components);
      this.usersToolStrip = new System.Windows.Forms.ToolStrip();
      this.aboutToolStripButton = new System.Windows.Forms.ToolStripButton();
      this.usersAddToolStripButton = new System.Windows.Forms.ToolStripButton();
      this.usersEditToolStripButton = new System.Windows.Forms.ToolStripButton();
      this.usersDeleteToolStripButton = new System.Windows.Forms.ToolStripButton();
      this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
      this.usersImportToolStripButton = new System.Windows.Forms.ToolStripButton();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this.usersRefreshStripButton = new System.Windows.Forms.ToolStripButton();
      this.mainTabControl = new System.Windows.Forms.TabControl();
      this.pendingChangesTabPage = new System.Windows.Forms.TabPage();
      this.pendingChangesListView = new System.Windows.Forms.ListView();
      this.UserName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.DisplayName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.Change = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.Server = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.TeamProject = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.Tier = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.Role = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.pendingChangesToolStrip = new System.Windows.Forms.ToolStrip();
      this.pendingChangesCommitToolStripButton = new System.Windows.Forms.ToolStripButton();
      this.pendingChangesUndoToolStripButton = new System.Windows.Forms.ToolStripButton();
      this.historyTabPage = new System.Windows.Forms.TabPage();
      this.historyListView = new System.Windows.Forms.ListView();
      this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.Date = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader14 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader11 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader12 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader13 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.historyListViewImageList = new System.Windows.Forms.ImageList(this.components);
      this.historyToolStrip = new System.Windows.Forms.ToolStrip();
      this.historyClearToolStripButton = new System.Windows.Forms.ToolStripButton();
      this.outputTabPage = new System.Windows.Forms.TabPage();
      this.outputTextBox = new System.Windows.Forms.TextBox();
      this.outputToolStrip = new System.Windows.Forms.ToolStrip();
      this.outputClearToolStripButton = new System.Windows.Forms.ToolStripButton();
      this.mainTabControlImageList = new System.Windows.Forms.ImageList(this.components);
      this.logDataGridView = new System.Windows.Forms.DataGridView();
      this.mainStatusStrip = new System.Windows.Forms.StatusStrip();
      this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
      this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
      this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
      this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
      this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
      this.serverTreeView = new TFSAdministrationTool.Controls.TFSAdminTreeView();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      this.splitContainer2.Panel1.SuspendLayout();
      this.splitContainer2.Panel2.SuspendLayout();
      this.splitContainer2.SuspendLayout();
      this.serverToolStrip.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.usersDataGridView)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.usersBindingSource)).BeginInit();
      this.usersToolStrip.SuspendLayout();
      this.mainTabControl.SuspendLayout();
      this.pendingChangesTabPage.SuspendLayout();
      this.pendingChangesToolStrip.SuspendLayout();
      this.historyTabPage.SuspendLayout();
      this.historyToolStrip.SuspendLayout();
      this.outputTabPage.SuspendLayout();
      this.outputToolStrip.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.logDataGridView)).BeginInit();
      this.mainStatusStrip.SuspendLayout();
      this.SuspendLayout();
      // 
      // splitContainer1
      // 
      resources.ApplyResources(this.splitContainer1, "splitContainer1");
      this.splitContainer1.Name = "splitContainer1";
      // 
      // splitContainer1.Panel1
      // 
      this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
      // 
      // splitContainer1.Panel2
      // 
      this.splitContainer1.Panel2.Controls.Add(this.mainTabControl);
      this.splitContainer1.Panel2.Controls.Add(this.logDataGridView);
      this.splitContainer1.Panel2.Controls.Add(this.mainStatusStrip);
      // 
      // splitContainer2
      // 
      resources.ApplyResources(this.splitContainer2, "splitContainer2");
      this.splitContainer2.Name = "splitContainer2";
      // 
      // splitContainer2.Panel1
      // 
      this.splitContainer2.Panel1.Controls.Add(this.serverTreeView);
      this.splitContainer2.Panel1.Controls.Add(this.serverToolStrip);
      // 
      // splitContainer2.Panel2
      // 
      this.splitContainer2.Panel2.Controls.Add(this.usersDataGridView);
      this.splitContainer2.Panel2.Controls.Add(this.usersToolStrip);
      // 
      // serverTreeViewImageList
      // 
      this.serverTreeViewImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
      resources.ApplyResources(this.serverTreeViewImageList, "serverTreeViewImageList");
      this.serverTreeViewImageList.TransparentColor = System.Drawing.Color.Transparent;
      // 
      // serverToolStrip
      // 
      this.serverToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.serverConnectToolStripButton,
            this.serverDisconnectToolStripButton});
      resources.ApplyResources(this.serverToolStrip, "serverToolStrip");
      this.serverToolStrip.Name = "serverToolStrip";
      // 
      // serverConnectToolStripButton
      // 
      this.serverConnectToolStripButton.Image = global::TFSAdministrationTool.Properties.Resources.serverConnectToolStripButton;
      resources.ApplyResources(this.serverConnectToolStripButton, "serverConnectToolStripButton");
      this.serverConnectToolStripButton.Name = "serverConnectToolStripButton";
      this.serverConnectToolStripButton.Click += new System.EventHandler(this.serverConnectToolStripButton_Click);
      // 
      // serverDisconnectToolStripButton
      // 
      this.serverDisconnectToolStripButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
      resources.ApplyResources(this.serverDisconnectToolStripButton, "serverDisconnectToolStripButton");
      this.serverDisconnectToolStripButton.Image = global::TFSAdministrationTool.Properties.Resources.serverDisconnectToolStripButton;
      this.serverDisconnectToolStripButton.Name = "serverDisconnectToolStripButton";
      this.serverDisconnectToolStripButton.Click += new System.EventHandler(this.serverDisconnectToolStripButton_Click);
      // 
      // usersDataGridView
      // 
      this.usersDataGridView.AllowUserToAddRows = false;
      this.usersDataGridView.AllowUserToDeleteRows = false;
      this.usersDataGridView.AllowUserToOrderColumns = true;
      this.usersDataGridView.AutoGenerateColumns = false;
      this.usersDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
      this.usersDataGridView.BackgroundColor = System.Drawing.Color.White;
      this.usersDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.usersDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.usersDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.State,
            this.userNameDataGridViewTextBoxColumn,
            this.displayNameDataGridViewTextBoxColumn,
            this.TfsRoles,
            this.SpRoles,
            this.RsRoles});
      this.usersDataGridView.DataSource = this.usersBindingSource;
      resources.ApplyResources(this.usersDataGridView, "usersDataGridView");
      this.usersDataGridView.Name = "usersDataGridView";
      this.usersDataGridView.ReadOnly = true;
      this.usersDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.usersDataGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.usersDataGridView_CellDoubleClick);
      this.usersDataGridView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.usersDataGridView_CellFormatting);
      // 
      // State
      // 
      this.State.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
      this.State.DataPropertyName = "State";
      this.State.FillWeight = 24F;
      this.State.Frozen = true;
      resources.ApplyResources(this.State, "State");
      this.State.Name = "State";
      this.State.ReadOnly = true;
      this.State.Resizable = System.Windows.Forms.DataGridViewTriState.False;
      // 
      // userNameDataGridViewTextBoxColumn
      // 
      this.userNameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
      this.userNameDataGridViewTextBoxColumn.DataPropertyName = "UserName";
      this.userNameDataGridViewTextBoxColumn.Frozen = true;
      resources.ApplyResources(this.userNameDataGridViewTextBoxColumn, "userNameDataGridViewTextBoxColumn");
      this.userNameDataGridViewTextBoxColumn.Name = "userNameDataGridViewTextBoxColumn";
      this.userNameDataGridViewTextBoxColumn.ReadOnly = true;
      // 
      // displayNameDataGridViewTextBoxColumn
      // 
      this.displayNameDataGridViewTextBoxColumn.DataPropertyName = "DisplayName";
      resources.ApplyResources(this.displayNameDataGridViewTextBoxColumn, "displayNameDataGridViewTextBoxColumn");
      this.displayNameDataGridViewTextBoxColumn.Name = "displayNameDataGridViewTextBoxColumn";
      this.displayNameDataGridViewTextBoxColumn.ReadOnly = true;
      // 
      // TfsRoles
      // 
      this.TfsRoles.DataPropertyName = "TfsRolesString";
      resources.ApplyResources(this.TfsRoles, "TfsRoles");
      this.TfsRoles.Name = "TfsRoles";
      this.TfsRoles.ReadOnly = true;
      // 
      // SpRoles
      // 
      this.SpRoles.DataPropertyName = "SpRolesString";
      resources.ApplyResources(this.SpRoles, "SpRoles");
      this.SpRoles.Name = "SpRoles";
      this.SpRoles.ReadOnly = true;
      // 
      // RsRoles
      // 
      this.RsRoles.DataPropertyName = "RsRolesString";
      resources.ApplyResources(this.RsRoles, "RsRoles");
      this.RsRoles.Name = "RsRoles";
      this.RsRoles.ReadOnly = true;
      // 
      // usersBindingSource
      // 
      this.usersBindingSource.DataSource = typeof(TFSAdministrationTool.Proxy.Common.TfsUser);
      // 
      // usersToolStrip
      // 
      this.usersToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripButton,
            this.usersAddToolStripButton,
            this.usersEditToolStripButton,
            this.usersDeleteToolStripButton,
            this.toolStripSeparator2,
            this.usersImportToolStripButton,
            this.toolStripSeparator1,
            this.usersRefreshStripButton});
      resources.ApplyResources(this.usersToolStrip, "usersToolStrip");
      this.usersToolStrip.Name = "usersToolStrip";
      // 
      // aboutToolStripButton
      // 
      this.aboutToolStripButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
      this.aboutToolStripButton.Image = global::TFSAdministrationTool.Properties.Resources.aboutToolStripButton;
      resources.ApplyResources(this.aboutToolStripButton, "aboutToolStripButton");
      this.aboutToolStripButton.Name = "aboutToolStripButton";
      this.aboutToolStripButton.Click += new System.EventHandler(this.aboutToolStripButton_Click);
      // 
      // usersAddToolStripButton
      // 
      resources.ApplyResources(this.usersAddToolStripButton, "usersAddToolStripButton");
      this.usersAddToolStripButton.Image = global::TFSAdministrationTool.Properties.Resources.usersAddToolStripButton;
      this.usersAddToolStripButton.Name = "usersAddToolStripButton";
      this.usersAddToolStripButton.Click += new System.EventHandler(this.usersAddToolStripButton_Click);
      // 
      // usersEditToolStripButton
      // 
      resources.ApplyResources(this.usersEditToolStripButton, "usersEditToolStripButton");
      this.usersEditToolStripButton.Image = global::TFSAdministrationTool.Properties.Resources.usersEditToolStripButton;
      this.usersEditToolStripButton.Name = "usersEditToolStripButton";
      this.usersEditToolStripButton.Click += new System.EventHandler(this.usersEditToolStripButton_Click);
      // 
      // usersDeleteToolStripButton
      // 
      resources.ApplyResources(this.usersDeleteToolStripButton, "usersDeleteToolStripButton");
      this.usersDeleteToolStripButton.Image = global::TFSAdministrationTool.Properties.Resources.usersDeleteToolStripButton;
      this.usersDeleteToolStripButton.Name = "usersDeleteToolStripButton";
      this.usersDeleteToolStripButton.Click += new System.EventHandler(this.usersDeleteToolStripButton_Click);
      // 
      // toolStripSeparator2
      // 
      this.toolStripSeparator2.Name = "toolStripSeparator2";
      resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
      // 
      // usersImportToolStripButton
      // 
      resources.ApplyResources(this.usersImportToolStripButton, "usersImportToolStripButton");
      this.usersImportToolStripButton.Image = global::TFSAdministrationTool.Properties.Resources.usersEditToolStripButton;
      this.usersImportToolStripButton.Name = "usersImportToolStripButton";
      this.usersImportToolStripButton.Click += new System.EventHandler(this.usersImportToolStripButton_Click);
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
      // 
      // usersRefreshStripButton
      // 
      resources.ApplyResources(this.usersRefreshStripButton, "usersRefreshStripButton");
      this.usersRefreshStripButton.Image = global::TFSAdministrationTool.Properties.Resources.usersRefreshStripButton;
      this.usersRefreshStripButton.Name = "usersRefreshStripButton";
      this.usersRefreshStripButton.Click += new System.EventHandler(this.usersRefreshStripButton_Click);
      // 
      // mainTabControl
      // 
      resources.ApplyResources(this.mainTabControl, "mainTabControl");
      this.mainTabControl.Controls.Add(this.pendingChangesTabPage);
      this.mainTabControl.Controls.Add(this.historyTabPage);
      this.mainTabControl.Controls.Add(this.outputTabPage);
      this.mainTabControl.ImageList = this.mainTabControlImageList;
      this.mainTabControl.Name = "mainTabControl";
      this.mainTabControl.SelectedIndex = 0;
      // 
      // pendingChangesTabPage
      // 
      this.pendingChangesTabPage.Controls.Add(this.pendingChangesListView);
      this.pendingChangesTabPage.Controls.Add(this.pendingChangesToolStrip);
      resources.ApplyResources(this.pendingChangesTabPage, "pendingChangesTabPage");
      this.pendingChangesTabPage.Name = "pendingChangesTabPage";
      this.pendingChangesTabPage.UseVisualStyleBackColor = true;
      // 
      // pendingChangesListView
      // 
      this.pendingChangesListView.CheckBoxes = true;
      this.pendingChangesListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.UserName,
            this.DisplayName,
            this.Change,
            this.Server,
            this.TeamProject,
            this.Tier,
            this.Role});
      resources.ApplyResources(this.pendingChangesListView, "pendingChangesListView");
      this.pendingChangesListView.FullRowSelect = true;
      this.pendingChangesListView.Name = "pendingChangesListView";
      this.pendingChangesListView.ShowGroups = false;
      this.pendingChangesListView.UseCompatibleStateImageBehavior = false;
      this.pendingChangesListView.View = System.Windows.Forms.View.Details;
      this.pendingChangesListView.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.lvPendingChanges_ItemChecked);
      this.pendingChangesListView.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lvPendingChanges_ItemSelectionChanged);
      this.pendingChangesListView.Leave += new System.EventHandler(this.pendingChangesListView_Leave);
      // 
      // UserName
      // 
      resources.ApplyResources(this.UserName, "UserName");
      // 
      // DisplayName
      // 
      resources.ApplyResources(this.DisplayName, "DisplayName");
      // 
      // Change
      // 
      resources.ApplyResources(this.Change, "Change");
      // 
      // Server
      // 
      resources.ApplyResources(this.Server, "Server");
      // 
      // TeamProject
      // 
      resources.ApplyResources(this.TeamProject, "TeamProject");
      // 
      // Tier
      // 
      resources.ApplyResources(this.Tier, "Tier");
      // 
      // Role
      // 
      resources.ApplyResources(this.Role, "Role");
      // 
      // pendingChangesToolStrip
      // 
      this.pendingChangesToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
      this.pendingChangesToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pendingChangesCommitToolStripButton,
            this.pendingChangesUndoToolStripButton});
      resources.ApplyResources(this.pendingChangesToolStrip, "pendingChangesToolStrip");
      this.pendingChangesToolStrip.Name = "pendingChangesToolStrip";
      // 
      // pendingChangesCommitToolStripButton
      // 
      resources.ApplyResources(this.pendingChangesCommitToolStripButton, "pendingChangesCommitToolStripButton");
      this.pendingChangesCommitToolStripButton.Image = global::TFSAdministrationTool.Properties.Resources.pendingChangesCommitToolStripButton;
      this.pendingChangesCommitToolStripButton.Name = "pendingChangesCommitToolStripButton";
      this.pendingChangesCommitToolStripButton.Click += new System.EventHandler(this.pendingChangesCommitToolStripButton_Click);
      // 
      // pendingChangesUndoToolStripButton
      // 
      resources.ApplyResources(this.pendingChangesUndoToolStripButton, "pendingChangesUndoToolStripButton");
      this.pendingChangesUndoToolStripButton.Image = global::TFSAdministrationTool.Properties.Resources.pendingChangesUndoToolStripButton;
      this.pendingChangesUndoToolStripButton.Name = "pendingChangesUndoToolStripButton";
      this.pendingChangesUndoToolStripButton.Click += new System.EventHandler(this.pendingChangesUndoToolStripButton_Click);
      // 
      // historyTabPage
      // 
      this.historyTabPage.Controls.Add(this.historyListView);
      this.historyTabPage.Controls.Add(this.historyToolStrip);
      resources.ApplyResources(this.historyTabPage, "historyTabPage");
      this.historyTabPage.Name = "historyTabPage";
      this.historyTabPage.UseVisualStyleBackColor = true;
      // 
      // historyListView
      // 
      this.historyListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader7,
            this.columnHeader8,
            this.columnHeader9,
            this.Date,
            this.columnHeader14,
            this.columnHeader10,
            this.columnHeader11,
            this.columnHeader12,
            this.columnHeader13});
      resources.ApplyResources(this.historyListView, "historyListView");
      this.historyListView.FullRowSelect = true;
      this.historyListView.Name = "historyListView";
      this.historyListView.ShowGroups = false;
      this.historyListView.SmallImageList = this.historyListViewImageList;
      this.historyListView.UseCompatibleStateImageBehavior = false;
      this.historyListView.View = System.Windows.Forms.View.Details;
      // 
      // columnHeader7
      // 
      resources.ApplyResources(this.columnHeader7, "columnHeader7");
      // 
      // columnHeader8
      // 
      resources.ApplyResources(this.columnHeader8, "columnHeader8");
      // 
      // columnHeader9
      // 
      resources.ApplyResources(this.columnHeader9, "columnHeader9");
      // 
      // Date
      // 
      resources.ApplyResources(this.Date, "Date");
      // 
      // columnHeader14
      // 
      resources.ApplyResources(this.columnHeader14, "columnHeader14");
      // 
      // columnHeader10
      // 
      resources.ApplyResources(this.columnHeader10, "columnHeader10");
      // 
      // columnHeader11
      // 
      resources.ApplyResources(this.columnHeader11, "columnHeader11");
      // 
      // columnHeader12
      // 
      resources.ApplyResources(this.columnHeader12, "columnHeader12");
      // 
      // columnHeader13
      // 
      resources.ApplyResources(this.columnHeader13, "columnHeader13");
      // 
      // historyListViewImageList
      // 
      this.historyListViewImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
      resources.ApplyResources(this.historyListViewImageList, "historyListViewImageList");
      this.historyListViewImageList.TransparentColor = System.Drawing.Color.Transparent;
      // 
      // historyToolStrip
      // 
      this.historyToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
      this.historyToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.historyClearToolStripButton});
      resources.ApplyResources(this.historyToolStrip, "historyToolStrip");
      this.historyToolStrip.Name = "historyToolStrip";
      // 
      // historyClearToolStripButton
      // 
      resources.ApplyResources(this.historyClearToolStripButton, "historyClearToolStripButton");
      this.historyClearToolStripButton.Image = global::TFSAdministrationTool.Properties.Resources.historyClearToolStripButton;
      this.historyClearToolStripButton.Name = "historyClearToolStripButton";
      this.historyClearToolStripButton.Click += new System.EventHandler(this.historyClearToolStripButton_Click);
      // 
      // outputTabPage
      // 
      this.outputTabPage.Controls.Add(this.outputTextBox);
      this.outputTabPage.Controls.Add(this.outputToolStrip);
      resources.ApplyResources(this.outputTabPage, "outputTabPage");
      this.outputTabPage.Name = "outputTabPage";
      this.outputTabPage.UseVisualStyleBackColor = true;
      // 
      // outputTextBox
      // 
      this.outputTextBox.BackColor = System.Drawing.Color.White;
      resources.ApplyResources(this.outputTextBox, "outputTextBox");
      this.outputTextBox.Name = "outputTextBox";
      this.outputTextBox.ReadOnly = true;
      // 
      // outputToolStrip
      // 
      this.outputToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
      this.outputToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.outputClearToolStripButton});
      resources.ApplyResources(this.outputToolStrip, "outputToolStrip");
      this.outputToolStrip.Name = "outputToolStrip";
      // 
      // outputClearToolStripButton
      // 
      this.outputClearToolStripButton.Image = global::TFSAdministrationTool.Properties.Resources.outputClearToolStripButton;
      resources.ApplyResources(this.outputClearToolStripButton, "outputClearToolStripButton");
      this.outputClearToolStripButton.Name = "outputClearToolStripButton";
      this.outputClearToolStripButton.Click += new System.EventHandler(this.outputClearToolStripButton_Click);
      // 
      // mainTabControlImageList
      // 
      this.mainTabControlImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
      resources.ApplyResources(this.mainTabControlImageList, "mainTabControlImageList");
      this.mainTabControlImageList.TransparentColor = System.Drawing.Color.Transparent;
      // 
      // logDataGridView
      // 
      resources.ApplyResources(this.logDataGridView, "logDataGridView");
      this.logDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.logDataGridView.Name = "logDataGridView";
      // 
      // mainStatusStrip
      // 
      this.mainStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel,
            this.toolStripProgressBar1});
      this.mainStatusStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
      resources.ApplyResources(this.mainStatusStrip, "mainStatusStrip");
      this.mainStatusStrip.Name = "mainStatusStrip";
      // 
      // toolStripStatusLabel
      // 
      this.toolStripStatusLabel.Name = "toolStripStatusLabel";
      resources.ApplyResources(this.toolStripStatusLabel, "toolStripStatusLabel");
      this.toolStripStatusLabel.Spring = true;
      // 
      // toolStripProgressBar1
      // 
      this.toolStripProgressBar1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
      this.toolStripProgressBar1.Name = "toolStripProgressBar1";
      resources.ApplyResources(this.toolStripProgressBar1, "toolStripProgressBar1");
      this.toolStripProgressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
      // 
      // toolStripButton1
      // 
      resources.ApplyResources(this.toolStripButton1, "toolStripButton1");
      this.toolStripButton1.Name = "toolStripButton1";
      // 
      // toolStripButton2
      // 
      resources.ApplyResources(this.toolStripButton2, "toolStripButton2");
      this.toolStripButton2.Name = "toolStripButton2";
      // 
      // columnHeader1
      // 
      resources.ApplyResources(this.columnHeader1, "columnHeader1");
      // 
      // columnHeader2
      // 
      resources.ApplyResources(this.columnHeader2, "columnHeader2");
      // 
      // columnHeader3
      // 
      resources.ApplyResources(this.columnHeader3, "columnHeader3");
      // 
      // columnHeader4
      // 
      resources.ApplyResources(this.columnHeader4, "columnHeader4");
      // 
      // columnHeader5
      // 
      resources.ApplyResources(this.columnHeader5, "columnHeader5");
      // 
      // columnHeader6
      // 
      resources.ApplyResources(this.columnHeader6, "columnHeader6");
      // 
      // dataGridViewImageColumn1
      // 
      this.dataGridViewImageColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
      this.dataGridViewImageColumn1.DataPropertyName = "State";
      this.dataGridViewImageColumn1.FillWeight = 24F;
      this.dataGridViewImageColumn1.Frozen = true;
      resources.ApplyResources(this.dataGridViewImageColumn1, "dataGridViewImageColumn1");
      this.dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
      this.dataGridViewImageColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
      // 
      // serverTreeView
      // 
      resources.ApplyResources(this.serverTreeView, "serverTreeView");
      this.serverTreeView.ImageList = this.serverTreeViewImageList;
      this.serverTreeView.Name = "serverTreeView";
      this.serverTreeView.DoubleClick += new System.EventHandler(this.serverTreeView_DoubleClick);
      // 
      // mainForm
      // 
      resources.ApplyResources(this, "$this");
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.splitContainer1);
      this.Name = "mainForm";
      this.TransparencyKey = System.Drawing.Color.Magenta;
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
      this.Load += new System.EventHandler(this.MainForm_Load);
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel2.ResumeLayout(false);
      this.splitContainer1.Panel2.PerformLayout();
      this.splitContainer1.ResumeLayout(false);
      this.splitContainer2.Panel1.ResumeLayout(false);
      this.splitContainer2.Panel1.PerformLayout();
      this.splitContainer2.Panel2.ResumeLayout(false);
      this.splitContainer2.Panel2.PerformLayout();
      this.splitContainer2.ResumeLayout(false);
      this.serverToolStrip.ResumeLayout(false);
      this.serverToolStrip.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.usersDataGridView)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.usersBindingSource)).EndInit();
      this.usersToolStrip.ResumeLayout(false);
      this.usersToolStrip.PerformLayout();
      this.mainTabControl.ResumeLayout(false);
      this.pendingChangesTabPage.ResumeLayout(false);
      this.pendingChangesTabPage.PerformLayout();
      this.pendingChangesToolStrip.ResumeLayout(false);
      this.pendingChangesToolStrip.PerformLayout();
      this.historyTabPage.ResumeLayout(false);
      this.historyTabPage.PerformLayout();
      this.historyToolStrip.ResumeLayout(false);
      this.historyToolStrip.PerformLayout();
      this.outputTabPage.ResumeLayout(false);
      this.outputTabPage.PerformLayout();
      this.outputToolStrip.ResumeLayout(false);
      this.outputToolStrip.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.logDataGridView)).EndInit();
      this.mainStatusStrip.ResumeLayout(false);
      this.mainStatusStrip.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.SplitContainer splitContainer1;
    private System.Windows.Forms.SplitContainer splitContainer2;
    private System.Windows.Forms.StatusStrip mainStatusStrip;
    private System.Windows.Forms.ToolStrip serverToolStrip;
    private System.Windows.Forms.ToolStrip usersToolStrip;
    private System.Windows.Forms.DataGridView logDataGridView;
    private System.Windows.Forms.ToolStripButton serverConnectToolStripButton;
    private System.Windows.Forms.ToolStripButton serverDisconnectToolStripButton;
    private System.Windows.Forms.ToolStripButton aboutToolStripButton;
    private System.Windows.Forms.ToolStripButton usersAddToolStripButton;
    private System.Windows.Forms.ToolStripButton usersEditToolStripButton;
    private System.Windows.Forms.ToolStripButton usersDeleteToolStripButton;
    private System.Windows.Forms.ToolStripButton usersRefreshStripButton;
    private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
    private System.Windows.Forms.TabControl mainTabControl;
    private System.Windows.Forms.TabPage pendingChangesTabPage;
    private System.Windows.Forms.ListView pendingChangesListView;
    private System.Windows.Forms.ColumnHeader UserName;
    private System.Windows.Forms.ColumnHeader DisplayName;
    private System.Windows.Forms.ColumnHeader Change;
    private System.Windows.Forms.ColumnHeader TeamProject;
    private System.Windows.Forms.ColumnHeader Tier;
    private System.Windows.Forms.ColumnHeader Role;
    private System.Windows.Forms.ToolStrip pendingChangesToolStrip;
    private System.Windows.Forms.ToolStripButton pendingChangesCommitToolStripButton;
    private System.Windows.Forms.ToolStripButton pendingChangesUndoToolStripButton;
    private System.Windows.Forms.TabPage historyTabPage;
    private System.Windows.Forms.ToolStripButton toolStripButton1;
    private System.Windows.Forms.ToolStripButton toolStripButton2;
    private System.Windows.Forms.ListView historyListView;
    private System.Windows.Forms.ColumnHeader columnHeader7;
    private System.Windows.Forms.ColumnHeader columnHeader8;
    private System.Windows.Forms.ColumnHeader columnHeader9;
    private System.Windows.Forms.ColumnHeader columnHeader10;
    private System.Windows.Forms.ColumnHeader columnHeader11;
    private System.Windows.Forms.ColumnHeader columnHeader12;
    private System.Windows.Forms.ColumnHeader columnHeader1;
    private System.Windows.Forms.ColumnHeader columnHeader2;
    private System.Windows.Forms.ColumnHeader columnHeader3;
    private System.Windows.Forms.ColumnHeader columnHeader4;
    private System.Windows.Forms.ColumnHeader columnHeader5;
    private System.Windows.Forms.ColumnHeader columnHeader6;
    private System.Windows.Forms.ColumnHeader Date;
    private System.Windows.Forms.ColumnHeader columnHeader13;
    private TFSAdminTreeView serverTreeView;
    private System.Windows.Forms.ColumnHeader Server;
    private System.Windows.Forms.ColumnHeader columnHeader14;
    private System.Windows.Forms.DataGridView usersDataGridView;
    private System.Windows.Forms.TabPage outputTabPage;
    private System.Windows.Forms.TextBox outputTextBox;
    private System.Windows.Forms.BindingSource usersBindingSource;
    private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
    private System.Windows.Forms.ToolStrip historyToolStrip;
    private System.Windows.Forms.ToolStripButton historyClearToolStripButton;
    private System.Windows.Forms.ToolStrip outputToolStrip;
    private System.Windows.Forms.ToolStripButton outputClearToolStripButton;
    private System.Windows.Forms.DataGridViewImageColumn State;
    private System.Windows.Forms.DataGridViewTextBoxColumn userNameDataGridViewTextBoxColumn;
    private System.Windows.Forms.DataGridViewTextBoxColumn displayNameDataGridViewTextBoxColumn;
    private System.Windows.Forms.DataGridViewTextBoxColumn TfsRoles;
    private System.Windows.Forms.DataGridViewTextBoxColumn SpRoles;
    private System.Windows.Forms.DataGridViewTextBoxColumn RsRoles;
    private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn1;
    private System.Windows.Forms.ImageList mainTabControlImageList;
    private System.Windows.Forms.ImageList serverTreeViewImageList;
    private System.Windows.Forms.ImageList historyListViewImageList;
    private System.Windows.Forms.ToolStripButton usersImportToolStripButton;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;

  }
}

