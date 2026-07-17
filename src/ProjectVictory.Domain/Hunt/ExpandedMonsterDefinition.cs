using System;

namespace ProjectVictory.Domain.Hunt;

public sealed record ExpandedMonsterDefinition(
    Guid MonsterId,
    string Name,
    int Rank,
    Guid EncounterId);