namespace ProjectVictory.Application.VerticalSlice;

public sealed record RuntimeRecoveryData(
    Guid RuntimeId,
    Guid CharacterId,
    bool Initialized,
    int Health,
    int Mana,
    // PV-RUNTIME-010: última Hunt escolhida (aditivo).
    string? LastHuntArea = null);
