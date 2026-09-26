using System.Web;
using System.Web.Mvc;
using Model;

namespace Security
{
    public class RequiredLoginAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            {
                if (HttpContext.Current != null)
                {
                    //try to get identity from session.
                    User identity = HttpContext.Current.Session["UserSession"] as User;
                    if (identity == null)
                    {
                        filterContext.Result =
                            new RedirectResult(string.Format("/Admin/Home/Login",
                                HttpContext.Current.Server.UrlEncode(HttpContext.Current.Request.Url.PathAndQuery)));
                    }
                }
            }
        }
    }
}
