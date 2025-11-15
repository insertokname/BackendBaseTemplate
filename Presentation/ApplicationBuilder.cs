using Infrastructure;

using Presentation.Configuration;

namespace Presentation
{
    public static class ApplicationBuilder
    {
        public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
        {
            var secretsManager = new SecretsManager(builder.Configuration);
            builder.Services.AddSingleton(secretsManager);

            builder.ConfigureKestrel(secretsManager);
            builder.Services.ConfigureSwagger();
            builder.Services.ConfigureDatabase();
            builder.Services.ConfigureApplicationServices();
            builder.Services.ConfigureAuthentication(secretsManager);
            builder.Services.ConfigureRateLimiting();
            builder.Services.ConfigureCors();
            builder.Services.AddControllers();

            return builder;
        }
    }
}
