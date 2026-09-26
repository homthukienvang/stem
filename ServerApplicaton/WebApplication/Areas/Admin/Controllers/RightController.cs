using System;
using System.Linq;
using System.Web.Mvc;
using DataModel.Base;
using Extensions;
using Model;
using Security;
using Services;
using WebApplication.Helper;

namespace WebApplication.Areas.Admin.Controllers
{
    // [RequiredLogin]
    public class RightController : Controller
    {
        private readonly IRightService _rightService;

        public RightController(IRightService rightService)
        {
            _rightService = rightService;
        }
        private void GetViewBag()
        {
            var ar = _rightService.GetAllRight().Where(x => x.ParentId == null).OrderBy(x => x.Order);
            ViewBag.ParentRights = new SelectList(ar, "RightId", "RightName");
        }
        private PagedData<Right> GetModel(int pageIndex)
        {
            var model = new PagedData<Right>();
            int totalRow;
            model.Data = _rightService.GetAllRight();

            model.Pagination = new PaginationModel
            {
                CurrentPage = pageIndex,
                PageSize = GlobalSession.PageSize,
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
            var model = new Right() { Status = true };
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
        public ActionResult Create(Right model)
        {
            try
            {
                var response = _rightService.Create(model);
                return Json(new
                {
                    Success = response.Success
                });
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Create Right");
                return Json(new { Success = false, error = ex.Message });
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            GetViewBag();
            var model = _rightService.GetRightById(id);
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
        public ActionResult Edit(Right model)
        {
            try
            {
                var response = _rightService.Update(model);
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
                Logger.LogError(ex, "Edit Right");
                return Json(new { Success = false, error = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                var currUser = (int)Session["UserId"];
                var response = _rightService.Delete(id, currUser);
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
                Logger.LogError(ex, "_RightService.Delete");
                return Json(new { Success = false, error = ex.Message });
            }
        }

    }
}