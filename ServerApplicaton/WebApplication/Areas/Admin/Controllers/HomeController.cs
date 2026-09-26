using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DataModel.Base;
using Extensions;
using Model;
using Security;
using Services;
using WebApplication.Helper;

namespace WebApplication.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRightService _accountService;

        private IUserService _userService;
        public HomeController(IUserService userService)
        {
            _userService = userService;
        }
        [RequiredLogin]
        public ActionResult MenuSideBar()
        {
            var model = Session["UserRight"] as List<Right>;
            return PartialView(model);
        }
        [RequiredLogin]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Error()
        {
            return View();
        }
        public ActionResult LogOut()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Home");
        }
        public ActionResult Login()
        {
            var user = new User();
            return View();
        }
        [HttpPost]
        public ActionResult Login(User model)
        {

            try
            {
                User response = _userService.Login(model);
                if (response == null)
                {

                    return Json(new
                    {
                        Success = false,
                        Message = "Tên đăng nhập hoặc mật khẩu không chính xác"
                    });
                }

                Session["UserSession"] = response;
                Session["UserId"] = response.UserId;
                Session["CityCode"] = response.CityCode;
                Session["DistrictCode"] = response.DistrictCode;

                var lstRight = _userService.GetRight(response.UserId);
                Session["UserRight"] = lstRight;

                return Json(new
                {
                    Success = true
                });
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Login");
                return Json(new { Success = false, error = "Đăng nhập không thành công" });
            }
        }

        #region file upload

        public FilePathResult Image()
        {
            string filename = Request.Url.AbsolutePath.Replace("/home/image", "");
            string contentType = "";
            var filePath = new FileInfo(Server.MapPath("~/App_Data") + filename);

            int index = filename.LastIndexOf(".") + 1;
            string extension = filename.Substring(index).ToUpperInvariant();

            // Fix for IE not handling jpg image types
            contentType = string.Compare(extension, "JPG") == 0 ? "image/jpeg" : string.Format("image/{0}", extension);

            return File(filePath.FullName, contentType);
        }

        [HttpPost]
        public ContentResult UploadImages()
        {
            var r = new List<UploadFilesResult>();

            foreach (string file in Request.Files)
            {
                HttpPostedFileBase hpf = Request.Files[file];
                if (hpf.ContentLength == 0)
                    continue;
                string lstfile = UploadHelper.UploadCropAndResizeImage(hpf, Server);
                r.Add(new UploadFilesResult
                {
                    Url = hpf.FileName,
                    Name = lstfile,
                    Length = hpf.ContentLength,
                    Type = hpf.ContentType
                });
            }
            return
                Content(
                    "{\"name\":\"" + r[0].Name + "\",\"url\":\"" + r[0].Url + "\",\"type\":\"" +
                    string.Format("{0} bytes", r[0].Type) + "\"}", "application/json");
        }

        [HttpPost]
        public ContentResult UploadFiles()
        {
            var r = new List<UploadFilesResult>();

            try
            {
                foreach (string file in Request.Files)
                {
                    HttpPostedFileBase hpf = Request.Files[file];
                    if (hpf.ContentLength == 0)
                        continue;
                    string lstfile = UploadHelper.UploadFile(hpf, GlobalSession.FileFolder, Server);
                    r.Add(new UploadFilesResult
                    {
                        Url = hpf.FileName,
                        Name = lstfile,
                        Length = hpf.ContentLength,
                        Type = hpf.ContentType
                    });
                }
               
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Edit Client");
                r.Add(new UploadFilesResult
                {
                    Url = "",
                    Name = "",
                    Length = 0 
                });
            }
            return
                   Content(
                       "{\"name\":\"" + r[0].Name + "\",\"url\":\"" + r[0].Url + "\",\"type\":\"" +
                       string.Format("{0} bytes", r[0].Type) + "\"}", "application/json");
        }

        #endregion

        [AllowAnonymous]
        [HttpPost]
        public JsonResult KeepAlive()
        {
            try
            {
                Tracking();
                Session["keepalive"] = DateTime.Now;

                return Json(new
                {
                    Success = true
                });
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, error = "Hết phiên đăng nhập. Vui lòng đăng nhập lại" });
            }
        }
        public void Tracking()
        {
            var session = Session["UserSession"];
            if (session == null)
            {
                FormsAuthentication.SignOut();
                Session.Abandon();
                //var url = System.Configuration.ConfigurationManager.AppSettings["RootUrl"] + "Admin/Home/Login";
                Response.Redirect("/Admin/Home/Login");
            }
        }
    }
}
