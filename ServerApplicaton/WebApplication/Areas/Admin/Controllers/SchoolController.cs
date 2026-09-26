using DataModel.Base;
using Extensions;
using Security;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication.Helper;
using Model;

namespace WebApplication.Areas.Admin.Controllers
{
    public class SchoolController : Controller
    {
        private readonly ISchoolService _schoolService;
        private readonly ISchoolLogService _schoolLogService = new SchoolLogService();

        public SchoolController(ISchoolService schoolService)
        {
            _schoolService = schoolService;
        }

        private PagedData<School> GetModel(int pageIndex, string schoolCode, string citycode, string districtcode, string deploymentmethod)
        {
            var model = new PagedData<School>();
            int totalRow;
            string usercitycode = Session["CityCode"] + "";
            string userdistrictcode = Session["DistrictCode"] + "";

            model.Data = _schoolService.GetByPage(pageIndex, GlobalSession.PageSize, schoolCode, citycode, districtcode, usercitycode, userdistrictcode, deploymentmethod, out totalRow);
            model.Pagination = new PaginationModel
            {
                CurrentPage = pageIndex,
                PageSize = GlobalSession.PageSize,
                NumberOfRows = totalRow
            };
            return model;
        }

        //
        // GET: /Admin/School/

        [RequiredLogin]
        [RequiredRight]
        public ActionResult Index()
        {
            var model = GetModel(0, "", "", "", "");
            return View(model);
        }

        public ActionResult List(int? pageIndex, string schoolCode, string citycode, string districtcode,string deploymentmethod)
        {
            TempData["pageIndex"] = pageIndex;
            TempData["schoolCode"] = schoolCode;
            TempData["citycode"] = citycode;
            TempData["districtcode"] = districtcode;
            TempData["deploymentmethod"] = deploymentmethod;

            var model = GetModel(pageIndex.HasValue ? pageIndex.Value : 0, schoolCode, citycode, districtcode, deploymentmethod);
            return PartialView(model);
        }

        public ActionResult Create()
        {
            var model = new School()
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
        public ActionResult Create(School model)
        {
            try
            {
                var response = _schoolService.Create(model);
                var modellist = GetModel(0, "", "", "", "");
                string html = "";
                if (response.Success)
                {

                    SchoolLog log = new SchoolLog();
                    log.UserId = Convert.ToInt32(Session["UserId"] + "");
                    log.SchoolId = model.SchoolId;
                    log.CreatedDate = DateTime.Now;
                    log.Description = model.GetDiff();

                    ISchoolLogService logsvr = new SchoolLogService();
                    logsvr.Create(log);


                    html = this.RenderPartialView("List", modellist);
                    return Json(new
                    {
                        Html = html,
                        Success = response.Success,
                        response.Message
                    });
                }
                else
                {
                    return Json(new { Success = false, Message = response.Message });
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Create school");
                return Json(new { Success = false, Message = "Unable to add school at this time." });
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(School model)
        {
            try
            {
                var response = _schoolService.Update(model);
                string html = "";

                int pageIndex = int.Parse(string.IsNullOrEmpty(TempData["pageIndex"] + "") ? "0" : TempData["pageIndex"] + "");
                string schoolCode = TempData["schoolCode"] + "";
                string citycode = TempData["citycode"] + "";
                string districtcode = TempData["districtcode"] + "";
                string deploymentmethod = TempData["deploymentmethod"] + "";

                if (response.Success)
                {
                    var lst = GetModel(pageIndex, schoolCode, citycode, districtcode, deploymentmethod);
                    html = this.RenderPartialView("List", lst);

                    if (TempData["EditSchool"] != null)
                    {
                        School school = (School)TempData["EditSchool"];
                        model.OldSchool = school;
                        SchoolLog log = new SchoolLog();
                        log.UserId = Convert.ToInt32(Session["UserId"] + "");
                        log.SchoolId = model.SchoolId;
                        log.CreatedDate = DateTime.Now;
                        log.Description = model.GetDiff();

                        ISchoolLogService logsvr = new SchoolLogService();
                        logsvr.Create(log);
                    }
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
                Logger.LogError(ex, "Edit School");
                return Json(new { Success = false, error = "Unable to edit school at this time." });
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var model = _schoolService.GetById(id);
            TempData["EditSchool"] = model;
            string html = this.RenderPartialView("Edit", model);
            return Json(new
            {
                Html = html,
                Success = model != null,
                Message = ""
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ShowLog(int id)
        {
            var model = _schoolService.GetById(id);
            model.SchoolLogs = _schoolLogService.GetBySchoolId(id);
            
            string html = this.RenderPartialView("ShowLog", model);
            return Json(new
            {
                Html = html,
                Success = model != null,
                Message = ""
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
