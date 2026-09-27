using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Extensions;
using DXWindows.Helper;
using Model;

namespace DXWindows.UserControl
{
    public partial class ucNews : DevExpress.XtraEditors.XtraUserControl
    {
        public ucNews()
        {
            InitializeComponent();
        }
       
        public void GetNews()
        {
            
            if (InternetHelper.CheckForInternetConnection())
                { // client
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(GlobalSession.BaseApiUrl);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        // HTTP GET
                        HttpResponseMessage response =   client.GetAsync("api/PublicApi/GetNews").Result;
                        {

                            News news =   response.Content.ReadAsAsync<News>().Result;
                            if (news != null && news.NewsId > 0)
                            {
                                lblTitle.Text = news.Title;
                                lbDes.Text =news.Description;

                            }
                            else
                            {

                            }
                        }
                    }
                }
        }

        private void ucNews_Load(object sender, EventArgs e)
        {
            GetNews();
        }
    }
}
