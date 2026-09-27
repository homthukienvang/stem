using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Windows.Forms;
using DXWindows.Helper;
using Extensions;
using Model;
using Services;

namespace DXWindows
{
    public partial class frmResetPass : Form
    {
        private readonly IClientService _clientService;

        string defaultPass = "123456";

        public frmResetPass(string username)
        {
            InitializeComponent();
            _clientService = new ClientService();
            lbUser.Text = username;
        }

        public frmResetPass()
        {
            InitializeComponent();
            _clientService = new ClientService();
            lbUser.Text = Globals.Userlogin.FullName;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnChangePass_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(lbUser.Text))
            {
                MessageBox.Show(this, @"Vui lòng kiểm tra tài khoản khi đặt lại mật khẩu", @"Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (InternetHelper.CheckForInternetConnection())
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(GlobalSession.BaseApiUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    // HTTP GET
                    HttpResponseMessage response =
                        client.GetAsync("api/PublicApi/PostChangePassword?userName=" + lbUser.Text + "&passWord=" + defaultPass.MD5Hash()).Result;
                    {
                        Client onlClient = response.Content.ReadAsAsync<Client>().Result;
                        if (onlClient != null && onlClient.ClientId > 0)
                        {
                            {
                                _clientService.UpdatePass(onlClient.ClientId, defaultPass.MD5Hash());
                            }
                            MessageBox.Show(this,
                           @"Đặt lại mật khẩu thành công",
                           @"Thông báo", MessageBoxButtons.OK,
                           MessageBoxIcon.Warning);

                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show(this,
                           @"Đặt lại mật khẩu không thành công. Vui lòng thử lại.",
                           @"Thông báo", MessageBoxButtons.OK,
                           MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show(this,@"Vui lòng kiểm tra internet khi đặt lại mật khẩu",@"Thông báo", MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
        }
    }
}
