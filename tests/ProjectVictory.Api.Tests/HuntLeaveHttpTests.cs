using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace ProjectVictory.Api.Tests;

// PV-HUNT-011/012 pela API real: saída manual (posse pelo token; 403 para Hunt alheia ou inexistente;
// 409 para Hunt já encerrada) e logout (pipeline único, volta à "Victoria").
[Collection("Api")]
public sealed class HuntLeaveHttpTests
{
    private static async Task<(HttpClient Client, Guid HuntId)> PlayerInHunt(ApiFactory factory)
    {
        var client = factory.CreateClient();
        ApiFactory.WithToken(client, await factory.RegisterAndLoginAsync(client, $"leave-{Guid.NewGuid():N}@test.com"));
        var created = await client.PostAsJsonAsync("/api/character", new { name = $"L{Guid.NewGuid():N}"[..16], characterClass = "Knight" });
        created.EnsureSuccessStatusCode();
        var characterId = await created.Content.ReadFromJsonAsync<Guid>();
        var enter = await (await client.PostAsync("/api/game/enter", null)).Content.ReadFromJsonAsync<JsonElement>();
        var hunt = await client.PostAsJsonAsync("/api/hunt/execute", new { characterId, runtimeId = enter.GetProperty("runtimeId").GetGuid() });
        hunt.EnsureSuccessStatusCode();
        return (client, (await hunt.Content.ReadFromJsonAsync<JsonElement>()).GetProperty("huntId").GetGuid());
    }

    [Fact]
    public async Task Leave_OwnHunt_Ends_OtherUnknownOrAlreadyDiscardedHuntIs403()
    {
        using var factory = new ApiFactory();
        var (owner, huntId) = await PlayerInHunt(factory);
        var (stranger, _) = await PlayerInHunt(factory);

        Assert.Equal(HttpStatusCode.Forbidden, (await stranger.PostAsJsonAsync("/api/hunt/leave", new { huntId })).StatusCode);
        Assert.Equal(HttpStatusCode.Forbidden, (await owner.PostAsJsonAsync("/api/hunt/leave", new { huntId = Guid.NewGuid() })).StatusCode);

        var leave = await owner.PostAsJsonAsync("/api/hunt/leave", new { huntId });
        leave.EnsureSuccessStatusCode();
        Assert.Equal("Manual", (await leave.Content.ReadFromJsonAsync<JsonElement>()).GetProperty("reason").GetString());

        // A Hunt saiu da memória: sair de novo responde como Hunt desconhecida (403, sem revelar o id).
        Assert.Equal(HttpStatusCode.Forbidden, (await owner.PostAsJsonAsync("/api/hunt/leave", new { huntId })).StatusCode);
    }

    [Fact]
    public async Task Logout_EndsTheRunningHunt_BackToVictoria()
    {
        using var factory = new ApiFactory();
        var (client, _) = await PlayerInHunt(factory);

        var logout = await client.PostAsync("/api/game/logout", null);
        logout.EnsureSuccessStatusCode();
        var body = await logout.Content.ReadFromJsonAsync<JsonElement>();

        Assert.True(body.GetProperty("huntEnded").GetBoolean());
        Assert.Equal("Victoria", body.GetProperty("returnCity").GetString());
        Assert.Equal("Logout", body.GetProperty("ending").GetProperty("reason").GetString());
    }
}
