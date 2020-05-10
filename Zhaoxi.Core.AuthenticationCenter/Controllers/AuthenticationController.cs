using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Zhaoxi.Core.AuthenticationCenter.Utility;

namespace Zhaoxi.Core.AuthenticationCenter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    { 
        #region 构造函数，依赖注入
        private ILogger<AuthenticationController> _logger = null;
        private IJWTService _iJWTService = null;
        private readonly IConfiguration _iConfiguration;
        public AuthenticationController(ILoggerFactory factory,
            ILogger<AuthenticationController> logger,
            IConfiguration configuration
            , IJWTService service)
        {
            this._logger = logger;
            this._iConfiguration = configuration;
            this._iJWTService = service;
        }
        #endregion
        [Route("Get")]
        [HttpGet]
        public IEnumerable<int> Get()
        {
            return new List<int>() { 1, 2, 3, 4, 6, 7 };
        }

        /// <summary>
        /// 登录获取token
        /// </summary>
        /// <param name="name">HuangGaoLiang</param>
        /// <param name="password">123456</param>
        /// <returns></returns>
        [Route("Login")]
        [HttpGet]
        public string Login(string name, string password)
        {
            ///这里肯定是需要去连接数据库做数据校验
            if ("HuangGaoLiang".Equals(name) && "123456".Equals(password))//应该数据库
            {
                string token = this._iJWTService.GetToken(name);
                return JsonConvert.SerializeObject(new
                {
                    result = true,
                    token
                });
            }
            else
            {
                return JsonConvert.SerializeObject(new
                {
                    result = false,
                    token = ""
                });
            }
        }
    }
}