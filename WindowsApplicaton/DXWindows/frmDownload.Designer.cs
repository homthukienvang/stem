namespace DXWindows
{
    partial class frmDownload
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
            this.lblTitle = new DevExpress.XtraEditors.LabelControl();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            this.proStatus = new DevExpress.XtraEditors.ProgressBarControl();
            this.lblFileName = new DevExpress.XtraEditors.LabelControl();
            this.lblStatus = new DevExpress.XtraEditors.LabelControl();
            this.btnDownload = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.proStatus.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Appearance.Options.UseFont = true;
            this.lblTitle.Appearance.Options.UseTextOptions = true;
            this.lblTitle.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lblTitle.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblTitle.Location = new System.Drawing.Point(14, 28);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(343, 20);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Tiêu đề tài liệu";
            // 
            // btnClose
            // 
            this.btnClose.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(138)))), ((int)(((byte)(202)))));
            this.btnClose.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.btnClose.Appearance.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnClose.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.btnClose.Appearance.Options.UseBackColor = true;
            this.btnClose.Appearance.Options.UseBorderColor = true;
            this.btnClose.Appearance.Options.UseForeColor = true;
            this.btnClose.ImageOptions.Image = global::DXWindows.Properties.Resources.exit1;
            this.btnClose.Location = new System.Drawing.Point(282, 151);
            this.btnClose.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
            this.btnClose.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 25);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Hủy";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // proStatus
            // 
            this.proStatus.Location = new System.Drawing.Point(14, 119);
            this.proStatus.Name = "proStatus";
            this.proStatus.Size = new System.Drawing.Size(343, 18);
            this.proStatus.TabIndex = 3;
            // 
            // lblFileName
            // 
            this.lblFileName.Appearance.Options.UseTextOptions = true;
            this.lblFileName.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;
            this.lblFileName.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblFileName.Location = new System.Drawing.Point(14, 84);
            this.lblFileName.Name = "lblFileName";
            this.lblFileName.Size = new System.Drawing.Size(343, 29);
            this.lblFileName.TabIndex = 4;
            this.lblFileName.Text = "lblFileName";
            // 
            // lblStatus
            // 
            this.lblStatus.Location = new System.Drawing.Point(14, 64);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(41, 13);
            this.lblStatus.TabIndex = 5;
            this.lblStatus.Text = "lblStatus";
            // 
            // btnDownload
            // 
            this.btnDownload.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(138)))), ((int)(((byte)(202)))));
            this.btnDownload.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.btnDownload.Appearance.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnDownload.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.btnDownload.Appearance.Options.UseBackColor = true;
            this.btnDownload.Appearance.Options.UseBorderColor = true;
            this.btnDownload.Appearance.Options.UseForeColor = true;
            this.btnDownload.ImageOptions.Image = global::DXWindows.Properties.Resources.download;
            this.btnDownload.Location = new System.Drawing.Point(94, 151);
            this.btnDownload.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
            this.btnDownload.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(75, 25);
            this.btnDownload.TabIndex = 1;
            this.btnDownload.Text = "Tải về";
            this.btnDownload.Visible = false;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.lblStatus);
            this.panelControl1.Controls.Add(this.lblTitle);
            this.panelControl1.Controls.Add(this.lblFileName);
            this.panelControl1.Controls.Add(this.btnDownload);
            this.panelControl1.Controls.Add(this.proStatus);
            this.panelControl1.Controls.Add(this.btnClose);
            this.panelControl1.Location = new System.Drawing.Point(12, 31);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(373, 194);
            this.panelControl1.TabIndex = 6;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.labelControl1.Appearance.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Appearance.Options.UseForeColor = true;
            this.labelControl1.Location = new System.Drawing.Point(15, 10);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(58, 13);
            this.labelControl1.TabIndex = 7;
            this.labelControl1.Text = "Tải tài liệu";
            // 
            // frmDownload
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(138)))), ((int)(((byte)(202)))));
            this.Appearance.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(138)))), ((int)(((byte)(67)))));
            this.Appearance.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.Appearance.Options.UseBackColor = true;
            this.Appearance.Options.UseForeColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(395, 235);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.panelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDownload";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tải tài liệu";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmDownload_FormClosing);
            this.Load += new System.EventHandler(this.frmDownload_Load);
            ((System.ComponentModel.ISupportInitialize)(this.proStatus.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lblTitle;
        private DevExpress.XtraEditors.SimpleButton btnDownload;
        private DevExpress.XtraEditors.SimpleButton btnClose;
        private DevExpress.XtraEditors.ProgressBarControl proStatus;
        private DevExpress.XtraEditors.LabelControl lblFileName;
        private DevExpress.XtraEditors.LabelControl lblStatus;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
    }
}