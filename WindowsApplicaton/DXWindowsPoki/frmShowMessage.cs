using System;
using System.Windows.Forms;

namespace DXWindows
{
    public partial class frmShowMessage : DevExpress.XtraEditors.XtraForm
    {
        int totalMessages = 0;

        public frmShowMessage(int total)
        {
            InitializeComponent();

            totalMessages = total;
        }

        private void LoadData()
        {
            lblMessage.Text = string.Format("Bạn nhận được ({0}) tin nhắn từ STEM+", totalMessages);
        }

        private void frmMessageDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        private void frmMessageDialog_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void pictureEdit1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}