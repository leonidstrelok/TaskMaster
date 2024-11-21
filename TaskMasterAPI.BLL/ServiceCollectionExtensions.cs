using System.Reflection;
using System.Text;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using TaskMasterAPI.BLL.Behaviours;
using TaskMasterAPI.BLL.Interfaces;
using TaskMasterAPI.BLL.Services;
using TaskMasterAPI.DAL;
using TaskMasterAPI.DAL.Context;
using TaskMasterAPI.Models.Clients;

namespace TaskMasterAPI.BLL;

public static class ServiceCollectionExtensions
{
    public static void AddBllServiceCollection(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestPostProcessorBehavior<,>));

        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddMediatR(p => p.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.AddIdentity<Client, IdentityRole>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireDigit = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireLowercase = false;
            }).AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();


        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(p =>
            {
                p.SaveToken = true;
                p.RequireHttpsMetadata = false;
                p.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration["ApplicationSettings:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = configuration["ApplicationSettings:Audience"],
                    ValidateLifetime = true,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["ApplicationSettings:Secret"]!)),
                    ValidateIssuerSigningKey = true,
                };
            });

        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IJwtAuthService, JwtAuthService>();
        services.AddScoped<IRoleMemberService, RoleMemberService>();

        services.AddDalServiceCollection(configuration);
    }
}