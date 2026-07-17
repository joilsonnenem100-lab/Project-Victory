namespace ProjectVictory.Application.VerticalSlice;

public sealed record ValidateGameplayRuntimeStateCommand(
    bool SessionActive,
    bool HuntActive,
    bool CombatActive);