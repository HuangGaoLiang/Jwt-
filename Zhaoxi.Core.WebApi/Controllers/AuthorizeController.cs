using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;
using Zhaoxi.Core.WebApi.Utility;

namespace Zhaoxi.Core.WebApi.Controllers
{
    /// <summary>
    /// 鉴权中心
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class AuthorizeController : ControllerBase
    {
        private ILogger<AuthorizeController> _Logger = null;

        //private IJwtHelper _JwtHelper = null;

        private IJWTService _iJWTService = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="iJWTService"></param>
        public AuthorizeController(ILogger<AuthorizeController> logger, IJWTService iJWTService)
        {
            this._Logger = logger;
            //this._JwtHelper = jwtHelper;
            this._iJWTService = iJWTService;
            _Logger.LogInformation($"{nameof(AuthorizeController)} 被构造。。。。 ");
        }

        /// <summary>
        /// 这个是OK的，这个Api并没有要求授权
        /// </summary>
        /// <param name="testClass">测试类</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Get")]
        [AllowAnonymous]
        public IActionResult Get(TestClass testClass)
        {
            return new JsonResult(new
            {
                Data = "这个是OK的，这个Api并没有要求授权！"

            });
        }

        /// <summary>
        /// 根据用户登录信息，获取tokend
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetToken")]
        [AllowAnonymous]
        public ActionResult<string> GetToken([FromQuery]UserInfo userInfo)
        {
            string jwtStr = string.Empty;
            bool suc = false;
            //这里就是用户登陆以后，通过数据库去调取数据，分配权限的操作
            //这里直接写死了

            if (string.IsNullOrEmpty(userInfo.UserName) || string.IsNullOrEmpty(userInfo.Password))
            {
                return new JsonResult(new
                {
                    Status = false,
                    message = "用户名或密码不能为空"
                });
            }

            //TokenModelJWT tokenModel = new TokenModelJWT
            //{
            //    Uid = 1,
            //    //Role = name
            //};

            jwtStr = this._iJWTService.GetToken(userInfo);

            //var jwtStr1 = this._JwtHelper.GetToken(tokenModel);
            suc = true;

            return Ok(new
            {
                success = suc,
                token = jwtStr
            });
        }

        /// <summary>
        /// 测试已授权方法
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAuthorizeData")]
        [Authorize] //Microsoft.AspNetCore.Authorization
        public IActionResult GetAuthorizeData()
        {
            var Name = base.HttpContext.AuthenticateAsync().Result.Principal.Claims.FirstOrDefault(a => a.Type.Contains("Name"))?.Value;

            Console.WriteLine($"this is Name {Name}");

            var ClaimsList = base.HttpContext.AuthenticateAsync().Result.Principal.Claims.ToList();
            return new JsonResult(new
            {
                Data = "已授权",
                Type = "GetAuthorizeData",
                Name
            });
        }
    }
}