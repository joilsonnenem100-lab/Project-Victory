using ProjectVictory.Domain.Primitives;

namespace ProjectVictory.Infrastructure.Services;

// PV-REWARD-010: aleatoriedade do servidor para sorteio de ouro e drops (thread-safe).
public sealed class SystemRandomSource : IRandomSource
{
    public double NextDouble() =>
        Random.Shared.NextDouble();

    public int NextInclusive(int minInclusive, int maxInclusive)
    {
        if (maxInclusive < minInclusive)
        {
            throw new ArgumentException("Maximum cannot be lower than minimum.");
        }

        return Random.Shared.Next(minInclusive, checked(maxInclusive + 1));
    }
}
