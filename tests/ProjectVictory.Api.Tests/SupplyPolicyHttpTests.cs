using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace ProjectVictory.Api.Tests;

// PV-SUPPLY-010: POST api/hunt/supplies — o servidor valida (posse pelo token e limites).
[Collection("Api")]
public sealed class SupplyPolicyHttpTests
{
    private readonly ApiFactory _factory;

    public SupplyPolicyHttpTests(ApiFactory factory)
    {
        _factory = factory;
    }

    private async Task<(HttpClient Client, Guid CharacterId)> NewPlayer()
    {
        var client = _factory.CreateClient();
        var token = await _factory.RegisterAndLoginAsync(client, $"sup-{Guid.NewGuid():N}@test.com");
        ApiFactory.WithToken(client, token);

        var created = await client.PostAsJsonAsync("/api/character", new { name = $"U{Guid.NewGuid():N}"[..16], characterClass = "Knight" });
        created.EnsureSuccessStatusCode();

        return (client, await created.Content.ReadFromJsonAsync<Guid>());
    }

    [Fact]
    public async Task ValidPolicy_IsAccepted_ForNextTurn()
    {
        var (client, characterId) = await NewPlayer();
        using (client)
        {
            var response = await client.PostAsJsonAsync("/api/hunt/supplies",
                new { characterId, healthPotionBelowPercent = 40, manaPotionBelowPercent = (int?)null });

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains("NextTurn", await response.Content.ReadAsStringAsync());
        }
    }

    [Theory]
    [InlineData(0)]
    [InlineData(150)]
    public async Task InvalidPercent_Returns400(int percent)
    {
        var (client, characterId) = await NewPlayer();
        using (client)
        {
            var response = await client.PostAsJsonAsync("/api/hunt/supplies",
                new { characterId, healthPotionBelowPercent = percent, manaPotionBelowPercent = (int?)null });

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }

    [Fact]
    public async Task OtherAccountsCharacter_Returns403()
    {
        var (owner, ownerCharacter) = await NewPlayer();
        var (other, _) = await NewPlayer();
        using (owner)
        using (other)
        {
            var response = await other.PostAsJsonAsync("/api/hunt/supplies",
                new { characterId = ownerCharacter, healthPotionBelowPercent = 50, manaPotionBelowPercent = 50 });

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }
    }
}
