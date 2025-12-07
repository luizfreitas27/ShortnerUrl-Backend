using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ShortnerUrl.Api.Auth;
using ShortnerUrl.Api.Constants;

namespace ShortnerUrl.Api.Configurations;

public static class JwtConfig
{
    public static void AddJwtConfig(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSection = configuration.GetSection("Jwt");
        services.Configure<JwtSettings>(jwtSection);
        var settings = jwtSection.Get<JwtSettings>()!;
        
        var key = Encoding.UTF8.GetBytes(settings.SecretKey);

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false; 
                options.SaveToken = true; 

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = settings.Issuer,
                    ValidAudience = settings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero,
                    RoleClaimType = ClaimTypes.Role
                };
            });
        
            services.AddAuthorization(options =>
            {
                options.AddPolicy(AuthConstants.Policies.RequireAdmin, policy =>
                    policy.RequireRole(AuthConstants.Roles.Admin));

                options.AddPolicy(AuthConstants.Policies.RequireAdminOrDeveloper, policy =>
                    policy.RequireRole(AuthConstants.Roles.Admin, AuthConstants.Roles.Developer));
            });
    }
    
}