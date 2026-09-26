using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DXWindows.Helper;
using Extensions;
using log4net;
using Model;
using Services;

namespace DXWindows
{
    /// <summary>
    /// https://social.msdn.microsoft.com/Forums/windows/en-US/94a67da1-8143-4e37-86c0-4deab0d1ec93/c-run-exe-from-a-form-and-keep-the-exe-within-the-boundaries-of-that-original-form?forum=winforms
    /// </summary>
    public partial class frmViewExe : DevExpress.XtraEditors.XtraForm
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public bool AllowPrint = false;
        public Lesson CurrentLesson { get; set; }
        public string FilePath { get; set; }
        public string FolderPath { get; set; }
        BackgroundWorker bw = new BackgroundWorker();

        [DllImport("user32.dll", SetLastError = true)]
        private static extern long SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern long SetWindowPos(IntPtr hwnd, long hWndInsertAfter, long x, long y, long cx, long cy, long wFlags);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool MoveWindow(IntPtr hwnd, int x, int y, int cx, int cy, bool repaint);

        IntPtr appWin;

        public frmViewExe()
        {
            InitializeComponent();
            this.Height = Screen.PrimaryScreen.WorkingArea.Height - 20;
            this.Width = Screen.PrimaryScreen.WorkingArea.Width - 20;
            this.Left = 10;
            this.Top = 10;

            lblWaiting.Left = Screen.PrimaryScreen.WorkingArea.Width / 2 - lblWaiting.Text.Length;
            lblWaiting.Top = Screen.PrimaryScreen.WorkingArea.Height / 2;
        }

        private void frmViewExe_Load(object sender, EventArgs e)
        {
            try
            {
                this.WindowState = FormWindowState.Maximized;

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

                LoadExe();
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void LoadExe()
        {
            var processStartInfo = new ProcessStartInfo(FilePath) {WindowStyle = ProcessWindowStyle.Maximized};
            var p = Process.Start(processStartInfo);
            Thread.Sleep(1000); // Allow the process to open it's window
            appWin = p.MainWindowHandle;
            // Put it into this form
            SetParent(appWin, pnView.Handle);
            // Move the window to overlay it on this window
            MoveWindow(appWin, 0, 20, pnView.Width / 2, pnView.Height, true);
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
        private void picturePrint_MouseClick(object sender, MouseEventArgs e)
        {
            //            var frm = new MyPrint(this);
            //            frm.ShowDialog();
        }
        private void picMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void frmViewExe_FormClosing(object sender, FormClosingEventArgs e)
        {
//            _log.Debug("closed");
            FileHeplper.LockFolder(FolderPath);
        }
        private void frmViewExe_Resize(object sender, EventArgs e)
        {
            if (this.appWin != IntPtr.Zero)
            {
                MoveWindow(appWin, this.Width / 2, 0, this.Width, this.Height, true);
            }
        }
        private void picClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmViewExe_KeyPress(object sender, KeyPressEventArgs e)
        {
//            _log.Debug(e.KeyChar);
            if (e.KeyChar == (char)Keys.Escape)
            {
                pnHeader.Visible = true;
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

            picturePrint.MouseHover += new EventHandler(picMouseHover);
            picturePrint.MouseLeave += new EventHandler(picMouseLeave);

            picMessage.MouseHover += new EventHandler(picMouseHover);
            picMessage.MouseLeave += new EventHandler(picMouseLeave);
        }
    }
}