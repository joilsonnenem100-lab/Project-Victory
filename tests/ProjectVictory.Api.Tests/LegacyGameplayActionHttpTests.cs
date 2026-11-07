using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace ProjectVictory.Api.Tests;

// PV-LEGACY-001 (DEC-20261107-04, R-053): teste FUNCIONAL do caminho legado POST api/gameplay/action pela
// API real, antes da retirada, e prova de que o fluxo validado api/hunt/* cobre o mesmo resultado de jogo.
// Depois da retirada, prova que a rota responde 404 e que o api/hunt/* segue funcionando.
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

        var created = await client.PostAsJsonAsync("/api/character", new { name = TestNames.Unique("L"), characterClass = "Knight" });
        created.EnsureSuccessStatusCode();
        var characterId = await created.Content.ReadFromJsonAsync<Guid>();

        var enter = await client.PostAsync("/api/game/enter", null);
        enter.EnsureSuccessStatusCode();
        var runtimeId = (await enter.Content.ReadFromJsonAsync<JsonElement>()).GetProperty("runtimeId").GetGuid();

        return (client, characterId, runtimeId);
    }

    // Antes da retirada (commit 694217c), este teste provava o comportamento do legado: 200
    // { actionId, gameplaySystem: "Hunt" }, Hunt em andamento (api/hunt/execute seguinte → 409) e 2ª chamada → 409.
    // Depois da retirada: a rota não existe mais (404, com token válido) e não cria Hunt — o api/hunt/execute
    // seguinte começa normalmente.
    [Fact]
    public async Task LegacyAction_NoLongerExists_Returns404_AndStartsNoHunt()
    {
        var (client, characterId, runtimeId) = await EnterWorld();
        using var _ = client;

        var legacy = await client.PostAsJsonAsync("/api/gameplay/action", new { actionId = Guid.NewGuid(), characterId });

        Assert.Equal(HttpStatusCode.NotFound, legacy.StatusCode);
        Assert.Equal(HttpStatusCode.OK,
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
