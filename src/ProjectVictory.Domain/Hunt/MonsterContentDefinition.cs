using System;

namespace ProjectVictory.Domain.Hunt;

public sealed record MonsterContentDefinition(
    Guid MonsterId,
    string Name,
    int Rank);