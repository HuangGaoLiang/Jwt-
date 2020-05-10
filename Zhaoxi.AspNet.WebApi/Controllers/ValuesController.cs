using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Zhaoxi.AspNet.WebApi.Utility;
using Zhaoxi.MVC5.OpenNew.Utility;

namespace Zhaoxi.AspNet.WebApi.Controllers
{
    public class ValuesController : ApiController
    {
        public ValuesController()
        {
            Console.WriteLine("this is ValuesController 被构造。。。");
        }

        // GET api/values

        [CustomAuthorizationFilterAttribute]
        [CustomActionFilterAttribute]
        [CustomExceptionFilterAttribute] 
        [Route("api/Values/Get")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }  
    }
}
