using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Model;
using System.IO;
using Extensions;
using Services;
using DXWindows.Helper;
using System.Reflection;
using System.Net.Http.Headers;
using System.Net.Http;
using AppServices;
using log4net;

namespace DXWindows.UserControl
{
    public partial class ucLesson : DevExpress.XtraEditors.XtraUserControl
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public Lesson ObjLesson { get; set; }
        public bool IsNew { get; set; }
        private readonly IWinControlService _winControlService;
        private readonly ILessonService _lessonService;
        private readonly IDocumentService _documentService;

        public ucLesson()
        {
            InitializeComponent();
        }

        public ucLesson(Lesson objLesson)
        {
            InitializeComponent();
            _winControlService = new WinControlService();
            _lessonService = new LessonService();
            _documentService = new DocumentService();
            ObjLesson = objLesson;
            Init();
        }

        private void Init()
        {
            if (ObjLesson == null) return;
            lblTitle.Text = ObjLesson.Name.Length > 100 ? ObjLesson.Name.Substring(0, 100) : ObjLesson.Name;
            lblTitle.ToolTip = ObjLesson.Name;

            picShow.Visible = true;
            IsNew = ObjLesson.NeedUpdate || !ObjLesson.IsDownloaded;
            picNew.Visible = IsNew;
            lblNo.Text = "";// String.Format("{0:00}", ObjLesson.NoIndex);

            RenderDocumentList();

            if (string.IsNullOrEmpty(ObjLesson.NormalImage))
                picImage.EditValue = global::DXWindows.Properties.Resources.logo1;
            else
            {
                try
                {
                    picImage.EditValue = Image.FromFile(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\App_data\\" + ObjLesson.NormalImage.ToLower().MD5Hash(), true);
                }
                catch (Exception)
                {
                    picImage.EditValue = global::DXWindows.Properties.Resources.logo1;
                }
            }

            picShow.MouseHover += new EventHandler(_winControlService.picMouseHover3);
            picShow.MouseLeave += new EventHandler(_winControlService.picMouseLeave3);

            picPdf.MouseHover += new EventHandler(_winControlService.picMouseHover3);
            picPdf.MouseLeave += new EventHandler(_winControlService.picMouseLeave3);

            foreach (Control ctl in this.Controls)
            {
                ctl.MouseEnter += new EventHandler(_winControlService.ctlMouseHover);
                ctl.MouseLeave += new EventHandler(_winControlService.ctlMouseLeave);
            }
        }

        public void SetBackColor(Color color)
        {
            this.BackColor = color;

            //set pic border
            picImage.Properties.Appearance.BorderColor = this.BackColor;
            picShow.Properties.Appearance.BorderColor = this.BackColor;
            picPdf.Properties.Appearance.BorderColor = this.BackColor;
        }

        private SynDataViewModel RunAsyncLesson(int lessonId)
        {
            SynDataViewModel model = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(GlobalSession.BaseApiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string url = "api/PublicApi/GetUpdateDocByLessons?lessonId=" + lessonId; // HTTP GET
                HttpResponseMessage response =
                    client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    model = response.Content.ReadAsAsync<SynDataViewModel>().Result;
                }
            }
            return model;
        }

        bool CheckIsValid()
        {
            var frmDownload = typeof(frmDownload).Name;
            foreach (Form form in Application.OpenForms)
            {
                if (form.GetType().Name == frmDownload)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Tài liệu để hướng dẫn, tham khảo cho giáo viên
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblGiaoAn_Click(object sender, EventArgs e)
        {
            LoadDocument(true);
        }

        /// <summary>
        /// Tài liệu/ứng dụng chạy trình chiếu (flash,video,html execute)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblTrinhChieu_Click(object sender, EventArgs e)
        {
            LoadDocument();
        }

        void LoadDocument(bool isOpenGuide = false)
        {
            if (!CheckIsValid()) return;

            if (!ObjLesson.IsDownloaded)
            {
                if (InternetHelper.CheckForInternetConnection())
                {
                    if (MessageBox.Show("Tài liệu chưa được tải. Bạn có muốn tải xuống tài liệu này không ?", "Tải tài liệu", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    {
                        var entry = RunAsyncLesson(ObjLesson.LessonId);
                        if (entry != null)
                        {
                            var f = new frmDownload
                            {
                                CurrentLesson = entry.Lesson,
                                CurrentDocument = entry.Documents.ToList(),
                                OldLesson = ObjLesson,
                                IsOpenGuide = isOpenGuide,
                            };
                            f.Disposed += new EventHandler(frmDownload_Disposed);
                            f.Show();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Tài liệu chưa được tải. Vui lòng kết nối internet để tải tài liệu !", "Tải tài liệu", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                return;
            }

            if (ObjLesson.NeedUpdate)
            {
                if (InternetHelper.CheckForInternetConnection())
                {
                    if (MessageBox.Show("Bài học đã được cập nhật, Bạn có muốn cập nhật bài học này không ?",
                            "Tải tài liệu", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                            MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    {
                        var entry = RunAsyncLesson(ObjLesson.LessonId);
                        if (entry != null)
                        {
                            var f = new frmDownload
                            {
                                CurrentLesson = entry.Lesson,
                                CurrentDocument = entry.Documents.ToList(),
                                OldLesson = ObjLesson,
                                IsOpenGuide = isOpenGuide,
                            };
                            f.Disposed += new EventHandler(frmDownload_Disposed);
                            f.Show();
                        }
                    }
                    else
                    {
                        if (isOpenGuide)
                            ExecDocument(ObjLesson.GuideFile, ObjLesson.Name);
                        else
                            DisplayDocumentList();
                    }
                }
                else
                {
                    MessageBox.Show("Tài liệu đã được cập nhật. Vui lòng kết nối internet để tải tài liệu !",
                        "Tải tài liệu", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else
            {
                if (isOpenGuide)
                    ExecDocument(ObjLesson.GuideFile, ObjLesson.Name);
                else
                    DisplayDocumentList();
            }
        }

        void ExecDocument(string fileName, string title, bool allowPrint=true)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName))
                {
                    MessageBox.Show("Không tìm thấy tài liệu này?");
                    return;
                }

                var rootPath = Application.StartupPath + @"\App_data\";
                var selectedFileName = fileName;
                var extension = Path.GetExtension(selectedFileName);
                
                if (extension.Equals(Constants.ZipFileExtension, StringComparison.OrdinalIgnoreCase))
                {
                    var folderHashName = FileHeplper.GetHashByCurrentUser(Path.GetFileNameWithoutExtension(selectedFileName));
                    var folderPath = rootPath + folderHashName;
                    FileHeplper.UnlockFolder(folderPath);

                    var filePathToOpen = folderPath + @"\" + Constants.MainHtml;
                    if (File.Exists(filePathToOpen))
                    {
//                        _log.Debug("filePathToOpen: " + filePathToOpen);
                        var f = new frmViewWeb
                        {
                            FolderPath = folderPath,
                            FilePath = filePathToOpen,
                            Text = title,
                            CurrentLesson = ObjLesson
                        };
                        f.Show();
                    }
                    else
                    {
                        MessageBox.Show(@"Tài liệu không đúng định dạng", @"Error", MessageBoxButtons.OK);
                    }
//                    else
//                    {
//                        //check exe file
//                        filePathToOpen = folderPath + @"\" + Constants.MainExe;
//                        if (File.Exists(filePathToOpen))
//                        {
//                            _log.Debug("filePathToOpen: " + filePathToOpen);
//                            var f = new frmViewExe
//                            {
//                                FolderPath = folderPath,
//                                FilePath = filePathToOpen,
//                                Text = title,
//                                CurrentLesson = ObjLesson
//                            };
//                            f.Show();
//                        }
//                        else
//                        {
//                            MessageBox.Show(@"Tài liệu không đúng định dạng", @"Error", MessageBoxButtons.OK);
//                        }
//                    }
                }
                else
                {
                    var filePathToOpen = rootPath + fileName.Trim().ToLower().MD5Hash();
                    FileHeplper.Unlock(filePathToOpen);
                    var f = new frmViewDoc
                    {
                        AllowPrint = true,// allowPrint,    //kv: alway 
                        pdfViewer1 = { Visible = false },
                        axWindowsMediaPlayer1 = { Visible = false },
                        FilePath = filePathToOpen,
                        FileName = fileName,
                        Text = title,
                        CurrentLesson = ObjLesson
                    };

                    f.Show();
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                MessageBox.Show(@"Không thể mở tài liệu này", @"Error", MessageBoxButtons.OK);
            }
        }

        void RenderDocumentList()
        {
            //render list context menu
            ctmDocument.Items.Clear();
            if (ObjLesson.Documents == null || ObjLesson.Documents.Count() <= 1) return;
            foreach (var d in ObjLesson.Documents)
            {
                var item = new ToolStripMenuItem { Text = d.DocumentName, Tag = d.FileName };
                item.Click += new EventHandler(onClickToolStrip);
                ctmDocument.Items.Add(item);
            }
        }
        void DisplayDocumentList()
        {
            if (ctmDocument.Items.Count > 0)
                ctmDocument.Show(picShow, 3, 45);
            else
            {
                //execute first document
                var doc = ObjLesson.Documents.FirstOrDefault();
                if (doc != null)
                    ExecDocument(doc.FileName, doc.DocumentName);
            }
        }

        void frmDownload_Disposed(object sender, EventArgs e)
        {
            var f = (sender) as frmDownload;
            ObjLesson = _lessonService.GetLessonById(ObjLesson.LessonId);
            ObjLesson.Documents = _documentService.GetAllDocumentByLesson(ObjLesson.LessonId);
            Init();
            if (f.IsOpenGuide)
            {
                ExecDocument(ObjLesson.GuideFile, ObjLesson.Name);
            }
            else
            {
                DisplayDocumentList();
            }
        }

        private void onClickToolStrip(object sender, EventArgs e)
        {
            var c = (ToolStripMenuItem)sender;
            ExecDocument(c.Tag.ToString(), c.Text);
        }
    }
}
