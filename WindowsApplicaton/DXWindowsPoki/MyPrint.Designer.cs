namespace DXWindows
{
    partial class MyPrint
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
            DevExpress.XtraEditors.Controls.RadioGroupItem radioGroupItem1 = new DevExpress.XtraEditors.Controls.RadioGroupItem();
            DevExpress.XtraEditors.Controls.RadioGroupItem radioGroupItem2 = new DevExpress.XtraEditors.Controls.RadioGroupItem();
            DevExpress.XtraEditors.Controls.RadioGroupItem radioGroupItem3 = new DevExpress.XtraEditors.Controls.RadioGroupItem();
            DevExpress.XtraEditors.Controls.RadioGroupItem radioGroupItem4 = new DevExpress.XtraEditors.Controls.RadioGroupItem();
            this.lblFileName = new DevExpress.XtraEditors.LabelControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.rdoPrint = new DevExpress.XtraEditors.RadioGroup();
            this.txtPageNumber = new DevExpress.XtraEditors.TextEdit();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.btnDownload = new DevExpress.XtraEditors.SimpleButton();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            this.labelQuantity = new DevExpress.XtraEditors.LabelControl();
            this.textQuantity = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rdoPrint.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPageNumber.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textQuantity.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // lblFileName
            // 
            this.lblFileName.Appearance.Options.UseTextOptions = true;
            this.lblFileName.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;
            this.lblFileName.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblFileName.Location = new System.Drawing.Point(14, 20);
            this.lblFileName.Name = "lblFileName";
            this.lblFileName.Size = new System.Drawing.Size(343, 28);
            this.lblFileName.TabIndex = 4;
            this.lblFileName.Text = "Chọn máy in được cài đặt để in";
            this.lblFileName.Visible = false;
            // 
            // panelControl1
            // 
            this.panelControl1.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(138)))), ((int)(((byte)(202)))));
            this.panelControl1.Appearance.Options.UseBackColor = true;
            this.panelControl1.Controls.Add(this.labelControl2);
            this.panelControl1.Controls.Add(this.rdoPrint);
            this.panelControl1.Controls.Add(this.txtPageNumber);
            this.panelControl1.Controls.Add(this.comboBox1);
            this.panelControl1.Controls.Add(this.lblFileName);
            this.panelControl1.Controls.Add(this.btnDownload);
            this.panelControl1.Controls.Add(this.btnClose);
            this.panelControl1.Controls.Add(this.labelQuantity);
            this.panelControl1.Controls.Add(this.textQuantity);
            this.panelControl1.Location = new System.Drawing.Point(9, 31);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(373, 245);
            this.panelControl1.TabIndex = 8;
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Options.UseTextOptions = true;
            this.labelControl2.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.labelControl2.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl2.Location = new System.Drawing.Point(128, 161);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(229, 38);
            this.labelControl2.TabIndex = 10;
            this.labelControl2.Text = "Số trang: 5-10: in từ trang 5 đến 10;  2,3,5,8: in các trang 2,3,5,8; 3: chỉ in t" +
    "rang 3";
            // 
            // rdoPrint
            // 
            this.rdoPrint.EditValue = 0;
            this.rdoPrint.Location = new System.Drawing.Point(14, 70);
            this.rdoPrint.Name = "rdoPrint";
            // 
            // 
            // 
            this.rdoPrint.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.rdoPrint.Properties.Appearance.Options.UseBackColor = true;
            this.rdoPrint.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            radioGroupItem1.Description = "In 2 mặt";
            radioGroupItem1.Value = 0;
            radioGroupItem2.Description = "In trang chẵn";
            radioGroupItem2.Value = 1;
            radioGroupItem3.Description = "In trang lẻ";
            radioGroupItem3.Value = 2;
            radioGroupItem4.Description = "In số trang";
            radioGroupItem4.Value = 3;
            this.rdoPrint.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            radioGroupItem1,
            radioGroupItem2,
            radioGroupItem3,
            radioGroupItem4});
            this.rdoPrint.Size = new System.Drawing.Size(108, 96);
            this.rdoPrint.TabIndex = 9;
            // 
            // txtPageNumber
            // 
            this.txtPageNumber.Location = new System.Drawing.Point(128, 143);
            this.txtPageNumber.Name = "txtPageNumber";
            // 
            // 
            // 
            this.txtPageNumber.Properties.NullValuePrompt = "Tất cả";
            this.txtPageNumber.Size = new System.Drawing.Size(229, 20);
            this.txtPageNumber.TabIndex = 8;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(15, 15);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(2);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(344, 21);
            this.comboBox1.TabIndex = 5;
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
            this.btnDownload.Location = new System.Drawing.Point(192, 204);
            this.btnDownload.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
            this.btnDownload.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.TabIndex = 1;
            this.btnDownload.Text = "In";
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
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
            this.btnClose.Location = new System.Drawing.Point(282, 204);
            this.btnClose.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
            this.btnClose.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnClose.Name = "btnClose";
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Hủy";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // labelQuantity
            // 
            this.labelQuantity.Appearance.Options.UseTextOptions = true;
            this.labelQuantity.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.labelQuantity.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelQuantity.Location = new System.Drawing.Point(60, 38);
            this.labelQuantity.Name = "labelQuantity";
            this.labelQuantity.Size = new System.Drawing.Size(109, 38);
            this.labelQuantity.TabIndex = 10;
            this.labelQuantity.Text = "Số bản in";
            // 
            // textQuantity
            // 
            this.textQuantity.EditValue = "1";
            this.textQuantity.Location = new System.Drawing.Point(15, 48);
            this.textQuantity.Name = "txtPageNumber";
            // 
            // 
            // 
            this.textQuantity.Properties.NullValuePrompt = "1";
            this.textQuantity.Size = new System.Drawing.Size(40, 20);
            this.textQuantity.TabIndex = 8;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.labelControl1.Appearance.ForeColor = System.Drawing.Color.White;
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Appearance.Options.UseForeColor = true;
            this.labelControl1.Location = new System.Drawing.Point(9, 10);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(53, 13);
            this.labelControl1.TabIndex = 9;
            this.labelControl1.Text = "In tài liệu";
            // 
            // MyPrint
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(138)))), ((int)(((byte)(202)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(394, 288);
            this.ControlBox = false;
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.panelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MyPrint";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MyPrint";
            this.Load += new System.EventHandler(this.MyPrint_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rdoPrint.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPageNumber.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textQuantity.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lblFileName;
        private DevExpress.XtraEditors.SimpleButton btnDownload;
        private DevExpress.XtraEditors.SimpleButton btnClose;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private System.Windows.Forms.ComboBox comboBox1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txtPageNumber;
        private DevExpress.XtraEditors.RadioGroup rdoPrint;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelQuantity;
        private DevExpress.XtraEditors.TextEdit textQuantity;
    }
}