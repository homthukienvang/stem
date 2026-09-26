using System;
using System.Web.Mvc;
using DataModel.Base;
using Extensions;
using Model;
using Security;
using Services;
using WebApplication.Helper;

namespace WebApplication.Areas.Admin.Controllers
{
    public class SubjectController : Controller
    {
        private readonly ISubjectService _subjectService;
        public SubjectController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        private PagedData<Subject> GetModel(int pageIndex)
        {
            var model = new PagedData<Subject>();
            int totalRow;
            model.Data = _subjectService.GetSubjectByPage(pageIndex, GlobalSession.PageSize, out totalRow);
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
            var model = GetModel(0);
            return View(model);
        }

        public ActionResult List(int? pageIndex)
        {
            var model = GetModel(pageIndex.HasValue ? pageIndex.Value : 0);
            return PartialView(model);
        }

        public ActionResult Create()
        {
            GetViewBag();
            var model = new Subject()
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
        public ActionResult Create(Subject model)
        {
            try
            {
                model.IsDeleted = false;
                model.IsLocked = false;
                model.CreatedDate = DateTime.Now;
                var response = _subjectService.Create(model);

                string html = "";
                if (response.Success)
                {
                    var lst = GetModel(0);
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
                Logger.LogError(ex, "Create Subject");
                return Json(new { Success = false, Message = "Không thể tạo môn học" });
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            GetViewBag();
            var model = _subjectService.GetSubjectById(id);
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
        public ActionResult Edit(Subject model)
        {
            try
            {
                var response = _subjectService.Update(model);
                string html = "";
                if (response.Success)
                {
                    var lst = GetModel(0);
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
                Logger.LogError(ex, "Edit Subject");
                return Json(new { Success = false, Message = "Không thể sửa môn học" });
            }
        }

        //[ValidateInput(false)]
        [HttpPost]
        public ActionResult Delete(int Id)
        {
            try
            {
                var response = _subjectService.Delete(Id);
                string html = "";
                if (response.Success)
                {
                    var lst = GetModel(0);
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
                Logger.LogError(ex, "_SubjectService.Delete");
                return Json(new { Success = false, Message = "Có lỗi" });
            }
        }

        protected void GetViewBag()
        {
        }
    }
}