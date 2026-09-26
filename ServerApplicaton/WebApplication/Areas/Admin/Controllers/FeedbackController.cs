using System;
using System.Web.Mvc;
using DataModel.Base;
using Extensions;
using Model;
using Security;
using Services;
using WebApplication.Helper;
using System.IO;
using OfficeOpenXml;
using System.Collections.Generic;
using System.Linq;

namespace WebApplication.Areas.Admin.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly IFeedbackService _feedbackService;
        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        private PagedData<Feedback> GetModel(int pageIndex, int cLientId, string fromdate, string todate, string fullName, string tClass, string tSchoolCode, string tLessonCode, int viewed)
        {
            var model = new PagedData<Feedback>();
            int totalRow;
            model.Data = _feedbackService.GetFeedbackByPage(pageIndex, GlobalSession.PageSize, cLientId, fromdate, todate, fullName, tClass, tSchoolCode, tLessonCode, viewed, out totalRow);
            model.Pagination = new PaginationModel
            {
                CurrentPage = pageIndex,
                PageSize = GlobalSession.PageSize,
                NumberOfRows = totalRow
            };
            return model;
        }
        [RequiredLogin]
        [RequiredRight]
        public ActionResult Index()
        {
            var model = GetModel(0, 0, "", "", "", "", "", "",-1);
            return View(model);
        }

        public ActionResult List(int? pageIndex, int cLientId, string fromdate, string todate, string fullName, string tClass, string tSchoolCode, string tLessonCode, int viewed)
        {
            var model = GetModel(pageIndex.HasValue ? pageIndex.Value : 0, cLientId, fromdate, todate, fullName, tClass, tSchoolCode, tLessonCode, viewed);
            return PartialView(model);
        }

        public ActionResult Create()
        {
            var model = new Feedback()
            {
            };
            string html = this.RenderPartialView("Create", model);
            return Json(new
            {
                Html = html,
                Success = true,
                Message = ""
            }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            var model = _feedbackService.GetFeedbackById(id);
            model.FeedBackFiles = _feedbackService.GetFeedbackFile(id);
            string html = this.RenderPartialView("Edit", model);
            return Json(new
            {
                Html = html,
                Success = model != null,
                Message = ""
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Feedback model)
        {
            try
            {
                var response = _feedbackService.Update(model);
                string html = "";
                if (response.Success)
                {
                    var lst = GetModel(0, 0, "", "", "", "", "", "", -1 );
                    html = this.RenderPartialView("List", lst);
                }
                return Json(new
                {
                    Html = html,
                    Success = response.Success,
                    Message = response.Message
                });
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Edit Feedback");
                return Json(new { Success = false, Message = "Không thể sửa môn học" });
            }
        }

        //[ValidateInput(false)]
        [HttpPost]
        public ActionResult Delete(int Id)
        {
            try
            {
                var response = _feedbackService.Delete(Id);
                string html = "";
                if (response.Success)
                {
                    var lst = GetModel(0, 0, "", "", "", "", "", "", -1);
                    html = this.RenderPartialView("List", lst);
                }
                return Json(new
                {
                    Html = html,
                    Success = response.Success,
                    Message = response.Message
                });
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "_FeedbackService.Delete");
                return Json(new { Success = false, Message = "Có lỗi" });
            }
        }

        public ActionResult ShowAnswer(int id)
        {
            var model = _feedbackService.GetFeedbackById(id);
            model.FeedbackAnswers = _feedbackService.GetFeedbackAnswer(id);
            string html = this.RenderPartialView("ShowAnswer", model);
            return Json(new
            {
                Html = html,
                Success = model != null,
                Message = ""
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ShowAttach(int id)
        {
            var model = _feedbackService.GetFeedbackById(id);
            model.FeedBackFiles = _feedbackService.GetFeedbackFile(id);
            string html = this.RenderPartialView("ShowAttach", model);
            return Json(new
            {
                Html = html,
                Success = model != null,
                Message = ""
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [RequiredLogin]
        [ValidateInput(false)]
        public ActionResult ShowAnswer(Feedback model)
        {
            try
            {
                User user = (User)Session["UserSession"];
                if (user != null)
                {
                    model.UserId = user.UserId;
                    model.UserName = user.UserName + " - " + user.DisplayName;
                }
                var response = _feedbackService.CreateFeedbackAnswer(model);
                string html = "";
                if (response.Success)
                {
                    var lst = GetModel(0, 0, "", "", "", "", "", "", -1 );
                    html = this.RenderPartialView("List", lst);
                }
                return Json(new
                {
                    Html = html,
                    Success = response.Success,
                    Message = response.Message
                });
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Edit Client");
                return Json(new { Success = false, Message = "Có lỗi xảy ra" });
            }
        }

        [HttpPost]
        [RequiredLogin]
        [ValidateInput(false)]
        public ActionResult FinishFeedback(Feedback model)
        {
            try
            {
                var response = _feedbackService.FinishFeedback(model);

                return Json(new
                {
                    Html = "",
                    Success = response.Success,
                    Message = response.Message
                });
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Finish Feedback");
                return Json(new { Success = false, Message = "Có lỗi xảy ra" });
            }
        }

        //Export
        public ActionResult ExportFeedback(int? cLientId, string fromdate, string todate, string fullName, string tClass, string tSchoolCode, string tLessonCode, int viewed)
        {
            int totalRow = 0;
            int id = 0;
            if (cLientId.HasValue) id = cLientId.Value;

            var data = _feedbackService.GetFeedbackByPage(0, 100000000, id, fromdate, todate, fullName, tClass, tSchoolCode, tLessonCode, viewed , out totalRow);

            string fileNameSave = "feedback_" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".xlsx";
            string filePath = Server.MapPath("/Images/" + fileNameSave);

            var fileInfo = new FileInfo(filePath);
            using (ExcelPackage excel = new ExcelPackage(fileInfo))
            {

                var sheet = excel.Workbook.Worksheets.Add("feedback");
                sheet.Column(1).Width = 10;
                sheet.Column(2).Width = 20;
                sheet.Column(3).Width = 20;
                sheet.Column(4).Width = 30;
                sheet.Column(5).Width = 20;
                sheet.Column(6).Width = 20;
                sheet.Column(7).Width = 20;
                sheet.Column(8).Width = 30;
                sheet.Column(9).Width = 60;
                sheet.Column(10).Width = 20;

                sheet.Cell(1, 1).Value = "STT";
                sheet.Cell(1, 2).Value = "ID giáo viên";
                sheet.Cell(1, 3).Value = "Tên giáo viên";
                sheet.Cell(1, 4).Value = "Phone";
                sheet.Cell(1, 5).Value = "Email";
                sheet.Cell(1, 6).Value = "Ngày";
                sheet.Cell(1, 7).Value = "Bài học";
                sheet.Cell(1, 8).Value = "Lớp";
                sheet.Cell(1, 9).Value = "Ý kiến";
                sheet.Cell(1, 10).Value = "Đã xem";

                int rowCount = 2; // start row (in row 1 are header cells)
                foreach (Feedback client in data)
                {
                    try
                    {
                        sheet.Cell(rowCount, 1).Value = (rowCount - 1).ToString();
                        sheet.Cell(rowCount, 2).Value = client.ClientId + "";
                        sheet.Cell(rowCount, 3).Value = client.Name + "";
                        sheet.Cell(rowCount, 4).Value = client.Phone + "";
                        sheet.Cell(rowCount, 5).Value = client.Email + "";
                        sheet.Cell(rowCount, 6).Value = client.CommentTime == null ? "" : client.CommentTime.Value.ToString("dd/MM/yyyy");
                        sheet.Cell(rowCount, 7).Value = client.LessonName + "";
                        sheet.Cell(rowCount, 8).Value = client.RoomName + "";
                        sheet.Cell(rowCount, 9).Value = client.Comment + "";
                        sheet.Cell(rowCount, 10).Value = client.Viewed ? "Y" : "N";

                        IEnumerable<FeedbackAnswer> list = _feedbackService.GetFeedbackAnswer(client.FeedbackId);

                        if (list.Any())
                        {
                            int rankc = 1;
                            rowCount++;
                            sheet.Cell(rowCount, 2).Value = "STT";
                            sheet.Cell(rowCount, 3).Value = "Người trả lời";
                            sheet.Cell(rowCount, 4).Value = "Ngày trả lời";
                            sheet.Cell(rowCount, 5).Value = "Nội dung";
                            sheet.Cell(rowCount, 6).Value = "Trạng thái";
                            rowCount++;
                            foreach (FeedbackAnswer fa in list)
                            {
                                sheet.Cell(rowCount, 2).Value = "" + rankc;
                                sheet.Cell(rowCount, 3).Value = fa.UserName + "";
                                sheet.Cell(rowCount, 4).Value = fa.AnswerTime.Value.ToString("dd/MM/yyyy");
                                sheet.Cell(rowCount, 5).Value = fa.Answer + "";
                                sheet.Cell(rowCount, 6).Value = fa.Status == 0 ? "Chưa xem" : "Đã xem";

                                rankc++;
                                rowCount++;
                            }
                        }
                    }
                    catch
                    {
                    }

                    rowCount++;
                }

                excel.Save();
            }

            return new DownloadResult { VirtualPath = "/Images/" + fileNameSave, FileDownloadName = fileNameSave };

        }


    }
}