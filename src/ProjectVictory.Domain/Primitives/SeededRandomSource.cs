using System;

namespace ProjectVictory.Domain.Primitives;

// PV-COMBAT-011: aleatoriedade com semente. A mesma semente produz a mesma sequência, então um
// combate pode ser reproduzido em teste (ADR-052: execução determinística). Uma instância por
// combate; não é thread-safe e não deve ser compartilhada entre combates.
public sealed class SeededRandomSource : IRandomSource
{
    private readonly Random _random;

    public int Seed { get; }

    public SeededRandomSource(int seed)
    {
        Seed = seed;
        _random = new Random(seed);
    }

    public double NextDouble() =>
        _random.NextDouble();

    public int NextInclusive(int minInclusive, int maxInclusive)
    {
        if (maxInclusive < minInclusive)
        {
            throw new ArgumentException("Maximum cannot be lower than minimum.");
        }

        return _random.Next(minInclusive, checked(maxInclusive + 1));
    }
}
