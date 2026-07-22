namespace ProjectVictory.Application.Hunt;

public sealed record EncounterResult(
    Guid Id,
    Guid HuntId,
    Guid CurrentTargetId,
    string MonsterName,
    int CurrentHealth,
    int MaximumHealth);