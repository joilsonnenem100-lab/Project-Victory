using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectVictory.Application.Character;
using ProjectVictory.Application.Combat.TurnBased;
using ProjectVictory.Application.Game;
using ProjectVictory.Application.Hunt;
using ProjectVictory.Application.VerticalSlice;
using ProjectVictory.Domain.Character;
using ProjectVictory.Domain.Hunt;
using ProjectVictory.Persistence.Database;
using Xunit;

namespace ProjectVictory.SmokeTests;

// PV-HUNT-011 (DEC-20261106-22; fecha R-036 e R-044): Hunt Loop contínuo — vencer um encounter gera o
// próximo (Spawn) até morte, saída ou queda; memória limpa no fim; comandos serializados por Hunt;
// tempo e XP medidos para o XP/hora.
public sealed class HuntLoopTests
{
    private sealed record World(ServiceProvider Provider, IServiceScope Scope, Guid CharacterId, Guid RuntimeId, ManualTimeProvider Clock) : IAsyncDisposable
    {
        public T Get<T>() where T : notnull => Scope.ServiceProvider.GetRequiredService<T>();

        public async ValueTask DisposeAsync()
        {
            Scope.Dispose();
            await Provider.DisposeAsync();
        }
    }

    private static async Task<World> NewWorld(int seed)
    {
        var clock = new ManualTimeProvider();
        var services = new ServiceCollection();
        services.AddProjectVictorySmokeTestServices();
        var databaseName = $"loop-{Guid.NewGuid():N}";
        services.AddDbContext<ProjectVictoryDbContext>(o => o.UseInMemoryDatabase(databaseName));
        services.AddSingleton(new CombatSettings(seed));
        services.AddSingleton<TimeProvider>(clock);
        var provider = services.BuildServiceProvider();
        var scope = provider.CreateScope();
        scope.ServiceProvider.GetRequiredService<TestCurrentPlayerContext>().SetAccount(Guid.NewGuid());

        var characterId = await scope.ServiceProvider.GetRequiredService<CreateCharacterHandler>()
            .Handle(new CreateCharacterCommand("L" + Guid.NewGuid().ToString("N")[..10], CharacterClass.Knight));
        var enter = await scope.ServiceProvider.GetRequiredService<EnterGameHandler>().Handle(new EnterGameCommand());

        return new World(provider, scope, characterId, enter.RuntimeId, clock);
    }

    private static async Task<AttackEncounterResult> FightEncounter(World world, Guid encounterId)
    {
        AttackEncounterResult? last = null;

        for (var i = 0; i < 300 && last?.CombatFinished != true; i++)
        {
            last = await world.Get<AttackEncounterHandler>().Handle(new AttackEncounterCommand(encounterId));
        }

        return last!;
    }

    [Fact]
    public async Task Victory_GeneratesTheNextEncounter_ThreeEncountersInARow_UntilLeave()
    {
        await using var world = await NewWorld(1);
        var hunt = await world.Get<ExecuteHuntHandler>().Handle(new ExecuteHuntCommand(world.CharacterId, world.RuntimeId));
        var encounterId = hunt.Encounter.Id;
        var seen = new List<Guid> { encounterId };
        long xp = 0;

        for (var n = 0; n < 3; n++)
        {
            world.Clock.Advance(TimeSpan.FromMinutes(1));
            var last = await FightEncounter(world, encounterId);

            Assert.Equal("Victory", last.CombatState);
            Assert.Null(last.HuntEndReason);
            Assert.NotNull(last.NextEncounterId);
            Assert.Equal(hunt.HuntId, last.NextEncounter!.HuntId);
            xp = last.HuntExperienceGained;

            encounterId = last.NextEncounterId!.Value;
            Assert.DoesNotContain(encounterId, seen);
            seen.Add(encounterId);
        }

        // Só o encounter em andamento fica na memória (os vencidos foram descartados e despawnados).
        var store = (EncounterStateStore)world.Get<IEncounterStateStore>();
        Assert.Equal(1, store.Count);
        Assert.Equal(1, world.Get<IActiveSpawnRegistry>().Count);

        world.Clock.Advance(TimeSpan.FromMinutes(57)); // 1 h no total
        var ending = await world.Get<LeaveHuntHandler>().Handle(new LeaveHuntCommand(hunt.HuntId));

        Assert.Equal(HuntEndReason.Manual, ending.Reason);
        Assert.Equal(3600, ending.ElapsedSeconds);
        Assert.Equal(xp, ending.ExperienceGained);
        Assert.Equal(xp, ending.ExperiencePerHour); // 1 h exata → XP/hora = XP da Hunt
        Assert.True(xp > 0);

        // Memória limpa: nenhum encounter, Spawn Instance, Hunt ou lock da Hunt encerrada.
        Assert.Equal(0, store.Count);
        Assert.Equal(0, world.Get<IActiveSpawnRegistry>().Count);
        Assert.Null(await world.Get<IHuntRepository>().GetByIdAsync(hunt.HuntId));
        Assert.Equal(0, ((HuntLock)world.Get<IHuntLock>()).Count);
    }

    [Fact]
    public async Task LeavingTwice_TheSecondIsRejected()
    {
        await using var world = await NewWorld(1);
        var hunt = await world.Get<ExecuteHuntHandler>().Handle(new ExecuteHuntCommand(world.CharacterId, world.RuntimeId));

        await world.Get<LeaveHuntHandler>().Handle(new LeaveHuntCommand(hunt.HuntId));

        // A Hunt saiu da memória: responde como Hunt desconhecida (403 na API), sem revelar o id.
        await Assert.ThrowsAsync<ProjectVictory.Application.Session.OwnershipViolationException>(() => world.Get<LeaveHuntHandler>().Handle(new LeaveHuntCommand(hunt.HuntId)));
        await Assert.ThrowsAsync<InvalidOperationException>(() => world.Get<AttackEncounterHandler>().Handle(new AttackEncounterCommand(hunt.Encounter.Id)));
    }

    // PV-STATE-001 D3: dois ataques simultâneos no mesmo encounter são executados um depois do outro.
    [Fact]
    public async Task ConcurrentAttacks_OnTheSameHunt_AreSerialized()
    {
        await using var world = await NewWorld(1);
        var hunt = await world.Get<ExecuteHuntHandler>().Handle(new ExecuteHuntCommand(world.CharacterId, world.RuntimeId));

        async Task<AttackEncounterResult> AttackInOwnScope()
        {
            using var scope = world.Provider.CreateScope();
            scope.ServiceProvider.GetRequiredService<TestCurrentPlayerContext>()
                .SetAccount(world.Get<TestCurrentPlayerContext>().AccountId);
            return await scope.ServiceProvider.GetRequiredService<AttackEncounterHandler>().Handle(new AttackEncounterCommand(hunt.Encounter.Id));
        }

        var results = await Task.WhenAll(AttackInOwnScope(), AttackInOwnScope());

        // Rodadas diferentes: nenhum turno aparece nas duas respostas.
        var turnsA = results[0].Turns!.Select(t => t.Turn).ToHashSet();
        var turnsB = results[1].Turns!.Select(t => t.Turn).ToHashSet();
        Assert.Empty(turnsA.Intersect(turnsB));
    }
}
