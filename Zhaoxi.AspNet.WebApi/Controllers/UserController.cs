using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Security;

namespace Zhaoxi.AspNet.WebApi.Controllers
{
    public class UserController : ApiController
    {
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpGet]
        public string Login(string userName, string password)
        {
            if (!ValidateUser(userName, password)) //用户名密码验证通过以后
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(new { bRes = false });
            }
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(0, userName, DateTime.Now,
                            DateTime.Now.AddHours(20), true, string.Format("{0}&{1}", userName, password),
                            FormsAuthentication.FormsCookiePath);
            return Newtonsoft.Json.JsonConvert.SerializeObject(new { bRes = true, Ticket = FormsAuthentication.Encrypt(ticket) }); ;
        }

        //校验用户名密码（正式环境中应该是数据库校验）
        private bool ValidateUser(string strUser, string strPwd)
        {
            if (strUser == "Richard" && strPwd == "123456") //这里是做demo  所以这里是一些伪代码，正式环境上肯定需要到数据库里去做验证的
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
