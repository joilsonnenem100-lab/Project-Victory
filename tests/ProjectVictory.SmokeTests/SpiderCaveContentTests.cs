using Microsoft.EntityFrameworkCore;
using ProjectVictory.Application.Hunt;
using ProjectVictory.Application.Supply;
using ProjectVictory.Domain.Hunt;
using ProjectVictory.Domain.Items;
using ProjectVictory.Infrastructure.Combat;
using ProjectVictory.Infrastructure.Content;
using ProjectVictory.Infrastructure.Loot;
using ProjectVictory.Persistence.Database;
using Xunit;

namespace ProjectVictory.SmokeTests;

// PV-CONTENT-011: Spider Cave definida por dados (DEC-20261106-09) + pendências do M2.
public sealed class SpiderCaveContentTests
{
    [Fact]
    public void Catalog_HasTheFiveConfirmedMonsters_EachWithCombatAndLoot()
    {
        Assert.Equal(
            new[] { "Rat", "Cave Rat", "Spider", "Poison Spider", "Giant Spider" },
            SpiderCaveCatalog.Monsters.Select(m => m.Name));

        var loot = new MonsterLootDefinitionProvider();
        var combat = new MonsterCombatProfileProvider();

        foreach (var monster in SpiderCaveCatalog.Monsters)
        {
            Assert.True(monster.Loot.Experience > 0, monster.Name);
            Assert.Same(monster.Loot, loot.GetByOpponentId(monster.Id));
            Assert.NotSame(MonsterCombatProfileProvider.Fallback, combat.Get(monster.Id));
            Assert.Equal(monster.Combat, combat.Get(monster.Id).Profile);
        }
    }

    [Fact]
    public void CaveRat_NowGivesExperienceAndLoot()
    {
        var caveRat = new MonsterLootDefinitionProvider().GetByOpponentId(SpiderCaveCatalog.CaveRatId);

        Assert.NotNull(caveRat);
        Assert.Equal(15, caveRat!.Experience);
        Assert.Contains(caveRat.Drops, d => d.Item.Name == "Cheese");
    }

    [Fact]
    public void EveryDroppableItem_HasAnNpcSellPrice()
    {
        var drops = SpiderCaveCatalog.Monsters.SelectMany(m => m.Loot.Drops).Select(d => d.Item).Distinct();

        Assert.All(drops, item => Assert.True(item.NpcSellPrice > 0, item.Name));
    }

    [Theory]
    [InlineData(-0.1)]
    [InlineData(1.01)]
    [InlineData(double.NaN)]
    public void DropChance_OutsideZeroToOne_IsRejected(double chance)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new LootDropDefinition(new ItemDefinition(Guid.NewGuid(), "X", new ItemCharacteristics("x")), chance));
    }

    [Fact]
    public void EncounterComposition_IsDrawnFromTheAreaBySeed_Deterministically()
    {
        var generator = new EncounterGenerator(new SpiderCaveHuntArea());
        var templates = SpiderCaveCatalog.EncounterTemplates;

        for (var seed = 1; seed <= 20; seed++)
        {
            var a = generator.Generate(Guid.NewGuid(), seed).Monsters.Select(m => m.MonsterId).ToList();
            var b = generator.Generate(Guid.NewGuid(), seed).Monsters.Select(m => m.MonsterId).ToList();

            Assert.Equal(a, b);
            Assert.Contains(templates, t => t.SequenceEqual(a));
        }

        var seen = Enumerable.Range(1, 200)
            .Select(seed => string.Join(",", generator.Generate(Guid.NewGuid(), seed).Monsters.Select(m => m.Name)))
            .Distinct()
            .Count();

        Assert.Equal(templates.Count, seen);
    }

    [Fact]
    public async Task ItemsTable_MatchesTheCatalogs_InRealSqlite()
    {
        var path = Path.Combine(Path.GetTempPath(), $"pv-content-{Guid.NewGuid():N}.db");

        try
        {
            await using var context = new ProjectVictoryDbContext(
                new DbContextOptionsBuilder<ProjectVictoryDbContext>().UseSqlite($"Data Source={path};Pooling=False").Options);
            await context.Database.MigrateAsync();

            var expected = SpiderCaveCatalog.LootItems.Concat([SupplyCatalog.HealthPotion, SupplyCatalog.ManaPotion]);

            foreach (var item in expected)
            {
                var row = await context.Items.SingleAsync(x => x.Id == item.Id);

                Assert.Equal(item.Name, row.Name);
                Assert.Equal(item.Characteristics.Description, row.Description);
                Assert.Equal(item.NpcSellPrice, row.NpcSellPrice);
                Assert.Equal(item.NpcBuyPrice, row.NpcBuyPrice);
            }

            // O seed do Training Sword continua funcionando com a tabela já populada.
            await ProjectVictory.Persistence.Database.Seed.ItemDefinitionSeed.ExecuteAsync(context);
            Assert.True(await context.Items.AnyAsync(x => x.Name == "Training Sword"));

            // O repositório lê o preço de compra (poção).
            var potion = await new ProjectVictory.Persistence.Item.ItemDefinitionRepository(context).GetByIdAsync(SupplyCatalog.HealthPotionId);
            Assert.Equal(50, potion!.NpcBuyPrice);
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
}
