using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using ProjectVictory.Domain.Primitives;
using ProjectVictory.Infrastructure.Content;
using Xunit;

namespace ProjectVictory.Api.Tests;

// PV-QA-004: ponta a ponta pela API REAL (Development, SQLite temporário, JWT aleatório).
// O personagem entra na Spider Cave; o combate é determinístico porque a semente de servidor é
// fixa (Combat:FixedSeed, só em Development). Cobre vitória, derrota, XP e loot (contagem de itens)
// pelo Reward Pipeline, uso de poção a partir do próximo turno, fim de suprimento e retorno à cidade.
// O ouro não aparece em nenhuma resposta HTTP: é verificado nos SmokeTests (limite do contrato).
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

    private sealed record Player(HttpClient Client, Guid CharacterId, Guid RuntimeId, string Name);

    private static async Task<Player> EnterWorld(ApiFactory factory, string characterClass)
    {
        var client = factory.CreateClient();
        var token = await factory.RegisterAndLoginAsync(client, $"e2e-{Guid.NewGuid():N}@test.com");
        ApiFactory.WithToken(client, token);

        var name = $"E{Guid.NewGuid():N}"[..16];
        var created = await client.PostAsJsonAsync("/api/character", new { name, characterClass });
        created.EnsureSuccessStatusCode();
        var characterId = await created.Content.ReadFromJsonAsync<Guid>();

        var enter = await client.PostAsync("/api/game/enter", null);
        enter.EnsureSuccessStatusCode();
        var runtimeId = (await enter.Content.ReadFromJsonAsync<JsonElement>()).GetProperty("runtimeId").GetGuid();

        return new Player(client, characterId, runtimeId, name);
    }

    // Revisão do PV-QA-004: confere pela resposta HTTP que o encounter sorteado é o esperado
    // (o primeiro alvo é o primeiro monstro do encounter).
    private static async Task<Guid> StartHunt(Player player, int? expectedTemplate = null)
    {
        var hunt = await player.Client.PostAsJsonAsync("/api/hunt/execute", new { characterId = player.CharacterId, runtimeId = player.RuntimeId });
        hunt.EnsureSuccessStatusCode();
        var encounter = (await hunt.Content.ReadFromJsonAsync<JsonElement>()).GetProperty("encounter");

        if (expectedTemplate is { } template)
        {
            var firstMonster = SpiderCaveCatalog.Find(SpiderCaveCatalog.EncounterTemplates[template][0])!.Name;
            Assert.Equal(firstMonster, encounter.GetProperty("monsterName").GetString());
        }

        return encounter.GetProperty("id").GetGuid();
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

        var rounds = await FightToTheEnd(player, await StartHunt(player, 0));
        var last = rounds[^1];

        // PV-HUNT-011 (DEC-20261106-22, fecha o R-036): a vitória do encounter NÃO encerra a Hunt — o
        // próximo encounter já vem na resposta; a Hunt termina pela saída manual (pipeline único).
        Assert.Equal("Victory", last.GetProperty("combatState").GetString());
        Assert.Equal(JsonValueKind.Null, last.GetProperty("huntEndReason").ValueKind);
        Assert.False(last.GetProperty("returnedToCity").GetBoolean());
        Assert.NotEqual(JsonValueKind.Null, last.GetProperty("nextEncounterId").ValueKind);

        // XP: soma das definições dos monstros mortos (Rat 10 + Cave Rat 15 + Rat 10), pelo Reward Pipeline.
        var expectedXp = SpiderCaveCatalog.EncounterTemplates[0].Sum(id => SpiderCaveCatalog.Find(id)!.Loot.Experience);
        Assert.Equal(expectedXp, rounds.Sum(r => r.GetProperty("experience").GetInt32()));
        Assert.Equal(expectedXp, (await CharacterOf(player)).GetProperty("experience").GetInt64());

        var huntId = last.GetProperty("nextEncounter").GetProperty("huntId").GetGuid();
        var leave = await player.Client.PostAsJsonAsync("/api/hunt/leave", new { huntId });
        leave.EnsureSuccessStatusCode();
        var ending = await leave.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal("Manual", ending.GetProperty("reason").GetString());
        Assert.True(ending.GetProperty("returnedToCity").GetBoolean());
        Assert.Equal(expectedXp, ending.GetProperty("experienceGained").GetInt64());
        Assert.Equal(expectedXp, (await CharacterOf(player)).GetProperty("experience").GetInt64()); // nada perdido

        // Loot: 2 Rat Tails (100%) no mínimo; Cheese é 50%.
        var inventory = await player.Client.GetFromJsonAsync<JsonElement>($"/api/inventory/{player.CharacterId}");
        Assert.InRange(inventory.GetProperty("items").GetArrayLength(), 2, 3);

        // O combate é por turnos: os monstros também atacaram.
        Assert.Contains(Turns(rounds), t => t.GetProperty("actorName").GetString() != player.Name
            && t.GetProperty("damage").GetInt32() > 0);
    }

    [Fact]
    public async Task Defeat_EndsHuntThroughSamePipeline_ReturnsToCity_WithFullHealth_AndCanHuntAgain()
    {
        // Encounter [Giant Spider] contra um Sorcerer de Level 1 (150 HP pelo Tibia, DEC-20261106-18; armadura 2).
        using var server = new Server(SeedForTemplate(4));
        var player = await EnterWorld(server.Factory, "Sorcerer");
        using var _ = player.Client;

        var rounds = await FightToTheEnd(player, await StartHunt(player, 4));
        var last = rounds[^1];

        Assert.True(last.GetProperty("playerDefeated").GetBoolean());
        Assert.Equal("Death", last.GetProperty("huntEndReason").GetString());
        Assert.True(last.GetProperty("returnedToCity").GetBoolean());

        // PV-SUPPLY-011 (DEC-20261106-22/24): toda morte leva à tela inicial ("Victoria") com os dados
        // da mensagem (conferir poções, runas e o level da Hunt).
        Assert.Equal("Victoria", last.GetProperty("returnCity").GetString());
        var notice = last.GetProperty("deathNotice");
        Assert.Equal("Death", notice.GetProperty("reasonCode").GetString());
        Assert.Equal("Victoria", notice.GetProperty("returnCity").GetString());
        Assert.Equal(new[] { "Potions", "Runes", "HuntLevel" }, notice.GetProperty("checks").EnumerateArray().Select(c => c.GetString()));
        Assert.Equal(0, (await CharacterOf(player)).GetProperty("experience").GetInt64()); // nada perdido (nada ganho)

        // Na cidade com HP cheio: a próxima Hunt começa e o primeiro turno do jogador acontece
        // com a vida máxima (150) antes do primeiro golpe recebido.
        var next = await Attack(player, await StartHunt(player));
        var firstPlayerTurn = next.GetProperty("turns").EnumerateArray().First();
        Assert.Equal(150, firstPlayerTurn.GetProperty("actorHealthAfter").GetInt32());
    }

    // PV-SUPPLY-011 (DEC-20261106-22): antes "OutOfSupplies"; agora a Hunt segue sem poção até a
    // vitória do encounter ou a morte.
    // PV-HUNT-011: desfecho do ENCOUNTER (a vitória não encerra mais a Hunt).
    private const string ExpectedPotionOutcome = "Victory";
    private const int ExpectedPotionTurns = 3;

    [Fact]
    public async Task Potion_PolicyAppliesFromNextTurn_ConsumesSupplies()
    {
        // Encounter [Rat, Cave Rat, Rat] com 3 poções de vida iniciais (configuração de servidor).
        using var server = new Server(SeedForTemplate(0), starterHealthPotions: 3);
        var player = await EnterWorld(server.Factory, "Knight");
        using var _ = player.Client;

        var encounterId = await StartHunt(player, 0);

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

        // Desfecho concreto com esta semente (revisão do PV-QA-004: sem asserção condicional).
        Assert.Equal(ExpectedPotionOutcome, rounds[^1].GetProperty("combatState").GetString());
        Assert.Equal(ExpectedPotionTurns, potionTurns);
        // PV-HUNT-011: a Hunt segue (próximo encounter), sem voltar à cidade.
        Assert.Equal(JsonValueKind.Null, rounds[^1].GetProperty("huntEndReason").ValueKind);
        Assert.False(rounds[^1].GetProperty("returnedToCity").GetBoolean());
    }

    // PV-SUPPLY-011: SUBSTITUI "fim de suprimento encerra a Hunt" — com a política de mana ligada e
    // nenhuma poção, a Hunt NÃO termina na primeira rodada.
    [Fact]
    public async Task NoSupplies_DoesNotEndTheHunt()
    {
        using var server = new Server(SeedForTemplate(1), starterHealthPotions: 1);
        var player = await EnterWorld(server.Factory, "Druid");
        using var _ = player.Client;

        var encounterId = await StartHunt(player, 1);

        (await player.Client.PostAsJsonAsync("/api/hunt/supplies",
            new { characterId = player.CharacterId, healthPotionBelowPercent = (int?)null, manaPotionBelowPercent = 50 })).EnsureSuccessStatusCode();

        var round = await Attack(player, encounterId);

        // Revisão do PV-SUPPLY-011: a Hunt segue de fato (sem motivo de fim, encounter ativo).
        Assert.Equal(JsonValueKind.Null, round.GetProperty("huntEndReason").ValueKind);
        Assert.False(round.GetProperty("combatFinished").GetBoolean());
        Assert.Equal("Active", round.GetProperty("combatState").GetString());
    }

    [Fact]
    public async Task SameSeed_SameWholeHunt()
    {
        using var server = new Server(SeedForTemplate(3));

        async Task<List<string>> Run()
        {
            var player = await EnterWorld(server.Factory, "Paladin");
            using var _ = player.Client;
            var rounds = await FightToTheEnd(player, await StartHunt(player, 3));

            return Turns(rounds)
                .Select(t => $"{(t.GetProperty("actorName").GetString() == player.Name ? "PLAYER" : t.GetProperty("actorName").GetString())}:{t.GetProperty("kind").GetString()}:{t.GetProperty("damage").GetInt32()}")
                .Append(rounds[^1].GetProperty("huntEndReason").GetString()!)
                .ToList();
        }

        Assert.Equal(await Run(), await Run());
    }
}
