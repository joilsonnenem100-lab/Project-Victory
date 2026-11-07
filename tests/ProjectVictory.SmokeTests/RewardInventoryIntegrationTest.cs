using Microsoft.Extensions.DependencyInjection;
using ProjectVictory.Application.Character;
using ProjectVictory.Application.Reward;
using ProjectVictory.Domain.Character;
using ProjectVictory.Domain.Entities;
using Xunit;

namespace ProjectVictory.SmokeTests;

public sealed class RewardInventoryIntegrationTest
{
    // PV-REWARD-010: Skip removido — o ApplyRewardHandler passou a aplicar drops e ouro.
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
                    TestNames.Unique("Loot"),
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

        // PV-REWARD-010: o drop da recompensa vira uma instância no Inventory com o
        // ItemDefinitionId do drop; o ouro e a XP (× XP rate 1x) também chegam ao personagem.
        var item =
            Assert.Single(
                character.Inventory.Items);

        Assert.NotEqual(
            Guid.Empty,
            item.Id);

        Assert.Equal(
            TestRewardProcessor.DropItemDefinitionId,
            item.ItemDefinitionId);

        Assert.Equal(
            TestRewardProcessor.Gold,
            character.Inventory.Gold);

        Assert.Equal(
            TestRewardProcessor.Experience,
            character.Experience.Value);
    }
}