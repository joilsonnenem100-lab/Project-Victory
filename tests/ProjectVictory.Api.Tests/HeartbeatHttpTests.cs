using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace ProjectVictory.Api.Tests;

// PV-RUNTIME-010: heartbeat HTTP pela API real — identidade pelo token, presença renovada.
[Collection("Api")]
public sealed class HeartbeatHttpTests
{
    [Fact]
    public async Task Heartbeat_ReturnsOnline_ForTheCharacterOfTheToken()
    {
        using var factory = new ApiFactory();
        var client = factory.CreateClient();
        ApiFactory.WithToken(client, await factory.RegisterAndLoginAsync(client, $"hb-{Guid.NewGuid():N}@test.com"));

        var created = await client.PostAsJsonAsync("/api/character", new { name = TestNames.Unique("B"), characterClass = "Knight" });
        created.EnsureSuccessStatusCode();
        var characterId = await created.Content.ReadFromJsonAsync<Guid>();

        var response = await client.PostAsync("/api/game/heartbeat", null);
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadFromJsonAsync<JsonElement>();

        Assert.Equal(characterId, body.GetProperty("characterId").GetGuid());
        Assert.Equal("Online", body.GetProperty("status").GetString());
        Assert.False(body.GetProperty("returnedToLastHunt").GetBoolean());
    }
}
