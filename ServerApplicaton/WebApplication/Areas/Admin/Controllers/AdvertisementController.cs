using System;
using System.Web.Mvc;
using Model;
using Services;
using Security;
using DataModel.Base;
using Extensions;

namespace WebApplication.Areas.Admin.Controllers
{
    public class AdvertisementController : Controller
    {
        //
        // GET: /Admin/Advertisement/

        private IAdvertisementService _advService;

        public AdvertisementController(IAdvertisementService advService)
        {
            this._advService = advService;
        }
        [RequiredLogin]
        [RequiredRight]
        public ActionResult Index()
        {
            return View();
        }
        [RequiredLogin]
        [RequiredRight]
        public ActionResult AdvInfo(int id)
        {
            var model = _advService.GetAdvById(id);

            return View(model);
        }
        [HttpPost]
        public ActionResult AdvInfo(Advertisement model)
        {
            model.StartDate = DateTime.Now;
            model.EndDate = DateTime.Now;

            var res = _advService.Update(model);
            ModelState.AddModelError("", res.Success ? "Cập nhật thành công." : res.Message);
            model = _advService.GetAdvById(model.Id);
            return View(model);
        }

        [RequiredLogin]
        [RequiredRight]
        public ActionResult AdvertisementUser(int id)
        {
            var model = _advService.GetAdvById(id);
            return View(model);
        }
        [HttpPost]
        public ActionResult AdvertisementUser(Advertisement model)
        {
            model.StartDate = DateTime.Now;
            model.EndDate = DateTime.Now;
            var res = _advService.Update(model);
            ModelState.AddModelError("", res.Success ? "Cập nhật thành công." : res.Message);
            model = _advService.GetAdvById(model.Id);
            return View(model);
        }


        private PagedData<AdvReport> GetModel(int pageIndex, int reportType, string searchvalue, string fromdate, string todate, int advid)
        {
            var model = new PagedData<AdvReport>();
            int totalRow;

            model.Data = _advService.GetAdvByPage(pageIndex, reportType, searchvalue, fromdate, todate, advid, GlobalSession.PageSize, out totalRow);
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
        public ActionResult ReportIndex()
        {
            var model = GetModel(0, 0, "", "", "", 1);
            return View(model);
        }

        //public ActionResult ListReportView(int? pageIndex, int reporttype, string searchvalue, string fromdate, string todate, int advid)
        //{
        //    var model = GetModel(pageIndex.HasValue ? pageIndex.Value : 0, reporttype, searchvalue, fromdate, todate, advid);
        //    return PartialView(model);
        //}

        public ActionResult List(int? pageIndex, int reporttype, string searchvalue, string fromdate, string todate, int advid)
        {
            var model = GetModel(pageIndex.HasValue ? pageIndex.Value : 0, reporttype, searchvalue, fromdate, todate, advid);
            return PartialView(model);
        }
    }
}
