using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DXWindows.Helper;
using Extensions;
using Gecko;
using log4net;
using Model;
using Newtonsoft.Json;
using Services;
using Screen = System.Windows.Forms.Screen;

namespace DXWindows
{
    public partial class frmViewWeb : DevExpress.XtraEditors.XtraForm
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public bool AllowPrint = false;
        public Lesson CurrentLesson { get; set; }
        public string FilePath { get; set; }
        public string FolderPath { get; set; }
        BackgroundWorker bw = new BackgroundWorker();

        public frmViewWeb()
        {
            InitializeComponent();
            this.Height = Screen.PrimaryScreen.WorkingArea.Height - 20;
            this.Width = Screen.PrimaryScreen.WorkingArea.Width - 20;
            this.Left = 10;
            this.Top = 10;

            lblWaiting.Left = Screen.PrimaryScreen.WorkingArea.Width / 2 - lblWaiting.Text.Length;
            lblWaiting.Top = Screen.PrimaryScreen.WorkingArea.Height / 2;

            geckoBrowser.Navigated += GeckoBrowser_Navigated;
            geckoBrowser.WindowClosed += GeckoBrowser_WindowClosed;
            geckoBrowser.DocumentCompleted += GeckoBrowser_DocumentCompleted;
        }

        private void GeckoBrowser_DocumentCompleted(object sender, Gecko.Events.GeckoDocumentCompletedEventArgs e)
        {
            var geckoWeb = (GeckoWebBrowser)sender;
            var txt = geckoWeb.Document.Body.InnerHtml;
            if (txt.Contains("Thank you for exiting the content."))
            {
                _log.Debug("gecko DocumentCompleted");
                this.Close();
            }
        }

        private void GeckoBrowser_WindowClosed(object sender, EventArgs e)
        {
            this.Close();
        }

        private void GeckoBrowser_Navigated(object sender, Gecko.GeckoNavigatedEventArgs e)
        {
            lblWaiting.Visible = false;
            geckoBrowser.Visible = true;
        }

        //        void frmViewWeb_KeyDown(object sender, KeyEventArgs e)
        //        {
        //            //if (e.Control && e.KeyCode == Keys.A || e.Control && e.KeyCode == Keys.P || e.Control && e.KeyCode == Keys.C ||
        //            //                      e.Control && e.KeyCode == Keys.S)
        //            if (e.Control)
        //            {
        //                e.Handled = true;
        //                MessageBox.Show("Vui lòng liên hệ với STEM+ nếu bạn muốn copy tài liệu này !",
        //                    "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //                return;
        //            }
        //        }

        private void frmViewWeb_Load(object sender, EventArgs e)
        {
            try
            {
                this.WindowState = FormWindowState.Maximized;
                //this.KeyPreview = true;
                //this.KeyDown += new KeyEventHandler(frmViewWeb_KeyDown);

                SetPictureHover();

                lblTitle.Text = CurrentLesson == null ? "" : CurrentLesson.Name;
                picturePrint.Visible = AllowPrint;

                var clientService = new ClientService();
                clientService.UpdateClientOpen(Globals.Userlogin.ClientId, CurrentLesson.LessonId);
                clientService.IncreeClientLessonDownload(Globals.Userlogin.ClientId, CurrentLesson.LessonId);

                if (InternetHelper.CheckForInternetConnection())
                {
                    SynDownloadCount(Globals.Userlogin.ClientId, CurrentLesson.LessonId);
                }

                geckoBrowser.Navigate(FilePath);
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void SynDownloadCount(int clientId, int lessonId)
        {
            bw.DoWork += (s, e) =>
            {
                var clientservice = new ClientService();
                var views = clientservice.GetClientLessonView(clientId, lessonId);
                // client
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(GlobalSession.BaseApiUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    // HTTP GET"?userName=" + txtUserName.Text + "&passWord=" + pass + "&macid=" + macip

                    views.LastUse = clientservice.GetClientUse(clientId, lessonId);

                    var content = new FormUrlEncodedContent(new[]
                        {
                            new KeyValuePair<string, string>("LessonId", lessonId.ToString()),
                            new KeyValuePair<string, string>("ClientId", clientId.ToString()),
                            new KeyValuePair<string, string>("OpenCount", views.OpenCount.ToString()),
                            new KeyValuePair<string, string>("DownloadCount", views.DownloadCount.ToString()),
                            new KeyValuePair<string, string>("LastUse", views.LastUse)
                        });

                    HttpResponseMessage response =
                        client.PostAsync("api/PublicApi/PostClientLessonView", content).Result;
                }
            };
            bw.RunWorkerCompleted += (s, e) =>
            {

            };
            bw.RunWorkerAsync();
        }

        private void picClose_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                this.Close();
            }
        }

        private void picMessage_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (CurrentLesson != null)
                {
                    frmFeedback f = new frmFeedback();
                    f.LessonId = CurrentLesson.LessonId;
                    f.LessonName = CurrentLesson.Name;
                    f.ShowDialog();
                }
            }
        }

        private void picMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void frmViewWeb_FormClosing(object sender, FormClosingEventArgs e)
        {
            FileHeplper.LockFolder(FolderPath);
        }

        private void picturePrint_MouseClick(object sender, MouseEventArgs e)
        {
            //            var frm = new MyPrint(this);
            //            frm.ShowDialog();
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //        private void picMaximize_Click(object sender, EventArgs e)
        //        {
        //            if (panHeader.Visible)
        //            {
        //                picMaximize.Image = DXWindows.Properties.Resources.max_button;
        //                panHeader.Visible = false;
        //                alertControl1.Show(this, "STEM+ - Thông báo !", "Bấm ESC để thoát khỏi chế độ toàn màn hình");
        //            }
        //            else
        //            {
        //                picMaximize.Image = DXWindows.Properties.Resources.mini_button;
        //            }
        //        }

        //
        //        private void PanelControl2_KeyPress(object sender, KeyPressEventArgs e)
        //        {
        //            _log.Debug(e.KeyChar);
        //            if (e.KeyChar == (char)Keys.Escape)
        //            {
        //                panHeader.Visible = true;
        //            }
        //        }
        //
        //        private void ChromeBrowser_KeyPress(object sender, KeyPressEventArgs e)
        //        {
        //            _log.Debug(e.KeyChar);
        //            if (e.KeyChar == (char)Keys.Escape)
        //            {
        //                panHeader.Visible = true;
        //            }
        //        }

        private void frmViewWeb_KeyPress(object sender, KeyPressEventArgs e)
        {
//            _log.Debug(e.KeyChar);
            if (e.KeyChar == (char)Keys.Escape)
            {
                pnHeader.Visible = true;
            }
        }
        private void frmViewWeb_MaximumSizeChanged(object sender, EventArgs e)
        {
            geckoBrowser.Visible = true;
        }

        private void picMouseHover(object sender, EventArgs e)
        {
            ((PictureEdit)sender).BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
        }

        private void picMouseLeave(object sender, EventArgs e)
        {
            ((PictureEdit)sender).BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
        }

        void SetPictureHover()
        {
            picClose.MouseHover += new EventHandler(picMouseHover);
            picClose.MouseLeave += new EventHandler(picMouseLeave);

            picMinimize.MouseHover += new EventHandler(picMouseHover);
            picMinimize.MouseLeave += new EventHandler(picMouseLeave);

            //            picMaximize.MouseHover += new EventHandler(picMouseHover);
            //            picMaximize.MouseLeave += new EventHandler(picMouseLeave);

            picturePrint.MouseHover += new EventHandler(picMouseHover);
            picturePrint.MouseLeave += new EventHandler(picMouseLeave);

            picMessage.MouseHover += new EventHandler(picMouseHover);
            picMessage.MouseLeave += new EventHandler(picMouseLeave);
        }
    }
}