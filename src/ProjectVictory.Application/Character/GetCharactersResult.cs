namespace ProjectVictory.Application.Character;

public sealed class GetCharactersResult
{
    public Guid CharacterId { get; init; }

    public string Name { get; init; } = string.Empty;

    public int Level { get; init; }
}