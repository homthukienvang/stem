using System;
using System.Windows.Forms;

namespace DXWindows
{
    public partial class frmPrinter : Form
    {
        public bool AllowPrint = false;
        public frmPrinter()
        {
            InitializeComponent();
        }

        private void pdfViewer1_PopupMenuShowing(object sender, DevExpress.XtraPdfViewer.PdfPopupMenuShowingEventArgs e)
        {
            if (!AllowPrint)
            {
                try
                {
                    string txt = "Select Tool,Print...,Select All,Document properties...";
                    foreach (DevExpress.XtraBars.BarItemLink itemLink in e.Menu.ItemLinks)
                    {
                        if (txt.Contains(itemLink.Caption))
                        {
                            itemLink.Visible = false;
                        }
                    }
                }
                catch (Exception)
                {

                }
            }

        }

        private void pdfViewer1_PasswordRequested(object sender, DevExpress.Pdf.PdfPasswordRequestedEventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
            //this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(Form2_KeyDown);
        }

        void Form2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A ||e.Control && e.KeyCode == Keys.P || e.Control && e.KeyCode == Keys.C ||
                                  e.Control && e.KeyCode == Keys.S)
            {
                e.Handled = true;
                MessageBox.Show("Vui lòng liên hệ với STEM+ nếu bạn muốn copy tài liệu này !",
                    "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pdfViewer1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
           
        }
    }
}
