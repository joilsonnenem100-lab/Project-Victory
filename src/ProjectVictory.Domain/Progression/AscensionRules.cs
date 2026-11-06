using System;

namespace ProjectVictory.Domain.Progression;

// PV-PROG-012 (DEC-20261106-19): a Ascension é DERIVADA do Level.
// - Ascension 1 inicial; uma nova a cada `LevelStep` Levels (padrão 500: 2 no 500, 3 no 1000, 4 no 1500).
// - Máximo atual = `MaximumAscension` (padrão 4). Passo e máximo vêm de configuração do servidor
//   ("Ascension:LevelStep", "Ascension:Maximum"), para expandir o conteúdo no futuro.
// - O Level nunca volta a 1; o desbloqueio é automático. O jogador escolhe caçar na Ascension nova
//   ou nas antigas (o conteúdo de cada Ascension está em dados).
public sealed record AscensionRules(
    int LevelStep = AscensionRules.DefaultLevelStep,
    int MaximumAscension = AscensionRules.DefaultMaximumAscension)
{
    public const int DefaultLevelStep = 500;

    public const int DefaultMaximumAscension = 4;

    public const string LevelStepKey = "Ascension:LevelStep";

    public const string MaximumAscensionKey = "Ascension:Maximum";

    public static AscensionRules Default { get; } = new();

    public int LevelStep { get; } = LevelStep > 0
        ? LevelStep
        : throw new ArgumentOutOfRangeException(nameof(LevelStep), "Ascension level step must be positive.");

    public int MaximumAscension { get; } = MaximumAscension >= 1
        ? MaximumAscension
        : throw new ArgumentOutOfRangeException(nameof(MaximumAscension), "Maximum Ascension must be at least 1.");

    public int AscensionFor(int level)
    {
        if (level < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(level));
        }

        // 1 + floor(Level / passo), limitado ao máximo (com passo 500: 499 → 1; 500 → 2; 1000 → 3).
        var ascension = 1 + (level / LevelStep);

        return Math.Min(ascension, MaximumAscension);
    }

    // Level em que a Ascension `ascension` é desbloqueada (1 → Level 1).
    public int UnlockLevel(int ascension)
    {
        if (ascension < 1 || ascension > MaximumAscension)
        {
            throw new ArgumentOutOfRangeException(nameof(ascension));
        }

        return ascension == 1 ? 1 : checked((ascension - 1) * LevelStep);
    }
}
