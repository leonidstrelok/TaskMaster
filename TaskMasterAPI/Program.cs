using TaskMasterAPI.BLL;
using TaskMasterAPI.DAL.Seeds;
using TaskMasterAPI.Middlewares;

namespace TaskMasterAPI;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.WebHost.ConfigureKestrel(serverOptions =>
        {
            serverOptions.Limits.MaxRequestBodySize = long.MaxValue;
        });
        
        await ConfigureServices(builder.Services, builder.Configuration);

        builder.WebHost.UseKestrel(options =>
        {
            options.ListenAnyIP(80); // HTTP
            options.ListenAnyIP(443, listenOptions => // HTTPS
            {
                listenOptions.UseHttps();
            });
        });
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

        app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseMiddleware<ExceptionHandlingMiddleware>();

        app.MapControllers();

        app.Run();
    }
}