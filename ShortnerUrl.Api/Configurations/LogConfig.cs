using Serilog;

namespace ShortnerUrl.Api.Configurations;

public static class LogConfig
{
    public static void AddSerilogConfig(this IHostBuilder host)
    {
        host.UseSerilog((context, services, configuration) =>
        {
            configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.Debug()
                .WriteTo.File("Logs/shortnerUrl.log", rollingInterval: RollingInterval.Day);
        });
    }
}