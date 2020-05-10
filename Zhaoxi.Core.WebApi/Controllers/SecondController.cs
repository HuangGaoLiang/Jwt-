using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Zhaoxi.Core.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecondController : ControllerBase
    {

        private ILogger<SecondController> _Logger = null;

        public SecondController(ILogger<SecondController> logger)
        {
            this._Logger = logger;
            _Logger.LogInformation("SecondController 被构造。。。。");
        }

        /// <summary>
        /// 测试
        /// </summary>
        /// <returns></returns>
        [Route("Get")]
        [HttpGet]
        public string Get()
        {
            _Logger.LogInformation("SecondController.Get 被调用");
            return "this is Zhaoxi--003";
        }
    }
}