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
    IReadOnlyList<RewardDrop> Drops,
    // PV-COMBAT-012: turnos executados nesta rodada (aditivo).
    IReadOnlyList<CombatTurnDto>? Turns = null,
    // PV-HUNT-010: motivo do encerramento da Hunt nesta rodada (Victory, Death…), se houve (aditivo).
    string? HuntEndReason = null,
    bool ReturnedToCity = false,
    // PV-SUPPLY-011: cidade de retorno (tela inicial) e dados da mensagem de morte (aditivos).
    string? ReturnCity = null,
    ProjectVictory.Application.World.DeathNotice? DeathNotice = null);

// PV-COMBAT-012 (revisão, ressalva 6): DTO do turno na resposta HTTP — a API não expõe tipos do Domain.
public sealed record CombatTurnDto(
    int Turn,
    Guid ActorId,
    string ActorName,
    Guid TargetId,
    string TargetName,
    string Kind,
    int Damage,
    int Healing,
    int TargetHealthAfter,
    int ActorHealthAfter,
    // PV-COMBAT-013: nome da skill usada (aditivo).
    string? SkillName = null)
{
    public static CombatTurnDto From(ProjectVictory.Domain.Combat.TurnBased.CombatTurnLogEntry entry) =>
        new(
            entry.Turn,
            entry.ActorId,
            entry.ActorName,
            entry.TargetId,
            entry.TargetName,
            entry.Kind.ToString(),
            entry.Damage,
            entry.Healing,
            entry.TargetHealthAfter,
            entry.ActorHealthAfter,
            entry.SkillName);
}
