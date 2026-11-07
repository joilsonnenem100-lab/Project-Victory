using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectVictory.Application.Character;
using ProjectVictory.Application.Game;
using ProjectVictory.Application.GameSession;
using ProjectVictory.Application.Hunt;
using ProjectVictory.Application.Runtime;
using ProjectVictory.Application.VerticalSlice;
using ProjectVictory.Domain.Character;
using ProjectVictory.Domain.Hunt;
using ProjectVictory.Persistence.Database;
using ProjectVictory.Persistence.Hunt;
using Xunit;

namespace ProjectVictory.SmokeTests;

// PV-RUNTIME-012 (Fila 8): reinício = queda (DEC-20261106-47); vida 0 no checkpoint → vida e Mana
// cheias (DEC-20261106-48, PROVISÓRIO); o EnterGame não desfaz o Recovery (R-055); Recovery em lote e
// ordem determinística (R-056). E a ressalva 1 da revisão do PV-HUNT-013 (colisão do retorno com um
// início manual).
public sealed class RestartAsDropTests
{
    private sealed class Server : IAsyncDisposable
    {
        public ServiceProvider Provider { get; }
        public IServiceScope Scope { get; }
        public ManualTimeProvider Clock { get; }

        public Server(string databaseName, ManualTimeProvider clock, Action<IServiceCollection>? extra = null)
        {
            Clock = clock;
            var services = new ServiceCollection();
            services.AddProjectVictorySmokeTestServices();
            services.AddDbContext<ProjectVictoryDbContext>(o => o.UseInMemoryDatabase(databaseName));
            services.AddSingleton<TimeProvider>(clock);
            extra?.Invoke(services);
            Provider = services.BuildServiceProvider();
            Scope = Provider.CreateScope();
        }

        public T Get<T>() where T : notnull => Scope.ServiceProvider.GetRequiredService<T>();

        public async ValueTask DisposeAsync()
        {
            Scope.Dispose();
            await Provider.DisposeAsync();
        }
    }

    private sealed record Seeded(Guid AccountId, Guid CharacterId, Guid RuntimeId, Guid HuntId);

    // "Primeira execução": personagem entra e inicia uma Hunt (grava a última Hunt escolhida).
    private static async Task<Seeded> FirstRun(string databaseName, ManualTimeProvider clock, bool startHunt = true)
    {
        await using var server = new Server(databaseName, clock);
        var accountId = Guid.NewGuid();
        server.Get<TestCurrentPlayerContext>().SetAccount(accountId);
        var characterId = await server.Get<CreateCharacterHandler>()
            .Handle(new CreateCharacterCommand(TestNames.Unique("R"), CharacterClass.Knight));
        var runtimeId = (await server.Get<EnterGameHandler>().Handle(new EnterGameCommand())).RuntimeId;
        var huntId = startHunt
            ? (await server.Get<ExecuteHuntHandler>().Handle(new ExecuteHuntCommand(characterId, runtimeId))).HuntId
            : Guid.Empty;
        return new Seeded(accountId, characterId, runtimeId, huntId);
    }

    [Fact]
    public async Task Restart_SchedulesTheReturn_NewHuntAfterMoreThan10sOnline()
    {
        var databaseName = $"restart-{Guid.NewGuid():N}";
        var clock = new ManualTimeProvider();
        var seeded = await FirstRun(databaseName, clock);

        await using var server = new Server(databaseName, clock);
        var report = await server.Get<RuntimeRecoveryService>().RecoverAsync();

        Assert.Equal(1, report.ReturnsScheduled);
        Assert.True(server.Get<IPresenceTracker>().Get(seeded.CharacterId)!.Dropped);

        server.Get<TestCurrentPlayerContext>().SetAccount(seeded.AccountId);
        var heartbeat = server.Get<HeartbeatHandler>();
        var first = await heartbeat.Handle(new HeartbeatCommand()); // volta: tela inicial
        Assert.False(first.ReturnedToLastHunt);
        Assert.Null(first.HuntId);

        clock.Advance(TimeSpan.FromSeconds(10)); // exatamente 10 s: ainda não
        Assert.False((await heartbeat.Handle(new HeartbeatCommand())).ReturnedToLastHunt);
        clock.Advance(TimeSpan.FromSeconds(1));
        var returned = await heartbeat.Handle(new HeartbeatCommand());

        Assert.True(returned.ReturnedToLastHunt);
        Assert.NotEqual(seeded.HuntId, returned.HuntId); // NOVA Hunt Session (ADR-057)
        Assert.Equal(returned.HuntId, (await server.Get<IHuntRepository>().GetRunningByCharacterIdAsync(seeded.CharacterId))!.Id);
    }

    [Fact]
    public async Task Restart_WithoutALastHunt_SchedulesNothing()
    {
        var databaseName = $"restart-none-{Guid.NewGuid():N}";
        var clock = new ManualTimeProvider();
        var seeded = await FirstRun(databaseName, clock, startHunt: false);

        await using var server = new Server(databaseName, clock);
        var report = await server.Get<RuntimeRecoveryService>().RecoverAsync();

        Assert.Equal(0, report.ReturnsScheduled);
        Assert.Null(server.Get<IPresenceTracker>().Get(seeded.CharacterId));
    }

    [Fact]
    public async Task HealthZeroInTheCheckpoint_ComesBackWithFullHealthAndMana()
    {
        var databaseName = $"restart-dead-{Guid.NewGuid():N}";
        var clock = new ManualTimeProvider();
        var seeded = await FirstRun(databaseName, clock);

        await using (var before = new Server(databaseName, clock))
        {
            await before.Get<IRuntimeRepository>().SaveCheckpointAsync(seeded.RuntimeId, 0, 1);
        }

        await using var server = new Server(databaseName, clock);
        await server.Get<RuntimeRecoveryService>().RecoverAsync();

        Assert.True(server.Get<IRuntimeExecutionStateStore>().TryGetByCharacterId(seeded.CharacterId, out var runtime));
        var character = await server.Get<global::ICharacterRepository>().GetByIdAsync(seeded.CharacterId);
        Assert.Equal(character!.MaximumHealth.Value, runtime.CharacterState!.CurrentHealth);
        Assert.Equal(character.MaximumMana.Value, runtime.CharacterState.CurrentMana);
        Assert.Equal(ProjectVictory.Application.World.HomeCity.Location, runtime.WorldContext);
    }

    // R-055: reinício → enter mantém o que o Recovery reconstruiu ("Victoria", limites).
    [Fact]
    public async Task EnterAfterRestart_KeepsTheRecoveredRuntime_AtHomeCity()
    {
        var databaseName = $"restart-enter-{Guid.NewGuid():N}";
        var clock = new ManualTimeProvider();
        var seeded = await FirstRun(databaseName, clock);

        await using (var before = new Server(databaseName, clock))
        {
            await before.Get<IRuntimeRepository>().SaveCheckpointAsync(seeded.RuntimeId, 99_999, 99_999);
        }

        await using var server = new Server(databaseName, clock);
        await server.Get<RuntimeRecoveryService>().RecoverAsync();
        server.Get<TestCurrentPlayerContext>().SetAccount(seeded.AccountId);

        var enter = await server.Get<EnterGameHandler>().Handle(new EnterGameCommand());

        Assert.Equal(seeded.RuntimeId, enter.RuntimeId);
        var runtime = server.Get<IRuntimeExecutionStateStore>().Get(seeded.RuntimeId);
        var character = await server.Get<global::ICharacterRepository>().GetByIdAsync(seeded.CharacterId);
        Assert.Equal(ProjectVictory.Application.World.HomeCity.Location, runtime.WorldContext);
        Assert.Equal(character!.MaximumHealth.Value, runtime.CharacterState!.CurrentHealth);
        Assert.Equal(character.MaximumMana.Value, runtime.CharacterState.CurrentMana);
    }

    // R-055: sem Recovery (store vazio), o enter usa a MESMA reconstrução (não mais WorldContext vazio).
    [Fact]
    public async Task EnterWithoutRecovery_UsesTheSameReconstruction()
    {
        var databaseName = $"restart-enter-raw-{Guid.NewGuid():N}";
        var clock = new ManualTimeProvider();
        var seeded = await FirstRun(databaseName, clock);

        await using var server = new Server(databaseName, clock);
        server.Get<TestCurrentPlayerContext>().SetAccount(seeded.AccountId);
        await server.Get<EnterGameHandler>().Handle(new EnterGameCommand());

        var runtime = server.Get<IRuntimeExecutionStateStore>().Get(seeded.RuntimeId);
        Assert.Equal(ProjectVictory.Application.World.HomeCity.Location, runtime.WorldContext);
    }

    // R-056: dois Runtimes do mesmo personagem → o Recovery e o EnterGame escolhem o MESMO (menor RuntimeId).
    [Fact]
    public async Task DuplicateRuntimes_RecoveryAndEnterPickTheSameOne()
    {
        var databaseName = $"restart-dup-{Guid.NewGuid():N}";
        var clock = new ManualTimeProvider();
        var seeded = await FirstRun(databaseName, clock, startHunt: false);
        var extra = Guid.NewGuid();

        await using (var before = new Server(databaseName, clock))
        {
            await before.Get<IRuntimeRepository>().AddAsync(extra, seeded.CharacterId, 10, 10);
        }

        var expected = new[] { seeded.RuntimeId, extra }.Min();

        await using var server = new Server(databaseName, clock);
        var report = await server.Get<RuntimeRecoveryService>().RecoverAsync();

        Assert.Equal(1, report.Recovered);
        Assert.Equal(1, report.SkippedDuplicates);
        Assert.True(server.Get<IRuntimeExecutionStateStore>().TryGetByCharacterId(seeded.CharacterId, out var recovered));
        Assert.Equal(expected, recovered.RuntimeId);

        server.Get<TestCurrentPlayerContext>().SetAccount(seeded.AccountId);
        Assert.Equal(expected, (await server.Get<EnterGameHandler>().Handle(new EnterGameCommand())).RuntimeId);
    }

    // R-056: o Recovery lê os personagens em LOTE (nenhuma leitura um a um).
    [Fact]
    public async Task Recovery_ReadsCharactersInBatch()
    {
        var databaseName = $"restart-batch-{Guid.NewGuid():N}";
        var clock = new ManualTimeProvider();
        for (var i = 0; i < 3; i++)
        {
            await FirstRun(databaseName, clock, startHunt: false);
        }

        var counter = new CallCounter();
        await using var server = new Server(databaseName, clock, services =>
            services.AddScoped<global::ICharacterRepository>(provider =>
                new CountingRepository(
                    ActivatorUtilities.CreateInstance<ProjectVictory.Persistence.Character.CharacterRepository>(provider),
                    counter)));

        var report = await server.Get<RuntimeRecoveryService>().RecoverAsync();

        Assert.Equal(3, report.Recovered);
        Assert.Equal(0, counter.SingleReads);
        Assert.Equal(1, counter.BatchReads);
    }

    // Ressalva 1 da revisão do PV-HUNT-013: o retorno vence, mas um início manual entra entre a conferência
    // do heartbeat e a criação → o heartbeat NÃO falha (sem 409): consome o retorno e informa a Hunt em
    // andamento, que continua.
    [Fact]
    public async Task ReturnCollidingWithAManualStart_IsConsumed_WithoutError()
    {
        var databaseName = $"restart-race-{Guid.NewGuid():N}";
        var clock = new ManualTimeProvider();
        var seeded = await FirstRun(databaseName, clock);
        var hunts = new BlindOnceHuntRepository(new HuntRepository());

        await using var server = new Server(databaseName, clock, services =>
            services.AddSingleton<IHuntRepository>(hunts));
        await server.Get<RuntimeRecoveryService>().RecoverAsync();
        server.Get<TestCurrentPlayerContext>().SetAccount(seeded.AccountId);
        var heartbeat = server.Get<HeartbeatHandler>();
        await heartbeat.Handle(new HeartbeatCommand());
        clock.Advance(TimeSpan.FromSeconds(11));

        // O jogador inicia uma Hunt; o próximo heartbeat "não vê" essa Hunt na conferência (corrida).
        var manual = await server.Get<ExecuteHuntHandler>().Handle(new ExecuteHuntCommand(seeded.CharacterId, seeded.RuntimeId));
        hunts.BlindNextRunningLookup();

        var result = await heartbeat.Handle(new HeartbeatCommand());

        Assert.False(result.ReturnedToLastHunt);
        Assert.Equal(manual.HuntId, result.HuntId);
        Assert.Equal(manual.HuntId, (await hunts.GetRunningByCharacterIdAsync(seeded.CharacterId))!.Id);
        clock.Advance(TimeSpan.FromSeconds(30));
        Assert.False((await heartbeat.Handle(new HeartbeatCommand())).ReturnedToLastHunt); // retorno consumido
    }

    private sealed class CallCounter
    {
        public int SingleReads;
        public int BatchReads;
    }

    private sealed class CountingRepository(global::ICharacterRepository inner, CallCounter counter) : global::ICharacterRepository
    {
        public Task AddAsync(ProjectVictory.Domain.Entities.Character character, CancellationToken cancellationToken = default) => inner.AddAsync(character, cancellationToken);

        public Task<ProjectVictory.Domain.Entities.Character?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            Interlocked.Increment(ref counter.SingleReads);
            return inner.GetByIdAsync(id, cancellationToken);
        }

        public Task<IReadOnlyList<ProjectVictory.Domain.Entities.Character>> GetByIdsAsync(IReadOnlyCollection<Guid> ids, CancellationToken cancellationToken = default)
        {
            Interlocked.Increment(ref counter.BatchReads);
            return inner.GetByIdsAsync(ids, cancellationToken);
        }

        public Task<ProjectVictory.Domain.Entities.Character?> GetByAccountIdAsync(Guid accountId, CancellationToken cancellationToken = default) => inner.GetByAccountIdAsync(accountId, cancellationToken);

        public Task<IReadOnlyList<ProjectVictory.Domain.Entities.Character>> GetAllByAccountIdAsync(Guid accountId, CancellationToken cancellationToken = default) => inner.GetAllByAccountIdAsync(accountId, cancellationToken);

        public Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default) => inner.ExistsByNameAsync(name, cancellationToken);

        public Task UpdateAsync(ProjectVictory.Domain.Entities.Character character, CancellationToken cancellationToken = default) => inner.UpdateAsync(character, cancellationToken);
    }

    private sealed class BlindOnceHuntRepository(IHuntRepository inner) : IHuntRepository
    {
        private int _blind;

        public void BlindNextRunningLookup() => Interlocked.Exchange(ref _blind, 1);

        public Task AddAsync(Hunt hunt, CancellationToken cancellationToken = default) => inner.AddAsync(hunt, cancellationToken);

        public Task<Hunt?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) => inner.GetByIdAsync(id, cancellationToken);

        public Task<Hunt?> GetRunningByCharacterIdAsync(Guid characterId, CancellationToken cancellationToken = default) =>
            Interlocked.Exchange(ref _blind, 0) == 1
                ? Task.FromResult<Hunt?>(null)
                : inner.GetRunningByCharacterIdAsync(characterId, cancellationToken);

        public Task RemoveAsync(Guid id, CancellationToken cancellationToken = default) => inner.RemoveAsync(id, cancellationToken);
    }
}
