using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;

namespace Security
{
    public class RequiredRightAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            {
                if (HttpContext.Current != null)
                {
                    //try to get identity from session.
                    var isPassed = false;
                    //check passed url
                    var rights = HttpContext.Current.Session["UserRight"];
                    if (rights != null)
                    {
                        var lstRight = ((List<Right>) rights).Where(x => x.Url != null).ToList();
                        var crUrlLower = HttpContext.Current.Request.RawUrl.ToLower();
                        isPassed = lstRight.Exists(x => crUrlLower == x.Url.ToLower());
                    }

                    if (!isPassed)
                    {
                        filterContext.Result =
                            new RedirectResult(string.Format("/Admin/Home/Error",
                                HttpContext.Current.Server.UrlEncode(HttpContext.Current.Request.Url.PathAndQuery)));

                    }
                }
            }
        }
    }
}
