using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Model;
using Services;

namespace DXWindows
{
    public partial class frmMessageDialog : XtraForm
    {
        public News objNew { get; set; }

        public frmMessageDialog()
        {
            InitializeComponent();
        }

        private void LoadData()
        {
            if (objNew != null)
            {
                lblTitle.DocumentText = objNew.Description;
                lblTitle.Navigating += new WebBrowserNavigatingEventHandler(lblTitle_Navigating);
            }
        }

        private void frmMessageDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (chkShow.Checked)
            {
                if (objNew != null)
                {
                    objNew.Status = 1;
                    new NewsService().Update(objNew);
                }
            }
        }

        private void frmMessageDialog_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void pictureEdit1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void lblTitle_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            e.Cancel = true;
            Process.Start(new ProcessStartInfo(e.Url.ToString()) { UseShellExecute = true });
        }
    }
}