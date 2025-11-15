using Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class AppDbContext(SecretsManager secretsManager) : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(
                $"Host={secretsManager.DbHost};" +
                $"Port={secretsManager.DbPort};" +
                $"Username={secretsManager.DbUsername};" +
                $"Password={secretsManager.DbPassword};" +
                $"Database={secretsManager.DbName}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
