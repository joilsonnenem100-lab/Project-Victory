using ProjectVictory.Domain.Hunt;

namespace ProjectVictory.Domain.Hunt;

public sealed record MonsterDefinition(
    Guid Id,
    string Name,
    Guid EncounterId,
    MonsterLootDefinition Loot);