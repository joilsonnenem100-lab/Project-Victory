using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectVictory.Application.Character;
using ProjectVictory.Application.Checkpoint;
using ProjectVictory.Application.Combat.TurnBased;
using ProjectVictory.Application.Game;
using ProjectVictory.Application.GameSession;
using ProjectVictory.Application.Hunt;
using ProjectVictory.Application.VerticalSlice;
using ProjectVictory.Domain.Character;
using ProjectVictory.Domain.Hunt;
using ProjectVictory.Domain.Runtime;
using ProjectVictory.Persistence.Database;
using Xunit;

namespace ProjectVictory.SmokeTests;

// PV-HUNT-012 (ADR-012, ADR-057 v1.2, DEC-20261106-20): queda > 1 min encerra a Hunt pelo MESMO
// pipeline (motivo Disconnect), volta à "Victoria", nada é perdido; queda curta não encerra; logout
// explícito encerra (motivo Logout) e liga o checkpoint de logout (R-048).
public sealed class DisconnectTests
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

    private static async Task<World> NewWorld(bool withCharacter = true)
    {
        var clock = new ManualTimeProvider();
        var services = new ServiceCollection();
        services.AddProjectVictorySmokeTestServices();
        var databaseName = $"drop-{Guid.NewGuid():N}";
        services.AddDbContext<ProjectVictoryDbContext>(o => o.UseInMemoryDatabase(databaseName));
        services.AddSingleton(new CombatSettings(1));
        services.AddSingleton<TimeProvider>(clock);
        var provider = services.BuildServiceProvider();
        var scope = provider.CreateScope();
        scope.ServiceProvider.GetRequiredService<TestCurrentPlayerContext>().SetAccount(Guid.NewGuid());

        if (!withCharacter)
        {
            return new World(provider, scope, Guid.Empty, Guid.Empty, clock);
        }

        var characterId = await scope.ServiceProvider.GetRequiredService<CreateCharacterHandler>()
            .Handle(new CreateCharacterCommand("D" + Guid.NewGuid().ToString("N")[..10], CharacterClass.Knight));
        var enter = await scope.ServiceProvider.GetRequiredService<EnterGameHandler>().Handle(new EnterGameCommand());

        return new World(provider, scope, characterId, enter.RuntimeId, clock);
    }

    private static async Task<long> WinOneEncounter(World world, Guid encounterId)
    {
                AttackEncounterResult? last = null;

        for (var i = 0; i < 300 && last?.CombatFinished != true; i++)
        {
            last = await world.Get<AttackEncounterHandler>().Handle(new AttackEncounterCommand(encounterId));
        }

        Assert.Equal("Victory", last!.CombatState);
        return last.HuntExperienceGained;
    }

    [Fact]
    public async Task DropOfMoreThanOneMinute_EndsTheHuntThroughThePipeline_BackToVictoria_NothingLost()
    {
        await using var world = await NewWorld();
        var hunt = await world.Get<ExecuteHuntHandler>().Handle(new ExecuteHuntCommand(world.CharacterId, world.RuntimeId));
        var xp = await WinOneEncounter(world, hunt.Encounter.Id);

        // Queda: sem sinal por mais de 1 minuto.
        world.Clock.Advance(TimeSpan.FromSeconds(61));
        var dropped = await world.Get<GameSessionCoordinator>().SweepAsync();

        Assert.Equal(world.CharacterId, Assert.Single(dropped).CharacterId);
        Assert.Null(await world.Get<IHuntRepository>().GetByIdAsync(hunt.HuntId));
        Assert.Null(await world.Get<IHuntRepository>().GetRunningByCharacterIdAsync(world.CharacterId));

        var runtime = world.Get<IRuntimeExecutionStateStore>().Get(world.RuntimeId);
        Assert.Equal(EndHuntHandler.MainCity, runtime.WorldContext);
        Assert.Equal(RuntimeActivity.Idle, runtime.GameplayContext!.CurrentActivity);

        var log = world.Get<ICheckpointLog>().For(world.CharacterId);
        Assert.Contains(CheckpointReason.HuntConclusion, log);
        Assert.Equal(CheckpointReason.Logout, log[^1]);

        var character = await world.Get<global::ICharacterRepository>().GetByIdAsync(world.CharacterId);
        Assert.Equal(xp, character!.Experience.Value); // nenhum ganho perdido
    }

    [Fact]
    public async Task ShortDrop_DoesNotEndTheHunt()
    {
        await using var world = await NewWorld();
        var hunt = await world.Get<ExecuteHuntHandler>().Handle(new ExecuteHuntCommand(world.CharacterId, world.RuntimeId));
        var coordinator = world.Get<GameSessionCoordinator>();

        world.Clock.Advance(TimeSpan.FromSeconds(45));
        Assert.Empty(await coordinator.SweepAsync());
        world.Clock.Advance(TimeSpan.FromSeconds(15)); // exatamente 60 s: segue
        Assert.Empty(await coordinator.SweepAsync());

        // Volta antes de passar 1 minuto: a Hunt continua e o ataque funciona.
        await world.Get<HeartbeatHandler>().Handle(new HeartbeatCommand());
        Assert.Equal(HuntState.Running, (await world.Get<IHuntRepository>().GetByIdAsync(hunt.HuntId))!.State);
        await world.Get<AttackEncounterHandler>().Handle(new AttackEncounterCommand(hunt.Encounter.Id));
    }

    [Fact]
    public async Task Logout_EndsTheHunt_SavesCheckpoint_AndDoesNotTriggerAReturn()
    {
        await using var world = await NewWorld();
        var hunt = await world.Get<ExecuteHuntHandler>().Handle(new ExecuteHuntCommand(world.CharacterId, world.RuntimeId));

        var result = await world.Get<LogoutHandler>().Handle(new LogoutCommand());

        Assert.True(result.HuntEnded);
        Assert.Equal("Victoria", result.ReturnCity);
        Assert.Equal(HuntEndReason.Logout, result.Ending!.Reason);
        Assert.Null(await world.Get<IHuntRepository>().GetByIdAsync(hunt.HuntId));
        Assert.Equal(CheckpointReason.Logout, world.Get<ICheckpointLog>().For(world.CharacterId)[^1]);
        Assert.Null(world.Get<IPresenceTracker>().Get(world.CharacterId));

        // Logout não vira "queda": voltar depois não inicia Hunt sozinho.
        world.Clock.Advance(TimeSpan.FromMinutes(5));
        Assert.Empty(await world.Get<GameSessionCoordinator>().SweepAsync());
        await world.Get<HeartbeatHandler>().Handle(new HeartbeatCommand());
        world.Clock.Advance(TimeSpan.FromSeconds(20));
        Assert.False((await world.Get<HeartbeatHandler>().Handle(new HeartbeatCommand())).ReturnedToLastHunt);
    }

    [Fact]
    public async Task Drop_EndsEveryRunningHuntOfTheCharacter()
    {
        await using var world = await NewWorld();
        await world.Get<ExecuteHuntHandler>().Handle(new ExecuteHuntCommand(world.CharacterId, world.RuntimeId));
        await world.Get<ExecuteHuntHandler>().Handle(new ExecuteHuntCommand(world.CharacterId, world.RuntimeId));

        world.Clock.Advance(TimeSpan.FromMinutes(2));
        await world.Get<GameSessionCoordinator>().SweepAsync();

        Assert.Null(await world.Get<IHuntRepository>().GetRunningByCharacterIdAsync(world.CharacterId));
    }

    // Revisão do PV-EVENT-010/011 (ressalva 7b): o logout encerra TODAS as Hunts em andamento.
    [Fact]
    public async Task Logout_EndsEveryRunningHunt_ReportsTheFirst()
    {
        await using var world = await NewWorld();
        var first = await world.Get<ExecuteHuntHandler>().Handle(new ExecuteHuntCommand(world.CharacterId, world.RuntimeId));
        await world.Get<ExecuteHuntHandler>().Handle(new ExecuteHuntCommand(world.CharacterId, world.RuntimeId));

        var result = await world.Get<LogoutHandler>().Handle(new LogoutCommand());

        Assert.True(result.HuntEnded);
        Assert.Null(await world.Get<IHuntRepository>().GetRunningByCharacterIdAsync(world.CharacterId));
        Assert.Contains(result.Ending!.HuntId, new[] { first.HuntId, result.Ending.HuntId });
    }

    // Revisão do PV-HUNT-011 (ressalva 5b): conta sem personagem.
    [Fact]
    public async Task Heartbeat_WithoutCharacter_ReportsNoCharacter()
    {
        await using var world = await NewWorld(withCharacter: false);

        var result = await world.Get<HeartbeatHandler>().Handle(new HeartbeatCommand());

        Assert.Equal("NoCharacter", result.Status);
    }

    // Revisão do PV-HUNT-011 (ressalva 5c): retorno vencido com Hunt em andamento é consumido.
    [Fact]
    public async Task ReturnDueWhileAHuntRuns_IsConsumed_NoExtraHuntLater()
    {
        await using var world = await NewWorld();
        var presence = world.Get<IPresenceTracker>();
        var heartbeat = world.Get<HeartbeatHandler>();

        await heartbeat.Handle(new HeartbeatCommand());
        world.Clock.Advance(TimeSpan.FromMinutes(2));
        presence.CollectDropped(world.Clock.GetUtcNow(), TimeSpan.FromMinutes(1)); // queda sem Hunt
        await heartbeat.Handle(new HeartbeatCommand());                            // volta

        var manual = await world.Get<ExecuteHuntHandler>().Handle(new ExecuteHuntCommand(world.CharacterId, world.RuntimeId));
        world.Clock.Advance(TimeSpan.FromSeconds(11));
        Assert.False((await heartbeat.Handle(new HeartbeatCommand())).ReturnedToLastHunt); // consome

        await world.Get<LeaveHuntHandler>().Handle(new LeaveHuntCommand(manual.HuntId));
        world.Clock.Advance(TimeSpan.FromSeconds(5));
        var after = await heartbeat.Handle(new HeartbeatCommand());
        Assert.False(after.ReturnedToLastHunt);
        Assert.Null(after.HuntId);
    }
}
