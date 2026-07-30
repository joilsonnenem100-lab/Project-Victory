namespace ProjectVictory.Domain.Combat;

public sealed class CombatResult
{
    public Guid Id { get; }

    public Guid HuntId { get; }

    public Guid EncounterId { get; }

    public Guid CharacterId { get; }

    public Guid OpponentId { get; }

    public CombatOutcome Outcome { get; }

    public DateTime CreatedAt { get; }


    public CombatResult(
        Guid id,
        Guid huntId,
        Guid encounterId,
        Guid characterId,
        Guid opponentId,
        CombatOutcome outcome)
    {
        Id = id;

        HuntId = huntId;

        EncounterId = encounterId;

        CharacterId = characterId;

        OpponentId = opponentId;

        Outcome = outcome;

        CreatedAt = DateTime.UtcNow;
    }
}