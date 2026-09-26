using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Extensions;
using System.IO;
using System.Reflection;
using Model;
using Services;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using DXWindows.Helper;
using log4net;

namespace DXWindows
{
    public partial class frmDownload : DevExpress.XtraEditors.XtraForm
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private IDocumentService _documentService;
        private ILessonService _lessonService;
        private IDownloadDocumentService _downloadDocumentService;

        public string FileName { get; set; }
        /// <summary>
        /// Là tài liệu hướng dẫn
        /// </summary>
        public bool IsOpenGuide { get; set; }
        public Lesson CurrentLesson { get; set; }
        public List<Document> CurrentDocument { get; set; }
        public Lesson OldLesson { get; set; }

        private List<DownloadDocument> _downloadList;
        private const string GiaoAn = "Giáo án";
        private int _totalFile = 0;
        WebClient client;
        BackgroundWorker bw = new BackgroundWorker();

        //class DownloadDocument
        //{
        //    public int Id { get; set; }
        //    public string Title { get; set; }
        //    public string FileName { get; set; }
        //    /// <summary>
        //    /// 0-giáo án/ 1-file trình chiếu
        //    /// </summary>
        //    public int FileType { get; set; }
        //}

        public frmDownload()
        {
            InitializeComponent();
        }

        private void frmDownload_Load(object sender, EventArgs e)
        {
            LoadData();

            if (InternetHelper.CheckForInternetConnection())
            {
                Download();
                var clientService = new ClientService();
                clientService.IncreeClientLessonDownload(Globals.Userlogin.ClientId, CurrentLesson.LessonId);
                SynDownloadCount(Globals.Userlogin.ClientId, CurrentLesson.LessonId);
            }
            else //load local
            {
                SetText(lblStatus, "Không tìm thấy kết nối internet !");
            }
        }

        private void LoadData()
        {
            proStatus.Properties.Minimum = 0;
            proStatus.Properties.Maximum = 100;
            proStatus.EditValue = 0;

            _documentService = new DocumentService();
            _lessonService = new LessonService();
            _downloadDocumentService = new DownloadDocumentService();

            if (CurrentLesson != null)
            {
                lblTitle.Text = "* " + CurrentLesson.Name + " *";
                lblFileName.Text = "";
                lblStatus.Text = "";
            }
        }

        private void Download()
        {
            try
            {
                btnDownload.Enabled = false;
                var allDocumentByLesson = _documentService.GetAllDocumentByLesson(CurrentLesson.LessonId);
                _downloadList = new List<DownloadDocument>();
                DeleteFile(OldLesson.GuideFile);
                if (CurrentLesson.GuideFile != null)
                {
                    var obj = new DownloadDocument
                    {
                        LessonId = CurrentLesson.LessonId,
                        FileName = CurrentLesson.GuideFile,
                        Title = GiaoAn,
                    };
                    _downloadList.Add(obj);
                }

                if (CurrentDocument == null) CurrentDocument = new List<Document>();
                foreach (var document in allDocumentByLesson)
                {
                    if (!CurrentDocument.Exists(x => x.DocumentId == document.DocumentId))
                    {
                        DeleteFile(document.FileName);
                        _documentService.Delete(document.DocumentId);
                    }
                }

                foreach (var document in CurrentDocument)
                {
                    if (!string.IsNullOrEmpty(document.FileName))
                    {
                        var obj = new DownloadDocument
                        {
                            DocumentId = document.DocumentId,
                            FileName = document.FileName,
                            Title = document.DocumentName,
                        };
                        _downloadList.Add(obj);
                    }
                }

                _totalFile = _downloadList.Count;
                if (_totalFile > 0)
                {
                    Thread thDownload = new Thread(new ThreadStart(() =>
                    {
                        DownloadFile(_downloadList[0]);
                    }));
                    thDownload.SetApartmentState(ApartmentState.STA);
                    thDownload.Start();
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }
        }

        private bool DownloadFile(DownloadDocument objFile)
        {
            try
            {
                proStatus.Invoke(new Action(() => proStatus.Properties.Minimum = 0));
                proStatus.Invoke(new Action(() => proStatus.Properties.Maximum = 100));
                proStatus.Invoke(new Action(() => proStatus.EditValue = 0));

                var local = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                var urlOriginal = GlobalSession.BaseApiUrl + "images/" + objFile.FileName;

                var fileNameHash = objFile.FileName.Trim().ToLower().MD5Hash();
                var sFilePathRoot = Path.Combine(local + "\\App_data\\");
                var sFilePathToWriteFileTo = Path.Combine(sFilePathRoot, fileNameHash);
                var sFilePathToWriteFileToCopy = Path.Combine(sFilePathRoot, fileNameHash + "_tmp");
                if (File.Exists(sFilePathToWriteFileToCopy))
                    File.Delete(sFilePathToWriteFileToCopy);

                // Create an instance of WebClient
                client = new WebClient(); // Hookup DownloadFileCompleted Event 

                // Start the download and copy the file to c:\temp
                client.DownloadProgressChanged += (s, e) =>
                {
                    try
                    {
                        // Displays the operation identifier, and the transfer progress.
                        var msgTotal = string.Format("Tiến độ {0}/{1}", _totalFile - _downloadList.Count + 1, _totalFile);
                        var msgOne = e.ProgressPercentage < 100 ?
                            string.Format("{0}: {1}%...", objFile.Title, e.ProgressPercentage) :
                            string.Format("{0}: đang giải nén...", objFile.Title);

                        SetText(lblStatus, msgOne);
                        SetText(lblFileName, msgTotal);

                        if (proStatus.InvokeRequired)
                            proStatus.Invoke(new Action(() => proStatus.EditValue = e.ProgressPercentage));
                        else
                        {
                            proStatus.EditValue = e.ProgressPercentage;
                        }
                    }
                    catch (Exception ex)
                    {
                        _log.Error(ex);
                        SetText(lblStatus, "ERROR: Có lỗi xảy ra vui lòng thực hiện lại sau");
                        client.CancelAsync();
                    }
                };
                client.DownloadFileAsync(new Uri(urlOriginal), sFilePathToWriteFileToCopy);

                client.DownloadFileCompleted += (sender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        client = new WebClient();
                        return;
                    }

                    OnDownloadComplete(objFile, sFilePathRoot, sFilePathToWriteFileTo, sFilePathToWriteFileToCopy);
                };

                return true;
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                return false;
            }
        }

        void OnDownloadComplete(DownloadDocument objFile, string sFilePathRoot, string sFilePathToWriteFileTo, string sFilePathToWriteFileToCopy)
        {
            try
            {
                var extension = Path.GetExtension(objFile.FileName);
                if (extension.Equals(Constants.ZipFileExtension, StringComparison.OrdinalIgnoreCase))
                {
                    var fileNameWithoutExt = Path.GetFileNameWithoutExtension(objFile.FileName);
                    //remove older
                    var folderZipExtract = Path.Combine(sFilePathRoot, FileHeplper.GetHashByCurrentUser(fileNameWithoutExt));
                    if (DeleteFolder(folderZipExtract))
                    {
                        //make new folder (with hidden)
                        var dir = Directory.CreateDirectory(folderZipExtract);
                        dir.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
                    }

                    //unzip
                    FileHeplper.UnZip(sFilePathToWriteFileToCopy, folderZipExtract);

                    //lock folder
                    FileHeplper.LockFolder(folderZipExtract);

                    //remove downloaded file
                    File.Delete(sFilePathToWriteFileToCopy);
                }
                else
                {
                    //move file
                    if (!File.Exists(sFilePathToWriteFileTo))
                    {
                        File.Move(sFilePathToWriteFileToCopy, sFilePathToWriteFileTo);
                        File.SetAttributes(sFilePathToWriteFileTo, FileAttributes.Hidden);  //set HIDDEN
                    }
                    else
                        File.Delete(sFilePathToWriteFileToCopy);

                    FileHeplper.Lock(sFilePathToWriteFileTo);
                }

                //update tương ứng vào Lesson hoặc Document:
                if (objFile.LessonId > 0)
                {
                    //lesson
                    var les = _lessonService.GetLessonById(objFile.LessonId);
                    if (les != null)
                    {
                        les.GuideFile = objFile.FileName;
                        les.Reversion = CurrentLesson.Reversion;
                        les.NeedUpdate = false;
                        les.IsDownloaded = false;
                        _lessonService.Update(les);
                    }
                }
                else if (objFile.DocumentId > 0)
                {
                    //document
                    Document objDoc = _documentService.GetDocumentById(objFile.DocumentId);
                    if (!(objDoc == null || objDoc.DocumentId <= 0))
                    {
                        objDoc.IsDownloaded = true;
                        _documentService.Update(objDoc);
                    }
                    else
                    {
                        objDoc = CurrentDocument.FirstOrDefault(x => x.DocumentId == objFile.DocumentId);
                        objDoc.IsDownloaded = true;
                        _documentService.Create(objDoc);
                    }
                }

                _downloadDocumentService.Create(objFile);

                _downloadList.RemoveAt(0);

                //get next
                if (_downloadList.Count > 0)
                {
                    DownloadFile(_downloadList[0]);
                }
                else
                {
                    SetText(lblStatus, "Hoàn thành tải nội dung bài học !");
                    //SetText(lblFileName, "");
                    this.Invoke(new Action(() =>
                    {
                        proStatus.EditValue = 0;
                        _lessonService.UpdateDowloadStatus(CurrentLesson);
                        this.Close();
                    }));
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }
        }

        /// <summary>
        /// delete file by object(class) filename (NOT FILE ABSOLUTE NAME)
        /// </summary>
        /// <param name="objFile"></param>
        /// <returns></returns>
        public static void DeleteFile(string objFile)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(objFile)) return;
                string sFilePathToWriteFileTo = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\App_data\\", objFile.Trim().ToLower().MD5Hash());

                if (File.Exists(sFilePathToWriteFileTo))
                {
                    FileHeplper.Unlock(sFilePathToWriteFileTo);
                    File.Delete(sFilePathToWriteFileTo);
                }
            }
            catch (Exception e)
            {
                _log.Error(e);
            }
        }

        //remove older
        private bool DeleteFolder(string path)
        {
            try
            {
                if (Directory.Exists(path))
                {
                    FileHeplper.UnlockFolder(path);
                    Directory.Delete(path, true);
                }
                return true;
            }
            catch (Exception ex)
            {
                _log.Error(path, ex);
            }
            return false;
        }

        private delegate void SetTextCallback(Control ctl, string text);
        private void SetText(Control ctl, string text)
        {
            if (ctl.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { ctl, text });
            }
            else
                ctl.Text = text;
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (InternetHelper.CheckForInternetConnection())
            {
                Download();
                var clientService = new ClientService();
                clientService.IncreeClientLessonDownload(Globals.Userlogin.ClientId, CurrentLesson.LessonId);
                SynDownloadCount(Globals.Userlogin.ClientId, CurrentLesson.LessonId);
            }
            else //load local
            {
                SetText(lblStatus, "Không tìm thấy kết nối internet !");
            }

        }

        void SynDownloadCount(int clientId, int lessonId)
        {
            bw.DoWork += (s, e) =>
            {
                var clientservice = new ClientService();
                var views = clientservice.GetClientLessonView(clientId, lessonId);
                {
                    // client
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(GlobalSession.BaseApiUrl);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        // HTTP GET"?userName=" + txtUserName.Text + "&passWord=" + pass + "&macid=" + macip

                        var content = new FormUrlEncodedContent(new[]
                        {
                            new KeyValuePair<string, string>("LessonId", lessonId.ToString()),
                            new KeyValuePair<string, string>("ClientId", clientId.ToString()),
                            new KeyValuePair<string, string>("OpenCount", views.OpenCount.ToString()),
                            new KeyValuePair<string, string>("DownloadCount", views.DownloadCount.ToString())
                        });

                        HttpResponseMessage response =
                            client.PostAsync("api/PublicApi/PostClientLessonView", content).Result;
                    }
                }
            };
            bw.RunWorkerCompleted += (s, e) =>
            {

            };
            bw.RunWorkerAsync();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            client?.CancelAsync();
            this.Close();
        }

        private void frmDownload_FormClosing(object sender, FormClosingEventArgs e)
        {
            client?.CancelAsync();
        }
    }
}