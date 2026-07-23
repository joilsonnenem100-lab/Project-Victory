namespace ProjectVictory.Domain.Runtime;

public sealed class CharacterRuntimeState
{
    public int CurrentHealth { get; private set; }

    public int CurrentMana { get; private set; }


    public CharacterRuntimeState(
        int currentHealth,
        int currentMana)
    {
        if (currentHealth < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(currentHealth),
                "Current health cannot be negative.");
        }

        if (currentMana < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(currentMana),
                "Current mana cannot be negative.");
        }

        CurrentHealth = currentHealth;
        CurrentMana = currentMana;
    }


    public void UpdateHealth(
        int value)
    {
        if (value < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(value),
                "Health cannot be negative.");
        }

        CurrentHealth = value;
    }


    public void UpdateMana(
        int value)
    {
        if (value < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(value),
                "Mana cannot be negative.");
        }

        CurrentMana = value;
    }


    public void ConsumeMana(
        int amount)
    {
        if (amount < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(amount),
                "Mana consumption cannot be negative.");
        }

        if (amount > CurrentMana)
        {
            throw new InvalidOperationException(
                "Insufficient mana.");
        }

        CurrentMana -= amount;
    }
}