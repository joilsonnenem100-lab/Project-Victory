namespace ProjectVictory.Application.Game;

public sealed record EnterGameResult(
    Guid AccountId,
    Guid CharacterId,
    Guid RuntimeId,
    string CharacterName,
    int Level,
    long Experience,
    // PV-PROG-013 (DEC-20261106-30): título e tier derivados do Level (aditivo; a UI só exibe).
    string Title = "",
    int TitleTier = 0,
    // PV-DAILY-010 (PO-60, aditivo): recompensa diária entregue nesta entrada; null = nada entregue.
    ProjectVictory.Application.DailyReward.DailyRewardGrant? DailyReward = null);