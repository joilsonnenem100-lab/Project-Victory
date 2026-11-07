using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace ProjectVictory.Api.Tests;

// PV-HUD-ANALYZER-010 pela API real: GET api/hunt/analyzer/{characterId} (tempo real) e
// GET api/hunt/analyzer/{characterId}/history (resumo das últimas Hunts). 401 sem token; 403 para
// personagem de outra conta (PV-SEC-001).
[Collection("Api")]
public sealed class HuntAnalyzerHttpTests
{
    private readonly ApiFactory _factory;

    public HuntAnalyzerHttpTests(ApiFactory factory)
    {
        _factory = factory;
    }

    private sealed record Player(HttpClient Client, Guid CharacterId, Guid RuntimeId);

    private async Task<Player> NewPlayer()
    {
        var client = _factory.CreateClient();
        ApiFactory.WithToken(client, await _factory.RegisterAndLoginAsync(client, $"analyzer-{Guid.NewGuid():N}@test.com"));
        var created = await client.PostAsJsonAsync("/api/character", new { name = $"Z{Guid.NewGuid():N}"[..16], characterClass = "Knight" });
        created.EnsureSuccessStatusCode();
        var characterId = await created.Content.ReadFromJsonAsync<Guid>();
        var runtimeId = (await (await client.PostAsync("/api/game/enter", null)).Content.ReadFromJsonAsync<JsonElement>())
            .GetProperty("runtimeId").GetGuid();
        return new Player(client, characterId, runtimeId);
    }

    [Fact]
    public async Task WithoutToken_Is401()
    {
        using var anonymous = _factory.CreateClient();
        var characterId = Guid.NewGuid();

        Assert.Equal(HttpStatusCode.Unauthorized, (await anonymous.GetAsync($"/api/hunt/analyzer/{characterId}")).StatusCode);
        Assert.Equal(HttpStatusCode.Unauthorized, (await anonymous.GetAsync($"/api/hunt/analyzer/{characterId}/history")).StatusCode);
    }

    [Fact]
    public async Task OtherAccountsCharacter_Is403()
    {
        var owner = await NewPlayer();
        var stranger = await NewPlayer();
        using (owner.Client)
        using (stranger.Client)
        {
            Assert.Equal(HttpStatusCode.Forbidden, (await stranger.Client.GetAsync($"/api/hunt/analyzer/{owner.CharacterId}")).StatusCode);
            Assert.Equal(HttpStatusCode.Forbidden, (await stranger.Client.GetAsync($"/api/hunt/analyzer/{owner.CharacterId}/history")).StatusCode);
        }
    }

    [Fact]
    public async Task DuringTheHunt_IsLive_AfterLeaving_HistoryHasTheSummary()
    {
        var player = await NewPlayer();
        using var client = player.Client;

        // Antes de caçar: sem Hunt e sem histórico.
        var idle = await client.GetFromJsonAsync<JsonElement>($"/api/hunt/analyzer/{player.CharacterId}");
        Assert.False(idle.GetProperty("hasActiveHunt").GetBoolean());
        Assert.Equal(JsonValueKind.Null, idle.GetProperty("lastHunt").ValueKind);
        Assert.Empty((await client.GetFromJsonAsync<JsonElement>($"/api/hunt/analyzer/{player.CharacterId}/history")).EnumerateArray());

        var start = await client.PostAsJsonAsync("/api/hunt/execute", new { characterId = player.CharacterId, runtimeId = player.RuntimeId });
        start.EnsureSuccessStatusCode();
        var hunt = await start.Content.ReadFromJsonAsync<JsonElement>();
        var huntId = hunt.GetProperty("huntId").GetGuid();
        var encounterId = hunt.GetProperty("encounter").GetProperty("id").GetGuid();

        // Rodadas até o fim do encounter (vitória ou morte): o Analyzer acompanha a XP da Hunt.
        long huntExperience = 0;
        string? endReason = null;
        for (var i = 0; i < 300; i++)
        {
            var attack = await client.PostAsJsonAsync("/api/hunt/attack", new { encounterId });
            attack.EnsureSuccessStatusCode();
            var round = await attack.Content.ReadFromJsonAsync<JsonElement>();
            huntExperience = round.GetProperty("huntExperienceGained").GetInt64();
            endReason = round.GetProperty("huntEndReason").GetString();

            if (round.GetProperty("combatFinished").GetBoolean())
            {
                break;
            }
        }

        if (endReason is null)
        {
            var live = await client.GetFromJsonAsync<JsonElement>($"/api/hunt/analyzer/{player.CharacterId}");
            Assert.True(live.GetProperty("hasActiveHunt").GetBoolean());
            Assert.Equal(huntId, live.GetProperty("huntId").GetGuid());
            var analyzer = live.GetProperty("analyzer");
            Assert.Equal(huntExperience, analyzer.GetProperty("experienceGained").GetInt64());
            Assert.True(huntExperience > 0);
            Assert.Equal(
                analyzer.GetProperty("profit").GetInt64() - analyzer.GetProperty("debit").GetInt64(),
                analyzer.GetProperty("netProfit").GetInt64());
            Assert.Equal(
                analyzer.GetProperty("lootValue").GetInt64() + analyzer.GetProperty("gold").GetInt64(),
                analyzer.GetProperty("profit").GetInt64());
            foreach (var field in new[] { "experiencePerHour", "elapsedSeconds", "suppliesUsed", "itemsLooted" })
            {
                Assert.True(analyzer.TryGetProperty(field, out _), field);
            }

            (await client.PostAsJsonAsync("/api/hunt/leave", new { huntId })).EnsureSuccessStatusCode();
            endReason = "Manual";
        }

        var history = await client.GetFromJsonAsync<JsonElement>($"/api/hunt/analyzer/{player.CharacterId}/history");
        var summary = Assert.Single(history.EnumerateArray());
        Assert.Equal(huntId, summary.GetProperty("huntId").GetGuid());
        Assert.Equal(endReason, summary.GetProperty("endReason").GetString());
        Assert.Equal(huntExperience, summary.GetProperty("analyzer").GetProperty("experienceGained").GetInt64());

        var after = await client.GetFromJsonAsync<JsonElement>($"/api/hunt/analyzer/{player.CharacterId}");
        Assert.False(after.GetProperty("hasActiveHunt").GetBoolean());
        Assert.Equal(huntId, after.GetProperty("lastHunt").GetProperty("huntId").GetGuid());
    }
}
