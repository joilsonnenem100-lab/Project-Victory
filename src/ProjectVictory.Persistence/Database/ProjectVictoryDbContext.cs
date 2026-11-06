using Microsoft.EntityFrameworkCore;

using ProjectVictory.Persistence.Account;
using ProjectVictory.Persistence.Character;
using ProjectVictory.Persistence.Item;
using ProjectVictory.Persistence.Monster;
using ProjectVictory.Persistence.Hunt;
using ProjectVictory.Persistence.Runtime;


namespace ProjectVictory.Persistence.Database;

public sealed class ProjectVictoryDbContext : DbContext
{
    public DbSet<PersistentAccountState> Accounts =>
        Set<PersistentAccountState>();

    public DbSet<PersistentCharacterState> Characters =>
        Set<PersistentCharacterState>();

    public DbSet<PersistentMonsterState> Monsters =>
        Set<PersistentMonsterState>();

    public DbSet<PersistentItemState> Items =>
        Set<PersistentItemState>();

    public DbSet<PersistentHuntSessionState> HuntSessions =>
        Set<PersistentHuntSessionState>();

    public DbSet<PersistentRuntimeState> RuntimeStates =>
        Set<PersistentRuntimeState>();


    public ProjectVictoryDbContext(
        DbContextOptions<ProjectVictoryDbContext> options)
        : base(options)
    {
    }


    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PersistentAccountState>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Email)
                .IsRequired();

            entity.Property(x => x.PasswordHash)
                .IsRequired();

            entity.HasIndex(x => x.Email)
                .IsUnique();
        });


        modelBuilder.Entity<PersistentCharacterState>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.AccountId)
                .IsRequired();

            entity.Property(x => x.Name)
                .IsRequired();

            entity.Property(x => x.Level)
                .IsRequired();

            entity.Property(x => x.Experience)
                .IsRequired();

            entity.Property(x => x.CharacterClass)
                .IsRequired();

            entity.Property(x => x.Health)
                .IsRequired();

            entity.Property(x => x.Mana)
                .IsRequired();


            entity.HasIndex(x => x.Name)
                .IsUnique();


            entity.OwnsMany(
                x => x.InventoryItems,
                inventory =>
                {
                    inventory.WithOwner()
                        .HasForeignKey("CharacterId");

                    inventory.HasKey("Id");

                    inventory.Property(x => x.ItemDefinitionId)
                        .IsRequired();
                });
        });


        modelBuilder.Entity<PersistentMonsterState>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .IsRequired();

            entity.Property(x => x.Health)
                .IsRequired();

            entity.Property(x => x.State)
                .IsRequired();
        });


        modelBuilder.Entity<PersistentItemState>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .IsRequired();

            entity.Property(x => x.Description)
                .IsRequired();
        });


        modelBuilder.Entity<PersistentHuntSessionState>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.CharacterId)
                .IsRequired();

            entity.Property(x => x.Status)
                .IsRequired();
        });

        modelBuilder.Entity<PersistentRuntimeState>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.HasIndex(x => x.RuntimeId).IsUnique();
            entity.HasIndex(x => x.CharacterId);
        });
    }
}