using ProjectVictory.Domain.Reward;

namespace ProjectVictory.Application.Reward;

// PV-ADMIN-010 (DEC-20261106-12): XP rate alterado pelo administrador e gravado no servidor.
public sealed record ExperienceRateOverride(
    decimal Value,
    Guid ChangedByAccountId,
    DateTimeOffset ChangedAt);

// PV-ADMIN-010: persistência do XP rate alterado (Persistence implementa; Permanente).
public interface IExperienceRateStore
{
    Task<ExperienceRateOverride?> GetAsync(CancellationToken cancellationToken = default);

    Task SaveAsync(ExperienceRateOverride value, CancellationToken cancellationToken = default);
}

// PV-ADMIN-010: XP rate EFETIVO usado pelo Reward Pipeline (ADR-024) nas novas recompensas.
public interface IExperienceRateProvider
{
    Task<ExperienceRate> GetCurrentAsync(CancellationToken cancellationToken = default);
}

// PV-ADMIN-010: valor persistido pelo administrador; sem valor persistido, o da configuração
// (Progression:ExperienceRate, PV-REWARD-010) — comportamento de antes (Regra Aditiva).
// Lido a cada recompensa (PROVISÓRIO: sem cache; é uma única linha).
public sealed class ExperienceRateProvider : IExperienceRateProvider
{
    private readonly RewardSettings _settings;
    private readonly IExperienceRateStore _store;

    public ExperienceRateProvider(RewardSettings settings, IExperienceRateStore store)
    {
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        _store = store ?? throw new ArgumentNullException(nameof(store));
    }

    public async Task<ExperienceRate> GetCurrentAsync(CancellationToken cancellationToken = default)
    {
        var persisted = await _store.GetAsync(cancellationToken);

        // Valor gravado inválido (ex.: banco editado à mão) não derruba a recompensa: vale a configuração.
        return persisted is { Value: > 0 }
            ? new ExperienceRate(persisted.Value)
            : _settings.ExperienceRate;
    }
}
