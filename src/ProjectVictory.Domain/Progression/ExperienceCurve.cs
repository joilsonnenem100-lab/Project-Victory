using System;

namespace ProjectVictory.Domain.Progression;

// PV-PROG-011/012: curva de Experience decidida pelo PO (DEC-20261106-10; tabela do Tibia).
// Experience total necessária para estar no Level L:
//     XP(L) = 50/3 × (L³ − 6L² + 17L − 12)
// A curva é a mesma em todas as Ascensions (DEC-20261106-11) e NÃO tem teto de Level
// (DEC-20261106-19): o limite é só o que cabe em long (Experience é long).
public static class ExperienceCurve
{
    // Maior Level cuja Experience total cabe em long (821.009). Revisão do PV-PROG-012: a conta divide
    // por 3 ANTES de multiplicar por 50 (o polinômio é sempre múltiplo de 3), então o limite é o da
    // própria Experience, e não o de um valor intermediário.
    public static int MaximumSupportedLevel { get; } = ComputeMaximumSupportedLevel();

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
        // checked: estouro de long vira OverflowException, nunca valor errado.
        var polynomial =
            checked((l * l * l) - (6 * l * l) + (17 * l) - 12);

        return checked((polynomial / 3) * 50);
    }

    // PV-PROG-012: Level da Experience SEM tabela fixa — maior L com XP(L) ≤ experience, por busca
    // binária na curva (a curva é estritamente crescente a partir do Level 1).
    public static int LevelFor(long experience)
    {
        if (experience < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(experience),
                "Experience cannot be negative.");
        }

        var low = 1;
        var high = MaximumSupportedLevel;

        while (low < high)
        {
            // Arredonda para cima, para o laço sempre avançar.
            var middle = low + ((high - low + 1) / 2);

            if (TotalExperienceForLevel(middle) <= experience)
            {
                low = middle;
            }
            else
            {
                high = middle - 1;
            }
        }

        return low;
    }

    private static int ComputeMaximumSupportedLevel()
    {
        // Int128 não estoura nesta faixa; procura o maior L com XP(L) ≤ long.MaxValue.
        static bool Fits(long l)
        {
            Int128 big = l;
            var value = (((big * big * big) - (6 * big * big) + (17 * big) - 12) / 3) * 50;

            return value <= long.MaxValue;
        }

        long low = 1;
        long high = int.MaxValue;

        while (low < high)
        {
            var middle = low + ((high - low + 1) / 2);

            if (Fits(middle))
            {
                low = middle;
            }
            else
            {
                high = middle - 1;
            }
        }

        return (int)low;
    }
}
