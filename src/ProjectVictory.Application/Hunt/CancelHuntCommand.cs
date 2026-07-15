namespace ProjectVictory.Application.Hunt;

public sealed record CancelHuntCommand(
    Guid HuntId);