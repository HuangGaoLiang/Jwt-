using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
 

namespace Zhaoxi.AspNet.WebApi.Utility
{
    public class CustomActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            Console.WriteLine("this is CustomActionFilterAttribute.OnActionExecuted");
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            Console.WriteLine("this is CustomActionFilterAttribute.OnActionExecuting");
        }
    }
}