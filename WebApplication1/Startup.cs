using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Database;
using WebApplication1.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using WebApplication1.Models;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Formatters;     // 配置服务 用于在 appsettings.json 文件内配置

namespace WebApplication1
{
    public class Startup
    {
        public IConfiguration Configuration { get; }    // 用于在 appsettings.json 文件内进行配置

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var secretByte = Encoding.UTF8.GetBytes(Configuration["Authentication:SecretKey"]);
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = Configuration["Authentication:Issuer"],

                        ValidateAudience = true,
                        ValidAudience = Configuration["Authentication:Audience"],

                        ValidateLifetime = true,

                        IssuerSigningKey = new SymmetricSecurityKey(secretByte)
                    };
                });


            services.AddControllers(setupAction =>
            {
                setupAction.ReturnHttpNotAcceptable = true;    // 如果数据格式 (如 application/zip) 不支持，则发送 HTTP 406 错误
            })
            .AddNewtonsoftJson(setupAction =>
            {
                setupAction.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            })
            .AddXmlDataContractSerializerFormatters()       // 对输入和输出的数据都支持 application/xml 格式
            .ConfigureApiBehaviorOptions(setupAction =>
            {
                setupAction.InvalidModelStateResponseFactory = context =>
                {
                    var problemDetail = new ValidationProblemDetails(context.ModelState)       // ModelState是一个内建的全局变量，包含当前数据模型状态以及该模型相应的数据验证逻辑
                    {
                        Type = "Some type",
                        Title = "Data validation error",           // 错误原因
                        Status = StatusCodes.Status422UnprocessableEntity,
                        Detail = "Please see detailed description",
                        Instance = context.HttpContext.Request.Path       // 请求路径
                    };
                    problemDetail.Extensions.Add("traceId", context.HttpContext.TraceIdentifier);
                    return new UnprocessableEntityObjectResult(problemDetail)
                    {
                        ContentTypes = { "application/problem+json" }
                    };
                };
            });       

            // 非法模型响应工厂

            // AddSingleton 系统在启动时有且仅创建一个数据仓库，之后每次处理请求都会使用同一个仓库实例 (可能造成数据污染)
            // AddScoped 将一系列的请求整合为一个事务 (Transaction), 在该事务中，只创建一个数据仓库
            services.AddTransient<ITravelRouteRepository, TravelRouteRepository>();   // AddTransient 在每次发起请求时创建一个全新的数据仓库, 请求结束后自动注销该仓库 (优点: 不同的请求之中数据仓库的数据完全独立)

            services.AddDbContext<AppDbContext>(option =>       // 配置 DbContext
            {
                //option.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=WebApplication1;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
                option.UseSqlServer(Configuration["DbContext:ConnectionString"]);
            });

            // 扫描 profile 文件，并把所有的 profile 文件加载到目前的 AppDomain 中
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddHttpClient();

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();   // 注册 urlHelper 服务，用来配置 url 路径

            services.AddTransient<IPropertyMappingService, PropertyMappingService>();

            services.Configure<MvcOptions>(config =>         // 全局支持 application/vnd.mycompany.hateoas+json 媒体类型
            {
                var outputFormatter = config.OutputFormatters
                    .OfType<NewtonsoftJsonOutputFormatter>()?.FirstOrDefault();

                if (outputFormatter != null)
                {
                    outputFormatter.SupportedMediaTypes
                    .Add("application/vnd.mycompany.hateoas+json");
                }
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapGet("/test", async context =>
                //{
                //    await context.Response.WriteAsync("Hello from test!");
                //});

                //endpoints.MapGet("/", async context =>
                //{
                //    await context.Response.WriteAsync("Hello World!");
                //});

                endpoints.MapControllers();
            });
        }
    }
}
