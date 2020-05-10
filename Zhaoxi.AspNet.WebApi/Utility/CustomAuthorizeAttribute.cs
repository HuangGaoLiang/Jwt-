using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Security;
using Zhaoxi.AspNet.WebApi.Utility;

namespace Zhaoxi.MVC5.OpenNew.Utility
{
    /// <summary>
    /// 自定义了一个最简单的Filter
    /// </summary>
    public class CustomAuthorizationFilterAttribute : AuthorizationFilterAttribute 
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var authorization = actionContext.Request.Headers.Authorization;
            if (authorization == null)
            {
                this.HandlerUnAuthorization();
            }
            else if (this.ValidateTicket(authorization.Parameter))
            {
                return;//继续
            }
            else
            {
                this.HandlerUnAuthorization();
            }
        }
        private void HandlerUnAuthorization()
        {
            throw new HttpResponseException(System.Net.HttpStatusCode.Unauthorized);
        }
        private bool ValidateTicket(string encryptTicket)
        {
            try
            {
                var strTicket = FormsAuthentication.Decrypt(encryptTicket).UserData;
                return string.Equals(strTicket, string.Format("{0}&{1}", "Richard", "123456"));//因为这里是做demo,所以是伪代码，正式环境必然是要到数据库验证的；
            }
            catch (Exception ex)
            {
                return false;
            }

        }
    }
}