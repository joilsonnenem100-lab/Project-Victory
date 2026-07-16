namespace ProjectVictory.Application.VerticalSlice;

public sealed record ValidateVerticalSliceBoundariesResult(
    bool HuntValid,
    bool RewardValid,
    bool ProgressionValid,
    bool CharacterValid);