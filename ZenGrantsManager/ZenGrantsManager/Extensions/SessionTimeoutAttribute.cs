﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZenGrantsManager.Extensions
{
    public class SessionTimeoutAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext ctx = HttpContext.Current;
            if (HttpContext.Current.Session["accessToken"] == null)
            {
                filterContext.Result = new RedirectResult("~/Login/Index");
                
                return;
            }
            base.OnActionExecuting(filterContext);
        }
    }
}