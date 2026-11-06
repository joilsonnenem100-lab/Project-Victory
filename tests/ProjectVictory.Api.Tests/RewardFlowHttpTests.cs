using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace ProjectVictory.Api.Tests;

// PV-REWARD-010: pela API real (Development, SQLite temporário, XP rate padrão 1x),
// derrotar um Rat entrega a XP da definição do monstro (10) e o drop (Rat Tail).
[Collection("Api")]
public sealed class RewardFlowHttpTests
{
    private readonly ApiFactory _factory;

    public RewardFlowHttpTests(ApiFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task DefeatingRat_GivesExperienceAndDrop()
    {
        using var client = _factory.CreateClient();
        var token = await _factory.RegisterAndLoginAsync(client, $"reward-{Guid.NewGuid():N}@test.com");
        ApiFactory.WithToken(client, token);

        var created = await client.PostAsJsonAsync(
            "/api/character",
            new { name = $"R{Guid.NewGuid():N}"[..16], characterClass = "Knight" });
        created.EnsureSuccessStatusCode();
        var characterId = await created.Content.ReadFromJsonAsync<Guid>();

        var enter = await client.PostAsync("/api/game/enter", null);
        enter.EnsureSuccessStatusCode();
        var runtimeId = (await enter.Content.ReadFromJsonAsync<JsonElement>()).GetProperty("runtimeId").GetGuid();

        var hunt = await client.PostAsJsonAsync("/api/hunt/execute", new { characterId, runtimeId });
        hunt.EnsureSuccessStatusCode();
        var huntBody = await hunt.Content.ReadFromJsonAsync<JsonElement>();
        var encounter = huntBody.GetProperty("encounter");
        Assert.Equal("Rat", encounter.GetProperty("monsterName").GetString());
        var encounterId = encounter.GetProperty("id").GetGuid();

        var defeated = false;
        for (var i = 0; i < 10 && !defeated; i++)
        {
            var attack = await client.PostAsJsonAsync("/api/hunt/attack", new { encounterId });
            attack.EnsureSuccessStatusCode();
            defeated = (await attack.Content.ReadFromJsonAsync<JsonElement>()).GetProperty("monsterDefeated").GetBoolean();
        }

        Assert.True(defeated);

        var characters = await client.GetFromJsonAsync<JsonElement>("/api/character");
        var character = characters.EnumerateArray().Single(x => x.GetProperty("characterId").GetGuid() == characterId);
        Assert.Equal(10, character.GetProperty("experience").GetInt64());
        Assert.Equal(1, character.GetProperty("level").GetInt32());

        var inventory = await client.GetFromJsonAsync<JsonElement>($"/api/inventory/{characterId}");
        // A resposta do inventário lista ids de instância; o ItemDefinitionId do drop é conferido
        // no SmokeTests (RewardPipelineTests). Aqui: exatamente 1 item (o Rat Tail, chance 1.0).
        Assert.Single(inventory.GetProperty("items").EnumerateArray());
    }
}
