using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Zhaoxi.Core.WebApi.Utility;

namespace Zhaoxi.Core.WebApi.Controllers
{
    //能不能注册以后让所有的Api 都生效？
    [Route("api/[controller]")]
    [ApiController]//2.对控制器下的所有Api生效
    //[ServiceFilter(typeof(CustomActionFilterAttribute))] //需要注册服务
   [ServiceFilter(typeof(CustomControllerActionFilterAttribute))]
    public class FilterController : ControllerBase
    {
        public FilterController()
        {
            Console.WriteLine("FilterController 被构造。。。");
        }

        [Route("Get")]
        [HttpGet]
        [CustomResourceFilterAttribute]
        public string Get()
        {
            return "this is Zhaoxi--003";
        }

        [Route("GetInfo")]
        [HttpGet]
        [CustomResourceFilterAttribute]
        public string GetInfo()
        {
            return $"this is Zhaoxi Open  {DateTime.Now}";
        }
        //难道每个方法都需要去注册一下吗？

        [Route("GetInfoByParamter")]
        [HttpGet]
        //[CustomIActionFilter] //特性的方式标记  只能支持带无参数构造函数的
        //[TypeFilter(typeof(CustomActionFilterAttribute))]//不需要注册服务
        //[ServiceFilter(typeof(CustomActionFilterAttribute))] //需要注册服务
        //[CustomIOCFilterFactory(typeof(CustomActionFilterAttribute))]  //需要注册服务1.方法注册
        [ServiceFilter(typeof(CustomActionFilterAttribute))]
        public string GetInfoByParamter(int id,string Name)
        {
            return $"this is Id={id},Name={Name}";
        }
    }
}