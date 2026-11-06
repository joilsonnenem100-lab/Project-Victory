using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectVictory.Application.Character;
using ProjectVictory.Application.Game;
using ProjectVictory.Application.GameSession;
using ProjectVictory.Application.Hunt;
using ProjectVictory.Application.VerticalSlice;
using ProjectVictory.Domain.Character;
using ProjectVictory.Domain.Hunt;
using ProjectVictory.Persistence.Database;
using Xunit;

namespace ProjectVictory.SmokeTests;

// Relógio manual para os testes de sessão (o servidor usa TimeProvider.System).
public sealed class ManualTimeProvider : TimeProvider
{
    private DateTimeOffset _now = new(2026, 11, 6, 12, 0, 0, TimeSpan.Zero);

    public override DateTimeOffset GetUtcNow() => _now;

    public void Advance(TimeSpan by) => _now += by;
}

// PV-RUNTIME-010 (ADR-057 v1.2; DEC-20261106-20): presença em memória, detecção de queda (> 1 min),
// heartbeat pelo token e retorno à última Hunt escolhida com NOVA Hunt depois de > 10 s online.
public sealed class GameSessionTests
{
    private static readonly TimeSpan Minute = TimeSpan.FromMinutes(1);

    [Fact]
    public void Presence_DropsOnlyAfterMoreThanTheTimeout_AndOnlyOnce()
    {
        var tracker = new PresenceTracker();
        var t0 = new DateTimeOffset(2026, 11, 6, 12, 0, 0, TimeSpan.Zero);
        var characterId = Guid.NewGuid();
        tracker.Touch(Guid.NewGuid(), characterId, t0);

        Assert.Empty(tracker.CollectDropped(t0 + Minute, Minute));                 // exatamente 1 min: segue
        var dropped = Assert.Single(tracker.CollectDropped(t0 + Minute + TimeSpan.FromSeconds(1), Minute));
        Assert.Equal(characterId, dropped.CharacterId);
        Assert.Empty(tracker.CollectDropped(t0 + TimeSpan.FromMinutes(5), Minute)); // a mesma queda não volta
    }

    [Fact]
    public void Presence_ReturnIsDue_OnlyAfterMoreThanTheDelay_FollowingADrop()
    {
        var tracker = new PresenceTracker();
        var t0 = new DateTimeOffset(2026, 11, 6, 12, 0, 0, TimeSpan.Zero);
        var characterId = Guid.NewGuid();
        var accountId = Guid.NewGuid();
        var delay = TimeSpan.FromSeconds(10);

        tracker.Touch(accountId, characterId, t0);
        Assert.False(tracker.IsReturnDue(characterId, t0 + TimeSpan.FromMinutes(1), delay)); // sem queda, sem retorno

        tracker.CollectDropped(t0 + TimeSpan.FromMinutes(2), Minute);
        var back = t0 + TimeSpan.FromMinutes(3);
        tracker.Touch(accountId, characterId, back);

        Assert.False(tracker.IsReturnDue(characterId, back + delay, delay));                  // exatamente 10 s
        Assert.True(tracker.IsReturnDue(characterId, back + delay + TimeSpan.FromSeconds(1), delay));

        tracker.ClearPendingReturn(characterId);
        Assert.False(tracker.IsReturnDue(characterId, back + TimeSpan.FromMinutes(1), delay));
    }

    private sealed class RecordingDisconnectHandler : IDisconnectHandler
    {
        public List<Guid> Characters { get; } = [];

        public Task HandleAsync(PresenceSnapshot dropped, CancellationToken cancellationToken = default)
        {
            Characters.Add(dropped.CharacterId);
            return Task.CompletedTask;
        }
    }

    [Fact]
    public async Task Coordinator_SweepDeliversEachDropToTheHandlers()
    {
        var clock = new ManualTimeProvider();
        var tracker = new PresenceTracker();
        var handler = new RecordingDisconnectHandler();
        var coordinator = new GameSessionCoordinator(tracker, GameSessionSettings.Default, clock, [handler]);
        var characterId = Guid.NewGuid();

        tracker.Touch(Guid.NewGuid(), characterId, clock.GetUtcNow());
        clock.Advance(TimeSpan.FromSeconds(30));
        Assert.Empty(await coordinator.SweepAsync());

        clock.Advance(TimeSpan.FromSeconds(31));
        Assert.Single(await coordinator.SweepAsync());
        Assert.Equal([characterId], handler.Characters);
    }

    [Fact]
    public async Task Heartbeat_AfterADropAndMoreThan10sOnline_StartsANewHuntInTheLastHunt()
    {
        var clock = new ManualTimeProvider();
        var services = new ServiceCollection();
        services.AddProjectVictorySmokeTestServices();
        var databaseName = $"session-{Guid.NewGuid():N}";
        services.AddDbContext<ProjectVictoryDbContext>(o => o.UseInMemoryDatabase(databaseName));
        services.AddSingleton<TimeProvider>(clock);
        await using var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();
        scope.ServiceProvider.GetRequiredService<TestCurrentPlayerContext>().SetAccount(Guid.NewGuid());

        var characterId = await scope.ServiceProvider.GetRequiredService<CreateCharacterHandler>()
            .Handle(new CreateCharacterCommand("H" + Guid.NewGuid().ToString("N")[..10], CharacterClass.Knight));
        var enter = await scope.ServiceProvider.GetRequiredService<EnterGameHandler>().Handle(new EnterGameCommand());
        var first = await scope.ServiceProvider.GetRequiredService<ExecuteHuntHandler>()
            .Handle(new ExecuteHuntCommand(characterId, enter.RuntimeId));

        // A última Hunt escolhida foi para o checkpoint do Runtime.
        var runtime = await scope.ServiceProvider.GetRequiredService<IRuntimeRepository>().GetByCharacterIdAsync(characterId);
        Assert.Equal("Spider Cave", runtime!.LastHuntArea);

        // A Hunt termina (aqui, à mão — o encerramento por queda é o PV-HUNT-012) e o jogador cai.
        await scope.ServiceProvider.GetRequiredService<EndHuntHandler>().Handle(new EndHuntCommand(first.HuntId, HuntEndReason.Disconnect));
        clock.Advance(TimeSpan.FromSeconds(61));
        Assert.Single(await scope.ServiceProvider.GetRequiredService<GameSessionCoordinator>().SweepAsync());

        // Volta: primeiro heartbeat → tela inicial (sem Hunt).
        var heartbeat = scope.ServiceProvider.GetRequiredService<HeartbeatHandler>();
        var back = await heartbeat.Handle(new HeartbeatCommand());
        Assert.False(back.ReturnedToLastHunt);
        Assert.Null(back.HuntId);

        // Exatamente 10 s: ainda não.
        clock.Advance(TimeSpan.FromSeconds(10));
        Assert.False((await heartbeat.Handle(new HeartbeatCommand())).ReturnedToLastHunt);

        // Mais que 10 s online: NOVA Hunt na última Hunt escolhida.
        clock.Advance(TimeSpan.FromSeconds(1));
        var returned = await heartbeat.Handle(new HeartbeatCommand());
        Assert.True(returned.ReturnedToLastHunt);
        Assert.Equal("Spider Cave", returned.LastHuntArea);
        Assert.NotNull(returned.HuntId);
        Assert.NotEqual(first.HuntId, returned.HuntId);
        Assert.NotNull(returned.Encounter);

        // Os próximos heartbeats só informam a Hunt em andamento (não criam outra).
        clock.Advance(TimeSpan.FromSeconds(5));
        var next = await heartbeat.Handle(new HeartbeatCommand());
        Assert.False(next.ReturnedToLastHunt);
        Assert.Equal(returned.HuntId, next.HuntId);
    }

    // Revisão do PV-RUNTIME-010 (ressalva 1): dois consumos concorrentes → só um vence.
    [Fact]
    public void TryConsumeReturn_IsAtomic()
    {
        var tracker = new PresenceTracker();
        var t0 = new DateTimeOffset(2026, 11, 6, 12, 0, 0, TimeSpan.Zero);
        var characterId = Guid.NewGuid();
        var accountId = Guid.NewGuid();
        tracker.Touch(accountId, characterId, t0);
        tracker.CollectDropped(t0 + TimeSpan.FromMinutes(2), Minute);
        tracker.Touch(accountId, characterId, t0 + TimeSpan.FromMinutes(3));
        var later = t0 + TimeSpan.FromMinutes(4);

        var wins = Enumerable.Range(0, 16).AsParallel()
            .Select(_ => tracker.TryConsumeReturn(characterId, later, TimeSpan.FromSeconds(10)))
            .Count(won => won);

        Assert.Equal(1, wins);
    }
}
