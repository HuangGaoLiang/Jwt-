using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Mvc;

namespace Zhaoxi.AspNet.WebApi.Utility
{
    public class CustomExceptionFilterAttribute :  ExceptionFilterAttribute
    {
        public void OnException(ExceptionContext filterContext)
        {
            Console.WriteLine("this is CustomExceptionFilterAttribute.OnException");
        }
    }
}