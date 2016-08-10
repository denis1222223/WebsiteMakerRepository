using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace CourseProject.Filters
{
    public class LanguageAttribute : FilterAttribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string language = GetLanguageFromCookie(filterContext.HttpContext.Request.Cookies["language"]);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(language);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(language);
        }

        private string GetLanguageFromCookie(HttpCookie languageCookie)
        {
            if (languageCookie != null)
                return languageCookie.Value;
            else
                return "ru";
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }
    }
}