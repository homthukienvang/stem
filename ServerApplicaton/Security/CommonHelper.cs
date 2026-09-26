using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Security
{
    public class CommonHelper
    {
        public static string GetClientIpAddress()
        {
            var userHostAddress = HttpContext.Current.Request.UserHostAddress;
            if (userHostAddress != null && userHostAddress.Equals(""))
            {
                userHostAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                if (userHostAddress.Equals(""))
                    userHostAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            }
            return userHostAddress;
        }
    }
}
