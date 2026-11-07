using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace ProjectVictory.Api.Tests;

// PV-FIX-001 (R-069): POST api/game/enter sem personagem na conta → 404 (antes: 500 genérico);
// com personagem, continua 200.
[Collection("Api")]
public sealed class EnterWithoutCharacterHttpTests
{
    private readonly ApiFactory _factory;

    public EnterWithoutCharacterHttpTests(ApiFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Enter_WithoutCharacter_Is404_ThenWithCharacter_Is200()
    {
        using var client = _factory.CreateClient();
        ApiFactory.WithToken(client, await _factory.RegisterAndLoginAsync(client, $"nochar-{Guid.NewGuid():N}@test.com"));

        var empty = await client.PostAsync("/api/game/enter", null);
        Assert.Equal(HttpStatusCode.NotFound, empty.StatusCode);
        Assert.Equal(404, (await empty.Content.ReadFromJsonAsync<JsonElement>()).GetProperty("status").GetInt32());

        (await client.PostAsJsonAsync("/api/character", new { name = TestNames.Unique("En"), characterClass = "Knight" })).EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, (await client.PostAsync("/api/game/enter", null)).StatusCode);
    }

    [Fact]
    public async Task Enter_WithoutToken_Is401()
    {
        using var anonymous = _factory.CreateClient();

        Assert.Equal(HttpStatusCode.Unauthorized, (await anonymous.PostAsync("/api/game/enter", null)).StatusCode);
    }
}
