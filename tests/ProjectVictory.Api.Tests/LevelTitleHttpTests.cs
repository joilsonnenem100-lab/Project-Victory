using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using ProjectVictory.API.Configuration;
using ProjectVictory.Domain.Progression;
using Xunit;

namespace ProjectVictory.Api.Tests;

// PV-PROG-013 (DEC-20261106-30): a API expõe título e tier do personagem; rótulos e faixas vêm da
// configuração (seção "LevelTitles") e podem ser trocados sem recompilar.
[Collection("Api")]
public sealed class LevelTitleHttpTests
{
    private static async Task<(HttpClient Client, Guid CharacterId)> NewCharacter(ApiFactory factory)
    {
        var client = factory.CreateClient();
        var token = await factory.RegisterAndLoginAsync(client, $"title-{Guid.NewGuid():N}@test.com");
        ApiFactory.WithToken(client, token);

        var created = await client.PostAsJsonAsync("/api/character", new { name = TestNames.Unique("T"), characterClass = "Knight" });
        created.EnsureSuccessStatusCode();

        return (client, await created.Content.ReadFromJsonAsync<Guid>());
    }

    [Fact]
    public async Task NewCharacter_IsNovice_Tier0_InCharacterListAndEnterGame()
    {
        using var factory = new ApiFactory();
        var (client, characterId) = await NewCharacter(factory);

        var all = await client.GetFromJsonAsync<JsonElement>("/api/character");
        var character = all.EnumerateArray().Single(c => c.GetProperty("characterId").GetGuid() == characterId);
        Assert.Equal("Novice", character.GetProperty("title").GetString());
        Assert.Equal(0, character.GetProperty("titleTier").GetInt32());

        var enter = await client.PostAsync("/api/game/enter", null);
        enter.EnsureSuccessStatusCode();
        var entered = await enter.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal("Novice", entered.GetProperty("title").GetString());
        Assert.Equal(0, entered.GetProperty("titleTier").GetInt32());
    }

    [Fact]
    public async Task Label_ComesFromConfiguration_WithoutRecompiling()
    {
        const string variable = "LevelTitles__0__Label";
        var previous = Environment.GetEnvironmentVariable(variable);
        Environment.SetEnvironmentVariable(variable, "Rookie");

        try
        {
            using var factory = new ApiFactory();
            var (client, characterId) = await NewCharacter(factory);

            var all = await client.GetFromJsonAsync<JsonElement>("/api/character");
            var character = all.EnumerateArray().Single(c => c.GetProperty("characterId").GetGuid() == characterId);
            Assert.Equal("Rookie", character.GetProperty("title").GetString());
            Assert.Equal(0, character.GetProperty("titleTier").GetInt32());
        }
        finally
        {
            Environment.SetEnvironmentVariable(variable, previous);
        }
    }

    // --- provedor de configuração (sem HTTP) ---

    private static IConfigurationRoot Config(Dictionary<string, string?> values) =>
        new ConfigurationBuilder().AddInMemoryCollection(values).Build();

    [Fact]
    public void Provider_WithoutSection_UsesDefaultTable()
    {
        var provider = new ConfigurationLevelTitleProvider(Config(new()), null);

        Assert.Same(LevelTitleTable.Default, provider.Current);
    }

    [Fact]
    public void Provider_ReadsOnEveryCall_ChangeTakesEffectWithoutRestart()
    {
        var config = Config(new()
        {
            ["LevelTitles:0:Tier"] = "0", ["LevelTitles:0:Label"] = "Novice", ["LevelTitles:0:MinimumLevel"] = "1",
            ["LevelTitles:1:Tier"] = "1", ["LevelTitles:1:Label"] = "Ascension 1", ["LevelTitles:1:MinimumLevel"] = "500"
        });
        var provider = new ConfigurationLevelTitleProvider(config, null);

        Assert.Equal("Ascension 1", provider.Current.For(500).Label);

        // Troca "a quente" (o appsettings.json é recarregado pelo host do mesmo jeito).
        config["LevelTitles:1:Label"] = "Veteran";
        config["LevelTitles:1:MinimumLevel"] = "300";

        Assert.Equal("Veteran", provider.Current.For(300).Label);
        Assert.Equal(1, provider.Current.For(300).Tier);
    }

    [Fact]
    public void Provider_InvalidConfiguration_KeepsLastValidTable()
    {
        var config = Config(new()
        {
            ["LevelTitles:0:Tier"] = "0", ["LevelTitles:0:Label"] = "Novice", ["LevelTitles:0:MinimumLevel"] = "1",
            ["LevelTitles:1:Tier"] = "1", ["LevelTitles:1:Label"] = "Hero", ["LevelTitles:1:MinimumLevel"] = "10"
        });
        var provider = new ConfigurationLevelTitleProvider(config, null);
        Assert.Equal("Hero", provider.Current.For(10).Label);

        config["LevelTitles:1:MinimumLevel"] = "1"; // sobrepõe a primeira faixa → inválida

        Assert.Equal("Hero", provider.Current.For(10).Label);
    }
}
