using AppServices;
using DevExpress.LookAndFeel;
using DevExpress.XtraEditors;
using DXWindows.Helper;
using DXWindows.UserControl;
using Extensions;
using Gecko;
using log4net;
using Model;
using Model.Model;
using Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Exception = System.Exception;
using Screen = System.Windows.Forms.Screen;

namespace DXWindows
{
    public partial class frmMain : XtraForm
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private ILessonService _lessonService;
        private IRoomService _roomService;
        private INewsService _newService;
        private ICommentService _commentService;
        private IWinControlService _wincontrolService;
        private ICommentAnswerService _commentAsService;
        private IClientService _clientService;
        private IAdvertisementService _advertisementService;
        private ISubjectService _subjectService;
        private IDocumentService _documentService;
        private IDownloadDocumentService _downloadDocumentService;

        private const int NewsNotifMessageId = 1;
        private const int NewsGuildId = 2;
        private const int NewsSoftwareInfoId = 3;
        private const int NewsHotlineId = 4;

        /// <summary>
        /// TTL by Day of files download! = 70 days
        /// </summary>
        private const int DownloadFileTtlByDays = 70;

        private static string processToEnd = Assembly.GetExecutingAssembly().GetName().Name;
        private static string postProcess = Application.StartupPath + @"\" + processToEnd + ".exe";
        private static readonly string TeamViewPath = Application.StartupPath + @"\UltraViewerQS.exe";
        public const string updateCurrent = "Không có bản nâng cấp nào trong thời điểm này";
        public const string updateInfoError = "Có lỗi xảy ra khi tải bản nâng cấp phần mềm";
        public static List<string> info = new List<string>();

        private string CurrentVersion;
        public frmMain()
        {
            try
            {
                if (Globals.Userlogin == null || Globals.Userlogin.ClientId <= 0)
                {
                    _log.Error("User not login.");
                    Application.Exit();
                }

                _newService = new NewsService();
                _lessonService = new LessonService();
                _roomService = new RoomService();
                _commentService = new CommentService();
                _wincontrolService = new WinControlService();
                _commentAsService = new CommentAnswerService();
                _clientService = new ClientService();
                _advertisementService = new AdvertisementService();
                _subjectService = new SubjectService();
                _documentService = new DocumentService();
                _downloadDocumentService = new DownloadDocumentService();

                InitializeComponent();

                DevExpress.Skins.SkinManager.EnableFormSkins();
                DevExpress.UserSkins.BonusSkins.Register();
                //UserLookAndFeel.Default.SetSkinStyle("Caramel");
                UserLookAndFeel.Default.SetSkinStyle("Office 2010 Silver");

                this.Bounds = Screen.PrimaryScreen.WorkingArea;

                SetPictureHover();

                //load version label
                CurrentVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                lblVersion.Text += CurrentVersion;

                //load GeckoFx45
                Xpcom.EnableProfileMonitoring = false;
                Xpcom.Initialize("Firefox");
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            try
            {
                string lstUser = ConfigurationManager.AppSettings["AdminUser"] == null
                    ? ""
                    : ConfigurationManager.AppSettings["AdminUser"].ToLower();
                if (lstUser.Contains(Globals.Userlogin.UserName.Trim().ToLower()))
                {
                    btnSynsLesson.Visible = true;
                }
                else
                {
                    btnSynsLesson.Visible = false;
                }

                UpdateDatabaseStruct();

                RemoveExpiredDownloadFiles();

                GetNewData(true);
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }
        }

        private void UpdateDatabaseStruct()
        {
            if (_commentService.CheckTableCA())
            {
                _commentService.UpdateTableComment();
                _commentService.CreateTableCA();
            }
            if (_advertisementService.CheckTableAdv())
            {
                _advertisementService.CreateTableAdv();
            }

            //version 2.0.1.0 (thêm bảng quản lý tài nguyên download)
            if (_downloadDocumentService.CheckAndCreateTable())
            {
                //kiểm tra nếu là lần đầu update lên phiên bản 2.0.1.0 thì xóa toàn bộ file trừ .sdf                
                ClearFilesOlder();

                //đồng thời reset thông số download trong DB
                _lessonService.ResetDownloadStatus();
                _documentService.ResetDownloadStatus();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromLoad">from load or not</param>
        public void GetNewData(bool fromLoad)
        {
            if (InternetHelper.CheckForInternetConnection())
            {
                RunAsync(fromLoad);
            }
            else //load local
            {
                FormLoad(fromLoad);
            }
        }

        private void RemoveExpiredDownloadFiles()
        {
            var expriredDownloadDocuments = _downloadDocumentService.GetAllExpired(DownloadFileTtlByDays);
            if (!expriredDownloadDocuments.Any()) return;

            BackgroundWorker bw = new BackgroundWorker();
            frmWaitForm f = new frmWaitForm();
            bw.WorkerReportsProgress = true;
            bw.ProgressChanged += (s, e) =>
            {
                var message = e.UserState.ToString();
                var percent = e.ProgressPercentage;
                if (percent > 100)
                    percent = 100;

                this.Invoke(new Action(() =>
                {
                    f.progressPanel1.Caption = "Xử lý tài liệu cũ";
                    f.progressPanel1.Description = message + " " + (percent > 0 ? percent + "" : "") + "% ...";
                }));
            };
            bw.DoWork += (s, e) =>
            {
                this.Invoke(new Action(() =>
                {
                    Thread thDialog = new Thread(new ThreadStart(() =>
                    {
                        this.Invoke(new Action(() =>
                        {
                            f.ShowDialog();
                        }));
                    }));
                    thDialog.SetApartmentState(ApartmentState.STA);
                    thDialog.Start();
                }));

                //get expired downloaded file and remove + update status references tables

                int i = 0;
                foreach (var downloadDocument in expriredDownloadDocuments)
                {
                    //remove file
                    frmDownload.DeleteFile(downloadDocument.FileName);

                    //update cac bang lien quan
                    if (downloadDocument.DocumentId > 0)
                    {
                        //day la tai lieu
                        var obj = _documentService.GetDocumentById(downloadDocument.DocumentId);
                        if (obj != null)
                        {
                            obj.IsDownloaded = false;
                            _documentService.Update(obj);
                        }
                    }
                    else
                    {
                        //day la bai hoc
                        var obj = _lessonService.GetLessonById(downloadDocument.LessonId);
                        if (obj != null)
                        {
                            obj.IsDownloaded = false;
                            _lessonService.Update(obj);
                        }
                    }
                    //xoa khoi manage
                    _downloadDocumentService.Delete(downloadDocument.Id);

                    bw.ReportProgress(i++ / expriredDownloadDocuments.Count * 100, "Hoàn thành");
                }

                bw.ReportProgress(0, "Xử lý tài liệu thành công.");
            };
            bw.RunWorkerCompleted += (e, s) =>
            {
                this.Invoke(new Action(() =>
                {
                    f.Dispose();
                }));

            };
            bw.RunWorkerAsync();
        }

        /// <summary>
        /// Xóa toàn bộ file + folder tài nguyên. Dành cho phần mềm lần đầu update lên version mới
        /// </summary>
        void ClearFilesOlder()
        {
            try
            {
                var folderRoot = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\App_data\\");
                var di = new DirectoryInfo(folderRoot);
                foreach (FileInfo file in di.GetFiles().Where(x => x.Extension != ".sdf"))
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }
        }

        void SetPictureHover()
        {
            btnSearch.MouseHover += new EventHandler(_wincontrolService.picMouseHover3);
            btnSearch.MouseLeave += new EventHandler(_wincontrolService.picMouseLeave3);

            btnReload.MouseHover += new EventHandler(_wincontrolService.picMouseHover3);
            btnReload.MouseLeave += new EventHandler(_wincontrolService.picMouseLeave3);

            picRoom.MouseHover += new EventHandler(_wincontrolService.picMouseHover);
            picRoom.MouseLeave += new EventHandler(_wincontrolService.picMouseLeave);

            picMainMenu.MouseHover += new EventHandler(_wincontrolService.picMouseHover3);
            picMainMenu.MouseLeave += new EventHandler(_wincontrolService.picMouseLeave3);

            picLessonList.MouseHover += new EventHandler(_wincontrolService.picMouseHover3);
            picLessonList.MouseLeave += new EventHandler(_wincontrolService.picMouseLeave3);

            picGuild.MouseHover += new EventHandler(_wincontrolService.picMouseHover3);
            picGuild.MouseLeave += new EventHandler(_wincontrolService.picMouseLeave3);

            picInbox.MouseHover += new EventHandler(_wincontrolService.picMouseHover3);
            picInbox.MouseLeave += new EventHandler(_wincontrolService.picMouseLeave3);

            picSupport.MouseHover += new EventHandler(_wincontrolService.picMouseHover3);
            picSupport.MouseLeave += new EventHandler(_wincontrolService.picMouseLeave3);

            picTaiLieu.MouseHover += new EventHandler(_wincontrolService.picMouseHover3);
            picTaiLieu.MouseLeave += new EventHandler(_wincontrolService.picMouseLeave3);
        }

        private void SendAdvertismentCount(int clientid, int advid)
        {
            if (DXWindows.Helper.InternetHelper.CheckForInternetConnection())
            {
                BackgroundWorker bw = new BackgroundWorker();
                bw.WorkerReportsProgress = true;
                bw.DoWork += (s, e) =>
                {
                    //var clientservice = new ClientService();
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(GlobalSession.BaseApiUrl);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        // HTTP GET
                        HttpResponseMessage response = client.GetAsync("api/PublicApi/PostAdvertisment?clientid=" + clientid + "&advid=" + advid).Result;
                    }
                };

                bw.RunWorkerAsync();
            }
        }

        private bool RunAsync(bool fromLoad)
        {
            bool syncResult = true;
            BackgroundWorker bw = new BackgroundWorker();
            float totalRecord = 0;
            frmWaitForm f = new frmWaitForm();
            bw.WorkerReportsProgress = true;
            bw.ProgressChanged += (s, e) =>
            {
                var message = e.UserState.ToString();
                var percent = e.ProgressPercentage;
                if (percent > 100)
                    percent = 100;

                this.Invoke(new Action(() =>
                {
                    f.progressPanel1.Caption = "Đồng bộ dữ liệu";
                    f.progressPanel1.Description = message + " " + (percent > 0 ? percent + "" : "") + "% ...";
                }));
            };
            bw.DoWork += (s, e) =>
            {
                this.Invoke(new Action(() =>
                {
                    Thread thDialog = new Thread(new ThreadStart(() =>
                    {
                        this.Invoke(new Action(() =>
                        {
                            f.ShowDialog();
                        }));
                    }));
                    thDialog.SetApartmentState(ApartmentState.STA);
                    thDialog.Start();
                }));
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(GlobalSession.BaseApiUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    string url = "api/PublicApi/GetUpdateData?clientId=" + Globals.Userlogin.ClientId + "&version=" + GetVersion(); // HTTP GET
                    HttpResponseMessage response =
                        client.GetAsync(url).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var model = response.Content.ReadAsAsync<SynDataViewModel>().Result;
                        if (model == null)
                        {
                            syncResult = false;
                            return;
                        }

                        // update news and guild
                        totalRecord = model.GetSyncTotalItems();

                        float i = 0;
                        var news = model.NewInfo;
                        if (news != null)
                        {
                            var obj = _newService.GetById(news.NewsId);
                            if (obj == null || obj.NewsId <= 0)
                            {
                                //save local
                                news.Status = 0;
                                news.CreatedDate = DateTime.Now;
                                _newService.Create(news);
                            }
                            else
                            {
                                //save local
                                if (obj.Description != news.Description)
                                {
                                    obj.Status = 0;
                                }
                                obj.Title = news.Title;
                                obj.Description = news.Description;
                                obj.ImageUrl = news.ImageUrl;

                                _newService.Update(obj);
                            }
                            bw.ReportProgress((int)((i++ / totalRecord) * 100), "Hoàn thành");
                        }
                        news = model.UserGuide;
                        if (news != null)
                        {
                            News obj = _newService.GetById(news.NewsId);
                            if (obj == null || obj.NewsId <= 0)
                            {
                                //save local
                                news.Status = 0;
                                news.CreatedDate = DateTime.Now;
                                _newService.Create(news);
                                DownloadFile(news.ImageUrl);
                            }
                            else
                            {
                                if (news.ImageUrl != obj.ImageUrl)
                                {//save local
                                    obj.Title = news.Title;
                                    obj.Description = news.Description;
                                    obj.ImageUrl = news.ImageUrl;
                                    DownloadFile(news.ImageUrl);
                                    _newService.Update(obj);

                                }
                            }
                            bw.ReportProgress((int)((i++ / totalRecord) * 100), "Hoàn thành");
                        }
                        news = model.HotlineInfo;
                        if (news != null)
                        {
                            News obj = _newService.GetById(news.NewsId);
                            if (obj == null || obj.NewsId <= 0)
                            {
                                //save local
                                news.Status = 0;
                                news.CreatedDate = DateTime.Now;
                                _newService.Create(news);
                            }
                            else
                            {
                                //if (news.ImageUrl != obj.ImageUrl)
                                {//save local
                                    obj.Title = news.Title;
                                    obj.Description = news.Description;
                                    obj.ImageUrl = news.ImageUrl;
                                    DownloadFile(news.ImageUrl);
                                    _newService.Update(obj);

                                }
                            }
                            bw.ReportProgress((int)((i++ / totalRecord) * 100), "Hoàn thành");
                        }

                        news = model.SoftInfo;
                        if (news != null)
                        {
                            News obj = _newService.GetById(news.NewsId);
                            if (obj == null || obj.NewsId <= 0)
                            {
                                //save local
                                news.Status = 0;
                                news.CreatedDate = DateTime.Now;
                                _newService.Create(news);
                            }
                            else
                            {
                                obj.Title = news.Title;
                                obj.Description = news.Description;
                                obj.ImageUrl = news.ImageUrl;
                                DownloadFile(news.ImageUrl);
                                _newService.Update(obj);
                            }
                            bw.ReportProgress((int)((i++ / totalRecord) * 100), "Hoàn thành");
                        }

                        var listSubject = _subjectService.GetAll();
                        var listRoom = _roomService.GetAll();
                        var listLesson = _lessonService.GetAll();
                        //                        var listDocs = _documentService.GetAll();

                        if (model.Subjects != null)
                        {
                            foreach (var subject in model.Subjects)
                            {
                                if (!listSubject.Exists(p => p.SubjectId == subject.SubjectId))
                                {
                                    _subjectService.Create(subject);
                                }
                                else
                                {
                                    _subjectService.Update(subject);
                                }

                                bw.ReportProgress((int)((i++ / totalRecord) * 100), "Hoàn thành");
                            }
                        }
                        if (model.Rooms != null)
                        {
                            foreach (var room in model.Rooms)
                            {
                                if (!listRoom.Exists(p => p.RoomId == room.RoomId))
                                {
                                    _roomService.Create(room);
                                }
                                else
                                {
                                    _roomService.Update(room);
                                }

                                bw.ReportProgress((int)((i++ / totalRecord) * 100), "Hoàn thành");
                            }
                        }
                        //                        if (model.Documents != null)
                        //                        {
                        //                            foreach (var entry in model.Documents)
                        //                            {
                        //                                if (!listDocs.Exists(p => p.DocumentId == entry.DocumentId))
                        //                                {
                        //                                    _documentService.Create(entry);
                        //                                }
                        //                                else
                        //                                {
                        //                                    if (entry.ChangeOperation == "D")
                        //                                    {
                        //                                        DeleteFile(entry.FileName);
                        //                                        _documentService.Delete(entry.DocumentId);
                        //                                    }
                        //                                    else
                        //                                    {
                        //                                        _documentService.Update(entry);
                        //                                    }
                        //                                }
                        //
                        //                                bw.ReportProgress((int)((i++ / totalRecord) * 100), "Hoàn thành");
                        //                            }
                        //                        }
                        if (model.Lessons != null)
                        {
                            foreach (var entry in model.Lessons)
                            {
                                var sub = listLesson.FirstOrDefault(p => p.LessonId == entry.LessonId);
                                if (sub == null)
                                {
                                    entry.IsDownloaded = false;
                                    entry.IsDeleted = false;
                                    entry.NeedUpdate = false;
                                    _lessonService.Create(entry);
                                    DownloadFile(entry.NormalImage);
                                }
                                else
                                {
                                    if (entry.ChangeOperation == "D")
                                    {
                                        var docs = _documentService.GetAllDocumentByLesson(entry.LessonId);
                                        foreach (var item in docs)
                                        {
                                            DeleteFile(item.FileName);
                                            _documentService.Delete(item.DocumentId);
                                        }
                                        DeleteFile(entry.GuideFile);

                                        _lessonService.Delete(entry.LessonId);
                                    }
                                    else
                                    {
                                        entry.NeedUpdate = entry.Reversion != sub.Reversion || sub.NeedUpdate;
                                        entry.IsDownloaded = sub.IsDownloaded;
                                        entry.GuideFile = sub.GuideFile;// keep guide link
                                        _lessonService.Update(entry);
                                        if (sub.NormalImage != entry.NormalImage)
                                        {
                                            DownloadFile(entry.NormalImage);
                                        }
                                    }

                                }

                                bw.ReportProgress((int)((i++ / totalRecord) * 100), "Hoàn thành");
                            }
                        }

                        if (model.ClientLessons != null)
                        {
                            _lessonService.UpdateClientLesson(model.ClientLessons, Globals.Userlogin.ClientId);
                            bw.ReportProgress((int)((i++ / totalRecord) * 100), "Hoàn thành");
                        }
                    }
                }
                // room
                //bw.ReportProgress(0, "Cập nhật phiên bản phần mềm");
                UpdateVersion();
                bw.ReportProgress(0, "Đồng bộ thành công");
            };
            bw.RunWorkerCompleted += (e, s) =>
            {
                this.Invoke(new Action(() =>
                {
                    f.Dispose();
                }));

                if (!syncResult)
                    MessageBox.Show("Chương trình không thể thực hiện đồng bộ dữ liệu tới máy chủ. Vui lòng thử lại hoặc liên hệ STEM+ để được giúp đỡ !", "STEM+ - Thông báo !", MessageBoxButtons.OK, MessageBoxIcon.Information);

                //show data
                FormLoad(fromLoad);
            };
            bw.RunWorkerAsync();
            return true;
        }

        private bool GetMessages()
        {
            bool syncResult = true;
            BackgroundWorker bw = new BackgroundWorker();
            float totalRecord = 0;
            bw.WorkerReportsProgress = true;
            bw.ProgressChanged += (s, e) =>
            {
                var message = e.UserState.ToString();
                var percent = e.ProgressPercentage;
            };
            bw.DoWork += (s, e) =>
            {
                if (InternetHelper.CheckForInternetConnection())
                    using (var comment = new HttpClient())
                    {
                        comment.BaseAddress = new Uri(GlobalSession.BaseApiUrl);
                        comment.DefaultRequestHeaders.Accept.Clear();
                        comment.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        string url = "api/PublicApi/GetFeedback?client=" + Globals.Userlogin.ClientId; // HTTP GET
                        HttpResponseMessage response =
                            comment.GetAsync(url).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            var model = response.Content.ReadAsAsync<SynDataViewModel>().Result;

                            if (model == null)
                            {
                                syncResult = false;
                            }
                            else
                            {
                                if (model.Feedbacks != null)
                                {
                                    foreach (var feedback in model.Feedbacks)
                                    {
                                        Comment obj = new Comment();
                                        obj.FeedbackId = feedback.FeedbackId;
                                        obj.ClientId = feedback.ClientId;
                                        obj.CommentType = 10;
                                        obj.Content = feedback.Comment;
                                        obj.LessonId = feedback.LessonId;
                                        obj.CreatedDate = feedback.CommentTime.Value;
                                        obj.Status = feedback.Viewed ? 1 : 0;

                                        _commentService.CreateFB(obj);
                                    }

                                }

                                if (model.FeedbackAnswers != null)
                                {
                                    foreach (var feedbacka in model.FeedbackAnswers)
                                    {
                                        CommentAnswer obj = new CommentAnswer();
                                        obj.CommentId = 0;
                                        obj.ClientId = Globals.Userlogin.ClientId;
                                        obj.ClientName = Globals.Userlogin.UserName;
                                        obj.UserId = feedbacka.UserId;
                                        obj.UserName = feedbacka.UserName;
                                        obj.Answer = feedbacka.Answer;
                                        obj.AnswerTime = feedbacka.AnswerTime == null ? DateTime.Now : (DateTime)feedbacka.AnswerTime.Value;
                                        obj.Status = feedbacka.Status;
                                        obj.FeedbackId = feedbacka.FeedbackId;
                                        obj.FBAnswerId = feedbacka.FBAnswerId;
                                        _commentService.CreateFBA(obj);
                                    }

                                }
                            }
                        }
                    }

                //bw.ReportProgress(0, "Đồng bộ thành công");
            };
            bw.RunWorkerCompleted += (e, s) =>
            {
                //show feedback message
                int total = _commentAsService.GetNewMessage();
                if (total > 0)
                {
                    frmShowMessage fsm = new frmShowMessage(total);
                    fsm.ShowDialog();
                }

                //load form feedback message
                frmMessage fmessage = new frmMessage();
                fmessage.Dock = DockStyle.Fill;
                fmessage.GetData();
                panelInbox.Controls.Clear();
                panelInbox.Controls.Add(fmessage);
            };
            bw.RunWorkerAsync();
            return true;
        }

        private bool GetAdvertisement(bool fromLoad)
        {
            bool syncResult = true;
            BackgroundWorker bw = new BackgroundWorker();
            float totalRecord = 0;
            bw.WorkerReportsProgress = true;
            bw.ProgressChanged += (s, e) =>
            {
                var message = e.UserState.ToString();
                var percent = e.ProgressPercentage;
            };
            bw.DoWork += (s, e) =>
            {
                if (InternetHelper.CheckForInternetConnection())
                    using (var comment = new HttpClient())
                    {
                        comment.BaseAddress = new Uri(GlobalSession.BaseApiUrl);
                        comment.DefaultRequestHeaders.Accept.Clear();
                        comment.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        string url = "api/PublicApi/GetAdvertisement"; // HTTP GET
                        HttpResponseMessage response =
                            comment.GetAsync(url).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            var model = response.Content.ReadAsAsync<SynDataViewModel>().Result;

                            if (model == null)
                            {
                                syncResult = false;
                            }

                            if (model != null)
                            {
                                if (model.Advertisements != null)
                                {
                                    foreach (var adv in model.Advertisements)
                                    {

                                        Advertisement obj = new Advertisement();
                                        obj = _advertisementService.GetAdvById(adv.Id);

                                        if (obj == null || obj.Id <= 0)
                                        {
                                            obj = new Advertisement();
                                            obj.Id = adv.Id;
                                            obj.Title = adv.Title;
                                            obj.AdvContent = adv.AdvContent;
                                            obj.AdvImage = adv.AdvImage;
                                            obj.AdvLink = adv.AdvLink;
                                            obj.ClickPerDay = adv.ClickPerDay;
                                            obj.StartDate = adv.StartDate;
                                            obj.EndDate = adv.EndDate;
                                            obj.Flag = adv.Flag;

                                            _advertisementService.Create(obj);
                                            DownloadFile(adv.AdvImage, true);
                                        }
                                        else
                                        {
                                            bool downloadstatus = true;
                                            if (!string.IsNullOrEmpty(adv.AdvImage) && !(adv.AdvImage.Equals(obj.AdvImage)))
                                            {
                                                downloadstatus = DownloadFile(adv.AdvImage, true);
                                            }
                                            if (downloadstatus)
                                            {
                                                obj.Title = adv.Title;
                                                obj.AdvContent = adv.AdvContent;
                                                obj.AdvImage = adv.AdvImage;
                                                obj.AdvLink = adv.AdvLink;
                                                obj.ClickPerDay = adv.ClickPerDay;
                                                //obj.StartDate = adv.StartDate;
                                                obj.EndDate = adv.EndDate;
                                                obj.Flag = adv.Flag;

                                                _advertisementService.Update(obj);
                                            }
                                        }
                                    }

                                }
                            }
                        }
                    }

                //bw.ReportProgress(0, "Đồng bộ thành công");
            };
            //            bw.RunWorkerCompleted += (e, s) =>
            //            {
            //                if (fromLoad)
            //                    ShowAdv();
            //            };
            bw.RunWorkerAsync();
            return true;
        }

        public static string GetVersion()
        {
            var configService = new SysConfigService();
            var syscofig = configService.GetSysConfigByKey("DATAVERSION");
            if (syscofig != null)
            {
                return syscofig.Value;
            }
            return "0";
        }

        public static void UpdateVersion()
        {
            var configService = new SysConfigService();
            var syscofig = configService.GetSysConfigByKey("DATAVERSION");
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
                        configService.Update(syscofig);
                    }
                    else
                    {
                        configService.Create(new SysConfig()
                        {
                            Code = "DATAVERSION",
                            Name = "DATAVERSION",
                            Value = version.ToString()
                        });
                    }
                }
            }
        }

        private void ShowData()
        {
            if (string.IsNullOrEmpty(picRoom.Tag + "")) return;
            int order = 0;
            string title = txtTitle.Text.Trim();

            this.Invoke(new Action(() =>
            {
                panelLesson.Controls.Clear();
            }));

            int roomId = int.Parse(picRoom.Tag + "");
            var lessons = _lessonService.GetLessonByRoom(roomId, title, order);

            bool fillWhite = false;
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int itemCount = 0;
            ucLesson item;

            if (lessons.Count > 0)
            {
                item = new ucLesson();
                itemCount = screenWidth / item.Width;
            }
            var countL = lessons.Count;
            for (int i = 0; i < lessons.Count; i++)
            {
                var lesson = lessons[i];
                lesson.Documents = _documentService.GetAllDocumentByLesson(lesson.LessonId);
                if (order == 0)
                {
                    lesson.NoIndex = countL - i;
                }
                else
                {
                    lesson.NoIndex = i + 1;
                }
                item = new ucLesson(lesson);

                if (i % itemCount == 0) fillWhite = !fillWhite;

                item.SetBackColor(fillWhite ? Color.FromArgb(200, 201, 204) : Color.FromArgb(226, 230, 233));

                panelLesson.Controls.Add(item);
            }
        }

        private void GetComboData()
        {
            int froomId = picRoom.Tag == null ? 0 : int.Parse(picRoom.Tag + "");
            contextMenuRoom.Items.Clear();
            var rooms = _roomService.GetRoomByClient(Globals.Userlogin.ClientId);
            var counter = rooms.Count;
            if (counter > 0)
            {
                lblTitle.Visible = true;

                if (counter == 1)
                {
                    Room r = rooms.First();
                    Subject obj = new SubjectService().GetSubjectByRoomId(r.RoomId);
                    lblTitle.Text = obj.Name + " / " + r.Name;

                    picRoom.Tag = r.RoomId;
                    picRoom.Visible = false;
                    lblRoomSelect.Visible = false;
                }
                else if (counter > 1)
                {
                    picRoom.Visible = true;
                    lblRoomSelect.Visible = true;
                    bool first = true;

                    foreach (Room r in rooms)
                    {
                        string menutext = "";
                        Subject obj = new SubjectService().GetSubjectByRoomId(r.RoomId);
                        if (obj != null)
                        {
                            menutext = obj.Name + " / " + r.Name + "     ";
                            contextMenuRoom.Items.Add(menutext, null, new EventHandler((o, s) =>
                            {
                                lblTitle.Text = menutext;
                                picRoom.Tag = r.RoomId;
                                ShowData();
                            }));

                            //load first room data
                            if (first)
                            {
                                lblTitle.Text = menutext;
                                picRoom.Tag = r.RoomId;
                                first = false;
                            }

                            if (froomId == r.RoomId)
                            {
                                lblTitle.Text = menutext;
                                picRoom.Tag = r.RoomId;
                                first = false;
                            }
                        }
                    }

                    foreach (ToolStripMenuItem item in contextMenuRoom.Items)
                    {
                        item.ForeColor = Color.FromArgb(219, 104, 41);
                    }
                }
            }
        }

        /// <summary>
        /// show notification message when first loggged
        /// </summary>
        private void ShowNotifHomeMessage()
        {
            News obj = _newService.GetById(NewsNotifMessageId);
            if (obj.Status == 0)
            {
                frmMessageDialog f = new frmMessageDialog();
                f.objNew = obj;
                f.ShowDialog(this);
            }
        }

        /// <summary>
        /// show notification message from menu
        /// </summary>
        private void ShowMenuNotifMessage()
        {
            News obj = _newService.GetById(NewsNotifMessageId);
            frmMessageDialog f = new frmMessageDialog();
            f.objNew = obj;
            f.ShowDialog(this);
        }

        private void LoadGuild()
        {
            //var list = _newService.GetById(NewsGuildId);
        }

        private void ShowListLesson()
        {
            panelGuild.Visible = false;
            panelInbox.Visible = false;
            panelHeadLesson.Visible = true;
            panelLesson.Visible = true;
            panelTaiLieu.Visible = false;

            picLessonList.EditValue = Properties.Resources._2a;
            picGuild.EditValue = Properties.Resources._3;
            picInbox.EditValue = Properties.Resources._4;
            picTaiLieu.EditValue = Properties.Resources._8;
        }
        private void ShowGuild()
        {
            panelHeadLesson.Visible = false;
            panelLesson.Visible = false;
            pdfViewer1.Enabled = true;
            pdfViewer1.Visible = true;
            panelGuild.Visible = true;
            panelInbox.Visible = false;
            panelTaiLieu.Visible = false;

            picLessonList.EditValue = Properties.Resources._2;
            picGuild.EditValue = Properties.Resources._3a;
            picInbox.EditValue = Properties.Resources._4;
            picTaiLieu.EditValue = Properties.Resources._8;
            try
            {
                var guild = _newService.GetById(NewsGuildId);
                string fileUrl =
                   Path.Combine(
                       Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\App_data\\",
                       guild.ImageUrl.ToLower().MD5Hash());

                pdfViewer1.DocumentFilePath = fileUrl;
            }
            catch { }
        }
        private void ShowInbox()
        {
            panelGuild.Visible = false;
            panelHeadLesson.Visible = false;
            panelLesson.Visible = false;
            panelInbox.Visible = true;
            panelTaiLieu.Visible = false;

            picLessonList.EditValue = Properties.Resources._2;
            picGuild.EditValue = Properties.Resources._3;
            picInbox.EditValue = Properties.Resources._4a;
            picTaiLieu.EditValue = Properties.Resources._8;
        }

        private void ShowTaiLieu()
        {
            panelGuild.Visible = false;
            panelHeadLesson.Visible = false;
            panelLesson.Visible = false;
            panelInbox.Visible = false;
            panelTaiLieu.Visible = true;

            picLessonList.EditValue = Properties.Resources._2;
            picGuild.EditValue = Properties.Resources._3;
            picInbox.EditValue = Properties.Resources._4;
            picTaiLieu.EditValue = Properties.Resources._8a;
        }

        private bool DownloadFile(string objFile, bool originalName = false)
        {
            if (string.IsNullOrWhiteSpace(objFile)) return false;
            string url = GlobalSession.BaseApiUrl + "images/" + objFile;

            var sFilePathToWriteFileTo = Path.Combine(
                Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\App_data\\",
                originalName ? objFile : objFile.ToLower().MD5Hash());

            var sFilePathToWriteFileToCOpy = Path.Combine(
                Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\App_data\\",
                originalName ? objFile : objFile.ToLower().MD5Hash() + "_tmp");

            if (File.Exists(sFilePathToWriteFileTo)) return true;
            // Create an instance of WebClient
            WebClient client = new WebClient(); // Hookup DownloadFileCompleted Event 

            // Start the download and copy the file to c:\temp           
            client.DownloadProgressChanged += (s, e) =>
            {
            };
            client.DownloadFileCompleted += (sender, args) =>
            {    //move file
                if (!File.Exists(sFilePathToWriteFileTo))
                    File.Move(sFilePathToWriteFileToCOpy, sFilePathToWriteFileTo);
                else
                {
                    if (!originalName)
                        File.Delete(sFilePathToWriteFileToCOpy);
                }
            };

            client.DownloadFileAsync(new Uri(url), sFilePathToWriteFileToCOpy);

            bool downloadstatus = true;
            client.DownloadFileCompleted += (sender, args) =>
            {
                if (args.Cancelled || args.Error != null)
                {
                    downloadstatus = false;
                }
            };
            return downloadstatus;
        }

        private bool DeleteFile(string objFile)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(objFile)) return false;
                string sFilePathToWriteFileTo =
                    Path.Combine(
                        Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\App_data\\",
                        objFile.Trim().ToLower().MD5Hash()); //move file
                if (File.Exists(sFilePathToWriteFileTo))
                    File.Delete(sFilePathToWriteFileTo);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void ShowClientConfirm()
        {
            if (InternetHelper.CheckForInternetConnection())
            {
                Client obj = _clientService.GetClientById(Globals.Userlogin.ClientId);
                if (obj == null || obj.ClientId <= 0) return;
                if (!string.IsNullOrEmpty(obj.Email) && !string.IsNullOrEmpty(obj.Phone)) return;

                frmClientConfirm f = new frmClientConfirm();
                f.txtEmail.Text = obj.Email;
                f.txtPhone.Text = obj.Phone;

                f.ShowDialog(this);
            }
        }

        void ShowfrmAdv()
        {
            Advertisement obj = _advertisementService.GetAdvById(2);
            if (obj == null || obj.Id <= 0) return;
            //check and update count
            if (obj.ClickCount > obj.ClickPerDay)
            {
                if (Math.Round((DateTime.Now - obj.StartDate).TotalDays) == 0)
                {
                    return;
                }
                else
                {
                    obj.StartDate = DateTime.Now;
                    obj.ClickCount = 0;
                }
                _advertisementService.Update(obj);
            }

            //frmAdvertisementFake fk = new frmAdvertisementFake();
            //fk.Left = Screen.PrimaryScreen.Bounds.Width - (fk.Width + 50);
            //fk.Top = Screen.PrimaryScreen.Bounds.Height - (fk.Height + 80);
            //fk.ShowDialog(this);
            //display form
            var f = new frmAdvertisement();
            f.StartPosition = FormStartPosition.CenterParent;
            //f.Left = Screen.PrimaryScreen.Bounds.Width - (f.Width + 50);
            //f.Top = Screen.PrimaryScreen.Bounds.Height - (f.Height + 80);
            f.ShowDialog(this);
        }

        private void ShowAdv()
        {
            ShowfrmAdv();

            Advertisement obj = _advertisementService.GetAdvById(1);
            if (obj == null || obj.Id <= 0) return;

            txtAdvertisement.Elements[0].Text = obj.AdvContent;

            txtAdvertisement.Tag = obj.AdvLink;
            txtAdvertisement.StartMarquee();
        }

        private void ShowHotlineHome()
        {
            var hotline = _newService.GetById(NewsHotlineId);
            if (hotline != null)
            {
                barButtonItem4.Caption = hotline.Description;
            }
        }

        private void FormLoad(bool fromLoad)
        {
            //show data
            GetComboData();
            ShowData();

            ShowNotifHomeMessage();
            //show guild
            panelGuild.Visible = false;
            LoadGuild();

            //create table commentanswer if not exists
            GetMessages();
            //client confirm information 7.0
            ShowClientConfirm();
            //show header advertisment
            GetAdvertisement(fromLoad);

            ShowHotlineHome();

            //bind tailieu
            panelTaiLieu.Controls.Clear();
            var item = new ucTaiLieu { Dock = DockStyle.Fill };
            panelTaiLieu.Controls.Add(item);
        }

        private void picMainMenu_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                contextMenuStrip1.Show(picMainMenu, 0, 52);
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ShowMenuNotifMessage();
        }

        private void txtTitle_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ShowData();
            }
        }

        private void txtTitle_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            ShowData();
        }

        private void SoftwareInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            News obj = _newService.GetById(NewsSoftwareInfoId);
            var f = new frmHotline();
            f.objNew = obj;
            f.ShowDialog();

        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn thoát chương trình ?", "STEM+ - Thông báo !", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmFeedback f = new frmFeedback();
            f.DialogType = 1;
            f.ShowDialog();
        }

        private void picGuild_Click(object sender, EventArgs e)
        {
            ShowGuild();
        }

        private void picLessonList_Click(object sender, EventArgs e)
        {
            ShowListLesson();
        }

        private void picExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn thoát chương trình ?", "STEM+ - Thông báo !", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void picRoom_Click(object sender, EventArgs e)
        {
            contextMenuRoom.Show(picRoom, 0, 30);
        }

        private void barStaticItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmFeedback f = new frmFeedback();
            f.DialogType = 1;
            f.ShowDialog();
        }

        private void picMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void HotlineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var obj = _newService.GetById(NewsHotlineId);
            var f = new frmHotline { objNew = obj, SetMinSize = true };
            f.ShowDialog();
        }

        private void btnSynsLesson_Click(object sender, EventArgs e)
        {
            var f = new frmSynsLessonOffline();
            f.ShowDialog();
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            GetNewData(false);
        }

        private void SoftwareUpdateInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (checkUpdate())
            {
                //var currentVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                if (MessageBox.Show("Đã có phiên bản mới của phần mềm, Bạn có muốn nâng cấp phần mềm không ?", "STEM+ - Thông báo !", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
                {
                    runUpdate();
                }
            }
            else
            {
                MessageBox.Show(updateCurrent);
            }
        }

        private bool checkUpdate()
        {
            var currentVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            info = DXWindows.Helper.UpdateChecker.GetUpdateInfo(GlobalSession.UpdateUrl, "UM.txt", Application.StartupPath + @"\", 1);

            if (info == null)
            {
                return false;
            }

            if (decimal.Parse(info[1].Replace(".", "")) > decimal.Parse(currentVersion.Replace(".", "")))
            {
                return true;
            }
            return false;
        }

        private bool runUpdate()
        {
            var currentVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            info = UpdateChecker.GetUpdateInfo(GlobalSession.UpdateUrl, "UM.txt", Application.StartupPath + @"\", 1);

            if (info == null)
            {
                MessageBox.Show(updateInfoError);
            }
            else
            {
                if (decimal.Parse(info[1].Replace(".", "")) > decimal.Parse(currentVersion.Replace(".", "")))
                {
                    UpdateChecker.InstallUpdateRestart(info[3], info[4], "\"" + Application.StartupPath + "\\", processToEnd,
                        postProcess, "updated", Application.StartupPath + @"\" + GlobalSession.Updater + ".exe");
                    Close();
                }
                else
                {
                    MessageBox.Show(updateCurrent);
                }
            }
            return true;
        }

        private void picExit_MouseHover(object sender, EventArgs e)
        {
            picExit.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
        }

        private void picExit_MouseLeave(object sender, EventArgs e)
        {
            picExit.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
        }

        private void picMaximize_Click(object sender, EventArgs e)
        {
            //if (this.WindowState == FormWindowState.Maximized)
            if (this.Bounds == Screen.PrimaryScreen.WorkingArea)
            {
                this.WindowState = FormWindowState.Normal;
                picMaximize.Image = DXWindows.Properties.Resources.maximizeb;
            }
            else
            {
                this.Bounds = Screen.PrimaryScreen.WorkingArea;
                picMaximize.Image = DXWindows.Properties.Resources.minimizeb;
            }
        }

        private void picMaximize_MouseHover(object sender, EventArgs e)
        {
            picMaximize.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
        }

        private void picMaximize_MouseLeave(object sender, EventArgs e)
        {
            picMaximize.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
        }

        private void picMinimize_MouseHover(object sender, EventArgs e)
        {
            picMinimize.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
        }

        private void picMinimize_MouseLeave(object sender, EventArgs e)
        {
            picMinimize.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
        }

        private void picInbox_Click(object sender, EventArgs e)
        {
            ShowInbox();
        }

        private void btnAdvertisement_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //display form
            frmAdvertisement f = new frmAdvertisement();
            f.Left = Screen.PrimaryScreen.Bounds.Width - (f.Width + 50);
            f.Top = Screen.PrimaryScreen.Bounds.Height - (f.Height + 80);
            f.ShowDialog(this);
        }

        private void txtAdvertisement_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtAdvertisement.Tag + ""))
            {
                System.Diagnostics.Process.Start(txtAdvertisement.Tag + "");
                SendAdvertismentCount(Globals.Userlogin.ClientId, 1);
            }
        }

        private void btnAdvertisementImg_ItemDoubleClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void btnAdvertisementImg_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //display form
            frmAdvertisement f = new frmAdvertisement();
            f.Left = Screen.PrimaryScreen.Bounds.Width - (f.Width + 50);
            f.Top = Screen.PrimaryScreen.Bounds.Height - (f.Height + 80);
            f.ShowDialog(this);
        }

        private void AccountInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Client obj = _clientService.GetClientById(Globals.Userlogin.ClientId);
            if (obj == null || obj.ClientId <= 0) return;

            frmClientConfirm f = new frmClientConfirm();
            f.txtEmail.Text = obj.Email;
            f.txtPhone.Text = obj.Phone;

            f.ShowDialog(this);
        }
        private void picMainMenu_MouseHover(object sender, EventArgs e)
        {
            picMainMenu.EditValue = Properties.Resources._1c;
        }

        private void picMainMenu_MouseLeave(object sender, EventArgs e)
        {
            picMainMenu.EditValue = Properties.Resources._1;
        }

        private void FrmMain_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void bsiSupport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //run open teamview
            Process.Start(TeamViewPath);
        }

        private void picSupport_Click(object sender, EventArgs e)
        {
            Process.Start(TeamViewPath);
        }

        private void picTaiLieu_Click(object sender, EventArgs e)
        {
            ShowTaiLieu();
        }
    }
}