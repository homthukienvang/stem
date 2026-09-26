namespace DXWindows
{
    partial class frmViewDoc
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmViewDoc));
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.axWindowsMediaPlayer1 = new AxWMPLib.AxWindowsMediaPlayer();
            this.pdfViewer1 = new DevExpress.XtraPdfViewer.PdfViewer();
            this.panHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.picMinimize = new DevExpress.XtraEditors.PictureEdit();
            this.picMaximize = new DevExpress.XtraEditors.PictureEdit();
            this.picturePrint = new DevExpress.XtraEditors.PictureEdit();
            this.picClose = new DevExpress.XtraEditors.PictureEdit();
            this.picMessage = new DevExpress.XtraEditors.PictureEdit();
            this.alertControl1 = new DevExpress.XtraBars.Alerter.AlertControl(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axWindowsMediaPlayer1)).BeginInit();
            this.panHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picMinimize.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMaximize.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picturePrint.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picClose.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMessage.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl2
            // 
            this.panelControl2.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl2.Controls.Add(this.axWindowsMediaPlayer1);
            this.panelControl2.Controls.Add(this.pdfViewer1);
            this.panelControl2.Controls.Add(this.panHeader);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(0, 0);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(1054, 667);
            this.panelControl2.TabIndex = 1;
            // 
            // axWindowsMediaPlayer1
            // 
            this.axWindowsMediaPlayer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axWindowsMediaPlayer1.Enabled = true;
            this.axWindowsMediaPlayer1.Location = new System.Drawing.Point(0, 38);
            this.axWindowsMediaPlayer1.Margin = new System.Windows.Forms.Padding(2);
            this.axWindowsMediaPlayer1.Name = "axWindowsMediaPlayer1";
            this.axWindowsMediaPlayer1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axWindowsMediaPlayer1.OcxState")));
            this.axWindowsMediaPlayer1.Size = new System.Drawing.Size(1054, 629);
            this.axWindowsMediaPlayer1.TabIndex = 5;
            this.axWindowsMediaPlayer1.Visible = false;
            // 
            // pdfViewer1
            // 
            this.pdfViewer1.AutoSize = true;
            this.pdfViewer1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pdfViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pdfViewer1.Location = new System.Drawing.Point(0, 38);
            this.pdfViewer1.Margin = new System.Windows.Forms.Padding(2);
            this.pdfViewer1.Name = "pdfViewer1";
            this.pdfViewer1.ShowPrintStatusDialog = false;
            this.pdfViewer1.Size = new System.Drawing.Size(1054, 629);
            this.pdfViewer1.TabIndex = 6;
            this.pdfViewer1.Visible = false;
            this.pdfViewer1.PopupMenuShowing += new DevExpress.XtraPdfViewer.PdfPopupMenuShowingEventHandler(this.pdfViewer1_PopupMenuShowing);
            // 
            // panHeader
            // 
            this.panHeader.BackgroundImage = global::DXWindows.Properties.Resources._9;
            this.panHeader.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panHeader.Controls.Add(this.lblTitle);
            this.panHeader.Controls.Add(this.panelControl3);
            this.panHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panHeader.Location = new System.Drawing.Point(0, 0);
            this.panHeader.Name = "panHeader";
            this.panHeader.Size = new System.Drawing.Size(1054, 38);
            this.panHeader.TabIndex = 7;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Font = new System.Drawing.Font("UTM BryantLG", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(12, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(116, 19);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "Tiêu đề tài liệu";
            // 
            // panelControl3
            // 
            this.panelControl3.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl3.ContentImage = global::DXWindows.Properties.Resources._9;
            this.panelControl3.Controls.Add(this.picMinimize);
            this.panelControl3.Controls.Add(this.picMaximize);
            this.panelControl3.Controls.Add(this.picturePrint);
            this.panelControl3.Controls.Add(this.picClose);
            this.panelControl3.Controls.Add(this.picMessage);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelControl3.Location = new System.Drawing.Point(803, 0);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(251, 38);
            this.panelControl3.TabIndex = 3;
            // 
            // picMinimize
            // 
            this.picMinimize.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picMinimize.EditValue = global::DXWindows.Properties.Resources.minimize;
            this.picMinimize.Location = new System.Drawing.Point(123, 6);
            this.picMinimize.Name = "picMinimize";
            this.picMinimize.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.picMinimize.Properties.Appearance.BorderColor = System.Drawing.Color.White;
            this.picMinimize.Properties.Appearance.Options.UseBackColor = true;
            this.picMinimize.Properties.Appearance.Options.UseBorderColor = true;
            this.picMinimize.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.picMinimize.Size = new System.Drawing.Size(30, 28);
            this.picMinimize.TabIndex = 8;
            this.picMinimize.Click += new System.EventHandler(this.picMinimize_Click_1);
            // 
            // picMaximize
            // 
            this.picMaximize.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picMaximize.EditValue = global::DXWindows.Properties.Resources.max_button;
            this.picMaximize.Location = new System.Drawing.Point(164, 6);
            this.picMaximize.Name = "picMaximize";
            this.picMaximize.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.picMaximize.Properties.Appearance.BorderColor = System.Drawing.Color.White;
            this.picMaximize.Properties.Appearance.Options.UseBackColor = true;
            this.picMaximize.Properties.Appearance.Options.UseBorderColor = true;
            this.picMaximize.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.picMaximize.Properties.Padding = new System.Windows.Forms.Padding(2);
            this.picMaximize.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom;
            this.picMaximize.Size = new System.Drawing.Size(30, 28);
            this.picMaximize.TabIndex = 7;
            this.picMaximize.Click += new System.EventHandler(this.picMaximize_Click);
            // 
            // picturePrint
            // 
            this.picturePrint.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picturePrint.EditValue = global::DXWindows.Properties.Resources.printer;
            this.picturePrint.Location = new System.Drawing.Point(23, 4);
            this.picturePrint.Name = "picturePrint";
            this.picturePrint.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.picturePrint.Properties.Appearance.BorderColor = System.Drawing.Color.White;
            this.picturePrint.Properties.Appearance.Options.UseBackColor = true;
            this.picturePrint.Properties.Appearance.Options.UseBorderColor = true;
            this.picturePrint.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.picturePrint.Properties.Padding = new System.Windows.Forms.Padding(1);
            this.picturePrint.Size = new System.Drawing.Size(30, 28);
            this.picturePrint.TabIndex = 6;
            this.picturePrint.ToolTip = "In tài liệu";
            this.picturePrint.MouseClick += new System.Windows.Forms.MouseEventHandler(this.picturePrint_MouseClick);
            // 
            // picClose
            // 
            this.picClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picClose.EditValue = global::DXWindows.Properties.Resources.closebtn;
            this.picClose.Location = new System.Drawing.Point(205, 6);
            this.picClose.Name = "picClose";
            this.picClose.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.picClose.Properties.Appearance.BorderColor = System.Drawing.Color.White;
            this.picClose.Properties.Appearance.Options.UseBackColor = true;
            this.picClose.Properties.Appearance.Options.UseBorderColor = true;
            this.picClose.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.picClose.Properties.Padding = new System.Windows.Forms.Padding(1);
            this.picClose.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
            this.picClose.Size = new System.Drawing.Size(30, 28);
            this.picClose.TabIndex = 2;
            this.picClose.Click += new System.EventHandler(this.picClose_Click);
            this.picClose.MouseClick += new System.Windows.Forms.MouseEventHandler(this.picClose_MouseClick);
            // 
            // picMessage
            // 
            this.picMessage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picMessage.EditValue = global::DXWindows.Properties.Resources.speech_bubble;
            this.picMessage.Location = new System.Drawing.Point(68, 4);
            this.picMessage.Name = "picMessage";
            this.picMessage.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.picMessage.Properties.Appearance.BorderColor = System.Drawing.Color.White;
            this.picMessage.Properties.Appearance.Options.UseBackColor = true;
            this.picMessage.Properties.Appearance.Options.UseBorderColor = true;
            this.picMessage.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.picMessage.Properties.Padding = new System.Windows.Forms.Padding(2);
            this.picMessage.Size = new System.Drawing.Size(30, 28);
            this.picMessage.TabIndex = 1;
            this.picMessage.ToolTip = "Góp ý bài học";
            this.picMessage.MouseClick += new System.Windows.Forms.MouseEventHandler(this.picMessage_MouseClick);
            // 
            // alertControl1
            // 
            this.alertControl1.AppearanceHotTrackedText.Font = new System.Drawing.Font("Tahoma", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            this.alertControl1.AppearanceHotTrackedText.Options.UseFont = true;
            this.alertControl1.AppearanceText.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.alertControl1.AppearanceText.Options.UseFont = true;
            this.alertControl1.AutoFormDelay = 15000;
            this.alertControl1.FormDisplaySpeed = DevExpress.XtraBars.Alerter.AlertFormDisplaySpeed.Slow;
            this.alertControl1.FormLocation = DevExpress.XtraBars.Alerter.AlertFormLocation.TopLeft;
            this.alertControl1.ShowPinButton = false;
            // 
            // frmViewDoc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1054, 667);
            this.Controls.Add(this.panelControl2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmViewDoc";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Xem tài liệu";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmViewDoc_FormClosing);
            this.Load += new System.EventHandler(this.frmViewDoc_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.frmViewDoc_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.panelControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axWindowsMediaPlayer1)).EndInit();
            this.panHeader.ResumeLayout(false);
            this.panHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picMinimize.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMaximize.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picturePrint.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picClose.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMessage.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl2;
        public DevExpress.XtraPdfViewer.PdfViewer pdfViewer1;
        public AxWMPLib.AxWindowsMediaPlayer axWindowsMediaPlayer1;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraEditors.PictureEdit picClose;
        private DevExpress.XtraEditors.PictureEdit picMessage;
        private DevExpress.XtraEditors.PictureEdit picturePrint;
        private DevExpress.XtraEditors.PictureEdit picMinimize;
        private DevExpress.XtraEditors.PictureEdit picMaximize;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panHeader;
        private DevExpress.XtraBars.Alerter.AlertControl alertControl1;
    }
}