namespace TFSAdministrationTool
{
  partial class SystemAvailability
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SystemAvailability));
      this.cbSupressWarnings = new System.Windows.Forms.CheckBox();
      this.btnOK = new System.Windows.Forms.Button();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.lblSharePointStatus = new System.Windows.Forms.Label();
      this.pbSharePointGood = new System.Windows.Forms.PictureBox();
      this.pbSharePointBad = new System.Windows.Forms.PictureBox();
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this.lblReportingServicesStatus = new System.Windows.Forms.Label();
      this.pbReportingServicesGood = new System.Windows.Forms.PictureBox();
      this.pbReportingServicesBad = new System.Windows.Forms.PictureBox();
      this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
      this.tbInfoRS = new System.Windows.Forms.TextBox();
      this.tbInfoSP = new System.Windows.Forms.TextBox();
      this.groupBox1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pbSharePointGood)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.pbSharePointBad)).BeginInit();
      this.groupBox2.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pbReportingServicesGood)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.pbReportingServicesBad)).BeginInit();
      this.SuspendLayout();
      // 
      // cbSupressWarnings
      // 
      resources.ApplyResources(this.cbSupressWarnings, "cbSupressWarnings");
      this.cbSupressWarnings.Name = "cbSupressWarnings";
      this.cbSupressWarnings.UseVisualStyleBackColor = true;
      // 
      // btnOK
      // 
      this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
      resources.ApplyResources(this.btnOK, "btnOK");
      this.btnOK.Name = "btnOK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.tbInfoSP);
      this.groupBox1.Controls.Add(this.lblSharePointStatus);
      this.groupBox1.Controls.Add(this.pbSharePointGood);
      this.groupBox1.Controls.Add(this.pbSharePointBad);
      resources.ApplyResources(this.groupBox1, "groupBox1");
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.TabStop = false;
      // 
      // lblSharePointStatus
      // 
      resources.ApplyResources(this.lblSharePointStatus, "lblSharePointStatus");
      this.lblSharePointStatus.Name = "lblSharePointStatus";
      // 
      // pbSharePointGood
      // 
      resources.ApplyResources(this.pbSharePointGood, "pbSharePointGood");
      this.pbSharePointGood.Name = "pbSharePointGood";
      this.pbSharePointGood.TabStop = false;
      // 
      // pbSharePointBad
      // 
      resources.ApplyResources(this.pbSharePointBad, "pbSharePointBad");
      this.pbSharePointBad.Name = "pbSharePointBad";
      this.pbSharePointBad.TabStop = false;
      // 
      // groupBox2
      // 
      this.groupBox2.Controls.Add(this.tbInfoRS);
      this.groupBox2.Controls.Add(this.lblReportingServicesStatus);
      this.groupBox2.Controls.Add(this.pbReportingServicesGood);
      this.groupBox2.Controls.Add(this.pbReportingServicesBad);
      resources.ApplyResources(this.groupBox2, "groupBox2");
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.TabStop = false;
      // 
      // lblReportingServicesStatus
      // 
      resources.ApplyResources(this.lblReportingServicesStatus, "lblReportingServicesStatus");
      this.lblReportingServicesStatus.Name = "lblReportingServicesStatus";
      // 
      // pbReportingServicesGood
      // 
      resources.ApplyResources(this.pbReportingServicesGood, "pbReportingServicesGood");
      this.pbReportingServicesGood.Name = "pbReportingServicesGood";
      this.pbReportingServicesGood.TabStop = false;
      // 
      // pbReportingServicesBad
      // 
      resources.ApplyResources(this.pbReportingServicesBad, "pbReportingServicesBad");
      this.pbReportingServicesBad.Name = "pbReportingServicesBad";
      this.pbReportingServicesBad.TabStop = false;
      // 
      // notifyIcon1
      // 
      resources.ApplyResources(this.notifyIcon1, "notifyIcon1");
      // 
      // tbInfoRS
      // 
      this.tbInfoRS.BackColor = System.Drawing.SystemColors.Control;
      this.tbInfoRS.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.tbInfoRS.Cursor = System.Windows.Forms.Cursors.No;
      resources.ApplyResources(this.tbInfoRS, "tbInfoRS");
      this.tbInfoRS.Name = "tbInfoRS";
      // 
      // tbInfoSP
      // 
      this.tbInfoSP.BackColor = System.Drawing.SystemColors.Control;
      this.tbInfoSP.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.tbInfoSP.Cursor = System.Windows.Forms.Cursors.No;
      resources.ApplyResources(this.tbInfoSP, "tbInfoSP");
      this.tbInfoSP.Name = "tbInfoSP";
      // 
      // SystemAvailability
      // 
      this.AcceptButton = this.btnOK;
      resources.ApplyResources(this, "$this");
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.groupBox2);
      this.Controls.Add(this.groupBox1);
      this.Controls.Add(this.btnOK);
      this.Controls.Add(this.cbSupressWarnings);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Name = "SystemAvailability";
      this.ShowInTaskbar = false;
      this.Load += new System.EventHandler(this.SystemAvailability_Load);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pbSharePointGood)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.pbSharePointBad)).EndInit();
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pbReportingServicesGood)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.pbReportingServicesBad)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.CheckBox cbSupressWarnings;
    private System.Windows.Forms.Button btnOK;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.PictureBox pbSharePointBad;
    private System.Windows.Forms.PictureBox pbSharePointGood;
    private System.Windows.Forms.Label lblSharePointStatus;
    private System.Windows.Forms.Label lblReportingServicesStatus;
    private System.Windows.Forms.PictureBox pbReportingServicesGood;
    private System.Windows.Forms.PictureBox pbReportingServicesBad;
    private System.Windows.Forms.NotifyIcon notifyIcon1;
    private System.Windows.Forms.TextBox tbInfoRS;
    private System.Windows.Forms.TextBox tbInfoSP;
  }
}