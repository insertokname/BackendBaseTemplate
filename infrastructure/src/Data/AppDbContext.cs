using BackendOlimpiadaIsto.domain;
using BackendOlimpiadaIsto.domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BackendOlimpiadaIsto.infrastructure.Data;

public class AppDbContext : DbContext
{
    public DbSet<Question> Questions { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        String? dbAddres = Environment.GetEnvironmentVariable("POSTGRESSQL_CONNECTION_ADDRESS");
        if (dbAddres == null)
        {
            Console.WriteLine("No POSTGRESSQL_CONNECTION_ADDRESS set!\nDefaulting to localhost ass posgres address!");
            dbAddres = "localhost";
        }
        optionsBuilder.UseNpgsql($"Host={dbAddres};Port=5432;Username=insertokname;Password=DebugPassword;Database=insertokname_db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Question>(question =>
        {
            question.OwnsOne(x => x.Answers);
        });
    }
}