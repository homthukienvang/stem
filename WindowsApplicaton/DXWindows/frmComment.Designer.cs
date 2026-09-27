namespace DXWindows
{
    partial class frmComment
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
            this.lblLesson = new DevExpress.XtraEditors.LabelControl();
            this.lblTitle = new DevExpress.XtraEditors.LabelControl();
            this.lblPurpose = new DevExpress.XtraEditors.LabelControl();
            this.optType = new DevExpress.XtraEditors.RadioGroup();
            this.txtContent = new DevExpress.XtraEditors.MemoEdit();
            this.lblContent = new DevExpress.XtraEditors.LabelControl();
            this.dxErrorProvider1 = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(this.components);
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.pictureEdit1 = new DevExpress.XtraEditors.PictureEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.optType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtContent.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // lblLesson
            // 
            this.lblLesson.Location = new System.Drawing.Point(14, 12);
            this.lblLesson.Name = "lblLesson";
            this.lblLesson.Size = new System.Drawing.Size(34, 13);
            this.lblLesson.TabIndex = 2;
            this.lblLesson.Text = "Bài học";
            // 
            // lblTitle
            // 
            this.lblTitle.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Appearance.Options.UseFont = true;
            this.lblTitle.Location = new System.Drawing.Point(85, 12);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(64, 13);
            this.lblTitle.TabIndex = 3;
            this.lblTitle.Text = "Tên bài học";
            // 
            // lblPurpose
            // 
            this.lblPurpose.Location = new System.Drawing.Point(14, 44);
            this.lblPurpose.Name = "lblPurpose";
            this.lblPurpose.Size = new System.Drawing.Size(41, 13);
            this.lblPurpose.TabIndex = 4;
            this.lblPurpose.Text = "Mục đích";
            // 
            // optType
            // 
            this.optType.EditValue = 0;
            this.optType.Location = new System.Drawing.Point(85, 37);
            this.optType.Name = "optType";
            this.optType.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.optType.Properties.Appearance.BackColor2 = System.Drawing.Color.Transparent;
            this.optType.Properties.Appearance.Options.UseBackColor = true;
            this.optType.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.optType.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(0, "Ghi chú bài học"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(1, "Gửi góp ý STEM+")});
            this.optType.Size = new System.Drawing.Size(239, 27);
            this.optType.TabIndex = 0;
            this.optType.SelectedIndexChanged += new System.EventHandler(this.optType_SelectedIndexChanged);
            // 
            // txtContent
            // 
            this.txtContent.Location = new System.Drawing.Point(85, 76);
            this.txtContent.Name = "txtContent";
            this.txtContent.Size = new System.Drawing.Size(472, 217);
            this.txtContent.TabIndex = 1;
            // 
            // lblContent
            // 
            this.lblContent.Location = new System.Drawing.Point(14, 76);
            this.lblContent.Name = "lblContent";
            this.lblContent.Size = new System.Drawing.Size(42, 13);
            this.lblContent.TabIndex = 7;
            this.lblContent.Text = "Nội dung";
            // 
            // dxErrorProvider1
            // 
            this.dxErrorProvider1.ContainerControl = this;
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
            this.btnClose.Location = new System.Drawing.Point(309, 304);
            this.btnClose.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
            this.btnClose.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(84, 25);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Quay lại";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(138)))), ((int)(((byte)(202)))));
            this.btnSave.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.btnSave.Appearance.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnSave.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.btnSave.Appearance.Options.UseBackColor = true;
            this.btnSave.Appearance.Options.UseBorderColor = true;
            this.btnSave.Appearance.Options.UseForeColor = true;
            this.btnSave.ImageOptions.Image = global::DXWindows.Properties.Resources.orange_response_16;
            this.btnSave.Location = new System.Drawing.Point(206, 304);
            this.btnSave.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
            this.btnSave.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(84, 25);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Lưu";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // panelControl1
            // 
            this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl1.Controls.Add(this.txtContent);
            this.panelControl1.Controls.Add(this.btnClose);
            this.panelControl1.Controls.Add(this.lblLesson);
            this.panelControl1.Controls.Add(this.btnSave);
            this.panelControl1.Controls.Add(this.lblTitle);
            this.panelControl1.Controls.Add(this.lblContent);
            this.panelControl1.Controls.Add(this.lblPurpose);
            this.panelControl1.Controls.Add(this.optType);
            this.panelControl1.Location = new System.Drawing.Point(11, 36);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(574, 338);
            this.panelControl1.TabIndex = 8;
            // 
            // pictureEdit1
            // 
            this.pictureEdit1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureEdit1.EditValue = global::DXWindows.Properties.Resources.close;
            this.pictureEdit1.Location = new System.Drawing.Point(559, 6);
            this.pictureEdit1.Name = "pictureEdit1";
            this.pictureEdit1.Properties.AllowFocused = false;
            this.pictureEdit1.Properties.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.False;
            this.pictureEdit1.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.pictureEdit1.Properties.Appearance.Options.UseBackColor = true;
            this.pictureEdit1.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pictureEdit1.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
            this.pictureEdit1.Size = new System.Drawing.Size(26, 24);
            this.pictureEdit1.TabIndex = 105;
            this.pictureEdit1.Click += new System.EventHandler(this.pictureEdit1_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.labelControl1.Appearance.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Appearance.Options.UseForeColor = true;
            this.labelControl1.Location = new System.Drawing.Point(12, 12);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(92, 13);
            this.labelControl1.TabIndex = 104;
            this.labelControl1.Text = "Ghi chú và góp ý";
            // 
            // frmComment
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(138)))), ((int)(((byte)(202)))));
            this.Appearance.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(160)))), ((int)(((byte)(221)))));
            this.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(597, 383);
            this.Controls.Add(this.pictureEdit1);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.panelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmComment";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ghi chú và góp ý";
            this.Load += new System.EventHandler(this.frmComment_Load);
            ((System.ComponentModel.ISupportInitialize)(this.optType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtContent.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lblLesson;
        private DevExpress.XtraEditors.LabelControl lblTitle;
        private DevExpress.XtraEditors.LabelControl lblPurpose;
        private DevExpress.XtraEditors.RadioGroup optType;
        private DevExpress.XtraEditors.MemoEdit txtContent;
        private DevExpress.XtraEditors.LabelControl lblContent;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.SimpleButton btnClose;
        private DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider dxErrorProvider1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PictureEdit pictureEdit1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
    }
}