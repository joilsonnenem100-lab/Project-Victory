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
using ProjectVictory.Infrastructure.Content;
using ProjectVictory.Persistence.Database;
using Xunit;

namespace ProjectVictory.SmokeTests;

// PV-RUNTIME-013 (Fila 8): R-054 — depois do logout o heartbeat não recria a presença (fechar a página
// não vira queda), a marca sobrevive ao reinício e acaba no próximo "entrar"/início de Hunt; R-051 — a
// área ESCOLHIDA é validada (existência e tier, R-041), gravada e usada no retorno.
public sealed class LogoutMarkAndHuntAreaTests
{
    // Catálogo com áreas extras (só para teste): a padrão continua a Spider Cave.
    private sealed class TwoAreaCatalog(IHuntAreaCatalog inner) : IHuntAreaCatalog
    {
        public HuntAreaDefinition GetDefault() => inner.GetDefault();

        public IReadOnlyList<HuntAreaDefinition> GetAll() =>
        [
            inner.GetDefault(),
            inner.GetDefault() with { Name = "Rat Sewers", RequiredTitleTier = 0 },
            inner.GetDefault() with { Name = "Dragon Lair", RequiredTitleTier = 5 }
        ];
    }

    private sealed class Server : IAsyncDisposable
    {
        public ServiceProvider Provider { get; }
        public IServiceScope Scope { get; }

        public Server(string databaseName, ManualTimeProvider clock, bool twoAreas = false)
        {
            var services = new ServiceCollection();
            services.AddProjectVictorySmokeTestServices();
            services.AddDbContext<ProjectVictoryDbContext>(o => o.UseInMemoryDatabase(databaseName));
            services.AddSingleton<TimeProvider>(clock);

            if (twoAreas)
            {
                services.AddSingleton<IHuntAreaCatalog>(new TwoAreaCatalog(new SpiderCaveHuntArea()));
            }

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

    private static async Task<(Guid AccountId, Guid CharacterId, Guid RuntimeId)> Enter(Server server)
    {
        var accountId = Guid.NewGuid();
        server.Get<TestCurrentPlayerContext>().SetAccount(accountId);
        var characterId = await server.Get<CreateCharacterHandler>()
            .Handle(new CreateCharacterCommand("L" + Guid.NewGuid().ToString("N")[..10], CharacterClass.Knight));
        var runtimeId = (await server.Get<EnterGameHandler>().Handle(new EnterGameCommand())).RuntimeId;
        return (accountId, characterId, runtimeId);
    }

    // R-054: logout → mais um heartbeat → fecha a página. Não vira queda; o próximo login não volta à Hunt.
    [Fact]
    public async Task HeartbeatAfterLogout_IsIgnored_ClosingThePageIsNotADrop_NoReturnOnNextLogin()
    {
        var clock = new ManualTimeProvider();
        await using var server = new Server($"logout-{Guid.NewGuid():N}", clock);
        var (_, characterId, runtimeId) = await Enter(server);
        await server.Get<ExecuteHuntHandler>().Handle(new ExecuteHuntCommand(characterId, runtimeId));
        await server.Get<LogoutHandler>().Handle(new LogoutCommand());

        var late = await server.Get<HeartbeatHandler>().Handle(new HeartbeatCommand());
        Assert.Equal("LoggedOut", late.Status);
        Assert.Null(server.Get<IPresenceTracker>().Get(characterId)); // presença NÃO recriada

        clock.Advance(TimeSpan.FromMinutes(5)); // página fechada
        Assert.Empty(await server.Get<GameSessionCoordinator>().SweepAsync());

        // Próximo login: entra, fica online > 10 s — sem Hunt automática.
        await server.Get<EnterGameHandler>().Handle(new EnterGameCommand());
        Assert.False(server.Get<IPresenceTracker>().IsLoggedOut(characterId));
        var heartbeat = server.Get<HeartbeatHandler>();
        Assert.Equal("Online", (await heartbeat.Handle(new HeartbeatCommand())).Status);
        clock.Advance(TimeSpan.FromSeconds(20));
        var later = await heartbeat.Handle(new HeartbeatCommand());
        Assert.False(later.ReturnedToLastHunt);
        Assert.Null(later.HuntId);
    }

    // R-054: iniciar Hunt depois do logout (sem "entrar") também encerra a marca — a Hunt não morre por
    // falta de presença.
    [Fact]
    public async Task StartingAHuntAfterLogout_ClearsTheMark()
    {
        var clock = new ManualTimeProvider();
        await using var server = new Server($"logout-hunt-{Guid.NewGuid():N}", clock);
        var (_, characterId, runtimeId) = await Enter(server);
        await server.Get<LogoutHandler>().Handle(new LogoutCommand());

        var hunt = await server.Get<ExecuteHuntHandler>().Handle(new ExecuteHuntCommand(characterId, runtimeId));

        Assert.False(server.Get<IPresenceTracker>().IsLoggedOut(characterId));
        Assert.Equal(hunt.HuntId, (await server.Get<HeartbeatHandler>().Handle(new HeartbeatCommand())).HuntId);
        Assert.False((await server.Get<IRuntimeRepository>().GetByCharacterIdAsync(characterId))!.LoggedOut);
    }

    // R-054 × DEC-20261106-47: logout ANTES do reinício → o Recovery não agenda retorno; "entrar" limpa a marca.
    [Fact]
    public async Task LogoutBeforeRestart_NoReturnScheduled_EnterClearsThePersistedMark()
    {
        var databaseName = $"logout-restart-{Guid.NewGuid():N}";
        var clock = new ManualTimeProvider();
        Guid accountId;
        Guid characterId;

        await using (var first = new Server(databaseName, clock))
        {
            Guid runtimeId;
            (accountId, characterId, runtimeId) = await Enter(first);
            await first.Get<ExecuteHuntHandler>().Handle(new ExecuteHuntCommand(characterId, runtimeId));
            await first.Get<LogoutHandler>().Handle(new LogoutCommand());
            Assert.True((await first.Get<IRuntimeRepository>().GetByCharacterIdAsync(characterId))!.LoggedOut);
        }

        await using var server = new Server(databaseName, clock);
        var report = await server.Get<RuntimeRecoveryService>().RecoverAsync();

        Assert.Equal(0, report.ReturnsScheduled);
        Assert.True(server.Get<IPresenceTracker>().IsLoggedOut(characterId));

        server.Get<TestCurrentPlayerContext>().SetAccount(accountId);
        Assert.Equal("LoggedOut", (await server.Get<HeartbeatHandler>().Handle(new HeartbeatCommand())).Status);
        await server.Get<EnterGameHandler>().Handle(new EnterGameCommand());
        Assert.False((await server.Get<IRuntimeRepository>().GetByCharacterIdAsync(characterId))!.LoggedOut);

        var heartbeat = server.Get<HeartbeatHandler>();
        await heartbeat.Handle(new HeartbeatCommand());
        clock.Advance(TimeSpan.FromSeconds(20));
        Assert.False((await heartbeat.Handle(new HeartbeatCommand())).ReturnedToLastHunt);
    }

    // R-051: a área escolhida é gravada (nome do catálogo) e o retorno após a queda usa ELA, não a padrão.
    [Fact]
    public async Task ChosenArea_IsSaved_AndTheReturnAfterADropUsesIt()
    {
        var clock = new ManualTimeProvider();
        await using var server = new Server($"area-{Guid.NewGuid():N}", clock, twoAreas: true);
        var (_, characterId, runtimeId) = await Enter(server);

        var hunt = await server.Get<ExecuteHuntHandler>().Handle(new ExecuteHuntCommand(characterId, runtimeId, "rat sewers"));
        Assert.Equal("Rat Sewers", (await server.Get<IRuntimeRepository>().GetByCharacterIdAsync(characterId))!.LastHuntArea);

        // Queda > 1 min, volta, > 10 s online → nova Hunt na área escolhida.
        clock.Advance(TimeSpan.FromSeconds(61));
        Assert.Single(await server.Get<GameSessionCoordinator>().SweepAsync());
        var heartbeat = server.Get<HeartbeatHandler>();
        await heartbeat.Handle(new HeartbeatCommand());
        clock.Advance(TimeSpan.FromSeconds(11));
        var returned = await heartbeat.Handle(new HeartbeatCommand());

        Assert.True(returned.ReturnedToLastHunt);
        Assert.NotEqual(hunt.HuntId, returned.HuntId);
        Assert.Equal("Rat Sewers", returned.LastHuntArea);
        Assert.Equal("Rat Sewers", (await server.Get<IRuntimeRepository>().GetByCharacterIdAsync(characterId))!.LastHuntArea);
    }

    // R-051/R-041 (Server Authority): área inexistente ou acima do tier → recusada, nada gravado; sem área → padrão.
    [Fact]
    public async Task UnknownOrAboveTierArea_IsRefused_NoAreaMeansTheDefault()
    {
        var clock = new ManualTimeProvider();
        await using var server = new Server($"area-refused-{Guid.NewGuid():N}", clock, twoAreas: true);
        var (_, characterId, runtimeId) = await Enter(server);
        var execute = server.Get<ExecuteHuntHandler>();

        await Assert.ThrowsAsync<InvalidHuntAreaException>(() => execute.Handle(new ExecuteHuntCommand(characterId, runtimeId, "Nowhere")));
        await Assert.ThrowsAsync<InvalidHuntAreaException>(() => execute.Handle(new ExecuteHuntCommand(characterId, runtimeId, "Dragon Lair")));
        Assert.Null(await server.Get<IHuntRepository>().GetRunningByCharacterIdAsync(characterId));
        Assert.Null((await server.Get<IRuntimeRepository>().GetByCharacterIdAsync(characterId))!.LastHuntArea);

        await execute.Handle(new ExecuteHuntCommand(characterId, runtimeId));
        Assert.Equal(SpiderCaveCatalog.AreaName, (await server.Get<IRuntimeRepository>().GetByCharacterIdAsync(characterId))!.LastHuntArea);
    }

    // Revisão do PV-EVENT-012 (ressalva 1): retorno automático que chega depois do logout é cancelado sob o
    // portão — sem Hunt e a marca continua.
    [Fact]
    public async Task AutomaticReturnAfterLogout_IsCancelled_MarkKept()
    {
        var clock = new ManualTimeProvider();
        await using var server = new Server($"return-logout-{Guid.NewGuid():N}", clock);
        var (_, characterId, runtimeId) = await Enter(server);
        await server.Get<LogoutHandler>().Handle(new LogoutCommand());

        await Assert.ThrowsAsync<AutomaticReturnCancelledException>(() => server.Get<ExecuteHuntHandler>()
            .Handle(new ExecuteHuntCommand(characterId, runtimeId, null, AutomaticReturn: true)));

        Assert.True(server.Get<IPresenceTracker>().IsLoggedOut(characterId));
        Assert.Null(await server.Get<IHuntRepository>().GetRunningByCharacterIdAsync(characterId));
    }
}
