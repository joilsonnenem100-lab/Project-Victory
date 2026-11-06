namespace ProjectVictory.Application.Reward;

public sealed class ResolvedReward
{
    public Guid CharacterId { get; }

    public long? ExperienceAmount { get; }

    public string SourceType { get; }

    public Guid SourceId { get; }


    public ResolvedReward(
        Guid characterId,
        long? experienceAmount,
        string sourceType,
        Guid sourceId)
    {
        CharacterId = characterId;

        ExperienceAmount = experienceAmount;

        SourceType = sourceType;

        SourceId = sourceId;
    }
}