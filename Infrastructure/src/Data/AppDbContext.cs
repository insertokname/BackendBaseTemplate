using BackendOlimpiadaIsto.domain;
using BackendOlimpiadaIsto.domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BackendOlimpiadaIsto.infrastructure.Data;

public class AppDbContext : DbContext
{
    public DbSet<Question> Questions { get; set; } = null!;
    public DbSet<PetPrompt> PetPrompts { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;

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
        modelBuilder.Entity<Question>(question =>
        {
            question.OwnsOne(x => x.Answers);
        });

        modelBuilder.Entity<User>(user =>
        {
            user.OwnsMany(u => u.AnsweredQuestions);
        });
    }
}