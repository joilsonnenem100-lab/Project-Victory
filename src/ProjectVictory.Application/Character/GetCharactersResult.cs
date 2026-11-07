namespace ProjectVictory.Application.Character;

public sealed class GetCharactersResult
{
    public Guid AccountId { get; init; }

    public Guid CharacterId { get; init; }

    public string Name { get; init; } = string.Empty;

    public int Level { get; init; }

    public long Experience { get; init; }

    // PV-PROG-013 (DEC-20261106-30): título e tier derivados do Level (a UI só exibe).
    public string Title { get; init; } = string.Empty;

    public int TitleTier { get; init; }

    public IReadOnlyList<string> AvailableAbilities { get; init; }
        = [];

    // PV-CHAR-013 (aditivo): classe, aparência e exclusão pendente (data em que o servidor finaliza).
    public string CharacterClass { get; init; } = string.Empty;

    public string Gender { get; init; } = string.Empty;

    public string? Outfit { get; init; }

    public DateTimeOffset? DeletionScheduledFor { get; init; }
}
