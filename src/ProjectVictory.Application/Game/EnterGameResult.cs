namespace ProjectVictory.Application.Game;

public sealed record EnterGameResult(
    Guid AccountId,
    Guid CharacterId,
    Guid RuntimeId,
    string CharacterName,
    int Level,
    int Experience);