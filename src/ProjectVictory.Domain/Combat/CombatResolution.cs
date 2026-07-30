namespace ProjectVictory.Domain.Combat;

public sealed class CombatResolution
{
    public Guid Id { get; }

    public Guid CombatContextId { get; }

    public Guid ActionResultId { get; }

    public int Damage { get; }

    public bool TargetDefeated { get; }

    public CombatResolution(
        Guid id,
        Guid combatContextId,
        Guid actionResultId,
        int damage,
        bool targetDefeated)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException(
                "Resolution id is required.",
                nameof(id));
        }

        if (combatContextId == Guid.Empty)
        {
            throw new ArgumentException(
                "CombatContextId is required.",
                nameof(combatContextId));
        }

        if (actionResultId == Guid.Empty)
        {
            throw new ArgumentException(
                "ActionResultId is required.",
                nameof(actionResultId));
        }

        Id = id;
        CombatContextId = combatContextId;
        ActionResultId = actionResultId;
        Damage = damage;
        TargetDefeated = targetDefeated;
    }
}
