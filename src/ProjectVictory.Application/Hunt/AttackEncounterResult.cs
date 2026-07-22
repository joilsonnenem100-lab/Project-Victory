namespace ProjectVictory.Application.Hunt;

public sealed record AttackEncounterResult(
    Guid EncounterId,
    string MonsterName,
    int CurrentHealth,
    int MaximumHealth,
    bool MonsterDefeated,
    int Experience,
    int Gold,
    int Level);