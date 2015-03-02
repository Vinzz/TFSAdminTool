using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Collections.ObjectModel;

using TFSAdministrationTool.Proxy;
using TFSAdministrationTool.Proxy.Common;
using TFSAdministrationTool.Properties;
using TFSAdministrationTool.Controls;
using TFSAdministrationTool.Controllers;

namespace TFSAdministrationTool
{
  public partial class UserConfiguration : Form
  {
    private RoleInfoCollection tfsRoles;
    private RoleInfoCollection spRoles;
    private RoleInfoCollection rsRoles;

    private bool IsDirty { get; set; }
    private bool IsEditing { get; set; }
    private bool PreventToggle { get; set; }

    #region constructors

    public UserConfiguration(ITeamFoundationServerProxy proxy)
    {
      InitializeComponent();

      UserController.TeamProject = proxy.SelectedTeamProject;            
      UserController.Initialize(proxy, null);

      IsDirty = false;
      IsEditing = false;
      PreventToggle = false;
    }

    public UserConfiguration(ITeamFoundationServerProxy proxy, TfsUserCollection selectedUsers)
    {
      InitializeComponent();

      UserController.TeamProject = proxy.SelectedTeamProject;            
      UserController.Initialize(proxy, selectedUsers);

      IsDirty = false;
      IsEditing = true;
      PreventToggle = false;
    }

    #endregion

    #region "helpers"

    private void StartTask(string task)
    {
      toolStripStatusMessage.Text = task;
      this.Cursor = Cursors.WaitCursor;
      Application.DoEvents();
    }

    private void EndTask()
    {
      toolStripStatusMessage.Text = Resources.TaskDoneStatusPrompt;
      this.Cursor = Cursors.Default;
    }

    #endregion

    private void RefreshContents()
    {
      lvwUsers.Items.Clear();

      foreach (TfsUser u in UserController.UserCollection.Users)
      {
        lvwUsers.Items.Add(u.UserName).SubItems.Add(u.DisplayName);
      }

      if (lvwUsers.Items.Count > 0)
        groupRoleConfig.Enabled = true;
      else
        groupRoleConfig.Enabled = false;

      InitializeRoleMappings(clbTeamFoundationRoles, SystemTier.TeamFoundation);
      InitializeRoleMappings(clbSharepointRoles, SystemTier.SharePoint);
      InitializeRoleMappings(clbReportingServicesRoles, SystemTier.ReportingServices);
    }

    private void InitializeRoleMappings(CheckedListBox clb, SystemTier tier)
    {
      RoleInfoCollection fullyMatchedRoles = null;
      RoleInfoCollection partiallyMatchedRoles = null;

      clb.ClearSelected();

      switch (tier)
      {
        case SystemTier.TeamFoundation:
          fullyMatchedRoles = GetMatchingRoles(tfsRoles, SystemTier.TeamFoundation, false);
          partiallyMatchedRoles = GetMatchingRoles(tfsRoles, SystemTier.TeamFoundation, true);
          break;
        case SystemTier.SharePoint:
          fullyMatchedRoles = GetMatchingRoles(spRoles, SystemTier.SharePoint, false);
          partiallyMatchedRoles = GetMatchingRoles(spRoles, SystemTier.SharePoint, true);
          break;
        case SystemTier.ReportingServices:
          fullyMatchedRoles = GetMatchingRoles(rsRoles, SystemTier.ReportingServices, false);
          partiallyMatchedRoles = GetMatchingRoles(rsRoles, SystemTier.ReportingServices, true);
          break;
      }

      foreach (RoleInfo role in fullyMatchedRoles.All)
      {
        int index = clb.Items.IndexOf(role.Name);
        if (!clb.GetItemChecked(index))
          clb.SetItemChecked(index, true);
      }

      foreach (RoleInfo role in partiallyMatchedRoles.All)
      {
        int index = clb.Items.IndexOf(role.Name);
        if (!clb.GetItemChecked(index))
          clb.SetItemCheckState(index, CheckState.Indeterminate);
      }
    }

    #region User related helpers

    private void ProcessAddUserRequest(TfsUser user)
    {
      try
      {
        if (UserController.AddUser(user, true))
        {
          IsDirty = true;
        }
      }
      catch (UserAlreadyExistException uae)
      {
        if (MessageBox.Show(uae.Message, Resources.UserAlreadyExistCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
        {
          UserController.AddUser(user, false);
        }
      }
      catch (UserHasPendingChangesException uhpc)
      {
        MessageBox.Show(uhpc.Message, Resources.UserHasPendingChangesCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void ProcessQuickAddUsers()
    {
      StartTask(Resources.UserResolvmentStatusPrompt);
      
      TfsUserCollection users = UserController.ResolveUsers(tbQuickAddUsers.Text.Split(';'));
      foreach (TfsUser user in users.Users)
      {
        ProcessAddUserRequest(user);
      }

      RefreshContents();

      tbQuickAddUsers.Text = "";

      EndTask();
    }

    #endregion

    #region Role related helpers

    private RoleInfoCollection GetMatchingRoles(RoleInfoCollection AvailableRoles, SystemTier Tier, bool AllowPartialMatch)
    {
      RoleInfoCollection matchedRoles = new RoleInfoCollection();

      if (UserController.UserCollection.Users.Count == 0)
        return matchedRoles;

      foreach (RoleInfo role in AvailableRoles.All)
      {
        bool roleMatchedAllUsers = true;
        bool roleFoundMatch = false;

        foreach (TfsUser user in UserController.UserCollection.Users)
        {
          if (!user.GetRolesBySystem(Tier).Contains(role.Name))
          {
            roleMatchedAllUsers = false;
            if (!AllowPartialMatch)
              break;
          }
          else
          {
            roleFoundMatch = true;
            if (AllowPartialMatch)
              break;
          }
        }

        if (AllowPartialMatch && roleFoundMatch)
          matchedRoles.Add(role.Name);
        else
        {
          if (roleMatchedAllUsers)
            matchedRoles.Add(role.Name);
        }
      }

      return matchedRoles;
    }

    private void SyncSelectedRolesWithUser(TfsUser user, CheckedListBox clb, SystemTier tier)
    {
      for (int index = 0; index < clb.Items.Count; index++)
      {
        string roleName = clb.Items[index].ToString();

        if (clb.GetItemCheckState(index) == CheckState.Checked)
        {
          user.AddRole(roleName, tier);
        }
        else if (clb.GetItemCheckState(index) == CheckState.Unchecked)
        {
          user.RemoveRole(roleName, tier);
        }
      }
    }

    private void ToggleMappedRoles(string role, CheckState state)
    {
      RoleInfoCollection spMappedRoles = UserController.GetMappedRolesBySystem(role, SystemTier.SharePoint);
      if (spMappedRoles.All.Count == 0)
      {
        // Warning
        TfsAdminToolTracer.TraceMessage(TfsAdminToolTracer.TraceSwitch.TraceInfo, "WARNING: No SharePoint roles for TeamFoundation role: " + role);
      }
      else
      {
        foreach (RoleInfo spMappedRole in spMappedRoles.All)
        {
          int index = clbSharepointRoles.Items.IndexOf(spMappedRole.Name);

          if (index == -1)
          {
            // Warning
            TfsAdminToolTracer.TraceMessage(TfsAdminToolTracer.TraceSwitch.TraceInfo, "WARNING: Cannot find SharePoint role: " + spMappedRole.Name);
          }
          else
          {
            if (clbSharepointRoles.GetItemCheckState(index) != state)
            {
              clbSharepointRoles.SetItemCheckState(index, state);

              // Trace messages
              if (state == CheckState.Checked)
              {
                TfsAdminToolTracer.TraceMessage(TfsAdminToolTracer.TraceSwitch.TraceInfo, "Toggle ON SharePoint role: " + spMappedRole.Name);
              }
              else
              {
                TfsAdminToolTracer.TraceMessage(TfsAdminToolTracer.TraceSwitch.TraceInfo, "Toggle OFF SharePoint role: " + spMappedRole.Name);
              }
            }
          }
        }
      }
            
      RoleInfoCollection rsMappedRoles = UserController.GetMappedRolesBySystem(role, SystemTier.ReportingServices);
      if (rsMappedRoles.All.Count == 0)
      {
        // Warning
        TfsAdminToolTracer.TraceMessage(TfsAdminToolTracer.TraceSwitch.TraceInfo, "WARNING: No ReportingServices roles for TeamFoundation role: " + role);
      }
      else
      {
        foreach (RoleInfo rsMappedRole in rsMappedRoles.All)
        {
          int index = clbReportingServicesRoles.Items.IndexOf(rsMappedRole.Name);

          if (index == -1)
          {
            // Warning
            TfsAdminToolTracer.TraceMessage(TfsAdminToolTracer.TraceSwitch.TraceInfo, "WARNING: Cannot find ReportingServices role: " + rsMappedRole.Name);
          }
          else
          {
            if (clbReportingServicesRoles.GetItemCheckState(index) != state)
            {
              clbReportingServicesRoles.SetItemCheckState(index, state);

              // Trace messages
              if (state == CheckState.Checked)
              {
                TfsAdminToolTracer.TraceMessage(TfsAdminToolTracer.TraceSwitch.TraceInfo, "Toggle ON ReportingServices role: " + rsMappedRole.Name);
              }
              else
              {
                TfsAdminToolTracer.TraceMessage(TfsAdminToolTracer.TraceSwitch.TraceInfo, "Toggle OFF ReportingServices role: " + rsMappedRole.Name);
              }
            }
          }
        }
      }      
    }

    private bool ValidateRoles(string tierRole, SystemTier tier)
    {
      string tfsRoles = String.Empty;
      RoleInfoCollection requiredRoles = null;

      foreach (string role in clbTeamFoundationRoles.CheckedItems)
        tfsRoles += role + ";";

      switch (tier)
      {
        case SystemTier.SharePoint:
          requiredRoles = UserController.GetMappedRolesBySystem(tfsRoles, SystemTier.SharePoint);
          if (requiredRoles.Contains(tierRole))
          {
            int index = clbSharepointRoles.Items.IndexOf(tierRole);
            // A required role is not checked so we need to highlight the item
            if (clbSharepointRoles.GetItemCheckState(index) != CheckState.Checked)
            {
              return false;
            }
          }
          break;
        case SystemTier.ReportingServices:
          requiredRoles = UserController.GetMappedRolesBySystem(tfsRoles, SystemTier.ReportingServices);
          if (requiredRoles.Contains(tierRole))
          {
            int index = clbReportingServicesRoles.Items.IndexOf(tierRole);
            // A required role is not checked so we need to highlight the item
            if (clbReportingServicesRoles.GetItemCheckState(index) != CheckState.Checked)
            {
              return false;
            }
          }
          break;
        default:
          // TODO: Log a warning as TeamFoundation tier should never be passed to this method
          break;
      }

      return true;
    }

    #endregion

    #region CheckedListBox helpers

    private void SyncToggleWithListSelection(CheckedListBox clb, CheckBox cb)
    {
      bool areAllItemsChecked = true;

      for (int index = 0; index < clb.Items.Count; index++)
      {
        areAllItemsChecked = clb.GetItemChecked(index);

        if (!areAllItemsChecked)
          break;
      }

      PreventToggle = true;
      cb.Checked = areAllItemsChecked;
      PreventToggle = false;
    }

    private void ToggleListSelection(CheckedListBox clb, CheckBox cb)
    {
      for (int index = 0; index < clb.Items.Count; index++)
      {
        clb.SetItemChecked(index, cb.Checked);
      }
    }

    #endregion

    #region Event handlers

    private void clbTeamFoundationRoles_ValidateItem(object sender, ValidateItemEventArgs e)
    {
      try
      {         
        //TODO we need to figure out how to do this better prior to v.Next
        if (clbTeamFoundationRoles.Items[e.Index].ToString() == "Service Accounts")
          e.IsDisabled = true;
      }
      catch (Exception ex)
      {
        TfsAdminToolTracer.TraceException(TfsAdminToolTracer.TraceSwitch.TraceError, ex);
        MessageBox.Show(this, Resources.UnexpectedExceptionPrompt, Resources.UnexpectedExceptionCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void clbReportingServicesRoles_ValidateItem(object sender, ValidateItemEventArgs e)
    {
      try
      {
        e.IsValid = ValidateRoles(clbReportingServicesRoles.Items[e.Index].ToString(), SystemTier.ReportingServices);
      }
      catch (Exception ex)
      {
        TfsAdminToolTracer.TraceException(TfsAdminToolTracer.TraceSwitch.TraceError, ex);
        MessageBox.Show(this, Resources.UnexpectedExceptionPrompt, Resources.UnexpectedExceptionCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void clbSharepointRoles_ValidateItem(object sender, ValidateItemEventArgs e)
    {
      try
      {
        e.IsValid = ValidateRoles(clbSharepointRoles.Items[e.Index].ToString(), SystemTier.SharePoint);
      }
      catch (Exception ex)
      {
        TfsAdminToolTracer.TraceException(TfsAdminToolTracer.TraceSwitch.TraceError, ex);
        MessageBox.Show(this, Resources.UnexpectedExceptionPrompt, Resources.UnexpectedExceptionCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void UserConfiguration_Load(object sender, EventArgs e)
    {
      try
      {
        clbTeamFoundationRoles.ValidateItem += new EventHandler<ValidateItemEventArgs>(clbTeamFoundationRoles_ValidateItem);
        clbReportingServicesRoles.ValidateItem += new EventHandler<ValidateItemEventArgs>(clbReportingServicesRoles_ValidateItem);
        clbSharepointRoles.ValidateItem += new EventHandler<ValidateItemEventArgs>(clbSharepointRoles_ValidateItem);

        tfsRoles = UserController.GetAvailableRolesBySystem(SystemTier.TeamFoundation);
        foreach (RoleInfo role in tfsRoles.All)
          clbTeamFoundationRoles.Items.Add(role.Name);

        spRoles = UserController.GetAvailableRolesBySystem(SystemTier.SharePoint);
        foreach (RoleInfo role in spRoles.All)
          clbSharepointRoles.Items.Add(role.Name);

        if (spRoles.All.Count == 0)
        {
          clbSharepointRoles.Enabled = false;
          clbSharepointRoles.BackColor = Color.DarkGray;
          cbToggleSharepointRoles.Enabled = false;
        }

        rsRoles = UserController.GetAvailableRolesBySystem(SystemTier.ReportingServices);
        foreach (RoleInfo role in rsRoles.All)
          clbReportingServicesRoles.Items.Add(role.Name);

        if (rsRoles.All.Count == 0)
        {
          clbReportingServicesRoles.Enabled = false;
          clbReportingServicesRoles.BackColor = Color.DarkGray;
          cbToggleReportingServicesRoles.Enabled = false;
        }

        RefreshContents();
      }
      catch (Exception ex)
      {
        TfsAdminToolTracer.TraceException(TfsAdminToolTracer.TraceSwitch.TraceError, ex);
        MessageBox.Show(this, Resources.UnexpectedExceptionPrompt, Resources.UnexpectedExceptionCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void UserConfiguration_FormClosing(object sender, FormClosingEventArgs e)
    {
      try
      {
        if (IsDirty)
        {
          if (MessageBox.Show(Resources.DiscardChangesPrompt, Resources.DiscardChangesCaption, MessageBoxButtons.YesNo) == DialogResult.No)
            e.Cancel = true;
        }
      }
      catch (Exception ex)
      {
        TfsAdminToolTracer.TraceException(TfsAdminToolTracer.TraceSwitch.TraceError, ex);
        MessageBox.Show(this, Resources.UnexpectedExceptionPrompt, Resources.UnexpectedExceptionCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void clbTeamFoundationRoles_ItemCheck(object sender, ItemCheckEventArgs e)
    {
      //TODO we need to figure out how to do this better prior to v.Next
      if (clbTeamFoundationRoles.Items[e.Index].ToString() == "Service Accounts")
        e.NewValue = CheckState.Unchecked;
    }

    private void clbTeamFoundationRoles_SelectedIndexChanged(object sender, EventArgs e)
    {
      try
      {
        this.SuspendLayout();
        
        SyncToggleWithListSelection(clbTeamFoundationRoles, cbToggleTeamFoundationRoles);
        IsDirty = true;

        if (clbTeamFoundationRoles.SelectedIndex != -1)
        {
          if (clbTeamFoundationRoles.GetItemCheckState(clbTeamFoundationRoles.SelectedIndex) == CheckState.Checked)
          {
            // Trace message
            TfsAdminToolTracer.TraceMessage(TfsAdminToolTracer.TraceSwitch.TraceInfo, "Toggle ON TeamFoundation role: " + clbTeamFoundationRoles.SelectedItem.ToString());

            if (!IsEditing)
              ToggleMappedRoles(clbTeamFoundationRoles.SelectedItem.ToString(), CheckState.Checked);
          }
          else
          {
            // Trace message
            TfsAdminToolTracer.TraceMessage(TfsAdminToolTracer.TraceSwitch.TraceInfo, "Toggle OFF TeamFoundation role: " + clbTeamFoundationRoles.SelectedItem.ToString());

            if (!IsEditing)
              ToggleMappedRoles(clbTeamFoundationRoles.SelectedItem.ToString(), CheckState.Unchecked);
          }
        }

        this.ResumeLayout();

        clbSharepointRoles.Refresh();
        clbReportingServicesRoles.Refresh();
      }
      catch (Exception ex)
      {
        TfsAdminToolTracer.TraceException(TfsAdminToolTracer.TraceSwitch.TraceError, ex);
        MessageBox.Show(this, Resources.UnexpectedExceptionPrompt, Resources.UnexpectedExceptionCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void clbSharepointRoles_SelectedIndexChanged(object sender, EventArgs e)
    {
      try
      {
        this.SuspendLayout();

        SyncToggleWithListSelection(clbSharepointRoles, cbToggleSharepointRoles);
        IsDirty = true;

        this.ResumeLayout();
      }
      catch (Exception ex)
      {
        TfsAdminToolTracer.TraceException(TfsAdminToolTracer.TraceSwitch.TraceError, ex);
        MessageBox.Show(this, Resources.UnexpectedExceptionPrompt, Resources.UnexpectedExceptionCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void clbReportingServicesRoles_SelectedIndexChanged(object sender, EventArgs e)
    {
      try
      {
        this.SuspendLayout();

        SyncToggleWithListSelection(clbReportingServicesRoles, cbToggleReportingServicesRoles);
        IsDirty = true;

        this.ResumeLayout();
      }
      catch (Exception ex)
      {
        TfsAdminToolTracer.TraceException(TfsAdminToolTracer.TraceSwitch.TraceError, ex);
        MessageBox.Show(this, Resources.UnexpectedExceptionPrompt, Resources.UnexpectedExceptionCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void cbToggleTeamFoundationRoles_CheckedChanged(object sender, EventArgs e)
    {
      try
      {
        this.SuspendLayout();

        IsDirty = true;
        if (!PreventToggle)      
          ToggleListSelection(clbTeamFoundationRoles, cbToggleTeamFoundationRoles);

        this.ResumeLayout();
      }
      catch (Exception ex)
      {
        TfsAdminToolTracer.TraceException(TfsAdminToolTracer.TraceSwitch.TraceError, ex);
        MessageBox.Show(this, Resources.UnexpectedExceptionPrompt, Resources.UnexpectedExceptionCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void cbToggleSharepointRoles_CheckedChanged(object sender, EventArgs e)
    {
      try
      {
        this.SuspendLayout();

        IsDirty = true;
        if (!PreventToggle)
          ToggleListSelection(clbSharepointRoles, cbToggleSharepointRoles);

        this.ResumeLayout();
      }
      catch (Exception ex)
      {
        TfsAdminToolTracer.TraceException(TfsAdminToolTracer.TraceSwitch.TraceError, ex);
        MessageBox.Show(this, Resources.UnexpectedExceptionPrompt, Resources.UnexpectedExceptionCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void cbToggleReportingServicesRoles_CheckedChanged(object sender, EventArgs e)
    {
      try
      {
        this.SuspendLayout();

        IsDirty = true;
        if (!PreventToggle)
          ToggleListSelection(clbReportingServicesRoles, cbToggleReportingServicesRoles);

        this.ResumeLayout();
      }
      catch (Exception ex)
      {
        TfsAdminToolTracer.TraceException(TfsAdminToolTracer.TraceSwitch.TraceError, ex);
        MessageBox.Show(this, Resources.UnexpectedExceptionPrompt, Resources.UnexpectedExceptionCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
      try
      {
        TfsAdminToolTracer.TraceMessage(TfsAdminToolTracer.TraceSwitch.TraceInfo, "Button clicked: User Configuration ADD");
        ProcessQuickAddUsers();
      }
      catch (Exception ex)
      {
        TfsAdminToolTracer.TraceException(TfsAdminToolTracer.TraceSwitch.TraceError, ex);
        MessageBox.Show(this, Resources.UnexpectedExceptionPrompt, Resources.UnexpectedExceptionCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void btnBrowse_Click(object sender, EventArgs e)
    {
      try
      {
        TfsAdminToolTracer.TraceMessage(TfsAdminToolTracer.TraceSwitch.TraceInfo, "Button clicked: User Configuration BROWSE");

        TfsUserCollection users = UserController.BrowseUsers(this);
        foreach (TfsUser user in users.Users)
        {
          ProcessAddUserRequest(user);
        }
      }
      catch (Exception ex)
      {
        TfsAdminToolTracer.TraceException(TfsAdminToolTracer.TraceSwitch.TraceError, ex);
        MessageBox.Show(this, Resources.UnexpectedExceptionPrompt, Resources.UnexpectedExceptionCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
      finally
      {
        RefreshContents();
      }
    }

    private void bntCancel_Click(object sender, EventArgs e)
    {
      try
      {
        TfsAdminToolTracer.TraceMessage(TfsAdminToolTracer.TraceSwitch.TraceInfo, "Button clicked: User Configuration CANCEL");

        //The user should not be prompted on discarding
        //changes when actively choosing discard

        IsDirty = false;
        DialogResult = DialogResult.Cancel;
        Close();
      }
      catch (Exception ex)
      {
        TfsAdminToolTracer.TraceException(TfsAdminToolTracer.TraceSwitch.TraceError, ex);
        MessageBox.Show(this, Resources.UnexpectedExceptionPrompt, Resources.UnexpectedExceptionCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      try
      {
        TfsAdminToolTracer.TraceMessage(TfsAdminToolTracer.TraceSwitch.TraceInfo, "Button clicked: User Configuration OK");

        // Sync role selection with edited/added users
        foreach (ListViewItem item in lvwUsers.Items)
        {
          TfsUser user = UserController.UserCollection.Users.Find(delegate(TfsUser u) { return string.Compare(u.UserName, item.Text, true) == 0; });

          SyncSelectedRolesWithUser(user, clbTeamFoundationRoles, SystemTier.TeamFoundation);
          SyncSelectedRolesWithUser(user, clbSharepointRoles, SystemTier.SharePoint);
          SyncSelectedRolesWithUser(user, clbReportingServicesRoles, SystemTier.ReportingServices);
        }

        IsDirty = false;
        DialogResult = DialogResult.OK;
        Close();
      }
      catch (Exception ex)
      {
        TfsAdminToolTracer.TraceException(TfsAdminToolTracer.TraceSwitch.TraceError, ex);
        MessageBox.Show(this, Resources.UnexpectedExceptionPrompt, Resources.UnexpectedExceptionCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void tbQuickAddUsers_KeyDown(object sender, KeyEventArgs e)
    {
      try
      {
        if (e.KeyCode == Keys.Return)
        {
          ProcessQuickAddUsers();
        }
      }
      catch (Exception ex)
      {
        TfsAdminToolTracer.TraceException(TfsAdminToolTracer.TraceSwitch.TraceError, ex);
        MessageBox.Show(this, Resources.UnexpectedExceptionPrompt, Resources.UnexpectedExceptionCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void tbQuickAddUsers_TextChanged(object sender, EventArgs e)
    {
      try
      {
        btnAdd.Enabled = tbQuickAddUsers.Text.Length > 0;
      }
      catch (Exception ex)
      {
        TfsAdminToolTracer.TraceException(TfsAdminToolTracer.TraceSwitch.TraceError, ex);
        MessageBox.Show(this, Resources.UnexpectedExceptionPrompt, Resources.UnexpectedExceptionCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void lvwUsers_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
    {
      try
      {
        e.DrawDefault = true;
        e.DrawBackground();
        e.DrawText();
      }
      catch (Exception ex)
      {
        TfsAdminToolTracer.TraceException(TfsAdminToolTracer.TraceSwitch.TraceError, ex);
        MessageBox.Show(this, Resources.UnexpectedExceptionPrompt, Resources.UnexpectedExceptionCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void lvwUsers_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
    {
      try
      {
        e.DrawText(TextFormatFlags.Left);
      }
      catch (Exception ex)
      {
        TfsAdminToolTracer.TraceException(TfsAdminToolTracer.TraceSwitch.TraceError, ex);
        MessageBox.Show(this, Resources.UnexpectedExceptionPrompt, Resources.UnexpectedExceptionCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    #endregion
  }
}
