using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using ProjectVictory.Application.GameSession;
using ProjectVictory.Application.VerticalSlice;
using Xunit;

namespace ProjectVictory.Api.Tests;

// PV-QA-007 (ordem "MODO FILA 8", item 6): ponta a ponta pela API REAL (Development, SQLite temporário,
// JWT aleatório — nunca o banco do PO) das decisões da Fila 8:
// - segunda Hunt com outra em andamento → 409, a primeira continua (DEC-20261106-46);
// - reinício do servidor = queda: > 10 s online → nova Hunt na ÚLTIMA Hunt escolhida (DEC-20261106-47, R-051);
// - vida 0 no checkpoint → volta com vida e Mana cheias (DEC-20261106-48, PROVISÓRIO);
// - logout = saída manual, sem retorno — nem com heartbeat atrasado, nem depois de reinício (DEC-20261106-49, R-054).
// Relógio manual injetado no host; varredura automática desligada (o teste dispara).
[Collection("Api")]
public sealed class Fila8EndToEndTests
{
    private sealed class ManualClock : TimeProvider
    {
        private DateTimeOffset _now = DateTimeOffset.UtcNow; // JWT usa o relógio real

        public override DateTimeOffset GetUtcNow() => _now;

        public void Advance(TimeSpan by) => _now += by;
    }

    private sealed class Server : IDisposable
    {
        private readonly Dictionary<string, string?> _previous = new();
        private readonly List<WebApplicationFactory<Program>> _restarts = [];

        public ApiFactory Root { get; }

        public WebApplicationFactory<Program> Current { get; private set; }

        public ManualClock Clock { get; } = new();

        public Server()
        {
            Set("GameSession__SweepIntervalSeconds", "0");
            Root = new ApiFactory();
            Current = WithClock();
        }

        private WebApplicationFactory<Program> WithClock() =>
            Root.WithWebHostBuilder(builder => builder.ConfigureServices(services => services.AddSingleton<TimeProvider>(Clock)));

        // Reinício: para o host atual e sobe outro sobre o MESMO banco (memória nova; Recovery no startup).
        public async Task RestartAsync()
        {
            await Current.Services.GetRequiredService<Microsoft.Extensions.Hosting.IHost>().StopAsync();
            _restarts.Add(Current);
            Current = WithClock();
        }

        public async Task<IReadOnlyList<PresenceSnapshot>> SweepAsync()
        {
            using var scope = Current.Services.CreateScope();
            return await scope.ServiceProvider.GetRequiredService<GameSessionCoordinator>().SweepAsync();
        }

        public async Task<T> InScope<T>(Func<IServiceProvider, Task<T>> action)
        {
            using var scope = Current.Services.CreateScope();
            return await action(scope.ServiceProvider);
        }

        private void Set(string name, string value)
        {
            _previous[name] = Environment.GetEnvironmentVariable(name);
            Environment.SetEnvironmentVariable(name, value);
        }

        public void Dispose()
        {
            Current.Dispose();
            _restarts.ForEach(factory => factory.Dispose());
            Root.Dispose();

            foreach (var (name, value) in _previous)
            {
                Environment.SetEnvironmentVariable(name, value);
            }
        }
    }

    private sealed record Player(string Email, Guid CharacterId, Guid RuntimeId);

    private static async Task<(HttpClient Client, Player Player)> EnterWorld(Server server)
    {
        var client = server.Current.CreateClient();
        var email = $"q8-{Guid.NewGuid():N}@test.com";
        ApiFactory.WithToken(client, await server.Root.RegisterAndLoginAsync(client, email));
        var created = await client.PostAsJsonAsync("/api/character", new { name = TestNames.Unique("Q"), characterClass = "Knight" });
        created.EnsureSuccessStatusCode();
        var characterId = await created.Content.ReadFromJsonAsync<Guid>();
        var runtimeId = (await (await client.PostAsync("/api/game/enter", null)).Content.ReadFromJsonAsync<JsonElement>())
            .GetProperty("runtimeId").GetGuid();
        return (client, new Player(email, characterId, runtimeId));
    }

    private static async Task<HttpClient> LoginAgain(Server server, Player player)
    {
        var client = server.Current.CreateClient();
        var login = await client.PostAsJsonAsync("/api/account/login", new { email = player.Email, password = ApiFactory.TestPassword });
        login.EnsureSuccessStatusCode();
        ApiFactory.WithToken(client, (await login.Content.ReadFromJsonAsync<JsonElement>()).GetProperty("token").GetString()!);
        return client;
    }

    private static async Task<JsonElement> Heartbeat(HttpClient client) =>
        await (await client.PostAsync("/api/game/heartbeat", null)).Content.ReadFromJsonAsync<JsonElement>();

    [Fact]
    public async Task SecondHuntRefused_RestartReturnsToTheLastHunt_WithFullHealthAndMana()
    {
        using var server = new Server();
        var (client, player) = await EnterWorld(server);
        var start = new { characterId = player.CharacterId, runtimeId = player.RuntimeId, huntArea = "spider cave" };

        var first = await client.PostAsJsonAsync("/api/hunt/execute", start);
        first.EnsureSuccessStatusCode();
        var firstHuntId = (await first.Content.ReadFromJsonAsync<JsonElement>()).GetProperty("huntId").GetGuid();

        // Segunda Hunt com a primeira em andamento → 409; a primeira continua.
        Assert.Equal(HttpStatusCode.Conflict, (await client.PostAsJsonAsync("/api/hunt/execute", start)).StatusCode);
        Assert.Equal(firstHuntId, (await Heartbeat(client)).GetProperty("huntId").GetGuid());

        // Checkpoint com vida 0 (ex.: morte gravada) e reinício do servidor.
        await server.InScope(async services =>
        {
            await services.GetRequiredService<IRuntimeRepository>().SaveCheckpointAsync(player.RuntimeId, 0, 0);
            return true;
        });
        await server.RestartAsync();
        var again = await LoginAgain(server, player);

        // Revisão do PV-QA-007 (ressalva 3): o estado carregado no reinício é mesmo o checkpoint com vida 0.
        Assert.Equal(0, await server.InScope(async services =>
            (await services.GetRequiredService<IRuntimeRepository>().GetByCharacterIdAsync(player.CharacterId))!.Health));

        // Volta: tela inicial; 10 s exatos ainda não; mais de 10 s → NOVA Hunt na última Hunt escolhida.
        Assert.Equal(JsonValueKind.Null, (await Heartbeat(again)).GetProperty("huntId").ValueKind);
        server.Clock.Advance(TimeSpan.FromSeconds(10));
        Assert.False((await Heartbeat(again)).GetProperty("returnedToLastHunt").GetBoolean());
        server.Clock.Advance(TimeSpan.FromSeconds(1));
        var returned = await Heartbeat(again);
        Assert.True(returned.GetProperty("returnedToLastHunt").GetBoolean());
        Assert.Equal("Spider Cave", returned.GetProperty("lastHuntArea").GetString());
        var returnedHuntId = returned.GetProperty("huntId").GetGuid();
        Assert.NotEqual(firstHuntId, returnedHuntId);

        // Vida e Mana cheias (sem combate desde o retorno): a saída manual informa o estado do Runtime.
        var leave = await again.PostAsJsonAsync("/api/hunt/leave", new { huntId = returnedHuntId });
        leave.EnsureSuccessStatusCode();
        var ending = await leave.Content.ReadFromJsonAsync<JsonElement>();
        var (maximumHealth, maximumMana) = await server.InScope(async services =>
        {
            var character = await services.GetRequiredService<ICharacterRepository>().GetByIdAsync(player.CharacterId);
            return (character!.MaximumHealth.Value, character.MaximumMana.Value);
        });
        Assert.Equal(maximumHealth, ending.GetProperty("currentHealth").GetInt32());
        Assert.Equal(maximumMana, ending.GetProperty("currentMana").GetInt32());
    }

    [Fact]
    public async Task Logout_IsAManualExit_NoReturn_EvenWithALateHeartbeatOrARestart()
    {
        using var server = new Server();
        var (client, player) = await EnterWorld(server);
        (await client.PostAsJsonAsync("/api/hunt/execute", new { characterId = player.CharacterId, runtimeId = player.RuntimeId })).EnsureSuccessStatusCode();

        var logout = await (await client.PostAsync("/api/game/logout", null)).Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal("Logout", logout.GetProperty("ending").GetProperty("reason").GetString());
        Assert.Equal("Completed", logout.GetProperty("ending").GetProperty("huntState").GetString());

        // Heartbeat atrasado e página fechada: não vira queda.
        Assert.Equal("LoggedOut", (await Heartbeat(client)).GetProperty("status").GetString());
        server.Clock.Advance(TimeSpan.FromMinutes(5));
        Assert.Empty(await server.SweepAsync());

        // Reinício: continua deslogado; ao entrar de novo e ficar online, nenhuma Hunt automática.
        await server.RestartAsync();
        var again = await LoginAgain(server, player);
        Assert.Equal("LoggedOut", (await Heartbeat(again)).GetProperty("status").GetString());
        (await again.PostAsync("/api/game/enter", null)).EnsureSuccessStatusCode();
        Assert.Equal("Online", (await Heartbeat(again)).GetProperty("status").GetString());
        server.Clock.Advance(TimeSpan.FromSeconds(30));
        var later = await Heartbeat(again);
        Assert.False(later.GetProperty("returnedToLastHunt").GetBoolean());
        Assert.Equal(JsonValueKind.Null, later.GetProperty("huntId").ValueKind);
    }
}
