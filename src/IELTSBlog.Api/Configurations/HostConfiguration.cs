namespace IELTSBlog.Api.Configurations;

using IELTSBlog.Repository.IRepositories;
using IELTSBlog.Repository.Repository;
using IELTSBlog.Service;
using IELTSBlog.Service.Interfaces;
using IELTSBlog.Service.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

public static class HostConfiguration
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services
            .AddScoped<IArticleService, ArticleService>()
            .AddScoped<IAttachmentService, AttachmentService>()
            .AddScoped<IAuthService, AuthService>()
            .AddScoped<ICategoryService, CategoryService>()
            .AddScoped<ICommentService, CommentService>()
            .AddScoped<IIdentityService, IdentityService>()
            .AddScoped<ITokenService, TokenService>()
            .AddScoped<IUserService, UserService>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddMemoryCache();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


        return services;
    }

    public static IServiceCollection AddJwt(this IServiceCollection services, IConfiguration configuration)
    {
        var key = Encoding.UTF8.GetBytes(configuration["JWT:Secret"] ?? throw new Exception("Key isn't configured."));

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
         .AddJwtBearer(options =>
         {
             options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
             {
                 ValidateIssuerSigningKey = true,
                 IssuerSigningKey = new SymmetricSecurityKey(key),
                 ValidateIssuer = false,
                 ValidateActor = false,
                 ValidateLifetime = true,
                 ClockSkew = TimeSpan.Zero
             };

             options.SaveToken = true;
             options.RequireHttpsMetadata = false;
         });

        return services;
    }

    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(option =>
        {
            option.SwaggerDoc("v1", new OpenApiInfo { Title = "IELTS Blog API", Version = "v1" });
            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            option.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    new string[]{}
                }
            });
        });

        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll",
                builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
        });
    }
}
