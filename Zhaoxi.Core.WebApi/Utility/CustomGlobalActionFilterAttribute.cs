using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zhaoxi.Core.WebApi.Utility
{
    public class CustomGlobalActionFilterAttribute : Attribute, IActionFilter //ActionFilterAttribute //IActionFilter,//IAsyncActionFilter
    {

        private ILogger<CustomGlobalActionFilterAttribute> _Logger = null;

        public CustomGlobalActionFilterAttribute(ILogger<CustomGlobalActionFilterAttribute> logger)
        {
            _Logger = logger;
        } 

        /// <summary>
        /// 方法执行后
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuted(ActionExecutedContext context)
        {  
            var result = context.Result;
            ObjectResult objectResult= result as ObjectResult; 
            var resultLog = $"{DateTime.Now} 调用 {context.RouteData.Values["action"]} api 完成；执行结果：{Newtonsoft.Json.JsonConvert.SerializeObject(objectResult.Value)}"; 
            _Logger.LogInformation(resultLog); 
            //Console.WriteLine("this is CustomIActionFilterAttribute.OnActionExecuted");
        }

        /// <summary>
        /// 方法执行前
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuting(ActionExecutingContext context)
        { 
            var actionLog = $"{DateTime.Now} 开始调用 {context.RouteData.Values["action"]} api；参数为：{Newtonsoft.Json.JsonConvert.SerializeObject(context.ActionArguments)}"; 
            _Logger.LogInformation(actionLog);

            //这里就可以把信息写入到日志中来；
            //Log4Net
            //Console.WriteLine("this is CustomIActionFilterAttribute.OnActionExecuting");
        }

        //public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
