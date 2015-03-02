using TFSAdministrationTool.Controls;

namespace TFSAdministrationTool
{
    partial class UserConfiguration
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
          System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserConfiguration));
          this.groupUserInfo = new System.Windows.Forms.GroupBox();
          this.btnBrowse = new System.Windows.Forms.Button();
          this.btnAdd = new System.Windows.Forms.Button();
          this.tbQuickAddUsers = new System.Windows.Forms.TextBox();
          this.lvwUsers = new System.Windows.Forms.ListView();
          this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
          this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
          this.groupRoleConfig = new System.Windows.Forms.GroupBox();
          this.label3 = new System.Windows.Forms.Label();
          this.label2 = new System.Windows.Forms.Label();
          this.cbToggleTeamFoundationRoles = new System.Windows.Forms.CheckBox();
          this.cbToggleReportingServicesRoles = new System.Windows.Forms.CheckBox();
          this.cbToggleSharepointRoles = new System.Windows.Forms.CheckBox();
          this.label1 = new System.Windows.Forms.Label();
          this.clbReportingServicesRoles = new TFSAdministrationTool.Controls.TFSAdminCheckedListBox();
          this.clbSharepointRoles = new TFSAdministrationTool.Controls.TFSAdminCheckedListBox();
          this.clbTeamFoundationRoles = new TFSAdministrationTool.Controls.TFSAdminCheckedListBox();
          this.statusStrip1 = new System.Windows.Forms.StatusStrip();
          this.toolStripStatusMessage = new System.Windows.Forms.ToolStripStatusLabel();
          this.btnOK = new System.Windows.Forms.Button();
          this.bntCancel = new System.Windows.Forms.Button();
          this.groupUserInfo.SuspendLayout();
          this.groupRoleConfig.SuspendLayout();
          this.statusStrip1.SuspendLayout();
          this.SuspendLayout();
          // 
          // groupUserInfo
          // 
          this.groupUserInfo.Controls.Add(this.btnBrowse);
          this.groupUserInfo.Controls.Add(this.btnAdd);
          this.groupUserInfo.Controls.Add(this.tbQuickAddUsers);
          this.groupUserInfo.Controls.Add(this.lvwUsers);
          resources.ApplyResources(this.groupUserInfo, "groupUserInfo");
          this.groupUserInfo.Name = "groupUserInfo";
          this.groupUserInfo.TabStop = false;
          // 
          // btnBrowse
          // 
          resources.ApplyResources(this.btnBrowse, "btnBrowse");
          this.btnBrowse.Name = "btnBrowse";
          this.btnBrowse.UseVisualStyleBackColor = true;
          this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
          // 
          // btnAdd
          // 
          resources.ApplyResources(this.btnAdd, "btnAdd");
          this.btnAdd.Name = "btnAdd";
          this.btnAdd.UseVisualStyleBackColor = true;
          this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
          // 
          // tbQuickAddUsers
          // 
          resources.ApplyResources(this.tbQuickAddUsers, "tbQuickAddUsers");
          this.tbQuickAddUsers.Name = "tbQuickAddUsers";
          this.tbQuickAddUsers.TextChanged += new System.EventHandler(this.tbQuickAddUsers_TextChanged);
          this.tbQuickAddUsers.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbQuickAddUsers_KeyDown);
          // 
          // lvwUsers
          // 
          this.lvwUsers.Activation = System.Windows.Forms.ItemActivation.TwoClick;
          this.lvwUsers.BackColor = System.Drawing.SystemColors.Window;
          this.lvwUsers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
          this.lvwUsers.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
          resources.ApplyResources(this.lvwUsers, "lvwUsers");
          this.lvwUsers.MultiSelect = false;
          this.lvwUsers.Name = "lvwUsers";
          this.lvwUsers.OwnerDraw = true;
          this.lvwUsers.TabStop = false;
          this.lvwUsers.UseCompatibleStateImageBehavior = false;
          this.lvwUsers.View = System.Windows.Forms.View.Details;
          this.lvwUsers.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.lvwUsers_DrawColumnHeader);
          this.lvwUsers.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.lvwUsers_DrawSubItem);
          // 
          // columnHeader1
          // 
          resources.ApplyResources(this.columnHeader1, "columnHeader1");
          // 
          // columnHeader2
          // 
          resources.ApplyResources(this.columnHeader2, "columnHeader2");
          // 
          // groupRoleConfig
          // 
          this.groupRoleConfig.Controls.Add(this.label3);
          this.groupRoleConfig.Controls.Add(this.label2);
          this.groupRoleConfig.Controls.Add(this.cbToggleTeamFoundationRoles);
          this.groupRoleConfig.Controls.Add(this.cbToggleReportingServicesRoles);
          this.groupRoleConfig.Controls.Add(this.cbToggleSharepointRoles);
          this.groupRoleConfig.Controls.Add(this.label1);
          this.groupRoleConfig.Controls.Add(this.clbReportingServicesRoles);
          this.groupRoleConfig.Controls.Add(this.clbSharepointRoles);
          this.groupRoleConfig.Controls.Add(this.clbTeamFoundationRoles);
          resources.ApplyResources(this.groupRoleConfig, "groupRoleConfig");
          this.groupRoleConfig.Name = "groupRoleConfig";
          this.groupRoleConfig.TabStop = false;
          // 
          // label3
          // 
          resources.ApplyResources(this.label3, "label3");
          this.label3.Name = "label3";
          // 
          // label2
          // 
          resources.ApplyResources(this.label2, "label2");
          this.label2.Name = "label2";
          // 
          // cbToggleTeamFoundationRoles
          // 
          resources.ApplyResources(this.cbToggleTeamFoundationRoles, "cbToggleTeamFoundationRoles");
          this.cbToggleTeamFoundationRoles.Name = "cbToggleTeamFoundationRoles";
          this.cbToggleTeamFoundationRoles.UseVisualStyleBackColor = true;
          this.cbToggleTeamFoundationRoles.CheckedChanged += new System.EventHandler(this.cbToggleTeamFoundationRoles_CheckedChanged);
          // 
          // cbToggleReportingServicesRoles
          // 
          resources.ApplyResources(this.cbToggleReportingServicesRoles, "cbToggleReportingServicesRoles");
          this.cbToggleReportingServicesRoles.Name = "cbToggleReportingServicesRoles";
          this.cbToggleReportingServicesRoles.UseVisualStyleBackColor = true;
          this.cbToggleReportingServicesRoles.CheckedChanged += new System.EventHandler(this.cbToggleReportingServicesRoles_CheckedChanged);
          // 
          // cbToggleSharepointRoles
          // 
          resources.ApplyResources(this.cbToggleSharepointRoles, "cbToggleSharepointRoles");
          this.cbToggleSharepointRoles.Name = "cbToggleSharepointRoles";
          this.cbToggleSharepointRoles.UseVisualStyleBackColor = true;
          this.cbToggleSharepointRoles.CheckedChanged += new System.EventHandler(this.cbToggleSharepointRoles_CheckedChanged);
          // 
          // label1
          // 
          resources.ApplyResources(this.label1, "label1");
          this.label1.Name = "label1";
          // 
          // clbReportingServicesRoles
          // 
          this.clbReportingServicesRoles.CheckOnClick = true;
          this.clbReportingServicesRoles.FormattingEnabled = true;
          resources.ApplyResources(this.clbReportingServicesRoles, "clbReportingServicesRoles");
          this.clbReportingServicesRoles.Name = "clbReportingServicesRoles";
          this.clbReportingServicesRoles.SelectedIndexChanged += new System.EventHandler(this.clbSharepointRoles_SelectedIndexChanged);
          // 
          // clbSharepointRoles
          // 
          this.clbSharepointRoles.CheckOnClick = true;
          this.clbSharepointRoles.FormattingEnabled = true;
          resources.ApplyResources(this.clbSharepointRoles, "clbSharepointRoles");
          this.clbSharepointRoles.Name = "clbSharepointRoles";
          this.clbSharepointRoles.SelectedIndexChanged += new System.EventHandler(this.clbSharepointRoles_SelectedIndexChanged);
          // 
          // clbTeamFoundationRoles
          // 
          this.clbTeamFoundationRoles.CheckOnClick = true;
          this.clbTeamFoundationRoles.FormattingEnabled = true;
          resources.ApplyResources(this.clbTeamFoundationRoles, "clbTeamFoundationRoles");
          this.clbTeamFoundationRoles.Name = "clbTeamFoundationRoles";
          this.clbTeamFoundationRoles.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.clbTeamFoundationRoles_ItemCheck);
          this.clbTeamFoundationRoles.SelectedIndexChanged += new System.EventHandler(this.clbTeamFoundationRoles_SelectedIndexChanged);
          // 
          // statusStrip1
          // 
          this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusMessage});
          resources.ApplyResources(this.statusStrip1, "statusStrip1");
          this.statusStrip1.Name = "statusStrip1";
          this.statusStrip1.SizingGrip = false;
          // 
          // toolStripStatusMessage
          // 
          this.toolStripStatusMessage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
          this.toolStripStatusMessage.Name = "toolStripStatusMessage";
          resources.ApplyResources(this.toolStripStatusMessage, "toolStripStatusMessage");
          // 
          // btnOK
          // 
          resources.ApplyResources(this.btnOK, "btnOK");
          this.btnOK.Name = "btnOK";
          this.btnOK.UseVisualStyleBackColor = true;
          this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
          // 
          // bntCancel
          // 
          this.bntCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
          resources.ApplyResources(this.bntCancel, "bntCancel");
          this.bntCancel.Name = "bntCancel";
          this.bntCancel.UseVisualStyleBackColor = true;
          this.bntCancel.Click += new System.EventHandler(this.bntCancel_Click);
          // 
          // UserConfiguration
          // 
          resources.ApplyResources(this, "$this");
          this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
          this.BackColor = System.Drawing.SystemColors.Control;
          this.CancelButton = this.bntCancel;
          this.Controls.Add(this.bntCancel);
          this.Controls.Add(this.btnOK);
          this.Controls.Add(this.statusStrip1);
          this.Controls.Add(this.groupRoleConfig);
          this.Controls.Add(this.groupUserInfo);
          this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
          this.MaximizeBox = false;
          this.MinimizeBox = false;
          this.Name = "UserConfiguration";
          this.ShowInTaskbar = false;
          this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.UserConfiguration_FormClosing);
          this.Load += new System.EventHandler(this.UserConfiguration_Load);
          this.groupUserInfo.ResumeLayout(false);
          this.groupUserInfo.PerformLayout();
          this.groupRoleConfig.ResumeLayout(false);
          this.groupRoleConfig.PerformLayout();
          this.statusStrip1.ResumeLayout(false);
          this.statusStrip1.PerformLayout();
          this.ResumeLayout(false);
          this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupUserInfo;
        private System.Windows.Forms.ListView lvwUsers;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.GroupBox groupRoleConfig;
        private TFSAdminCheckedListBox clbSharepointRoles;
        private TFSAdminCheckedListBox clbTeamFoundationRoles;
        private System.Windows.Forms.CheckBox cbToggleTeamFoundationRoles;
        private System.Windows.Forms.CheckBox cbToggleReportingServicesRoles;
        private System.Windows.Forms.CheckBox cbToggleSharepointRoles;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private TFSAdminCheckedListBox clbReportingServicesRoles;
        private System.Windows.Forms.TextBox tbQuickAddUsers;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusMessage;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button bntCancel;
    }
}