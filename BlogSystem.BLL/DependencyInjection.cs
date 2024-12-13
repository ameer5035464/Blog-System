using BlogSystem.BLL.Contracts;
using BlogSystem.BLL.helpers;
using BlogSystem.BLL.Mapping;
using BlogSystem.BLL.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BlogSystem.BLL
{
    public static class DependencyInjection
    {
        public static IServiceCollection BLLServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(IServiceManager), typeof(ServiceManager));
            services.AddScoped(typeof(IPhotoService), typeof(PhotoService));

            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            services.Configure<CloudinarySettings>(configuration.GetSection("CloudinarySettings"));

            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("AdminsAndEditorsOnly", policy => policy.Requirements.Add(new RoleRequirement(["Admin", "Editor"])));
            //    options.AddPolicy("AdminsOnly", policy => policy.Requirements.Add(new RoleRequirement(["Admin"])));
            //});

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,

                        ValidAudience = configuration["JwtSettings:Audience"],
                        ValidIssuer = configuration["JwtSettings:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]!))
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnChallenge = context =>
                        {
                            if (!context.Response.HasStarted)
                            {
                                context.Response.StatusCode = 401;
                                context.Response.ContentType = "application/json";
                                var response = new { message = "Token is invalid or missing!" };
                                return context.Response.WriteAsJsonAsync(response);
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            //services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();


            //services.AddScoped<IAuthorizationHandler, RoleRequirementHandler>();

            services.AddAutoMapper(typeof(ProfileMapping));

            return services;
        }
    }
}
