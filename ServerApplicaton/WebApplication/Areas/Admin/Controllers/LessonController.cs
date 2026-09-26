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

namespace WebApplication.Areas.Admin.Controllers
{
    public class LessonController : Controller
    {
        private readonly IRoomService _roomService;
        private readonly ILessonService _lessonService;
        private readonly ISubjectService _subjectService;
        private IDocumentService _documentService;
        public LessonController(ILessonService lessonService, ISubjectService subjectService, IRoomService roomService, IDocumentService documentService)
        {
            _lessonService = lessonService;
            _subjectService = subjectService;
            _roomService = roomService;
            _documentService = documentService;
        }

        private PagedData<Lesson> GetModel(int pageIndex, int s, int r, string keyword)
        {
            var model = new PagedData<Lesson>();
            int totalRow;
            model.Data = _lessonService.GetLessonByPage(pageIndex, s, r, keyword, GlobalSession.PageSize, out totalRow);
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
            var model = GetModel(0, 0, 0, "");
            return View(model);
        }

        public ActionResult List(int? pageIndex, int s, int r, string keyword)
        {
            var model = GetModel(pageIndex.HasValue ? pageIndex.Value : 0, s, r, keyword);
            return PartialView(model);
        }

        public ActionResult Create()
        {
            GetViewBag(0);
            var model = new Lesson()
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

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Lesson model)
        {
            try
            {
                model.IsDeleted = false;
                model.IsLocked = false;
                model.CreatedDate = DateTime.Now;
                var response = _lessonService.Create(model);

                string html = "";
                if (response.Success)
                {
                    var lst = GetModel(0, 0, 0, "");
                    html = this.RenderPartialView("List", lst);
                }
                return Json(new
                {
                    Html = html,
                    Success = response.Success
                });
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Create Lesson");
                return Json(new { Success = false, Message = "Không thể tạo bài học" });
            }
        }

        [HttpGet]
        public ActionResult GetSubject()
        {
            var model = _subjectService.GetAllSubject();
            return Json(new
            {
                Data = model,
                Success = model != null,
                Message = ""
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetRoom(int id)
        {
            var model = _roomService.GetRoomBySubjectId(id);
            return Json(new
            {
                Data = model,
                Success = model != null,
                Message = ""
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {

            var model = _lessonService.GetLessonById(id);
            GetViewBag(model.SubjectId);
            string html = this.RenderPartialView("Edit", model);
            return Json(new
            {
                Html = html,
                Success = model != null,
                Message = ""
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult UploadDocument(int id)
        {

            var model = _lessonService.GetLessonById(id);
            model.Documents = _documentService.GetDocumentByLesson(id);
            GetViewBag(model.SubjectId);
            string html = this.RenderPartialView("UploadDocument", model);
            return Json(new
            {
                Html = html,
                Success = model != null,
                Message = ""
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UploadDocument(Lesson model)
        {
            try
            {
                _lessonService.UpdateGuideFile(model);
                var response = _documentService.UpdateListDocument(model.Documents, model.LessonId);
                string html = "";
                if (response.Success)
                {
                    var lst = GetModel(0, 0, 0, "");
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
                Logger.LogError(ex, "UploadDocument");
                return Json(new { Success = false, Message = "Không thể tải tài liệu" });
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Lesson model)
        {
            try
            {
                var response = _lessonService.Update(model);
                string html = "";
                if (response.Success)
                {
                    var lst = GetModel(0, 0, 0, "");
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
                Logger.LogError(ex, "Edit Lesson");
                return Json(new { Success = false, Message = "Không thể sửa bài học" });
            }
        }

        //[ValidateInput(false)]
        [HttpPost]
        public ActionResult Lock(int Id, bool islock)
        {
            try
            {
                var response = _lessonService.Lock(Id, islock);
                string html = "";
                if (response.Success)
                {
                    var lst = GetModel(0, 0, 0, "");
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
                Logger.LogError(ex, "_LessonService.Delete");
                return Json(new { Success = false, Message = "Có lỗi" });
            }
        }

        [HttpPost]
        public ActionResult LockLessons(Lesson model)
        {
            try
            {
                var response = _lessonService.LockLessons(model);
                string html = "";
                if (response.Success)
                {
                    //var lst = GetModel(0);
                    //html = this.RenderPartialView("List", lst);
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
                Logger.LogError(ex, "_LessonService.Delete");
                return Json(new { Success = false, Message = "Có lỗi" });
            }
        }

        [HttpPost]
        public ActionResult Delete(int Id)
        {
            try
            {
                var response = _lessonService.Delete(Id);
                string html = "";
                if (response.Success)
                {
                    //var lst = GetModel(0);
                    //html = this.RenderPartialView("List", lst);
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
                Logger.LogError(ex, "_LessonService.Delete");
                return Json(new { Success = false, Message = "Có lỗi" });
            }
        }
        [HttpPost]
        public ActionResult DeleteLessons(Lesson model)
        {
            try
            {
                var response = _lessonService.DeleteLessons(model);
                string html = "";
                if (response.Success)
                {
                    //var lst = GetModel(0);
                    //html = this.RenderPartialView("List", lst);
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
                Logger.LogError(ex, "_LessonService.Delete");
                return Json(new { Success = false, Message = "Có lỗi" });
            }
        }

        protected void GetViewBag(int subid)
        {
            ViewBag.Subjects = _subjectService.GetAllSubject();
            if (subid != 0)
            {
                ViewBag.Rooms = _roomService.GetRoomBySubjectId(subid);
            }

        }

        //Export
        public ActionResult ExportLesson(string tlessonname, int ddsubject, int ddroom)
        {
            int totalRow = 0;
            var data = _lessonService.GetLessonByPage(0, ddsubject, ddroom, tlessonname, 100000000, out totalRow);
            string fileNameSave = "lesson_" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".xlsx";
            string filePath = Server.MapPath("/Images/" + fileNameSave);

            var fileInfo = new FileInfo(filePath);
            using (ExcelPackage excel = new ExcelPackage(fileInfo))
            {

                var sheet = excel.Workbook.Worksheets.Add("Lesson");
                sheet.Column(1).Width = 10;
                sheet.Column(2).Width = 20;
                sheet.Column(3).Width = 50;
                sheet.Column(4).Width = 40;
                sheet.Column(5).Width = 20;
                sheet.Cell(1, 1).Value = "STT";
                sheet.Cell(1, 2).Value = "Mã bài học";
                sheet.Cell(1, 3).Value = "Tên bài học";
                sheet.Cell(1, 4).Value = "Môn học";
                sheet.Cell(1, 5).Value = "Lớp học";
                sheet.Cell(1, 6).Value = "Bài học đang khóa";

                int rowCount = 2; // start row (in row 1 are header cells)

                foreach (Lesson client in data)
                {
                    {

                        {
                            try
                            {
                                sheet.Cell(rowCount, 1).Value = (rowCount - 1).ToString();
                                sheet.Cell(rowCount, 2).Value = client.Code;
                                sheet.Cell(rowCount, 3).Value = client.Name;
                                sheet.Cell(rowCount, 4).Value = client.SubjectName;
                                sheet.Cell(rowCount, 5).Value = client.RoomName;
                                sheet.Cell(rowCount, 6).Value = client.IsLocked ? "Y" : "N";

                                rowCount++;
                            }
                            catch
                            {
                                rowCount++;
                            }
                        }

                    }
                }

                excel.Save();
            }

            return new DownloadResult { VirtualPath = "/Images/" + fileNameSave, FileDownloadName = fileNameSave };

        }
    }
}