using Microsoft.Extensions.DependencyInjection;
using ProjectVictory.Application.Character;
using ProjectVictory.Application.Reward;
using ProjectVictory.Domain.Character;
using Xunit;

namespace ProjectVictory.SmokeTests;

// Revisão do PV-SPAWN-010 (ressalva 7) sobre as guardas do PV-XP-010 no ApplyRewardHandler.
public sealed class ApplyRewardGuardTests
{
    private sealed class FixedDistributor(Func<Guid, IReadOnlyList<ExperienceShare>> shares) : IExperienceDistributor
    {
        public IReadOnlyList<ExperienceShare> Distribute(long finalExperience, IReadOnlyList<Guid> participants) =>
            shares(participants[0]);
    }

    private static async Task<(ApplyRewardHandler Handler, ICharacterRepository Repository, Guid CharacterId, IServiceScope Scope, ServiceProvider Provider)> Setup(
        Func<Guid, IReadOnlyList<ExperienceShare>> shares)
    {
        var services = new ServiceCollection();
        services.AddProjectVictorySmokeTestServices();
        services.AddSingleton<IExperienceDistributor>(new FixedDistributor(shares));
        var provider = services.BuildServiceProvider();
        var scope = provider.CreateScope();
        scope.ServiceProvider.GetRequiredService<TestCurrentPlayerContext>().SetAccount(Guid.NewGuid());

        var characterId = await scope.ServiceProvider.GetRequiredService<CreateCharacterHandler>()
            .Handle(new CreateCharacterCommand(TestNames.Unique("G"), CharacterClass.Knight));

        return (scope.ServiceProvider.GetRequiredService<ApplyRewardHandler>(),
            scope.ServiceProvider.GetRequiredService<ICharacterRepository>(), characterId, scope, provider);
    }

    [Fact]
    public async Task ShareForAnotherCharacter_IsRejected()
    {
        var (handler, _, characterId, scope, provider) = await Setup(_ => [new ExperienceShare(Guid.NewGuid(), 50)]);
        using (scope)
        await using (provider)
        {
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                handler.Handle(new ApplyRewardCommand(characterId, new RewardRequest(characterId, new object()))));
        }
    }

    [Fact]
    public async Task ZeroShare_DoesNotChangeExperience()
    {
        var (handler, repository, characterId, scope, provider) = await Setup(id => [new ExperienceShare(id, 0)]);
        using (scope)
        await using (provider)
        {
            var before = (await repository.GetByIdAsync(characterId))!.Experience.Value;

            await handler.Handle(new ApplyRewardCommand(characterId, new RewardRequest(characterId, new object())));

            Assert.Equal(before, (await repository.GetByIdAsync(characterId))!.Experience.Value);
        }
    }
}
