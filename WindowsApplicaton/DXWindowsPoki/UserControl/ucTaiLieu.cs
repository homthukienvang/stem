using System;
using System.IO;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace DXWindows.UserControl
{
    public partial class ucTaiLieu : DevExpress.XtraEditors.XtraUserControl
    {
        public ucTaiLieu()
        {
            InitializeComponent();
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            var item = (SimpleButton)sender;
            var fName = item.ToolTip + ".pdf";
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = @"PDF (*.pdf)|*.pdf";
                dialog.FilterIndex = 2;
                dialog.FileName = fName;
                dialog.RestoreDirectory = true;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string fileUrl = Path.Combine(Application.StartupPath + "\\pdf\\", fName);
                    File.Copy(fileUrl, dialog.FileName, true);
                }
            }
        }

        private void hp_OpenLink(object sender, DevExpress.XtraEditors.Controls.OpenLinkEventArgs e)
        {
            var item = (HyperLinkEdit)sender;
            var fName = item.ToolTip + ".pdf";
            var f = new frmViewDoc
            {
                AllowPrint = true,
                pdfViewer1 = { Visible = true },
                axWindowsMediaPlayer1 = { Visible = false },
                FilePath = Application.StartupPath + @"\pdf\" + fName,
                FileName = fName,
                CustomTitle = item.Text
            };

            f.Show();
        }
    }
}
