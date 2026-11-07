using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectVictory.Application.Character;
using ProjectVictory.Application.Events;
using ProjectVictory.Application.Game;
using ProjectVictory.Application.GameSession;
using ProjectVictory.Application.Hunt;
using ProjectVictory.Application.VerticalSlice;
using ProjectVictory.Domain.Character;
using ProjectVictory.Domain.Events;
using ProjectVictory.Domain.Hunt;
using ProjectVictory.Persistence.Database;
using Xunit;

namespace ProjectVictory.SmokeTests;

// PV-EVENT-012 (Fila 8; ADR-058): notificação com assinante isolado (R-060, HuntEndedEvent depois do
// commit); um escopo de DI por queda (R-060). E as ressalvas 1 e 3 da revisão do PV-RUNTIME-013.
public sealed class EventIsolationAndDropScopeTests
{
    private sealed record PingEvent : EngineEvent;

    private sealed class Failing<TEvent> : IAsyncEventHandler<TEvent>
        where TEvent : EngineEvent
    {
        public Task HandleAsync(TEvent domainEvent, CancellationToken cancellationToken = default) =>
            throw new InvalidOperationException("subscriber failed");
    }

    private sealed class Counting<TEvent> : IAsyncEventHandler<TEvent>
        where TEvent : EngineEvent
    {
        public int Count;

        public Task HandleAsync(TEvent domainEvent, CancellationToken cancellationToken = default)
        {
            Interlocked.Increment(ref Count);
            return Task.CompletedTask;
        }
    }

    [Fact]
    public async Task Notification_IsolatesAFailingSubscriber_OthersStillRun_FailureReported()
    {
        var counting = new Counting<PingEvent>();
        var sink = new RecordingEventFailureSink();
        var services = new ServiceCollection();
        services.AddSingleton<IAsyncEventHandler<PingEvent>>(new Failing<PingEvent>());
        services.AddSingleton<IAsyncEventHandler<PingEvent>>(counting);
        services.AddSingleton<IEventFailureSink>(sink);
        await using var provider = services.BuildServiceProvider();
        var bus = new InProcessEventBus(provider);

        await bus.PublishNotificationAsync(new PingEvent()); // não lança

        Assert.Equal(1, counting.Count);
        var failure = Assert.Single(sink.Failures);
        Assert.Equal(typeof(Failing<PingEvent>), failure.Subscriber);

        // PublishAsync (fluxo com efeito obrigatório, ex.: Reward) continua propagando a falha.
        await Assert.ThrowsAsync<InvalidOperationException>(() => bus.PublishAsync(new PingEvent()));
    }

    [Fact]
    public async Task Notification_WithoutASink_PropagatesLikePublishAsync()
    {
        var services = new ServiceCollection();
        services.AddSingleton<IAsyncEventHandler<PingEvent>>(new Failing<PingEvent>());
        await using var provider = services.BuildServiceProvider();

        await Assert.ThrowsAsync<InvalidOperationException>(() => new InProcessEventBus(provider).PublishNotificationAsync(new PingEvent()));
    }

    // Composição oficial: um assinante do HuntEndedEvent que falha NÃO quebra a saída manual (já gravada).
    [Fact]
    public async Task HuntEndedSubscriberFailure_DoesNotBreakTheEnding()
    {
        var services = new ServiceCollection();
        services.AddProjectVictorySmokeTestServices();
        var databaseName = $"notify-{Guid.NewGuid():N}";
        services.AddDbContext<ProjectVictoryDbContext>(o => o.UseInMemoryDatabase(databaseName));
        services.AddScoped<IAsyncEventHandler<HuntEndedEvent>, Failing<HuntEndedEvent>>();
        await using var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();
        scope.ServiceProvider.GetRequiredService<TestCurrentPlayerContext>().SetAccount(Guid.NewGuid());
        var characterId = await scope.ServiceProvider.GetRequiredService<CreateCharacterHandler>()
            .Handle(new CreateCharacterCommand(TestNames.Unique("N"), CharacterClass.Knight));
        var runtimeId = (await scope.ServiceProvider.GetRequiredService<EnterGameHandler>().Handle(new EnterGameCommand())).RuntimeId;
        var hunt = await scope.ServiceProvider.GetRequiredService<ExecuteHuntHandler>().Handle(new ExecuteHuntCommand(characterId, runtimeId));

        var ending = await scope.ServiceProvider.GetRequiredService<LeaveHuntHandler>().Handle(new LeaveHuntCommand(hunt.HuntId));

        Assert.Equal(HuntEndReason.Manual, ending.Reason);
        var recorded = scope.ServiceProvider.GetRequiredService<ProjectVictory.Composition.LoggingEventFailureSink>().Recorded;
        Assert.IsType<HuntEndedEvent>(Assert.Single(recorded.Failures).Event);
    }

    private sealed class DbContextProbe : IDisconnectHandler
    {
        public static readonly List<ProjectVictoryDbContext> Seen = [];

        private readonly ProjectVictoryDbContext _context;

        public DbContextProbe(ProjectVictoryDbContext context)
        {
            _context = context;
        }

        public Task HandleAsync(PresenceSnapshot dropped, CancellationToken cancellationToken = default)
        {
            lock (Seen)
            {
                Seen.Add(_context);
            }

            return Task.CompletedTask;
        }
    }

    // R-060: duas quedas na mesma varredura → dois escopos (DbContexts diferentes), nenhum igual ao do chamador.
    [Fact]
    public async Task EachDrop_IsHandledInItsOwnScope()
    {
        var clock = new ManualTimeProvider();
        var services = new ServiceCollection();
        services.AddProjectVictorySmokeTestServices();
        var databaseName = $"scope-{Guid.NewGuid():N}";
        services.AddDbContext<ProjectVictoryDbContext>(o => o.UseInMemoryDatabase(databaseName));
        services.AddSingleton<TimeProvider>(clock);
        services.AddScoped<IDisconnectHandler, DbContextProbe>();
        await using var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();
        var tracker = scope.ServiceProvider.GetRequiredService<IPresenceTracker>();
        tracker.Touch(Guid.NewGuid(), Guid.NewGuid(), clock.GetUtcNow());
        tracker.Touch(Guid.NewGuid(), Guid.NewGuid(), clock.GetUtcNow());
        lock (DbContextProbe.Seen)
        {
            DbContextProbe.Seen.Clear();
        }

        clock.Advance(TimeSpan.FromMinutes(2));
        var handled = await scope.ServiceProvider.GetRequiredService<GameSessionCoordinator>().SweepAsync();

        Assert.Equal(2, handled.Count);
        var callerContext = scope.ServiceProvider.GetRequiredService<ProjectVictoryDbContext>();
        Assert.Equal(2, DbContextProbe.Seen.Distinct().Count());
        Assert.DoesNotContain(callerContext, DbContextProbe.Seen);
    }

    // Ressalva 1 do PV-RUNTIME-013: deslogado não ganha presença por Touch; presença que sobrou não vira queda.
    [Fact]
    public void LoggedOutCharacter_TouchIgnored_LeftoverPresenceIsNotADrop()
    {
        var tracker = new PresenceTracker();
        var characterId = Guid.NewGuid();
        var now = DateTimeOffset.UtcNow;

        tracker.Touch(Guid.NewGuid(), characterId, now);   // presença de antes (corrida com o logout)
        tracker.MarkLoggedOut(characterId);
        tracker.Touch(Guid.NewGuid(), characterId, now);   // ignorado

        Assert.Empty(tracker.CollectDropped(now.AddMinutes(5), TimeSpan.FromMinutes(1)));
        Assert.Null(tracker.Get(characterId));
    }

    // Ressalva 3 do PV-RUNTIME-013: marca só no banco (sem memória) é limpa pelo "entrar".
    [Fact]
    public async Task Enter_ClearsAMarkThatOnlyTheCheckpointHas()
    {
        var services = new ServiceCollection();
        services.AddProjectVictorySmokeTestServices();
        var databaseName = $"mark-db-{Guid.NewGuid():N}";
        services.AddDbContext<ProjectVictoryDbContext>(o => o.UseInMemoryDatabase(databaseName));
        await using var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();
        scope.ServiceProvider.GetRequiredService<TestCurrentPlayerContext>().SetAccount(Guid.NewGuid());
        var characterId = await scope.ServiceProvider.GetRequiredService<CreateCharacterHandler>()
            .Handle(new CreateCharacterCommand(TestNames.Unique("K"), CharacterClass.Knight));
        var runtimeId = (await scope.ServiceProvider.GetRequiredService<EnterGameHandler>().Handle(new EnterGameCommand())).RuntimeId;
        var runtimes = scope.ServiceProvider.GetRequiredService<IRuntimeRepository>();
        await runtimes.SaveLoggedOutAsync(runtimeId, true);

        await scope.ServiceProvider.GetRequiredService<EnterGameHandler>().Handle(new EnterGameCommand());

        Assert.False((await runtimes.GetByCharacterIdAsync(characterId))!.LoggedOut);
    }
}
