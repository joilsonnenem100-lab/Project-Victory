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
        var created = await client.PostAsJsonAsync("/api/character", new { name = TestNames.Unique("L"), characterClass = "Knight" });
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

    // PV-HUNT-013 (DEC-20261106-46, fecha o R-052): 2ª Hunt com outra em andamento → 409; a 1ª continua
    // (sair dela funciona) e depois uma nova é aceita.
    [Fact]
    public async Task SecondHunt_WhileOneRuns_Is409_AndTheFirstContinues()
    {
        using var factory = new ApiFactory();
        var client = factory.CreateClient();
        ApiFactory.WithToken(client, await factory.RegisterAndLoginAsync(client, $"second-{Guid.NewGuid():N}@test.com"));
        var created = await client.PostAsJsonAsync("/api/character", new { name = TestNames.Unique("S"), characterClass = "Knight" });
        created.EnsureSuccessStatusCode();
        var characterId = await created.Content.ReadFromJsonAsync<Guid>();
        var enter = await (await client.PostAsync("/api/game/enter", null)).Content.ReadFromJsonAsync<JsonElement>();
        var start = new { characterId, runtimeId = enter.GetProperty("runtimeId").GetGuid() };

        var first = await client.PostAsJsonAsync("/api/hunt/execute", start);
        first.EnsureSuccessStatusCode();
        var huntId = (await first.Content.ReadFromJsonAsync<JsonElement>()).GetProperty("huntId").GetGuid();

        Assert.Equal(HttpStatusCode.Conflict, (await client.PostAsJsonAsync("/api/hunt/execute", start)).StatusCode);

        var leave = await client.PostAsJsonAsync("/api/hunt/leave", new { huntId });
        leave.EnsureSuccessStatusCode(); // a 1ª não foi encerrada pela recusa
        Assert.Equal("Manual", (await leave.Content.ReadFromJsonAsync<JsonElement>()).GetProperty("reason").GetString());

        (await client.PostAsJsonAsync("/api/hunt/execute", start)).EnsureSuccessStatusCode();
    }

    // PV-HUNT-013 (DEC-20261106-49): logout = saída manual — Hunt Completed.
    [Fact]
    public async Task Logout_CountsAsManualExit_HuntCompleted()
    {
        using var factory = new ApiFactory();
        var (client, _) = await PlayerInHunt(factory);

        var body = await (await client.PostAsync("/api/game/logout", null)).Content.ReadFromJsonAsync<JsonElement>();

        Assert.Equal("Completed", body.GetProperty("ending").GetProperty("huntState").GetString());
    }

    // PV-RUNTIME-013 (R-051/R-041): área escolhida inexistente → 400; a área existente é aceita.
    [Fact]
    public async Task ChosenHuntArea_UnknownIs400_KnownIsAccepted()
    {
        using var factory = new ApiFactory();
        var client = factory.CreateClient();
        ApiFactory.WithToken(client, await factory.RegisterAndLoginAsync(client, $"area-{Guid.NewGuid():N}@test.com"));
        var created = await client.PostAsJsonAsync("/api/character", new { name = TestNames.Unique("A"), characterClass = "Knight" });
        created.EnsureSuccessStatusCode();
        var characterId = await created.Content.ReadFromJsonAsync<Guid>();
        var runtimeId = (await (await client.PostAsync("/api/game/enter", null)).Content.ReadFromJsonAsync<JsonElement>()).GetProperty("runtimeId").GetGuid();

        Assert.Equal(HttpStatusCode.BadRequest, (await client.PostAsJsonAsync("/api/hunt/execute", new { characterId, runtimeId, huntArea = "Nowhere" })).StatusCode);
        (await client.PostAsJsonAsync("/api/hunt/execute", new { characterId, runtimeId, huntArea = "Spider Cave" })).EnsureSuccessStatusCode();
    }

    // PV-RUNTIME-013 (R-054): heartbeat depois do logout → "LoggedOut" (presença não recriada).
    [Fact]
    public async Task HeartbeatAfterLogout_ReportsLoggedOut_UntilEnter()
    {
        using var factory = new ApiFactory();
        var (client, _) = await PlayerInHunt(factory);

        (await client.PostAsync("/api/game/logout", null)).EnsureSuccessStatusCode();
        var late = await (await client.PostAsync("/api/game/heartbeat", null)).Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal("LoggedOut", late.GetProperty("status").GetString());

        (await client.PostAsync("/api/game/enter", null)).EnsureSuccessStatusCode();
        var back = await (await client.PostAsync("/api/game/heartbeat", null)).Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal("Online", back.GetProperty("status").GetString());
    }

    // Revisão do PV-QA-007 (ressalva 1): `automaticReturn` vindo do cliente estando deslogado → 409 (não 500).
    [Fact]
    public async Task AutomaticReturnFlagFromClient_WhileLoggedOut_Is409()
    {
        using var factory = new ApiFactory();
        var client = factory.CreateClient();
        ApiFactory.WithToken(client, await factory.RegisterAndLoginAsync(client, $"flag-{Guid.NewGuid():N}@test.com"));
        var created = await client.PostAsJsonAsync("/api/character", new { name = TestNames.Unique("F"), characterClass = "Knight" });
        created.EnsureSuccessStatusCode();
        var characterId = await created.Content.ReadFromJsonAsync<Guid>();
        var runtimeId = (await (await client.PostAsync("/api/game/enter", null)).Content.ReadFromJsonAsync<JsonElement>()).GetProperty("runtimeId").GetGuid();
        (await client.PostAsync("/api/game/logout", null)).EnsureSuccessStatusCode();

        Assert.Equal(HttpStatusCode.Conflict,
            (await client.PostAsJsonAsync("/api/hunt/execute", new { characterId, runtimeId, automaticReturn = true })).StatusCode);
    }
}
