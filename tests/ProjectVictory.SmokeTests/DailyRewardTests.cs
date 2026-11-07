using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectVictory.Application.Character;
using ProjectVictory.Application.DailyReward;
using ProjectVictory.Application.Game;
using ProjectVictory.Application.Supply;
using ProjectVictory.Application.VerticalSlice;
using ProjectVictory.Domain.Character;
using ProjectVictory.Persistence.Database;
using Xunit;

namespace ProjectVictory.SmokeTests;

// PV-DAILY-010 (PO-60, R-068): recompensa diária por conta, na primeira entrada depois do reset, no máximo
// uma vez por ciclo, decidida no servidor com relógio controlado (antes, depois e mesmo dia).
// O relógio começa em 2026-11-06 12:00 UTC (ManualTimeProvider); reset às 09:00 UTC.
public sealed class DailyRewardTests
{
    private static readonly Guid HealthPotionId = PotionCatalog.FindByKey("HealthPotion")!.ItemId;

    private static DailyRewardSettings Settings() =>
        new(new Dictionary<Guid, int> { [HealthPotionId] = 3 }, 50, TimeSpan.FromHours(9));

    private sealed class Server : IAsyncDisposable
    {
        public ServiceProvider Provider { get; }

        public Server(string databaseName, ManualTimeProvider clock, DailyRewardSettings? settings)
        {
            var services = new ServiceCollection();
            services.AddProjectVictorySmokeTestServices();
            services.AddDbContext<ProjectVictoryDbContext>(o => o.UseInMemoryDatabase(databaseName));
            services.AddSingleton<TimeProvider>(clock);

            if (settings is not null)
            {
                services.AddSingleton(settings);
            }

            Provider = services.BuildServiceProvider();
        }

        // Um escopo por requisição, como na API.
        public async Task<EnterGameResult> EnterAs(Guid accountId)
        {
            using var scope = Provider.CreateScope();
            scope.ServiceProvider.GetRequiredService<TestCurrentPlayerContext>().SetAccount(accountId);
            return await scope.ServiceProvider.GetRequiredService<EnterGameHandler>().Handle(new EnterGameCommand());
        }

        public async Task<Guid> CreateCharacter(Guid accountId)
        {
            using var scope = Provider.CreateScope();
            scope.ServiceProvider.GetRequiredService<TestCurrentPlayerContext>().SetAccount(accountId);
            return await scope.ServiceProvider.GetRequiredService<CreateCharacterHandler>()
                .Handle(new CreateCharacterCommand(TestNames.Unique("D"), CharacterClass.Knight));
        }

        public async Task<(int Potions, long Gold)> InventoryOf(Guid characterId)
        {
            using var scope = Provider.CreateScope();
            var character = await scope.ServiceProvider.GetRequiredService<ICharacterRepository>().GetByIdAsync(characterId);
            return (character!.Inventory.Items.Count(i => i.ItemDefinitionId == HealthPotionId), character.Inventory.Gold);
        }

        public async ValueTask DisposeAsync() => await Provider.DisposeAsync();
    }

    [Fact]
    public async Task FirstEnter_AfterReset_GrantsTheListOnce_SameDayGrantsNothing()
    {
        var clock = new ManualTimeProvider();
        await using var server = new Server($"daily-{Guid.NewGuid():N}", clock, Settings());
        var accountId = Guid.NewGuid();
        var characterId = await server.CreateCharacter(accountId);
        var (potionsBefore, goldBefore) = await server.InventoryOf(characterId);

        var first = await server.EnterAs(accountId);
        Assert.NotNull(first.DailyReward);
        Assert.Equal(50, first.DailyReward!.Gold);
        Assert.Equal(3, Assert.Single(first.DailyReward.Items).Quantity);
        Assert.Equal((potionsBefore + 3, goldBefore + 50), await server.InventoryOf(characterId));

        // Mesmo dia (antes do próximo reset): nada.
        clock.Advance(TimeSpan.FromHours(11)); // 23:00
        Assert.Null((await server.EnterAs(accountId)).DailyReward);
        Assert.Equal((potionsBefore + 3, goldBefore + 50), await server.InventoryOf(characterId));
    }

    [Fact]
    public async Task NextDay_BeforeReset_Nothing_AfterReset_GrantsAgain()
    {
        var clock = new ManualTimeProvider();
        await using var server = new Server($"daily-reset-{Guid.NewGuid():N}", clock, Settings());
        var accountId = Guid.NewGuid();
        var characterId = await server.CreateCharacter(accountId);
        var (potionsBefore, goldBefore) = await server.InventoryOf(characterId);

        Assert.NotNull((await server.EnterAs(accountId)).DailyReward); // dia 6, 12:00

        clock.Advance(TimeSpan.FromHours(20).Add(TimeSpan.FromMinutes(59))); // dia 7, 08:59 — antes do reset
        Assert.Null((await server.EnterAs(accountId)).DailyReward);

        clock.Advance(TimeSpan.FromMinutes(1)); // dia 7, 09:00 — reset
        Assert.NotNull((await server.EnterAs(accountId)).DailyReward);
        Assert.Null((await server.EnterAs(accountId)).DailyReward);

        Assert.Equal((potionsBefore + 6, goldBefore + 100), await server.InventoryOf(characterId));
    }

    [Fact]
    public async Task EachAccount_HasItsOwnClaim()
    {
        var clock = new ManualTimeProvider();
        await using var server = new Server($"daily-accounts-{Guid.NewGuid():N}", clock, Settings());
        var first = Guid.NewGuid();
        var second = Guid.NewGuid();
        await server.CreateCharacter(first);
        await server.CreateCharacter(second);

        Assert.NotNull((await server.EnterAs(first)).DailyReward);
        Assert.NotNull((await server.EnterAs(second)).DailyReward);
        Assert.Null((await server.EnterAs(first)).DailyReward);
    }

    [Fact]
    public async Task SimultaneousEnters_GrantOnlyOnce()
    {
        var clock = new ManualTimeProvider();
        await using var server = new Server($"daily-race-{Guid.NewGuid():N}", clock, Settings());
        var accountId = Guid.NewGuid();
        var characterId = await server.CreateCharacter(accountId);
        var (potionsBefore, _) = await server.InventoryOf(characterId);

        var results = await Task.WhenAll(Enumerable.Range(0, 5).Select(_ => server.EnterAs(accountId)));

        Assert.Single(results, r => r.DailyReward is not null);
        Assert.Equal(potionsBefore + 3, (await server.InventoryOf(characterId)).Potions);
    }

    [Fact]
    public async Task WithoutConfiguration_IsDisabled()
    {
        var clock = new ManualTimeProvider();
        await using var server = new Server($"daily-off-{Guid.NewGuid():N}", clock, settings: null);
        var accountId = Guid.NewGuid();
        await server.CreateCharacter(accountId);

        Assert.Null((await server.EnterAs(accountId)).DailyReward);
    }

    [Theory]
    [InlineData("2026-11-06T08:59:00Z", "2026-11-05T09:00:00Z")]
    [InlineData("2026-11-06T09:00:00Z", "2026-11-06T09:00:00Z")]
    [InlineData("2026-11-06T23:59:00Z", "2026-11-06T09:00:00Z")]
    public void CycleStart_IsTheLastResetAtOrBeforeNow(string now, string expected)
    {
        Assert.Equal(DateTimeOffset.Parse(expected), Settings().CycleStartFor(DateTimeOffset.Parse(now)));
    }

    [Fact]
    public void InvalidConfiguration_IsRefused()
    {
        Assert.Throws<InvalidOperationException>(() => DailyRewardSettings.ParseItems([new("NotAPotion", 1)]));
        Assert.Throws<InvalidOperationException>(() => DailyRewardSettings.ParseItems([new("HealthPotion", -1)]));
        Assert.Throws<ArgumentOutOfRangeException>(() => new DailyRewardSettings(new Dictionary<Guid, int>(), -1, TimeSpan.Zero));
        Assert.Throws<ArgumentOutOfRangeException>(() => new DailyRewardSettings(new Dictionary<Guid, int>(), 1, TimeSpan.FromHours(24)));
    }
}
