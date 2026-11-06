using Microsoft.EntityFrameworkCore;
using ProjectVictory.Domain.Account;
using ProjectVictory.Domain.Character;
using ProjectVictory.Domain.ValueObjects;
using ProjectVictory.Persistence.Character;
using ProjectVictory.Persistence.Database;
using Xunit;
using CharacterEntity = ProjectVictory.Domain.Entities.Character;

namespace ProjectVictory.SmokeTests;

// PV-PROG-010: Experience long persistida em SQLite real (todas as migrations aplicadas);
// o Level gravado é só cache — ao carregar, o Level vem da Experience.
public sealed class CharacterExperiencePersistenceTests : IDisposable
{
    private readonly string _databasePath =
        Path.Combine(Path.GetTempPath(), $"pv-prog-010-{Guid.NewGuid():N}.db");

    private ProjectVictoryDbContext NewContext() =>
        new(new DbContextOptionsBuilder<ProjectVictoryDbContext>()
            .UseSqlite($"Data Source={_databasePath};Pooling=False")
            .Options);

    [Fact]
    public async Task ExperienceAboveIntRange_RoundTrips_AndLevelIsDerived()
    {
        await using (var context = NewContext())
        {
            await context.Database.MigrateAsync();
        }

        var definition = CharacterClassDefinition.Get(CharacterClass.Knight);
        var character = new CharacterEntity(
            Guid.NewGuid(),
            new AccountId(Guid.NewGuid()),
            "BigXp",
            CharacterClass.Knight,
            definition,
            new Health(definition.InitialHealth),
            new Mana(definition.InitialMana),
            new Experience(3_000_000_000L));

        await using (var context = NewContext())
        {
            await new CharacterRepository(context).AddAsync(character);
        }

        await using (var context = NewContext())
        {
            var state = await context.Characters.SingleAsync(x => x.Id == character.Id);
            Assert.Equal(3_000_000_000L, state.Experience);
            Assert.Equal(566, state.Level); // cache gravado com o valor derivado (PV-PROG-012: Level sem teto; antes 500)

            // Simula um cache antigo divergente: não pode ser lido como verdade.
            state.Level = 3;
            await context.SaveChangesAsync();
        }

        await using (var context = NewContext())
        {
            var loaded = await new CharacterRepository(context).GetByIdAsync(character.Id);
            Assert.NotNull(loaded);
            Assert.Equal(3_000_000_000L, loaded!.Experience.Value);
            Assert.Equal(566, loaded.Level.Value);
        }
    }

    [Fact]
    public async Task Gold_AndItemSellPrice_ArePersisted()
    {
        // PV-REWARD-010: colunas novas da migration InventoryGoldAndItemSellPrice.
        await using (var context = NewContext())
        {
            await context.Database.MigrateAsync();
            await ProjectVictory.Persistence.Database.Seed.ItemDefinitionSeed.ExecuteAsync(context);
        }

        var definition = CharacterClassDefinition.Get(CharacterClass.Knight);
        var character = new CharacterEntity(
            Guid.NewGuid(),
            new AccountId(Guid.NewGuid()),
            "GoldOwner",
            CharacterClass.Knight,
            definition,
            new Health(definition.InitialHealth),
            new Mana(definition.InitialMana),
            new Experience(0));

        character.Inventory.AddGold(42);

        await using (var context = NewContext())
        {
            await new CharacterRepository(context).AddAsync(character);
        }

        await using (var context = NewContext())
        {
            var loaded = await new CharacterRepository(context).GetByIdAsync(character.Id);
            Assert.Equal(42, loaded!.Inventory.Gold);

            loaded.Inventory.AddGold(8);
            await new CharacterRepository(context).UpdateAsync(loaded);
        }

        await using (var context = NewContext())
        {
            var reloaded = await new CharacterRepository(context).GetByIdAsync(character.Id);
            Assert.Equal(50, reloaded!.Inventory.Gold);

            var sword = await context.Items.SingleAsync(x => x.Name == "Training Sword");
            var item = await new ProjectVictory.Persistence.Item.ItemDefinitionRepository(context).GetByIdAsync(sword.Id);
            Assert.Equal(10, item!.NpcSellPrice);
        }
    }

    public void Dispose()
    {
        foreach (var suffix in new[] { "", "-wal", "-shm" })
        {
            try { File.Delete(_databasePath + suffix); } catch (IOException) { }
        }
    }
}
