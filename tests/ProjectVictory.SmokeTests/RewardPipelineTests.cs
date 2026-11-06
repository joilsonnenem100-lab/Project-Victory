using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectVictory.Application.Character;
using ProjectVictory.Application.Reward;
using ProjectVictory.Domain.Character;
using ProjectVictory.Domain.Combat;
using ProjectVictory.Domain.Primitives;
using ProjectVictory.Domain.Reward;
using ProjectVictory.Infrastructure.Loot;
using ProjectVictory.Infrastructure.Services;
using ProjectVictory.Persistence.Database;
using Xunit;

namespace ProjectVictory.SmokeTests;

// PV-REWARD-010: fluxo real do ataque (RewardResolutionHandler) entrega XP × XP rate,
// ouro e drops da definição do monstro; RewardProcessor deixa de ser stub.
public sealed class RewardPipelineTests
{
    private static readonly Guid RatId = Guid.Parse("10000000-0000-0000-0000-000000000001");
    private static readonly Guid RatTailId = Guid.Parse("00000000-0000-0000-0000-000000000001");
    private static readonly Guid UnknownMonsterId = Guid.Parse("10000000-0000-0000-0000-0000000000ff");

    private static ServiceProvider BuildProvider(decimal experienceRate, int goldPick)
    {
        var services = new ServiceCollection();

        services.AddProjectVictorySmokeTestServices();

        // Banco isolado por teste e configuração do servidor sob teste (último registro vence).
        var databaseName = $"reward-{Guid.NewGuid():N}";
        services.AddDbContext<ProjectVictoryDbContext>(
            options => options.UseInMemoryDatabase(databaseName));
        services.AddSingleton(new RewardSettings(new ExperienceRate(experienceRate)));
        services.AddSingleton<IRandomSource>(new FixedRandomSource(goldPick));

        return services.BuildServiceProvider();
    }

    private static async Task<Guid> CreateCharacter(IServiceScope scope)
    {
        scope.ServiceProvider.GetRequiredService<TestCurrentPlayerContext>().SetAccount(Guid.NewGuid());

        var name = "Reward" + Guid.NewGuid().ToString("N")[..10];

        return await scope.ServiceProvider.GetRequiredService<CreateCharacterHandler>()
            .Handle(new CreateCharacterCommand(name, CharacterClass.Knight));
    }

    private static CombatResult Victory(Guid characterId, Guid opponentId) =>
        new(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), characterId, opponentId, CombatOutcome.Victory);

    [Theory]
    [InlineData(1, 10L)]
    [InlineData(2, 20L)]
    public async Task AttackVictory_GivesExperienceTimesRate_GoldAndDrop(int rate, long expectedXp)
    {
        await using var provider = BuildProvider(rate, goldPick: 3);
        using var scope = provider.CreateScope();
        var characterId = await CreateCharacter(scope);

        var reward = await scope.ServiceProvider.GetRequiredService<RewardResolutionHandler>()
            .Handle(Victory(characterId, RatId));

        Assert.Equal(expectedXp, reward.ExperienceAmount);

        var character = await scope.ServiceProvider.GetRequiredService<ICharacterRepository>().GetByIdAsync(characterId);
        Assert.NotNull(character);
        Assert.Equal(expectedXp, character!.Experience.Value);
        Assert.Equal(3, character.Inventory.Gold);
        Assert.Equal(RatTailId, Assert.Single(character.Inventory.Items).ItemDefinitionId);
    }

    [Fact]
    public async Task Defeat_OrUnknownMonster_GivesNothing()
    {
        await using var provider = BuildProvider(1m, goldPick: 3);
        using var scope = provider.CreateScope();
        var characterId = await CreateCharacter(scope);
        var handler = scope.ServiceProvider.GetRequiredService<RewardResolutionHandler>();

        var defeat = await handler.Handle(new CombatResult(
            Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), characterId, RatId, CombatOutcome.Defeat));
        var unknown = await handler.Handle(Victory(characterId, UnknownMonsterId));

        Assert.Null(defeat.ExperienceAmount);
        Assert.Null(unknown.ExperienceAmount);

        var character = await scope.ServiceProvider.GetRequiredService<ICharacterRepository>().GetByIdAsync(characterId);
        Assert.Equal(0, character!.Experience.Value);
        Assert.Equal(0, character.Inventory.Gold);
        Assert.Empty(character.Inventory.Items);
    }

    [Fact]
    public void RewardProcessor_UsesMonsterDefinition_AndIgnoresOtherSources()
    {
        var processor = new RewardProcessor(new MonsterLootDefinitionProvider(), new FixedRandomSource(goldPick: 4));

        var fromCombat = processor.Process(new RewardRequest(Guid.NewGuid(), Victory(Guid.NewGuid(), RatId)));
        var fromOther = processor.Process(new RewardRequest(Guid.NewGuid(), new object()));

        Assert.Equal(10, fromCombat.Experience);
        Assert.Equal(4, fromCombat.Gold);
        Assert.Equal(RatTailId, Assert.Single(fromCombat.Drops).ItemDefinitionId);
        Assert.Equal(0, fromOther.Experience);
        Assert.Equal(0, fromOther.Gold);
        Assert.Empty(fromOther.Drops);
    }

    [Fact]
    public void RatTail_HasProvisionalSellPrice()
    {
        var definition = new MonsterLootDefinitionProvider().GetByOpponentId(RatId);

        Assert.Equal(2, Assert.Single(definition!.Drops).Item.NpcSellPrice);
    }
}
