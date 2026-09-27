namespace DXWindows.UserControl
{
    partial class ucLesson
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lblNo = new DevExpress.XtraEditors.LabelControl();
            this.lblTitle = new DevExpress.XtraEditors.LabelControl();
            this.ctmDocument = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.picPdf = new DevExpress.XtraEditors.PictureEdit();
            this.picShow = new DevExpress.XtraEditors.PictureEdit();
            this.picImage = new DevExpress.XtraEditors.PictureEdit();
            this.picNew = new DevExpress.XtraEditors.PictureEdit();
            ((System.ComponentModel.ISupportInitialize)(this.picPdf.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picShow.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picImage.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picNew.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // lblNo
            // 
            this.lblNo.Appearance.Font = new System.Drawing.Font("Arial", 22F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNo.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblNo.Appearance.Options.UseFont = true;
            this.lblNo.Appearance.Options.UseForeColor = true;
            this.lblNo.Appearance.Options.UseTextOptions = true;
            this.lblNo.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;
            this.lblNo.Location = new System.Drawing.Point(0, 88);
            this.lblNo.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.lblNo.Name = "lblNo";
            this.lblNo.Size = new System.Drawing.Size(32, 33);
            this.lblNo.TabIndex = 0;
            this.lblNo.Text = "99";
            this.lblNo.Visible = false;
            // 
            // lblTitle
            // 
            this.lblTitle.Appearance.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Appearance.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblTitle.Appearance.Options.UseFont = true;
            this.lblTitle.Appearance.Options.UseForeColor = true;
            this.lblTitle.Appearance.Options.UseTextOptions = true;
            this.lblTitle.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.lblTitle.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;
            this.lblTitle.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.lblTitle.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblTitle.Location = new System.Drawing.Point(13, 8);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(290, 54);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "Kỹ năng sống, lớp 01, năm học 2017, hà nội ngày tháng năm";
            // 
            // ctmDocument
            // 
            this.ctmDocument.Name = "ctmDocument";
            this.ctmDocument.Size = new System.Drawing.Size(61, 4);
            // 
            // picPdf
            // 
            this.picPdf.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picPdf.EditValue = global::DXWindows.Properties.Resources.Giao_an;
            this.picPdf.Location = new System.Drawing.Point(186, 65);
            this.picPdf.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.picPdf.Name = "picPdf";
            // 
            // 
            // 
            this.picPdf.Properties.AllowFocused = false;
            this.picPdf.Properties.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.False;
            this.picPdf.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.picPdf.Properties.Appearance.Options.UseBackColor = true;
            this.picPdf.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.picPdf.Properties.Padding = new System.Windows.Forms.Padding(-1);
            this.picPdf.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
            this.picPdf.Size = new System.Drawing.Size(75, 45);
            this.picPdf.TabIndex = 6;
            this.picPdf.Click += new System.EventHandler(this.lblGiaoAn_Click);
            // 
            // picShow
            // 
            this.picShow.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picShow.EditValue = global::DXWindows.Properties.Resources.trinh_chieu;
            this.picShow.Location = new System.Drawing.Point(91, 65);
            this.picShow.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.picShow.Name = "picShow";
            // 
            // 
            // 
            this.picShow.Properties.AllowFocused = false;
            this.picShow.Properties.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.False;
            this.picShow.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.picShow.Properties.Appearance.Options.UseBackColor = true;
            this.picShow.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.picShow.Properties.Padding = new System.Windows.Forms.Padding(-1);
            this.picShow.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
            this.picShow.Size = new System.Drawing.Size(75, 45);
            this.picShow.TabIndex = 3;
            this.picShow.Click += new System.EventHandler(this.lblTrinhChieu_Click);
            // 
            // picImage
            // 
            this.picImage.EditValue = global::DXWindows.Properties.Resources.document_excel_icon;
            this.picImage.Location = new System.Drawing.Point(12, 63);
            this.picImage.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.picImage.Name = "picImage";
            // 
            // 
            // 
            this.picImage.Properties.AllowFocused = false;
            this.picImage.Properties.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.False;
            this.picImage.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.picImage.Properties.Appearance.Options.UseBackColor = true;
            this.picImage.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.picImage.Properties.PictureAlignment = System.Drawing.ContentAlignment.TopCenter;
            this.picImage.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
            this.picImage.Size = new System.Drawing.Size(50, 50);
            this.picImage.TabIndex = 2;
            // 
            // picNew
            // 
            this.picNew.EditValue = global::DXWindows.Properties.Resources.newicon;
            this.picNew.Location = new System.Drawing.Point(269, 78);
            this.picNew.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.picNew.Name = "picNew";
            // 
            // 
            // 
            this.picNew.Properties.AllowFocused = false;
            this.picNew.Properties.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.False;
            this.picNew.Properties.AllowScrollOnMouseWheel = DevExpress.Utils.DefaultBoolean.False;
            this.picNew.Properties.AllowZoomOnMouseWheel = DevExpress.Utils.DefaultBoolean.False;
            this.picNew.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.picNew.Properties.Appearance.Options.UseBackColor = true;
            this.picNew.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.picNew.Properties.Padding = new System.Windows.Forms.Padding(-8, 3, 0, -3);
            this.picNew.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom;
            this.picNew.Properties.ZoomPercent = 60D;
            this.picNew.Size = new System.Drawing.Size(46, 50);
            this.picNew.TabIndex = 5;
            // 
            // ucLesson
            // 
            this.Appearance.BorderColor = System.Drawing.Color.White;
            this.Appearance.Options.UseBorderColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.picPdf);
            this.Controls.Add(this.picShow);
            this.Controls.Add(this.picImage);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblNo);
            this.Controls.Add(this.picNew);
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Name = "ucLesson";
            this.Size = new System.Drawing.Size(308, 123);
            ((System.ComponentModel.ISupportInitialize)(this.picPdf.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picShow.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picImage.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picNew.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lblNo;
        private DevExpress.XtraEditors.LabelControl lblTitle;
        private DevExpress.XtraEditors.PictureEdit picImage;
        private DevExpress.XtraEditors.PictureEdit picShow;
        private DevExpress.XtraEditors.PictureEdit picNew;
        private DevExpress.XtraEditors.PictureEdit picPdf;
        private System.Windows.Forms.ContextMenuStrip ctmDocument;
    }
}
