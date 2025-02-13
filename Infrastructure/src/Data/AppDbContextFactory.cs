using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using BackendOlimpiadaIsto.infrastructure;

namespace BackendOlimpiadaIsto.infrastructure.Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {

            var basePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "..", "Presentation");
            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var secretsManager = new SecretsManager(configuration);

            return new AppDbContext(secretsManager);
        }
    }
}