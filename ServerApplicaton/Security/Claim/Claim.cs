using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Security.Claim
{
    public class Claim
    {
        /// <summary>
        /// Initialize the specified identity.
        /// </summary>
        /// <param name="identity">The identity.</param>
        public static void Initializes(Identity identity)
        {
            HttpContext.Current.Session["Identity"] = identity;
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public static void Clear()
        {
            //clear all session data.
            HttpContext.Current.Session.Clear();
        }

        /// <summary>
        /// Gets a value indicating whether this instance is logged.
        /// </summary>
        /// <value><c>true</c> if this instance is logged; otherwise, <c>false</c>.</value>
        public static bool IsLogged
        {
            get
            {
                return GetCurrentThread() != null;
            }
        }

        // <summary>
        // Determines whether the specified controler has permistion.
        // </summary>
        // <param name="controler">The controler.</param>
        // <param name="action">The action.</param>
        // <returns></returns>
        //public static bool HasPermistion(string controler, string action)
        //{
        //    var identity = GetCurrentThread();
        //    if (identity != null && identity.RightItems != null && identity.RightItems.Count > 0)
        //    {
        //        foreach (var item in identity.RightItems)
        //        {
        //            string iaction = "index";
        //            if (!string.IsNullOrEmpty(item.RightController) && item.RightController.ToLower().Similar(controler))
        //            {
        //                if (!string.IsNullOrEmpty(item.RightAction))
        //                    iaction = item.RightAction;
        //                if (iaction.Similar(action))
        //                    return true;
        //            }
        //        }
        //    }
        //    else
        //        return false;

        //    return false;
        //}

        /// <summary>
        /// Gets the user id.
        /// </summary>
        /// <value>The user id.</value>
        public static long UserId
        {
            get
            {
                if (IsLogged)
                    return GetCurrentThread().UserId;
                return 0;
            }
        }

        /// <summary>
        /// Gets the display name of the user.
        /// </summary>
        /// <value>The display name of the user.</value>
        public static string DisplayName
        {
            get
            {
                if (IsLogged)
                    return GetCurrentThread().DisplayName;
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        public static string UserName
        {
            get
            {
                if (IsLogged)
                    return GetCurrentThread().UserName;
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <returns>Identity.</returns>
        public static Identity GetCurrentThread()
        {
            if (HttpContext.Current != null)
            {
                //try to get identity from session.
                var identity = HttpContext.Current.Session["Identity"] as Identity;
                if (identity != null)
                    return identity;
            }
            //return null if doesn't exist.
            return null;
        }
    }
}
