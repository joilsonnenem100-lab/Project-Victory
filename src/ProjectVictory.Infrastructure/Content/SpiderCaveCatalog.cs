using ProjectVictory.Application.Combat.TurnBased;
using ProjectVictory.Application.Hunt;
using ProjectVictory.Domain.Combat.TurnBased;
using ProjectVictory.Domain.Hunt;
using ProjectVictory.Domain.Items;

namespace ProjectVictory.Infrastructure.Content;

// PV-CONTENT-011: um monstro, definido por dados (DEC-20261106-09; ADR-003: definição única).
public sealed record MonsterContent(
    Guid Id,
    string Name,
    int Level,
    int MaximumHealth,
    CombatProfile Combat,
    MonsterLootDefinition Loot,
    // PV-MONSTER-010 (ADR-005): comportamento por dados (regras de Secondary Action). Padrão: só ataque básico.
    ProjectVictory.Domain.Combat.Rules.MonsterBehavior? Behavior = null);

// PV-CONTENT-011: Spider Cave — primeira Hunt (DEC-20261106-09: local e os 5 monstros confirmados
// pelo PO). ÚNICA fonte dos dados de monstros e itens de loot: os providers de loot e de combate e
// o gerador de encounters leem daqui.
//
// *** PROVISÓRIO *** — todos os NÚMEROS (vida, Level, ataque, defesa, XP, ouro, chances), os itens
// de drop novos (Cheese, Spider Silk, Spider Fang), os preços de venda ao NPC e a composição dos
// encounters são BALANCEAMENTO PROVISÓRIO do Arquiteto, inspirado no Tibia, para o PO ajustar.
// Rat mantém os valores já validados no M2 (10 XP, 1–5 de ouro, Rat Tail 100%).
public static class SpiderCaveCatalog
{
    public const string AreaName = "Spider Cave";

    // PV-PROG-013 (DEC-20261106-30): a Spider Cave exige tier 0 (Novice).
    public const int RequiredTitleTier = 0;

    public static readonly Guid RatId = Guid.Parse("10000000-0000-0000-0000-000000000001");
    public static readonly Guid CaveRatId = Guid.Parse("10000000-0000-0000-0000-000000000002");
    public static readonly Guid SpiderId = Guid.Parse("10000000-0000-0000-0000-000000000003");
    public static readonly Guid PoisonSpiderId = Guid.Parse("10000000-0000-0000-0000-000000000004");
    public static readonly Guid GiantSpiderId = Guid.Parse("10000000-0000-0000-0000-000000000005");

    // Itens de loot (Ids fixos; também gravados na tabela Items pela migration SpiderCaveItems).
    public static ItemDefinition RatTail { get; } =
        new(Guid.Parse("00000000-0000-0000-0000-000000000001"), "Rat Tail", new ItemCharacteristics("Validation loot item"), 2);

    public static ItemDefinition Cheese { get; } =
        new(Guid.Parse("00000000-0000-0000-0000-000000000002"), "Cheese", new ItemCharacteristics("Queijo levado pelos ratos."), 3);

    public static ItemDefinition SpiderFang { get; } =
        new(Guid.Parse("00000000-0000-0000-0000-000000000003"), "Spider Fang", new ItemCharacteristics("Presa de aranha."), 8);

    public static ItemDefinition SpiderSilk { get; } =
        new(Guid.Parse("00000000-0000-0000-0000-000000000004"), "Spider Silk", new ItemCharacteristics("Seda de aranha gigante."), 25);

    public static IReadOnlyList<ItemDefinition> LootItems { get; } = [RatTail, Cheese, SpiderFang, SpiderSilk];

    // PV-MONSTER-010: comportamento atual dos 5 monstros, agora em dados — só o ataque básico (sem
    // Secondary Action), exatamente como antes. Habilidades de monstro: dados novos aqui (PROVISÓRIO).
    private static readonly ProjectVictory.Domain.Combat.Rules.MonsterBehavior BasicAttackOnly =
        ProjectVictory.Domain.Combat.Rules.MonsterBehavior.BasicAttackOnly;

    public static IReadOnlyList<MonsterContent> Monsters { get; } =
    [
        new(RatId, "Rat", 1, 20, new CombatProfile(10, 8, 1, 1),
            new MonsterLootDefinition(10, 1, 5, [new LootDropDefinition(RatTail, 1.0)]),
            BasicAttackOnly),

        new(CaveRatId, "Cave Rat", 2, 50, new CombatProfile(12, 9, 2, 2),
            new MonsterLootDefinition(15, 2, 8, [new LootDropDefinition(Cheese, 0.5)]),
            BasicAttackOnly),

        new(SpiderId, "Spider", 2, 30, new CombatProfile(12, 8, 2, 1),
            new MonsterLootDefinition(12, 1, 6, []),
            BasicAttackOnly),

        new(PoisonSpiderId, "Poison Spider", 4, 60, new CombatProfile(14, 10, 3, 2),
            new MonsterLootDefinition(22, 2, 10, [new LootDropDefinition(SpiderFang, 0.3)]),
            BasicAttackOnly),

        new(GiantSpiderId, "Giant Spider", 8, 220, new CombatProfile(20, 16, 6, 6),
            new MonsterLootDefinition(120, 10, 40, [new LootDropDefinition(SpiderSilk, 0.3), new LootDropDefinition(SpiderFang, 0.5)]),
            BasicAttackOnly)
    ];

    // Encounters possíveis na Spider Cave (sorteados pela semente do encounter).
    public static IReadOnlyList<IReadOnlyList<Guid>> EncounterTemplates { get; } =
    [
        [RatId, CaveRatId, RatId],
        [SpiderId, SpiderId],
        [CaveRatId, PoisonSpiderId],
        [PoisonSpiderId, SpiderId, RatId],
        [GiantSpiderId]
    ];

    public static MonsterContent? Find(Guid monsterId) =>
        Monsters.FirstOrDefault(m => m.Id == monsterId);
}

// PV-CONTENT-011: a área de caça vista pela Application (o gerador de encounters não conhece dados).
public sealed class SpiderCaveHuntArea : IHuntAreaCatalog
{
    public HuntAreaDefinition GetDefault() =>
        new(
            SpiderCaveCatalog.AreaName,
            SpiderCaveCatalog.EncounterTemplates
                .Select(template => (IReadOnlyList<EncounterSpawn>)template
                    .Select(id => SpiderCaveCatalog.Find(id)!)
                    .Select(m => new EncounterSpawn(m.Id, m.Name, m.MaximumHealth))
                    .ToList())
                .ToList(),
            SpiderCaveCatalog.RequiredTitleTier);
}
