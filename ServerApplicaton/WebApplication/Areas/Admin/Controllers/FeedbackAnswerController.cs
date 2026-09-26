using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication.Areas.Admin.Controllers
{
    public class FeedbackAnswerController : Controller
    {
        //
        // GET: /Admin/FeedbackAnswer/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ShowAnswer(int feedbackid)
        {
            return View();
        }
    }
}
