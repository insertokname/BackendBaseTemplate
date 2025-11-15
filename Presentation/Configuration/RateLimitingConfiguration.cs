using System.Threading.RateLimiting;

namespace Presentation.Configuration
{
    public static class RateLimitingConfiguration
    {
        public static void ConfigureRateLimiting(this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                options.AddPolicy("UnauthorizedEndpointRateLimiter", httpContext =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 20,
                            Window = TimeSpan.FromSeconds(10)
                        }
                    )
                );

                options.AddPolicy("LoginRateLimiter", httpContext =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 6,
                            Window = TimeSpan.FromSeconds(60)
                        }
                    )
                );
            });
        }
    }
}
