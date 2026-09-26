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
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private IRoleService _roleServicel;
        public UserController(IUserService userService, IRoleService roleServicel)
        {
            _userService = userService;
            _roleServicel = roleServicel;
        }

        private void GetViewBag()
        {
            //ViewBag.Rights = new SelectList(_rightService.GetActiveRight(), "RightId", "RightName");
        }
        private PagedData<User> GetModel(int pageIndex)
        {
            var model = new PagedData<User>();
            model.Data= _userService.GetAllUser();
             
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
            var model = new User() { UserActivated = true };
            model.Roles = _roleServicel.GetAllRole().ToList();
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
        public ActionResult Create(User model)
        {
            try
            {
                var response = _userService.Create(model);
                string html ="";
                if (response.Success)
                    html = this.RenderPartialView("List", GetModel(0));
                return Json(new
                {
                    Html = html,
                    Success = response.Success,
                    response.Message
                });
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Create User");
                return Json(new { Success = false, Message = "Unable to add User at this time." });
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            GetViewBag();
            var model = _userService.GetUserById(id);
            model.ConfirmPassword = model.Password;
            string html = this.RenderPartialView("Edit", model);
            return Json(new
            {
                Html = html,
                Success = true,
                Message = ""
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(User model)
        {
            try
            {
                var response = _userService.Update(model);
                string html = "";
                if (response.Success)
                {
                    html = this.RenderPartialView("List", GetModel(0));
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
                Logger.LogError(ex, "Edit User");
                return Json(new { Success = false, error = "Unable to add User at this time." });
            }
        }

        //[ValidateInput(false)]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                var response = _userService.Delete(id);
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
                Logger.LogError(ex, "_UserService.Delete");
                return Json(new { Success = false, error = "Có lỗi" });
            }
        }

        [HttpGet]
        public ActionResult ResetPassword(int id)
        {
            GetViewBag();
            var model = _userService.GetUserById(id);
            model.ConfirmPassword = "";
            model.Password = "";
            string html = this.RenderPartialView("ResetPassword", model);
            return Json(new
            {
                Html = html,
                Success = true,
                Message = ""
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ResetPassword(User model)
        {
            try
            {
                var response = _userService.ResetPassword(model);
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
                Logger.LogError(ex, "Edit User");
                return Json(new { Success = false, error = "Unable to add User at this time." });
            }
        }

    }
}