using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectVictory.Application.Character;
using ProjectVictory.Application.Combat.TurnBased;
using ProjectVictory.Application.Events;
using ProjectVictory.Application.Game;
using ProjectVictory.Application.Hunt;
using ProjectVictory.Application.HuntAnalyzer;
using ProjectVictory.Application.Reward;
using ProjectVictory.Composition;
using ProjectVictory.Domain.Combat;
using ProjectVictory.Domain.Hunt;
using ProjectVictory.Persistence.HuntAnalyzer;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ProjectVictory.Domain.Character;
using ProjectVictory.Domain.Events;
using ProjectVictory.Domain.Items;
using ProjectVictory.Domain.Loot;
using ProjectVictory.Persistence.Database;
using Xunit;

namespace ProjectVictory.SmokeTests;

// PV-HUD-ANALYZER-010: Hunt Analyzer pela composição real (DI + InMemory) — apuração durante a Hunt,
// resumo gravado pelo assinante do HuntEndedEvent (pipeline único, ADR-012) e histórico limitado a N.
public sealed class HuntAnalyzerSmokeTests
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

    private static async Task<World> NewWorld(HuntAnalyzerSettings? settings = null, Action<IServiceCollection>? configure = null)
    {
        var clock = new ManualTimeProvider();
        var services = new ServiceCollection();
        services.AddProjectVictorySmokeTestServices();
        var databaseName = $"analyzer-{Guid.NewGuid():N}";
        services.AddDbContext<ProjectVictoryDbContext>(o => o.UseInMemoryDatabase(databaseName));
        services.AddSingleton(new CombatSettings(1));
        services.AddSingleton<TimeProvider>(clock);

        if (settings is not null)
        {
            services.AddSingleton(settings);
        }

        configure?.Invoke(services);

        var provider = services.BuildServiceProvider();
        var scope = provider.CreateScope();
        scope.ServiceProvider.GetRequiredService<ProjectVictoryDbContext>().Database.EnsureCreated(); // preços NPC (seed)
        scope.ServiceProvider.GetRequiredService<TestCurrentPlayerContext>().SetAccount(Guid.NewGuid());

        var characterId = await scope.ServiceProvider.GetRequiredService<CreateCharacterHandler>()
            .Handle(new CreateCharacterCommand(TestNames.Unique("H"), CharacterClass.Knight));
        var enter = await scope.ServiceProvider.GetRequiredService<EnterGameHandler>().Handle(new EnterGameCommand());

        return new World(provider, scope, characterId, enter.RuntimeId, clock);
    }

    [Fact]
    public async Task DuringTheHunt_AnalyzerIsLive_AfterLeaving_TheSummaryIsInTheHistory()
    {
        await using var world = await NewWorld();
        var hunt = await world.Get<ExecuteHuntHandler>().Handle(new ExecuteHuntCommand(world.CharacterId, world.RuntimeId));

        AttackEncounterResult? last = null;
        for (var i = 0; i < 300 && last?.CombatFinished != true; i++)
        {
            last = await world.Get<AttackEncounterHandler>().Handle(new AttackEncounterCommand(hunt.Encounter.Id));
        }

        Assert.Equal("Victory", last!.CombatState);
        world.Clock.Advance(TimeSpan.FromMinutes(30));

        var live = await world.Get<GetHuntAnalyzerHandler>().Handle(new GetHuntAnalyzerQuery(world.CharacterId));
        Assert.True(live.HasActiveHunt);
        Assert.Equal(hunt.HuntId, live.HuntId);
        Assert.Equal(last.HuntExperienceGained, live.Analyzer!.ExperienceGained);
        Assert.True(live.Analyzer.ExperienceGained > 0);
        Assert.Equal(live.Analyzer.ExperienceGained * 3600 / live.Analyzer.ElapsedSeconds, live.Analyzer.ExperiencePerHour);
        Assert.Equal(live.Analyzer.Profit - live.Analyzer.Debit, live.Analyzer.NetProfit);

        await world.Get<LeaveHuntHandler>().Handle(new LeaveHuntCommand(hunt.HuntId));

        var history = await world.Get<GetHuntAnalyzerHistoryHandler>().Handle(new GetHuntAnalyzerHistoryQuery(world.CharacterId));
        var summary = Assert.Single(history);
        Assert.Equal(hunt.HuntId, summary.HuntId);
        Assert.Equal("Manual", summary.EndReason);
        Assert.Equal(live.Analyzer.ExperienceGained, summary.Analyzer.ExperienceGained);
        Assert.Equal(live.Analyzer.Profit, summary.Analyzer.Profit);

        // Sem Hunt em andamento: o Analyzer mostra o resumo da última Hunt.
        var after = await world.Get<GetHuntAnalyzerHandler>().Handle(new GetHuntAnalyzerQuery(world.CharacterId));
        Assert.False(after.HasActiveHunt);
        Assert.Equal(hunt.HuntId, after.LastHunt!.HuntId);
    }

    [Fact]
    public async Task HuntEndedSubscriber_PricesSuppliesAndLoot_AndKeepsOnlyTheLastN()
    {
        await using var world = await NewWorld(new HuntAnalyzerSettings(2));
        var tally = world.Get<IHuntAnalyzerTally>();
        var spiderFang = Guid.Parse("00000000-0000-0000-0000-000000000003"); // venda 8 (seed)
        var hunts = new[] { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };

        for (var i = 0; i < hunts.Length; i++)
        {
            // 2 poções de vida (compra 50) + 1 de mana (compra 50) = 150; 3 Spider Fang (8) + 20 ouro = 44.
            tally.RecordSupplies(hunts[i], world.CharacterId, 2, 1);
            tally.RecordLoot(hunts[i], new LootResolutionResult(
                [new ItemInstance(Guid.NewGuid(), spiderFang), new ItemInstance(Guid.NewGuid(), spiderFang), new ItemInstance(Guid.NewGuid(), spiderFang)],
                20));

            await world.Get<IEventBus>().PublishNotificationAsync(
                new HuntEndedEvent(hunts[i], world.CharacterId, "Manual", 1800, 600)
                {
                    OccurredAt = new DateTimeOffset(2026, 11, 7, 1, i, 0, TimeSpan.Zero)
                });
        }

        var history = await world.Get<GetHuntAnalyzerHistoryHandler>().Handle(new GetHuntAnalyzerHistoryQuery(world.CharacterId));

        Assert.Equal([hunts[2], hunts[1]], history.Select(h => h.HuntId));
        var analyzer = history[0].Analyzer;
        Assert.Equal(150, analyzer.Debit);
        Assert.Equal(24, analyzer.LootValue);
        Assert.Equal(44, analyzer.Profit);
        Assert.Equal(-106, analyzer.NetProfit);
        Assert.Equal(1200, analyzer.ExperiencePerHour);
        Assert.Null(tally.Get(hunts[2])); // apuração descartada no fim da Hunt
    }

    // Ressalva R1: a falha do observador do Reward (ex.: Analyzer) é isolada e registrada; a recompensa
    // (XP da Hunt) continua sendo entregue.
    private sealed class ThrowingObserver : IRewardDeliveryObserver
    {
        public void OnLootDelivered(CombatResult combatResult, LootResolutionResult loot) =>
            throw new InvalidOperationException("observador quebrado (teste)");
    }

    [Fact]
    public async Task RewardObserverFailure_IsIsolatedAndLogged_RewardStillDelivered()
    {
        await using var world = await NewWorld(configure: s => s.AddScoped<IRewardDeliveryObserver, ThrowingObserver>());
        var hunt = await world.Get<ExecuteHuntHandler>().Handle(new ExecuteHuntCommand(world.CharacterId, world.RuntimeId));

        AttackEncounterResult? last = null;
        for (var i = 0; i < 300 && last?.CombatFinished != true; i++)
        {
            last = await world.Get<AttackEncounterHandler>().Handle(new AttackEncounterCommand(hunt.Encounter.Id));
        }

        Assert.Equal("Victory", last!.CombatState);
        Assert.True(last.HuntExperienceGained > 0);
        // Uma falha registrada por entrega de loot (o encounter pode ter mais de um monstro).
        var failures = world.Get<LoggingRewardObserverFailureSink>().Recorded.Failures;
        Assert.NotEmpty(failures);
        Assert.All(failures, failure =>
        {
            Assert.Equal(typeof(ThrowingObserver), failure.Observer);
            Assert.Equal(world.CharacterId, failure.CharacterId);
        });
    }

    // Ressalva R2: se a gravação do resumo falhar, a linha sai do change tracker e não é regravada pelo
    // próximo SaveChanges do mesmo DbContext (o do escopo do EndHunt).
    private sealed class FailFirstSaveInterceptor : SaveChangesInterceptor
    {
        public int Calls { get; private set; }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            if (Calls++ == 0)
            {
                throw new InvalidOperationException("falha de gravação (teste)");
            }

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }

    [Fact]
    public async Task HistoryStore_SaveFailure_DetachesTheRow_SoItIsNotSavedAgain()
    {
        var interceptor = new FailFirstSaveInterceptor();
        var options = new DbContextOptionsBuilder<ProjectVictoryDbContext>()
            .UseInMemoryDatabase($"analyzer-r2-{Guid.NewGuid():N}")
            .AddInterceptors(interceptor)
            .Options;
        await using var context = new ProjectVictoryDbContext(options);
        var store = new HuntAnalyzerHistoryStore(context);
        var characterId = Guid.NewGuid();
        var report = new HuntAnalyzerReport(10, 60, 600, 0, 0, 0, 0, 0, 0, 0);
        var failed = new HuntAnalyzerSummary(Guid.NewGuid(), characterId, "Manual", DateTimeOffset.UnixEpoch.AddMinutes(1), report);
        var saved = new HuntAnalyzerSummary(Guid.NewGuid(), characterId, "Manual", DateTimeOffset.UnixEpoch.AddMinutes(2), report);

        await Assert.ThrowsAsync<InvalidOperationException>(() => store.AddAsync(failed, 10));
        Assert.Empty(context.ChangeTracker.Entries<PersistentHuntAnalyzerSummaryState>());

        await store.AddAsync(saved, 10);

        var history = await store.GetRecentAsync(characterId, 10);
        Assert.Equal([saved.HuntId], history.Select(h => h.HuntId));
    }
}
