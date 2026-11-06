using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using ProjectVictory.Domain.Primitives;
using ProjectVictory.Infrastructure.Content;
using Xunit;

namespace ProjectVictory.Api.Tests;

// PV-QA-004: ponta a ponta pela API REAL (Development, SQLite temporário, JWT aleatório).
// O personagem entra na Spider Cave; o combate é determinístico porque a semente de servidor é
// fixa (Combat:FixedSeed, só em Development). Cobre vitória, derrota, XP/ouro/loot pelo Reward
// Pipeline, uso de poção a partir do próximo turno, fim de suprimento e retorno à cidade.
[Collection("Api")]
public sealed class SpiderCaveEndToEndTests
{
    private const string SeedVariable = "Combat__FixedSeed";
    private const string StarterHealthVariable = "Supplies__StarterHealthPotions";
    private const string StarterManaVariable = "Supplies__StarterManaPotions";

    // Semente cuja composição sorteada na Spider Cave é o encounter de índice `template`.
    private static int SeedForTemplate(int template)
    {
        for (var seed = 1; ; seed++)
        {
            if (new SeededRandomSource(seed).NextInclusive(0, SpiderCaveCatalog.EncounterTemplates.Count - 1) == template)
            {
                return seed;
            }
        }
    }

    private sealed class Server : IDisposable
    {
        private readonly Dictionary<string, string?> _previous = new();

        public ApiFactory Factory { get; }

        public Server(int seed, int starterHealthPotions = 0, int starterManaPotions = 0)
        {
            Set(SeedVariable, seed.ToString());
            Set(StarterHealthVariable, starterHealthPotions.ToString());
            Set(StarterManaVariable, starterManaPotions.ToString());
            Factory = new ApiFactory();
        }

        private void Set(string name, string value)
        {
            _previous[name] = Environment.GetEnvironmentVariable(name);
            Environment.SetEnvironmentVariable(name, value);
        }

        public void Dispose()
        {
            Factory.Dispose();
            foreach (var (name, value) in _previous)
            {
                Environment.SetEnvironmentVariable(name, value);
            }
        }
    }

    private sealed record Player(HttpClient Client, Guid CharacterId, Guid RuntimeId);

    private static async Task<Player> EnterWorld(ApiFactory factory, string characterClass)
    {
        var client = factory.CreateClient();
        var token = await factory.RegisterAndLoginAsync(client, $"e2e-{Guid.NewGuid():N}@test.com");
        ApiFactory.WithToken(client, token);

        var created = await client.PostAsJsonAsync("/api/character", new { name = $"E{Guid.NewGuid():N}"[..16], characterClass });
        created.EnsureSuccessStatusCode();
        var characterId = await created.Content.ReadFromJsonAsync<Guid>();

        var enter = await client.PostAsync("/api/game/enter", null);
        enter.EnsureSuccessStatusCode();
        var runtimeId = (await enter.Content.ReadFromJsonAsync<JsonElement>()).GetProperty("runtimeId").GetGuid();

        return new Player(client, characterId, runtimeId);
    }

    private static async Task<Guid> StartHunt(Player player)
    {
        var hunt = await player.Client.PostAsJsonAsync("/api/hunt/execute", new { characterId = player.CharacterId, runtimeId = player.RuntimeId });
        hunt.EnsureSuccessStatusCode();

        return (await hunt.Content.ReadFromJsonAsync<JsonElement>()).GetProperty("encounter").GetProperty("id").GetGuid();
    }

    private static async Task<JsonElement> Attack(Player player, Guid encounterId)
    {
        var attack = await player.Client.PostAsJsonAsync("/api/hunt/attack", new { encounterId });
        attack.EnsureSuccessStatusCode();
        return await attack.Content.ReadFromJsonAsync<JsonElement>();
    }

    private static async Task<List<JsonElement>> FightToTheEnd(Player player, Guid encounterId)
    {
        var rounds = new List<JsonElement>();

        for (var i = 0; i < 300; i++)
        {
            var round = await Attack(player, encounterId);
            rounds.Add(round);

            if (round.GetProperty("combatFinished").GetBoolean())
            {
                return rounds;
            }
        }

        throw new Xunit.Sdk.XunitException("O combate não terminou em 300 rodadas.");
    }

    private static async Task<JsonElement> CharacterOf(Player player)
    {
        var all = await player.Client.GetFromJsonAsync<JsonElement>("/api/character");
        return all.EnumerateArray().Single(c => c.GetProperty("characterId").GetGuid() == player.CharacterId);
    }

    private static IEnumerable<JsonElement> Turns(IEnumerable<JsonElement> rounds) =>
        rounds.SelectMany(r => r.GetProperty("turns").EnumerateArray());

    [Fact]
    public async Task Victory_RewardsByMonster_ThroughRewardPipeline_AndReturnsToCity()
    {
        // Encounter [Rat, Cave Rat, Rat]: um Knight de Level 1 vence.
        using var server = new Server(SeedForTemplate(0));
        var player = await EnterWorld(server.Factory, "Knight");
        using var _ = player.Client;

        var rounds = await FightToTheEnd(player, await StartHunt(player));
        var last = rounds[^1];

        Assert.Equal("Victory", last.GetProperty("huntEndReason").GetString());
        Assert.True(last.GetProperty("returnedToCity").GetBoolean());

        // XP: soma das definições dos monstros mortos (Rat 10 + Cave Rat 15 + Rat 10), pelo Reward Pipeline.
        var expectedXp = SpiderCaveCatalog.EncounterTemplates[0].Sum(id => SpiderCaveCatalog.Find(id)!.Loot.Experience);
        Assert.Equal(expectedXp, rounds.Sum(r => r.GetProperty("experience").GetInt32()));
        Assert.Equal(expectedXp, (await CharacterOf(player)).GetProperty("experience").GetInt64());

        // Loot: 2 Rat Tails (100%) no mínimo; Cheese é 50%.
        var inventory = await player.Client.GetFromJsonAsync<JsonElement>($"/api/inventory/{player.CharacterId}");
        Assert.InRange(inventory.GetProperty("items").GetArrayLength(), 2, 3);

        // O combate é por turnos: os monstros também atacaram.
        Assert.Contains(Turns(rounds), t => t.GetProperty("actorName").GetString() != (string?)null
            && !t.GetProperty("actorName").GetString()!.StartsWith('E') && t.GetProperty("damage").GetInt32() > 0);
    }

    [Fact]
    public async Task Defeat_EndsHuntThroughSamePipeline_ReturnsToCity_WithFullHealth_AndCanHuntAgain()
    {
        // Encounter [Giant Spider] contra um Sorcerer de Level 1 (125 HP, armadura 2).
        using var server = new Server(SeedForTemplate(4));
        var player = await EnterWorld(server.Factory, "Sorcerer");
        using var _ = player.Client;

        var rounds = await FightToTheEnd(player, await StartHunt(player));
        var last = rounds[^1];

        Assert.True(last.GetProperty("playerDefeated").GetBoolean());
        Assert.Equal("Death", last.GetProperty("huntEndReason").GetString());
        Assert.True(last.GetProperty("returnedToCity").GetBoolean());
        Assert.Equal(0, (await CharacterOf(player)).GetProperty("experience").GetInt64()); // nada perdido (nada ganho)

        // Na cidade com HP cheio: a próxima Hunt começa e o primeiro turno do jogador acontece
        // com a vida máxima (125) antes do primeiro golpe recebido.
        var next = await Attack(player, await StartHunt(player));
        var firstPlayerTurn = next.GetProperty("turns").EnumerateArray().First();
        Assert.Equal(125, firstPlayerTurn.GetProperty("actorHealthAfter").GetInt32());
    }

    [Fact]
    public async Task Potion_PolicyAppliesFromNextTurn_ConsumesSupplies()
    {
        // Encounter [Rat, Cave Rat, Rat] com 3 poções de vida iniciais (configuração de servidor).
        using var server = new Server(SeedForTemplate(0), starterHealthPotions: 3);
        var player = await EnterWorld(server.Factory, "Knight");
        using var _ = player.Client;

        var encounterId = await StartHunt(player);

        // Rodada 1 sem política: nenhuma poção.
        var first = await Attack(player, encounterId);
        Assert.DoesNotContain(first.GetProperty("turns").EnumerateArray(), t => t.GetProperty("kind").GetString() == "UseHealthPotion");

        // O jogador liga a política durante a Hunt; o servidor responde "NextTurn".
        var policy = await player.Client.PostAsJsonAsync("/api/hunt/supplies",
            new { characterId = player.CharacterId, healthPotionBelowPercent = 100, manaPotionBelowPercent = (int?)null });
        Assert.Equal(HttpStatusCode.OK, policy.StatusCode);

        var rounds = new List<JsonElement> { first };
        if (!first.GetProperty("combatFinished").GetBoolean())
        {
            rounds.AddRange(await FightToTheEnd(player, encounterId));
        }

        var potionTurns = Turns(rounds.Skip(1)).Count(t => t.GetProperty("kind").GetString() == "UseHealthPotion");

        Assert.True(potionTurns > 0, "a política passou a valer a partir do próximo turno");
        Assert.True(potionTurns <= 3, "não se usam mais poções do que existem");

        // Cada poção cura: depois dela, a vida do jogador sobe.
        Assert.All(
            Turns(rounds.Skip(1)).Where(t => t.GetProperty("kind").GetString() == "UseHealthPotion"),
            t => Assert.True(t.GetProperty("healing").GetInt32() > 0));

        if (rounds[^1].GetProperty("huntEndReason").GetString() == "OutOfSupplies")
        {
            Assert.Equal(3, potionTurns);
            Assert.True(rounds[^1].GetProperty("returnedToCity").GetBoolean());
        }
    }

    [Fact]
    public async Task OutOfSupplies_EndsHuntThroughSamePipeline_AndReturnsToCity()
    {
        // Política de mana ligada sem nenhuma poção de mana: fim de suprimento já na primeira rodada.
        using var server = new Server(SeedForTemplate(1), starterHealthPotions: 1);
        var player = await EnterWorld(server.Factory, "Druid");
        using var _ = player.Client;

        var encounterId = await StartHunt(player);

        (await player.Client.PostAsJsonAsync("/api/hunt/supplies",
            new { characterId = player.CharacterId, healthPotionBelowPercent = (int?)null, manaPotionBelowPercent = 50 })).EnsureSuccessStatusCode();

        var round = await Attack(player, encounterId);

        Assert.Equal("OutOfSupplies", round.GetProperty("huntEndReason").GetString());
        Assert.True(round.GetProperty("returnedToCity").GetBoolean());
        Assert.True(round.GetProperty("combatFinished").GetBoolean());
        Assert.Equal("Cancelled", round.GetProperty("combatState").GetString());
    }

    [Fact]
    public async Task SameSeed_SameWholeHunt()
    {
        using var server = new Server(SeedForTemplate(3));

        async Task<List<string>> Run()
        {
            var player = await EnterWorld(server.Factory, "Paladin");
            using var _ = player.Client;
            var rounds = await FightToTheEnd(player, await StartHunt(player));

            return Turns(rounds)
                .Select(t => $"{(t.GetProperty("actorName").GetString()!.StartsWith('E') ? "PLAYER" : t.GetProperty("actorName").GetString())}:{t.GetProperty("kind").GetString()}:{t.GetProperty("damage").GetInt32()}")
                .Append(rounds[^1].GetProperty("huntEndReason").GetString()!)
                .ToList();
        }

        Assert.Equal(await Run(), await Run());
    }
}
