using ProjectVictory.Application.Reward;
using ProjectVictory.Application.Session;
using ProjectVictory.Domain.Account;

namespace ProjectVictory.Application.Admin;

// PV-ADMIN-010: limites do XP rate ajustável pelo administrador.
public static class ExperienceRateLimits
{
    // PROVISÓRIO: limite superior 100x (sem decisão do PO; trocar aqui).
    public const decimal Maximum = 100m;
}

// PV-ADMIN-010: valor fora de (0, Maximum] → 400.
public sealed class InvalidExperienceRateException : Exception
{
    public InvalidExperienceRateException(decimal value)
        : base($"Experience rate must be greater than 0 and at most {ExperienceRateLimits.Maximum}. Received: {value}.")
    {
    }
}

public sealed record ExperienceRateResult(
    decimal Value,
    decimal ConfiguredValue,
    bool IsOverridden,
    DateTimeOffset? ChangedAt,
    decimal Maximum);

public sealed record SetExperienceRateCommand(decimal Value);

// PV-ADMIN-010: base dos casos de uso de administração — papel conferido pela identidade do token
// (defesa em profundidade; a API também exige a policy "Administrator").
public abstract class AdministratorUseCase
{
    private readonly ICurrentPlayerContext _currentPlayer;
    private readonly IAccountRoleResolver _roles;

    protected AdministratorUseCase(ICurrentPlayerContext currentPlayer, IAccountRoleResolver roles)
    {
        _currentPlayer = currentPlayer ?? throw new ArgumentNullException(nameof(currentPlayer));
        _roles = roles ?? throw new ArgumentNullException(nameof(roles));
    }

    protected async Task<Guid> EnsureAdministratorAsync(CancellationToken cancellationToken)
    {
        var accountId = _currentPlayer.AccountId;

        if (await _roles.GetRoleAsync(accountId, cancellationToken) != AccountRole.Administrator)
        {
            throw new AdministratorRequiredException();
        }

        return accountId;
    }

    protected static ExperienceRateResult ToResult(RewardSettings settings, ExperienceRateOverride? persisted) =>
        persisted is { Value: > 0 }
            ? new ExperienceRateResult(persisted.Value, settings.ExperienceRate.Value, true, persisted.ChangedAt, ExperienceRateLimits.Maximum)
            : new ExperienceRateResult(settings.ExperienceRate.Value, settings.ExperienceRate.Value, false, null, ExperienceRateLimits.Maximum);
}

// PV-ADMIN-010: leitura do XP rate efetivo (só administrador).
public sealed class GetExperienceRateHandler : AdministratorUseCase
{
    private readonly RewardSettings _settings;
    private readonly IExperienceRateStore _store;

    public GetExperienceRateHandler(
        ICurrentPlayerContext currentPlayer,
        IAccountRoleResolver roles,
        RewardSettings settings,
        IExperienceRateStore store)
        : base(currentPlayer, roles)
    {
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        _store = store ?? throw new ArgumentNullException(nameof(store));
    }

    public async Task<ExperienceRateResult> Handle(CancellationToken cancellationToken = default)
    {
        await EnsureAdministratorAsync(cancellationToken);

        return ToResult(_settings, await _store.GetAsync(cancellationToken));
    }
}

// PV-ADMIN-010: alteração do XP rate (só administrador). Vale para as NOVAS recompensas.
public sealed class SetExperienceRateHandler : AdministratorUseCase
{
    private readonly RewardSettings _settings;
    private readonly IExperienceRateStore _store;
    private readonly TimeProvider _clock;

    public SetExperienceRateHandler(
        ICurrentPlayerContext currentPlayer,
        IAccountRoleResolver roles,
        RewardSettings settings,
        IExperienceRateStore store,
        TimeProvider clock)
        : base(currentPlayer, roles)
    {
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        _store = store ?? throw new ArgumentNullException(nameof(store));
        _clock = clock ?? throw new ArgumentNullException(nameof(clock));
    }

    public async Task<ExperienceRateResult> Handle(SetExperienceRateCommand command, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        var accountId = await EnsureAdministratorAsync(cancellationToken);

        if (command.Value <= 0 || command.Value > ExperienceRateLimits.Maximum)
        {
            throw new InvalidExperienceRateException(command.Value);
        }

        var saved = new ExperienceRateOverride(command.Value, accountId, _clock.GetUtcNow());
        await _store.SaveAsync(saved, cancellationToken);

        return ToResult(_settings, saved);
    }
}
