namespace Presentation.Configuration
{
    public static class CorsConfiguration
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy
                        .WithOrigins(
                            "http://localhost:8888"
                        )
                        .WithMethods("GET", "POST", "PUT", "DELETE", "OPTIONS")
                        .AllowAnyHeader()
                        .SetPreflightMaxAge(TimeSpan.FromHours(1))
                        .AllowCredentials();
                });
            });
        }
    }
}
