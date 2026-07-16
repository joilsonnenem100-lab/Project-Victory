using System;

namespace ProjectVictory.Domain.Progression;

public sealed class Experience
{
    public int Amount { get; }

    public Experience(int amount)
    {
        if (amount < 0)
        {
            throw new ArgumentException(
                "Experience amount cannot be negative.",
                nameof(amount));
        }

        Amount = amount;
    }
}