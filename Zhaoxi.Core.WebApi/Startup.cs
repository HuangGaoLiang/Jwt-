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
    /// Core ��Ŀ�е�������IOC + DI
    /// </summary>
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        /// <summary>
        /// ������ʱ���õ�  ����ʲô��
        /// ִ����ִֻ��һ�Σ�
        /// </summary>
        /// <param name="services"></param>

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region ע��Swagger���� 
            services.AddSwaggerGen(s =>
            {

                s.SwaggerDoc("V1", new OpenApiInfo()
                {
                    Title = "Web API",
                    Version = "v1.0",
                    Description = "Apiѧϰ",
                    Contact = new OpenApiContact
                    {
                        Name = "hgl",
                        Email = "1439194034@qq.com",
                        Url = new Uri("https://blog.csdn.net/zt102545")
                    }//��ϵ��
                });

                #region Ϊ Swagger JSON and UI����xml�ĵ�ע��·��

                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);//��ȡӦ�ó�������Ŀ¼�����ԣ����ܹ���Ŀ¼Ӱ�죬������ô˷�����ȡ·����
                var xmlAPIPath = Path.Combine(basePath, "Zhaoxi.Core.WebApi.xml");//������Ǹո����õ�xml�ļ���
                var xmlModelPath = Path.Combine(basePath, "Zhaoxi.Core.WebApi.xml");//���������model���XML�ĵ����������XML�ĵ��ķ����������һ����
                s.IncludeXmlComments(xmlAPIPath, true);//�ڶ�������true��ʾ�ÿ�������XMLע�͡�Ĭ����false
                s.IncludeXmlComments(xmlModelPath, true);

                s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "���¿�����������ͷ����Ҫ����Jwt��ȨToken��Bearer Token",
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
            //        //�Ƿ�����Կ��֤��keyֵ
            //        ValidateIssuerSigningKey = true,
            //        IssuerSigningKey = signingKey,

            //        //�Ƿ�����������֤�ͷ�����
            //        ValidateIssuer = true,
            //        ValidIssuer = Configuration["Audience:Issuer"],

            //        //�Ƿ�����������֤�Ͷ�����
            //        ValidateAudience = true,
            //        ValidAudience = Configuration["Audience:Audience"],

            //        //��֤ʱ���ƫ����
            //        ClockSkew = TimeSpan.Zero,
            //        //�Ƿ���ʱ����֤
            //        ValidateLifetime = true,
            //        //�Ƿ�����Ʊ�����й���ʱ��
            //        RequireExpirationTime = true
            //    };
            //});

            #endregion

            services.AddControllers();

            /////ע�����
            services.AddSingleton<CustomActionFilterAttribute>();
            services.AddSingleton<CustomGlobalActionFilterAttribute>();
            services.AddSingleton<CustomControllerActionFilterAttribute>();

            //services.AddScoped<IJwtHelper, JwtHelper>();
            services.AddScoped<IJWTService, JWTService>();

            #region ȫ��ע��Filter
            //3.�����еķ�������Ч
            //services.AddMvc(option => {
            //    option.Filters.Add(typeof(CustomActionFilterAttribute));
            //}); 

            #region JWT��Ȩ��Ȩ
            //1.Nuget����������Microsoft.AspNetCore.Authentication.JwtBearer 
            //services.AddAuthentication();//����  
            var ValidAudience = this.Configuration["audience"];
            var ValidIssuer = this.Configuration["issuer"];
            var SecurityKey = this.Configuration["SecurityKey"];
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)  //Ĭ����Ȩ�������ƣ�                                      
                     .AddJwtBearer(options =>
                     {
                         options.TokenValidationParameters = new TokenValidationParameters
                         {
                             ValidateIssuer = true,//�Ƿ���֤Issuer
                             ValidateAudience = true,//�Ƿ���֤Audience
                             ValidateLifetime = true,//�Ƿ���֤ʧЧʱ��
                             ValidateIssuerSigningKey = true,//�Ƿ���֤SecurityKey
                             ValidAudience = ValidAudience,//Audience
                             ValidIssuer = ValidIssuer,//Issuer���������ǰ��ǩ��jwt������һ��  ��ʾ˭ǩ����Token
                             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecurityKey))//�õ�SecurityKey
                             //AudienceValidator = (m, n, z) =>
                             //{
                             //    return m != null && m.FirstOrDefault().Equals(this.Configuration["audience"]);
                             //},//�Զ���У����򣬿����µ�¼��֮ǰ����Ч 
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
            #region ʹ��Swagger�м��
            app.UseSwagger();
            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("/swagger/V1/swagger.json", "ApiHelp V1");
                s.RoutePrefix = string.Empty;
            });
            #endregion


            //ʹ���м��
            app.UseRouting();

            #region ͨ���м����֧�ּ�Ȩ��Ȩ
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