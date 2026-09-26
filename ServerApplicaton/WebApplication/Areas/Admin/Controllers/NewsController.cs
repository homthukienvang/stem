using System.Web.Mvc;
using Model;
using Security;
using Services;

namespace WebApplication.Areas.Admin.Controllers
{
    public class NewsController : Controller
    {
        private INewsService _newsService;

        public NewsController(INewsService newsService)
        {
            this._newsService = newsService;
        }
        [RequiredLogin]
        [RequiredRight]
        public ActionResult Index()
        {
            return View();
        }

        [RequiredLogin]
        [RequiredRight]
        public ActionResult NewInfo(int id)
        {
            var model = _newsService.GetNewsById(id);
          
            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult NewInfo(News model)
        {
            var res = _newsService.Update(model);
            ModelState.AddModelError("",res.Success?"Cập nhật thành công.": res.Message);
            model = _newsService.GetNewsById(model.NewsId); 
            return View(model);
        }

        [RequiredLogin]
        [RequiredRight]
        public ActionResult UserGuide(int id)
        {
            var model = _newsService.GetNewsById(id);
          
            return View(model);
        }
        [HttpPost]
        public ActionResult UserGuide(News model)
        {
            var res = _newsService.Update(model);
            ModelState.AddModelError("",res.Success?"Cập nhật thành công.": res.Message);
            model = _newsService.GetNewsById(model.NewsId); 
            return View(model);
        }

    }
}
