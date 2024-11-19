using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using TaskMasterAPI.BLL;
using TaskMasterAPI.DAL.Seeds;

namespace TaskMasterAPI;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        await ConfigureServices(builder.Services, builder.Configuration);

        ConfigureWebApplication(builder.Build());
    }

    private static async Task ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddBllServiceCollection(configuration);
        await using var scope = services.BuildServiceProvider().CreateAsyncScope();
        
        var dataSeed = scope.ServiceProvider.GetRequiredService<DataSeed>();
        await dataSeed.Record();
    }

    private static void ConfigureWebApplication(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}