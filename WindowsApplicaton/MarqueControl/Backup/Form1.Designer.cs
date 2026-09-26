namespace MarqueControlTest
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.superMarquee1 = new MarqueControl.Controls.SuperMarquee();
            this.textElement1 = new MarqueControl.Entity.TextElement();
            this.textElement2 = new MarqueControl.Entity.TextElement();
            this.textElement3 = new MarqueControl.Entity.TextElement();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "bullet_more01.gif");
            this.imageList1.Images.SetKeyName(1, "down_arrow.gif");
            this.imageList1.Images.SetKeyName(2, "uparrow02.gif");
            this.imageList1.Images.SetKeyName(3, "align-left.gif");
            // 
            // superMarquee1
            // 
            this.superMarquee1.BackgroundImage = global::MarqueControlTest.Properties.Resources.vista;
            this.superMarquee1.Elements.AddRange(new MarqueControl.Entity.TextElement[] {
            this.textElement1,
            this.textElement2,
            this.textElement3});
            this.superMarquee1.ImageList = this.imageList1;
            this.superMarquee1.Location = new System.Drawing.Point(12, 2);
            this.superMarquee1.Name = "superMarquee1";
            this.superMarquee1.Size = new System.Drawing.Size(306, 35);
            this.superMarquee1.TabIndex = 0;
            this.superMarquee1.Text = "superMarquee1";
            // 
            // textElement1
            // 
            this.textElement1.Font = new System.Drawing.Font("Arial Black", 9F, ((System.Drawing.FontStyle)(((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic)
                            | System.Drawing.FontStyle.Strikeout))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textElement1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.textElement1.LeftImageIndex = 1;
            this.textElement1.RightImageIndex = 2;
            this.textElement1.Text = "Element1";
            this.textElement1.ToolTipText = "ToolTip for Element1";
            // 
            // textElement2
            // 
            this.textElement2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textElement2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.textElement2.LeftImageIndex = 1;
            this.textElement2.RightImageIndex = 0;
            this.textElement2.Text = "Element2";
            this.textElement2.ToolTipText = "ToolTip for Element2";
            // 
            // textElement3
            // 
            this.textElement3.Font = new System.Drawing.Font("Times New Roman", 9F, ((System.Drawing.FontStyle)(((System.Drawing.FontStyle.Italic | System.Drawing.FontStyle.Underline)
                            | System.Drawing.FontStyle.Strikeout))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textElement3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.textElement3.LeftImageIndex = 3;
            this.textElement3.Text = "Element3";
            this.textElement3.ToolTipText = "ToolTip for Element3";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(321, 37);
            this.Controls.Add(this.superMarquee1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList imageList1;
        private MarqueControl.Controls.SuperMarquee superMarquee1;
        private MarqueControl.Entity.TextElement textElement1;
        private MarqueControl.Entity.TextElement textElement2;
        private MarqueControl.Entity.TextElement textElement3;

    }
}