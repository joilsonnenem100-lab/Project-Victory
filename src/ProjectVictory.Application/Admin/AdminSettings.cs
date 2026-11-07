using ProjectVictory.Domain.Account;

namespace ProjectVictory.Application.Admin;

// PV-ADMIN-010 (DEC-20261106-12): quem é administrador vem da configuração do servidor
// "Admin:Accounts" (appsettings, user-secrets ou variável de ambiente). Cada entrada é o Id da conta
// (Guid) ou o e-mail de login (PROVISÓRIO). Padrão: lista vazia = ninguém é administrador.
// Nunca gravar Id/e-mail real no repositório.
public sealed class AdminSettings
{
    public const string AccountsKey = "Admin:Accounts";

    public static AdminSettings Default { get; } = new([]);

    public IReadOnlySet<Guid> AccountIds { get; }

    public IReadOnlyList<string> AccountEmails { get; }

    public AdminSettings(IEnumerable<string?> accounts)
    {
        ArgumentNullException.ThrowIfNull(accounts);

        var ids = new HashSet<Guid>();
        var emails = new List<string>();

        foreach (var raw in accounts)
        {
            var entry = raw?.Trim();

            if (string.IsNullOrEmpty(entry))
            {
                continue;
            }

            if (Guid.TryParse(entry, out var id))
            {
                if (id != Guid.Empty)
                {
                    ids.Add(id);
                }

                continue;
            }

            if (!emails.Contains(entry, StringComparer.Ordinal))
            {
                emails.Add(entry);
            }
        }

        AccountIds = ids;
        AccountEmails = emails;
    }

    public bool IsEmpty => AccountIds.Count == 0 && AccountEmails.Count == 0;
}

// PV-ADMIN-010: papel da conta autenticada (identidade sempre do token — Server Authority).
public interface IAccountRoleResolver
{
    Task<AccountRole> GetRoleAsync(Guid accountId, CancellationToken cancellationToken = default);
}

// PV-ADMIN-010 (PROVISÓRIO): papel pela configuração. E-mails são resolvidos pelo repositório de
// contas já existente (GetByEmailAsync, comparação exata como no login).
public sealed class ConfiguredAccountRoleResolver : IAccountRoleResolver
{
    private readonly AdminSettings _settings;
    private readonly IAccountRepository _accounts;

    public ConfiguredAccountRoleResolver(AdminSettings settings, IAccountRepository accounts)
    {
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        _accounts = accounts ?? throw new ArgumentNullException(nameof(accounts));
    }

    public async Task<AccountRole> GetRoleAsync(Guid accountId, CancellationToken cancellationToken = default)
    {
        if (accountId == Guid.Empty || _settings.IsEmpty)
        {
            return AccountRole.Player;
        }

        if (_settings.AccountIds.Contains(accountId))
        {
            return AccountRole.Administrator;
        }

        foreach (var email in _settings.AccountEmails)
        {
            var account = await _accounts.GetByEmailAsync(new Email(email), cancellationToken);

            if (account is not null && account.Id == accountId)
            {
                return AccountRole.Administrator;
            }
        }

        return AccountRole.Player;
    }
}

// PV-ADMIN-010: conta sem papel de administrador tentou usar a ferramenta → 403.
public sealed class AdministratorRequiredException : Exception
{
    public AdministratorRequiredException()
        : base("Administrator role required.")
    {
    }
}
