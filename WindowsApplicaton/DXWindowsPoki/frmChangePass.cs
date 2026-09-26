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
    public partial class frmChangePass : Form
    {
        private IClientService _clientService;
        public frmChangePass()
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
            if (string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show(this,
                           @"Vui lòng nhập mật khẩu mới.",
                           @"Thông báo", MessageBoxButtons.OK,
                           MessageBoxIcon.Warning);
                return;
            } if (txtPassword.Text != txtRePass.Text)
            {
                MessageBox.Show(this,
                           @"Xác nhận mật khẩu chưa đúng.",
                           @"Thông báo", MessageBoxButtons.OK,
                           MessageBoxIcon.Warning);
                return;
            }

            if (InternetHelper.CheckForInternetConnection())
            { // client
                var clientservice = new ClientService();
                using (var client = new HttpClient()){
                    client.BaseAddress = new Uri(GlobalSession.BaseApiUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    // HTTP GET
                    HttpResponseMessage response =
                        client.GetAsync("api/PublicApi/PostChangePassword?userName=" + Globals.Userlogin.UserName.ToLower() + "&passWord=" + txtPassword.Text.MD5Hash()).Result;
                    {
                        Client onlClient = response.Content.ReadAsAsync<Client>().Result;
                        if (onlClient != null && onlClient.ClientId > 0)
                        {
                            {
                                _clientService.UpdatePass(Globals.Userlogin.ClientId, txtPassword.Text.MD5Hash());
                            }
                            MessageBox.Show(this,
                           @"Đổi mật khẩu thành công",
                           @"Thông báo", MessageBoxButtons.OK,
                           MessageBoxIcon.Warning);

                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show(this,
                           @"Đổi mật khẩu không thành công",
                           @"Thông báo", MessageBoxButtons.OK,
                           MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show(this,
                          @"Vui lòng kiểm tra internet khi đổi mật khẩu",
                          @"Thông báo", MessageBoxButtons.OK,
                          MessageBoxIcon.Warning);
            }



        }
    }
}
