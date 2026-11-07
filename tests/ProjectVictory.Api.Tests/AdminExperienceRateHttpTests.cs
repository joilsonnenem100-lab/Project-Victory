using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace ProjectVictory.Api.Tests;

// PV-ADMIN-010 pela API real: GET/PUT api/admin/xp-rate. Sem token → 401; jogador comum → 403;
// conta configurada em Admin:Accounts → 200 (leitura e alteração); valor inválido → 400; a nova
// recompensa usa o rate gravado (Rat = 10 XP × 3 = 30).
[Collection("Api")]
public sealed class AdminExperienceRateHttpTests
{
    private const string AdminVariable = "Admin__Accounts__0";
    private const string SeedVariable = "Combat__FixedSeed";

    private readonly ApiFactory _factory;

    public AdminExperienceRateHttpTests(ApiFactory factory)
    {
        _factory = factory;
    }

    // Mesmo critério do RewardFlowHttpTests: semente que sorteia o encounter [Rat, Cave Rat, Rat].
    private static int SeedForRatEncounter()
    {
        var templates = ProjectVictory.Infrastructure.Content.SpiderCaveCatalog.EncounterTemplates;
        var ratEncounter = templates
            .Select((t, i) => (t, i))
            .First(x => x.t.SequenceEqual(new[]
            {
                ProjectVictory.Infrastructure.Content.SpiderCaveCatalog.RatId,
                ProjectVictory.Infrastructure.Content.SpiderCaveCatalog.CaveRatId,
                ProjectVictory.Infrastructure.Content.SpiderCaveCatalog.RatId
            })).i;

        for (var seed = 1; ; seed++)
        {
            if (new ProjectVictory.Domain.Primitives.SeededRandomSource(seed).NextInclusive(0, templates.Count - 1) == ratEncounter)
            {
                return seed;
            }
        }
    }

    [Fact]
    public async Task WithoutToken_Is401()
    {
        using var anonymous = _factory.CreateClient();

        Assert.Equal(HttpStatusCode.Unauthorized, (await anonymous.GetAsync("/api/admin/xp-rate")).StatusCode);
        Assert.Equal(HttpStatusCode.Unauthorized, (await anonymous.PutAsJsonAsync("/api/admin/xp-rate", new { value = 2 })).StatusCode);
    }

    [Fact]
    public async Task CommonPlayer_Is403_AndNobodyIsAdminByDefault()
    {
        using var client = _factory.CreateClient();
        ApiFactory.WithToken(client, await _factory.RegisterAndLoginAsync(client, $"player-{Guid.NewGuid():N}@test.com"));

        Assert.Equal(HttpStatusCode.Forbidden, (await client.GetAsync("/api/admin/xp-rate")).StatusCode);
        Assert.Equal(HttpStatusCode.Forbidden, (await client.PutAsJsonAsync("/api/admin/xp-rate", new { value = 2 })).StatusCode);
    }

    [Fact]
    public async Task ConfiguredAdministrator_ReadsAndChanges_NewRewardUsesNewRate()
    {
        var adminEmail = $"admin-{Guid.NewGuid():N}@test.com";
        var previousAdmin = Environment.GetEnvironmentVariable(AdminVariable);
        var previousSeed = Environment.GetEnvironmentVariable(SeedVariable);
        Environment.SetEnvironmentVariable(AdminVariable, adminEmail);
        Environment.SetEnvironmentVariable(SeedVariable, SeedForRatEncounter().ToString());

        try
        {
            using var factory = new ApiFactory();
            using var admin = factory.CreateClient();
            ApiFactory.WithToken(admin, await factory.RegisterAndLoginAsync(admin, adminEmail));

            // Leitura: sem valor gravado, vale a configuração (1x).
            var initial = await admin.GetFromJsonAsync<JsonElement>("/api/admin/xp-rate");
            Assert.Equal(1m, initial.GetProperty("value").GetDecimal());
            Assert.Equal(1m, initial.GetProperty("configuredValue").GetDecimal());
            Assert.False(initial.GetProperty("isOverridden").GetBoolean());

            // Valor inválido → 400 e nada muda.
            foreach (var invalid in new decimal[] { 0m, -1m, 100.5m })
            {
                Assert.Equal(HttpStatusCode.BadRequest, (await admin.PutAsJsonAsync("/api/admin/xp-rate", new { value = invalid })).StatusCode);
            }

            Assert.False((await admin.GetFromJsonAsync<JsonElement>("/api/admin/xp-rate")).GetProperty("isOverridden").GetBoolean());

            // Alteração → 200 e leitura devolve o valor novo.
            var put = await admin.PutAsJsonAsync("/api/admin/xp-rate", new { value = 3 });
            Assert.Equal(HttpStatusCode.OK, put.StatusCode);
            Assert.Equal(3m, (await put.Content.ReadFromJsonAsync<JsonElement>()).GetProperty("value").GetDecimal());

            var read = await admin.GetFromJsonAsync<JsonElement>("/api/admin/xp-rate");
            Assert.Equal(3m, read.GetProperty("value").GetDecimal());
            Assert.Equal(1m, read.GetProperty("configuredValue").GetDecimal());
            Assert.True(read.GetProperty("isOverridden").GetBoolean());

            // Jogador comum continua 403 neste servidor (só a conta configurada é administradora).
            using var player = factory.CreateClient();
            ApiFactory.WithToken(player, await factory.RegisterAndLoginAsync(player, $"player-{Guid.NewGuid():N}@test.com"));
            Assert.Equal(HttpStatusCode.Forbidden, (await player.PutAsJsonAsync("/api/admin/xp-rate", new { value = 50 })).StatusCode);

            // Nova recompensa: derrotar um Rat (10 XP base) entrega 30 XP.
            var created = await player.PostAsJsonAsync("/api/character", new { name = $"A{Guid.NewGuid():N}"[..16], characterClass = "Knight" });
            created.EnsureSuccessStatusCode();
            var characterId = await created.Content.ReadFromJsonAsync<Guid>();
            var runtimeId = (await (await player.PostAsync("/api/game/enter", null)).Content.ReadFromJsonAsync<JsonElement>())
                .GetProperty("runtimeId").GetGuid();

            var hunt = await player.PostAsJsonAsync("/api/hunt/execute", new { characterId, runtimeId });
            hunt.EnsureSuccessStatusCode();
            var encounter = (await hunt.Content.ReadFromJsonAsync<JsonElement>()).GetProperty("encounter");
            Assert.Equal("Rat", encounter.GetProperty("monsterName").GetString());
            var encounterId = encounter.GetProperty("id").GetGuid();

            var defeated = false;
            for (var i = 0; i < 10 && !defeated; i++)
            {
                var attack = await player.PostAsJsonAsync("/api/hunt/attack", new { encounterId });
                attack.EnsureSuccessStatusCode();
                defeated = (await attack.Content.ReadFromJsonAsync<JsonElement>()).GetProperty("monsterDefeated").GetBoolean();
            }

            Assert.True(defeated);

            var character = (await player.GetFromJsonAsync<JsonElement>("/api/character"))
                .EnumerateArray().Single(x => x.GetProperty("characterId").GetGuid() == characterId);
            Assert.Equal(30, character.GetProperty("experience").GetInt64());
        }
        finally
        {
            Environment.SetEnvironmentVariable(AdminVariable, previousAdmin);
            Environment.SetEnvironmentVariable(SeedVariable, previousSeed);
        }
    }
}
