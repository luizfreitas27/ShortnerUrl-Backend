using ShortnerUrl.Api.Auth;
using ShortnerUrl.Api.Repositories;
using ShortnerUrl.Api.Services.Auth;
using ShortnerUrl.Api.Shared.Auth;
using ShortnerUrl.Api.Shared.Repositories;

namespace ShortnerUrl.Api.Configurations;

public static class DependencyInjectionConfig
{
    public static IServiceCollection AddDependencyInjectionConfig(this IServiceCollection services)
    {
        services.AddScoped<IUnityOfWork, UnityOfWork>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IJwtTokenProvider, JwtTokenProvider>();

        return services;
    }
    
}