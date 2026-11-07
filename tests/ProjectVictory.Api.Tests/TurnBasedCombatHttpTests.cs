using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace ProjectVictory.Api.Tests;

// PV-COMBAT-012: pela API real, o ataque executa o combate por turnos (o jogador também apanha),
// e com a mesma semente de servidor (Combat:FixedSeed, só em Development) o combate é idêntico.
[Collection("Api")]
public sealed class TurnBasedCombatHttpTests
{
    private const string SeedVariable = "Combat__FixedSeed";

    private static async Task<List<string>> FightOnce(ApiFactory factory)
    {
        using var client = factory.CreateClient();
        var token = await factory.RegisterAndLoginAsync(client, $"turns-{Guid.NewGuid():N}@test.com");
        ApiFactory.WithToken(client, token);

        var created = await client.PostAsJsonAsync("/api/character", new { name = TestNames.Unique("T"), characterClass = "Knight" });
        created.EnsureSuccessStatusCode();
        var characterId = await created.Content.ReadFromJsonAsync<Guid>();

        var enter = await client.PostAsync("/api/game/enter", null);
        enter.EnsureSuccessStatusCode();
        var runtimeId = (await enter.Content.ReadFromJsonAsync<JsonElement>()).GetProperty("runtimeId").GetGuid();

        var hunt = await client.PostAsJsonAsync("/api/hunt/execute", new { characterId, runtimeId });
        hunt.EnsureSuccessStatusCode();
        var encounterId = (await hunt.Content.ReadFromJsonAsync<JsonElement>()).GetProperty("encounter").GetProperty("id").GetGuid();

        var turns = new List<string>();
        var playerWasHit = false;

        for (var i = 0; i < 100; i++)
        {
            var attack = await client.PostAsJsonAsync("/api/hunt/attack", new { encounterId });
            attack.EnsureSuccessStatusCode();
            var body = await attack.Content.ReadFromJsonAsync<JsonElement>();

            Assert.True(body.GetProperty("playerMaximumHealth").GetInt32() >= body.GetProperty("playerHealth").GetInt32());

            foreach (var turn in body.GetProperty("turns").EnumerateArray())
            {
                turns.Add($"{turn.GetProperty("actorName").GetString()}:{turn.GetProperty("kind").GetString()}:{turn.GetProperty("damage").GetInt32()}");

                // O jogador se chama "T…"; os monstros (Rat, Cave Rat) não.
                if (!turn.GetProperty("actorName").GetString()!.StartsWith('T') && turn.GetProperty("damage").GetInt32() > 0)
                {
                    playerWasHit = true;
                }
            }

            if (body.GetProperty("combatFinished").GetBoolean())
            {
                break;
            }
        }

        Assert.NotEmpty(turns);
        Assert.True(playerWasHit, "os monstros também atacam no combate por turnos");
        return turns;
    }

    [Fact]
    public async Task SameServerSeed_ProducesIdenticalCombat()
    {
        var previous = Environment.GetEnvironmentVariable(SeedVariable);
        Environment.SetEnvironmentVariable(SeedVariable, "424242");

        try
        {
            using var factory = new ApiFactory();

            // O log usa o nome do personagem (diferente a cada execução); Normalize o substitui.
            var first = await FightOnce(factory);
            var second = await FightOnce(factory);

            Assert.Equal(Normalize(first), Normalize(second));
        }
        finally
        {
            Environment.SetEnvironmentVariable(SeedVariable, previous);
        }
    }

    private static List<string> Normalize(List<string> turns) =>
        turns.Select(t => t.StartsWith('T') ? "PLAYER" + t[t.IndexOf(':')..] : t).ToList();
}
