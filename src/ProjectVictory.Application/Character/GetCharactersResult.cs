namespace ProjectVictory.Application.Character;

public sealed class GetCharactersResult
{
    public Guid AccountId { get; init; }

    public Guid CharacterId { get; init; }

    public string Name { get; init; } = string.Empty;

    public int Level { get; init; }

    public int Experience { get; init; }
}