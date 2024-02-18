using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Music.Base.JWT;
using Music.DbMigrator;
using Music.IService;
using Music.IService.Login;
using Music.IService.Music;
using Music.Redis;
using MusicApplication.Filter;
using MusicService;
using MusicService.Login;
using MusicService.Music;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MusicApplication.Extension;

/// <summary>
/// 依赖注入扩展类
/// </summary>
public static class RegisterExtension
{
    /// <summary>
    /// 依赖注入
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static WebApplicationBuilder RegisterDI(this WebApplicationBuilder builder)
    {
        var config = builder.Configuration;
        builder.Services.AddHttpClient();
        builder.Services.AddControllers();
        builder.Services.AddScoped<ICaptchaCodeService, CaptchaCodeService>();
        builder.Services.AddScoped<ILoginService, LoginService>();
        builder.Services.AddScoped<IMusicInfoService, MusicInfoService>();
        builder.Services.AddScoped<IUserSessionService, UserSessionService>();
        builder.Services.AddSingleton(new JWTHelper(config));
        builder.Services.AddHttpContextAccessor();
        var redisConnStr = config["Redis:connString"];
        builder.Services.AddSingleton(new RedisService(redisConnStr));
        return builder;
    }

    /// <summary>
    /// 注入AutoMapper
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static WebApplicationBuilder AddAutoMapper(this WebApplicationBuilder builder)
    {
        builder.Services.AddAutoMapper(typeof(RegisterExtension));
        return builder;
    }
    /// <summary>
    /// 注入MVC相关
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static WebApplicationBuilder AddMvcService(this WebApplicationBuilder builder)
    {
        //swagger
        builder.Services.AddEndpointsApiExplorer();
        var apiName = "Music.Application.xml";
        builder.Services.AddSwaggerGen(s =>
        {
            s.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = $"{apiName}接口文档-NetCore6.0"
            });
            s.OrderActionsBy(x => x.RelativePath);
            s.IncludeXmlComments("Music.Application.xml", true);
            s.DocInclusionPredicate((docName, description) => true);
            s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT授权(数据将在请求头中进行传输) 在下方输入Bearer {token} 即可，注意两者之间有空格",
                Name = "Authorization",//jwt默认的参数名称
                In = ParameterLocation.Header,//jwt默认存放Authorization信息的位置(请求头中)
                Type = SecuritySchemeType.ApiKey
            });
            //认证方式，此方式为全局添加
            s.AddSecurityRequirement(new OpenApiSecurityRequirement 
            {
                { 
                    new OpenApiSecurityScheme{
                        Reference = new OpenApiReference(){
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                    }, Array.Empty<string>() }
            });
        });
        
        //过滤器
        builder.Services
            .AddControllers(opt => {
            opt.Filters.Add<ExceptionFilter>();
    
        })
            .AddNewtonsoftJson(options => {
            //忽略循环引用
            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            //设置时间格式
            options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            // 小写
            options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        });
        
        //注入jwt帮助类
        builder.Services.AddSingleton(new JWTHelper(builder.Configuration));
        //注册服务
        builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true, //是否验证Issuer
                    ValidIssuer = builder.Configuration["Jwt:Issuer"], //发行人Issuer
                    ValidateAudience = true, //是否验证Audience
                    ValidAudience = builder.Configuration["Jwt:Audience"], //订阅人Audience
                    ValidateIssuerSigningKey = true, //是否验证SecurityKey
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"])), //SecurityKey
                    ValidateLifetime = true, //是否验证失效时间
                    ClockSkew = TimeSpan.FromMinutes(30), //过期时间容错值，解决服务器端时间不同步问题（秒）
                    RequireExpirationTime = true,
                };
            });
        
        //跨域
        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });
        return builder;
    }
    /// <summary>
    /// 注入DBContext
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static WebApplicationBuilder AddDBContextService(this WebApplicationBuilder builder)
    {
        string connectionString = builder.Configuration["DefaultConnection"];
        //var serverVersion = ServerVersion.AutoDetect(connectionString);
        builder.Services.AddDbContext<MusicDbContext>(opt =>
        {
            opt.UseMySql(connectionString, Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.27-mysql"));
        });
        return builder;
    }
}