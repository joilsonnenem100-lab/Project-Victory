using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace ProjectVictory.Api.Tests;

// PV-LEGACY-001 (DEC-20261107-04, R-053): teste FUNCIONAL do caminho legado POST api/gameplay/action pela
// API real, antes da retirada, e prova de que o fluxo validado api/hunt/* cobre o mesmo resultado de jogo.
[Collection(ApiCollection.Name)]
public sealed class LegacyGameplayActionHttpTests
{
    private readonly ApiFactory _factory;

    public LegacyGameplayActionHttpTests(ApiFactory factory)
    {
        _factory = factory;
    }

    private async Task<(HttpClient Client, Guid CharacterId, Guid RuntimeId)> EnterWorld()
    {
        var client = _factory.CreateClient();
        ApiFactory.WithToken(client, await _factory.RegisterAndLoginAsync(client, $"legacy-{Guid.NewGuid():N}@test.com"));

        var created = await client.PostAsJsonAsync("/api/character", new { name = $"L{Guid.NewGuid():N}"[..16], characterClass = "Knight" });
        created.EnsureSuccessStatusCode();
        var characterId = await created.Content.ReadFromJsonAsync<Guid>();

        var enter = await client.PostAsync("/api/game/enter", null);
        enter.EnsureSuccessStatusCode();
        var runtimeId = (await enter.Content.ReadFromJsonAsync<JsonElement>()).GetProperty("runtimeId").GetGuid();

        return (client, characterId, runtimeId);
    }

    // Comportamento atual do legado: 200 { actionId, gameplaySystem: "Hunt" }; deixa uma Hunt em andamento
    // (o api/hunt/execute seguinte → 409); a 2ª chamada do legado → 409 (PV-HUNT-013, sem teste até aqui).
    [Fact]
    public async Task LegacyAction_StartsARunningHunt_SecondCallIsConflict()
    {
        var (client, characterId, runtimeId) = await EnterWorld();
        using var _ = client;
        var actionId = Guid.NewGuid();

        var first = await client.PostAsJsonAsync("/api/gameplay/action", new { actionId, characterId });

        Assert.Equal(HttpStatusCode.OK, first.StatusCode);
        var body = await first.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal(actionId, body.GetProperty("actionId").GetGuid());
        Assert.Equal("Hunt", body.GetProperty("gameplaySystem").GetString());

        Assert.Equal(HttpStatusCode.Conflict,
            (await client.PostAsJsonAsync("/api/gameplay/action", new { actionId = Guid.NewGuid(), characterId })).StatusCode);
        Assert.Equal(HttpStatusCode.Conflict,
            (await client.PostAsJsonAsync("/api/hunt/execute", new { characterId, runtimeId })).StatusCode);
    }

    // Cobertura: o fluxo validado api/hunt/* produz o mesmo resultado de jogo (Hunt em andamento com
    // Encounter; 2ª Hunt → 409) e mais: luta de verdade (attack) e encerra (leave).
    [Fact]
    public async Task HuntFlow_CoversTheSameGameResult()
    {
        var (client, characterId, runtimeId) = await EnterWorld();
        using var _ = client;

        var hunt = await client.PostAsJsonAsync("/api/hunt/execute", new { characterId, runtimeId });
        Assert.Equal(HttpStatusCode.OK, hunt.StatusCode);
        var started = await hunt.Content.ReadFromJsonAsync<JsonElement>();
        var huntId = started.GetProperty("huntId").GetGuid();
        var encounterId = started.GetProperty("encounter").GetProperty("id").GetGuid();

        Assert.Equal(HttpStatusCode.Conflict,
            (await client.PostAsJsonAsync("/api/hunt/execute", new { characterId, runtimeId })).StatusCode);

        var attack = await client.PostAsJsonAsync("/api/hunt/attack", new { encounterId });
        Assert.Equal(HttpStatusCode.OK, attack.StatusCode);
        Assert.NotEmpty((await attack.Content.ReadFromJsonAsync<JsonElement>()).GetProperty("turns").EnumerateArray());

        Assert.Equal(HttpStatusCode.OK, (await client.PostAsJsonAsync("/api/hunt/leave", new { huntId })).StatusCode);

        // Depois de sair, uma nova Hunt pode começar.
        Assert.Equal(HttpStatusCode.OK,
            (await client.PostAsJsonAsync("/api/hunt/execute", new { characterId, runtimeId })).StatusCode);
    }
}
