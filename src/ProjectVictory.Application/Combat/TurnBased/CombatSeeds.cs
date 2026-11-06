namespace ProjectVictory.Application.Combat.TurnBased;

// PV-COMBAT-011: configuração de combate lida só no servidor (nunca do cliente).
// "Combat:FixedSeed" fixa a semente de TODOS os combates — uso exclusivo de testes e
// depuração; em produção a chave fica ausente e cada encounter recebe uma semente aleatória.
public sealed class CombatSettings
{
    public const string FixedSeedKey = "Combat:FixedSeed";

    public int? FixedSeed { get; }

    public CombatSettings(int? fixedSeed = null)
    {
        FixedSeed = fixedSeed;
    }
}

// PV-COMBAT-011: fornece a semente de cada combate.
public interface ICombatSeedProvider
{
    int NextSeed();
}

public sealed class CombatSeedProvider : ICombatSeedProvider
{
    private readonly CombatSettings _settings;

    public CombatSeedProvider(CombatSettings settings)
    {
        _settings = settings;
    }

    public int NextSeed() =>
        _settings.FixedSeed ?? Random.Shared.Next();
}
