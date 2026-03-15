using System.Threading.RateLimiting;
using ShortnerUrl.Api.Settings;

namespace ShortnerUrl.Api.Configurations;

public static class RateLimitingConfig
{
    public const string LoginPolicy = "login";
    public const string ApiPolicy = "api";
    public const string RefreshTokenPolicy = "refresh";
    
    public static IServiceCollection AddRateLimiting(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RateLimitingSettings>(configuration.GetSection("RateLimiting"));
        
        var settings = configuration.GetSection("RateLimiting").Get<RateLimitingSettings>()!;

        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            options.OnRejected = async (context, cancellationToken) =>
            {
                context.HttpContext.Response.ContentType = "application/json";
                
                var retryAfter = context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfterValue)
                    ? retryAfterValue.Seconds : settings.Login.WindowSeconds;

                context.HttpContext.Response.Headers.RetryAfter = ((int)retryAfter).ToString();

                var response = new
                {
                    success = false,
                    error = "A lot of attempts. Try again later.",
                    code = 429,
                    retryAfterSeconds = (int)retryAfter
                };

                await context.HttpContext.Response.WriteAsJsonAsync(response, cancellationToken);
            };

            options.AddPolicy(LoginPolicy, context =>
            {
                var clientIp = GetClientIpAddress(context);

                return RateLimitPartition.GetSlidingWindowLimiter(
                    partitionKey: clientIp,
                    factory: _ => new SlidingWindowRateLimiterOptions
                    {
                        PermitLimit =  settings.Login.PermitLimit,
                        Window = TimeSpan.FromSeconds(settings.Login.WindowSeconds),
                        SegmentsPerWindow =  settings.Login.SegmentsPerWindow,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 0
                    }
                );
            });

            options.AddPolicy(RefreshTokenPolicy, context =>
            {
                var clientIp = GetClientIpAddress(context);

                return RateLimitPartition.GetSlidingWindowLimiter(
                    partitionKey: clientIp,
                    factory: _ => new SlidingWindowRateLimiterOptions
                    {
                        PermitLimit = settings.RefreshToken.PermitLimit,
                        Window = TimeSpan.FromSeconds(settings.RefreshToken.WindowSeconds),
                        SegmentsPerWindow = settings.RefreshToken.SegmentsPerWindow,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 0
                    }
                );
            });

            options.AddPolicy(ApiPolicy, context =>
            {
                var clientIp = GetClientIpAddress(context);

                return RateLimitPartition.GetTokenBucketLimiter(
                    partitionKey: clientIp,
                    factory: _ => new TokenBucketRateLimiterOptions
                    {
                        TokenLimit = settings.Api.TokenLimit,
                        TokensPerPeriod = settings.Api.TokensPerPeriod,
                        ReplenishmentPeriod = TimeSpan.FromSeconds(settings.Api.ReplenishmentPeriodSeconds),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = settings.Api.QueueLimit
                    });
            });
        });

        return services;

    }

    private static string GetClientIpAddress(HttpContext context)
    {
        var forwardedFor = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();

        if (!string.IsNullOrEmpty(forwardedFor))
        {
            return forwardedFor.Split(",")[0].Trim();
        }

        var realIp = context.Request.Headers["X-Real-IP"].FirstOrDefault();
        
        if (!string.IsNullOrEmpty(realIp))
        {
            return realIp;
        }
        
        return context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
    }
}