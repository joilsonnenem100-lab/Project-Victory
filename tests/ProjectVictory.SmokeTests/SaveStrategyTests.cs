using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectVictory.Application.Character;
using ProjectVictory.Application.Checkpoint;
using ProjectVictory.Application.Combat.TurnBased;
using ProjectVictory.Application.Game;
using ProjectVictory.Application.Hunt;
using ProjectVictory.Application.Supply;
using ProjectVictory.Application.VerticalSlice;
using ProjectVictory.Domain.Character;
using ProjectVictory.Domain.Combat.TurnBased;
using ProjectVictory.Domain.Primitives;
using ProjectVictory.Infrastructure.Content;
using ProjectVictory.Persistence.Character;
using ProjectVictory.Persistence.Database;
using ProjectVictory.Persistence.Runtime;
using Xunit;
using CharacterEntity = ProjectVictory.Domain.Entities.Character;

namespace ProjectVictory.SmokeTests;

// PV-PERSIST-011 (Handbook §19): salvar em início, recompensa, conclusão (e logout); NÃO a cada
// ação pequena. Uma rodada de combate sem morte de monstro e sem fim da Hunt não grava nada — nem
// quando o jogador usa poção (antes gravava o personagem nessa rodada).
public sealed class SaveStrategyTests
{
    private sealed class WriteCounter
    {
        public int CharacterWrites;
        public int RuntimeWrites;
    }

    private sealed class CountingCharacterRepository(ICharacterRepository inner, WriteCounter counter) : ICharacterRepository
    {
        public Task AddAsync(CharacterEntity character, CancellationToken cancellationToken = default)
        {
            counter.CharacterWrites++;
            return inner.AddAsync(character, cancellationToken);
        }

        public Task UpdateAsync(CharacterEntity character, CancellationToken cancellationToken = default)
        {
            counter.CharacterWrites++;
            return inner.UpdateAsync(character, cancellationToken);
        }

        public Task<CharacterEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) => inner.GetByIdAsync(id, cancellationToken);

        public Task<CharacterEntity?> GetByAccountIdAsync(Guid accountId, CancellationToken cancellationToken = default) => inner.GetByAccountIdAsync(accountId, cancellationToken);

        public Task<IReadOnlyList<CharacterEntity>> GetAllByAccountIdAsync(Guid accountId, CancellationToken cancellationToken = default) => inner.GetAllByAccountIdAsync(accountId, cancellationToken);

        public Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default) => inner.ExistsByNameAsync(name, cancellationToken);
    }

    private sealed class CountingRuntimeRepository(IRuntimeRepository inner, WriteCounter counter) : IRuntimeRepository
    {
        public Task AddAsync(Guid runtimeId, Guid characterId, int health, int mana, CancellationToken cancellationToken = default)
        {
            counter.RuntimeWrites++;
            return inner.AddAsync(runtimeId, characterId, health, mana, cancellationToken);
        }

        public Task<RuntimeRecoveryData?> GetByCharacterIdAsync(Guid characterId, CancellationToken cancellationToken = default) =>
            inner.GetByCharacterIdAsync(characterId, cancellationToken);

        public Task SaveCheckpointAsync(Guid runtimeId, int health, int mana, CancellationToken cancellationToken = default)
        {
            counter.RuntimeWrites++;
            return inner.SaveCheckpointAsync(runtimeId, health, mana, cancellationToken);
        }
    }

    // Semente cujo encounter é o Giant Spider (luta longa: muitas rodadas sem morte de monstro).
    private static int SeedForGiantSpider()
    {
        var index = SpiderCaveCatalog.EncounterTemplates.Count - 1;
        for (var seed = 1; ; seed++)
        {
            if (new SeededRandomSource(seed).NextInclusive(0, SpiderCaveCatalog.EncounterTemplates.Count - 1) == index)
            {
                return seed;
            }
        }
    }

    [Fact]
    public async Task CombatRounds_DoNotWrite_OnlyCheckpointsDo()
    {
        var counter = new WriteCounter();
        var services = new ServiceCollection();
        services.AddProjectVictorySmokeTestServices();
        var databaseName = $"save-{Guid.NewGuid():N}";
        services.AddDbContext<ProjectVictoryDbContext>(o => o.UseInMemoryDatabase(databaseName));
        services.AddSingleton(new CombatSettings(SeedForGiantSpider()));
        services.AddSingleton(new SupplySettings(5, 0));
        services.AddScoped<CharacterRepository>();
        services.AddScoped<RuntimeRepository>();
        services.AddScoped<ICharacterRepository>(sp => new CountingCharacterRepository(sp.GetRequiredService<CharacterRepository>(), counter));
        services.AddScoped<IRuntimeRepository>(sp => new CountingRuntimeRepository(sp.GetRequiredService<RuntimeRepository>(), counter));
        await using var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();
        scope.ServiceProvider.GetRequiredService<TestCurrentPlayerContext>().SetAccount(Guid.NewGuid());

        var characterId = await scope.ServiceProvider.GetRequiredService<CreateCharacterHandler>()
            .Handle(new CreateCharacterCommand("V" + Guid.NewGuid().ToString("N")[..10], CharacterClass.Knight));
        var enter = await scope.ServiceProvider.GetRequiredService<EnterGameHandler>().Handle(new EnterGameCommand());
        await scope.ServiceProvider.GetRequiredService<SetSupplyPolicyHandler>()
            .Handle(new SetSupplyPolicyCommand(characterId, 90, null));

        var hunt = await scope.ServiceProvider.GetRequiredService<ExecuteHuntHandler>()
            .Handle(new ExecuteHuntCommand(characterId, enter.RuntimeId));
        var log = scope.ServiceProvider.GetRequiredService<ICheckpointLog>();
        Assert.Equal([CheckpointReason.HuntStart], log.For(characterId));

        var quietRounds = 0;
        var quietRoundsWithPotion = 0;
        var potionTurns = 0;
        AttackEncounterResult? last = null;

        for (var i = 0; i < 300 && last?.CombatFinished != true; i++)
        {
            var before = (counter.CharacterWrites, counter.RuntimeWrites);
            last = await scope.ServiceProvider.GetRequiredService<AttackEncounterHandler>()
                .Handle(new AttackEncounterCommand(hunt.Encounter.Id));
            potionTurns += last.Turns!.Count(t => t.Kind == nameof(CombatActionKind.UseHealthPotion));

            if (!last.MonsterDefeated && !last.CombatFinished)
            {
                // Rodada "pequena": nenhuma escrita, nem no personagem nem na cópia do Runtime.
                Assert.Equal(before, (counter.CharacterWrites, counter.RuntimeWrites));
                quietRounds++;

                if (last.Turns!.Any(t => t.Kind == nameof(CombatActionKind.UseHealthPotion)))
                {
                    quietRoundsWithPotion++;
                }
            }
        }

        Assert.True(last!.CombatFinished);
        Assert.True(quietRounds >= 2, $"esperava várias rodadas sem checkpoint, houve {quietRounds}");
        Assert.True(quietRoundsWithPotion >= 1, "esperava ao menos uma rodada com poção sem gravação");

        // O fim da Hunt é checkpoint de conclusão (e, na vitória, também de recompensa).
        Assert.Contains(CheckpointReason.HuntConclusion, log.For(characterId));

        // Nada se perdeu: as poções usadas saíram do inventário no checkpoint.
        var potionsUsed = scope.ServiceProvider.GetRequiredService<IPendingSupplyConsumption>().Peek(characterId);
        Assert.Equal((0, 0), potionsUsed);
        var character = await scope.ServiceProvider.GetRequiredService<ICharacterRepository>().GetByIdAsync(characterId);
        Assert.Equal(5 - potionTurns, character!.Inventory.Items.Count(item => item.ItemDefinitionId == SupplyCatalog.HealthPotionId));
    }

    [Fact]
    public void PendingConsumption_Accumulates_AndIsTakenOnce()
    {
        var pending = new PendingSupplyConsumption();
        pending.Add(Guid.Empty, 0, 0);
        var characterId = Guid.NewGuid();

        pending.Add(characterId, 1, 0);
        pending.Add(characterId, 2, 1);

        Assert.Equal((3, 1), pending.Peek(characterId));
        Assert.Equal((3, 1), pending.Take(characterId));
        Assert.Equal((0, 0), pending.Peek(characterId));
        Assert.Throws<ArgumentOutOfRangeException>(() => pending.Add(characterId, -1, 0));
    }
}
