namespace ProjectVictory.Domain.Combat;

public sealed class CombatActionExecutionResult
{
    public Guid Id { get; }

    public Guid CombatContextId { get; }

    public CombatActionResultState State { get; }

    public CombatActionExecutionResult(
        Guid id,
        Guid combatContextId)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException(
                "Result id is required.",
                nameof(id));
        }

        if (combatContextId == Guid.Empty)
        {
            throw new ArgumentException(
                "CombatContextId is required.",
                nameof(combatContextId));
        }

        Id = id;
        CombatContextId = combatContextId;
        State = CombatActionResultState.Processed;
    }
}