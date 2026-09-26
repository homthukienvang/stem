using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Model;
using AppServices;
using System.Net.Http;
using Extensions;
using System.Net.Http.Headers;
using Services;
using System.IO;

namespace DXWindows
{
    public partial class frmAdvertisement : DevExpress.XtraEditors.XtraForm
    {
        AdvertisementService _advertisementService = new AdvertisementService();
        public frmAdvertisement()
        {
            InitializeComponent();
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
            //UpdateChecker show
            //get count from db
            Advertisement obj = new Advertisement();
            obj = _advertisementService.GetAdvById(2);
            if (obj == null || obj.Id <= 0) return;

            if (Math.Round((DateTime.Now - obj.StartDate).TotalDays) == 0)
            {
                obj.ClickCount = obj.ClickCount + 1;
            }
            else
            {
                obj.StartDate = DateTime.Now;
                obj.ClickCount = 0;
            }

            _advertisementService.Update(obj);

            this.Tag = obj.AdvLink;
            //display image
            try
            {
                string filepath = Application.StartupPath + @"\App_data\" + obj.AdvImage;
                if (File.Exists(filepath))
                {
                    picImage.Visible = true;
                    picImage.Image = new Bitmap(filepath);
                }
            }
            catch (Exception ex) { }
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

    }

}