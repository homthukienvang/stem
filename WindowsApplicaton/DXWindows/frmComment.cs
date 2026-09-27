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

namespace DXWindows
{
    public partial class frmComment : DevExpress.XtraEditors.XtraForm
    {
        public int LessonId { get; set; }
        public string LessonName { get; set; }
        public int CommentId { get; set; }
        CommentService _commentService;
        Comment obj;
        public int DialogType { get; set; }

        public frmComment()
        {
            InitializeComponent();
            _commentService = new CommentService();
        }

        private bool InputValidate()
        {
            bool result = true;

            if (string.IsNullOrEmpty(txtContent.Text) || txtContent.Text.Length > 500)
            {
                if (string.IsNullOrEmpty(txtContent.Text))
                    dxErrorProvider1.SetError(txtContent, "Nhập nội dung ghi chú");
                else
                    dxErrorProvider1.SetError(txtContent, "Vui lòng gửi nội dung ghi chú không quá 500 ký tự !");

                dxErrorProvider1.SetIconAlignment(txtContent, ErrorIconAlignment.BottomRight);
                txtContent.BackColor = Color.Yellow;
                txtContent.Focus();
                result = false;
            }
            else
            {
                dxErrorProvider1.ClearErrors();
                txtContent.BackColor = Color.White;
            }

            return result;
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
                    lblTitle.Text = LessonName;
                    optType.SelectedIndex = obj.CommentType;
                    txtContent.Text = obj.Content;
                }
            }
        }

        private void SendFeedback()
        {
            if (InternetHelper.CheckForInternetConnection())
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(GlobalSession.BaseApiUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var content = new FormUrlEncodedContent(new[]
                        {new KeyValuePair<string, string>("ClientId", Globals.Userlogin.ClientId +""),
                            new KeyValuePair<string, string>("LessonId", LessonId +""),
                            new KeyValuePair<string, string>("Comment", txtContent.Text.Replace("'",""))
                        });
                    // HTTP POST
                    HttpResponseMessage response =
                        client.PostAsync("api/PublicApi/PostFeedback" , content).Result;
                    {
                        if (!response.IsSuccessStatusCode)
                        {
                            XtraMessageBox.Show(this, @"Gửi thông báo không thành công, vui lòng thử lại",
                    @"Thông báo", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                        }
                        else
                        {
                            XtraMessageBox.Show(this, @"Nội dung góp ý đã được gửi tới STEM+ !",
                    @"Thông báo", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                            this.Close();
                        }
                    }
                }
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
                    SendFeedback();
                }
            }
        }

        private void frmComment_Load(object sender, EventArgs e)
        {
            if (DialogType == 1)
            {
                //send comment
                LessonId = 0;
                LessonName = "";
                lblLesson.Visible = false;
                lblTitle.Visible = false;
                lblPurpose.Visible = false;
                optType.Visible = false;
                optType.SelectedIndex = 1;
                btnSave.Text = "Gửi STEM+";
                lblContent.Top = lblLesson.Top;
                lblContent.Left = lblLesson.Left;
                txtContent.Height = 281;
                txtContent.Top = lblTitle.Top;
                txtContent.Focus();
            }
            else
                GetData();
        }

        private void pictureEdit1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}