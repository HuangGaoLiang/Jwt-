using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Text;
using Zhaoxi.Core.WebApi.Utility;

namespace Zhaoxi.Core.WebApi
{
    /// <summary>
    /// Core 项目中导出都是IOC + DI
    /// </summary>
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        /// <summary>
        /// 被运行时调用的  还有什么？
        /// 执行且只执行一次；
        /// </summary>
        /// <param name="services"></param>

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region 注册Swagger服务 
            services.AddSwaggerGen(s =>
            {

                s.SwaggerDoc("V1", new OpenApiInfo()
                {
                    Title = "Web API",
                    Version = "v1.0",
                    Description = "Api学习",
                    Contact = new OpenApiContact
                    {
                        Name = "hgl",
                        Email = "1439194034@qq.com",
                        Url = new Uri("https://blog.csdn.net/zt102545")
                    }//联系人
                });

                #region 为 Swagger JSON and UI设置xml文档注释路径

                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);//获取应用程序所在目录（绝对，不受工作目录影响，建议采用此方法获取路径）
                var xmlAPIPath = Path.Combine(basePath, "Zhaoxi.Core.WebApi.xml");//这个就是刚刚配置的xml文件名
                var xmlModelPath = Path.Combine(basePath, "Zhaoxi.Core.WebApi.xml");//这个是引用model层的XML文档。设置输出XML文档的方法跟上面的一样。
                s.IncludeXmlComments(xmlAPIPath, true);//第二个参数true表示用控制器的XML注释。默认是false
                s.IncludeXmlComments(xmlModelPath, true);

                s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "在下框中输入请求头中需要添加Jwt授权Token：Bearer Token",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                s.AddSecurityRequirement(new OpenApiSecurityRequirement{
                {
                     new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });

                #endregion
            });
            #endregion

            #region JWT
            //var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["Audience:Secret"]));
            //services.AddAuthentication("Bearer").AddJwtBearer(o =>
            //{
            //    o.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        //是否开启密钥认证和key值
            //        ValidateIssuerSigningKey = true,
            //        IssuerSigningKey = signingKey,

            //        //是否开启发行人认证和发行人
            //        ValidateIssuer = true,
            //        ValidIssuer = Configuration["Audience:Issuer"],

            //        //是否开启订阅人认证和订阅人
            //        ValidateAudience = true,
            //        ValidAudience = Configuration["Audience:Audience"],

            //        //认证时间的偏移量
            //        ClockSkew = TimeSpan.Zero,
            //        //是否开启时间认证
            //        ValidateLifetime = true,
            //        //是否该令牌必须带有过期时间
            //        RequireExpirationTime = true
            //    };
            //});

            #endregion

            services.AddControllers();

            /////注册服务
            services.AddSingleton<CustomActionFilterAttribute>();
            services.AddSingleton<CustomGlobalActionFilterAttribute>();
            services.AddSingleton<CustomControllerActionFilterAttribute>();

            //services.AddScoped<IJwtHelper, JwtHelper>();
            services.AddScoped<IJWTService, JWTService>();

            #region 全局注册Filter
            //3.对所有的方法都生效
            //services.AddMvc(option => {
            //    option.Filters.Add(typeof(CustomActionFilterAttribute));
            //}); 

            #region JWT鉴权授权
            //1.Nuget引入程序包：Microsoft.AspNetCore.Authentication.JwtBearer 
            //services.AddAuthentication();//禁用  
            var ValidAudience = this.Configuration["audience"];
            var ValidIssuer = this.Configuration["issuer"];
            var SecurityKey = this.Configuration["SecurityKey"];
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)  //默认授权机制名称；                                      
                     .AddJwtBearer(options =>
                     {
                         options.TokenValidationParameters = new TokenValidationParameters
                         {
                             ValidateIssuer = true,//是否验证Issuer
                             ValidateAudience = true,//是否验证Audience
                             ValidateLifetime = true,//是否验证失效时间
                             ValidateIssuerSigningKey = true,//是否验证SecurityKey
                             ValidAudience = ValidAudience,//Audience
                             ValidIssuer = ValidIssuer,//Issuer，这两项和前面签发jwt的设置一致  表示谁签发的Token
                             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecurityKey))//拿到SecurityKey
                             //AudienceValidator = (m, n, z) =>
                             //{
                             //    return m != null && m.FirstOrDefault().Equals(this.Configuration["audience"]);
                             //},//自定义校验规则，可以新登录后将之前的无效 
                         };
                     });
            #endregion


            //services.AddMvc(option => {
            //    option.Filters.Add(typeof(CustomGlobalActionFilterAttribute));
            //});

            #endregion

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            #region 使用Swagger中间件
            app.UseSwagger();
            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("/swagger/V1/swagger.json", "ApiHelp V1");
                s.RoutePrefix = string.Empty;
            });
            #endregion


            //使用中间件
            app.UseRouting();

            #region 通过中间件来支持鉴权授权
            app.UseAuthentication();
            #endregion


            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
