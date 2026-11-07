using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace ProjectVictory.Api.Tests;

// PV-DAILY-010 (PO-60) pela API real, com a configuração do appsettings.json: o primeiro "enter" da
// conta no ciclo traz a recompensa e a põe no inventário; o segundo "enter" no mesmo ciclo não traz nada.
// (Relógio real: os casos antes/depois do reset, com relógio controlado, estão no SmokeTests/DailyRewardTests.)
[Collection("Api")]
public sealed class DailyRewardHttpTests
{
    private readonly ApiFactory _factory;

    public DailyRewardHttpTests(ApiFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public void Appsettings_DailyRewardIsConfigured()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile(Path.Combine(AppContext.BaseDirectory, "appsettings.json"), optional: false)
            .Build();

        Assert.True(configuration.GetValue<long>("DailyReward:Gold") > 0);
        Assert.NotEmpty(configuration.GetSection("DailyReward:Items").GetChildren());
        Assert.True(TimeSpan.TryParse(configuration["DailyReward:ResetTimeUtc"], out _));
    }

    [Fact]
    public async Task FirstEnter_GrantsTheReward_SecondEnterInTheSameCycle_DoesNot()
    {
        using var client = _factory.CreateClient();
        ApiFactory.WithToken(client, await _factory.RegisterAndLoginAsync(client, $"daily-{Guid.NewGuid():N}@test.com"));
        var created = await client.PostAsJsonAsync("/api/character", new { name = TestNames.Unique("Q"), characterClass = "Knight" });
        created.EnsureSuccessStatusCode();
        var characterId = await created.Content.ReadFromJsonAsync<Guid>();
        var before = (await client.GetFromJsonAsync<JsonElement>($"/api/inventory/{characterId}")).GetProperty("items").GetArrayLength();

        var first = await client.PostAsync("/api/game/enter", null);
        Assert.Equal(HttpStatusCode.OK, first.StatusCode);
        var reward = (await first.Content.ReadFromJsonAsync<JsonElement>()).GetProperty("dailyReward");
        Assert.Equal(JsonValueKind.Object, reward.ValueKind);
        var quantity = reward.GetProperty("items").EnumerateArray().Sum(i => i.GetProperty("quantity").GetInt32());
        Assert.True(quantity > 0);

        var after = (await client.GetFromJsonAsync<JsonElement>($"/api/inventory/{characterId}")).GetProperty("items").GetArrayLength();
        Assert.Equal(before + quantity, after);

        var second = await client.PostAsync("/api/game/enter", null);
        Assert.Equal(HttpStatusCode.OK, second.StatusCode);
        Assert.Equal(JsonValueKind.Null, (await second.Content.ReadFromJsonAsync<JsonElement>()).GetProperty("dailyReward").ValueKind);
        Assert.Equal(after, (await client.GetFromJsonAsync<JsonElement>($"/api/inventory/{characterId}")).GetProperty("items").GetArrayLength());
    }
}
