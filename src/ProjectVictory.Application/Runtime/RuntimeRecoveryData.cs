namespace ProjectVictory.Application.VerticalSlice;

public sealed record RuntimeRecoveryData(
    Guid RuntimeId,
    Guid CharacterId,
    bool Initialized,
    int Health,
    int Mana);
