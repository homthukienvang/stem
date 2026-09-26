using System;
using System.Reflection;
using Model;

namespace DXWindows
{
    public partial class frmHotline : DevExpress.XtraEditors.XtraForm
    {
        public News objNew { get; set; }
        public bool SetMinSize { get; set; }
        public frmHotline()
        {
            InitializeComponent();
        }

        private void LoadData()
        {
            //resize
            if (SetMinSize)
            {
                Height = MinimumSize.Height;
                panelControl1.Height = panelControl1.MinimumSize.Height;
                lblTitle.Height = lblTitle.MinimumSize.Height;
            }
            if (objNew != null)
            {
                if (objNew.NewsId == 4)
                {
                    labelControl1.Text = "Hotline";
                }
                else {
                    var currentVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                    labelControl1.Text = "Thông tin phần mềm (Version "+currentVersion +")";
                }
                lblTitle.Text = objNew.Description;
            }
        }

        private void frmHotline_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void pictureEdit1_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void pictureEdit1_Click(object sender, EventArgs e)
        {

            this.Dispose();
        }
    }
}
