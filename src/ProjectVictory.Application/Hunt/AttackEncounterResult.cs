using ProjectVictory.Application.Reward;

namespace ProjectVictory.Application.Hunt;

public sealed record AttackEncounterResult(
    Guid EncounterId,
    string MonsterName,
    int CurrentHealth,
    int MaximumHealth,
    bool MonsterDefeated,
    int PlayerHealth,
    int PlayerMaximumHealth,
    bool PlayerDefeated,
    bool CombatFinished,
    string CombatState,
    int Experience,
    int Gold,
    int Level,
    IReadOnlyList<RewardDrop> Drops);