using ProjectVictory.Domain.Character;
using ProjectVictory.Domain.Items;

namespace ProjectVictory.Application.Reward;

public sealed class ApplyRewardHandler
{
    private readonly IRewardProcessor _rewardProcessor;
    private readonly ICharacterRepository _characterRepository;
    private readonly RewardSettings _settings;

    public ApplyRewardHandler(
        IRewardProcessor rewardProcessor,
        ICharacterRepository characterRepository,
        RewardSettings settings)
    {
        _rewardProcessor = rewardProcessor;
        _characterRepository = characterRepository;
        _settings = settings;
    }

    public async Task<ApplyRewardResult> Handle(
        ApplyRewardCommand command,
        CancellationToken cancellationToken = default)
    {
        var character =
            await _characterRepository.GetByIdAsync(
                command.CharacterId,
                cancellationToken);

        if (character is null)
        {
            throw new InvalidOperationException(
                "Character not found.");
        }

        var reward =
            _rewardProcessor.Process(
                command.Request);

        // PV-REWARD-010: XP × XP rate do servidor (DEC-20261106-12); o Reward Pipeline é o
        // único que muda Experience (ADR-024).
        var experience =
            _settings.ExperienceRate.Apply(
                Math.Max(0, reward.Experience));

        if (experience > 0)
        {
            character.GainExperience(
                new ProjectVictory.Domain.ValueObjects.Experience(
                    experience));
        }

        // PV-REWARD-010: drops e ouro chegam ao Inventory do personagem (lacuna do PV-QA-001).
        foreach (var drop in reward.Drops)
        {
            character.Inventory.AddItem(
                new ItemInstance(
                    Guid.NewGuid(),
                    drop.ItemDefinitionId));
        }

        if (reward.Gold > 0)
        {
            character.Inventory.AddGold(
                reward.Gold);
        }

        await _characterRepository.UpdateAsync(
            character,
            cancellationToken);

        return new ApplyRewardResult(
            character.Id,
            true);
    }
}
