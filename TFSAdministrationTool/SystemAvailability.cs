using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TFSAdministrationTool.Properties;
using TFSAdministrationTool.Proxy.Common;

namespace TFSAdministrationTool
{
  public partial class SystemAvailability : Form
  {
    private SiteStatus spStatus;
    private SiteStatus rsStatus;

    public SystemAvailability(SiteStatus sharePointStatus, SiteStatus reportingServicesStatus)
    {
      InitializeComponent();

      spStatus = sharePointStatus;
      rsStatus = reportingServicesStatus;
    }

    private void SystemAvailability_Load(object sender, EventArgs e)
    {
      lblSharePointStatus.Text = Enum.GetName(typeof(SiteStatus), spStatus);
      lblReportingServicesStatus.Text = Enum.GetName(typeof(SiteStatus), rsStatus);
      cbSupressWarnings.Checked = !Properties.Settings.Default.PromptForSystemAvaiablity;

      switch (spStatus)
      {
        case SiteStatus.Available:
          pbSharePointGood.Visible = true;
          pbSharePointBad.Visible = false;
          break;
        case SiteStatus.Unavailable:
          pbSharePointGood.Visible = false;
          pbSharePointBad.Visible = true;
          tbInfoSP.Text = String.Format(Resources.MissingTierConnection, "SharePoint");
          break;
        case SiteStatus.Unauthorized:
        case SiteStatus.Error:
          pbSharePointGood.Visible = false;
          pbSharePointBad.Visible = true;
          tbInfoSP.Text = String.Format(Resources.ErrorTierConnection, "SharePoint");
          break;
      }
        
      switch (rsStatus)
      {
        case SiteStatus.Available:
          pbReportingServicesGood.Visible = true;
          pbReportingServicesBad.Visible = false;
          break;
        case SiteStatus.Unavailable:
          pbReportingServicesGood.Visible = false;
          pbReportingServicesBad.Visible = true;
          tbInfoRS.Text = String.Format(Resources.MissingTierConnection, "Reporting Services");
          break;
        case SiteStatus.Unauthorized:
        case SiteStatus.Error:
          pbReportingServicesGood.Visible = false;
          pbReportingServicesBad.Visible = true;
          tbInfoRS.Text = String.Format(Resources.ErrorTierConnection, "Reporting Services");
          break;
      }
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (cbSupressWarnings.Checked)
      {
        Properties.Settings.Default.PromptForSystemAvaiablity = !cbSupressWarnings.Checked;
        Properties.Settings.Default.Save();
      }
    }
  }
}