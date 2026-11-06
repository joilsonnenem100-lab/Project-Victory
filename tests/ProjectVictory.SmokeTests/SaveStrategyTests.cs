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
        public List<Guid> RuntimeCheckpointIds { get; } = [];
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
            counter.RuntimeCheckpointIds.Add(runtimeId);
            return inner.SaveCheckpointAsync(runtimeId, health, mana, cancellationToken);
        }
    }

    // Semente cujo encounter é o de índice `index` (o último é o Giant Spider: luta longa).
    private static int SeedForGiantSpider() => SeedForTemplate(SpiderCaveCatalog.EncounterTemplates.Count - 1);

    private static int SeedForTemplate(int index)
    {
        for (var seed = 1; ; seed++)
        {
            if (new SeededRandomSource(seed).NextInclusive(0, SpiderCaveCatalog.EncounterTemplates.Count - 1) == index)
            {
                return seed;
            }
        }
    }

    private sealed record World(ServiceProvider Provider, IServiceScope Scope, WriteCounter Counter, Guid CharacterId, Guid RuntimeId) : IAsyncDisposable
    {
        public T Get<T>() where T : notnull => Scope.ServiceProvider.GetRequiredService<T>();

        public async ValueTask DisposeAsync()
        {
            Scope.Dispose();
            await Provider.DisposeAsync();
        }
    }

    private static async Task<World> NewWorld(int seed, int healthPotions, int? potionBelowPercent)
    {
        var counter = new WriteCounter();
        var services = new ServiceCollection();
        services.AddProjectVictorySmokeTestServices();
        var databaseName = $"save-{Guid.NewGuid():N}";
        services.AddDbContext<ProjectVictoryDbContext>(o => o.UseInMemoryDatabase(databaseName));
        services.AddSingleton(new CombatSettings(seed));
        services.AddSingleton(new SupplySettings(healthPotions, 0));
        services.AddScoped<CharacterRepository>();
        services.AddScoped<RuntimeRepository>();
        services.AddScoped<ICharacterRepository>(sp => new CountingCharacterRepository(sp.GetRequiredService<CharacterRepository>(), counter));
        services.AddScoped<IRuntimeRepository>(sp => new CountingRuntimeRepository(sp.GetRequiredService<RuntimeRepository>(), counter));
        var provider = services.BuildServiceProvider();
        var scope = provider.CreateScope();
        scope.ServiceProvider.GetRequiredService<TestCurrentPlayerContext>().SetAccount(Guid.NewGuid());

        var characterId = await scope.ServiceProvider.GetRequiredService<CreateCharacterHandler>()
            .Handle(new CreateCharacterCommand("V" + Guid.NewGuid().ToString("N")[..10], CharacterClass.Knight));
        var enter = await scope.ServiceProvider.GetRequiredService<EnterGameHandler>().Handle(new EnterGameCommand());

        if (potionBelowPercent is { } percent)
        {
            // A ativação da política grava UMA vez (mudança de configuração do jogador, PV-PERSIST-010);
            // essa escrita é na tabela SupplyPolicies e fica fora destas contagens.
            await scope.ServiceProvider.GetRequiredService<SetSupplyPolicyHandler>()
                .Handle(new SetSupplyPolicyCommand(characterId, percent, null));
        }

        return new World(provider, scope, counter, characterId, enter.RuntimeId);
    }

    private static async Task<List<AttackEncounterResult>> Fight(World world, Guid encounterId, Action<AttackEncounterResult, (int, int)>? perRound = null)
    {
        var rounds = new List<AttackEncounterResult>();

        for (var i = 0; i < 300 && rounds.LastOrDefault()?.CombatFinished != true; i++)
        {
            var before = (world.Counter.CharacterWrites, world.Counter.RuntimeWrites);
            var round = await world.Get<AttackEncounterHandler>().Handle(new AttackEncounterCommand(encounterId));
            rounds.Add(round);
            perRound?.Invoke(round, before);
        }

        return rounds;
    }

    [Fact]
    public async Task CombatRounds_DoNotWrite_OnlyCheckpointsDo()
    {
        await using var world = await NewWorld(SeedForGiantSpider(), 5, 90);
        var counter = world.Counter;
        var characterId = world.CharacterId;
        var scope = world.Scope;

        var hunt = await scope.ServiceProvider.GetRequiredService<ExecuteHuntHandler>()
            .Handle(new ExecuteHuntCommand(characterId, world.RuntimeId));
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

    // Revisão do PV-PERSIST-011 (ressalva 5): vitória explícita — checkpoints de recompensa e de
    // conclusão; a XP gravada pelo Reward Pipeline fica (não é sobrescrita pelo checkpoint).
    [Fact]
    public async Task Victory_RewardAndConclusionCheckpoints_ExperienceKept()
    {
        await using var world = await NewWorld(SeedForTemplate(0), 0, null);
        var hunt = await world.Get<ExecuteHuntHandler>().Handle(new ExecuteHuntCommand(world.CharacterId, world.RuntimeId));

        var rounds = await Fight(world, hunt.Encounter.Id);

        Assert.Equal(nameof(HuntEndReason.Victory), rounds[^1].HuntEndReason);
        var log = world.Get<ICheckpointLog>().For(world.CharacterId);
        Assert.Equal(CheckpointReason.HuntStart, log[0]);
        Assert.Contains(CheckpointReason.Reward, log);
        Assert.Equal(CheckpointReason.HuntConclusion, log[^1]);

        var character = await world.Get<ICharacterRepository>().GetByIdAsync(world.CharacterId);
        Assert.Equal(rounds.Sum(r => (long)r.Experience), character!.Experience.Value);
        Assert.True(character.Experience.Value > 0);
    }

    // Revisão do PV-PERSIST-011 (ressalva 5): morte explícita — sem recompensa, checkpoint de
    // conclusão grava a cópia do Runtime com a vida já restaurada (Cidade Principal).
    [Fact]
    public async Task Death_ConclusionCheckpoint_SavesRestoredRuntime()
    {
        await using var world = await NewWorld(SeedForGiantSpider(), 0, null);
        var hunt = await world.Get<ExecuteHuntHandler>().Handle(new ExecuteHuntCommand(world.CharacterId, world.RuntimeId));

        var rounds = await Fight(world, hunt.Encounter.Id);

        Assert.Equal(nameof(HuntEndReason.Death), rounds[^1].HuntEndReason);
        var log = world.Get<ICheckpointLog>().For(world.CharacterId);
        Assert.Equal(new[] { CheckpointReason.HuntStart, CheckpointReason.HuntConclusion }, log);

        var saved = await world.Get<IRuntimeRepository>().GetByCharacterIdAsync(world.CharacterId);
        var character = await world.Get<ICharacterRepository>().GetByIdAsync(world.CharacterId);
        Assert.Equal(character!.MaximumHealth.Value, saved!.Health);
    }

    // Revisão do PV-PERSIST-011 (ressalva 1, Server Authority): o checkpoint usa o Runtime do servidor,
    // nunca o RuntimeId enviado no corpo da requisição.
    [Fact]
    public async Task StartCheckpoint_IgnoresTheRuntimeIdFromTheRequest()
    {
        await using var world = await NewWorld(SeedForTemplate(0), 0, null);

        await world.Get<ExecuteHuntHandler>().Handle(new ExecuteHuntCommand(world.CharacterId, Guid.NewGuid()));

        Assert.Equal(new[] { world.RuntimeId }, world.Counter.RuntimeCheckpointIds);
    }

    [Fact]
    public void Subtract_RemovesOnlyWhatWasSaved()
    {
        var pending = new PendingSupplyConsumption();
        var characterId = Guid.NewGuid();
        pending.Add(characterId, 3, 1);

        pending.Subtract(characterId, 2, 1);
        Assert.Equal((1, 0), pending.Peek(characterId));

        pending.Subtract(characterId, 1, 0);
        Assert.Equal((0, 0), pending.Peek(characterId));
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
