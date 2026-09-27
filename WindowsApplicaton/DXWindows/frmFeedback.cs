using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Model.Model;
using Services;
using System.Net.Http;
using Extensions;
using System.Net.Http.Headers;
using DXWindows.Helper;
using Model;
using System.Net;
using System.Threading;
using System.IO;

namespace DXWindows
{
    public partial class frmFeedback : DevExpress.XtraEditors.XtraForm
    {
        public int LessonId { get; set; }
        public string LessonName { get; set; }
        public int CommentId { get; set; }
        CommentService _commentService;
        ClientService _clientService;
        Comment obj;
        public int DialogType { get; set; }
        frmWaitForm f = new frmWaitForm();

        public frmFeedback()
        {
            InitializeComponent();
            _commentService = new CommentService();
            _clientService = new ClientService();
        }

        private bool InputValidate()
        {
            bool result = true;

            if (string.IsNullOrEmpty(txtContent.Text) || txtContent.Text.Length > 4000)
            {
                if (string.IsNullOrEmpty(txtContent.Text))
                    dxErrorProvider1.SetError(txtContent, "Nhập nội dung ghi chú");
                else
                    dxErrorProvider1.SetError(txtContent, "Vui lòng gửi nội dung ghi chú không quá 4000 ký tự !");

                dxErrorProvider1.SetIconAlignment(txtContent, ErrorIconAlignment.BottomRight);
                txtContent.BackColor = Color.Yellow;
                result = false;
            }
            else
            {
                dxErrorProvider1.ClearErrors();
                txtContent.BackColor = Color.White;
            }

            return result;
        }

        void GetClientInfo()
        {
            Client client = _clientService.GetClientById(Globals.Userlogin.ClientId);
            if (client == null || client.ClientId <= 0) return;

            txtClientName.Text = client.FullName  + "";
            txtPhone.Text = client.Phone + "";
            txtEmail.Text = client.Email + "";
        }

        private void GetData()
        {
            if (LessonId > 0)
            {
                obj = _commentService.GetByLessonId(LessonId);
                if (obj == null || obj.CommentId <= 0)
                {
                    obj = new Comment();
                }
                else
                {
                    optType.SelectedIndex = obj.CommentType;
                    txtContent.Text = obj.Content;
                }
                lblTitle.Text = LessonName;
                Room r = new RoomService().GetRoomByLessonId(LessonId).First();
                if (r != null)
                {
                    lblClass.Text = r.Name;
                }
                labelControl3.Visible = true;
                labelControl2.Visible = false;

                lblLesson.Visible = true;
            }
            else
            {
                lblTitle.Text = "";
                lblClass.Text = "";
                lblLesson.Visible = false;
                label10.Visible = false;
                labelControl3.Visible = false;
                labelControl2.Visible = true;
            }
        }

        private void DoSend()
        {
            if (InternetHelper.CheckForInternetConnection())
            {
                BackgroundWorker bw = new BackgroundWorker();
                bw.WorkerReportsProgress = true;
                bw.ProgressChanged += (s, e) =>
                {
                    var message = e.UserState.ToString();
                    var percent = e.ProgressPercentage;

                    this.Invoke(new Action(() =>
                    {
                        f.progressPanel1.Caption = "Gửi góp ý STEM+";
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
                        //thDialog.SetApartmentState(ApartmentState.STA);
                        thDialog.Start();
                    }));

                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(GlobalSession.BaseApiUrl);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        var content = new FormUrlEncodedContent(new[]
                            {new KeyValuePair<string, string>("ClientId", Globals.Userlogin.ClientId +""),
                                new KeyValuePair<string, string>("Name", txtClientName.Text.Replace("'","")),                            
                                new KeyValuePair<string, string>("Comment", txtContent.Text.Replace("'","")),
                                new KeyValuePair<string, string>("LessonId", LessonId +""),
                                new KeyValuePair<string, string>("Phone", txtPhone.Text.Replace("'","")),
                                new KeyValuePair<string, string>("Email", txtEmail.Text.Replace("'","")),
                                new KeyValuePair<string, string>("FileAttachs", (lnkFileAttach.Tag + "").Replace("'",""))                                
                            });
                        bw.ReportProgress((int)(0), "Gửi góp ý");
                        // HTTP POST
                        HttpResponseMessage response =
                            client.PostAsync("api/PublicApi/PostFeedback", content).Result;
                        {
                            if (!response.IsSuccessStatusCode)
                            {
                                XtraMessageBox.Show(this, @"Gửi thông báo không thành công, vui lòng thử lại", @"Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                            {
                                //upload attach file
                                foreach (var fileName in listAttach)
                                {
                                    var clientAttach = new WebClient();
                                    clientAttach.UploadProgressChanged += (ss, ee) =>
                                    {
                                        try
                                        {
                                            // Displays the operation identifier, and the transfer progress.
                                            string msg = string.Format("Đang tải file {0}.\n{1}Đã tải lên {2} / {3} bytes. {4} % hoàn thành...",
                                                fileName,
                                                (string)ee.UserState,
                                                ee.BytesReceived,
                                                ee.TotalBytesToReceive,
                                                ee.ProgressPercentage);
                                            //display message to dialog
                                            bw.ReportProgress(ee.ProgressPercentage, msg);
                                        }
                                        catch (Exception ex)
                                        {
                                            //SetText(lblStatus, "ERROR: Có lỗi xảy ra vui lòng thực hiện lại sau");
                                        }
                                    };

                                    var uri = new Uri(GlobalSession.BaseApiUrl + "api/PublicApi/UploadFiles/");
                                    try
                                    {
                                        clientAttach.Headers.Add("fileName", System.IO.Path.GetFileName(fileName));
                                        var data = System.IO.File.ReadAllBytes(Application.StartupPath + @"/App_data/" + fileName);
                                        clientAttach.UploadDataAsync(uri, data);
                                    }
                                    catch (Exception ex)
                                    {
                                        //MessageBox.Show(ex.Message);
                                    }

                                    clientAttach.UploadFileCompleted += (sss, eee) =>
                                    {
                                        if (File.Exists(Application.StartupPath + @"/App_data/" + fileName))
                                            File.Delete(Application.StartupPath + @"/App_data/" + fileName);
                                    };
                                }

                                this.Invoke(new Action(() =>
                                {
                                    XtraMessageBox.Show(this, @"Nội dung góp ý đã được gửi tới STEM+ !", @"Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    this.Close();
                                }));
                            }
                        }
                    }
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
            else
            {
                XtraMessageBox.Show(this, @"Không thể kết nối internet. Vui lòng kết nối internet để gửi góp ý",
                    @"Thông báo", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private void optType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (optType.SelectedIndex == 0)
                btnSave.Text = "Lưu";
            else if (optType.SelectedIndex == 1)
                btnSave.Text = "Gửi";
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (InputValidate())
            {
                //save local
                if (optType.SelectedIndex == 0)
                {
                    if (obj.CommentId == 0)
                    {
                        obj = new Comment();
                        obj.ClientId = Globals.Userlogin.ClientId;
                        obj.CommentType = optType.SelectedIndex;
                        obj.Content = txtContent.Text;
                        obj.LessonId = LessonId;

                        _commentService.Create(obj);
                        this.Close();
                    }
                    else if (obj.CommentId > 0)
                    {
                        obj.ClientId = Globals.Userlogin.ClientId;
                        obj.CommentType = optType.SelectedIndex;
                        obj.Content = txtContent.Text;
                        obj.LessonId = LessonId;
                        obj.CreatedDate = DateTime.Now;

                        _commentService.Update(obj);
                        this.Close();
                    }
                }
                else if (optType.SelectedIndex == 1) //send
                {
                    DoSend();
                }
            }
        }

        private void frmComment_Load(object sender, EventArgs e)
        {
            GetClientInfo();
            if (DialogType == 1)
            {
                //send comment
                optType.SelectedIndex = 1;
                btnSave.Text = "Gửi STEM+";
                obj = new Comment();
            }
            // else
            GetData();
        }

        private void pictureEdit1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        HashSet<string> listAttach = new HashSet<string>();

        private void lnkAttach_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.bmp, *.png) | *.jpg; *.jpeg; *.bmp; *.png";
            openFileDialog.Multiselect = true;

            var dialogResult = openFileDialog.ShowDialog();
            if (dialogResult != DialogResult.OK) return;

            if (listAttach.Count + openFileDialog.FileNames.Length > GlobalSession.FileCount)
            {
                XtraMessageBox.Show("Số lượng file đính kèm không quá " + GlobalSession.FileCount + " files. Vui lòng đính kèm và gửi lại", "STEM+ - Thông báo !", MessageBoxButtons.OK);
                return;
            }
            string list = "";
            string listupload = "";
            foreach (var fname in openFileDialog.FileNames)
            {
                FileInfo f = new FileInfo(fname);
                if (f.Exists)
                {
                    if (f.Length > GlobalSession.FileSize) //10mb
                    {
                        XtraMessageBox.Show("File đính kèm có dung lượng không quá " + (Math.Round((double)GlobalSession.FileSize / 1024 / 1024)) + "MB. Vui lòng đính kèm và gửi lại", "STEM+ Thông báo!", MessageBoxButtons.OK);
                        lnkFileAttach.Text = "";
                        lnkFileAttach.Tag = "";
                        listAttach = new HashSet<string>();
                        return;
                    }
                    string newfile = DateTime.Now.ToString("yyyy_mm_dd_HH_mm_ss") + "_" + Guid.NewGuid() + ".jpg";
                    File.Copy(fname, Application.StartupPath + @"/App_data/" + newfile);

                    listAttach.Add(newfile);
                    list += f.Name + ";";
                    listupload += newfile + ";";
                }
            }

            if (!string.IsNullOrEmpty(list))
                list = list.Substring(0, list.Length - 1);
            if (!string.IsNullOrEmpty(listupload))
                listupload = listupload.Substring(0, listupload.Length - 1);


            lnkFileAttach.Text = list;
            lnkFileAttach.ToolTip = list;
            lnkFileAttach.Tag = listupload;
            lnkDeleteFile.Visible = !string.IsNullOrEmpty(list);
        }

        //private void Upload()
        //{
        //    foreach (var fileName in listAttach)
        //    {
        //        var client = new WebClient();
        //        var uri = new Uri(GlobalSession.BaseApiUrl + "api/PublicApi/UploadFiles/");
        //        try
        //        {
        //            client.Headers.Add("fileName", System.IO.Path.GetFileName(fileName));
        //            var data = System.IO.File.ReadAllBytes(fileName);
        //            client.UploadDataAsync(uri, data);
        //        }
        //        catch (Exception ex)
        //        {
        //            //MessageBox.Show(ex.Message);
        //        }
        //    }
        //}

        private void lnkDeleteFile_Click(object sender, EventArgs e)
        {
            listAttach = new HashSet<string>();
            lnkFileAttach.Text = "";
            lnkFileAttach.Tag = "";
            lnkDeleteFile.Visible = false;
        }
    }
}