using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Model;
using AppServices;
using System.Net.Http;
using Extensions;
using System.Net.Http.Headers;
using Services;
using System.IO;
using DXWindows.Helper;

namespace DXWindows
{
    public partial class frmAdvertisementFake : DevExpress.XtraEditors.XtraForm
    {
        AdvertisementService _advertisementService = new AdvertisementService();

        public frmAdvertisementFake()
        {
            InitializeComponent();
        }

        void pnl_Click(object sender, EventArgs e)
        {
            XtraMessageBox.Show("click");
        }

        private void SendAdvertismentCount(int clientid, int advid)
        {
            if (DXWindows.Helper.InternetHelper.CheckForInternetConnection())
            {
                BackgroundWorker bw = new BackgroundWorker();
                bw.WorkerReportsProgress = true;
                bw.DoWork += (s, e) =>
                {
                    var clientservice = new ClientService();
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(GlobalSession.BaseApiUrl);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        // HTTP GET
                        HttpResponseMessage response = client.GetAsync("api/PublicApi/PostAdvertisment?clientid=" + clientid + "&advid=" + advid).Result;
                    }
                };

                bw.RunWorkerAsync();
            }
        }

        private void pictureEdit1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmAdvertisement_Load(object sender, EventArgs e)
        {
           
        }

        private void picImage_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Tag + ""))
            {
                System.Diagnostics.Process.Start(this.Tag + "");
                SendAdvertismentCount(Globals.Userlogin.ClientId, 2);
            }
        }

        private void pictureEdit1_MouseHover(object sender, EventArgs e)
        {
            pictureEdit1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
        }

        private void pictureEdit1_MouseLeave(object sender, EventArgs e)
        {
            pictureEdit1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
        }

        private void panelControlClick_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Tag + ""))
            {
                System.Diagnostics.Process.Start(this.Tag + "");
                SendAdvertismentCount(Globals.Userlogin.ClientId, 2);
            }
        }

        private void frmAdvertisementFake_Click(object sender, EventArgs e)
        {
            XtraMessageBox.Show("form click aaa");
        }                
    }


}