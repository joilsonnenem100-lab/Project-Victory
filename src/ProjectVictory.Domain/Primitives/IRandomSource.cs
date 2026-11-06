namespace ProjectVictory.Domain.Primitives;

// PV-REWARD-010: fonte de aleatoriedade injetável (sorteio de ouro e de drops),
// para que as regras do Domain sejam testáveis de forma determinística.
public interface IRandomSource
{
    // Valor em [0, 1).
    double NextDouble();

    // Inteiro em [minInclusive, maxInclusive].
    int NextInclusive(int minInclusive, int maxInclusive);
}
