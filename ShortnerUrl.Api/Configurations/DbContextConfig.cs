using Microsoft.EntityFrameworkCore;
using ShortnerUrl.Api.Persistence;

namespace ShortnerUrl.Api.Configurations;

public static class DbContextConfig
{
    public static IServiceCollection AddDbContextConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ShortnerUrlContext>(options
            => options.UseNpgsql(configuration.GetConnectionString("Default")));

        return services;
    }
    
}