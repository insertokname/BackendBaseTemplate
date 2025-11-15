using Infrastructure;

using Presentation.Extensions;
using Presentation.Middleware;

namespace Presentation
{
    public static class ApplicationPipeline
    {
        public static WebApplication ConfigurePipeline(this WebApplication app)
        {
            var secretsManager = app.Services.GetRequiredService<SecretsManager>();

            app.ConfigureExceptionHandling();
            app.ApplyDatabaseMigrations();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors("AllowFrontend");

            app.UseRateLimiter();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            return app;
        }
    }
}
