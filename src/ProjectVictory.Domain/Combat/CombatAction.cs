namespace ProjectVictory.Domain.Combat;

public sealed class CombatAction
{
    public Guid Id { get; }

    public int Damage { get; }

    public CombatAction(
        Guid id,
        int damage = 5)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException(
                "Combat action id is required.",
                nameof(id));
        }

        if (damage <= 0)
        {
            throw new ArgumentException(
                "Damage must be positive.",
                nameof(damage));
        }

        Id = id;
        Damage = damage;
    }
}
