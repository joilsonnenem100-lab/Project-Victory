using System;
using System.Collections.Generic;
using ProjectVictory.Domain.Primitives;

namespace ProjectVictory.Domain.Reward;

// PV-REWARD-010: XP rate do servidor (DEC-20261106-12). Começa em 1x; vem só da configuração
// do servidor e é aplicado pelo Reward Pipeline (ADR-024). Nunca vem do cliente.
public sealed class ExperienceRate : ValueObject
{
    public static ExperienceRate Default { get; } = new(1m);

    public decimal Value { get; }

    public ExperienceRate(decimal value)
    {
        if (value <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(value),
                "Experience rate must be greater than zero.");
        }

        Value = value;
    }

    // Experience final = base × rate, arredondada para baixo (o jogador nunca recebe XP
    // fracionária; com 1x o valor é exatamente o da definição do monstro).
    public long Apply(long baseExperience)
    {
        if (baseExperience < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(baseExperience));
        }

        return checked((long)decimal.Floor(baseExperience * Value));
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
