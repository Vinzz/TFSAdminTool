namespace TFSAdministrationTool
{
  partial class UserImport
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
      this.btnImport = new System.Windows.Forms.Button();
      this.btnCancel = new System.Windows.Forms.Button();
      this.lblServerName = new System.Windows.Forms.Label();
      this.cbTeamProject = new System.Windows.Forms.ComboBox();
      this.usersDataGridView = new System.Windows.Forms.DataGridView();
      this.Selected = new System.Windows.Forms.DataGridViewCheckBoxColumn();
      this.IdentyType = new System.Windows.Forms.DataGridViewImageColumn();
      this.displayNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.TfsRoles = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.SpRoles = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.RsRoles = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.usersBindingSource = new System.Windows.Forms.BindingSource(this.components);
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.lblTargetName = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this.usersDataGridView)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.usersBindingSource)).BeginInit();
      this.SuspendLayout();
      // 
      // btnImport
      // 
      this.btnImport.Enabled = false;
      this.btnImport.Location = new System.Drawing.Point(952, 349);
      this.btnImport.Name = "btnImport";
      this.btnImport.Size = new System.Drawing.Size(75, 23);
      this.btnImport.TabIndex = 0;
      this.btnImport.Text = "Import";
      this.btnImport.UseVisualStyleBackColor = true;
      this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
      // 
      // btnCancel
      // 
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Location = new System.Drawing.Point(871, 349);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 1;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
      // 
      // lblServerName
      // 
      this.lblServerName.AutoSize = true;
      this.lblServerName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblServerName.Location = new System.Drawing.Point(136, 9);
      this.lblServerName.Name = "lblServerName";
      this.lblServerName.Size = new System.Drawing.Size(76, 13);
      this.lblServerName.TabIndex = 2;
      this.lblServerName.Text = "ServerName";
      // 
      // cbTeamProject
      // 
      this.cbTeamProject.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cbTeamProject.FormattingEnabled = true;
      this.cbTeamProject.Location = new System.Drawing.Point(139, 25);
      this.cbTeamProject.Name = "cbTeamProject";
      this.cbTeamProject.Size = new System.Drawing.Size(151, 21);
      this.cbTeamProject.TabIndex = 3;
      this.cbTeamProject.SelectionChangeCommitted += new System.EventHandler(this.cbTeamProject_SelectionChangeCommitted);
      this.cbTeamProject.Enter += new System.EventHandler(this.cbTeamProject_Enter);
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
            this.Selected,
            this.IdentyType,
            this.displayNameDataGridViewTextBoxColumn,
            this.TfsRoles,
            this.SpRoles,
            this.RsRoles});
      this.usersDataGridView.DataSource = this.usersBindingSource;
      this.usersDataGridView.Location = new System.Drawing.Point(12, 69);
      this.usersDataGridView.Name = "usersDataGridView";
      this.usersDataGridView.ReadOnly = true;
      this.usersDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.usersDataGridView.Size = new System.Drawing.Size(1015, 274);
      this.usersDataGridView.TabIndex = 4;
      this.usersDataGridView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.usersDataGridView_CellFormatting);
      this.usersDataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.usersDataGridView_CellClick);
      // 
      // Selected
      // 
      this.Selected.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
      this.Selected.FalseValue = "False";
      this.Selected.FillWeight = 24F;
      this.Selected.Frozen = true;
      this.Selected.HeaderText = "";
      this.Selected.IndeterminateValue = "False";
      this.Selected.MinimumWidth = 24;
      this.Selected.Name = "Selected";
      this.Selected.ReadOnly = true;
      this.Selected.TrueValue = "True";
      this.Selected.Width = 24;
      // 
      // IdentyType
      // 
      this.IdentyType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
      this.IdentyType.DataPropertyName = "IdentityType";
      this.IdentyType.FillWeight = 24F;
      this.IdentyType.Frozen = true;
      this.IdentyType.HeaderText = "";
      this.IdentyType.MinimumWidth = 24;
      this.IdentyType.Name = "IdentyType";
      this.IdentyType.ReadOnly = true;
      this.IdentyType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
      this.IdentyType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
      this.IdentyType.Width = 24;
      // 
      // displayNameDataGridViewTextBoxColumn
      // 
      this.displayNameDataGridViewTextBoxColumn.DataPropertyName = "DisplayName";
      this.displayNameDataGridViewTextBoxColumn.HeaderText = "DisplayName";
      this.displayNameDataGridViewTextBoxColumn.Name = "displayNameDataGridViewTextBoxColumn";
      this.displayNameDataGridViewTextBoxColumn.ReadOnly = true;
      // 
      // TfsRoles
      // 
      this.TfsRoles.DataPropertyName = "TfsRolesString";
      this.TfsRoles.HeaderText = "Team Foundation Roles";
      this.TfsRoles.Name = "TfsRoles";
      this.TfsRoles.ReadOnly = true;
      // 
      // SpRoles
      // 
      this.SpRoles.DataPropertyName = "SpRolesString";
      this.SpRoles.HeaderText = "SharePoint Roles";
      this.SpRoles.Name = "SpRoles";
      this.SpRoles.ReadOnly = true;
      // 
      // RsRoles
      // 
      this.RsRoles.DataPropertyName = "RsRolesString";
      this.RsRoles.HeaderText = "Reporting Services Roles";
      this.RsRoles.Name = "RsRoles";
      this.RsRoles.ReadOnly = true;
      // 
      // usersBindingSource
      // 
      this.usersBindingSource.DataSource = typeof(TFSAdministrationTool.Proxy.Common.TfsUser);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(12, 9);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(35, 13);
      this.label1.TabIndex = 2;
      this.label1.Text = "label1";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(12, 29);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(110, 13);
      this.label2.TabIndex = 2;
      this.label2.Text = "Source Team Project:";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(12, 9);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(127, 13);
      this.label3.TabIndex = 2;
      this.label3.Text = "Team Foundation Server:";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(12, 49);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(107, 13);
      this.label4.TabIndex = 5;
      this.label4.Text = "Target Team Project:";
      // 
      // lblTargetName
      // 
      this.lblTargetName.AutoSize = true;
      this.lblTargetName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblTargetName.Location = new System.Drawing.Point(136, 49);
      this.lblTargetName.Name = "lblTargetName";
      this.lblTargetName.Size = new System.Drawing.Size(118, 13);
      this.lblTargetName.TabIndex = 6;
      this.lblTargetName.Text = "Team Project Name";
      // 
      // UserImport
      // 
      this.AcceptButton = this.btnImport;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btnCancel;
      this.ClientSize = new System.Drawing.Size(1043, 384);
      this.Controls.Add(this.lblTargetName);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.usersDataGridView);
      this.Controls.Add(this.cbTeamProject);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.lblServerName);
      this.Controls.Add(this.btnCancel);
      this.Controls.Add(this.btnImport);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "UserImport";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Import User(s)";
      this.Load += new System.EventHandler(this.UserImport_Load);
      ((System.ComponentModel.ISupportInitialize)(this.usersDataGridView)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.usersBindingSource)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button btnImport;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Label lblServerName;
    private System.Windows.Forms.ComboBox cbTeamProject;
    private System.Windows.Forms.DataGridView usersDataGridView;
    private System.Windows.Forms.BindingSource usersBindingSource;
    private System.Windows.Forms.DataGridViewCheckBoxColumn Selected;
    private System.Windows.Forms.DataGridViewImageColumn IdentyType;
    private System.Windows.Forms.DataGridViewTextBoxColumn displayNameDataGridViewTextBoxColumn;
    private System.Windows.Forms.DataGridViewTextBoxColumn TfsRoles;
    private System.Windows.Forms.DataGridViewTextBoxColumn SpRoles;
    private System.Windows.Forms.DataGridViewTextBoxColumn RsRoles;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label lblTargetName;
  }
}