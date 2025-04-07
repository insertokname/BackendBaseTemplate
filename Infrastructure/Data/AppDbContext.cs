using BackendBaseTemplate.domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace BackendBaseTemplate.infrastructure.Data;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<EntityTemplate> EntityTemplates { get; set; } = null!;

    private readonly SecretsManager _secretsManager;

    public AppDbContext(SecretsManager secretsManager)
    {
        _secretsManager = secretsManager;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(
            $"Host={_secretsManager.DbHost};" +
            $"Port={_secretsManager.DbPort};" +
            $"Username={_secretsManager.DbUsername};" +
            $"Password={_secretsManager.DbPassword};" +
            $"Database={_secretsManager.DbName}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EntityTemplate>(et =>
        {
            et.OwnsOne(e => e.EntityTemplateDataObjects);
        });
    }
}