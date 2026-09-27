using System;
using System.Windows.Forms;

namespace DXWindows
{
    partial class frmViewExe
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
            this.alertControl1 = new DevExpress.XtraBars.Alerter.AlertControl(this.components);
            this.pnHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnUtil = new DevExpress.XtraEditors.PanelControl();
            this.picMinimize = new DevExpress.XtraEditors.PictureEdit();
            this.picturePrint = new DevExpress.XtraEditors.PictureEdit();
            this.picClose = new DevExpress.XtraEditors.PictureEdit();
            this.picMessage = new DevExpress.XtraEditors.PictureEdit();
            this.pnMain = new DevExpress.XtraEditors.PanelControl();
            this.pnView = new DevExpress.XtraEditors.PanelControl();
            this.lblWaiting = new System.Windows.Forms.Label();
            this.pnHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnUtil)).BeginInit();
            this.pnUtil.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picMinimize.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picturePrint.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picClose.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMessage.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnMain)).BeginInit();
            this.pnMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnView)).BeginInit();
            this.pnView.SuspendLayout();
            this.SuspendLayout();
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
            // pnHeader
            // 
            this.pnHeader.BackgroundImage = global::DXWindows.Properties.Resources._9;
            this.pnHeader.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnHeader.Controls.Add(this.lblTitle);
            this.pnHeader.Controls.Add(this.pnUtil);
            this.pnHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnHeader.Location = new System.Drawing.Point(0, 0);
            this.pnHeader.Name = "pnHeader";
            this.pnHeader.Size = new System.Drawing.Size(1054, 38);
            this.pnHeader.TabIndex = 7;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(12, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(126, 20);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "Tiêu đề tài liệu";
            // 
            // pnUtil
            // 
            this.pnUtil.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pnUtil.ContentImage = global::DXWindows.Properties.Resources._9;
            this.pnUtil.Controls.Add(this.picMinimize);
            this.pnUtil.Controls.Add(this.picturePrint);
            this.pnUtil.Controls.Add(this.picClose);
            this.pnUtil.Controls.Add(this.picMessage);
            this.pnUtil.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnUtil.Location = new System.Drawing.Point(803, 0);
            this.pnUtil.Name = "pnUtil";
            this.pnUtil.Size = new System.Drawing.Size(251, 38);
            this.pnUtil.TabIndex = 3;
            // 
            // picMinimize
            // 
            this.picMinimize.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picMinimize.EditValue = global::DXWindows.Properties.Resources.minimize;
            this.picMinimize.Location = new System.Drawing.Point(165, 6);
            this.picMinimize.Name = "picMinimize";
            this.picMinimize.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.picMinimize.Properties.Appearance.BorderColor = System.Drawing.Color.White;
            this.picMinimize.Properties.Appearance.Options.UseBackColor = true;
            this.picMinimize.Properties.Appearance.Options.UseBorderColor = true;
            this.picMinimize.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.picMinimize.Size = new System.Drawing.Size(30, 28);
            this.picMinimize.TabIndex = 8;
            this.picMinimize.Click += new System.EventHandler(this.picMinimize_Click);
            // 
            // picturePrint
            // 
            this.picturePrint.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picturePrint.EditValue = global::DXWindows.Properties.Resources.printer;
            this.picturePrint.Location = new System.Drawing.Point(78, 6);
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
            this.picMessage.Location = new System.Drawing.Point(123, 6);
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
            // pnMain
            // 
            this.pnMain.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pnMain.Controls.Add(this.pnView);
            this.pnMain.Controls.Add(this.pnHeader);
            this.pnMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnMain.Location = new System.Drawing.Point(0, 0);
            this.pnMain.Name = "pnMain";
            this.pnMain.Size = new System.Drawing.Size(1054, 667);
            this.pnMain.TabIndex = 1;
            // 
            // pnView
            // 
            this.pnView.Controls.Add(this.lblWaiting);
            this.pnView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnView.Location = new System.Drawing.Point(0, 38);
            this.pnView.Name = "pnView";
            this.pnView.Size = new System.Drawing.Size(1054, 629);
            this.pnView.TabIndex = 9;
            // 
            // lblWaiting
            // 
            this.lblWaiting.AutoSize = true;
            this.lblWaiting.BackColor = System.Drawing.Color.White;
            this.lblWaiting.ForeColor = System.Drawing.Color.Black;
            this.lblWaiting.Location = new System.Drawing.Point(494, 304);
            this.lblWaiting.Name = "lblWaiting";
            this.lblWaiting.Size = new System.Drawing.Size(50, 13);
            this.lblWaiting.TabIndex = 8;
            this.lblWaiting.Text = "xin đợi...";
            // 
            // frmViewExe
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1054, 667);
            this.Controls.Add(this.pnMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmViewExe";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Xem tài liệu";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmViewExe_FormClosing);
            this.Load += new System.EventHandler(this.frmViewExe_Load);
            this.Resize += new EventHandler(frmViewExe_Resize);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.frmViewExe_KeyPress);
            this.pnHeader.ResumeLayout(false);
            this.pnHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnUtil)).EndInit();
            this.pnUtil.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picMinimize.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picturePrint.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picClose.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMessage.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnMain)).EndInit();
            this.pnMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pnView)).EndInit();
            this.pnView.ResumeLayout(false);
            this.pnView.PerformLayout();
            this.ResumeLayout(false);

        }


        #endregion
        private DevExpress.XtraBars.Alerter.AlertControl alertControl1;
        private System.Windows.Forms.Panel pnHeader;
        private System.Windows.Forms.Label lblTitle;
        private DevExpress.XtraEditors.PanelControl pnUtil;
        private DevExpress.XtraEditors.PictureEdit picMinimize;
//        private DevExpress.XtraEditors.PictureEdit picMaximize;
        private DevExpress.XtraEditors.PictureEdit picturePrint;
        private DevExpress.XtraEditors.PictureEdit picClose;
        private DevExpress.XtraEditors.PictureEdit picMessage;
        private DevExpress.XtraEditors.PanelControl pnMain;
        private System.Windows.Forms.Label lblWaiting;
        private DevExpress.XtraEditors.PanelControl pnView;
    }
}