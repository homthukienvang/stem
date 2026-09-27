using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DXWindows.Helper;
using Extensions;
using log4net;
using Model;
using Services;

namespace DXWindows
{
    public partial class frmViewDoc : DevExpress.XtraEditors.XtraForm
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public bool AllowPrint = false;
        public Lesson CurrentLesson { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        /// <summary>
        /// sẽ được ưu tiên thể hiện nến có
        /// </summary>
        public string CustomTitle { get; set; }
        BackgroundWorker bw = new BackgroundWorker();
        public frmViewDoc()
        {
            InitializeComponent();

            this.Height = Screen.PrimaryScreen.WorkingArea.Height - 20;
            this.Width = Screen.PrimaryScreen.WorkingArea.Width - 20;
            this.Left = 10;
            this.Top = 10;
        }

        void frmViewDoc_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.Control && e.KeyCode == Keys.A || e.Control && e.KeyCode == Keys.P || e.Control && e.KeyCode == Keys.C ||
            //                      e.Control && e.KeyCode == Keys.S)
            if (e.Control)
            {
                e.Handled = true;
                MessageBox.Show("Vui lòng liên hệ với STEM+ nếu bạn muốn copy tài liệu này !",
                    "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void frmViewDoc_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(frmViewDoc_KeyDown);

            SetPictureHover();

            lblTitle.Text = CustomTitle ?? (CurrentLesson == null ? "" : CurrentLesson.Name);
            try
            {
                var clientService = new ClientService();
                clientService.UpdateClientOpen(Globals.Userlogin.ClientId, CurrentLesson.LessonId);
                clientService.IncreeClientLessonDownload(Globals.Userlogin.ClientId, CurrentLesson.LessonId);

                if (InternetHelper.CheckForInternetConnection())
                {
                    SynDownloadCount(Globals.Userlogin.ClientId, CurrentLesson.LessonId);
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }
            picturePrint.Visible = AllowPrint;

            pdfViewer1.ZoomMode = DevExpress.XtraPdfViewer.PdfZoomMode.FitToWidth;
            //axShockwaveFlash1.Zoom

            //display
            var extension = Path.GetExtension(FileName);
            //            _log.Debug("FilePath: " + FilePath + ", FileName: " + FileName + ", extension: " + extension);
            if (string.IsNullOrEmpty(extension)) return;

            if (extension.Equals(Constants.PdfFileExtension, StringComparison.OrdinalIgnoreCase))
            {
                pdfViewer1.Enabled = true;
                pdfViewer1.Visible = true;
                pdfViewer1.DocumentFilePath = FilePath;
            }
            else if (extension.Equals(Constants.Mp3FileExtension, StringComparison.OrdinalIgnoreCase) ||
                     extension.Equals(Constants.Mp4FileExtension, StringComparison.OrdinalIgnoreCase) ||
                     extension.Equals(Constants.AviFileExtension, StringComparison.OrdinalIgnoreCase))
            {
                axWindowsMediaPlayer1.Enabled = true;
                axWindowsMediaPlayer1.URL = FilePath;
                axWindowsMediaPlayer1.Show();
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

        private void pdfViewer1_PopupMenuShowing(object sender, DevExpress.XtraPdfViewer.PdfPopupMenuShowingEventArgs e)
        {
            //if (!AllowPrint)
            {
                try
                {
                    string txt = "Copy,Select Tool,Print...,Select All,Document properties...";
                    foreach (DevExpress.XtraBars.BarItemLink itemLink in e.Menu.ItemLinks)
                    {
                        if (txt.Contains(itemLink.Caption))
                        {
                            itemLink.Visible = false;
                        }
                    }
                }
                catch (Exception)
                {

                }
            }
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
                    var f = new frmFeedback
                    {
                        LessonId = CurrentLesson.LessonId,
                        LessonName = CurrentLesson.Name
                    };
                    f.ShowDialog();
                }
            }
        }

        private void picMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void frmViewDoc_FormClosing(object sender, FormClosingEventArgs e)
        {
            FileHeplper.Lock(FilePath);
        }

        private void picturePrint_MouseClick(object sender, MouseEventArgs e)
        {
            var frm = new MyPrint(this);
            frm.ShowDialog();
        }

        private void picMinimize_Click_1(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void picMaximize_Click(object sender, EventArgs e)
        {
            if (panHeader.Visible == true)
            {
                picMaximize.Image = DXWindows.Properties.Resources.max_button;
                panHeader.Visible = false;
                if (pdfViewer1.Visible == true)
                {
                    pdfViewer1.Focus();
                }
                alertControl1.Show(this, "STEM+ - Thông báo !", "Bấm ESC để thoát khỏi chế độ toàn màn hình");
            }
            else
            {
                picMaximize.Image = DXWindows.Properties.Resources.mini_button;
            }
        }

        private void frmViewDoc_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                panHeader.Visible = true;
            }
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

            picMaximize.MouseHover += new EventHandler(picMouseHover);
            picMaximize.MouseLeave += new EventHandler(picMouseLeave);

            picturePrint.MouseHover += new EventHandler(picMouseHover);
            picturePrint.MouseLeave += new EventHandler(picMouseLeave);

            picMessage.MouseHover += new EventHandler(picMouseHover);
            picMessage.MouseLeave += new EventHandler(picMouseLeave);
        }
    }
}