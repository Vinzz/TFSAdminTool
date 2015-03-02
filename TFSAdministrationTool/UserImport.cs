using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using TFSAdministrationTool.Proxy.Common;
using TFSAdministrationTool.Properties;
using TFSAdministrationTool.Controllers;
using Microsoft.TeamFoundation.Server;

namespace TFSAdministrationTool
{
  public partial class UserImport : Form
  {
    #region Variables
    private Icon userIcon = new Icon(Resources.User, new Size(16, 16));
    private Icon groupIcon = new Icon(Resources.Group, new Size(16, 16));
    private Font userExistsFont = null;

    private int selectedUsersCount = 0;
    private string selectedTeamProject = "";
    #endregion

    public UserImport()
    {
      InitializeComponent();

      UserController.Initialize(MainController.CurrentServer, null);
    }

    #region Event handlers

    private void UserImport_Load(object sender, EventArgs e)
    {
      /// Initialize the labels
      lblServerName.Text = MainController.CurrentServer.Server.Name;
      lblTargetName.Text = MainController.CurrentServer.SelectedTeamProject;

      // Initialize the dropdown list
      cbTeamProject.Items.Add("Select a team project");
      foreach (ProjectInfo pi in MainController.CurrentServer.TeamProjects)
      {
        if (pi.Name != MainController.CurrentServer.SelectedTeamProject)
        {
          cbTeamProject.Items.Add(pi.Name);
        }
      }      
      cbTeamProject.SelectedIndex = 0;
    }

    private void cbTeamProject_Enter(object sender, EventArgs e)
    {
      if (selectedTeamProject == "")
        selectedTeamProject = (string)cbTeamProject.Items[cbTeamProject.SelectedIndex];
    }

    private void cbTeamProject_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.Cursor = Cursors.WaitCursor;

      if (cbTeamProject.SelectedIndex > -1)
      {
        if (selectedUsersCount > 0)
        {
          if (MessageBox.Show(this, Resources.UserImportSelectionExistsChangesPrompt, Resources.UserImportSelectionExistsChangesCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
          {
            this.Cursor = Cursors.Default;
            cbTeamProject.SelectedIndex = cbTeamProject.Items.IndexOf(selectedTeamProject);
            return;
          }
        }

        selectedTeamProject = (string)cbTeamProject.Items[cbTeamProject.SelectedIndex];
        selectedUsersCount = 0;
        btnImport.Enabled = false;

        if (cbTeamProject.SelectedIndex > 0)
        {
          foreach (ProjectInfo pi in MainController.CurrentServer.TeamProjects)
          {
            if (pi.Name == selectedTeamProject)
            {
              TfsUserCollection users = MainController.CurrentServer.GetTeamProjectUsers(pi.Name);
              usersBindingSource.DataSource = users.Users;
              usersBindingSource.CurrencyManager.Refresh();
              usersDataGridView.ClearSelection();
              break;
            }
          }
        }
        else
        {
          usersBindingSource.DataSource = null;
          usersBindingSource.CurrencyManager.Refresh();
          usersDataGridView.ClearSelection();
        }
      }
      this.Cursor = Cursors.Default;      
    }

    private void btnImport_Click(object sender, EventArgs e)
    {
      try
      {
        TfsAdminToolTracer.TraceMessage(TfsAdminToolTracer.TraceSwitch.TraceInfo, "Button clicked: User Import IMPORT");

        UserController.TeamProject = selectedTeamProject;
        
        foreach (DataGridViewRow row in usersDataGridView.Rows)
        {
          if ((string)row.Cells[0].Value == "True")
          {
            UserController.ImportUser((TfsUser)row.DataBoundItem);
          }
        }

        DialogResult = DialogResult.OK;
        Close();
      }
      catch (Exception ex)
      {
        TfsAdminToolTracer.TraceException(TfsAdminToolTracer.TraceSwitch.TraceError, ex);
        MessageBox.Show(this, Resources.UnexpectedExceptionPrompt, Resources.UnexpectedExceptionCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      try
      {
        TfsAdminToolTracer.TraceMessage(TfsAdminToolTracer.TraceSwitch.TraceInfo, "Button clicked: User Import CANCEL");
        DialogResult = DialogResult.Cancel;
        Close();
      }
      catch (Exception ex)
      {
        TfsAdminToolTracer.TraceException(TfsAdminToolTracer.TraceSwitch.TraceError, ex);
        MessageBox.Show(this, Resources.UnexpectedExceptionPrompt, Resources.UnexpectedExceptionCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void usersDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
      if (e.ColumnIndex == 1)
      {
        switch (((IdentityType)e.Value))
        {
          case IdentityType.WindowsUser:
            e.Value = userIcon;
            break;
          case IdentityType.WindowsGroup:
            e.Value = groupIcon;
            break;
        }
      }
      else
      {
        string displayName = (string)usersDataGridView.Rows[e.RowIndex].Cells[2].Value;
        if (MainController.CurrentServer.UserCollection.Users.Exists(delegate(TfsUser u) { return string.Compare(u.DisplayName, displayName, true) == 0; }))
        {
          if (userExistsFont == null)
            userExistsFont = new Font(e.CellStyle.Font.FontFamily, e.CellStyle.Font.Size, FontStyle.Italic);

          e.CellStyle.Font = userExistsFont;
          e.CellStyle.ForeColor = Color.Salmon;
        }
      }
    }

    private void usersDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
    {
      if (e.RowIndex > -1 && e.ColumnIndex > -1)
      {
        string selectionState = (string)usersDataGridView.Rows[e.RowIndex].Cells[0].Value;
        string displayName = (string)usersDataGridView.Rows[e.RowIndex].Cells[2].Value;

        if (selectionState == "False" || String.IsNullOrEmpty(selectionState))
        {
          if (MainController.CurrentServer.UserCollection.Users.Exists(delegate(TfsUser u) { return string.Compare(u.DisplayName, displayName, true) == 0; }))
          {
            MessageBox.Show(this, String.Format(Resources.UserImportExistsChangesPrompt, displayName, MainController.CurrentServer.SelectedTeamProject), Resources.UserImportExistsChangesCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
          }
          selectedUsersCount++;
          usersDataGridView.Rows[e.RowIndex].Cells[0].Value = "True";
        }
        else
        {
          selectedUsersCount--;
          usersDataGridView.Rows[e.RowIndex].Cells[0].Value = "False";
        }
        btnImport.Enabled = selectedUsersCount > 0;
      }
    }

    #endregion
  }
}