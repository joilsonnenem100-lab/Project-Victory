using ProjectVictory.Domain.Account;

namespace ProjectVictory.Application.Admin;

// PV-ADMIN-010 (DEC-20261106-12): quem é administrador vem da configuração do servidor
// "Admin:Accounts" (appsettings, user-secrets ou variável de ambiente). Cada entrada é SOMENTE o Id
// da conta (Guid). Ressalva R1 da revisão: e-mail deixou de ser aceito, porque o cadastro não
// verifica e-mail (quem se cadastrasse primeiro com o e-mail configurado viraria administrador).
// Entradas que não são Guid são ignoradas e contadas em IgnoredEntryCount (o Program.cs avisa no log
// da inicialização, sem escrever o valor). Padrão: lista vazia = ninguém é administrador.
// Nunca gravar Id real no repositório.
public sealed class AdminSettings
{
    public const string AccountsKey = "Admin:Accounts";

    public static AdminSettings Default { get; } = new([]);

    public IReadOnlySet<Guid> AccountIds { get; }

    // Entradas não vazias que não são Guid válido (ex.: e-mail) ou são Guid.Empty: ignoradas.
    public int IgnoredEntryCount { get; }

    public AdminSettings(IEnumerable<string?> accounts)
    {
        ArgumentNullException.ThrowIfNull(accounts);

        var ids = new HashSet<Guid>();
        var ignored = 0;

        foreach (var raw in accounts)
        {
            var entry = raw?.Trim();

            if (string.IsNullOrEmpty(entry))
            {
                continue;
            }

            if (Guid.TryParse(entry, out var id) && id != Guid.Empty)
            {
                ids.Add(id);
                continue;
            }

            ignored++;
        }

        AccountIds = ids;
        IgnoredEntryCount = ignored;
    }

    public bool IsEmpty => AccountIds.Count == 0;
}

// PV-ADMIN-010: papel da conta autenticada (identidade sempre do token — Server Authority).
public interface IAccountRoleResolver
{
    Task<AccountRole> GetRoleAsync(Guid accountId, CancellationToken cancellationToken = default);
}

// PV-ADMIN-010 (PROVISÓRIO): papel pela configuração — só pelo Id da conta (ressalva R1).
public sealed class ConfiguredAccountRoleResolver : IAccountRoleResolver
{
    private readonly AdminSettings _settings;

    public ConfiguredAccountRoleResolver(AdminSettings settings)
    {
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
    }

    public Task<AccountRole> GetRoleAsync(Guid accountId, CancellationToken cancellationToken = default)
    {
        var role = accountId != Guid.Empty && _settings.AccountIds.Contains(accountId)
            ? AccountRole.Administrator
            : AccountRole.Player;

        return Task.FromResult(role);
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
