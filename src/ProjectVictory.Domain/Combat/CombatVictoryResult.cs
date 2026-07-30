namespace ProjectVictory.Domain.Combat;

public sealed class CombatVictoryResult
{
    public Guid Id { get; }

    public Guid HuntId { get; }

    public Guid EncounterId { get; }

    public Guid CharacterId { get; }

    public Guid MonsterId { get; }

    public string ResultStatus { get; }

    public DateTime CreatedAt { get; }

    public CombatVictoryResult(
        Guid id,
        Guid huntId,
        Guid encounterId,
        Guid characterId,
        Guid monsterId)
    {
        Id = id;
        HuntId = huntId;
        EncounterId = encounterId;
        CharacterId = characterId;
        MonsterId = monsterId;
        ResultStatus = "Victory";
        CreatedAt = DateTime.UtcNow;
    }
}
