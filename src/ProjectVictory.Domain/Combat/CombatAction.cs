namespace ProjectVictory.Domain.Combat;

public sealed class CombatAction
{
    public Guid Id { get; }

    public CombatAction(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException(
                "Combat action id is required.",
                nameof(id));
        }

        Id = id;
    }
}