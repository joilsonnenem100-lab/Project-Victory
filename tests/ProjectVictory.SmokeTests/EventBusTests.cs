using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectVictory.Application.Character;
using ProjectVictory.Application.Events;
using ProjectVictory.Application.Game;
using ProjectVictory.Application.GameSession;
using ProjectVictory.Application.Hunt;
using ProjectVictory.Application.VerticalSlice;
using ProjectVictory.Domain.Character;
using ProjectVictory.Domain.Combat;
using ProjectVictory.Domain.Events;
using ProjectVictory.Persistence.Database;
using Xunit;

namespace ProjectVictory.SmokeTests;

// PV-EVENT-010/011 (ADR-049/050): Event Bus tipado (só Domain Events); CombatCompletedEvent → Reward;
// ciclo de vida do Spawn publicado (fecha o R-046); fim da Hunt publicado.
public sealed class EventBusTests
{
    private sealed record OtherEvent : EngineEvent;

    private sealed class Recorder<TEvent> : IEventHandler<TEvent>
        where TEvent : EngineEvent
    {
        public List<TEvent> Received { get; } = [];

        public void Handle(TEvent domainEvent) => Received.Add(domainEvent);
    }

    private sealed class AsyncRecorder<TEvent> : IAsyncEventHandler<TEvent>
        where TEvent : EngineEvent
    {
        public List<TEvent> Received { get; } = [];

        public Task HandleAsync(TEvent domainEvent, CancellationToken cancellationToken = default)
        {
            Received.Add(domainEvent);
            return Task.CompletedTask;
        }
    }

    [Fact]
    public async Task Dispatch_IsByType_SyncPublishRejectsAsyncHandlers()
    {
        var combat = new Recorder<CombatCompletedEvent>();
        var other = new Recorder<OtherEvent>();
        var asyncOther = new AsyncRecorder<OtherEvent>();
        var services = new ServiceCollection();
        services.AddSingleton<IEventHandler<CombatCompletedEvent>>(combat);
        services.AddSingleton<IEventHandler<OtherEvent>>(other);
        services.AddSingleton<IAsyncEventHandler<OtherEvent>>(asyncOther);
        await using var provider = services.BuildServiceProvider();
        var bus = new InProcessEventBus(provider);

        var completed = new CombatCompletedEvent(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), CombatOutcome.Victory);
        bus.Publish(completed);
        await bus.PublishAsync(new OtherEvent());

        Assert.Equal([completed], combat.Received);   // só o handler do tipo recebe
        Assert.Single(other.Received);
        Assert.Single(asyncOther.Received);
        Assert.Throws<InvalidOperationException>(() => bus.Publish(new OtherEvent())); // há handler assíncrono
    }

    private static async Task<(ServiceProvider Provider, IServiceScope Scope, Guid CharacterId, Guid RuntimeId)> World(
        Action<IServiceCollection>? extra = null)
    {
        var services = new ServiceCollection();
        services.AddProjectVictorySmokeTestServices();
        var databaseName = $"events-{Guid.NewGuid():N}";
        services.AddDbContext<ProjectVictoryDbContext>(o => o.UseInMemoryDatabase(databaseName));
        services.AddSingleton(new ProjectVictory.Application.Combat.TurnBased.CombatSettings(1)); // determinístico
        extra?.Invoke(services);
        var provider = services.BuildServiceProvider();
        var scope = provider.CreateScope();
        scope.ServiceProvider.GetRequiredService<TestCurrentPlayerContext>().SetAccount(Guid.NewGuid());
        var characterId = await scope.ServiceProvider.GetRequiredService<CreateCharacterHandler>()
            .Handle(new CreateCharacterCommand("E" + Guid.NewGuid().ToString("N")[..10], CharacterClass.Knight));
        var enter = await scope.ServiceProvider.GetRequiredService<EnterGameHandler>().Handle(new EnterGameCommand());
        return (provider, scope, characterId, enter.RuntimeId);
    }

    [Fact]
    public async Task KillingMonsters_PublishesCombatCompleted_RewardReactsToTheEvent_SpawnAndHuntEndPublished()
    {
        var combatEvents = new AsyncRecorder<CombatCompletedEvent>();
        var huntEnded = new AsyncRecorder<HuntEndedEvent>();
        var (provider, scope, characterId, runtimeId) = await World(services =>
        {
            services.AddSingleton<IAsyncEventHandler<CombatCompletedEvent>>(combatEvents); // além do Reward
            services.AddSingleton<IAsyncEventHandler<HuntEndedEvent>>(huntEnded);
        });

        await using (provider)
        using (scope)
        {
            var counter = scope.ServiceProvider.GetRequiredService<ProjectVictory.Application.Spawn.SpawnLifecycleCounter>();
            var hunt = await scope.ServiceProvider.GetRequiredService<ExecuteHuntHandler>().Handle(new ExecuteHuntCommand(characterId, runtimeId));
            
            AttackEncounterResult? last = null;
            for (var i = 0; i < 300 && last?.CombatFinished != true; i++)
            {
                last = await scope.ServiceProvider.GetRequiredService<AttackEncounterHandler>().Handle(new AttackEncounterCommand(hunt.Encounter.Id));
            }

            Assert.Equal("Victory", last!.CombatState); // semente 1: vitória no 1º encounter

            // Um CombatCompletedEvent por monstro morto; o Reward reagiu (XP no personagem = XP da Hunt).
            Assert.True(combatEvents.Received.Count > 0, $"sem CombatCompletedEvent: state={last!.CombatState} reason={last.HuntEndReason} xp={last.HuntExperienceGained} playerDefeated={last.PlayerDefeated}");
            Assert.All(combatEvents.Received, e => Assert.Equal(CombatOutcome.Victory, e.Outcome));
            var character = await scope.ServiceProvider.GetRequiredService<ICharacterRepository>().GetByIdAsync(characterId);
            Assert.Equal(last!.HuntExperienceGained, character!.Experience.Value);
            Assert.True(character.Experience.Value > 0);

            await scope.ServiceProvider.GetRequiredService<LeaveHuntHandler>().Handle(new LeaveHuntCommand(hunt.HuntId));

            // Ciclo de vida do Spawn publicado: tudo o que foi ativado foi despawnado no fim da Hunt.
            Assert.True(counter.Activated > 0);
            Assert.Equal(counter.Activated, counter.Despawned);

            var ended = Assert.Single(huntEnded.Received);
            Assert.Equal("Manual", ended.Reason);
            Assert.Equal(hunt.HuntId, ended.HuntId);
        }
    }

    // Revisão do PV-HUNT-012 (ressalva 2): uma queda cujo tratamento falha volta a ser detectada.
    private sealed class FailingOnce : IDisconnectHandler
    {
        public int Calls;

        public Task HandleAsync(PresenceSnapshot dropped, CancellationToken cancellationToken = default) =>
            Interlocked.Increment(ref Calls) == 1 ? throw new InvalidOperationException("boom") : Task.CompletedTask;
    }

    [Fact]
    public async Task Sweep_FailureOnOneDrop_IsRetriedOnTheNextSweep()
    {
        var clock = new ManualTimeProvider();
        var tracker = new PresenceTracker();
        var failing = new FailingOnce();
        var coordinator = new GameSessionCoordinator(tracker, GameSessionSettings.Default, clock, [failing]);
        tracker.Touch(Guid.NewGuid(), Guid.NewGuid(), clock.GetUtcNow());
        clock.Advance(TimeSpan.FromMinutes(2));

        Assert.Empty(await coordinator.SweepAsync());   // falhou: não conta como tratada
        Assert.Equal(1, coordinator.Failures);
        Assert.Single(await coordinator.SweepAsync());  // detectada de novo e tratada
    }

    // Revisão do PV-EVENT-010/011 (ressalva 4): o assinante do Reward é obrigatório hoje — a composição
    // precisa registrá-lo.
    [Fact]
    public void Composition_RegistersTheRewardSubscriber()
    {
        var services = new ServiceCollection();
        services.AddProjectVictorySmokeTestServices();
        using var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();

        Assert.Contains(
            scope.ServiceProvider.GetServices<IAsyncEventHandler<CombatCompletedEvent>>(),
            handler => handler is ProjectVictory.Application.Reward.RewardOnCombatCompleted);
    }
}
