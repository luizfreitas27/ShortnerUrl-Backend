using System.Reflection;
using FluentValidation;

namespace ShortnerUrl.Api.Configurations;

public static class ValidationConfig
{
    public static IServiceCollection AddValidationConfig(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        
        return services;
    }
    
}