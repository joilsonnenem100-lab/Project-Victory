using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using ProjectVictory.Application.Supply;
using Xunit;

namespace ProjectVictory.Api.Tests;

// PV-CONTENT-012 (PO-16): o kit inicial vem do appsettings.json; o personagem novo nasce com ele.
[Collection("Api")]
public sealed class StarterKitHttpTests
{
    private readonly ApiFactory _factory;

    public StarterKitHttpTests(ApiFactory factory)
    {
        _factory = factory;
    }

    private static IReadOnlyDictionary<Guid, int> KitFromAppsettings()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
        var configuration = new ConfigurationBuilder().AddJsonFile(path, optional: false).Build();

        return SupplySettings.ParseStarterKit(
            configuration.GetSection(SupplySettings.StarterKitKey).GetChildren()
                .Select(x => new KeyValuePair<string, int>(x.Key, int.Parse(x.Value!))));
    }

    [Fact]
    public void Appsettings_KitIsValid_AndHasEveryTier()
    {
        var kit = KitFromAppsettings();

        Assert.All(PotionCatalog.All, p => Assert.True(kit.GetValueOrDefault(p.ItemId) > 0, p.Key));
    }

    [Fact]
    public async Task NewCharacter_StartsWithTheKit()
    {
        using var client = _factory.CreateClient();
        ApiFactory.WithToken(client, await _factory.RegisterAndLoginAsync(client, $"kit-{Guid.NewGuid():N}@test.com"));

        var created = await client.PostAsJsonAsync("/api/character", new { name = TestNames.Unique("K"), characterClass = "Druid" });
        created.EnsureSuccessStatusCode();
        var characterId = await created.Content.ReadFromJsonAsync<Guid>();

        var inventory = await client.GetFromJsonAsync<JsonElement>($"/api/inventory/{characterId}");

        Assert.Equal(KitFromAppsettings().Values.Sum(), inventory.GetProperty("items").GetArrayLength());
    }
}
