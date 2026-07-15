namespace ProjectVictory.Application.Character;

public sealed record CreateCharacterCommand(
    Guid AccountId,
    string Name);