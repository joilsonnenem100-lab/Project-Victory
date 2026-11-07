using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using ProjectVictory.Application.GameSession;
using ProjectVictory.Domain.Primitives;
using ProjectVictory.Infrastructure.Content;
using Xunit;

namespace ProjectVictory.Api.Tests;

// PV-QA-006 (ordem "MODO FILA 7", item 6): ponta a ponta pela API REAL (Development, SQLite temporário,
// JWT aleatório — nunca o banco do PO) do M6:
// - Hunt contínua com 2+ encounters (PV-HUNT-011);
// - queda > 1 min encerra; queda curta (inclusive exatamente 60 s) não encerra (PV-HUNT-012);
// - ao voltar: tela inicial; > 10 s online → NOVA Hunt na última Hunt escolhida (PV-RUNTIME-010);
// - reinício do servidor: personagem e Runtime voltam, a Hunt não (PV-RUNTIME-011, ADR-057); o
//   reinício vale como queda — retorno após > 10 s online (PV-RUNTIME-012, DEC-20261106-47).
// O tempo é controlado por um relógio manual injetado no host; a varredura automática fica desligada
// (GameSession:SweepIntervalSeconds = 0) e é disparada pelo teste.
[Collection("Api")]
public sealed class ContinuousHuntEndToEndTests
{
    private sealed class ManualClock : TimeProvider
    {
        // Revisão do PV-QA-006 (ressalva 4): começa na hora real — o JWT usa o relógio real; um
        // componente de autenticação que passe a ler o TimeProvider não quebra o teste.
        private DateTimeOffset _now = DateTimeOffset.UtcNow;

        public override DateTimeOffset GetUtcNow() => _now;

        public void Advance(TimeSpan by) => _now += by;
    }

    private static int SeedForTemplate(int template)
    {
        for (var seed = 1; ; seed++)
        {
            if (new SeededRandomSource(seed).NextInclusive(0, SpiderCaveCatalog.EncounterTemplates.Count - 1) == template)
            {
                return seed;
            }
        }
    }

    private sealed class Server : IDisposable
    {
        private readonly Dictionary<string, string?> _previous = new();

        public ApiFactory Root { get; }

        public WebApplicationFactory<Program> Factory { get; }

        public ManualClock Clock { get; } = new();

        public Server()
        {
            Set("Combat__FixedSeed", SeedForTemplate(0).ToString()); // [Rat, Cave Rat, Rat]: o Knight vence
            Set("GameSession__SweepIntervalSeconds", "0");             // varredura só pelo teste
            Root = new ApiFactory();
            Factory = Root.WithWebHostBuilder(builder =>
                builder.ConfigureServices(services => services.AddSingleton<TimeProvider>(Clock)));
        }

        public async Task<IReadOnlyList<PresenceSnapshot>> SweepAsync()
        {
            using var scope = Factory.Services.CreateScope();
            return await scope.ServiceProvider.GetRequiredService<GameSessionCoordinator>().SweepAsync();
        }

        private void Set(string name, string value)
        {
            _previous[name] = Environment.GetEnvironmentVariable(name);
            Environment.SetEnvironmentVariable(name, value);
        }

        public void Dispose()
        {
            Factory.Dispose();
            Root.Dispose();
            foreach (var (name, value) in _previous)
            {
                Environment.SetEnvironmentVariable(name, value);
            }
        }
    }

    private sealed record Player(HttpClient Client, string Email, Guid CharacterId, Guid RuntimeId);

    private static async Task<Player> EnterWorld(Server server)
    {
        var client = server.Factory.CreateClient();
        var email = $"m6-{Guid.NewGuid():N}@test.com";
        ApiFactory.WithToken(client, await server.Root.RegisterAndLoginAsync(client, email));
        var created = await client.PostAsJsonAsync("/api/character", new { name = TestNames.Unique("M"), characterClass = "Knight" });
        created.EnsureSuccessStatusCode();
        var characterId = await created.Content.ReadFromJsonAsync<Guid>();
        var runtimeId = (await (await client.PostAsync("/api/game/enter", null)).Content.ReadFromJsonAsync<JsonElement>())
            .GetProperty("runtimeId").GetGuid();
        return new Player(client, email, characterId, runtimeId);
    }

    private static async Task<JsonElement> StartHunt(Player player)
    {
        var hunt = await player.Client.PostAsJsonAsync("/api/hunt/execute", new { characterId = player.CharacterId, runtimeId = player.RuntimeId });
        hunt.EnsureSuccessStatusCode();
        return await hunt.Content.ReadFromJsonAsync<JsonElement>();
    }

    private static async Task<JsonElement> FightEncounter(Player player, Guid encounterId)
    {
        for (var i = 0; i < 300; i++)
        {
            var attack = await player.Client.PostAsJsonAsync("/api/hunt/attack", new { encounterId });
            attack.EnsureSuccessStatusCode();
            var round = await attack.Content.ReadFromJsonAsync<JsonElement>();

            if (round.GetProperty("combatFinished").GetBoolean())
            {
                return round;
            }
        }

        throw new Xunit.Sdk.XunitException("O encounter não terminou em 300 rodadas.");
    }

    private static async Task<JsonElement> Heartbeat(Player player) =>
        await (await player.Client.PostAsync("/api/game/heartbeat", null)).Content.ReadFromJsonAsync<JsonElement>();

    [Fact]
    public async Task ContinuousHunt_TwoOrMoreEncounters_ThenLeave_KeepsEverything()
    {
        using var server = new Server();
        var player = await EnterWorld(server);
        var hunt = await StartHunt(player);
        var huntId = hunt.GetProperty("huntId").GetGuid();

        // 1º encounter: vitória → a Hunt continua com o 2º, gerado pelo Spawn.
        var first = await FightEncounter(player, hunt.GetProperty("encounter").GetProperty("id").GetGuid());
        Assert.Equal("Victory", first.GetProperty("combatState").GetString());
        Assert.Equal(JsonValueKind.Null, first.GetProperty("huntEndReason").ValueKind);
        var second = first.GetProperty("nextEncounterId").GetGuid();
        Assert.Equal(huntId, first.GetProperty("nextEncounter").GetProperty("huntId").GetGuid());

        // 2º encounter na MESMA Hunt — com a semente fixa, o desfecho é determinístico: vitória, e o 3º já
        // vem gerado (revisão do PV-QA-006, ressalva 1: sem asserção condicional).
        var secondEnd = await FightEncounter(player, second);
        Assert.Equal("Victory", secondEnd.GetProperty("combatState").GetString());
        Assert.Equal(JsonValueKind.Null, secondEnd.GetProperty("huntEndReason").ValueKind);
        Assert.True(secondEnd.GetProperty("huntExperienceGained").GetInt64() > first.GetProperty("huntExperienceGained").GetInt64());
        var third = secondEnd.GetProperty("nextEncounterId").GetGuid();
        Assert.NotEqual(second, third);
        Assert.Equal(huntId, secondEnd.GetProperty("nextEncounter").GetProperty("huntId").GetGuid());

        // Saída manual: pipeline único, motivo Manual, XP da Hunt preservada.
        var leave = await player.Client.PostAsJsonAsync("/api/hunt/leave", new { huntId });
        leave.EnsureSuccessStatusCode();
        var ending = await leave.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal("Manual", ending.GetProperty("reason").GetString());
        Assert.Equal(secondEnd.GetProperty("huntExperienceGained").GetInt64(), ending.GetProperty("experienceGained").GetInt64());

        // Nada perdido: a XP do personagem é a XP ganha na Hunt.
        var character = (await player.Client.GetFromJsonAsync<JsonElement>("/api/character")).EnumerateArray()
            .Single(c => c.GetProperty("characterId").GetGuid() == player.CharacterId);
        Assert.Equal(secondEnd.GetProperty("huntExperienceGained").GetInt64(), character.GetProperty("experience").GetInt64());
    }

    [Fact]
    public async Task ShortDrop_DoesNotEnd_LongDropEnds_ReturnAfter10sOnline_StartsANewHuntInTheLastHunt()
    {
        using var server = new Server();
        var player = await EnterWorld(server);
        var hunt = await StartHunt(player);
        var huntId = hunt.GetProperty("huntId").GetGuid();
        var encounterId = hunt.GetProperty("encounter").GetProperty("id").GetGuid();

        // Queda curta: exatamente 60 s sem sinal → a Hunt segue.
        server.Clock.Advance(TimeSpan.FromSeconds(60));
        Assert.Empty(await server.SweepAsync()); // nenhuma queda tratada
        Assert.Equal(huntId, (await Heartbeat(player)).GetProperty("huntId").GetGuid());
        (await player.Client.PostAsJsonAsync("/api/hunt/attack", new { encounterId })).EnsureSuccessStatusCode();

        // Queda longa: 61 s sem sinal → a Hunt termina pelo pipeline (Disconnect).
        server.Clock.Advance(TimeSpan.FromSeconds(61));
        var dropped = Assert.Single(await server.SweepAsync()); // a queda foi tratada (pipeline, motivo Disconnect)
        Assert.Equal(player.CharacterId, dropped.CharacterId);
        var attackAfter = await player.Client.PostAsJsonAsync("/api/hunt/attack", new { encounterId });
        // O encounter foi descartado com a Hunt: a API responde como erro genérico do servidor (500), sem
        // detalhe (mesmo comportamento de atacar um encounter inexistente).
        Assert.Equal(HttpStatusCode.InternalServerError, attackAfter.StatusCode);

        // Volta: tela inicial (entra no jogo); o 1º heartbeat não volta à Hunt.
        (await player.Client.PostAsync("/api/game/enter", null)).EnsureSuccessStatusCode();
        var back = await Heartbeat(player);
        Assert.Equal(JsonValueKind.Null, back.GetProperty("huntId").ValueKind);
        Assert.False(back.GetProperty("returnedToLastHunt").GetBoolean());

        // Exatamente 10 s: ainda não. Mais de 10 s online: NOVA Hunt na última Hunt escolhida.
        server.Clock.Advance(TimeSpan.FromSeconds(10));
        Assert.False((await Heartbeat(player)).GetProperty("returnedToLastHunt").GetBoolean());
        server.Clock.Advance(TimeSpan.FromSeconds(1));
        var returned = await Heartbeat(player);
        Assert.True(returned.GetProperty("returnedToLastHunt").GetBoolean());
        Assert.Equal("Spider Cave", returned.GetProperty("lastHuntArea").GetString());
        Assert.NotEqual(huntId, returned.GetProperty("huntId").GetGuid());

        // A nova Hunt funciona (o encounter dela é atacável).
        var newEncounter = returned.GetProperty("encounter").GetProperty("id").GetGuid();
        (await player.Client.PostAsJsonAsync("/api/hunt/attack", new { encounterId = newEncounter })).EnsureSuccessStatusCode();
    }

    [Fact]
    // PV-RUNTIME-012 (DEC-20261106-47): o reinício passou a valer como QUEDA — antes este teste fixava
    // "sem retorno automático" (pergunta ao PO da Fila 7, respondida na Fila 8).
    public async Task ServerRestart_CharacterAndRuntimeBack_HuntNot_ReturnsAfter10sOnlineLikeADrop()
    {
        using var server = new Server();
        var player = await EnterWorld(server);
        var hunt = await StartHunt(player);
        var first = await FightEncounter(player, hunt.GetProperty("encounter").GetProperty("id").GetGuid());
        var xp = first.GetProperty("huntExperienceGained").GetInt64();

        await server.Factory.Services.GetRequiredService<Microsoft.Extensions.Hosting.IHost>().StopAsync();

        using var restarted = server.Root.WithWebHostBuilder(builder =>
            builder.ConfigureServices(services => services.AddSingleton<TimeProvider>(server.Clock)));
        var client = restarted.CreateClient();
        var login = await client.PostAsJsonAsync("/api/account/login", new { email = player.Email, password = ApiFactory.TestPassword });
        login.EnsureSuccessStatusCode();
        ApiFactory.WithToken(client, (await login.Content.ReadFromJsonAsync<JsonElement>()).GetProperty("token").GetString()!);
        var again = player with { Client = client };

        // Personagem e XP voltaram; a Hunt não (ADR-057).
        var character = (await client.GetFromJsonAsync<JsonElement>("/api/character")).EnumerateArray()
            .Single(c => c.GetProperty("characterId").GetGuid() == player.CharacterId);
        Assert.Equal(xp, character.GetProperty("experience").GetInt64());
        Assert.Equal(JsonValueKind.Null, (await Heartbeat(again)).GetProperty("huntId").ValueKind);

        // Reinício = queda: depois de MAIS que 10 s online, NOVA Hunt na última Hunt escolhida.
        server.Clock.Advance(TimeSpan.FromSeconds(10));
        Assert.False((await Heartbeat(again)).GetProperty("returnedToLastHunt").GetBoolean());
        server.Clock.Advance(TimeSpan.FromSeconds(1));
        var returned = await Heartbeat(again);
        Assert.True(returned.GetProperty("returnedToLastHunt").GetBoolean());
        Assert.Equal("Spider Cave", returned.GetProperty("lastHuntArea").GetString());
        Assert.NotEqual(hunt.GetProperty("huntId").GetGuid(), returned.GetProperty("huntId").GetGuid());

        // O Runtime é o mesmo; com a Hunt de retorno em andamento, outra Hunt é recusada (DEC-20261106-46).
        var runtimeId = (await (await client.PostAsync("/api/game/enter", null)).Content.ReadFromJsonAsync<JsonElement>()).GetProperty("runtimeId").GetGuid();
        Assert.Equal(player.RuntimeId, runtimeId);
        Assert.Equal(HttpStatusCode.Conflict, (await client.PostAsJsonAsync("/api/hunt/execute", new { characterId = player.CharacterId, runtimeId })).StatusCode);
    }
}
