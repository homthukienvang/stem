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
    //[RequiredLogin]
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;
        private IRightService _rightService;
        public RoleController(IRoleService roleService, IRightService rightService)
        {
            _roleService = roleService;
            _rightService = rightService;
        }

        private void GetViewBag()
        {
            //ViewBag.Rights = new SelectList(_rightService.GetActiveRight(), "RightId", "RightName");
        }
        private PagedData<Role> GetModel(int pageIndex)
        {
            var model = new PagedData<Role>();
            model.Data= _roleService.GetAllRole();
             
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
            var model = new Role() { RoleActivated = true };
            model.Rights = _rightService.GetActiveRight();
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
        public ActionResult Create(Role model)
        {
            try
            {
                var response = _roleService.Create(model);
                var modellist = GetModel(0);
                string html = this.RenderPartialView("List", modellist);
                return Json(new
                {
                    Html = html,
                    Success = response.Success
                });
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Create Role");
                return Json(new { Success = false, error = "Unable to add Role at this time." });
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            GetViewBag();
            var model = _roleService.GetRoleById(id);
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
        public ActionResult Edit(Role model)
        {
            try
            {
                var response = _roleService.Update(model);
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
                Logger.LogError(ex, "Edit Role");
                return Json(new { Success = false, error = "Unable to add Role at this time." });
            }
        }

        //[ValidateInput(false)]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                var response = _roleService.Delete(id);
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
                Logger.LogError(ex, "_RoleService.Delete");
                return Json(new { Success = false, error = "Có lỗi" });
            }
        }

    }
}