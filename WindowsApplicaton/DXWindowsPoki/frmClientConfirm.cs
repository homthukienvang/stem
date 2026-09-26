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
using System.Text.RegularExpressions;
using DevExpress.XtraEditors;

namespace DXWindows
{
    public partial class frmClientConfirm : Form
    {
        private IClientService _clientService;
        public frmClientConfirm()
        {
            InitializeComponent();
            _clientService = new ClientService();
            lbUser.Text = Globals.Userlogin.FullName.Trim();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool IsDouble(string inputData)
        {
            Regex regex = new Regex(@"^[0-9]*(?:\.[0-9]*)?$");
            Match m = regex.Match(inputData);
            return m.Success;
        }

        private bool IsEmail(string inputData)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match m = regex.Match(inputData);
            return m.Success;
        }

        private void btnChangePass_Click(object sender, EventArgs e)
        {
            txtEmail.Text = txtEmail.Text.Trim();
            txtPhone.Text = txtPhone.Text.Trim();

            if (string.IsNullOrEmpty(txtEmail.Text) || !IsEmail(txtEmail.Text))
            {
                dxErrorProvider1.SetError(txtEmail, "Vui lòng nhập địa chỉ Email.");

                dxErrorProvider1.SetIconAlignment(txtEmail, ErrorIconAlignment.BottomRight);
                txtEmail.BackColor = Color.Yellow;
                return;
            }
            else
            {
                dxErrorProvider1.ClearErrors();
                txtEmail.BackColor = Color.White;
            }

            if (string.IsNullOrEmpty(txtPhone.Text) || !IsDouble(txtPhone.Text))
            {
                dxErrorProvider1.SetError(txtPhone, "Vui lòng nhập số điện thoại liên hệ.");

                dxErrorProvider1.SetIconAlignment(txtPhone, ErrorIconAlignment.BottomRight);
                txtPhone.BackColor = Color.Yellow;
                return;
            }
            else
            {
                dxErrorProvider1.ClearErrors();
                txtPhone.BackColor = Color.White;
            }

            if (MessageBox.Show("Vui lòng xác nhận thông tin gửi STEM+: Tài khoản " + lbUser.Text + ". Email " + txtEmail.Text.Trim() + ". Điện thoại " + txtPhone.Text.Trim(), "Xác nhận thông tin", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.Yes)
            {
                return;
            }

            if (InternetHelper.CheckForInternetConnection())
            { // client
                var clientservice = new ClientService();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(GlobalSession.BaseApiUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var content = new FormUrlEncodedContent(new[]
                        {new KeyValuePair<string, string>("ClientId", Globals.Userlogin.ClientId +""),                            
                            new KeyValuePair<string, string>("Email", txtEmail.Text.Replace("'","")),
                            new KeyValuePair<string, string>("Phone", txtPhone.Text.Replace("'",""))                            
                        });
                    // HTTP POST
                    HttpResponseMessage response =
                        client.GetAsync("api/PublicApi/PostUpdateInfo?ClientId=" + Globals.Userlogin.ClientId + "&Email=" + txtEmail.Text + "&Phone=" + txtPhone.Text).Result;
                    {
                        if (!response.IsSuccessStatusCode)
                        {
                            XtraMessageBox.Show(this, @"Gửi thông tin cập nhật không thành công, vui lòng thử lại", @"Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            //UpdateChecker local
                            Client obj = _clientService.GetClientById(Globals.Userlogin.ClientId);
                            obj.Address = txtEmail.Text.Trim();
                            obj.Phone = txtPhone.Text.Trim();
                            _clientService.Update(obj);
                            this.Close();
                            XtraMessageBox.Show(this, @"Thông tin cập nhật đã được gửi tới STEM+", @"Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);                            
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show(this, @"Vui lòng kiểm tra internet khi cập nhật thông tin", @"Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
