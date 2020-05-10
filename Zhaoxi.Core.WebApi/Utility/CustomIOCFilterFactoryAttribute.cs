using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zhaoxi.Core.WebApi.Utility
{
    public class CustomIOCFilterFactoryAttribute : Attribute, IFilterFactory
    {
        private Type _FilterType = null;
        public CustomIOCFilterFactoryAttribute(Type type)
        {
            _FilterType = type;
        }
        public bool IsReusable => true;

        /// <summary>
        /// 在Core 中对象的创建 都是通过IOC 容器来创建
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            return (IFilterMetadata)serviceProvider.GetService(this._FilterType);
        }
    }
}
