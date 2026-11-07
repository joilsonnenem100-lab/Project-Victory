using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectVictory.Application.Character;
using ProjectVictory.Application.Checkpoint;
using ProjectVictory.Application.Combat.TurnBased;
using ProjectVictory.Application.Game;
using ProjectVictory.Application.Hunt;
using ProjectVictory.Application.HuntAnalyzer;
using ProjectVictory.Application.Supply;
using ProjectVictory.Domain.Character;
using ProjectVictory.Domain.Combat.Rules;
using ProjectVictory.Domain.Combat.TurnBased;
using ProjectVictory.Domain.Primitives;
using ProjectVictory.Infrastructure.Combat;
using ProjectVictory.Infrastructure.Content;
using ProjectVictory.Infrastructure.Services;
using ProjectVictory.Persistence.Database;
using Xunit;

namespace ProjectVictory.SmokeTests;

// PV-CONTENT-012 (PO-15/PO-16): poções em três níveis, escolha do nível, kit inicial e dados.
public sealed class PotionTiersTests
{
    private static Combatant Player(int maxHp, int hp, int maxMana = 100, int mana = 100) =>
        new(Guid.NewGuid(), Guid.NewGuid(), CombatSide.Player, "P", 1, maxHp, hp, maxMana, mana, new CombatProfile(10, 14, 4, 6));

    private static CombatSession Session(Combatant player) =>
        new(Guid.NewGuid(), player,
            [new Combatant(Guid.NewGuid(), Guid.NewGuid(), CombatSide.Monster, "Rat", 1, 20, 20, 0, 0, new CombatProfile(10, 8, 1, 1))],
            new SeededRandomSource(1));

    private static Dictionary<Guid, int> AllTiers(int each = 1) =>
        PotionCatalog.All.ToDictionary(p => p.ItemId, _ => each);

    // --- dados ---

    [Fact]
    public void Catalog_HasThreeTiersPerKind_IncreasingAmountsAndPrices()
    {
        foreach (var kind in new[] { PotionKind.Health, PotionKind.Mana })
        {
            var tiers = PotionCatalog.All.Where(p => p.Kind == kind).OrderBy(p => p.Size).ToList();

            Assert.Equal(new[] { PotionSize.Small, PotionSize.Medium, PotionSize.Large }, tiers.Select(p => p.Size));
            Assert.True(tiers[0].Amount < tiers[1].Amount && tiers[1].Amount < tiers[2].Amount, kind.ToString());
            Assert.True(tiers[0].Item.NpcBuyPrice < tiers[1].Item.NpcBuyPrice && tiers[1].Item.NpcBuyPrice < tiers[2].Item.NpcBuyPrice, kind.ToString());
        }

        Assert.All(PotionCatalog.All, p =>
        {
            Assert.True(p.Item.NpcBuyPrice > p.Item.NpcSellPrice && p.Item.NpcSellPrice > 0, p.Key);
            Assert.True(SupplyCatalog.IsPotion(p.ItemId), p.Key);
            Assert.Same(p, PotionCatalog.FindByKey(p.Key.ToLowerInvariant()));
        });

        Assert.Equal(PotionCatalog.All.Count, PotionCatalog.All.Select(p => p.ItemId).Distinct().Count());
        Assert.Equal(PotionCatalog.All.Count, PotionCatalog.All.Select(p => p.Key).Distinct(StringComparer.OrdinalIgnoreCase).Count());
        Assert.False(SupplyCatalog.IsPotion(SpiderCaveCatalog.RatTail.Id));
    }

    [Fact]
    public void ExistingPotions_AreTheSmallTier_Unchanged()
    {
        var smallHealth = PotionCatalog.Small(PotionKind.Health);
        var smallMana = PotionCatalog.Small(PotionKind.Mana);

        Assert.Same(SupplyCatalog.HealthPotion, smallHealth.Item);
        Assert.Same(SupplyCatalog.ManaPotion, smallMana.Item);
        Assert.Equal((75, 50), (smallHealth.Amount, smallMana.Amount));
        Assert.Equal((50, 10), (SupplyCatalog.HealthPotion.NpcBuyPrice, SupplyCatalog.HealthPotion.NpcSellPrice));

        var effects = new ProvisionalSupplyEffects();
        Assert.Equal((75, 50), (effects.HealthPotionHealing, effects.ManaPotionRestore));
    }

    [Fact]
    public async Task ItemsTable_HasTheNewTiers_InRealSqlite()
    {
        var path = Path.Combine(Path.GetTempPath(), $"pv-potions-{Guid.NewGuid():N}.db");

        try
        {
            await using var context = new ProjectVictoryDbContext(
                new DbContextOptionsBuilder<ProjectVictoryDbContext>().UseSqlite($"Data Source={path};Pooling=False").Options);
            await context.Database.MigrateAsync();

            foreach (var item in PotionCatalog.All.Select(p => p.Item))
            {
                var row = await context.Items.SingleAsync(x => x.Id == item.Id);

                Assert.Equal(item.Name, row.Name);
                Assert.Equal(item.Characteristics.Description, row.Description);
                Assert.Equal(item.NpcSellPrice, row.NpcSellPrice);
                Assert.Equal(item.NpcBuyPrice, row.NpcBuyPrice);
            }
        }
        finally
        {
            Microsoft.Data.Sqlite.SqliteConnection.ClearAllPools();
            foreach (var suffix in new[] { "", "-wal", "-shm" })
            {
                try { File.Delete(path + suffix); } catch (IOException) { }
            }
        }
    }

    [Fact]
    public void SpiderCaveLoot_EveryMonsterDropsAnItem_WithSellPrice()
    {
        Assert.All(SpiderCaveCatalog.Monsters, m => Assert.NotEmpty(m.Loot.Drops));
        Assert.Contains(SpiderCaveCatalog.Find(SpiderCaveCatalog.SpiderId)!.Loot.Drops, d => d.Item == SpiderCaveCatalog.SpiderFang);
    }

    // --- escolha do nível ---

    [Theory]
    [InlineData(10, "HealthPotion")]         // falta pouco: a pequena resolve
    [InlineData(75, "HealthPotion")]         // exatamente o valor da pequena
    [InlineData(76, "StrongHealthPotion")]   // a pequena não resolve; a média sim
    [InlineData(200, "GreatHealthPotion")]
    [InlineData(1000, "GreatHealthPotion")]  // nenhuma resolve: a maior
    public void Choose_SmallestThatResolves_ElseLargest(int missing, string expected)
    {
        Assert.Equal(expected, PotionCatalog.Choose(PotionKind.Health, missing, AllTiers())!.Key);
    }

    [Fact]
    public void Choose_OnlyAmongOwnedTiers()
    {
        var onlyLarge = new Dictionary<Guid, int> { [PotionCatalog.GreatManaPotionId] = 1, [PotionCatalog.StrongManaPotionId] = 0 };
        Assert.Equal("GreatManaPotion", PotionCatalog.Choose(PotionKind.Mana, 1, onlyLarge)!.Key);

        var smallAndMedium = new Dictionary<Guid, int> { [SupplyCatalog.HealthPotionId] = 2, [PotionCatalog.StrongHealthPotionId] = 1 };
        Assert.Equal("StrongHealthPotion", PotionCatalog.Choose(PotionKind.Health, 500, smallAndMedium)!.Key);

        Assert.Null(PotionCatalog.Choose(PotionKind.Mana, 10, smallAndMedium));
    }

    [Fact]
    public void Selector_PicksTheTier_AndResolverHealsByTier_AndContextCountsByTier()
    {
        var supplies = new CombatSupplyContext();
        var selector = new SupplyAwareCombatActionSelector(supplies);
        var resolver = new CombatResolver(new ProvisionalTibiaLikeDamageFormula(), new ProvisionalSupplyEffects());
        supplies.Load(new SupplyPolicy(100, 100), AllTiers(each: 2));

        Assert.Equal(6, supplies.HealthPotions);
        Assert.Equal(6, supplies.ManaPotions);

        // Vida 400, faltam 120 → média (150).
        var hurt = Player(maxHp: 400, hp: 280);
        var choice = selector.SelectSecondaryChoice(Session(hurt), hurt)!;
        Assert.Equal(CombatActionKind.UseHealthPotion, choice.Kind);
        Assert.Equal(PotionCatalog.StrongHealthPotionId, choice.PotionItemId);

        var action = new CombatTurnAction(hurt, hurt, choice.Kind, choice.Skill, choice.PotionItemId);
        Assert.Equal(150, resolver.ResolveAction(action, new SeededRandomSource(1)).Healing);

        new SupplyConsumptionObserver(supplies).OnActionExecuted(Session(hurt), action, CombatActionOutcome.NotExecuted);
        Assert.Equal(1, supplies.Used[PotionCatalog.StrongHealthPotionId]);
        Assert.Equal(1, supplies.Available[PotionCatalog.StrongHealthPotionId]);
        Assert.Equal(1, supplies.HealthPotionsUsed);
        Assert.Equal(5, supplies.HealthPotions);

        // Vida cheia, Mana 0 de 300 → nenhuma resolve: a grande (100).
        var dry = Player(maxHp: 100, hp: 100, maxMana: 300, mana: 0);
        var manaChoice = selector.SelectSecondaryChoice(Session(dry), dry)!;
        Assert.Equal(PotionCatalog.GreatManaPotionId, manaChoice.PotionItemId);
        Assert.Equal(100, resolver.ResolveAction(new CombatTurnAction(dry, dry, manaChoice.Kind, null, manaChoice.PotionItemId), new SeededRandomSource(1)).ManaRestored);

        // Sem nível (contrato antigo) = a pequena.
        Assert.Equal(75, resolver.ResolveAction(new CombatTurnAction(hurt, hurt, CombatActionKind.UseHealthPotion), new SeededRandomSource(1)).Healing);
    }

    [Fact]
    public void LegacyLoad_IsTheSmallTier()
    {
        var supplies = new CombatSupplyContext();
        supplies.Load(new SupplyPolicy(50, null), healthPotions: 2, manaPotions: 1);

        Assert.Equal(2, supplies.Available[SupplyCatalog.HealthPotionId]);
        Assert.Equal(1, supplies.Available[SupplyCatalog.ManaPotionId]);

        supplies.Consume(CombatActionKind.UseHealthPotion);
        Assert.Equal(1, supplies.Used[SupplyCatalog.HealthPotionId]);
    }

    [Fact]
    public void PendingConsumption_ByTier_AndLegacyTotals()
    {
        var pending = new PendingSupplyConsumption();
        var characterId = Guid.NewGuid();

        pending.AddItems(characterId, new Dictionary<Guid, int> { [PotionCatalog.GreatHealthPotionId] = 2, [SupplyCatalog.ManaPotionId] = 1 });
        pending.Add(characterId, 1, 0);

        Assert.Equal((3, 1), pending.Peek(characterId));
        Assert.Equal(2, pending.PeekItems(characterId)[PotionCatalog.GreatHealthPotionId]);

        pending.SubtractItems(characterId, new Dictionary<Guid, int> { [PotionCatalog.GreatHealthPotionId] = 2 });
        Assert.Equal((1, 1), pending.Peek(characterId));

        pending.Subtract(characterId, 1, 1);
        Assert.Empty(pending.PeekItems(characterId));
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            pending.AddItems(characterId, new Dictionary<Guid, int> { [SupplyCatalog.HealthPotionId] = -1 }));
    }

    // --- kit inicial ---

    [Fact]
    public void StarterKit_ParsedByKey_AddsLegacyKeys_AndRejectsInvalid()
    {
        var kit = SupplySettings.ParseStarterKit(
        [
            new("HealthPotion", 3),
            new("strongmanapotion", 2),
            new("GreatHealthPotion", 0)
        ]);

        var settings = new SupplySettings(2, 1, kit);
        var items = settings.StarterItems();

        Assert.Equal(5, items[SupplyCatalog.HealthPotionId]);
        Assert.Equal(1, items[SupplyCatalog.ManaPotionId]);
        Assert.Equal(2, items[PotionCatalog.StrongManaPotionId]);
        Assert.False(items.ContainsKey(PotionCatalog.GreatHealthPotionId));

        Assert.Throws<InvalidOperationException>(() => SupplySettings.ParseStarterKit([new("Elixir", 1)]));
        Assert.Throws<InvalidOperationException>(() => SupplySettings.ParseStarterKit([new("ManaPotion", -1)]));
        Assert.Throws<ArgumentException>(() => new SupplySettings(0, 0, new Dictionary<Guid, int> { [SpiderCaveCatalog.RatTail.Id] = 1 }));
        Assert.Empty(new SupplySettings().StarterItems());
    }

    private static ServiceProvider Provider(int seed, SupplySettings supplies)
    {
        var services = new ServiceCollection();
        services.AddProjectVictorySmokeTestServices();

        var databaseName = $"potion-tiers-{Guid.NewGuid():N}";
        services.AddDbContext<ProjectVictoryDbContext>(o => o.UseInMemoryDatabase(databaseName));
        services.AddSingleton(new CombatSettings(seed));
        services.AddSingleton(supplies);

        return services.BuildServiceProvider();
    }

    [Fact]
    public async Task NewCharacter_GetsTheStarterKit_AndHuntConsumesByTier()
    {
        var kit = new Dictionary<Guid, int>
        {
            [SupplyCatalog.HealthPotionId] = 1,
            [PotionCatalog.GreatHealthPotionId] = 1,
            [PotionCatalog.StrongManaPotionId] = 2
        };
        await using var provider = Provider(seed: 11, new SupplySettings(0, 0, kit));
        using var scope = provider.CreateScope();
        scope.ServiceProvider.GetRequiredService<TestCurrentPlayerContext>().SetAccount(Guid.NewGuid());

        var characterId = await scope.ServiceProvider.GetRequiredService<CreateCharacterHandler>()
            .Handle(new CreateCharacterCommand("K" + Guid.NewGuid().ToString("N")[..10], CharacterClass.Knight));
        var repository = scope.ServiceProvider.GetRequiredService<ICharacterRepository>();

        int Count(ProjectVictory.Domain.Entities.Character c, Guid id) => c.Inventory.Items.Count(i => i.ItemDefinitionId == id);

        var created = (await repository.GetByIdAsync(characterId))!;
        Assert.Equal(4, created.Inventory.Items.Count);
        Assert.Equal((1, 1, 2), (Count(created, SupplyCatalog.HealthPotionId), Count(created, PotionCatalog.GreatHealthPotionId), Count(created, PotionCatalog.StrongManaPotionId)));

        // Hunt com política de vida em 100%: as poções de vida saem do inventário pelo nível usado.
        var enter = await scope.ServiceProvider.GetRequiredService<EnterGameHandler>().Handle(new EnterGameCommand());
        await scope.ServiceProvider.GetRequiredService<SetSupplyPolicyHandler>()
            .Handle(new SetSupplyPolicyCommand(characterId, 100, null));
        var hunt = await scope.ServiceProvider.GetRequiredService<ExecuteHuntHandler>()
            .Handle(new ExecuteHuntCommand(characterId, enter.RuntimeId));

        var rounds = new List<AttackEncounterResult>();
        for (var i = 0; i < 300 && rounds.LastOrDefault()?.CombatFinished != true; i++)
        {
            rounds.Add(await scope.ServiceProvider.GetRequiredService<AttackEncounterHandler>()
                .Handle(new AttackEncounterCommand(hunt.Encounter.Id)));
        }

        var potionTurns = rounds.SelectMany(r => r.Turns!).Where(t => t.Kind == nameof(CombatActionKind.UseHealthPotion)).ToList();
        Assert.InRange(potionTurns.Count, 1, 2);
        Assert.All(potionTurns, t => Assert.Contains(t.Healing, new[] { 75, 250 }));

        // Checkpoint (recompensa/fim do encounter) grava o consumo por nível.
        await scope.ServiceProvider.GetRequiredService<ICheckpointService>()
            .SaveAsync(characterId, enter.RuntimeId, CheckpointReason.HuntConclusion);
        var after = (await repository.GetByIdAsync(characterId))!;
        var usedSmall = potionTurns.Count(t => t.Healing == 75);
        var usedGreat = potionTurns.Count(t => t.Healing == 250);
        Assert.Equal(1 - usedSmall, Count(after, SupplyCatalog.HealthPotionId));
        Assert.Equal(1 - usedGreat, Count(after, PotionCatalog.GreatHealthPotionId));
        Assert.Equal(2, Count(after, PotionCatalog.StrongManaPotionId)); // a política de Mana está desligada

        // O Hunt Analyzer registra o débito por nível (Id do item).
        var tally = scope.ServiceProvider.GetRequiredService<IHuntAnalyzerTally>().Get(hunt.HuntId);
        Assert.NotNull(tally);
        Assert.Equal(usedSmall, tally!.SuppliesUsed.GetValueOrDefault(SupplyCatalog.HealthPotionId));
        Assert.Equal(usedGreat, tally.SuppliesUsed.GetValueOrDefault(PotionCatalog.GreatHealthPotionId));
    }
}
