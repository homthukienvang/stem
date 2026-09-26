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
    public class RoomController : Controller
    {
        private readonly IRoomService _roomService;
        private readonly ISubjectService _subjectService;
        public RoomController(IRoomService roomService, ISubjectService subjectService)
        {
            _roomService = roomService;
            _subjectService = subjectService;
        }

        private PagedData<Room> GetModel(int pageIndex)
        {
            var model = new PagedData<Room>();
            int totalRow;
            model.Data = _roomService.GetRoomByPage(pageIndex, GlobalSession.PageSize, out totalRow);
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
            var model = new Room()
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
        public ActionResult Create(Room model)
        {
            try
            {
                model.IsDeleted = false;
                model.IsLocked = false;
                model.CreatedDate = DateTime.Now;
                var response = _roomService.Create(model);

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
                Logger.LogError(ex, "Create Room");
                return Json(new { Success = false, Message = "Không thể tạo môn học" });
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            GetViewBag();
            var model = _roomService.GetRoomById(id);
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
        public ActionResult Edit(Room model)
        {
            try
            {
                var response = _roomService.Update(model);
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
                Logger.LogError(ex, "Edit Room");
                return Json(new { Success = false, Message = "Không thể sửa môn học" });
            }
        }

        //[ValidateInput(false)]
        [HttpPost]
        public ActionResult Delete(int Id)
        {
            try
            {
                var response = _roomService.Delete(Id);
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
                Logger.LogError(ex, "_RoomService.Delete");
                return Json(new { Success = false, Message = "Có lỗi" });
            }
        }

        protected void GetViewBag()
        {
            ViewBag.Subjects = _subjectService.GetAllSubject();
        }
    }
}