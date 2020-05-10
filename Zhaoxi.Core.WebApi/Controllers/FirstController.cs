using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Zhaoxi.Core.WebApi.Controllers
{
    /// <summary>
    /// 严格遵循RestFull 风格 get、post、put、delete
    /// </summary>
    [Route("api/[controller]")]
    [ApiController] // First 当成一个资源  对完提供增删改查的Api
    public class FirstController : ControllerBase
    {
        private ILogger<FirstController> _Logger =null;

        public FirstController(ILogger<FirstController> logger)
        {
            this._Logger = logger;
            _Logger.LogInformation($"{typeof(FirstController)} 被构造..... ");
        }

        [Route("Get")]
        [HttpGet]
        public string Get()
        {
            return "朝夕教育--RenChen";
        }


        [Route("Info")]
        [HttpGet]
        public string Info()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                Id = 1223,
                Name = "ynw"
            });
        }

        [Route("GetInfo")]
        [HttpGet]
        public string GetInfo(int id,string name)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                Id = id,
                Name = name
            });
        }

        [Route("GetStr")]
        [HttpGet]
        public string GetStr()
        {
            return "朝夕教育003";
        }
    }
}