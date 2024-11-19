using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskMasterAPI.DAL.Context;
using TaskMasterAPI.DAL.Interfaces;

namespace TaskMasterAPI.DAL;

public static class ServiceCollectionExtensions
{
    public static void AddDalServiceCollection(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(p =>
        {
            p.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddScoped<IApplicationDbContext>(p => p.GetService<AppDbContext>()!);
    }
}