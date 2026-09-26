using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraTreeList;
using DXWindows.Helper;
using DXWindows.Properties;
using Extensions;
using Model;
using Services;

namespace DXWindows.UserControl
{
    public partial class ucUpdateLesson : XtraUserControl
    {
        private readonly ILessonService _lessonService;
        private readonly ISubjectService _subjectService;
        private readonly IRoomService _roomService;
        private readonly IDocumentService _documentService;
        private readonly ISysConfigService _configService;
        public Lesson CurrenLesson;
        public List<Document> CurrentDocumentsLesson;

        private List<string> _fileList;
        private List<string> _fileListTitle;
        private const string DataVersionKey = "DATAVERSION";
        public ucUpdateLesson()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            _configService = new SysConfigService();
            _lessonService = new LessonService();
            _subjectService = new SubjectService();
            _roomService = new RoomService();
            _documentService = new DocumentService();
            CurrentDocumentsLesson = new List<Document>();
        }

        public void InitData()
        {
            btnDownload.Enabled = false;
            lblLessonName.Text = "";
            lblDes.Text = "";
            lbNew.Visible = false;
            var subs = _subjectService.GetAllSubject().ToList();
            var rooms = _roomService.GetAllRoom().ToList();
            var lesson = _lessonService.GetLessonByClient().ToList();
            treeList1.Nodes.Clear();
            this.treeList1.BeginUnboundLoad();
            var nid = 0;
            foreach (var sub in subs)
            {
                var sub1 = sub;
                var rooms1 = rooms.Where(x => x.SubjectId == sub1.SubjectId).ToList();
                if (rooms1.Any())
                {
                    var hadRoom = false;
                    for (int r = 0; r < rooms1.Count(); r++)
                    {
                        hadRoom = lesson.Any(x => x.RoomId == rooms1[r].RoomId);
                        if (hadRoom)
                        {
                            break;
                        }
                    }

                    if (!hadRoom) continue;
                    this.treeList1.Nodes.Add(new object[] { sub.Name, null });
                    var rid = 0;
                    for (int r = 0; r < rooms1.Count(); r++)
                    {
                        if (lesson.Any(x => x.RoomId == rooms1[r].RoomId))
                        {
                            this.treeList1.Nodes[nid].Nodes.Add(new object[] { rooms1[r].Name, null });
                            var lesson1 = lesson.Where(x => x.RoomId == rooms1[r].RoomId).ToList();
                            foreach (var t in lesson1)
                            {
                                this.treeList1.Nodes[nid].Nodes[rid].Nodes.Add(new object[] { (t.IsDownloaded ? "" : "[New]") + t.Name, t.LessonId });
                            }
                            rid++;
                        }

                    }
                    nid++;
                }
            }
            this.treeList1.EndUnboundLoad();
        }

        public string GetVersion()
        {
            var syscofig = _configService.GetSysConfigByKey(DataVersionKey);
            if (syscofig != null)
            {
                return syscofig.Value;
            }
            return "0";
        }

        public void UpdateVersion()
        {
            var syscofig = _configService.GetSysConfigByKey(DataVersionKey);
            //Globals.Userlogin.ClientId;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(GlobalSession.BaseApiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // HTTP GET
                HttpResponseMessage response =
                    client.GetAsync("api/PublicApi/GetLastVersion").Result;
                {
                    int version = response.Content.ReadAsAsync<int>().Result;
                    if (syscofig != null)
                    {
                        syscofig.Value = version.ToString();
                        _configService.Update(syscofig);
                    }
                    else
                    {
                        _configService.Create(new SysConfig()
                        {
                            Code = DataVersionKey,
                            Name = DataVersionKey,
                            Value = version.ToString()
                        });
                    }
                }
            }
        }

        public void GetNewData()
        {
            if (InternetHelper.CheckForInternetConnection())
            {
                var updated = RunAsync();
                if (updated)
                {
                    InitData();
                }
            }
            else
            {
                XtraMessageBox.Show(this, @"Không thể kết nối internet. Vui lòng kết nối internet để cập nhật dữ liệu",
                    @"Thông báo", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private bool RunAsync()
        {
            lblStatus.Text = "Đang cập nhật 25 % dữ liệu...";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(GlobalSession.BaseApiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var url = $"api/PublicApi/GetUpdateData?clientId={Globals.Userlogin.ClientId}&version={GetVersion()}"; // HTTP GET
                HttpResponseMessage response = client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    var model = response.Content.ReadAsAsync<SynDataViewModel>().Result;
                    if (model != null)
                    {
                        if (model.Subjects != null)
                        {
                            foreach (var subject in model.Subjects)
                            {
                                var sub = _subjectService.GetSubjectById(subject.SubjectId);
                                if (sub == null)
                                {
                                    _subjectService.Create(subject);
                                }
                                else
                                {
                                    _subjectService.Update(subject);
                                }
                            }
                        }
                        if (model.Rooms != null)
                        {
                            foreach (var room in model.Rooms)
                            {
                                var sub = _roomService.GetRoomById(room.RoomId);
                                if (sub == null)
                                {
                                    _roomService.Create(room);
                                }
                                else
                                {
                                    _roomService.Update(room);
                                }
                            }
                        }
                        if (model.Lessons != null)
                        {
                            foreach (var entry in model.Lessons)
                            {
                                var sub = _lessonService.GetLessonById(entry.LessonId);
                                if (sub == null)
                                {
                                    entry.IsDownloaded = false;
                                    entry.IsDeleted = false;
                                    _lessonService.Create(entry);
                                }
                                else
                                {
                                    entry.IsDownloaded = false;
                                    _lessonService.Update(entry);
                                }
                            }
                        }
                        if (model.Documents != null)
                        {
                            CurrentDocumentsLesson = model.Documents.ToList();
                        }
                        if (model.ClientLessons != null)
                        {
                            _lessonService.UpdateClientLesson(model.ClientLessons, Globals.Userlogin.ClientId);
                        }
                    }
                }
            }
            // room
            lblStatus.Text = "Đang cập nhật 50 % dữ liệu...";
            UpdateVersion();

            lblStatus.Text = "";
            return true;
        }

        private IEnumerable<Document> GetAllDocumentByLessonFromTemp()
        {
            return CurrentDocumentsLesson.Where(x => x.LessonId == CurrenLesson.LessonId);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            btnDownload.Enabled = false;
            SetSEnableTree(false);
            var documentInLession = GetAllDocumentByLessonFromTemp().ToList();
            _fileList = new List<string>();
            _fileListTitle = new List<string>();
            if (CurrenLesson.GuideFile != null)
            {
                _fileList.Add(CurrenLesson.GuideFile);
                _fileListTitle.Add("Giáo án");
            }

            foreach (var document in documentInLession)
            {
                if (document.FileName != null)
                {
                    _fileList.Add(document.FileName);
                    _fileListTitle.Add(document.DocumentName);
                }

            }
            DownloadFile(_fileList.FirstOrDefault(), _fileListTitle.FirstOrDefault());

            treeList1.Selection[0].SetValue(0, lblLessonName.Text);

            //  m_oWorker.DoWork += (s, a) => { DownloadFile(_fileList.FirstOrDefault(), _fileListTitle.FirstOrDefault()); };
            //   m_oWorker.RunWorkerAsync();
            return;

            //foreach (var document in documentInLession)
            //{
            //    //Start the async operation here
            //    m_oWorker.DoWork += (s, a) => { download = DownloadFile(document.FileName, document.DocumentName); };
            //    m_oWorker.RunWorkerAsync();
            //}

            //btnDownload.Enabled = false;

            ////  _frmsyn.Hide();
            //if (download)
            //{

            //    MessageBox.Show("Cập nhật các bài học từ hệ thống thành công."); // backgroundWorker1.RunWorkerAsync();
            //    btnDownload.Enabled = false;
            //    lblLessonName.Text = "";
            //    lblDes.Text = "";
            //    lbNew.Visible = false;
            //}
            //else
            //{
            //    MessageBox.Show("Bài học chưa download đầy đủ. Vui lòng thực hiện lại.");
            //}
        }

        private delegate void SetStatusCallback(bool show);

        private void SetStatus(bool show)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.lbNew.InvokeRequired)
            {
                SetStatusCallback d = new SetStatusCallback(SetStatus);
                this.Invoke(d, new object[] { show });
            }
            else
            {
                this.lbNew.Visible = show;
            }
        }
        private delegate void SetEnableTreeCallback(bool show);

        private void SetSEnableTree(bool show)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.lbNew.InvokeRequired)
            {
                SetEnableTreeCallback d = new SetEnableTreeCallback(SetStatus);
                this.Invoke(d, new object[] { show });
            }
            else
            {
                this.lbNew.Enabled = show;
            }
        }

        private delegate void SetTextCallback(string text);

        private void SetText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.lblStatus.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.lblStatus.Text = text;
            }
        }

        private bool DownloadFile(string fileName, string docName)
        {
            string url = GlobalSession.BaseApiUrl + "images/" + fileName;
            string sFilePathToWriteFileTo =
                Path.Combine(
                    Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\App_data\\",
                    fileName.Trim().ToLower().MD5Hash());
            string sFilePathToWriteFileToCOpy =
                Path.Combine(
                    Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\App_data\\",
                    fileName.Trim().ToLower().MD5Hash() + "_tmp");
            if (File.Exists(sFilePathToWriteFileTo))
            {
                _fileList.RemoveAt(0);
                _fileListTitle.RemoveAt(0);
                if (_fileList.Count > 0)
                {
                    DownloadFile(_fileList.FirstOrDefault(), _fileListTitle.FirstOrDefault());
                }
                else
                {
                    SetSEnableTree(true);
                    SetStatus(false);
                    SetText("Download thành công ");
                    var documentInLession = GetAllDocumentByLessonFromTemp().ToList();
                    _documentService.DeleteByLeson(CurrenLesson.LessonId);
                    foreach (var document in documentInLession)
                    {
                        _documentService.Create(document);
                    }

                    _lessonService.UpdateDowloadStatus(CurrenLesson);
                }
                return true;
            }
            //_frmsyn.labelInformation.Text = "Đang tải " + docName + "...";
            //_frmsyn.Show();
            // Create an instance of WebClient
            WebClient client = new WebClient(); // Hookup DownloadFileCompleted Event 

            // Start the download and copy the file to c:\temp
            //client.DownloadFile(new Uri(url), sFilePathToWriteFileToCOpy);
            //System.IO.File.Move(sFilePathToWriteFileToCOpy, sFilePathToWriteFileTo);
            client.DownloadProgressChanged += (s, e) =>
            {
                // Displays the operation identifier, and the transfer progress.
                string msg = string.Format("{0}    downloaded {1} of {2} bytes. {3} % complete...",
                    (string)e.UserState,
                    e.BytesReceived,
                    e.TotalBytesToReceive,
                    e.ProgressPercentage);
                string msg1 = string.Format("Còn {0} tài liệu chưa download.\n Đang Download '{2}' \n {3} % ...",
                    _fileList.Count,
                    e.BytesReceived,
                    docName, e.ProgressPercentage);
                SetText(msg1); //m_oWorker.ReportProgress(e.ProgressPercentage);
            };
            client.DownloadFileAsync(new Uri(url), sFilePathToWriteFileToCOpy);

            client.DownloadFileCompleted += (sender, args) =>
            {
                SetText("");
                if (_fileList.Count > 0)
                {
                    _fileList.RemoveAt(0);
                    _fileListTitle.RemoveAt(0);
                }

                File.Move(sFilePathToWriteFileToCOpy, sFilePathToWriteFileTo);
                if (_fileList.Count > 0)
                {
                    DownloadFile(_fileList.FirstOrDefault(), _fileListTitle.FirstOrDefault());
                    //m_oWorker.DoWork +=
                    //    (s, a) => { DownloadFile(_fileList.FirstOrDefault(), _fileListTitle.FirstOrDefault()); };
                }
                else
                {
                    SetText("Download thành công ");
                    SetSEnableTree(true);
                    SetStatus(false);
                    var documentInLession = GetAllDocumentByLessonFromTemp().ToList();
                    _documentService.DeleteByLeson(CurrenLesson.LessonId);
                    foreach (var document in documentInLession)
                    {
                        _documentService.Create(document);
                    }

                    _lessonService.UpdateDowloadStatus(CurrenLesson);
                }
            };
            return true;
        }

        private void treeList1_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
        {
            var tree = sender as TreeList;
            var idlesson = tree.Selection[0].GetDisplayText(1);
            this.tileGroup1.Items.Clear();
            if (!string.IsNullOrEmpty(idlesson))
            {
                CurrenLesson = _lessonService.GetLessonById(Convert.ToInt32(idlesson));
                lblLessonName.Text = CurrenLesson.Name;
                lblDes.Text = CurrenLesson.Description;

                lbNew.Visible = !CurrenLesson.IsDownloaded;
                btnDownload.Enabled = !CurrenLesson.IsDownloaded;

                var docs = GetAllDocumentByLessonFromTemp().ToList();
                // _documentService.GetAllDocumentByLesson(Convert.ToInt32(idlesson));
                foreach (var doc in docs)
                {
                    var tileItem1 = new TileItem();
                    TileItemElement tileItemElement1 = new TileItemElement();
                    var extension = Path.GetExtension(doc.FileName);
                    if (extension == null)
                    {
                        extension = "";
                    }
                    if (extension.Equals(Constants.FlashExtension, StringComparison.OrdinalIgnoreCase))
                    {
                        tileItemElement1.Image = Resources.flash_icon;
                    }

                    if (extension.Equals(Constants.PdfFileExtension, StringComparison.OrdinalIgnoreCase))
                    {
                        tileItemElement1.Image = Resources.Adobe_PDF_Document_icon;
                    }
                    if (extension.Equals(Constants.Mp3FileExtension, StringComparison.OrdinalIgnoreCase) ||
                        extension.Equals(Constants.Mp4FileExtension, StringComparison.OrdinalIgnoreCase) ||
                        extension.Equals(Constants.AviFileExtension, StringComparison.OrdinalIgnoreCase))
                    {
                        tileItemElement1.Image = Resources.Video_File_icon;
                    }
                    tileItemElement1.Text = doc.DocumentName;
                    tileItem1.Elements.Add(tileItemElement1);
                    this.tileGroup1.Items.Add(tileItem1);
                }

                //if (CurrentLesson.IsDownloaded)
                //{
                //    btnDownload.Enabled = false;
                //}
                //else
                //{
                //    btnDownload.Enabled = true;
                //}
            }
            else
            {
                btnDownload.Enabled = false;
                lblLessonName.Text = "";
                lblDes.Text = "";
                lbNew.Visible = false;
            }
        }

        private void ucUpdateLesson_Load(object sender, EventArgs e)
        {
            GetNewData();
        }

        private void tileControl1_ItemClick(object sender, TileItemEventArgs e)
        {
            MessageBox.Show("Để mở tài liệu bạn vui lòng mở trong danh sách bài học");
        }
    }
}