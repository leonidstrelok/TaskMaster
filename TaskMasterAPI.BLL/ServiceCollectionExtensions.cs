using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using TaskMasterAPI.DAL;

namespace TaskMasterAPI.BLL;

public static class ServiceCollectionExtensions
{
    public static void AddBLLServiceCollection(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(p =>
            {
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

        services.AddDalServiceCollection(configuration);
    }
}