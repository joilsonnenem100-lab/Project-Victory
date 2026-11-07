using System.Net;
using System.Text.Json;
using Xunit;

namespace ProjectVictory.Api.Tests;

// PV-HUNT-014 pela API real: GET api/hunt/catalog — regiões e Hunts (somente leitura, exige token).
[Collection("Api")]
public sealed class HuntCatalogHttpTests
{
    private readonly ApiFactory _factory;

    public HuntCatalogHttpTests(ApiFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task WithoutToken_Is401()
    {
        using var anonymous = _factory.CreateClient();

        Assert.Equal(HttpStatusCode.Unauthorized, (await anonymous.GetAsync("/api/hunt/catalog")).StatusCode);
    }

    [Fact]
    public async Task Catalog_ListsSpiderCave_WithRecommendationAndMonsters()
    {
        using var client = _factory.CreateClient();
        ApiFactory.WithToken(client, await _factory.RegisterAndLoginAsync(client, $"catalog-{Guid.NewGuid():N}@test.com"));

        var response = await client.GetAsync("/api/hunt/catalog");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;
        var region = Assert.Single(body.GetProperty("regions").EnumerateArray());
        Assert.Equal("Spider Cave", region.GetProperty("name").GetString());

        var hunt = Assert.Single(region.GetProperty("hunts").EnumerateArray());
        Assert.Equal("Spider Cave", hunt.GetProperty("name").GetString());
        Assert.Equal(0, hunt.GetProperty("recommendedTitleTier").GetInt32());
        Assert.Equal("Novice", hunt.GetProperty("recommendedTitle").GetString());
        Assert.Equal(1, hunt.GetProperty("recommendedLevel").GetInt32());

        var monsters = hunt.GetProperty("monsters").EnumerateArray().Select(m => m.GetProperty("name").GetString()).ToList();
        Assert.Contains("Spider", monsters);
        Assert.Equal(monsters.Count, monsters.Distinct().Count());
    }
}
