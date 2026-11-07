using ProjectVictory.Application.Admin;
using ProjectVictory.Application.Reward;
using ProjectVictory.Domain.Account;
using ProjectVictory.Domain.Reward;
using Xunit;
using DomainAccount = ProjectVictory.Domain.Account.Account;

namespace ProjectVictory.Application.Tests;

// PV-ADMIN-010: papel pela configuração (Id ou e-mail; padrão ninguém) e XP rate efetivo.
public sealed class AdminRoleAndRateTests
{
    private sealed class Accounts : IAccountRepository
    {
        private readonly Dictionary<string, DomainAccount> _byEmail = new(StringComparer.Ordinal);

        public Task AddAsync(DomainAccount account, CancellationToken cancellationToken = default)
        {
            _byEmail[account.Email.Value] = account;
            return Task.CompletedTask;
        }

        public Task<DomainAccount?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default) =>
            Task.FromResult(_byEmail.GetValueOrDefault(email.Value));
    }

    private sealed class Store : IExperienceRateStore
    {
        private readonly ExperienceRateOverride? _value;

        public Store(ExperienceRateOverride? value)
        {
            _value = value;
        }

        public Task<ExperienceRateOverride?> GetAsync(CancellationToken cancellationToken = default) => Task.FromResult(_value);

        public Task SaveAsync(ExperienceRateOverride saved, CancellationToken cancellationToken = default) => Task.CompletedTask;
    }

    [Fact]
    public void Settings_ParseIdsAndEmails_IgnoringBlanks()
    {
        var id = Guid.NewGuid();
        var settings = new AdminSettings([id.ToString(), " ", null, "admin@example.test", Guid.Empty.ToString()]);

        Assert.Equal([id], settings.AccountIds);
        Assert.Equal(["admin@example.test"], settings.AccountEmails);
        Assert.True(AdminSettings.Default.IsEmpty);
    }

    [Fact]
    public async Task Resolver_ById_ByEmail_AndDefaultIsPlayer()
    {
        var byId = Guid.NewGuid();
        var accounts = new Accounts();
        var emailAccount = DomainAccount.Create(new AccountId(Guid.NewGuid()), new Email("admin@example.test"), new PasswordHash("x"));
        await accounts.AddAsync(emailAccount);

        var resolver = new ConfiguredAccountRoleResolver(new AdminSettings([byId.ToString(), "admin@example.test"]), accounts);

        Assert.Equal(AccountRole.Administrator, await resolver.GetRoleAsync(byId));
        Assert.Equal(AccountRole.Administrator, await resolver.GetRoleAsync(emailAccount.Id));
        Assert.Equal(AccountRole.Player, await resolver.GetRoleAsync(Guid.NewGuid()));
        Assert.Equal(AccountRole.Player, await new ConfiguredAccountRoleResolver(AdminSettings.Default, accounts).GetRoleAsync(byId));
    }

    [Fact]
    public async Task Provider_PersistedValueWins_OtherwiseConfiguration()
    {
        var settings = new RewardSettings(new ExperienceRate(1.5m));

        Assert.Equal(1.5m, (await new ExperienceRateProvider(settings, new Store(null)).GetCurrentAsync()).Value);
        Assert.Equal(3m, (await new ExperienceRateProvider(settings, new Store(new ExperienceRateOverride(3m, Guid.NewGuid(), DateTimeOffset.UnixEpoch))).GetCurrentAsync()).Value);
        // Valor gravado inválido não derruba a recompensa: vale a configuração.
        Assert.Equal(1.5m, (await new ExperienceRateProvider(settings, new Store(new ExperienceRateOverride(0m, Guid.NewGuid(), DateTimeOffset.UnixEpoch))).GetCurrentAsync()).Value);
    }
}
