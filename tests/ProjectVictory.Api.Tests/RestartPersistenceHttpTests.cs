using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using ProjectVictory.Application.Supply;
using Xunit;

namespace ProjectVictory.Api.Tests;

// PV-PERSIST-010 (M5): "reiniciar a API não perde Account, Character, Inventory, Runtime".
// Um SEGUNDO host (DI, singletons e memória novos) sobe sobre o MESMO banco SQLite de teste — nunca o
// banco real do PO — e o estado permanente volta: conta (login), personagem (Level/XP), Runtime e a
// política de poções (antes só em memória). O primeiro host é PARADO antes do segundo subir.
[Collection("Api")]
public sealed class RestartPersistenceHttpTests
{
    [Fact]
    public async Task AfterRestart_AccountCharacterRuntimeAndSupplyPolicy_AreBack()
    {
        using var first = new ApiFactory();
        var email = $"restart-{Guid.NewGuid():N}@test.com";
        var client = first.CreateClient();
        ApiFactory.WithToken(client, await first.RegisterAndLoginAsync(client, email));

        var created = await client.PostAsJsonAsync("/api/character", new { name = $"R{Guid.NewGuid():N}"[..16], characterClass = "Knight" });
        created.EnsureSuccessStatusCode();
        var characterId = await created.Content.ReadFromJsonAsync<Guid>();

        var enter = await client.PostAsync("/api/game/enter", null);
        enter.EnsureSuccessStatusCode();
        var runtimeId = (await enter.Content.ReadFromJsonAsync<JsonElement>()).GetProperty("runtimeId").GetGuid();

        (await client.PostAsJsonAsync("/api/hunt/supplies",
            new { characterId, healthPotionBelowPercent = 40, manaPotionBelowPercent = 25 })).EnsureSuccessStatusCode();

        var before = (await client.GetFromJsonAsync<JsonElement>("/api/character")).EnumerateArray()
            .Single(c => c.GetProperty("characterId").GetGuid() == characterId);

        // Revisão do PV-PERSIST-010 (ressalva 3): a política também fica ATIVA antes do reinício.
        using (var firstScope = first.Services.CreateScope())
        {
            Assert.Equal(new SupplyPolicy(40, 25), firstScope.ServiceProvider.GetRequiredService<ISupplyPolicyStore>().ActivatePending(characterId));
        }

        // Revisão (ressalva 1): o primeiro host para antes do segundo subir.
        await first.Services.GetRequiredService<Microsoft.Extensions.Hosting.IHost>().StopAsync();

        // "Reinício": um host novo sobre o mesmo banco (o primeiro não guarda nada que o segundo veja).
        using var restarted = first.WithWebHostBuilder(_ => { });
        var again = restarted.CreateClient();

        var login = await again.PostAsJsonAsync("/api/account/login", new { email, password = "Senha123!" });
        login.EnsureSuccessStatusCode();
        ApiFactory.WithToken(again, (await login.Content.ReadFromJsonAsync<JsonElement>()).GetProperty("token").GetString()!);

        var after = (await again.GetFromJsonAsync<JsonElement>("/api/character")).EnumerateArray()
            .Single(c => c.GetProperty("characterId").GetGuid() == characterId);
        Assert.Equal(before.GetProperty("level").GetInt32(), after.GetProperty("level").GetInt32());
        Assert.Equal(before.GetProperty("experience").GetInt64(), after.GetProperty("experience").GetInt64());

        using var scope = restarted.Services.CreateScope();
        var runtimes = scope.ServiceProvider.GetRequiredService<ProjectVictory.Application.VerticalSlice.IRuntimeRepository>();
        Assert.Equal(runtimeId, (await runtimes.GetByCharacterIdAsync(characterId))!.RuntimeId); // Runtime persistido (semi-permanente)

        var policies = scope.ServiceProvider.GetRequiredService<ISupplyPolicyStore>();
        Assert.Equal(new SupplyPolicy(40, 25), policies.GetActive(characterId));
    }
}
