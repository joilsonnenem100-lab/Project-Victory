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

    // PV-PERSIST-010: política de poções do jogador (configuração permanente).
    public DbSet<ProjectVictory.Persistence.Supply.PersistentSupplyPolicyState> SupplyPolicies =>
        Set<ProjectVictory.Persistence.Supply.PersistentSupplyPolicyState>();

    // PV-HUD-ANALYZER-010: histórico do Hunt Analyzer (últimas Hunts por personagem).
    public DbSet<ProjectVictory.Persistence.HuntAnalyzer.PersistentHuntAnalyzerSummaryState> HuntAnalyzerSummaries =>
        Set<ProjectVictory.Persistence.HuntAnalyzer.PersistentHuntAnalyzerSummaryState>();


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

            // PV-REWARD-010: coluna nova com padrão 0 (migration aditiva).
            entity.Property(x => x.Gold)
                .HasDefaultValue(0L);

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

            // PV-REWARD-010: coluna nova com padrão 0 (migration aditiva).
            entity.Property(x => x.NpcSellPrice)
                .HasDefaultValue(0);

            // PV-CONTENT-011: coluna nova com padrão 0 (migration aditiva).
            entity.Property(x => x.NpcBuyPrice)
                .HasDefaultValue(0);

            // PV-CONTENT-011: itens de loot da Spider Cave e poções (PV-SUPPLY-010) na tabela Items.
            // Espelham SpiderCaveCatalog (Infrastructure) e SupplyCatalog (Application); um teste
            // garante que os dados batem. Preços PROVISÓRIOS.
            entity.HasData(
                new PersistentItemState { Id = Guid.Parse("00000000-0000-0000-0000-000000000001"), Name = "Rat Tail", Description = "Validation loot item", NpcSellPrice = 2, NpcBuyPrice = 0 },
                new PersistentItemState { Id = Guid.Parse("00000000-0000-0000-0000-000000000002"), Name = "Cheese", Description = "Queijo levado pelos ratos.", NpcSellPrice = 3, NpcBuyPrice = 0 },
                new PersistentItemState { Id = Guid.Parse("00000000-0000-0000-0000-000000000003"), Name = "Spider Fang", Description = "Presa de aranha.", NpcSellPrice = 8, NpcBuyPrice = 0 },
                new PersistentItemState { Id = Guid.Parse("00000000-0000-0000-0000-000000000004"), Name = "Spider Silk", Description = "Seda de aranha gigante.", NpcSellPrice = 25, NpcBuyPrice = 0 },
                new PersistentItemState { Id = Guid.Parse("00000000-0000-0000-0000-0000000000a1"), Name = "Health Potion", Description = "Restaura vida (suprimento de Hunt).", NpcSellPrice = 10, NpcBuyPrice = 50 },
                new PersistentItemState { Id = Guid.Parse("00000000-0000-0000-0000-0000000000a2"), Name = "Mana Potion", Description = "Restaura mana (suprimento de Hunt).", NpcSellPrice = 10, NpcBuyPrice = 50 });
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

        // PV-PERSIST-010: uma linha por personagem.
        modelBuilder.Entity<ProjectVictory.Persistence.Supply.PersistentSupplyPolicyState>(entity =>
        {
            entity.ToTable("SupplyPolicies");
            entity.HasKey(x => x.CharacterId);
            entity.Property(x => x.CharacterId).ValueGeneratedNever();
        });

        // PV-HUD-ANALYZER-010: uma linha por Hunt encerrada; consulta por personagem.
        modelBuilder.Entity<ProjectVictory.Persistence.HuntAnalyzer.PersistentHuntAnalyzerSummaryState>(entity =>
        {
            entity.ToTable("HuntAnalyzerSummaries");
            entity.HasKey(x => x.HuntId);
            entity.Property(x => x.HuntId).ValueGeneratedNever();
            entity.Property(x => x.EndReason).IsRequired().HasMaxLength(32);
            entity.HasIndex(x => new { x.CharacterId, x.EndedAtUnixMilliseconds });
        });
    }
}