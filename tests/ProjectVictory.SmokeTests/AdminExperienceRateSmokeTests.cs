using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectVictory.Application.Admin;
using ProjectVictory.Application.Character;
using ProjectVictory.Application.Reward;
using ProjectVictory.Domain.Account;
using ProjectVictory.Domain.Character;
using ProjectVictory.Domain.Combat;
using ProjectVictory.Domain.Primitives;
using ProjectVictory.Domain.Reward;
using ProjectVictory.Persistence.Database;
using Xunit;

namespace ProjectVictory.SmokeTests;

// PV-ADMIN-010 (DEC-20261106-12): papel de administrador pela configuração, leitura/alteração do XP rate
// pela composição real (InMemory) e nova recompensa com o rate gravado. Sem valor gravado = configuração.
public sealed class AdminExperienceRateSmokeTests
{
    private static readonly Guid RatId = Guid.Parse("10000000-0000-0000-0000-000000000001");
    private static readonly Guid AdminAccountId = Guid.NewGuid();

    private static ServiceProvider BuildProvider(decimal configuredRate, params string[] adminAccounts)
    {
        var services = new ServiceCollection();
        services.AddProjectVictorySmokeTestServices();

        var databaseName = $"admin-{Guid.NewGuid():N}";
        services.AddDbContext<ProjectVictoryDbContext>(options => options.UseInMemoryDatabase(databaseName));
        services.AddSingleton(new RewardSettings(new ExperienceRate(configuredRate)));
        services.AddSingleton<IRandomSource>(new FixedRandomSource(0));
        services.AddSingleton(new AdminSettings(adminAccounts));

        return services.BuildServiceProvider();
    }

    private static CombatResult Victory(Guid characterId) =>
        new(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), characterId, RatId, CombatOutcome.Victory);

    [Fact]
    public async Task DefaultSettings_NobodyIsAdministrator()
    {
        await using var provider = BuildProvider(1m);
        using var scope = provider.CreateScope();
        scope.ServiceProvider.GetRequiredService<TestCurrentPlayerContext>().SetAccount(AdminAccountId);

        Assert.Equal(AccountRole.Player, await scope.ServiceProvider.GetRequiredService<IAccountRoleResolver>().GetRoleAsync(AdminAccountId));
        await Assert.ThrowsAsync<AdministratorRequiredException>(
            () => scope.ServiceProvider.GetRequiredService<GetExperienceRateHandler>().Handle());
        await Assert.ThrowsAsync<AdministratorRequiredException>(
            () => scope.ServiceProvider.GetRequiredService<SetExperienceRateHandler>().Handle(new SetExperienceRateCommand(5m)));
        Assert.Null(await scope.ServiceProvider.GetRequiredService<IExperienceRateStore>().GetAsync());
    }

    [Fact]
    public async Task WithoutPersistedValue_EffectiveRateIsTheConfiguredOne()
    {
        await using var provider = BuildProvider(2m, AdminAccountId.ToString());
        using var scope = provider.CreateScope();
        scope.ServiceProvider.GetRequiredService<TestCurrentPlayerContext>().SetAccount(AdminAccountId);

        var read = await scope.ServiceProvider.GetRequiredService<GetExperienceRateHandler>().Handle();

        Assert.Equal(2m, read.Value);
        Assert.Equal(2m, read.ConfiguredValue);
        Assert.False(read.IsOverridden);
        Assert.Null(read.ChangedAt);
        Assert.Equal(2m, (await scope.ServiceProvider.GetRequiredService<IExperienceRateProvider>().GetCurrentAsync()).Value);
    }

    [Theory]
    [InlineData("0")]
    [InlineData("-1")]
    [InlineData("100.0001")]
    public async Task InvalidValue_IsRejected_AndNothingIsSaved(string value)
    {
        await using var provider = BuildProvider(1m, AdminAccountId.ToString());
        using var scope = provider.CreateScope();
        scope.ServiceProvider.GetRequiredService<TestCurrentPlayerContext>().SetAccount(AdminAccountId);

        await Assert.ThrowsAsync<InvalidExperienceRateException>(
            () => scope.ServiceProvider.GetRequiredService<SetExperienceRateHandler>()
                .Handle(new SetExperienceRateCommand(decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture))));
        Assert.Null(await scope.ServiceProvider.GetRequiredService<IExperienceRateStore>().GetAsync());
    }

    [Fact]
    public async Task AdministratorChangesRate_NewRewardsUseIt()
    {
        await using var provider = BuildProvider(1m, AdminAccountId.ToString());
        Guid characterId;

        using (var scope = provider.CreateScope())
        {
            scope.ServiceProvider.GetRequiredService<TestCurrentPlayerContext>().SetAccount(Guid.NewGuid());
            characterId = await scope.ServiceProvider.GetRequiredService<CreateCharacterHandler>()
                .Handle(new CreateCharacterCommand("Admin" + Guid.NewGuid().ToString("N")[..10], CharacterClass.Knight));

            // Antes de alterar: 1x (Rat = 10 XP), como antes do PV-ADMIN-010.
            var before = await scope.ServiceProvider.GetRequiredService<RewardResolutionHandler>().Handle(Victory(characterId));
            Assert.Equal(10, before.ExperienceAmount);
        }

        using (var scope = provider.CreateScope())
        {
            scope.ServiceProvider.GetRequiredService<TestCurrentPlayerContext>().SetAccount(AdminAccountId);
            var saved = await scope.ServiceProvider.GetRequiredService<SetExperienceRateHandler>().Handle(new SetExperienceRateCommand(2.5m));

            Assert.Equal(2.5m, saved.Value);
            Assert.Equal(1m, saved.ConfiguredValue);
            Assert.True(saved.IsOverridden);
            Assert.NotNull(saved.ChangedAt);
            Assert.Equal(AdminAccountId, (await scope.ServiceProvider.GetRequiredService<IExperienceRateStore>().GetAsync())!.ChangedByAccountId);
        }

        using (var scope = provider.CreateScope())
        {
            var after = await scope.ServiceProvider.GetRequiredService<RewardResolutionHandler>().Handle(Victory(characterId));
            Assert.Equal(25, after.ExperienceAmount);

            var character = await scope.ServiceProvider.GetRequiredService<ICharacterRepository>().GetByIdAsync(characterId);
            Assert.Equal(35, character!.Experience.Value);
        }
    }
}
