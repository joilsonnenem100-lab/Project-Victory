using ProjectVictory.Domain.Primitives;

namespace ProjectVictory.SmokeTests;

// PV-REWARD-010: sorteio determinístico para os testes (todo drop com chance > 0 cai;
// ouro = valor configurado, limitado ao intervalo da definição).
public sealed class FixedRandomSource(int goldPick = int.MaxValue) : IRandomSource
{
    public double NextDouble() => 0;

    public int NextInclusive(int minInclusive, int maxInclusive) =>
        Math.Clamp(goldPick, minInclusive, maxInclusive);
}
