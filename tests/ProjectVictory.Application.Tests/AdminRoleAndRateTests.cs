using ProjectVictory.Application.Admin;
using ProjectVictory.Application.Reward;
using ProjectVictory.Domain.Account;
using ProjectVictory.Domain.Reward;
using Xunit;

namespace ProjectVictory.Application.Tests;

// PV-ADMIN-010: papel pela configuração (só Id da conta — ressalva R1; padrão ninguém) e XP rate efetivo.
public sealed class AdminRoleAndRateTests
{
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

    // Ressalva R1 da revisão: só o Id (Guid) vale; e-mail e Guid.Empty são ignorados e contados.
    [Fact]
    public void Settings_AcceptOnlyGuid_IgnoringBlanksAndCountingInvalid()
    {
        var id = Guid.NewGuid();
        var settings = new AdminSettings([id.ToString(), " ", null, "admin@example.test", Guid.Empty.ToString(), "não-é-guid"]);

        Assert.Equal([id], settings.AccountIds);
        Assert.Equal(3, settings.IgnoredEntryCount);
        Assert.False(settings.IsEmpty);
        Assert.True(AdminSettings.Default.IsEmpty);
        Assert.Equal(0, AdminSettings.Default.IgnoredEntryCount);
        Assert.True(new AdminSettings(["admin@example.test"]).IsEmpty);
    }

    [Fact]
    public async Task Resolver_ById_EmailNeverGrants_AndDefaultIsPlayer()
    {
        var byId = Guid.NewGuid();
        var resolver = new ConfiguredAccountRoleResolver(new AdminSettings([byId.ToString(), "admin@example.test"]));

        Assert.Equal(AccountRole.Administrator, await resolver.GetRoleAsync(byId));
        Assert.Equal(AccountRole.Player, await resolver.GetRoleAsync(Guid.NewGuid()));
        Assert.Equal(AccountRole.Player, await resolver.GetRoleAsync(Guid.Empty));
        Assert.Equal(AccountRole.Player, await new ConfiguredAccountRoleResolver(AdminSettings.Default).GetRoleAsync(byId));
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
