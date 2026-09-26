using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace WebApplication.Helper
{
    public static class HtmlExtention
    {
        public static string RenderPartialView(this Controller controller, string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = controller.ControllerContext.RouteData
                    .GetRequiredString("action");

            controller.ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines
                    .FindPartialView(controller.ControllerContext, viewName);
                var viewContext = new ViewContext(controller.ControllerContext,
                    viewResult.View, controller.ViewData, controller.TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }
        public static MvcHtmlString RadioButtonListFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, string id, string name, string strClass, IEnumerable<SelectListItem> items)
        {
            TagBuilder tableTag = new TagBuilder("div");
            tableTag.AddCssClass("control-group");
            var lbTag = new TagBuilder("label");
            lbTag.AddCssClass("control-label bolder blue");
            lbTag.InnerHtml = name;
            tableTag.InnerHtml = lbTag.ToString();
            foreach (var item in items)
            {
                var tdTag = new TagBuilder("div");
                tdTag.AddCssClass("radio");
                var lbTagItem = new TagBuilder("label");
                var rbValue = item.Value;
                var radioTag = helper.RadioButtonFor(expression, rbValue, new { name = name, @Class = strClass });

                var labelTag = new TagBuilder("span");
                labelTag.AddCssClass("lbl");
                labelTag.InnerHtml = item.Text;

                lbTagItem.InnerHtml = radioTag.ToString() + labelTag.ToString();
                tdTag.InnerHtml = lbTagItem.ToString();

                tableTag.InnerHtml += tdTag.ToString();
            }

            return MvcHtmlString.Create(tableTag.ToString());
        }
    }
}