using Microsoft.EntityFrameworkCore;
using ProjectVictory.Persistence.Account;

namespace ProjectVictory.Persistence.Database;

public sealed class ProjectVictoryDbContext : DbContext
{
    public DbSet<PersistentAccountState> Accounts =>
        Set<PersistentAccountState>();


    public ProjectVictoryDbContext(
        DbContextOptions<ProjectVictoryDbContext> options)
        : base(options)
    {
    }


    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PersistentAccountState>(
            entity =>
            {
                entity.HasKey(
                    x => x.Id);


                entity.Property(
                    x => x.Email)
                    .IsRequired();


                entity.Property(
                    x => x.PasswordHash)
                    .IsRequired();


                entity.HasIndex(
                    x => x.Email)
                    .IsUnique();
            });
    }
}