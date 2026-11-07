using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using ProjectVictory.Application.VerticalSlice;
using ProjectVictory.Domain.Hunt;
using ProjectVictory.Domain.Runtime;
using Xunit;

namespace ProjectVictory.Api.Tests;

// PV-RUNTIME-011 pela API real: o servidor reinicia no meio de uma Hunt; o novo host recupera o
// personagem e o Runtime no startup (sem "entrar no jogo") e NÃO recupera a Hunt (ADR-057).
[Collection("Api")]
public sealed class RuntimeRecoveryHttpTests
{
    [Fact]
    public async Task ServerRestart_RecoversRuntimeAtHome_ButNotTheHunt()
    {
        using var first = new ApiFactory();
        var email = $"recover-{Guid.NewGuid():N}@test.com";
        var client = first.CreateClient();
        ApiFactory.WithToken(client, await first.RegisterAndLoginAsync(client, email));

        var created = await client.PostAsJsonAsync("/api/character", new { name = TestNames.Unique("V"), characterClass = "Knight" });
        created.EnsureSuccessStatusCode();
        var characterId = await created.Content.ReadFromJsonAsync<Guid>();
        var runtimeId = (await (await client.PostAsync("/api/game/enter", null)).Content.ReadFromJsonAsync<JsonElement>())
            .GetProperty("runtimeId").GetGuid();
        (await client.PostAsJsonAsync("/api/hunt/execute", new { characterId, runtimeId })).EnsureSuccessStatusCode();

        await first.Services.GetRequiredService<Microsoft.Extensions.Hosting.IHost>().StopAsync();

        using var restarted = first.WithWebHostBuilder(_ => { });
        _ = restarted.CreateClient(); // sobe o host (startup roda o Recovery)

        var store = restarted.Services.GetRequiredService<IRuntimeExecutionStateStore>();
        Assert.True(store.TryGetByCharacterId(characterId, out var runtime));
        Assert.Equal(runtimeId, runtime.RuntimeId);
        Assert.Equal(RuntimeActivity.Idle, runtime.GameplayContext!.CurrentActivity);

        Assert.Null(await restarted.Services.GetRequiredService<IHuntRepository>().GetRunningByCharacterIdAsync(characterId));

        // O jogador volta: login e heartbeat funcionam; nenhuma Hunt em andamento.
        var again = restarted.CreateClient();
        var login = await again.PostAsJsonAsync("/api/account/login", new { email, password = "Senha123!" });
        ApiFactory.WithToken(again, (await login.Content.ReadFromJsonAsync<JsonElement>()).GetProperty("token").GetString()!);
        var heartbeat = await (await again.PostAsync("/api/game/heartbeat", null)).Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal(JsonValueKind.Null, heartbeat.GetProperty("huntId").ValueKind);
    }
}
