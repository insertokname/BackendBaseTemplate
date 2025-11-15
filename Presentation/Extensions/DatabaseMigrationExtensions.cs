using Infrastructure.Data;

using Microsoft.EntityFrameworkCore;

namespace Presentation.Extensions
{
    public static class DatabaseMigrationExtensions
    {
        public static void ApplyDatabaseMigrations(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            const int maxRetry = 5;
            int retryCount = 0;
            bool migrated = false;

            while (!migrated && retryCount < maxRetry)
            {
                try
                {
                    Console.WriteLine("Migrating");
                    db.Database.Migrate();
                    migrated = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Migration attempt {retryCount + 1} failed: {ex.Message}");
                    retryCount++;
                    Thread.Sleep(5000);
                }
            }

            if (!migrated)
            {
                Console.WriteLine("Database migration failed after maximum retries.");
                throw new Exception("Database not ready");
            }
        }
    }
}
