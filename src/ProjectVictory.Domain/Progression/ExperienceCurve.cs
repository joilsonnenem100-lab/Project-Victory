using System;

namespace ProjectVictory.Domain.Progression;

// PV-PROG-011: curva de Experience decidida pelo PO (DEC-20261106-10).
// Experience total necessária para estar no Level L:
//     XP(L) = 50/3 × (L³ − 6L² + 17L − 12)
// A curva é a mesma em todas as Ascensions (DEC-20261106-11).
// Ela só GERA as entradas da Experience Table; quem interpreta a Experience é a tabela (ADR-025).
public static class ExperienceCurve
{
    public static long TotalExperienceForLevel(int level)
    {
        if (level < 1)
        {
            throw new ArgumentOutOfRangeException(
                nameof(level),
                "Level must be at least 1.");
        }

        long l = level;

        // (L − 1)(L² − 5L + 12) é sempre múltiplo de 3, então a divisão é exata.
        var polynomial =
            checked((l * l * l) - (6 * l * l) + (17 * l) - 12);

        return checked(50 * polynomial) / 3;
    }
}
