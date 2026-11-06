using Microsoft.Extensions.DependencyInjection;
using ProjectVictory.Application.Character;
using ProjectVictory.Application.Reward;
using ProjectVictory.Domain.Character;
using ProjectVictory.Domain.Combat;
using ProjectVictory.Infrastructure.Content;
using Xunit;

namespace ProjectVictory.SmokeTests;

// PV-XP-010 (ADR-007/024): o Reward Pipeline passa a Experience final pela Experience Distribution e
// aplica cada parte. Regressão: com 1 participante (hoje, 1 jogador por Hunt) o XP é o mesmo de antes.
public sealed class ExperienceDistributionPipelineTests
{
    private sealed class SpyDistributor : IExperienceDistributor
    {
        public List<(long Experience, IReadOnlyList<Guid> Participants)> Calls { get; } = [];

        public IReadOnlyList<ExperienceShare> Distribute(long finalExperience, IReadOnlyList<Guid> participants)
        {
            Calls.Add((finalExperience, participants));
            return EvenSplitExperienceDistributor.Instance.Distribute(finalExperience, participants);
        }
    }

    private static async Task<(long Before, long After, SpyDistributor Spy)> DefeatRat(CombatOutcome outcome)
    {
        var spy = new SpyDistributor();
        var services = new ServiceCollection();
        services.AddProjectVictorySmokeTestServices();
        services.AddSingleton<IExperienceDistributor>(spy);
        await using var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();
        scope.ServiceProvider.GetRequiredService<TestCurrentPlayerContext>().SetAccount(Guid.NewGuid());

        var characterId = await scope.ServiceProvider.GetRequiredService<CreateCharacterHandler>()
            .Handle(new CreateCharacterCommand("X" + Guid.NewGuid().ToString("N")[..10], CharacterClass.Knight));
        var repository = scope.ServiceProvider.GetRequiredService<ICharacterRepository>();
        var before = (await repository.GetByIdAsync(characterId))!.Experience.Value;

        await scope.ServiceProvider.GetRequiredService<RewardResolutionHandler>().Handle(
            new CombatResult(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), characterId, SpiderCaveCatalog.RatId, outcome));

        var after = (await repository.GetByIdAsync(characterId))!.Experience.Value;

        return (before, after, spy);
    }

    [Fact]
    public async Task Victory_OneParticipant_SameExperienceAsBefore_ThroughTheDistributor()
    {
        var (before, after, spy) = await DefeatRat(CombatOutcome.Victory);

        // Rat = 10 XP × XP rate 1 (valores já validados no M2).
        Assert.Equal(10, after - before);

        var call = Assert.Single(spy.Calls);
        Assert.Equal(10, call.Experience);
        Assert.Single(call.Participants);
    }

    [Fact]
    public async Task Defeat_NoExperience_DistributorNotCalled()
    {
        var (before, after, spy) = await DefeatRat(CombatOutcome.Defeat);

        Assert.Equal(before, after);
        Assert.Empty(spy.Calls);
    }
}
