using Microsoft.Extensions.DependencyInjection;
using ProjectVictory.Application.Character;
using ProjectVictory.Application.Reward;
using ProjectVictory.Domain.Character;
using ProjectVictory.Domain.Entities;
using ProjectVictory.Domain.Character;
using Xunit;

namespace ProjectVictory.SmokeTests;

public sealed class RewardInventoryIntegrationTest
{
    [Fact]
    public async Task ApplyReward_Should_Add_Item_To_Character_Inventory()
    {
        var services = new ServiceCollection();

        services.AddProjectVictorySmokeTestServices();

        await using var provider =
            services.BuildServiceProvider();

        using var scope =
            provider.CreateScope();

        var currentPlayer =
            scope.ServiceProvider
                .GetRequiredService<TestCurrentPlayerContext>();

        currentPlayer.SetAccount(
            Guid.NewGuid());

        var createCharacter =
            scope.ServiceProvider
                .GetRequiredService<CreateCharacterHandler>();

        var characterId =
            await createCharacter.Handle(
                new CreateCharacterCommand(
                    "LootTestCharacter",
                    CharacterClass.Knight));

        var applyReward =
            scope.ServiceProvider
                .GetRequiredService<ApplyRewardHandler>();

        var result =
            await applyReward.Handle(
                new ApplyRewardCommand(
                    characterId,
                    new RewardRequest(
                        characterId,
                        new object())));

        Assert.True(
            result.Applied);

        var repository =
            scope.ServiceProvider
                .GetRequiredService<ICharacterRepository>();

        var character =
            await repository.GetByIdAsync(
                characterId);

        Assert.NotNull(character);

        Assert.Contains(
            "Smoke Test Item",
            character.Inventory.Items);
    }
}