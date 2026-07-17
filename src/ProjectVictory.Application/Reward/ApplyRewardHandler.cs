using ProjectVictory.Domain.Character;
using ProjectVictory.Domain.ValueObjects;
using Experience = ProjectVictory.Domain.ValueObjects.Experience;

namespace ProjectVictory.Application.Reward;

public sealed class ApplyRewardHandler
{
    private readonly IRewardProcessor _rewardProcessor;
    private readonly ICharacterRepository _characterRepository;

    public ApplyRewardHandler(
        IRewardProcessor rewardProcessor,
        ICharacterRepository characterRepository)
    {
        _rewardProcessor = rewardProcessor;
        _characterRepository = characterRepository;
    }

    public async Task<ApplyRewardResult> Handle(
        ApplyRewardCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.Request.CharacterId == Guid.Empty)
        {
            throw new InvalidOperationException(
                "CharacterId is required.");
        }

        if (command.Request.SourceResult is null)
        {
            throw new InvalidOperationException(
                "Reward source result is required.");
        }

        var character = await _characterRepository.GetByIdAsync(
            command.CharacterId,
            cancellationToken);

        if (character is null)
        {
            throw new InvalidOperationException(
                "Character not found.");
        }

        var reward = _rewardProcessor.Process(
            command.Request);

        if (reward is null)
        {
            throw new InvalidOperationException(
                "Reward processing failed.");
        }

        if (reward.Experience > 0)
        {
            character.GainExperience(
                new global::ProjectVictory.Domain.ValueObjects.Experience(reward.Experience));
        }

        await _characterRepository.UpdateAsync(
            character,
            cancellationToken);

        return new ApplyRewardResult(
            character.Id,
            true);
    }
}