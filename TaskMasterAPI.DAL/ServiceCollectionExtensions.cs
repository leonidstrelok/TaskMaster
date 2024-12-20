﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskMasterAPI.DAL.Context;
using TaskMasterAPI.DAL.Interfaces;
using TaskMasterAPI.DAL.Seeds;

namespace TaskMasterAPI.DAL;

public static class ServiceCollectionExtensions
{
    public static void AddDalServiceCollection(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<DataSeed>();
        services.AddDbContext<AppDbContext>(p =>
        {
            p.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddScoped<IApplicationDbContext>(p => p.GetService<AppDbContext>()!);
    }
}