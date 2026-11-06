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
            Assert.Equal(500, state.Level); // cache gravado com o valor derivado

            // Simula um cache antigo divergente: não pode ser lido como verdade.
            state.Level = 3;
            await context.SaveChangesAsync();
        }

        await using (var context = NewContext())
        {
            var loaded = await new CharacterRepository(context).GetByIdAsync(character.Id);
            Assert.NotNull(loaded);
            Assert.Equal(3_000_000_000L, loaded!.Experience.Value);
            Assert.Equal(500, loaded.Level.Value);
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
