using System;
using System.Windows.Forms;
using DevExpress.XtraTab;
using DevExpress.XtraTab.ViewInfo;

namespace DXWindows.Helper
{
    class TabControlManage
    {
        public static bool AddTab(XtraTabControl xtraTabParent, string icon, string xtraTabName, System.Windows.Forms.UserControl userControl)
        {
            bool check = false;
            foreach (XtraTabPage tab in xtraTabParent.TabPages)
            {
                if (tab.Text == xtraTabName)
                {
                    xtraTabParent.SelectedTabPage = tab;
                    check = true;
                }
            }
            if (!check)
            {
                XtraTabPage tabControl = new XtraTabPage();
                tabControl.Name = "xtraTab";
                tabControl.Text = xtraTabName;
                tabControl.Controls.Add(userControl);
                userControl.Dock = DockStyle.Fill;
                try
                {
                    //tabControl.Image = System.Drawing.Bitmap.FromFile(System.Windows.Forms.Application.StartupPath.ToString() + @"\Icons\" + icon);
                }
                catch
                {
                }
              //  xtraTabParent.TabPages.Clear();
                xtraTabParent.TabPages.Add(tabControl);        
                xtraTabParent.SelectedTabPage = tabControl;
            }
           
            return check;
        }
        public static void CloseTab(object sender, EventArgs e)
        {
            XtraTabControl tabControl = sender as XtraTabControl;
            ClosePageButtonEventArgs arg = e as ClosePageButtonEventArgs;
            (arg.Page as XtraTabPage).Dispose();
        }
    }
}
