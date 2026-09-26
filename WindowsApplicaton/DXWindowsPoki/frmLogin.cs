using DevExpress.LookAndFeel;
using DXWindows.Helper;
using Extensions;
using log4net;
using Model;
using Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace DXWindows
{
    public partial class frmLogin : Form
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IClientService _clientService;
        //private bool savepass = false;
        //private string passsave = "";
        BackgroundWorker bw = new BackgroundWorker();
        private static readonly string ProcessToEnd = Assembly.GetExecutingAssembly().GetName().Name;
        private static readonly string PostProcess = Application.StartupPath + @"\" + ProcessToEnd + ".exe";
        private static readonly string TeamViewPath = Application.StartupPath + @"\UltraViewerQS.exe";
        public const string UpdateCurrent = "Không có bản nâng cấp nào trong thời điểm này";
        public const string UpdateInfoError = "Có lỗi xảy ra khi tải bản nâng cấp phần mềm";
        public static List<string> Info = new List<string>();
        private Client _clientDefault;
        private bool _isConnectInternet;
        private string _currentVersion;
        public frmLogin()
        {

            InitializeComponent();

            DevExpress.Skins.SkinManager.EnableFormSkins();
            DevExpress.UserSkins.BonusSkins.Register();
            //UserLookAndFeel.Default.SetSkinStyle("Summer 2008");
            UserLookAndFeel.Default.SetSkinStyle("Office 2010 Silver");
                        
            lblStatusNet.Text = @"Đang kiểm tra internet...";
            btnLogin.Enabled = false;

            _clientService = new ClientService();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            bw = new BackgroundWorker();
            bw.DoWork += (o, ev) =>
            {
                BackgroundInit();
            };
            bw.RunWorkerCompleted += (o, ev) =>
            {
                BackgroundSucceed();
            };
            bw.RunWorkerAsync();
        }

        /// <summary>
        /// NOTE: không được set giá trị cho bất kỳ control nào trong này.
        /// Ở đây chỉ đơn thuần loading các tài nguyên
        /// </summary>
        void BackgroundInit()
        {
            try
            {
                _clientDefault = _clientService.GetDefaultClient();
                _currentVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                _isConnectInternet = InternetHelper.CheckForInternetConnection();
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }
        }

        void BackgroundSucceed()
        {
            try
            {
                if (_clientDefault != null && _clientDefault.SavePass)
                {
                    txtUserName.Text = _clientDefault.UserName;
                    txtPassword.Text = _clientDefault.PassWork;

                    //lock
                    txtUserName.Enabled = false;
                    txtPassword.Enabled = false;
                }
                lblVersion.Text = @"Version " + _currentVersion;
                btnLogin.Enabled = true;

                DisplayStatusNet();

                //load internet connection
                if (_isConnectInternet)
                {
                    if (CheckUpdate())
                    {
                        //if (MessageBox.Show(@"Đã có phiên bản mới của phần mềm, Bạn có muốn nâng cấp phần mềm không ?", "STEM+ Thông báo !", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
                        if (MessageBox.Show($"Đã có phiên bản mới. Hãy bấm OK để tiếp tục nâng cấp", "STEM+ Thông báo !", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information) == DialogResult.OK)
                        {
                            RunUpdate();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            Login();
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Login();
            }
        }

        void Login()
        {
            if (this.txtUserName.Text.Equals(String.Empty) || this.txtPassword.Text.Equals(String.Empty))
            {
                progressPanel1.Hide();
                MessageBox.Show(this, @"Vui lòng điền thông tin đăng nhập", @"Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUserName.Focus();
            }
            else
            {
                progressPanel1.Show();
                progressPanel1.BringToFront();

                int loginResult = 0;
                bw = new BackgroundWorker();
                bw.DoWork += (s, e) =>
                {
                    //_log.Debug("start worker");
                    loginResult = DoLogin();
                };


                bw.RunWorkerCompleted += (s, e) =>
                {
                    this.Invoke(new Action(() =>
                    {
                        DoLoginComplete(loginResult);
                    }));
                };
                bw.RunWorkerAsync();
            }
        }

        int DoLogin()
        {
            try
            {
                var pass = IsMD5(txtPassword.Text) ? txtPassword.Text : txtPassword.Text.MD5Hash();
                // first login
                var serverTime = DateTime.Now;
                if (_isConnectInternet)
                {
                    //chỉ build mã máy khi là lần đầu đăng nhập (nếu đã có DATA LOCAL, thì ko cần build mã máy)
                    var macIp = _clientDefault != null ? _clientDefault.MacIp : DISKHelper.GetDeviceId();
                    if (string.IsNullOrEmpty(macIp))
                        macIp = txtUserName.Text.MD5Hash();
                    if (macIp.Length > 50)
                        macIp = macIp.Substring(0, 50);

                    var clientService = new ClientService();

                    //init optional for HTTPS authentication
                    if (GlobalSession.BaseApiUrl.StartsWith("https"))
                        ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(GlobalSession.BaseApiUrl);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(
                            new MediaTypeWithQualityHeaderValue("application/json"));
                        // HTTP GET"?userName=" + txtUserName.Text + "&passWord=" + pass + "&macid=" + macip
                        var content = new FormUrlEncodedContent(new[]
                        {
                                    new KeyValuePair<string, string>("UserName", txtUserName.Text),
                                    new KeyValuePair<string, string>("PassWork", pass),
                                    new KeyValuePair<string, string>("MacId", macIp)
                                });

                        try
                        {
                            var response = client.PostAsync("api/PublicApi/PostLogin", content).Result;
                            var onlClient = response.Content.ReadAsAsync<Client>().Result;
                            if (onlClient != null && onlClient.ClientId > 0)
                            {
                                serverTime = onlClient.ServerTime;
                                if (onlClient.MacIp != macIp)
                                {
                                    return 5;
                                }

                                var localClient = clientService.GetClientById(onlClient.ClientId);
                                if (localClient == null)
                                {
                                    if (_clientDefault == null)
                                        clientService.Create(onlClient);
                                    else
                                    {
                                        return 1;
                                    }
                                }
                                else
                                {
                                    clientService.Update(onlClient);
                                }
                            }
                            else
                            {
                                return 2;
                            }
                        }
                        catch (Exception ex)
                        {
                            _log.Error(ex);
                        }
                    }
                }

                var modelCheck = _clientService.LoginToApp(txtUserName.Text, pass, true);
                if (modelCheck != null)
                {
                    if (modelCheck.EndDate == null || modelCheck.EndDate < serverTime)
                        return 3;
                    else
                        Globals.SetUserlogin(modelCheck);
                }
                else
                    return 4;
            }
            catch (Exception ex)
            {
                _log.Error("worker exception", ex);
            }
            return 0;
        }

        void DoLoginComplete(int loginResult)
        {
            //_log.Debug("worker complete, login result: " + loginResult);
            progressPanel1.Hide();

            if (loginResult == 0)
            {
                if (Globals.Userlogin == null || Globals.Userlogin.ClientId <= 0)
                {
                    MessageBox.Show(this, @"Đăng nhập không thành công. Vui lòng kiểm tra tài khoản/mật khẩu hoặc liên hệ với STEM+ để được giúp đỡ.", @"Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                this.Hide();
                frmMain f = new frmMain();
                f.Show();
            }
            else
            {
                switch (loginResult)
                {
                    case 1:
                        MessageBox.Show(this, @"Đã tồn tại tài khoản khác trên máy tính. Vui lòng liên hệ với STEM+ để được giúp đỡ.",
                                                    @"Thông báo", MessageBoxButtons.OK,
                                                    MessageBoxIcon.Warning);
                        break;
                    case 2:
                        MessageBox.Show(this, @"Tài khoản hoặc mật khẩu của bạn không hợp lệ hoặc bị khoá. Vui lòng liên hệ với STEM+ để được giúp đỡ.",
                                                @"Thông báo", MessageBoxButtons.OK,
                                                MessageBoxIcon.Warning);
                        break;
                    case 3:
                        MessageBox.Show(this,
                    @"Tài khoản của bạn đã hết hạn sử dụng. Vui lòng liên hệ với STEM+ để được giúp đỡ.",
                    @"Thông báo", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                        break;
                    case 4:
                        MessageBox.Show(this, @"Tên đăng nhập hoặc mật khẩu chưa đúng", @"Thông báo", MessageBoxButtons.OK,
               MessageBoxIcon.Warning);
                        break;
                    case 5:
                        MessageBox.Show(this, @"Tài khoản của bạn chỉ được sử dụng trên một máy tính. Vui lòng liên hệ với STEM+ để được giúp đỡ.",
                                           @"Thông báo", MessageBoxButtons.OK,
                                       MessageBoxIcon.Warning);
                        break;
                }
            }

        }

        private bool IsMD5(string input)
        {
            if (String.IsNullOrEmpty(input))
            {
                return false;
            }

            return Regex.IsMatch(input, "^[0-9a-fA-F]{32}$", RegexOptions.Compiled);
        }

        private bool CheckUpdate()
        {
            try
            {

                Info = Helper.UpdateChecker.GetUpdateInfo(GlobalSession.UpdateUrl, "UM.txt", Application.StartupPath + @"\",
                    1);

                if (Info == null)
                {
                    return false;
                }
                return decimal.Parse(Info[1].Replace(".", "")) > decimal.Parse(_currentVersion.Replace(".", ""));
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }

            return false;
        }

        private bool RunUpdate()
        {
            try
            {
                Info = Helper.UpdateChecker.GetUpdateInfo(GlobalSession.UpdateUrl, "UM.txt", Application.StartupPath + @"\",
                    1);

                if (Info == null)
                {
                    MessageBox.Show(UpdateInfoError);
                }
                else
                {
                    if (decimal.Parse(Info[1].Replace(".", "")) > decimal.Parse(_currentVersion.Replace(".", "")))
                    {
                        Helper.UpdateChecker.InstallUpdateRestart(Info[3], Info[4], "\"" + Application.StartupPath + "\\",
                            ProcessToEnd,
                            PostProcess, "updated", Application.StartupPath + @"\" + GlobalSession.Updater + ".exe");
                        Close();

                    }
                    else
                    {
                        MessageBox.Show(UpdateCurrent);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }
            return false;
        }

        private void lblResetPass_Click(object sender, EventArgs e)
        {
            frmResetPass f = new frmResetPass(txtUserName.Text);
            f.ShowDialog(this);
        }

        private void StatusNetClick(object sender, System.EventArgs e)
        {
            DisplayStatusNet();
        }

        private void TeamviewClick(object sender, System.EventArgs e)
        {
            //run open teamview
            Process.Start(TeamViewPath);
        }

        void DisplayStatusNet()
        {
            if (_isConnectInternet)
            {
                //ready connected internet
                picStatusNet.Image = Properties.Resources.status_net_1;
                lblStatusNet.Text = @"Đang kết nối internet";
            }
            else
            {
                //not ready
                picStatusNet.Image = Properties.Resources.status_net_2;
                lblStatusNet.Text = @"Không kết nối internet";
            }
        }
    }
}